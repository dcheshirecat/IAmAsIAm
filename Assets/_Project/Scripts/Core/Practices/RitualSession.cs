using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.Practices
{
    /// <summary>
    /// The five parts every ritual has (owner: small hands-on steps that feel like rituals, not games): arrive (one breath),
    /// prepare (two or three small steps), the moment, reflect, and close.
    /// </summary>
    public enum RitualPhase { Arrive, Prepare, Moment, Reflect, Close }

    /// <summary>
    /// How a step is done. Nothing is timed against the player: a Hold that's let go early just waits; a Breathe circle
    /// goes at its own pace; there's no score and no wrong answer.
    /// </summary>
    public enum RitualStepKind
    {
        /// <summary>One slow breath with the breathing circle, then the button.</summary>
        Breathe,
        /// <summary>Press and hold the button for <see cref="RitualStep.Seconds"/> (shuffling, watching a flame, roots going down).</summary>
        Hold,
        /// <summary>One tap (light it, cut the deck, draw).</summary>
        Tap,
        /// <summary>Pick one of <see cref="RitualStep.Options"/>.</summary>
        Choose,
        /// <summary>Type something (it can be left blank).</summary>
        Write,
    }

    public sealed class RitualOption
    {
        public string Id;
        public string Label;
        public RitualOption(string id, string label) { Id = id; Label = label; }
    }

    public sealed class RitualStep
    {
        public RitualPhase Phase;
        public RitualStepKind Kind;
        public string Text;
        /// <summary>The button's word (Tap, Hold, Breathe, Write).</summary>
        public string Action;
        public float Seconds;
        public int MaxLength;
        public List<RitualOption> Options = new List<RitualOption>();
        /// <summary>Things laid out on the cloth (runes): shown in a row, each with its meaning above it.</summary>
        public List<RitualToken> Tokens = new List<RitualToken>();
    }

    /// <summary>One thing on the cloth: a rune's glyph and name, with its meaning above it (owner).</summary>
    public sealed class RitualToken
    {
        public string Glyph;
        public string Name;
        /// <summary>Shown above it: the position ("What was") and what it means.</summary>
        public string Caption;
    }

    /// <summary>One herb or stone she can put in a charm bag, with who gave it to her (a display name, or "").</summary>
    public sealed class ShelfItem
    {
        public string Id;
        public string From;
        public ShelfItem(string id, string from = "") { Id = id; From = from ?? ""; }
    }

    /// <summary>
    /// What she has to make a charm bag with (owner: gained from different people through the game). Null in the session
    /// means everything in the codex (tests, or before the shelf is known).
    /// </summary>
    public sealed class CharmShelf
    {
        public List<ShelfItem> Herbs = new List<ShelfItem>();
        public List<ShelfItem> Stones = new List<ShelfItem>();
    }

    /// <summary>What a finished ritual leaves behind: a Book of Shadows entry, a little grounding, and story variables.</summary>
    public sealed class RitualResult
    {
        public string PracticeId;
        public string MemberId;
        public BookTab Tab = BookTab.Notes;
        public string Title = "";
        public string Text = "";
        /// <summary>How much it steadied her (0..1): the story screen lowers arousal by this much.</summary>
        public float Grounding;
        /// <summary>True when the practice stirred something up (owner: "yes, sometimes"): the story screen starts a gentle depersonalization.</summary>
        public bool StirredUp;
        public Dictionary<string, string> Vars = new Dictionary<string, string>();
        /// <summary>A charm bag she made (the ritual screen puts it in her pocket).</summary>
        public CharmBagState CharmBag;
        /// <summary>She cast a circle around it.</summary>
        public bool InCircle;
        /// <summary>An alter's own practice (not Wicca): it never goes in the Book of Shadows.</summary>
        public bool Private;
    }

    /// <summary>
    /// One ritual, step by step. Pure rules (no Unity): the ritual screen shows <see cref="Current"/>, and calls
    /// <see cref="Answer"/> when the player has done it. "Not today" (<see cref="LeaveEarly"/>) is always allowed, with no
    /// penalty and nothing written. Practices built so far: tarot (one card), candle magick, grounding and centering, runes,
    /// the charm bag, cleansing a space, and casting a circle (also offered as an opening around any other ritual).
    /// </summary>
    public sealed class RitualSession
    {
        public static readonly string[] Supported =
        {
            "tarot", "candle", "grounding-centering", "runes", "charm-bag", "cleansing", "circle-casting",
            "pendulum", "tea-leaves", "oracle", "scrying", "ancestor-altar", "offering", "sigil", "dream-journal",
            "esbat", "sabbat",
        };
        /// <summary>What the home altar offers (owner: cleansing is for moving in and after the breakup: story moments only).</summary>
        public static readonly string[] AtTheAltar =
        {
            "grounding-centering", "circle-casting", "tarot", "runes", "oracle", "pendulum", "tea-leaves", "scrying",
            "candle", "charm-bag", "ancestor-altar", "offering", "sigil", "dream-journal",
        };

        /// <summary>
        /// A sigil's letters: the wish, upper case, letters only, with the vowels and every repeated letter crossed out
        /// (the first of each kept). "I am safe here" -> "MSFHR".
        /// </summary>
        public static string SigilLetters(string wish)
        {
            var seen = new HashSet<char>();
            var sb = new System.Text.StringBuilder();
            foreach (char raw in (wish ?? "").ToUpperInvariant())
            {
                if (raw < 'A' || raw > 'Z' || "AEIOU".IndexOf(raw) >= 0) continue;
                if (seen.Add(raw)) sb.Append(raw);
            }
            return sb.ToString();
        }
        public static bool IsSupported(string practiceId) => Array.IndexOf(Supported, practiceId) >= 0;

        const string WriteOwn = "write", SitWithIt = "sit";

        readonly CraftData _craft;
        readonly PracticeLibrary _library;
        readonly Random _rng;
        readonly bool _reversals;
        readonly IEnumerator<RitualStep> _steps;
        string _answer = "";
        readonly string _variant;
        CharmShelf _shelf;
        IReadOnlyList<ShelfItem> _ancestors;

        public string PracticeId { get; }
        public string MemberId { get; }
        public string Title { get; }
        public RitualStep Current { get; private set; }
        public bool IsDone { get; private set; }
        public bool LeftEarly { get; private set; }
        public RitualResult Result { get; private set; }

        /// <param name="offerCircle">Ask "Cast a circle first?" after arriving (owner: an optional opening around any ritual).</param>
        /// <param name="variant">"hard": the moment stirs something up (the story's cleansing after the breakup).</param>
        public RitualSession(string practiceId, string memberId, CraftData craft, PracticeLibrary library, int seed, bool allowReversals,
            bool offerCircle = false, string variant = "", CharmShelf shelf = null, IReadOnlyList<ShelfItem> ancestors = null)
        {
            _shelf = shelf;
            _ancestors = ancestors;
            if (!IsSupported(practiceId)) throw new ArgumentException($"No ritual for '{practiceId}' yet.");
            PracticeId = practiceId;
            MemberId = memberId ?? "maya";
            _craft = craft ?? new CraftData();
            _library = library;
            _rng = new Random(seed);
            _reversals = allowReversals;
            Title = _craft.practices?.FirstOrDefault(p => p.id == practiceId)?.name ?? practiceId;
            Result = new RitualResult { PracticeId = practiceId, MemberId = MemberId };
            _variant = variant ?? "";
            IEnumerator<RitualStep> steps = practiceId switch
            {
                "tarot" => Tarot(),
                "candle" => Candle(),
                "runes" => Runes(),
                "charm-bag" => CharmBagRitual(),
                "cleansing" => Cleansing(),
                "circle-casting" => Circle(),
                "pendulum" => Pendulum(),
                "tea-leaves" => TeaLeaves(),
                "oracle" => Oracle(),
                "scrying" => Scrying(),
                "ancestor-altar" => AncestorAltar(),
                "offering" => Offering(),
                "sigil" => Sigil(),
                "dream-journal" => DreamJournal(),
                "esbat" => Esbat(),
                "sabbat" => Sabbat(),
                _ => Grounding(),
            };
            _steps = offerCircle && practiceId != "circle-casting" && practiceId != "grounding-centering" ? WithCircle(steps) : steps;
            Advance();
        }

        /// <summary>
        /// An alter's own practice (owner: Jacob's rosary, Nina's and Lily's quiet, Heather's candle...), from its data steps.
        /// Same gentle frame: no timer, no score, "Not today" always. Never written in the Book of Shadows.
        /// </summary>
        public RitualSession(IAmAsIAm.Core.PluralSystem.OwnPractice own, string memberId, int seed = 0)
        {
            if (own == null) throw new ArgumentNullException(nameof(own));
            PracticeId = "own:" + own.id;
            MemberId = memberId ?? own.memberId;
            _craft = new CraftData();
            _rng = new Random(seed);
            _variant = "";
            Title = own.name;
            Result = new RitualResult { PracticeId = PracticeId, MemberId = MemberId, Private = true, Grounding = own.grounding, Title = own.name };
            _steps = Own(own);
            Advance();
        }

        IEnumerator<RitualStep> Own(IAmAsIAm.Core.PluralSystem.OwnPractice own)
        {
            int i = 0;
            foreach (var d in own.steps ?? new List<IAmAsIAm.Core.PluralSystem.OwnPracticeStep>())
            {
                var phase = Enum.TryParse<RitualPhase>(d.phase, true, out var ph) ? ph : RitualPhase.Moment;
                var kind = Enum.TryParse<RitualStepKind>(d.kind, true, out var k) ? k : RitualStepKind.Tap;
                var step = Step(phase, kind, d.text ?? "", string.IsNullOrEmpty(d.action) ? "Continue" : d.action, d.seconds > 0 ? d.seconds : 6f);
                if (kind == RitualStepKind.Breathe && d.seconds <= 0) step.Seconds = 8f;
                if (kind == RitualStepKind.Choose)
                {
                    var opts = d.options ?? new List<string>();
                    for (int o = 0; o < opts.Count; o++) step.Options.Add(new RitualOption(o.ToString(), opts[o]));
                    if (step.Options.Count == 0) step.Options.Add(new RitualOption("0", "Continue"));
                }
                if (kind == RitualStepKind.Write) step.MaxLength = 300;
                yield return step;
                if (kind == RitualStepKind.Choose)
                {
                    var picked = step.Options.FirstOrDefault(o => o.Id == _answer) ?? step.Options[0];
                    Result.Vars[$"own_{own.id}_{i}"] = picked.Label;
                }
                else if (kind == RitualStepKind.Write) Result.Text = _answer.Trim();
                i++;
            }
        }

        /// <summary>The player did the current step: a choice id for Choose, the text for Write, anything (or "") otherwise.</summary>
        public void Answer(string value = "")
        {
            if (IsDone) return;
            _answer = value ?? "";
            Advance();
        }

        /// <summary>"Not today": stops here. Nothing is written and nothing changes (no guilt, no penalty).</summary>
        public void LeaveEarly()
        {
            if (IsDone) return;
            LeftEarly = true;
            IsDone = true;
            Current = null;
            Result = null;
        }

        void Advance()
        {
            if (_steps.MoveNext()) Current = _steps.Current;
            else { Current = null; IsDone = true; }
        }

        static RitualStep Step(RitualPhase phase, RitualStepKind kind, string text, string action = "Continue", float seconds = 0f) =>
            new RitualStep { Phase = phase, Kind = kind, Text = text, Action = action, Seconds = seconds };

        static RitualStep Arrive(string text) => Step(RitualPhase.Arrive, RitualStepKind.Breathe, text, "Breathe", 8f);

        /// <summary>Reflect: write something, or just sit with it. Whatever's written goes in the Book.</summary>
        IEnumerable<RitualStep> Reflect(string prompt, string writeHint)
        {
            var choose = Step(RitualPhase.Reflect, RitualStepKind.Choose, prompt);
            choose.Options.Add(new RitualOption(WriteOwn, "Write it down"));
            choose.Options.Add(new RitualOption(SitWithIt, "Just sit with it"));
            yield return choose;
            if (_answer == WriteOwn)
            {
                var write = Step(RitualPhase.Reflect, RitualStepKind.Write, writeHint, "Keep it");
                write.MaxLength = BookOfShadows.MaxEntryLength;
                yield return write;
                Written = _answer.Trim();
            }
        }

        string Written = "";

        // ── Tarot: one card ─────────────────────────────────────────────────────

        IEnumerator<RitualStep> Tarot()
        {
            yield return Arrive("Sit down at the altar. One breath in. One breath out.");
            yield return Step(RitualPhase.Prepare, RitualStepKind.Hold, "Hold the deck. Shuffle until it feels done.", "Shuffle", 3f);
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Cut the deck.", "Cut");
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Draw one card.", "Draw");

            var cards = _library?.Tarot;
            TarotDraw draw;
            if (cards == null || cards.Count == 0) { yield break; }
            var deck = new TarotDeck(cards, _rng.Next()) { AllowReversals = _reversals };
            draw = deck.Draw();
            var card = draw.Card;
            bool hard = (_craft.hardCardIds ?? new List<string>()).Contains(card.id) && _rng.NextDouble() < 0.5;
            Result.StirredUp = hard;
            string name = card.name + (draw.Reversed ? " (reversed)" : "");
            string moment = $"<b>{name}</b>\n{string.Join(", ", draw.Keywords)}";
            if (!string.IsNullOrEmpty(card.gentleNote)) moment += $"\n<i>{card.gentleNote}</i>";
            if (hard) moment += "\n\nSomething in it pulls at you. Feet on the floor. You can put it down.";
            yield return Step(RitualPhase.Moment, RitualStepKind.Tap, moment, "Look at it");

            foreach (var s in Reflect(card.reflectionPrompt, "What do you see in it?")) yield return s;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Put the card back in the deck. Thank it.", "Close");

            Result.Tab = BookTab.Readings;
            Result.Title = name;
            Result.Text = Written.Length > 0 ? Written : "Sat with it.";
            Result.Vars["ritual_card"] = card.id;
        }

        // ── Candle magick ───────────────────────────────────────────────────────

        IEnumerator<RitualStep> Candle()
        {
            yield return Arrive("Clear a space on the altar. One breath in. One breath out.");
            var pick = Step(RitualPhase.Prepare, RitualStepKind.Choose, "Choose a candle.");
            foreach (var c in _craft.candleColours ?? new List<CandleColour>()) pick.Options.Add(new RitualOption(c.id, $"{c.name}: {c.meaning}"));
            if (pick.Options.Count == 0) pick.Options.Add(new RitualOption("white", "White: for anything"));
            yield return pick;
            var colour = (_craft.candleColours ?? new List<CandleColour>()).FirstOrDefault(c => c.id == _answer);
            string colourName = colour?.name ?? "White";

            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Dress it with a little oil: towards you to bring something in, away to send it off.", "Dress it");
            var carve = Step(RitualPhase.Prepare, RitualStepKind.Write, "Carve one word into the wax.", "Carve it");
            carve.MaxLength = 24;
            yield return carve;
            string word = _answer.Trim();
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Light it.", "Light it");

            string moment = "The flame catches, wobbles, and steadies." + (word.Length > 0 ? $"\n\n{colourName}. \"{word}\"." : $"\n\n{colourName}.");
            yield return Step(RitualPhase.Moment, RitualStepKind.Tap, moment, "Watch");
            yield return Step(RitualPhase.Moment, RitualStepKind.Hold, "Watch it for a while. Never leave it burning alone.", "Keep watching", 6f);

            foreach (var s in Reflect("What do you want this flame to hold?", "Write it down...")) yield return s;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Snuff it: don't blow it out. Thank the flame.", "Snuff it");

            Result.Tab = BookTab.Notes;
            Result.Title = $"A {colourName.ToLowerInvariant()} candle" + (word.Length > 0 ? $": \"{word}\"" : "");
            Result.Text = Written.Length > 0 ? Written : (colour != null ? $"{colour.name}, {colour.meaning}." : "Lit it and watched.");
            Result.Grounding = 0.1f;
            Result.Vars["ritual_colour"] = colour?.id ?? "white";
            Result.Vars["ritual_word"] = word;
        }

        // ── Grounding and centering ─────────────────────────────────────────────

        IEnumerator<RitualStep> Grounding()
        {
            yield return Arrive("Stand, or sit, with your feet flat. One breath in. One breath out.");
            yield return Step(RitualPhase.Prepare, RitualStepKind.Hold, "Breathe down, into the floor. Feel it push back.", "Breathe down", 4f);
            yield return Step(RitualPhase.Prepare, RitualStepKind.Hold, "Roots, going down. As far as they need to.", "Roots", 4f);
            yield return Step(RitualPhase.Moment, RitualStepKind.Hold, "Branches, going up. Light coming in through the leaves.", "Branches", 4f);
            yield return Step(RitualPhase.Moment, RitualStepKind.Tap, "Let the extra go, back down into the earth.", "Let it go");

            var where = Step(RitualPhase.Reflect, RitualStepKind.Choose, "Where are you, right now?");
            where.Options.Add(new RitualOption("here", "Here"));
            where.Options.Add(new RitualOption("partly", "Partly here"));
            where.Options.Add(new RitualOption("far", "Still far away"));
            yield return where;
            string here = _answer;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, here == "far"
                ? "That's all right. Feet on the floor. You can do it again, or come back later."
                : "Hands on the floor for a moment. Then up.", "Close");

            Result.Tab = BookTab.Notes;
            Result.Title = "Grounding";
            Result.Text = here == "here" ? "Here." : here == "partly" ? "Partly here." : "Still far away. Tried anyway.";
            Result.Grounding = here == "far" ? 0.15f : 0.3f;
            Result.Vars["ritual_here"] = here;
        }

        // ── Casting a circle ────────────────────────────────────────────────────

        static readonly (string dir, string element)[] Quarters =
            { ("East", "air"), ("South", "fire"), ("West", "water"), ("North", "earth") };

        IEnumerable<RitualStep> Cast()
        {
            yield return Step(RitualPhase.Prepare, RitualStepKind.Hold, "Walk the edge, sunwise. See it close behind you.", "Walk the edge", 5f);
            foreach (var q in Quarters)
                yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, $"Turn to the {q.dir.ToLowerInvariant()}. Call {q.element}.", q.dir);
        }

        IEnumerable<RitualStep> OpenAgain()
        {
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Thank the quarters: north, west, south, east.", "Thank them");
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "The circle is open, but unbroken.", "Open it");
        }

        /// <summary>The optional opening (owner): after the first breath, "Cast a circle first?"; if so, it closes after.</summary>
        IEnumerator<RitualStep> WithCircle(IEnumerator<RitualStep> inner)
        {
            if (inner.MoveNext()) yield return inner.Current; // Arrive first.
            var ask = Step(RitualPhase.Prepare, RitualStepKind.Choose, "Cast a circle first?");
            ask.Options.Add(new RitualOption("cast", "Cast a circle"));
            ask.Options.Add(new RitualOption("no", "Not this time"));
            yield return ask;
            bool cast = _answer == "cast";
            if (cast) foreach (var s in Cast()) yield return s;
            while (inner.MoveNext()) yield return inner.Current;
            if (cast)
            {
                foreach (var s in OpenAgain()) yield return s;
                Result.InCircle = true;
                Result.Vars["ritual_circle"] = "yes";
            }
        }

        IEnumerator<RitualStep> Circle()
        {
            yield return Arrive("Stand in the middle of the room. One breath in. One breath out.");
            foreach (var s in Cast()) yield return s;
            yield return Step(RitualPhase.Moment, RitualStepKind.Tap, "Inside, it's quiet. Nothing has to happen in here.", "Sit down");
            yield return Step(RitualPhase.Moment, RitualStepKind.Hold, "Sit inside it for as long as you like.", "Sit", 8f);
            foreach (var s in Reflect("What's it like, in here?", "Write it down...")) yield return s;
            foreach (var s in OpenAgain()) yield return s;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Merry meet, and merry part, and merry meet again.", "Close");

            Result.Tab = BookTab.Notes;
            Result.Title = "A circle";
            Result.Text = Written.Length > 0 ? Written : "Sat inside it a while.";
            Result.Grounding = 0.2f;
            Result.InCircle = true;
        }

        // ── Runes ───────────────────────────────────────────────────────────────

        static readonly string[] ThreePositions = { "What was", "What is", "What may come" };

        static string Meaning(RuneEntry r)
        {
            var m = r.modernAssociations ?? new List<string>();
            return m.Count > 0 ? string.Join(", ", m.Take(2)) : r.literalMeaning ?? "";
        }

        IEnumerator<RitualStep> Runes()
        {
            yield return Arrive("Lay the cloth on the altar. One breath in. One breath out.");
            // Owner: the player chooses one rune, three, or a cast on the cloth.
            var how = Step(RitualPhase.Prepare, RitualStepKind.Choose, "How would you like to ask?");
            how.Options.Add(new RitualOption("one", "One rune"));
            how.Options.Add(new RitualOption("three", "Three runes: what was, what is, what may come"));
            how.Options.Add(new RitualOption("cast", "Cast them on the cloth"));
            yield return how;
            string mode = _answer == "three" || _answer == "cast" ? _answer : "one";

            yield return Step(RitualPhase.Prepare, RitualStepKind.Hold, "Hold the bag. Think of the question.", "Hold the bag", 3f);
            yield return mode == "cast"
                ? Step(RitualPhase.Prepare, RitualStepKind.Tap, "Shake the bag, and cast a handful onto the cloth.", "Cast")
                : Step(RitualPhase.Prepare, RitualStepKind.Tap, mode == "three" ? "Draw three, one at a time, and lay them left to right." : "Reach in. Draw one.", "Draw");

            var all = _library?.Runes?.ToList() ?? new List<RuneEntry>();
            if (all.Count == 0) yield break;
            var bag = all.OrderBy(_ => _rng.Next()).ToList();
            List<RuneEntry> drawn;
            string intro;
            if (mode == "three") { drawn = bag.Take(3).ToList(); intro = ""; }
            else if (mode == "cast")
            {
                // Seven from the handful land on the cloth; she reads only the ones face up (at least one).
                var landed = bag.Take(7).ToList();
                drawn = landed.Where(_ => _rng.NextDouble() < 0.4).ToList();
                if (drawn.Count == 0) drawn.Add(landed[0]);
                intro = $"{landed.Count} land on the cloth. {(drawn.Count == 1 ? "One is" : $"{drawn.Count} are")} face up.";
            }
            else { drawn = bag.Take(1).ToList(); intro = ""; }

            var hardIds = _craft.hardRuneIds ?? new List<string>();
            bool hard = drawn.Any(r => hardIds.Contains(r.id)) && _rng.NextDouble() < 0.5;
            Result.StirredUp = hard;

            var moment = Step(RitualPhase.Moment, RitualStepKind.Tap, intro, "Look at them");
            for (int i = 0; i < drawn.Count; i++)
            {
                var r = drawn[i];
                string caption = mode == "three" ? $"<i>{ThreePositions[i]}</i>\n{Meaning(r)}" : Meaning(r);
                moment.Tokens.Add(new RitualToken { Glyph = r.glyph, Name = r.name, Caption = caption });
            }
            if (drawn.Count == 1) moment.Text = (intro.Length > 0 ? intro + "\n\n" : "") + $"<i>{drawn[0].reflectionPrompt}</i>";
            if (hard) moment.Text += "\n\nSomething in it pulls at you. Feet on the floor. You can put them down.";
            if (drawn.Count == 1 && mode == "one") moment.Action = "Look at it";
            yield return moment;

            foreach (var s in Reflect(drawn.Count == 1 ? drawn[0].reflectionPrompt : "What do they say together?", "What do you see in them?")) yield return s;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Put them back in the bag. Thank them.", "Close");

            Result.Tab = BookTab.Readings;
            Result.Title = (mode == "three" ? "Three runes: " : mode == "cast" ? "A cast: " : "A rune: ") + string.Join(", ", drawn.Select(r => r.name));
            string listed = string.Join("\n", drawn.Select((r, i) => (mode == "three" ? ThreePositions[i] + ": " : "") + $"{r.name} ({Meaning(r)})"));
            Result.Text = listed + "\n\n" + (Written.Length > 0 ? Written : "Sat with it.");
            Result.Vars["ritual_runes"] = string.Join(",", drawn.Select(r => r.id));
            Result.Vars["rune_pulled"] = drawn[0].id;
        }

        // ── The charm bag ───────────────────────────────────────────────────────

        IEnumerator<RitualStep> CharmBagRitual()
        {
            yield return Arrive("Lay it all out on the altar. One breath in. One breath out.");

            // Owner: only what she's been given (or found), each with what it's for and who it came from.
            var herbs = (_library?.Herbs ?? new List<HerbEntry>()).ToDictionary(h => h.id);
            var stones = (_library?.Crystals ?? new List<CrystalEntry>()).ToDictionary(c => c.id);
            var herbIds = _craft.charmHerbIds ?? new List<string>();
            var stoneIds = _craft.charmStoneIds ?? new List<string>();
            var herbShelf = _shelf?.Herbs ?? herbs.Keys.Where(id => herbIds.Count == 0 || herbIds.Contains(id)).Select(id => new ShelfItem(id)).ToList();
            var stoneShelf = _shelf?.Stones ?? stones.Keys.Where(id => stoneIds.Count == 0 || stoneIds.Contains(id)).Select(id => new ShelfItem(id)).ToList();

            static string Label(string name, string meaning, string from) =>
                name + (string.IsNullOrEmpty(meaning) ? "" : $": {meaning}") + (string.IsNullOrEmpty(from) ? "" : $"  <i>(from {from})</i>");

            var pickHerb = Step(RitualPhase.Prepare, RitualStepKind.Choose, "A pinch of something dried.");
            foreach (var it in herbShelf)
                if (herbs.TryGetValue(it.Id, out var h)) pickHerb.Options.Add(new RitualOption(h.id, Label(h.commonName, h.charmMeaning, it.From)));
            if (pickHerb.Options.Count == 0) pickHerb.Options.Add(new RitualOption("rosemary", "Rosemary"));
            yield return pickHerb;
            string herbId = pickHerb.Options.Any(o => o.Id == _answer) ? _answer : pickHerb.Options[0].Id;
            string herb = herbs.TryGetValue(herbId, out var hh) ? hh.commonName : "Rosemary";

            var pickStone = Step(RitualPhase.Prepare, RitualStepKind.Choose, "A small stone.");
            foreach (var it in stoneShelf)
                if (stones.TryGetValue(it.Id, out var c)) pickStone.Options.Add(new RitualOption(c.id, Label(c.name, c.charmMeaning, it.From)));
            if (pickStone.Options.Count == 0) pickStone.Options.Add(new RitualOption("clear-quartz", "Clear quartz"));
            yield return pickStone;
            string stoneId = pickStone.Options.Any(o => o.Id == _answer) ? _answer : pickStone.Options[0].Id;
            string stone = stones.TryGetValue(stoneId, out var cc) ? cc.name : "Clear quartz";
            string herbMeaning = hh?.charmMeaning ?? "", stoneMeaning = cc?.charmMeaning ?? "";

            var pickOwn = Step(RitualPhase.Prepare, RitualStepKind.Choose, "Something of yours.");
            var own = _craft.charmOwnThings ?? new List<string>();
            for (int i = 0; i < own.Count; i++) pickOwn.Options.Add(new RitualOption(i.ToString(), own[i]));
            if (pickOwn.Options.Count == 0) pickOwn.Options.Add(new RitualOption("0", "a thread from your sleeve"));
            yield return pickOwn;
            string mine = pickOwn.Options.FirstOrDefault(o => o.Id == _answer)?.Label ?? pickOwn.Options[0].Label;

            var intent = Step(RitualPhase.Prepare, RitualStepKind.Write, "Write what it's for, small, on a scrap of paper. Fold it in.", "Fold it in");
            intent.MaxLength = 60;
            yield return intent;
            string what = _answer.Trim();

            yield return Step(RitualPhase.Prepare, RitualStepKind.Hold, "Tie it closed with three knots.", "Tie the knots", 4f);
            yield return Step(RitualPhase.Moment, RitualStepKind.Tap,
                $"{herb}, {stone}, {mine}" + (what.Length > 0 ? $", and \"{what}\"." : ".") + "\n\nIt's small enough to close a hand around.", "Hold it");
            foreach (var st in Reflect("When you hold it, what do you want to remember?", "Write it down...")) yield return st;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Put it in your pocket. Carry it.", "Carry it");

            Result.Tab = BookTab.Notes;
            Result.Title = "A charm bag" + (what.Length > 0 ? $": \"{what}\"" : "");
            Result.Text = $"{herb}" + (herbMeaning.Length > 0 ? $" ({herbMeaning})" : "") + $", {stone}" + (stoneMeaning.Length > 0 ? $" ({stoneMeaning})" : "")
                + $", {mine}. Three knots." + (Written.Length > 0 ? "\n\n" + Written : "");
            Result.Grounding = 0.1f;
            Result.CharmBag = new CharmBagState { has = true, carried = true, herb = herb, stone = stone, own = mine, intent = what };
            Result.Vars["charm_herb"] = herbId;
            Result.Vars["charm_stone"] = stoneId;
            Result.Vars["charm_intent"] = what;
        }

        // ── Cleansing a space ───────────────────────────────────────────────────

        IEnumerator<RitualStep> Cleansing()
        {
            yield return Arrive("Stand in the doorway. One breath in. One breath out.");
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Open a window. Let the old air out.", "Open it");
            var with = Step(RitualPhase.Prepare, RitualStepKind.Choose, "What will you use?");
            with.Options.Add(new RitualOption("rosemary", "A bundle of rosemary and bay, smouldering"));
            with.Options.Add(new RitualOption("salt", "Salt water, a few drops in each corner"));
            with.Options.Add(new RitualOption("bell", "The bell, rung in each room"));
            yield return with;
            string tool = _answer == "salt" || _answer == "bell" ? _answer : "rosemary";
            string how = tool == "salt" ? "Salt water in the corners" : tool == "bell" ? "The bell" : "Rosemary and bay";
            yield return Step(RitualPhase.Moment, RitualStepKind.Hold, "Room by room, slowly, sunwise. Into the corners.", "Room by room", 6f);
            if (_variant == "hard")
            {
                Result.StirredUp = true;
                yield return Step(RitualPhase.Moment, RitualStepKind.Tap,
                    "One room keeps hold of something. You can leave that corner for today.", "Leave it for today");
            }
            var holds = Step(RitualPhase.Reflect, RitualStepKind.Write, "Say what you want this place to hold now.", "Say it");
            holds.MaxLength = 120;
            yield return holds;
            string said = _answer.Trim();
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Close the window. It's yours.", "Close it");

            Result.Tab = BookTab.Notes;
            Result.Title = "Cleansing";
            Result.Text = how + "." + (said.Length > 0 ? $" \"{said}\"" : "");
            Result.Grounding = _variant == "hard" ? 0.1f : 0.2f;
            Result.Vars["ritual_cleansing"] = tool;
            Result.Vars["ritual_holds"] = said;
        }

        // ── Pendulum ────────────────────────────────────────────────────────────

        IEnumerator<RitualStep> Pendulum()
        {
            yield return Arrive("Sit at the altar. One breath in. One breath out.");
            yield return Step(RitualPhase.Prepare, RitualStepKind.Hold, "Elbow on the table. Let it go still.", "Hold it still", 3f);
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Ask it to show you yes. It swings, front to back.", "Show me yes");
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Ask it to show you no. Side to side.", "Show me no");
            var ask = Step(RitualPhase.Prepare, RitualStepKind.Write, "Ask one small question. A yes-or-no one.", "Ask it");
            ask.MaxLength = 80;
            yield return ask;
            string question = _answer.Trim();
            yield return Step(RitualPhase.Moment, RitualStepKind.Hold, "Watch it.", "Watch", 4f);
            double r = _rng.NextDouble();
            string swing = r < 0.4 ? "yes" : r < 0.8 ? "no" : "unclear";
            yield return Step(RitualPhase.Moment, RitualStepKind.Tap, swing == "yes" ? "Front to back. Yes."
                : swing == "no" ? "Side to side. No." : "It circles. It won't say.", "See it");
            // Tanya's real lesson with a pendulum: notice what you hoped for.
            foreach (var st in Reflect("What did you hope it would say?", "Write it down...")) yield return st;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Let it go still again. Put it away. Thank it.", "Close");

            Result.Tab = BookTab.Readings;
            Result.Title = "The pendulum" + (question.Length > 0 ? $": \"{question}\"" : "");
            Result.Text = (swing == "yes" ? "Yes." : swing == "no" ? "No." : "It circled.") + (Written.Length > 0 ? "\n\n" + Written : "");
            Result.Vars["ritual_pendulum"] = swing;
        }

        // ── Tea leaves ──────────────────────────────────────────────────────────

        IEnumerator<RitualStep> TeaLeaves()
        {
            yield return Arrive("Put the kettle on. One breath in. One breath out.");
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Loose tea, no strainer. Drink it slowly.", "Drink");
            yield return Step(RitualPhase.Prepare, RitualStepKind.Hold, "Leave a little at the bottom. Swirl it three times.", "Swirl", 3f);
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Turn the cup over on the saucer. Wait. Turn it back.", "Turn it");
            var shapes = (_craft.teaLeafShapes ?? new List<TeaLeafShape>()).ToList();
            if (shapes.Count == 0) shapes.Add(new TeaLeafShape { shape = "a bird, flying", meaning = "news, coming quickly" });
            var picked = shapes.OrderBy(_ => _rng.Next()).Take(Math.Min(2, shapes.Count)).ToList();
            string near = $"Near the rim, <b>{picked[0].shape}</b>: <i>{picked[0].meaning}</i>. Soon.";
            string far = picked.Count > 1 ? $"\n\nDown at the bottom, <b>{picked[1].shape}</b>: <i>{picked[1].meaning}</i>. Further off." : "";
            yield return Step(RitualPhase.Moment, RitualStepKind.Tap, "Look for shapes, like clouds.\n\n" + near + far, "Look");
            foreach (var st in Reflect("What do YOU see in it?", "Write it down...")) yield return st;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Rinse the cup.", "Close");

            Result.Tab = BookTab.Readings;
            Result.Title = "Tea leaves: " + string.Join(", ", picked.Select(x => x.shape));
            Result.Text = string.Join("\n", picked.Select(x => $"{x.shape}: {x.meaning}")) + (Written.Length > 0 ? "\n\n" + Written : "");
            Result.Vars["ritual_tea"] = string.Join(",", picked.Select(x => x.shape));
        }

        // ── Oracle cards ────────────────────────────────────────────────────────

        IEnumerator<RitualStep> Oracle()
        {
            yield return Arrive("Selene's deck, in its cloth. One breath in. One breath out.");
            yield return Step(RitualPhase.Prepare, RitualStepKind.Hold, "Shuffle until it feels done.", "Shuffle", 3f);
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "One card, any question.", "Draw");
            var cards = (_craft.oracleCards ?? new List<OracleCard>()).ToList();
            if (cards.Count == 0) cards.Add(new OracleCard { word = "Here", prompt = "Five things you can see, right now." });
            var card = cards[_rng.Next(cards.Count)];
            yield return Step(RitualPhase.Moment, RitualStepKind.Tap, $"<size=90><b>{card.word}</b></size>\n\n<i>{card.prompt}</i>", "Sit with it");
            foreach (var st in Reflect("One word. Sit with it.", "Write it down...")) yield return st;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Back in the deck. Back in the cloth.", "Close");

            Result.Tab = BookTab.Readings;
            Result.Title = "Oracle: " + card.word;
            Result.Text = Written.Length > 0 ? Written : card.prompt;
            Result.Vars["ritual_oracle"] = card.word;
        }

        // ── Scrying (owner: Tanya warns her off until she's steadier, after Chapter 3) ──

        IEnumerator<RitualStep> Scrying()
        {
            yield return Arrive("Lights low, not off. One breath in. One breath out.");
            // Grounding first, always (Tanya; Angie): scrying is soft eyes, and soft eyes can drift.
            yield return Step(RitualPhase.Prepare, RitualStepKind.Hold, "Ground first. Feet flat. Roots, going down.", "Roots", 4f);
            var what = Step(RitualPhase.Prepare, RitualStepKind.Choose, "What will you look into?");
            what.Options.Add(new RitualOption("bowl", "A dark bowl of water"));
            what.Options.Add(new RitualOption("flame", "One candle flame"));
            yield return what;
            bool flame = _answer == "flame";
            yield return Step(RitualPhase.Moment, RitualStepKind.Hold, "Soft eyes. Don't stare. Eyes open, feet on the floor.", "Look", 6f);
            var came = Step(RitualPhase.Moment, RitualStepKind.Choose, "Did anything come?");
            came.Options.Add(new RitualOption("shape", "A shape"));
            came.Options.Add(new RitualOption("colour", "A colour"));
            came.Options.Add(new RitualOption("nothing", "Nothing, and that's fine"));
            came.Options.Add(new RitualOption("drift", "I started to drift"));
            yield return came;
            string seen = _answer;
            if (seen == "drift" || _rng.NextDouble() < 0.2)
            {
                Result.StirredUp = true;
                yield return Step(RitualPhase.Moment, RitualStepKind.Tap,
                    "Stop. Look away. Lights up. Name three things in the room. You can stop here.", "Three things");
            }
            foreach (var st in Reflect("What was there?", "Write it down...")) yield return st;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, flame ? "Snuff the flame. Lights on." : "Pour the water out, onto the earth. Lights on.", "Close");

            Result.Tab = BookTab.Readings;
            Result.Title = flame ? "Scrying: a flame" : "Scrying: dark water";
            Result.Text = (seen == "shape" ? "A shape." : seen == "colour" ? "A colour." : seen == "drift" ? "Started to drift. Stopped." : "Nothing. That's fine.")
                + (Written.Length > 0 ? "\n\n" + Written : "");
            Result.Vars["ritual_scrying"] = seen;
        }

        // ── The ancestor altar ──────────────────────────────────────────────────

        IEnumerator<RitualStep> AncestorAltar()
        {
            yield return Arrive("The photos, the little things. One breath in. One breath out.");
            var who = Step(RitualPhase.Prepare, RitualStepKind.Choose, "Who are you sitting with tonight?");
            foreach (var a in _ancestors ?? new List<ShelfItem>()) who.Options.Add(new RitualOption(a.Id, string.IsNullOrEmpty(a.From) ? a.Id : a.From));
            if (who.Options.Count == 0) who.Options.Add(new RitualOption("", "Someone you've lost"));
            yield return who;
            var chosen = who.Options.FirstOrDefault(o => o.Id == _answer) ?? who.Options[0];
            string name = chosen.Label;
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Light a candle for them. Water beside it. Something they liked.", "Light it");
            bool hard = _rng.NextDouble() < 0.3;
            Result.StirredUp = hard;
            yield return Step(RitualPhase.Moment, RitualStepKind.Tap, $"Say their name.\n\n<b>{name}</b>"
                + (hard ? "\n\nIt's heavier tonight. Feet on the floor. You can stop here." : ""), "Say it");
            var tell = Step(RitualPhase.Reflect, RitualStepKind.Write, "Tell them about your day.", "Tell them");
            tell.MaxLength = BookOfShadows.MaxEntryLength;
            yield return tell;
            string said = _answer.Trim();
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Thank them. Let the candle burn a little longer, then snuff it.", "Thank them");

            Result.Tab = BookTab.Notes;
            Result.Title = "For " + name;
            Result.Text = said.Length > 0 ? said : "Sat with them a while.";
            Result.Grounding = 0.1f;
            Result.Vars["ritual_ancestor"] = chosen.Id;
        }

        // ── Offerings ───────────────────────────────────────────────────────────

        IEnumerator<RitualStep> Offering()
        {
            yield return Arrive("A small dish on the altar. One breath in. One breath out.");
            var what = Step(RitualPhase.Prepare, RitualStepKind.Choose, "What will you give?");
            foreach (var o in _craft.offeringThings ?? new List<CraftOption>()) what.Options.Add(new RitualOption(o.id, o.label));
            if (what.Options.Count == 0) what.Options.Add(new RitualOption("bread", "A piece of bread"));
            yield return what;
            var thing = what.Options.FirstOrDefault(o => o.Id == _answer) ?? what.Options[0];
            var forWhom = Step(RitualPhase.Prepare, RitualStepKind.Choose, "Who is it for?");
            foreach (var o in _craft.offeringFor ?? new List<CraftOption>()) forWhom.Options.Add(new RitualOption(o.id, o.label));
            foreach (var a in _ancestors ?? new List<ShelfItem>()) forWhom.Options.Add(new RitualOption("ancestor:" + a.Id, string.IsNullOrEmpty(a.From) ? a.Id : a.From));
            if (forWhom.Options.Count == 0) forWhom.Options.Add(new RitualOption("season", "The season"));
            yield return forWhom;
            var to = forWhom.Options.FirstOrDefault(o => o.Id == _answer) ?? forWhom.Options[0];
            yield return Step(RitualPhase.Moment, RitualStepKind.Tap, thing.Id == "song"
                ? $"Hum it, quietly. For {to.Label}."
                : $"Set it down. Say who it's for: {to.Label}.", "Give it");
            foreach (var st in Reflect("What are you giving thanks for?", "Write it down...")) yield return st;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, thing.Id == "song" ? "Let the quiet come back." : "Leave it. Give it back to the earth tomorrow.", "Close");

            Result.Tab = BookTab.Notes;
            Result.Title = $"An offering: {thing.Label.ToLowerInvariant()}, for {to.Label}";
            Result.Text = Written.Length > 0 ? Written : "Given.";
            Result.Grounding = 0.1f;
            Result.Vars["ritual_offering"] = thing.Id;
            Result.Vars["ritual_offering_for"] = to.Id;
        }

        // ── Sigils ──────────────────────────────────────────────────────────────

        IEnumerator<RitualStep> Sigil()
        {
            yield return Arrive("A clean page and a pencil. One breath in. One breath out.");
            var wish = Step(RitualPhase.Prepare, RitualStepKind.Write, "Write what you want, as if it's already true.", "Write it");
            wish.MaxLength = 60;
            yield return wish;
            string text = _answer.Trim();
            string letters = SigilLetters(text);
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, letters.Length > 0
                ? $"Cross out the vowels, and every letter that repeats.\n\n<size=80><b>{string.Join(" ", letters.ToCharArray())}</b></size>"
                : "Nothing left to cross out. That's all right. Draw a circle instead.", "Cross them out");
            yield return Step(RitualPhase.Moment, RitualStepKind.Hold, "Make one shape from what's left. Draw it until it isn't words anymore.", "Draw", 6f);
            foreach (var st in Reflect("Where will you keep it?", "Write it down...")) yield return st;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Fold it into the Book. Now let it go: stop thinking about the words.", "Let it go");

            Result.Tab = BookTab.Notes;
            Result.Title = "A sigil";
            Result.Text = (text.Length > 0 ? $"\"{text}\"  ({letters})" : "A circle.") + (Written.Length > 0 ? "\n\n" + Written : "");
            Result.Vars["ritual_sigil"] = letters;
        }

        // ── The dream journal ───────────────────────────────────────────────────

        IEnumerator<RitualStep> DreamJournal()
        {
            yield return Arrive("Morning. Don't move yet. One breath in. One breath out.");
            yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Reach for the book, not the phone.", "The book");
            yield return Step(RitualPhase.Moment, RitualStepKind.Tap, "Close your eyes again for a moment. What's left of it?", "Look");
            var any = Step(RitualPhase.Reflect, RitualStepKind.Choose, "Do you remember anything?");
            any.Options.Add(new RitualOption("something", "Something"));
            any.Options.Add(new RitualOption("feeling", "Only a feeling"));
            any.Options.Add(new RitualOption("nothing", "Nothing"));
            yield return any;
            string kept = _answer;
            string dream = "";
            if (kept != "nothing")
            {
                var w = Step(RitualPhase.Reflect, RitualStepKind.Write, "Write it down. Even one word.", "Keep it");
                w.MaxLength = BookOfShadows.MaxEntryLength;
                yield return w;
                dream = _answer.Trim();
            }
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Close the book. Now the phone.", "Close");

            Result.Tab = BookTab.Dreams;
            Result.Title = kept == "feeling" ? "A feeling" : kept == "nothing" ? "" : "A dream";
            Result.Text = dream.Length > 0 ? dream : kept == "nothing" ? "Nothing I remember." : "Only a feeling.";
            Result.Vars["ritual_dream"] = kept;
        }

        // ── The moon (esbat): variant "full" or "new" ─────────────────────────

        IEnumerator<RitualStep> Esbat()
        {
            bool full = _variant != "new";
            if (full)
            {
                yield return Arrive("The full moon, through the window. One breath in. One breath out.");
                yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Fill a jar with water. Set it on the windowsill, in the moonlight.", "Set it there");
                yield return Step(RitualPhase.Moment, RitualStepKind.Hold, "Look at the moon for a while.", "Look", 5f);
                yield return Step(RitualPhase.Moment, RitualStepKind.Tap, "Say thank you for something from this month. Out loud, or not.", "Thank you");
                foreach (var st in Reflect("What are you grateful for, this month?", "Write it down...")) yield return st;
                yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Leave the water there overnight. Tomorrow, water a plant with it.", "Close");
            }
            else
            {
                yield return Arrive("No moon tonight. One breath in. One breath out.");
                yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "No candle, or just one.", "Just one");
                yield return Step(RitualPhase.Moment, RitualStepKind.Hold, "Rest. Breathe out the whole month.", "Rest", 6f);
                yield return Step(RitualPhase.Moment, RitualStepKind.Tap, "Now think of one small thing to begin.", "Begin");
                foreach (var st in Reflect("What small thing will you start?", "Write it down...")) yield return st;
                yield return Step(RitualPhase.Close, RitualStepKind.Tap, "Snuff the candle. Go to bed early.", "Close");
            }
            Result.Tab = BookTab.Wheel;
            Result.Title = full ? "Full moon" : "New moon";
            Result.Text = Written.Length > 0 ? Written : full ? "Water on the windowsill. Said thank you." : "Rested. Began something small.";
            Result.Grounding = 0.15f;
            Result.Vars["ritual_esbat"] = full ? "full" : "new";
        }

        // ── A sabbat at home: variant = the sabbat's id (craft.json sabbats) ──

        IEnumerator<RitualStep> Sabbat()
        {
            var s = (_craft.sabbats ?? new List<SabbatDef>()).FirstOrDefault(x => x.id == _variant)
                    ?? new SabbatDef { id = _variant, name = "The season", arrive = "One breath in. One breath out.", moment = "The wheel turns.", reflect = "What is this season asking of you?", close = "Thank the season." };
            yield return Arrive(string.IsNullOrEmpty(s.arrive) ? "One breath in. One breath out." : s.arrive);
            foreach (var p in s.prepare ?? new List<string>()) yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, p, "Do it");
            if ((s.prepare ?? new List<string>()).Count == 0) yield return Step(RitualPhase.Prepare, RitualStepKind.Tap, "Clear a space on the altar.", "Clear it");
            yield return Step(RitualPhase.Moment, RitualStepKind.Hold, s.moment ?? "The wheel turns.", "Stay", 5f);
            // Owner: some sabbats are hard, sometimes (Samhain: Mira; Yule: the night Izzy first came forward).
            if (s.hardChance > 0f && _rng.NextDouble() < s.hardChance)
            {
                Result.StirredUp = true;
                yield return Step(RitualPhase.Moment, RitualStepKind.Tap,
                    string.IsNullOrEmpty(s.hardLine) ? "It's heavy tonight. Feet on the floor. You can stop here." : s.hardLine, "Feet on the floor");
            }
            foreach (var st in Reflect(s.reflect ?? "What is this season asking of you?", "Write it down...")) yield return st;
            yield return Step(RitualPhase.Close, RitualStepKind.Tap, s.close ?? "Thank the season.", "Close");

            Result.Tab = BookTab.Wheel;
            Result.Title = s.name;
            Result.Text = Written.Length > 0 ? Written : "Kept it.";
            Result.Grounding = 0.15f;
            Result.Vars["ritual_sabbat"] = s.id;
        }
    }
}
