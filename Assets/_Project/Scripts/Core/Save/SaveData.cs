using System;
using System.Collections.Generic;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Safety;
using IAmAsIAm.Core.Skills;

namespace IAmAsIAm.Core.Save
{
    /// <summary>
    /// Everything persisted. Stored locally only. Journal text is private and is never uploaded
    /// or included in crash reports.
    /// </summary>
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentVersion = 1;

        public int version = CurrentVersion;
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
