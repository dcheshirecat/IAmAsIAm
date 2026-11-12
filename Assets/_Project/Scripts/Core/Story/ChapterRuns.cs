using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.Story
{
    /// <summary>Which alters can start a chapter, besides its default starter (saved with preferences).</summary>
    [Serializable]
    public sealed class ChapterUnlock
    {
        public string chapterId;
        public List<string> memberIds = new List<string>();
    }

    /// <summary>
    /// The owner's replay rule (2026-11-11): a chapter starts with its default alter (the prologue: young Maya).
    /// Anyone who fronts during a run of the chapter becomes a possible starting alter for that chapter,
    /// but only once the chapter is finished in full. Abandoning a run unlocks nothing.
    /// Paced (owner): at most <see cref="MaxNewUnlocksPerRun"/> new alter per finished run, the first new one to front.
    /// </summary>
    public sealed class ChapterRuns
    {
        readonly Dictionary<string, HashSet<string>> _unlocked = new Dictionary<string, HashSet<string>>();
        readonly List<string> _frontedThisRun = new List<string>();

        /// <summary>How many new starting alters one finished run can unlock (the owner wants this paced).</summary>
        public int MaxNewUnlocksPerRun { get; set; } = 1;

        public string CurrentChapterId { get; private set; }
        public string StartedAsId { get; private set; }
        public bool InChapter => CurrentChapterId != null;
        public IReadOnlyCollection<string> FrontedThisRun => _frontedThisRun;

        /// <summary>Raised when finishing a chapter unlocks new starting alters.</summary>
        public event Action<string, IReadOnlyList<string>> Unlocked;

        public void Begin(string chapterId, string startingMemberId)
        {
            if (string.IsNullOrEmpty(chapterId)) throw new ArgumentException("A chapter run needs a chapter id.");
            CurrentChapterId = chapterId;
            StartedAsId = startingMemberId;
            _frontedThisRun.Clear();
            if (!string.IsNullOrEmpty(startingMemberId)) _frontedThisRun.Add(startingMemberId);

        }

        /// <summary>Call whenever someone comes to the front (by choice or not) during a chapter.</summary>
        public void RecordFront(string memberId)
        {
            if (InChapter && !string.IsNullOrEmpty(memberId) && !_frontedThisRun.Contains(memberId)) _frontedThisRun.Add(memberId);
        }

        /// <summary>The chapter was finished in full: everyone who fronted can now start it. Returns the newly unlocked.</summary>
        public IReadOnlyList<string> Complete()
        {
            if (!InChapter) return Array.Empty<string>();
            if (!_unlocked.TryGetValue(CurrentChapterId, out var set)) _unlocked[CurrentChapterId] = set = new HashSet<string>();
            var fresh = _frontedThisRun.Where(id => id != StartedAsId && !set.Contains(id)).Take(Math.Max(0, MaxNewUnlocksPerRun)).ToList();
            foreach (var id in fresh) set.Add(id);
            string chapter = CurrentChapterId;
            CurrentChapterId = null;
            _frontedThisRun.Clear();
            if (fresh.Count > 0) Unlocked?.Invoke(chapter, fresh);
            return fresh;
        }

        /// <summary>Left the chapter before the end: nothing is unlocked.</summary>
        public void Abandon()
        {
            CurrentChapterId = null;
            _frontedThisRun.Clear();
        }

        /// <summary>Who can start this chapter: the default starter first, then the unlocked alters.</summary>
        public IReadOnlyList<string> StartersFor(string chapterId, string defaultMemberId)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(defaultMemberId)) list.Add(defaultMemberId);
            if (_unlocked.TryGetValue(chapterId ?? "", out var set))
                list.AddRange(set.Where(id => id != defaultMemberId).OrderBy(id => id, StringComparer.Ordinal));
            return list;
        }

        public List<ChapterUnlock> Export() =>
            _unlocked.Select(kv => new ChapterUnlock { chapterId = kv.Key, memberIds = kv.Value.OrderBy(x => x, StringComparer.Ordinal).ToList() }).ToList();

        public void Restore(IEnumerable<ChapterUnlock> unlocks)
        {
            _unlocked.Clear();
            foreach (var u in unlocks ?? Enumerable.Empty<ChapterUnlock>())
            {
                if (string.IsNullOrEmpty(u?.chapterId)) continue;
                if (!_unlocked.TryGetValue(u.chapterId, out var set)) _unlocked[u.chapterId] = set = new HashSet<string>();
                foreach (var id in u.memberIds ?? new List<string>()) if (!string.IsNullOrEmpty(id)) set.Add(id);
            }
        }
    }
}
