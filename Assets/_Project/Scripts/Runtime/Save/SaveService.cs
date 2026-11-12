using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IAmAsIAm.Core.Save;
using IAmAsIAm.Core.Story;
using IAmAsIAm.Core.Triggers;
using IAmAsIAm.Runtime.Story;
using UnityEngine;

namespace IAmAsIAm.Runtime.Save
{
    /// <summary>
    /// PC file storage under Application.persistentDataPath. It writes to a temp file first and
    /// then swaps it in, so a crash mid-write never corrupts a save. Console backends implement
    /// <see cref="ISaveStorage"/> with each platform's SDK.
    /// </summary>
    public sealed class FileSaveStorage : ISaveStorage
    {
        readonly string _root;

        public FileSaveStorage(string root = null)
        {
            _root = root ?? Path.Combine(Application.persistentDataPath, "saves");
            Directory.CreateDirectory(_root);
        }

        string PathFor(string key) => Path.Combine(_root, key + ".json");

        public bool Exists(string key) => File.Exists(PathFor(key));
        public string Read(string key) => Exists(key) ? File.ReadAllText(PathFor(key)) : null;

        public void Write(string key, string contents)
        {
            string final = PathFor(key);
            string temp = final + ".tmp";
            File.WriteAllText(temp, contents);
            if (File.Exists(final)) File.Replace(temp, final, null);
            else File.Move(temp, final);
        }

        public void Delete(string key)
        {
            if (Exists(key)) File.Delete(PathFor(key));
        }
    }

    /// <summary>JsonUtility as the save format.</summary>
    public sealed class UnityJsonSerializer : ISaveSerializer
    {
        public string ToJson(object value) => JsonUtility.ToJson(value);
        public T FromJson<T>(string json) => JsonUtility.FromJson<T>(json);
    }

    /// <summary>
    /// Builds <see cref="SaveData"/> from <see cref="GameServices"/> and restores it, slot by slot.
    /// One slot is "active" (the game being played); autosaves go there. Saves happen only at safe points,
    /// when nobody is talking: Ink's state then sits between scenes, so a load always resumes cleanly.
    /// </summary>
    public static class SaveService
    {
        const string PreferencesKey = "preferences";

        static ISaveStorage _storage;
        static SaveSlots _slots;

        public static ISaveStorage Storage
        {
            get => _storage ??= new FileSaveStorage();
            set { _storage = value; _slots = null; }
        }

        public static SaveSlots Slots => _slots ??= new SaveSlots(Storage, new UnityJsonSerializer());

        /// <summary>The slot being played (1-based), or 0 on the title screen.</summary>
        public static int ActiveSlot { get; private set; }

        /// <summary>Raised after a save is loaded into the game, so views can refresh (who's fronting, etc.).</summary>
        public static event Action Loaded;
        public static event Action<int> Saved;

        /// <summary>Starts a new game in a slot. The state must be fresh (title screen, or just after a restart).</summary>
        public static void BeginNewGame(int slot)
        {
            ActiveSlot = slot;
            GameServices.PlaySeconds = 0f;
            RememberLastSlot(slot);
            Save(slot);
        }

        /// <summary>Saves to the active slot. Does nothing on the title screen or while a story is playing.</summary>
        public static bool Autosave(StoryRunner runner = null)
        {
            if (ActiveSlot == 0 || (runner != null && runner.IsPlaying)) return false;
            Save(ActiveSlot, runner);
            return true;
        }

