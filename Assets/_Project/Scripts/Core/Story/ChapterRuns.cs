using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.Story
{
    /// <summary>Which alters can start a chapter, besides its default starter (saved per save slot, never shared).</summary>
    [Serializable]
    public sealed class ChapterUnlock
    {
        public string chapterId;
        public List<string> memberIds = new List<string>();
    }

    /// <summary>A chapter run in progress, as saved (so a save between scenes of a chapter keeps the run).</summary>
    [Serializable]
    public sealed class ChapterRunState
    {
        /// <summary>Empty when no chapter was being played.</summary>
        public string chapterId;
        public string startedAsId;
        public List<string> frontedIds = new List<string>();
        /// <summary>True when finishing this run unlocks the alter it started as (a forced first run, e.g. Axel's Chapter 1.5).</summary>
        public bool starterUnlocks;
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
        /// <summary>
        /// True when finishing this run unlocks the alter it started as. Normally the starter is already unlocked (or the
        /// chapter's default); a forced first run (owner: Chapter 1.5 is always Axel's first) is how that alter gets unlocked.
        /// </summary>
        public bool StarterUnlocks { get; private set; }
        public bool InChapter => CurrentChapterId != null;
        public IReadOnlyCollection<string> FrontedThisRun => _frontedThisRun;

        /// <summary>Raised when finishing a chapter unlocks new starting alters.</summary>
        public event Action<string, IReadOnlyList<string>> Unlocked;

        public void Begin(string chapterId, string startingMemberId, bool starterUnlocks = false)
        {
            if (string.IsNullOrEmpty(chapterId)) throw new ArgumentException("A chapter run needs a chapter id.");
            CurrentChapterId = chapterId;
            StartedAsId = startingMemberId;
            StarterUnlocks = starterUnlocks && !string.IsNullOrEmpty(startingMemberId);
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
            var fresh = _frontedThisRun.Where(id => (StarterUnlocks || id != StartedAsId) && !set.Contains(id)).Take(Math.Max(0, MaxNewUnlocksPerRun)).ToList();
            foreach (var id in fresh) set.Add(id);
            string chapter = CurrentChapterId;
            CurrentChapterId = null;
            StarterUnlocks = false;
            _frontedThisRun.Clear();
            if (fresh.Count > 0) Unlocked?.Invoke(chapter, fresh);
            return fresh;
        }

        /// <summary>Left the chapter before the end: nothing is unlocked.</summary>
        public void Abandon()
        {
            CurrentChapterId = null;
            StarterUnlocks = false;
            _frontedThisRun.Clear();
        }

        /// <summary>
        /// Who can start a chapter. With <paramref name="firstRunMemberId"/> set and the chapter never finished in this save,
        /// only that member can (owner: Chapter 1.5 is Axel's the first time).
        /// </summary>
        public IReadOnlyList<string> StartersFor(string chapterId, string defaultMemberId, string firstRunMemberId)
        {
            if (!string.IsNullOrEmpty(firstRunMemberId) && !HasFinished(chapterId)) return new List<string> { firstRunMemberId };
            return StartersFor(chapterId, defaultMemberId);
        }

        /// <summary>True once this chapter has been played through in full in this save.</summary>
        public bool HasFinished(string chapterId) => _unlocked.ContainsKey(chapterId ?? "");

        /// <summary>Who can start this chapter: the default starter first, then the unlocked alters.</summary>

        public IReadOnlyList<string> StartersFor(string chapterId, string defaultMemberId)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(defaultMemberId)) list.Add(defaultMemberId);
            if (_unlocked.TryGetValue(chapterId ?? "", out var set))
                list.AddRange(set.Where(id => id != defaultMemberId).OrderBy(id => id, StringComparer.Ordinal));
            return list;
        }

        /// <summary>
        /// True if this alter has been unlocked as a starter for any chapter in this save. Hidden memories and unexpected
        /// switches use it: once an alter's version of a chapter is unlocked, they can surface anywhere.
        /// </summary>
        public bool IsUnlockedAnywhere(string memberId) =>
            !string.IsNullOrEmpty(memberId) && _unlocked.Values.Any(set => set.Contains(memberId));

        /// <summary>Every alter unlocked anywhere in this save, in a stable order.</summary>
        public IReadOnlyList<string> AllUnlocked() =>
            _unlocked.Values.SelectMany(s => s).Distinct().OrderBy(id => id, StringComparer.Ordinal).ToList();

        public List<ChapterUnlock> Export() =>
            _unlocked.Select(kv => new ChapterUnlock { chapterId = kv.Key, memberIds = kv.Value.OrderBy(x => x, StringComparer.Ordinal).ToList() }).ToList();

        /// <summary>The run in progress, for saving. An empty chapterId means none.</summary>
        public ChapterRunState ExportRun() => new ChapterRunState
        {
            chapterId = CurrentChapterId ?? "",
            startedAsId = StartedAsId ?? "",
            frontedIds = InChapter ? _frontedThisRun.ToList() : new List<string>(),
            starterUnlocks = InChapter && StarterUnlocks,
        };

        /// <summary>Puts back a run in progress from a save (or clears it when the save had none).</summary>
        public void RestoreRun(ChapterRunState run)
        {
            _frontedThisRun.Clear();
            if (run == null || string.IsNullOrEmpty(run.chapterId)) { CurrentChapterId = null; StartedAsId = null; StarterUnlocks = false; return; }
            CurrentChapterId = run.chapterId;
            StarterUnlocks = run.starterUnlocks;
            StartedAsId = string.IsNullOrEmpty(run.startedAsId) ? null : run.startedAsId;
            foreach (var id in run.frontedIds ?? new List<string>())
                if (!string.IsNullOrEmpty(id) && !_frontedThisRun.Contains(id)) _frontedThisRun.Add(id);
        }

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
