using System;
using System.Collections.Generic;
using IAmAsIAm.Core.Devices;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Safety;
using IAmAsIAm.Core.Skills;
using IAmAsIAm.Core.Story;
using IAmAsIAm.Core.Triggers;

namespace IAmAsIAm.Core.Save
{
    /// <summary>
    /// Everything persisted. Stored locally only. Journal text is private and is never uploaded
    /// or included in crash reports.
    /// </summary>
    [Serializable]
    public sealed class SaveData
    {
        /// <summary>2: save slots (header, chapter run in progress, play time). 3: devices and messages. 4: the Book of Shadows.</summary>
        public const int CurrentVersion = 4;

        public int version = CurrentVersion;
        /// <summary>What the save screen shows. Also kept in its own small file (see <see cref="SaveSlots"/>).</summary>
        public SaveSlotHeader header = new SaveSlotHeader();
        public string chapterId;
        public int inGameDay;

        public float arousal;
        public float windowHalfWidth;

        public string frontingId;
        public List<string> coConsciousIds = new List<string>();
        public List<SystemMemberState> members = new List<SystemMemberState>();

        public List<JournalEntry> journal = new List<JournalEntry>();
        public List<TimeGap> gaps = new List<TimeGap>();
        public List<ThoughtRecord> thoughtRecords = new List<ThoughtRecord>();

        public List<string> learnedSkillIds = new List<string>();
        public List<string> collectedPracticeIds = new List<string>();
        public List<string> storyFlags = new List<string>();

        /// <summary>Ink's own state (story position, variables, visit counts) as JSON.</summary>
        public string inkStateJson;
        public List<ChoiceRecord> choices = new List<ChoiceRecord>();
        public List<string> exploredTraditionIds = new List<string>();
        public List<string> triedPracticeIds = new List<string>();
        public List<string> craftedItemIds = new List<string>();
        public List<TriggerProgress> triggerProgress = new List<TriggerProgress>();
        /// <summary>Alters unlocked as chapter starters. Per save: they do NOT carry across saves (owner, 2026-11-12).</summary>
        public List<ChapterUnlock> chapterUnlocks = new List<ChapterUnlock>();
        /// <summary>A chapter run in progress when the game saved (empty chapterId = none).</summary>
        public ChapterRunState chapterRun = new ChapterRunState();

        /// <summary>Maya's age in the scene, her devices, and her texts and emails (v3; empty in older saves).</summary>
        public DevicesState devices = new DevicesState();

        /// <summary>Maya's Book of Shadows: practices learned, readings, sabbats, dreams, notes (v4; empty in older saves).</summary>
        public IAmAsIAm.Core.Practices.BookOfShadowsState book = new IAmAsIAm.Core.Practices.BookOfShadowsState();

        /// <summary>
        /// Saved in the middle of a chapter (owner: "Continue" started from the beginning): the story stopped at a choice,
        /// and "Continue" picks it up there, in this place, with this goal line. False = between chapters.
        /// </summary>
        public bool storyResume;
        public string storyPlace = "";
        public string storyObjective = "";
        /// <summary>The Ink path of the checkpoint (e.g. ch1_part3.samhain): if the story changed in an update and the exact
        /// position can't be restored, the chapter part starts again from its beginning instead of from the very start.</summary>
        public string storyPath = "";
    }

    /// <summary>
    /// Saved separately from game progress, so comfort choices persist across save slots and
    /// apply from the title screen.
    /// </summary>
    [Serializable]
    public sealed class PreferencesData
    {
        public ComfortSettings comfort = new ComfortSettings();
        public bool acknowledgedNotTherapyNotice;

        public string crisisRegion;

        /// <summary>The save slot played last (1-based), or 0 for none. The title screen highlights it.</summary>
        public int lastSlot;
        /// <summary>The card the last loading screen showed, so the next one is a different card.</summary>
        public string lastLoadingCardId;
    }

    /// <summary>
    /// Platform storage abstraction. Implementations: a file-based one (PC) in the Runtime layer,
    /// and platform SDK backends for consoles later.
    /// </summary>
    public interface ISaveStorage
    {
        bool Exists(string key);
        string Read(string key);
        void Write(string key, string contents);
        void Delete(string key);
    }

    /// <summary>In-memory storage for tests and editor play.</summary>
    public sealed class MemorySaveStorage : ISaveStorage
    {
        readonly Dictionary<string, string> _store = new Dictionary<string, string>();
        public bool Exists(string key) => _store.ContainsKey(key);
        public string Read(string key) => _store.TryGetValue(key, out var v) ? v : null;
        public void Write(string key, string contents) => _store[key] = contents;
        public void Delete(string key) => _store.Remove(key);
    }
}
