using System;
using System.Collections.Generic;
using IAmAsIAm.Core.Mind;

namespace IAmAsIAm.Core.Skills
{
    public enum SkillFamily
    {
        Grounding,
        Breathing,
        Mindfulness,
        DistressTolerance,
        EmotionRegulation,
        InterpersonalEffectiveness,
        Cbt,
        PluralCommunication,
    }

    /// <summary>
    /// One codex entry. Authored in Resources/Data/skills.json.
    /// Steps must match the source; see TherapeuticSkillsReference.md.
    /// </summary>
    [Serializable]
    public sealed class SkillEntry
    {
        public string id;
        public string name;
        /// <summary>For acronym skills: e.g. "TIPP", "DEAR MAN".</summary>
        public string acronym;
        /// <summary>A <see cref="SkillFamily"/> name. Stored as a string so the JSON stays human-editable.</summary>
        public string family;
        /// <summary>A <see cref="SkillDirection"/> name.</summary>
        public string direction = nameof(SkillDirection.Either);
        public string summary;
        public List<string> steps = new List<string>();
        public string whenToUse;
        /// <summary>Medical or emotional cautions. Shown prominently, never collapsed by default.</summary>
        public List<string> cautions = new List<string>();
        public string source;
        /// <summary>Whether this skill has an interactive Practice Mode exercise.</summary>
        public bool hasPracticeMode;

        public SkillFamily Family => EnumText.Parse(family, SkillFamily.Grounding);
        public SkillDirection Direction => EnumText.Parse(direction, SkillDirection.Either);
    }

    [Serializable]
    public sealed class SkillCatalogData
    {
        public List<SkillEntry> skills = new List<SkillEntry>();
    }

    /// <summary>Which skills the player has learned. Learned skills are always available in Practice Mode.</summary>
    public sealed class SkillCodex
    {
        readonly Dictionary<string, SkillEntry> _all = new Dictionary<string, SkillEntry>();
        readonly HashSet<string> _learned = new HashSet<string>();
        readonly Dictionary<string, int> _practiceCounts = new Dictionary<string, int>();

        public event Action<SkillEntry> Learned;

        public SkillCodex(IEnumerable<SkillEntry> catalog)
        {
            foreach (var s in catalog) _all[s.id] = s;
        }

        public IEnumerable<SkillEntry> All => _all.Values;
        public IReadOnlyCollection<string> LearnedIds => _learned;

        public SkillEntry Get(string id) => _all.TryGetValue(id, out var s) ? s : null;
        public bool IsLearned(string id) => _learned.Contains(id);

        public bool Learn(string id)
        {
            if (!_all.TryGetValue(id, out var skill) || !_learned.Add(id)) return false;
            Learned?.Invoke(skill);
            return true;
        }

        /// <summary>Counts practice locally. Never transmitted anywhere (see ContentAndSafetyGuidelines.md §1).</summary>
        public void RecordPractice(string id)
        {
            _practiceCounts.TryGetValue(id, out int n);
            _practiceCounts[id] = n + 1;
        }

        public int PracticeCount(string id) => _practiceCounts.TryGetValue(id, out int n) ? n : 0;

        public void Restore(IEnumerable<string> learnedIds)
        {
            _learned.Clear();
            if (learnedIds == null) return;
            foreach (var id in learnedIds) if (_all.ContainsKey(id)) _learned.Add(id);
        }
    }
}
