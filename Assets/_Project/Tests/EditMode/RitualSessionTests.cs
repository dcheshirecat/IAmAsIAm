using System.Collections.Generic;
using IAmAsIAm.Core.Practices;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    public sealed class RitualSessionTests
    {
        static CraftData Craft() => new CraftData
        {
            practices = new List<CraftPractice>
            {
                new CraftPractice { id = "tarot", name = "Tarot", steps = new List<string> { "Shuffle." } },
                new CraftPractice { id = "candle", name = "Candle magick", steps = new List<string> { "Light it." } },
                new CraftPractice { id = "grounding-centering", name = "Grounding and centering", steps = new List<string> { "Roots." } },
            },
            candleColours = new List<CandleColour> { new CandleColour { id = "green", name = "Green", meaning = "for growth" } },
            hardCardIds = new List<string> { "major-16-tower" },
        };

        static RuneData Runes()
        {
            var d = new RuneData();
            foreach (var (id, name) in new[] { ("fehu", "Fehu"), ("uruz", "Uruz"), ("hagalaz", "Hagalaz"), ("isa", "Isaz"), ("jera", "Jera"), ("sowilo", "Sowilo"), ("berkano", "Berkano"), ("dagaz", "Dagaz") })
                d.runes.Add(new RuneEntry { id = id, name = name, glyph = "x", literalMeaning = "a meaning", modernAssociations = new List<string> { "one", "two", "three" }, reflectionPrompt = "What do you notice?" });
            return d;
        }

        static PracticeLibrary Library() => new PracticeLibrary(Runes(),
            new TarotData { cards = new List<TarotCardEntry> { new TarotCardEntry { id = "major-17-star", name = "The Star", uprightKeywords = new List<string> { "hope" }, reflectionPrompt = "What gives you hope?" } } },
            new HerbData(), new CrystalData(), new CharmData());

        /// <summary>Does every step with a sensible answer until the ritual ends.</summary>
        static void Walk(RitualSession s, string choice = null, string write = "")
        {
            int guard = 0;
            while (!s.IsDone && guard++ < 50)
            {
                var step = s.Current;
                if (step.Kind == RitualStepKind.Choose) s.Answer(choice != null && step.Options.Exists(o => o.Id == choice) ? choice : step.Options[0].Id);
                else if (step.Kind == RitualStepKind.Write) s.Answer(write);
                else s.Answer();
            }
            Assert.That(s.IsDone, Is.True, "a ritual always ends");
        }

        [Test]
        public void EveryRitual_GoesArrivePrepareMomentReflectClose()
        {
            foreach (var id in RitualSession.Supported)
            {
                var s = new RitualSession(id, "maya", Craft(), Library(), 1, false);
                Assert.That(s.Current.Phase, Is.EqualTo(RitualPhase.Arrive), id);
                var phases = new List<RitualPhase>();
                int guard = 0;
                while (!s.IsDone && guard++ < 50)
                {
                    if (phases.Count == 0 || phases[phases.Count - 1] != s.Current.Phase) phases.Add(s.Current.Phase);
                    s.Answer(s.Current.Kind == RitualStepKind.Choose ? s.Current.Options[s.Current.Options.Count - 1].Id : "");
                }
                Assert.That(phases, Is.EqualTo(new[] { RitualPhase.Arrive, RitualPhase.Prepare, RitualPhase.Moment, RitualPhase.Reflect, RitualPhase.Close }), id);
                Assert.That(s.Result, Is.Not.Null, id);
                Assert.That(s.Result.MemberId, Is.EqualTo("maya"));
            }
        }

        [Test]
        public void Tarot_WritesTheCardAndWhatWasWrittenToReadings()
        {
            var s = new RitualSession("tarot", "izzy", Craft(), Library(), 3, false);
            Walk(s, "write", "  Hope, after all.  ");
            Assert.That(s.Result.Tab, Is.EqualTo(BookTab.Readings));
            Assert.That(s.Result.Title, Is.EqualTo("The Star"));
            Assert.That(s.Result.Text, Is.EqualTo("Hope, after all."));
            Assert.That(s.Result.Vars["ritual_card"], Is.EqualTo("major-17-star"));
            Assert.That(s.Result.StirredUp, Is.False, "the Star isn't a hard card");
        }

        [Test]
        public void Candle_KeepsTheColourAndTheCarvedWord()
        {
            var s = new RitualSession("candle", "maya", Craft(), Library(), 1, false);
            int guard = 0;
            while (!s.IsDone && guard++ < 50)
            {
                var step = s.Current;
                if (step.Kind == RitualStepKind.Choose) s.Answer(step.Phase == RitualPhase.Prepare ? "green" : "sit");
                else if (step.Kind == RitualStepKind.Write) { Assert.That(step.MaxLength, Is.EqualTo(24)); s.Answer("home"); }
                else s.Answer();
            }
            Assert.That(s.Result.Vars["ritual_colour"], Is.EqualTo("green"));
            Assert.That(s.Result.Vars["ritual_word"], Is.EqualTo("home"));
            Assert.That(s.Result.Title, Does.Contain("green"));
            Assert.That(s.Result.Grounding, Is.GreaterThan(0f));
        }

        [Test]
        public void Grounding_SteadiesMoreWhenSheIsHere()
        {
            var here = new RitualSession("grounding-centering", "maya", Craft(), Library(), 1, false);
            Walk(here, "here");
            var far = new RitualSession("grounding-centering", "maya", Craft(), Library(), 1, false);
            Walk(far, "far");
            Assert.That(here.Result.Grounding, Is.GreaterThan(far.Result.Grounding));
            Assert.That(far.Result.Grounding, Is.GreaterThan(0f), "trying still counts");
        }

        [Test]
        public void NotToday_StopsWithNothingWritten()
        {
            var s = new RitualSession("candle", "maya", Craft(), Library(), 1, false);
            s.Answer();
            s.LeaveEarly();
            Assert.That(s.IsDone, Is.True);
            Assert.That(s.LeftEarly, Is.True);
            Assert.That(s.Result, Is.Null);
            Assert.That(RitualSession.IsSupported("hoodoo"), Is.False);
        }
    
        [Test]
        public void Runes_ThePlayerChoosesOneThreeOrACast_EachWithItsMeaningAbove()
        {
            foreach (var (mode, count) in new[] { ("one", 1), ("three", 3) })
            {
                var s = new RitualSession("runes", "maya", Craft(), Library(), 3, false);
                s.Answer(); // arrive
                Assert.That(s.Current.Options.ConvertAll(o => o.Id), Is.EquivalentTo(new[] { "one", "three", "cast" }));
                s.Answer(mode);
                s.Answer(); s.Answer(); // hold the bag, draw
                Assert.That(s.Current.Phase, Is.EqualTo(RitualPhase.Moment));
                Assert.That(s.Current.Tokens.Count, Is.EqualTo(count), mode);
                Assert.That(s.Current.Tokens.TrueForAll(t => t.Caption.Contains("one, two")), Is.True, "the meaning is shown with each rune");
                if (mode == "three") Assert.That(s.Current.Tokens[0].Caption, Does.Contain("What was"));
                Walk(s);
                Assert.That(s.Result.Tab, Is.EqualTo(BookTab.Readings));
                Assert.That(s.Result.Vars["ritual_runes"].Split(',').Length, Is.EqualTo(count));
            }

            for (int seed = 0; seed < 20; seed++)
            {
                var c = new RitualSession("runes", "maya", Craft(), Library(), seed, false);
                c.Answer(); c.Answer("cast"); c.Answer(); c.Answer();
                Assert.That(c.Current.Tokens.Count, Is.InRange(1, 7), "at least one lands face up");
            }
        }

        [Test]
        public void CircleOffer_WrapsTheRitual_AndOpensAgainAtTheEnd()
        {
            var s = new RitualSession("candle", "maya", Craft(), Library(), 1, false, offerCircle: true);
            Assert.That(s.Current.Phase, Is.EqualTo(RitualPhase.Arrive));
            s.Answer();
            Assert.That(s.Current.Options.ConvertAll(o => o.Id), Is.EqualTo(new[] { "cast", "no" }));
            s.Answer("cast");
            var texts = new List<string>();
            int guard = 0;
            while (!s.IsDone && guard++ < 60)
            {
                texts.Add(s.Current.Text);
                var st = s.Current;
                s.Answer(st.Kind == RitualStepKind.Choose ? st.Options[0].Id : "");
            }
            Assert.That(texts.Exists(t => t.Contains("Call air")), Is.True);
            Assert.That(texts[texts.Count - 1], Does.Contain("open, but unbroken"));
            Assert.That(s.Result.InCircle, Is.True);

            var no = new RitualSession("candle", "maya", Craft(), Library(), 1, false, offerCircle: true);
            no.Answer(); no.Answer("no");
            Walk(no);
            Assert.That(no.Result.InCircle, Is.False);

            var ground = new RitualSession("grounding-centering", "maya", Craft(), Library(), 1, false, offerCircle: true);
            ground.Answer();
            Assert.That(ground.Current.Kind, Is.Not.EqualTo(RitualStepKind.Choose), "grounding comes before a circle, never inside one");
        }

        [Test]
        public void CharmBag_IsMadeAndCarried()
        {
            var s = new RitualSession("charm-bag", "maya", Craft(), Library(), 1, false);
            Walk(s, write: "steady");
            Assert.That(s.Result.CharmBag, Is.Not.Null);
            Assert.That(s.Result.CharmBag.carried, Is.True);
            Assert.That(s.Result.CharmBag.intent, Is.EqualTo("steady"));
        }

        [Test]
        public void Cleansing_Hard_StirsSomethingUp()
        {
            var easy = new RitualSession("cleansing", "maya", Craft(), Library(), 1, false);
            Walk(easy, write: "quiet");
            Assert.That(easy.Result.StirredUp, Is.False);
            Assert.That(easy.Result.Vars["ritual_holds"], Is.EqualTo("quiet"));
            var hard = new RitualSession("cleansing", "maya", Craft(), Library(), 1, false, variant: "hard");
            Walk(hard);
            Assert.That(hard.Result.StirredUp, Is.True);
        }

        [Test]
        public void Altar_OffersEverythingButCleansing()
        {
            Assert.That(RitualSession.AtTheAltar, Has.No.Member("cleansing"));
            foreach (var id in RitualSession.AtTheAltar) Assert.That(RitualSession.IsSupported(id), Is.True, id);
        }
    }
}
