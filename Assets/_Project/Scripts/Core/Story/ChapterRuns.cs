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
        /// <summary>Who this chapter has been played through as (the chapter tree's branches). Empty in older saves.</summary>
        public List<string> playedAsIds = new List<string>();
    }

    /// <summary>A chapter run in progress, as saved (so a save between scenes of a chapter keeps the run).</summary>
    [Serializable]
    public sealed class ChapterRunState
    {
        /// <summary>Empty when no chapter was being played.</summary>
        public string chapterId;
        public string startedAsId;
        public List<string> frontedIds = new List<string>();
    }

    /// <summary>
    /// The owner's chapter rules (2026-11-12):
    /// - The FIRST playthrough of a chapter always starts as that chapter's starting alter (Maya for everything up to Chapter 1;
    ///   Axel for Chapter 1.5).
    /// - Each chapter unlocks ONE set alter when it's finished in full (never random): Prologue Nina, Prelude Izzy, Chapter 1 Jacob,
    ///   Chapter 1.5 Axel. Abandoning a run unlocks nothing.
    /// - Once a chapter is finished it can be replayed starting as any alter unlocked so far (e.g. the Prelude as Nina).
    /// - Dissociation and switches can be triggered in any chapter, but only to the host or an alter already unlocked
    ///   (<see cref="CanSwitchTo"/>); story-scripted switches (# switch:) happen as written.
    /// Saved per save slot, never shared between saves.
    /// </summary>
    public sealed class ChapterRuns
    {
        /// <summary>The host is always available, whoever is fronting.</summary>
        public const string HostId = "maya";

        readonly Dictionary<string, HashSet<string>> _unlocked = new Dictionary<string, HashSet<string>>();
        readonly List<string> _frontedThisRun = new List<string>();
        readonly Dictionary<string, HashSet<string>> _playedAs = new Dictionary<string, HashSet<string>>();

        /// <summary>Who a chapter has been played through as, in full (for the chapter tree). Empty if never finished.</summary>
        public IReadOnlyCollection<string> PlayedAs(string chapterId) =>
            _playedAs.TryGetValue(chapterId ?? "", out var set) ? (IReadOnlyCollection<string>)set : Array.Empty<string>();

        public string CurrentChapterId { get; private set; }
        public string StartedAsId { get; private set; }
        public bool InChapter => CurrentChapterId != null;
        public IReadOnlyCollection<string> FrontedThisRun => _frontedThisRun;

        /// <summary>Raised when finishing a chapter unlocks its alter for the first time.</summary>
        public event Action<string, IReadOnlyList<string>> Unlocked;

        public void Begin(string chapterId, string startingMemberId)
        {
            if (string.IsNullOrEmpty(chapterId)) throw new ArgumentException("A chapter run needs a chapter id.");
            CurrentChapterId = chapterId;
            StartedAsId = startingMemberId;
            _frontedThisRun.Clear();
            if (!string.IsNullOrEmpty(startingMemberId)) _frontedThisRun.Add(startingMemberId);
        }

        /// <summary>Call whenever someone comes to the front (by choice or not) during a chapter. Kept for the story; unlocks don't use it.</summary>
        public void RecordFront(string memberId)
        {
            if (InChapter && !string.IsNullOrEmpty(memberId) && !_frontedThisRun.Contains(memberId)) _frontedThisRun.Add(memberId);
        }

        /// <summary>
        /// The chapter was finished in full: it's marked finished and its set alter (<paramref name="unlocksMemberId"/>) is unlocked.
        /// Returns the alter if this is the first time they're unlocked anywhere, else nothing.
        /// </summary>
        public IReadOnlyList<string> Complete(string unlocksMemberId = null)
        {
            if (!InChapter) return Array.Empty<string>();
            if (!_unlocked.TryGetValue(CurrentChapterId, out var set)) _unlocked[CurrentChapterId] = set = new HashSet<string>();
            var fresh = new List<string>();
            if (!string.IsNullOrEmpty(unlocksMemberId) && unlocksMemberId != HostId)
            {
                if (!IsUnlockedAnywhere(unlocksMemberId)) fresh.Add(unlocksMemberId);
                set.Add(unlocksMemberId);
            }
            string chapter = CurrentChapterId;
            if (!string.IsNullOrEmpty(StartedAsId))
            {
                if (!_playedAs.TryGetValue(chapter, out var played)) _playedAs[chapter] = played = new HashSet<string>();
                played.Add(StartedAsId);
            }
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

        /// <summary>True once this chapter has been played through in full in this save.</summary>
        public bool HasFinished(string chapterId) => _unlocked.ContainsKey(chapterId ?? "");

        /// <summary>
        /// Who can start a chapter: only its starting alter the first time; once it's finished, the starting alter and then every
        /// alter unlocked so far (in any chapter).
        /// </summary>
        public IReadOnlyList<string> StartersFor(string chapterId, string startingMemberId)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(startingMemberId)) list.Add(startingMemberId);
            if (!HasFinished(chapterId)) return list;
            if (startingMemberId != HostId && !list.Contains(HostId)) list.Add(HostId);
            foreach (var id in AllUnlocked()) if (!list.Contains(id)) list.Add(id);
            return list;
        }

        /// <summary>Who a trigger or an unexpected switch may bring forward: the host, or an alter already unlocked.</summary>
        public bool CanSwitchTo(string memberId) => memberId == HostId || IsUnlockedAnywhere(memberId);

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
            _unlocked.Select(kv => new ChapterUnlock
            {
                chapterId = kv.Key,
                memberIds = kv.Value.OrderBy(x => x, StringComparer.Ordinal).ToList(),
                playedAsIds = PlayedAs(kv.Key).OrderBy(x => x, StringComparer.Ordinal).ToList(),
            }).ToList();

        /// <summary>The run in progress, for saving. An empty chapterId means none.</summary>
        public ChapterRunState ExportRun() => new ChapterRunState
        {
            chapterId = CurrentChapterId ?? "",
            startedAsId = StartedAsId ?? "",
            frontedIds = InChapter ? _frontedThisRun.ToList() : new List<string>(),
        };

        /// <summary>Puts back a run in progress from a save (or clears it when the save had none).</summary>
        public void RestoreRun(ChapterRunState run)
        {
            _frontedThisRun.Clear();
            if (run == null || string.IsNullOrEmpty(run.chapterId)) { CurrentChapterId = null; StartedAsId = null; return; }
            CurrentChapterId = run.chapterId;
            StartedAsId = string.IsNullOrEmpty(run.startedAsId) ? null : run.startedAsId;
            foreach (var id in run.frontedIds ?? new List<string>())
                if (!string.IsNullOrEmpty(id) && !_frontedThisRun.Contains(id)) _frontedThisRun.Add(id);
        }

        public void Restore(IEnumerable<ChapterUnlock> unlocks)
        {
            _unlocked.Clear();
            _playedAs.Clear();
            foreach (var u in unlocks ?? Enumerable.Empty<ChapterUnlock>())
            {
                if (string.IsNullOrEmpty(u?.chapterId)) continue;
                if (!_unlocked.TryGetValue(u.chapterId, out var set)) _unlocked[u.chapterId] = set = new HashSet<string>();
                foreach (var id in u.memberIds ?? new List<string>()) if (!string.IsNullOrEmpty(id)) set.Add(id);
                if (!_playedAs.TryGetValue(u.chapterId, out var played)) _playedAs[u.chapterId] = played = new HashSet<string>();
                foreach (var id in u.playedAsIds ?? new List<string>()) if (!string.IsNullOrEmpty(id)) played.Add(id);
            }
        }
    }
}