        public static void Save(int slot, StoryRunner runner = null)
        {
            if (!GameServices.IsReady) return;
            runner ??= UnityEngine.Object.FindAnyObjectByType<StoryRunner>();
            var mind = GameServices.Mind;
            var roster = GameServices.Roster;
            var runs = GameServices.ChapterRuns;
            var run = runs?.ExportRun() ?? new ChapterRunState();
            var chapter = GameServices.Chapter(run.chapterId);

            var data = new SaveData
            {
                chapterId = run.chapterId,
                inGameDay = GameServices.InGameDay,
                arousal = mind.Arousal,
                windowHalfWidth = mind.WindowHalfWidth,
                frontingId = roster.FrontingId,
                coConsciousIds = roster.CoConscious.ToList(),
                members = roster.ExportStates().ToList(),
                journal = GameServices.Journal.Entries.ToList(),
                gaps = GameServices.Journal.Gaps.ToList(),
                learnedSkillIds = GameServices.Skills.LearnedIds.ToList(),
                inkStateJson = runner != null ? runner.SaveState() ?? "" : "",
                choices = GameServices.Choices?.Records.ToList() ?? new List<ChoiceRecord>(),
                exploredTraditionIds = GameServices.Traditions?.ExploredIds.ToList() ?? new List<string>(),
                triedPracticeIds = GameServices.Traditions?.TriedPracticeIds.ToList() ?? new List<string>(),
                triggerProgress = GameServices.Triggers?.ExportProgress().ToList() ?? new List<TriggerProgress>(),
                chapterUnlocks = runs?.Export() ?? new List<ChapterUnlock>(),
                chapterRun = run,
                devices = GameServices.Devices?.Export() ?? new IAmAsIAm.Core.Devices.DevicesState(),
                header = new SaveSlotHeader
                {
                    chapterId = run.chapterId,
                    chapterTitle = chapter?.title ?? "",
                    frontingId = roster.FrontingId,
                    frontingName = roster.GetDefinition(roster.FrontingId)?.displayName ?? roster.FrontingId,
                    inGameDay = GameServices.InGameDay,
                    savedAtUtcTicks = DateTime.UtcNow.Ticks,
                    playSeconds = GameServices.PlaySeconds,
                },
            };

            try
            {
                Slots.Write(slot, data);
                if (slot != ActiveSlot) { ActiveSlot = slot; RememberLastSlot(slot); }
                Saved?.Invoke(slot);
            }
            catch (Exception e)
            {
                Debug.LogError($"[IAmAsIAm] Couldn't save to slot {slot}: {e.Message}");
            }
        }

        /// <summary>Loads a slot into the game. The state must be fresh (title screen, or just after a restart).</summary>
        public static bool TryLoad(int slot, StoryRunner runner = null)
        {
            if (!Slots.IsValid(slot) || !Slots.TryRead(slot, out var data))
            {
                Debug.LogError($"[IAmAsIAm] Couldn't read save slot {slot}.");
                return false;
            }

            GameServices.InGameDay = data.inGameDay;
            GameServices.PlaySeconds = data.header?.playSeconds ?? 0f;
            GameServices.Mind.Restore(data.arousal, data.windowHalfWidth);
            GameServices.Roster.ImportStates(data.members, data.frontingId, data.coConsciousIds);
            GameServices.Journal.Restore(data.journal, data.gaps);
            GameServices.Skills.Restore(data.learnedSkillIds);
            GameServices.Choices?.Restore(data.choices);
            GameServices.Traditions?.Restore(data.exploredTraditionIds, data.triedPracticeIds);
            GameServices.Triggers?.Restore(data.triggerProgress);
            GameServices.ChapterRuns?.Restore(data.chapterUnlocks);
            GameServices.ChapterRuns?.RestoreRun(data.chapterRun);
            GameServices.Devices?.Restore(data.devices);

            runner ??= UnityEngine.Object.FindAnyObjectByType<StoryRunner>();
            if (runner != null && !string.IsNullOrEmpty(data.inkStateJson))
            {
                try { runner.LoadState(data.inkStateJson); }
                catch (Exception e) { Debug.LogWarning($"[IAmAsIAm] The story position in slot {slot} couldn't be restored: {e.Message}"); }
            }

            ActiveSlot = slot;
            RememberLastSlot(slot);
            Loaded?.Invoke();
            return true;
        }

        public static void Delete(int slot)
        {
            if (slot == ActiveSlot) return; // Never delete the game being played.
            Slots.Delete(slot);
        }

        /// <summary>Back to the title screen: no slot is active.</summary>
        internal static void ClearActiveSlot() => ActiveSlot = 0;

        static void RememberLastSlot(int slot)
        {
            var prefs = GameServices.Preferences;
            if (prefs == null || prefs.lastSlot == slot) return;
            prefs.lastSlot = slot;
            GameServices.SavePreferences();
        }

        public static PreferencesData LoadPreferences()
        {
            try
            {
                string json = Storage.Read(PreferencesKey);
                return string.IsNullOrEmpty(json) ? new PreferencesData() : JsonUtility.FromJson<PreferencesData>(json);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[IAmAsIAm] Preferences unreadable, using defaults: {e.Message}");
                return new PreferencesData();
            }
        }

        public static void SavePreferences(PreferencesData prefs)
        {
            Storage.Write(PreferencesKey, JsonUtility.ToJson(prefs));
            GameServices.NotifyComfortChanged();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            _storage = null;
            _slots = null;
            ActiveSlot = 0;
            Loaded = null;
            Saved = null;
        }
    }
}
