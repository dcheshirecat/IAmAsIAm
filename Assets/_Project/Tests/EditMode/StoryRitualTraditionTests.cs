using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Story;
using IAmAsIAm.Core.Traditions;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    public class ChoiceLogTests
    {
        [Test]
        public void RedecidingReplacesTheOldAnswer()
        {
            var log = new ChoiceLog();
            log.Record(new ChoiceRecord { decisionId = "samhain.note", chapterId = "ch1", optionId = "kept" });
            log.Record(new ChoiceRecord { decisionId = "samhain.note", chapterId = "ch1", optionId = "binned" });
            Assert.That(log.Records.Count, Is.EqualTo(1));
            Assert.That(log.Chose("samhain.note", "binned"), Is.True);
        }

        [Test]
        public void RecapListsOnlyThatChapter()
        {
            var log = new ChoiceLog();
            log.Record(new ChoiceRecord { decisionId = "a", chapterId = "ch1", optionId = "x" });
            log.Record(new ChoiceRecord { decisionId = "b", chapterId = "ch2", optionId = "y" });
            Assert.That(log.ForChapter("ch1").Select(r => r.decisionId), Is.EqualTo(new[] { "a" }));
        }
    }

    public class RitualTests
    {
        static List<TarotCardEntry> Cards(int n) =>
            Enumerable.Range(0, n).Select(i => new TarotCardEntry { id = "c" + i, name = "Card " + i }).ToList();

        [Test]
        public void ThreeCardSpread_FillsEachPosition_AndFeedsStoryVariables()
        {
            var spread = new TarotSpreadDefinition
            {
                id = "three-card",
                positions = new List<SpreadPosition> { new SpreadPosition { id = "was" }, new SpreadPosition { id = "is" }, new SpreadPosition { id = "help" } },
            };
            var reading = TarotReader.Read(spread, new TarotDeck(Cards(22), 1));
            Assert.That(reading.Cards.Count, Is.EqualTo(3));
            Assert.That(reading.Cards.Select(c => c.Draw.Card.id).Distinct().Count(), Is.EqualTo(3));

            var vars = reading.ToStoryVariables();
            Assert.That(vars["tarot_spread"], Is.EqualTo("three-card"));
            Assert.That(vars.ContainsKey("tarot_help"), Is.True);
        }

        [Test]
        public void InnerCouncil_EachPresentPartDraws()
        {
            var spread = new TarotSpreadDefinition { id = "inner-council", onePerPart = true };
            var reading = TarotReader.Read(spread, new TarotDeck(Cards(22), 2), new[] { "wren", "juniper" });
            Assert.That(reading.Cards.Select(c => c.DrawnById), Is.EqualTo(new[] { "wren", "juniper" }));
            Assert.That(reading.ToStoryVariables().ContainsKey("tarot_part_juniper"), Is.True);
        }

        [Test]
        public void CraftRitual_StepsInOrder_AndCanResume()
        {
            var r = new CraftRitual("rowan-cross", new[] { "a", "b", "c" });
            string crafted = null;
            r.Crafted += id => crafted = id;
            r.CompleteStep();
            Assert.That(r.CurrentStep, Is.EqualTo("b"));
            r.ResumeAt(2);
            r.CompleteStep();
            Assert.That(crafted, Is.EqualTo("rowan-cross"));
            Assert.That(r.IsComplete, Is.True);
        }

        [Test]
        public void Sachet_NeedsScentedHerbs_UpToThree()
        {
            var lib = new PracticeLibrary(null, null, new HerbData
            {
                herbs = new List<HerbEntry>
                {
                    new HerbEntry { id = "lavender", sensory = "sweet", anchorSenses = new List<string> { "Smell" }, safetyNotes = new List<string> { "n" } },
                    new HerbEntry { id = "rowan", sensory = "red", anchorSenses = new List<string> { "See" }, safetyNotes = new List<string> { "n" } },
                },
            }, null, null);

            Assert.That(SachetBlend.IsValid(new[] { "lavender" }, lib, out _), Is.True);
            Assert.That(SachetBlend.IsValid(new[] { "rowan" }, lib, out var why), Is.False);
            Assert.That(why, Does.Contain("scent"));
            Assert.That(SachetBlend.IsValid(new string[0], lib, out _), Is.False);
        }
    }

    public class TraditionTests
    {
        static TraditionEntry Closed() => new TraditionEntry { id = "closed", openness = "ClosedOrInitiatory", opennessNote = "n", references = new List<string> { "r" } };

        [Test]
        public void ClosedTradition_CannotBeTried()
        {
            var t = Closed();
            t.tryInGame.Add(new TryablePractice { id = "p", appropriateForNonMembers = true, respectNote = "n", steps = new List<string> { "s" } });
            var journal = new TraditionJournal(new[] { t });
            Assert.That(journal.TryPractice("closed", "p"), Is.False);

            var issues = DataValidator.Validate(new DataValidator.Bundle { Traditions = new TraditionData { traditions = new List<TraditionEntry> { t } } });
            Assert.That(issues, Has.Some.Contains("must not have playable practices"));
        }

        [Test]
        public void UnapprovedTestimonyWithText_IsRejected()
        {
            var t = new TraditionEntry { id = "t", openness = "OpenToAll", opennessNote = "n", references = new List<string> { "r" } };
            t.livedExperiences.Add(new LivedExperience { id = "le", status = "awaiting-interview", text = "Invented words" });
            var issues = DataValidator.Validate(new DataValidator.Bundle { Traditions = new TraditionData { traditions = new List<TraditionEntry> { t } } });
            Assert.That(issues, Has.Some.Contains("consented, reviewed interview"));
        }

        [Test]
        public void OpenPractice_RecordsExploration()
        {
            var t = new TraditionEntry { id = "quakers", openness = "OpenToAll" };
            t.tryInGame.Add(new TryablePractice { id = "silence", appropriateForNonMembers = true });
            var journal = new TraditionJournal(new[] { t });
            Assert.That(journal.TryPractice("quakers", "silence"), Is.True);
            Assert.That(journal.ExploredIds, Does.Contain("quakers"));
        }
    }
}
