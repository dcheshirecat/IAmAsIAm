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

        static PracticeLibrary Library() => new PracticeLibrary(new RuneData(),
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
    }
}
