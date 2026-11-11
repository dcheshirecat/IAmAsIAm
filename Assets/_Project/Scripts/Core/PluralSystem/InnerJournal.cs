using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.PluralSystem
{
    public enum JournalEntryKind
    {
        /// <summary>A note one part leaves for others. A real system-communication practice.</summary>
        Note,
        /// <summary>An agreement between parts. Keeping agreements builds trust.</summary>
        Agreement,
        /// <summary>What the system pieced together after time loss.</summary>
        GapRecovery,
        ThoughtRecord,
        TarotReflection,
        RuneReflection,
        Gratitude,
    }

    [Serializable]
    public sealed class JournalEntry
    {
        public string id;
        public JournalEntryKind kind;
        /// <summary>Member who wrote it. Null if unknown, which is meaningful in itself (unfamiliar handwriting).</summary>
        public string authorId;
        public int inGameDay;
        public string text;
        /// <summary>Optional link to a skill, card, rune or gap id.</summary>
        public string referenceId;
        /// <summary>For Agreements: whether it was kept.</summary>
        public bool kept;
    }

    /// <summary>
    /// A period of time loss. The player gently reconstructs it from clues. Some pieces may stay
    /// missing, and that's allowed. Recovery is never required to progress.
    /// </summary>
    [Serializable]
    public sealed class TimeGap
    {
        public string id;
        public int inGameDay;
        /// <summary>Who was fronting during the gap, if the system ever learns it.</summary>
        public string frontedById;
        public List<string> clueIds = new List<string>();
        public List<string> foundClueIds = new List<string>();

        public float Recovered => clueIds.Count == 0 ? 1f : (float)foundClueIds.Count / clueIds.Count;
    }

    /// <summary>The shared notebook between parts.</summary>
    public sealed class InnerJournal
    {
        readonly List<JournalEntry> _entries = new List<JournalEntry>();
        readonly Dictionary<string, TimeGap> _gaps = new Dictionary<string, TimeGap>();
        int _nextId;

        public IReadOnlyList<JournalEntry> Entries => _entries;
        public IEnumerable<TimeGap> Gaps => _gaps.Values;

        public event Action<JournalEntry> EntryAdded;
        public event Action<TimeGap, string> ClueFound;

        public JournalEntry Write(JournalEntryKind kind, string authorId, int day, string text, string referenceId = null)
        {
            var entry = new JournalEntry
            {
                id = $"j{_nextId++}",
                kind = kind,
                authorId = authorId,
                inGameDay = day,
                text = text ?? string.Empty,
                referenceId = referenceId,
            };
            _entries.Add(entry);
            EntryAdded?.Invoke(entry);
            return entry;
        }

        public IEnumerable<JournalEntry> ByAuthor(string authorId) => _entries.Where(e => e.authorId == authorId);

        public TimeGap OpenGap(string gapId, int day, string frontedById, IEnumerable<string> clueIds)
        {
            var gap = new TimeGap
            {
                id = gapId,
                inGameDay = day,
                frontedById = frontedById,
                clueIds = clueIds?.ToList() ?? new List<string>(),
            };
            _gaps[gapId] = gap;
            return gap;
        }

        public TimeGap GetGap(string gapId) => _gaps.TryGetValue(gapId, out var g) ? g : null;

        /// <returns>True if this was a new clue for the gap.</returns>
        public bool FindClue(string gapId, string clueId)
        {
            if (!_gaps.TryGetValue(gapId, out var gap)) return false;
            if (!gap.clueIds.Contains(clueId) || gap.foundClueIds.Contains(clueId)) return false;
            gap.foundClueIds.Add(clueId);
            ClueFound?.Invoke(gap, clueId);
            return true;
        }

        public void Restore(IEnumerable<JournalEntry> entries, IEnumerable<TimeGap> gaps)
        {
            _entries.Clear();
            _gaps.Clear();
            if (entries != null) _entries.AddRange(entries);
            if (gaps != null) foreach (var g in gaps) _gaps[g.id] = g;
            _nextId = _entries.Count;
        }
    }
}
