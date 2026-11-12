using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using IAmAsIAm.Core.Save;
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

    /// <summary>Builds <see cref="SaveData"/> from <see cref="GameServices"/>, and restores it.</summary>
    public static class SaveService
    {
        const string PreferencesKey = "preferences";

        static ISaveStorage _storage;

        public static ISaveStorage Storage
        {
            get => _storage ??= new FileSaveStorage();
            set => _storage = value;
        }

        public static void Save(string slot, string chapterId)
        {
            var mind = GameServices.Mind;
            var roster = GameServices.Roster;
            var data = new SaveData
            {
                chapterId = chapterId,
                inGameDay = GameServices.InGameDay,
                arousal = mind.Arousal,
                windowHalfWidth = mind.WindowHalfWidth,
                frontingId = roster.FrontingId,
                coConsciousIds = roster.CoConscious.ToList(),
                members = roster.ExportStates().ToList(),
                journal = GameServices.Journal.Entries.ToList(),
                gaps = GameServices.Journal.Gaps.ToList(),
                learnedSkillIds = GameServices.Skills.LearnedIds.ToList(),
                chapterUnlocks = GameServices.ChapterRuns?.Export() ?? new List<IAmAsIAm.Core.Story.ChapterUnlock>(),
            };
            Storage.Write(slot, JsonUtility.ToJson(data));
        }

        public static bool TryLoad(string slot, out SaveData data)
        {
            data = null;
            string json = Storage.Read(slot);
            if (string.IsNullOrEmpty(json)) return false;
            try
            {
                data = JsonUtility.FromJson<SaveData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[IAmAsIAm] Couldn't read save '{slot}': {e.Message}");
                return false;
            }

            GameServices.InGameDay = data.inGameDay;
            GameServices.Mind.Restore(data.arousal, data.windowHalfWidth);
            GameServices.Roster.ImportStates(data.members, data.frontingId, data.coConsciousIds);
            GameServices.Journal.Restore(data.journal, data.gaps);
            GameServices.Skills.Restore(data.learnedSkillIds);
            GameServices.ChapterRuns?.Restore(data.chapterUnlocks);
            return true;
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
        static void ResetStatics() => _storage = null;
    }
}
