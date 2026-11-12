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
    }

    /// <summary>
    /// One ritual, step by step. Pure rules (no Unity): the ritual screen shows <see cref="Current"/>, and calls
    /// <see cref="Answer"/> when the player has done it. "Not today" (<see cref="LeaveEarly"/>) is always allowed, with no
    /// penalty and nothing written. Practices built so far: tarot (one card), candle magick, grounding and centering, runes,
    /// the charm bag, cleansing a space, and casting a circle (also offered as an opening around any other ritual).
    /// </summary>
    public sealed class RitualSession
    {
        public static readonly string[] Supported = { "tarot", "candle", "grounding-centering", "runes", "charm-bag", "cleansing", "circle-casting" };
        /// <summary>What the home altar offers (owner: cleansing is for moving in and after the breakup: story moments only).</summary>
        public static readonly string[] AtTheAltar = { "grounding-centering", "circle-casting", "tarot", "runes", "candle", "charm-bag" };
        public static bool IsSupported(string practiceId) => Array.IndexOf(Supported, practiceId) >= 0;

        const string WriteOwn = "write", SitWithIt = "sit";

        readonly CraftData _craft;
        readonly PracticeLibrary _library;
        readonly Random _rng;
        readonly bool _reversals;
        readonly IEnumerator<RitualStep> _steps;
        string _answer = "";
        readonly string _variant;

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
            bool offerCircle = false, string variant = "")
        {
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
                _ => Grounding(),
            };
            _steps = offerCircle && practiceId != "circle-casting" && practiceId != "grounding-centering" ? WithCircle(steps) : steps;
            Advance();
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

            var herbIds = _craft.charmHerbIds ?? new List<string>();
            var herbs = (_library?.Herbs ?? new List<HerbEntry>()).Where(h => herbIds.Count == 0 || herbIds.Contains(h.id)).ToList();
            var pickHerb = Step(RitualPhase.Prepare, RitualStepKind.Choose, "A pinch of something dried.");
            foreach (var h in herbs) pickHerb.Options.Add(new RitualOption(h.id, h.commonName));
            if (pickHerb.Options.Count == 0) pickHerb.Options.Add(new RitualOption("rosemary", "Rosemary"));
            yield return pickHerb;
            string herb = pickHerb.Options.FirstOrDefault(o => o.Id == _answer)?.Label ?? pickHerb.Options[0].Label;

            var stoneIds = _craft.charmStoneIds ?? new List<string>();
            var stones = (_library?.Crystals ?? new List<CrystalEntry>()).Where(c => stoneIds.Count == 0 || stoneIds.Contains(c.id)).ToList();
            var pickStone = Step(RitualPhase.Prepare, RitualStepKind.Choose, "A small stone.");
            foreach (var c in stones) pickStone.Options.Add(new RitualOption(c.id, c.name));
            if (pickStone.Options.Count == 0) pickStone.Options.Add(new RitualOption("clear-quartz", "Clear quartz"));
            yield return pickStone;
            string stone = pickStone.Options.FirstOrDefault(o => o.Id == _answer)?.Label ?? pickStone.Options[0].Label;

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
            Result.Text = $"{herb}, {stone}, {mine}. Three knots." + (Written.Length > 0 ? "\n\n" + Written : "");
            Result.Grounding = 0.1f;
            Result.CharmBag = new CharmBagState { has = true, carried = true, herb = herb, stone = stone, own = mine, intent = what };
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
    }
}
