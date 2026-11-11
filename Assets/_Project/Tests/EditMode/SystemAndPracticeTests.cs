using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Skills;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    public class SystemRosterTests
    {
        static SystemRoster MakeRoster() => new SystemRoster(new[]
        {
            new SystemMemberDefinition { id = "wren", displayName = "Wren", trustToFront = 0f },
            new SystemMemberDefinition { id = "juniper", displayName = "Juniper", trustToFront = 0.4f, perceptionTraits = new List<string> { "herb-lore" } },
            new SystemMemberDefinition { id = "bramble", displayName = "Bramble", trustToFront = 0.8f, initialPresentation = "Symbolic" },
            new SystemMemberDefinition { id = "sorrel", displayName = "Sorrel", canFront = false },
        }, "wren");

        [Test]
        public void LowTrust_MemberSaysNotNow_AndFrontIsUnchanged()
        {
            var roster = MakeRoster();
            Assert.That(roster.RequestSwitch("juniper"), Is.EqualTo(SwitchResult.DeclinedNotNow));
            Assert.That(roster.FrontingId, Is.EqualTo("wren"));
        }

        [Test]
        public void PartialTrust_OffersCoFront()
        {
            var roster = MakeRoster();
            roster.AdjustTrust("juniper", 0.25f);
            Assert.That(roster.RequestSwitch("juniper"), Is.EqualTo(SwitchResult.OfferedCoFront));
            Assert.That(roster.CoConscious, Does.Contain("juniper"));
            Assert.That(roster.CanPerceive("herb-lore"), Is.True);
        }

        [Test]
        public void EnoughTrust_Switches_Voluntarily()
        {
            var roster = MakeRoster();
            FrontChange? change = null;
            roster.FrontChanged += c => change = c;
            roster.AdjustTrust("juniper", 0.5f);

            Assert.That(roster.RequestSwitch("juniper"), Is.EqualTo(SwitchResult.Switched));
            Assert.That(roster.FrontingId, Is.EqualTo("juniper"));
            Assert.That(change.HasValue && change.Value.Voluntary, Is.True);
        }

        [Test]
        public void RestingByChoice_AlwaysWins_EvenAtFullTrust()
        {
            var roster = MakeRoster();
            roster.AdjustTrust("juniper", 1f);
            roster.SetResting("juniper", true);
            Assert.That(roster.RequestSwitch("juniper"), Is.EqualTo(SwitchResult.DeclinedNotNow));
        }

        [Test]
        public void SymbolicOrNonFrontingMembers_AreNotAvailable()
        {
            var roster = MakeRoster();
            roster.AdjustTrust("bramble", 1f);
            Assert.That(roster.RequestSwitch("bramble"), Is.EqualTo(SwitchResult.NotAvailable));
            Assert.That(roster.RequestSwitch("sorrel"), Is.EqualTo(SwitchResult.NotAvailable));

            roster.SetPresentation("bramble", MemberPresentation.Embodied);
            Assert.That(roster.RequestSwitch("bramble"), Is.EqualTo(SwitchResult.Switched));
        }

        [Test]
        public void ForcedSwitch_IsMarkedInvoluntary()
        {
            var roster = MakeRoster();
            FrontChange? change = null;
            roster.FrontChanged += c => change = c;
            roster.ForceSwitch("bramble");
            Assert.That(change.HasValue && !change.Value.Voluntary, Is.True);
        }
    }

    public class InnerJournalTests
    {
        [Test]
        public void GapRecovery_TracksCluesOnce()
        {
            var journal = new InnerJournal();
            var gap = journal.OpenGap("samhain-afternoon", 1, null, new[] { "receipt", "rosemary", "note" });

            Assert.That(journal.FindClue(gap.id, "receipt"), Is.True);
            Assert.That(journal.FindClue(gap.id, "receipt"), Is.False);
            Assert.That(journal.FindClue(gap.id, "not-a-clue"), Is.False);
            Assert.That(gap.Recovered, Is.EqualTo(1f / 3f).Within(1e-5));
        }
    }

    public class ThoughtRecordTests
    {
        [Test]
        public void WalksThroughStepsInOrder_AndReportsShift()
        {
            var r = new ThoughtRecord();
            Assert.That(r.NextStep, Is.EqualTo(ThoughtRecord.Step.Situation));
            r.situation = "Found a note I don't remember writing.";
            r.automaticThought = "I'm losing my mind.";
            r.RateBefore("Fear", 85);
            Assert.That(r.NextStep, Is.EqualTo(ThoughtRecord.Step.BalancedThought));

            r.distortions.Add(CognitiveDistortion.Catastrophizing);
            r.AddEvidence(false, "Juniper says they wrote it to help.", "juniper");
            r.balancedThought = "Someone in the system was trying to reach me.";
            r.RateAfter(55);

            Assert.That(r.IsComplete, Is.True);
            Assert.That(r.IntensityShift, Is.EqualTo(30));
            Assert.That(r.contributorIds, Does.Contain("juniper"));
        }

        [Test]
        public void RatingsAreClampedTo0To100()
        {
            var r = new ThoughtRecord();
            r.RateBefore("Shame", 250);
            r.RateAfter(-5);
            Assert.That(r.intensityBefore, Is.EqualTo(100));
            Assert.That(r.intensityAfter, Is.EqualTo(0));
        }
    }

    public class TarotDeckTests
    {
        static List<TarotCardEntry> Cards(int n) =>
            Enumerable.Range(0, n).Select(i => new TarotCardEntry { id = "c" + i, name = "Card " + i }).ToList();

        [Test]
        public void SameSeed_SameOrder()
        {
            var a = new TarotDeck(Cards(22), 7);
            var b = new TarotDeck(Cards(22), 7);
            for (int i = 0; i < 22; i++) Assert.That(a.Draw().Card.id, Is.EqualTo(b.Draw().Card.id));
        }

        [Test]
        public void DrawsEveryCardOnce_BeforeReshuffling()
        {
            var deck = new TarotDeck(Cards(22), 3);
            var ids = Enumerable.Range(0, 22).Select(_ => deck.Draw().Card.id).ToList();
            Assert.That(ids.Distinct().Count(), Is.EqualTo(22));
        }

        [Test]
        public void ReversalsOffByDefault()
        {
            var deck = new TarotDeck(Cards(22), 11);
            for (int i = 0; i < 50; i++) Assert.That(deck.Draw().Reversed, Is.False);
        }
    }
}
