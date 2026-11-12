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
        /// <summary>Candle colours for candle magick (a colour and what it's for).</summary>
        public List<CandleColour> candleColours = new List<CandleColour>();
        /// <summary>Cards that can sometimes stir something up (owner: practice can be hard, sometimes).</summary>
        public List<string> hardCardIds = new List<string>();
        /// <summary>Runes that can sometimes stir something up (hail, need, ice).</summary>
        public List<string> hardRuneIds = new List<string>();
        /// <summary>Charm bag ingredients: herb and stone ids (herbs.json, crystals.json) and "something of yours".</summary>
        public List<string> charmHerbIds = new List<string>();
        public List<string> charmStoneIds = new List<string>();
        public List<string> charmOwnThings = new List<string>();
        /// <summary>Small things that turn up in the charm bag after lost time (owner: alters react to it). Never says who.</summary>
        public List<string> charmAdditions = new List<string>();
        /// <summary>Who takes the bag out of her pocket and leaves it in the altar's drawer (owner: Jacob despises the tools).</summary>
        public List<string> charmRemoverIds = new List<string>();
        /// <summary>Who, in lost time, puts something small into the bag (Izzy).</summary>
        public List<string> charmAdderIds = new List<string>();
    }

    /// <summary>
    /// A herb or a stone Maya has (owner: she gains them from different people through the game, and they can go in a
    /// charm bag). Who gave it is kept and shown ("from Selene").
    /// </summary>
    [Serializable]
    public sealed class CharmIngredient
    {
        /// <summary>"herb" (herbs.json) or "stone" (crystals.json).</summary>
        public string kind;
        public string id;
        /// <summary>A people.json id, or "" if she found or bought it herself.</summary>
        public string giverId;
        public int day;
        public string chapterId;
    }

    /// <summary>One thing that turned up in the charm bag. Who put it there is saved, never shown.</summary>
    [Serializable]
    public sealed class CharmBagAddition
    {
        public string thing;
        public string memberId;
        public bool noticed;
    }

    /// <summary>
    /// The charm bag Maya made (owner: she carries it and can hold it to ground; alters react to it). It wears out after
    /// a while (the knots come loose) and she can make another.
    /// </summary>
    [Serializable]
    public sealed class CharmBagState
    {
        public bool has;
        /// <summary>In her pocket (false = left in the altar's drawer).</summary>
        public bool carried;
        public string herb = "";
        public string stone = "";
        public string own = "";
        public string intent = "";
        public int madeDay;
        public int holds;
        public List<CharmBagAddition> added = new List<CharmBagAddition>();
    }

    [Serializable]
    public sealed class CandleColour
    {
        public string id;
        public string name;
        public string meaning;
    }

    /// <summary>The Book of Shadows' tabs. Shelf = the herbs and stones she has (what they mean, who gave them); nothing is written there.</summary>
    public enum BookTab { Practices, Readings, Wheel, Dreams, Notes, Shelf }

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
        public CharmBagState charmBag = new CharmBagState();
        public List<CharmIngredient> ingredients = new List<CharmIngredient>();
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
        readonly CraftData _data;
        readonly List<LearnedPractice> _learned = new List<LearnedPractice>();
        readonly List<BookEntry> _entries = new List<BookEntry>();
        int _nextId = 1;

        public BookOfShadows(CraftData data)
        {
            data ??= new CraftData();
            _data = data;
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

        // ── The charm bag ───────────────────────────────────────────────────────

        /// <summary>How many times it can be held before the knots come loose.</summary>
        public const int CharmBagHolds = 6;

        public CharmBagState CharmBag { get; private set; } = new CharmBagState();

        readonly List<CharmIngredient> _ingredients = new List<CharmIngredient>();

        /// <summary>The herbs and stones she has, in the order she got them.</summary>
        public IReadOnlyList<CharmIngredient> Ingredients => _ingredients;

        public bool HasIngredient(string kind, string id) => _ingredients.Any(i => i.kind == kind && i.id == id);

        public List<CharmIngredient> IngredientsOf(string kind) => _ingredients.Where(i => i.kind == kind).ToList();

        /// <summary>Someone gives her a herb or a stone (or she finds one). Returns true if it's new to her.</summary>
        public bool GainIngredient(string kind, string id, string giverId, int day, string chapterId = "")
        {
            if ((kind != "herb" && kind != "stone") || string.IsNullOrEmpty(id) || HasIngredient(kind, id)) return false;
            _ingredients.Add(new CharmIngredient { kind = kind, id = id, giverId = giverId ?? "", day = day, chapterId = chapterId ?? "" });
            Changed?.Invoke();
            return true;
        }

        /// <summary>A charm bag needs at least one herb and one stone she has been given.</summary>
        public bool CanMakeCharmBag => _ingredients.Any(i => i.kind == "herb") && _ingredients.Any(i => i.kind == "stone");

        /// <summary>She made a new charm bag (it replaces the old one) and put it in her pocket.</summary>
        public void MakeCharmBag(string herb, string stone, string own, string intent, int day)
        {
            CharmBag = new CharmBagState
            {
                has = true, carried = true, herb = herb ?? "", stone = stone ?? "", own = own ?? "",
                intent = (intent ?? "").Trim(), madeDay = day,
            };
            Changed?.Invoke();
        }

        /// <summary>Can the one fronting hold it right now? Only the ones who practise (owner); it has to be in her pocket.</summary>
        public bool CanHoldCharmBag(string frontingId) => CharmBag.has && CharmBag.carried && Practises(frontingId);

        /// <summary>
        /// She holds it (a touch anchor). Returns what's new in it (things that turned up in lost time, never saying who),
        /// and whether the knots came loose this time (then it's done, and she can make another).
        /// </summary>
        public (List<string> found, bool cameApart) HoldCharmBag()
        {
            var found = new List<string>();
            if (!CharmBag.has || !CharmBag.carried) return (found, false);
            foreach (var a in CharmBag.added)
                if (!a.noticed) { a.noticed = true; found.Add(a.thing); }
            CharmBag.holds++;
            bool apart = CharmBag.holds >= CharmBagHolds;
            if (apart) { CharmBag.has = false; CharmBag.carried = false; }
            Changed?.Invoke();
            return (found, apart);
        }

        /// <summary>She takes it back out of the altar's drawer.</summary>
        public void TakeCharmBag()
        {
            if (!CharmBag.has || CharmBag.carried) return;
            CharmBag.carried = true;
            Changed?.Invoke();
        }

        /// <summary>
        /// Someone else came forward while she carried the bag (owner: alters react to it). Some leave it alone; Jacob takes
        /// it out of her pocket and puts it in the drawer; Izzy sometimes adds something small. Nothing is ever pointed out.
        /// <paramref name="roll"/> is 0..1 (random); <paramref name="pick"/> chooses which thing turns up.
        /// </summary>
        public bool OnFrontChanged(string newId, double roll, int pick)
        {
            if (!CharmBag.has || !CharmBag.carried || string.IsNullOrEmpty(newId)) return false;
            if ((_data.charmRemoverIds ?? new List<string>()).Contains(newId) && roll < 0.6)
            {
                CharmBag.carried = false;
                Changed?.Invoke();
                return true;
            }
            var things = _data.charmAdditions ?? new List<string>();
            if ((_data.charmAdderIds ?? new List<string>()).Contains(newId) && roll < 0.4 && CharmBag.added.Count < 3 && things.Count > 0)
            {
                var left = things.Where(t => CharmBag.added.All(a => a.thing != t)).ToList();
                if (left.Count == 0) return false;
                CharmBag.added.Add(new CharmBagAddition { thing = left[Math.Abs(pick) % left.Count], memberId = newId });
                Changed?.Invoke();
                return true;
            }
            return false;
        }

        public BookOfShadowsState Export() => new BookOfShadowsState
        {
            charmBag = CharmBag,
            ingredients = _ingredients.ToList(),
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
            CharmBag = state?.charmBag ?? new CharmBagState();
            CharmBag.added ??= new List<CharmBagAddition>();
            _ingredients.Clear();
            if (state?.ingredients != null) _ingredients.AddRange(state.ingredients.Where(i => i != null && !string.IsNullOrEmpty(i.id)));
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
