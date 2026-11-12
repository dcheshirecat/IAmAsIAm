using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.Practices
{
    /// <summary>One Wiccan practice Maya can learn (craft.json). Open practices only (owner, 2026-11-12).</summary>
    [Serializable]
    public sealed class CraftPractice
    {
        public string id;
        public string name;
        /// <summary>circle, divination, candle, charm, devotion or wheel.</summary>
        public string category;
        /// <summary>The steps, in Maya's own words: what goes in the Book of Shadows the first time she does it.</summary>
        public List<string> steps = new List<string>();
        /// <summary>Can this practice sometimes stir something up (owner: "yes, sometimes")?</summary>
        public bool canBeHard;
    }

    /// <summary>What an alter who doesn't practise Wicca says if the player tries to open the Book while they front.</summary>
    [Serializable]
    public sealed class CraftRefusal
    {
        public string memberId;
        public string line;
    }

    [Serializable]
    public sealed class CraftData
    {
        /// <summary>Owner: only Maya and Izzy practise Wicca. Everyone else keeps their own faith and leaves the tools alone.</summary>
        public List<string> wiccanMemberIds = new List<string>();
        public List<CraftPractice> practices = new List<CraftPractice>();
        public List<CraftRefusal> refusals = new List<CraftRefusal>();
    }

    /// <summary>The Book of Shadows' tabs.</summary>
    public enum BookTab { Practices, Readings, Wheel, Dreams, Notes }

    [Serializable]
    public sealed class LearnedPractice
    {
        public string practiceId;
        /// <summary>A person id (people.json), e.g. "tanya" or "selene"; empty if she taught herself.</summary>
        public string teacherId;
        public int day;
        public string chapterId;
    }

    [Serializable]
    public sealed class BookEntry
    {
        public int id;
        public string tab;          // BookTab, as text
        /// <summary>Whose hand wrote it. Shown only as handwriting, never as a name (owner: lost time is never pointed out).</summary>
        public string memberId;
        public string title;
        public string text;
        public int day;
        public string chapterId;
        public bool byPlayer;

        public BookTab Tab => Enum.TryParse<BookTab>(tab, out var t) ? t : BookTab.Notes;
    }

    [Serializable]
    public sealed class BookOfShadowsState
    {
        public bool has;
        public string givenById;
        public int givenDay;
        public List<LearnedPractice> learned = new List<LearnedPractice>();
        public List<BookEntry> entries = new List<BookEntry>();
        public int nextId = 1;
    }

    /// <summary>
    /// Maya's Book of Shadows (owner, 2026-11-12): Tanya gives it to her at Yule, December 2017. She can open it any time,
    /// like the phone. The steps of each practice go in the first time she learns it; readings, sabbats, dreams and notes
    /// are written in as she goes. Only Maya and Izzy practise Wicca: for anyone else it stays shut.
    /// Pure rules (no Unity).
    /// </summary>
    public sealed class BookOfShadows
    {
        public const int MaxEntryLength = 1000;

        readonly Dictionary<string, CraftPractice> _practices;
        readonly HashSet<string> _wiccan;
        readonly Dictionary<string, string> _refusals;
        readonly List<LearnedPractice> _learned = new List<LearnedPractice>();
        readonly List<BookEntry> _entries = new List<BookEntry>();
        int _nextId = 1;

        public BookOfShadows(CraftData data)
        {
            data ??= new CraftData();
            _practices = (data.practices ?? new List<CraftPractice>()).Where(p => !string.IsNullOrEmpty(p.id))
                .GroupBy(p => p.id).ToDictionary(g => g.Key, g => g.First());
            _wiccan = new HashSet<string>(data.wiccanMemberIds ?? new List<string>());
            _refusals = (data.refusals ?? new List<CraftRefusal>()).Where(r => !string.IsNullOrEmpty(r.memberId))
                .GroupBy(r => r.memberId).ToDictionary(g => g.Key, g => g.First().line ?? "");
        }

        public bool Has { get; private set; }
        public string GivenById { get; private set; } = "";
        public event Action Changed;

        public IReadOnlyList<LearnedPractice> Learned => _learned;
        public IReadOnlyList<BookEntry> Entries => _entries;
        public IEnumerable<CraftPractice> AllPractices => _practices.Values;

        public CraftPractice Practice(string id) => id != null && _practices.TryGetValue(id, out var p) ? p : null;

        /// <summary>Does this alter practise Wicca (open the Book, use the altar's practices)?</summary>
        public bool Practises(string memberId) => !string.IsNullOrEmpty(memberId) && _wiccan.Contains(memberId);

        /// <summary>What someone who doesn't practise says when the Book is tried while they front.</summary>
        public string RefusalLine(string memberId) =>
            memberId != null && _refusals.TryGetValue(memberId, out var l) && !string.IsNullOrEmpty(l) ? l : "It stays closed.";

        /// <summary>Whether the Book can be opened right now, with this alter fronting.</summary>
        public bool CanOpen(string frontingId) => Has && Practises(frontingId);

        /// <summary>Tanya's gift (owner: at Yule, December 2017).</summary>
        public void Give(string fromId, int day)
        {
            if (Has) return;
            Has = true;
            GivenById = fromId ?? "";
            Changed?.Invoke();
        }

        public bool HasLearned(string practiceId) => _learned.Any(l => l.practiceId == practiceId);

        /// <summary>
        /// She learns a practice. The first time, its steps go in the Practices tab (with who taught her). Returns true if
        /// it was new. Learning before she has the Book still counts (Selene teaches her tarot first); the steps are there
        /// when she gets it.
        /// </summary>
        public bool Learn(string practiceId, string teacherId, int day, string chapterId = "")
        {
            if (Practice(practiceId) == null || HasLearned(practiceId)) return false;
            _learned.Add(new LearnedPractice { practiceId = practiceId, teacherId = teacherId ?? "", day = day, chapterId = chapterId ?? "" });
            Changed?.Invoke();
            return true;
        }

        /// <summary>Writes an entry. Blank ones are ignored; long ones are cut to <see cref="MaxEntryLength"/>.</summary>
        public BookEntry Write(BookTab tab, string memberId, string title, string text, int day, string chapterId = "", bool byPlayer = false)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(memberId)) return null;
            text = text.Trim();
            if (text.Length > MaxEntryLength) text = text.Substring(0, MaxEntryLength);
            var e = new BookEntry
            {
                id = _nextId++, tab = tab.ToString(), memberId = memberId, title = (title ?? "").Trim(), text = text,
                day = day, chapterId = chapterId ?? "", byPlayer = byPlayer,
            };
            _entries.Add(e);
            Changed?.Invoke();
            return e;
        }

        /// <summary>One tab's entries, newest first.</summary>
        public List<BookEntry> EntriesIn(BookTab tab) => _entries.Where(e => e.Tab == tab).OrderByDescending(e => e.id).ToList();

        /// <summary>The practices she has learned, in the order she learned them, with their steps.</summary>
        public List<(CraftPractice practice, LearnedPractice learned)> LearnedPractices() =>
            _learned.Select(l => (Practice(l.practiceId), l)).Where(p => p.Item1 != null).ToList();

        public BookOfShadowsState Export() => new BookOfShadowsState
        {
            has = Has,
            givenById = GivenById,
            learned = _learned.ToList(),
            entries = _entries.ToList(),
            nextId = _nextId,
        };

        public void Restore(BookOfShadowsState state)
        {
            _learned.Clear();
            _entries.Clear();
            Has = state?.has ?? false;
            GivenById = state?.givenById ?? "";
            _nextId = 1;
            if (state != null)
            {
                _learned.AddRange((state.learned ?? new List<LearnedPractice>()).Where(l => l != null && !string.IsNullOrEmpty(l.practiceId)));
                _entries.AddRange((state.entries ?? new List<BookEntry>()).Where(e => e != null));
                _nextId = Math.Max(state.nextId, (_entries.Count == 0 ? 0 : _entries.Max(e => e.id)) + 1);
            }
            Changed?.Invoke();
        }
    }
}
