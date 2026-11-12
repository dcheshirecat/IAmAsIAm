using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Traditions;
using IAmAsIAm.Core.Triggers;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    public class TriggerTests
    {
        static SystemRoster Roster() => new SystemRoster(new[]
        {
            new SystemMemberDefinition { id = "host" },
            new SystemMemberDefinition { id = "a", trustToFront = 1f },
            new SystemMemberDefinition { id = "b", trustToFront = 1f },
            new SystemMemberDefinition { id = "shy", initialPresentation = "Symbolic" },
        }, "host");

        static TriggerDefinition Def(params TriggerOutcome[] outcomes) => new TriggerDefinition
        {
            id = "voices", name = "Raised voices", hint = "Loud rooms…", learnAfter = 2,
            cues = new List<string> { "raised-voices" }, outcomes = outcomes.ToList(),
        };

        [Test]
        public void UnrelatedCue_DoesNothing()
        {
            var sys = new TriggerSystem(new[] { Def(new TriggerOutcome { response = "Derealization" }) }, 1);
            Assert.That(sys.Encounter("birdsong", Roster()), Is.Null);
        }

        [Test]
        public void PlayerLearnsTriggerOverEncounters()
        {
            var sys = new TriggerSystem(new[] { Def(new TriggerOutcome { response = "Derealization" }) }, 1);
            Assert.That(sys.Knowledge("voices"), Is.EqualTo(TriggerKnowledge.Unknown));
            sys.Encounter("raised-voices", Roster());
            Assert.That(sys.Knowledge("voices"), Is.EqualTo(TriggerKnowledge.Noticed));
            Assert.That(sys.IsKnownCue("raised-voices"), Is.False);
            sys.Encounter("raised-voices", Roster());
            Assert.That(sys.Knowledge("voices"), Is.EqualTo(TriggerKnowledge.Known));
            Assert.That(sys.IsKnownCue("raised-voices"), Is.True);
        }

        [Test]
        public void OutcomeIsRandomByWeight_NotChosenByPlayer()
        {
            var sys = new TriggerSystem(new[] { Def(
                new TriggerOutcome { response = "Depersonalization", weight = 1 },
                new TriggerOutcome { response = "Derealization", weight = 1 },
                new TriggerOutcome { response = "Fog", weight = 1 }) }, 42);
            var seen = new HashSet<TriggerResponse>();
            for (int i = 0; i < 60; i++) seen.Add(sys.Encounter("raised-voices", Roster()).Response);
            Assert.That(seen.Count, Is.EqualTo(3), "All three outcomes should occur over many encounters.");
        }

        [Test]
        public void ABriefEncounter_NeverSwitches()
        {
            var sys = new TriggerSystem(new[] { Def(new TriggerOutcome { response = "Derealization" }) }, 1);
            for (int i = 0; i < 20; i++)
                Assert.That(sys.Encounter("raised-voices", Roster()).Response, Is.Not.EqualTo(TriggerResponse.Switch));
        }

        [Test]
        public void SustainedExposure_LeadsToOneSwitch_IgnoringConsent()
        {
            var roster = Roster();
            roster.SetResting("a", true); // An involuntary switch doesn't ask.
            var def = Def(new TriggerOutcome { response = "Derealization" });
            def.switchAfterSeconds = 10f;
            var sys = new TriggerSystem(new[] { def }, 3);

            Assert.That(sys.Expose("raised-voices", roster, 0f).Response, Is.EqualTo(TriggerResponse.Derealization));
            Assert.That(sys.Expose("raised-voices", roster, 9f), Is.Null, "Not yet: the character might still get away.");
            var sw = sys.Expose("raised-voices", roster, 2f);
            Assert.That(sw.Response, Is.EqualTo(TriggerResponse.Switch));
            Assert.That(sw.Escalated, Is.True);
            Assert.That(new[] { "a", "b" }, Does.Contain(sw.SwitchToId));
            Assert.That(sys.Expose("raised-voices", roster, 30f), Is.Null, "Only one switch per exposure.");
        }

        [Test]
        public void GettingAway_ResetsExposure()
        {
            var def = Def(new TriggerOutcome { response = "Fog" });
            def.switchAfterSeconds = 10f;
            var sys = new TriggerSystem(new[] { def }, 3);
            sys.Expose("raised-voices", Roster(), 0f);
            sys.Expose("raised-voices", Roster(), 8f);
            sys.EndExposure("raised-voices");
            Assert.That(sys.ExposureSeconds("raised-voices"), Is.EqualTo(0f));
            sys.Expose("raised-voices", Roster(), 0f);
            Assert.That(sys.Expose("raised-voices", Roster(), 8f), Is.Null);
        }

        [Test]
        public void Trapped_DissociatesThenSwitches()
        {
            var sys = new TriggerSystem(new[] { Def(new TriggerOutcome { response = "Derealization" }) }, 3);
            var fired = new List<TriggerActivation>();
            sys.Fired += fired.Add;
            sys.Trapped("raised-voices", Roster());
            Assert.That(fired.Select(f => f.Response), Is.EqualTo(new[] { TriggerResponse.Derealization, TriggerResponse.Switch }));
        }

        [Test]
        public void Switch_WithNoOneAvailable_FallsBackToDepersonalization()
        {
            var roster = new SystemRoster(new[] { new SystemMemberDefinition { id = "solo" } }, "solo");
            var sys = new TriggerSystem(new[] { Def(new TriggerOutcome { response = "Fog" }) }, 3);
            Assert.That(sys.Trapped("raised-voices", roster).Response, Is.EqualTo(TriggerResponse.Depersonalization));
        }

        [Test]
        public void SwitchAsADirectOutcome_IsRejectedByValidator()
        {
            var def = Def(new TriggerOutcome { response = "Switch" });
            var issues = DataValidator.Validate(new DataValidator.Bundle { Triggers = new TriggerData { triggers = new List<TriggerDefinition> { def } } });
            Assert.That(issues, Has.Some.Contains("switches only come from not being able to get away"));
        }

        [Test]
        public void MemberSpecificTrigger_OnlyFiresForThatMember()
        {
            var def = Def(new TriggerOutcome { response = "Derealization" });
            def.affectsMembers.Add("a");
            var sys = new TriggerSystem(new[] { def }, 1);
            Assert.That(sys.Encounter("raised-voices", Roster()), Is.Null);
        }

        [Test]
        public void Activation_PushesMindOutOfWindow()
        {
            var mind = new MindModel();
            var sys = new TriggerSystem(new[] { Def(new TriggerOutcome { response = "Derealization", intensity = 0.8f }) }, 1);
            TriggerSystem.ApplyToMind(sys.Encounter("raised-voices", Roster()), mind);
            Assert.That(mind.GetIntensity(DissociativeExperience.Derealization), Is.EqualTo(0.8f).Within(1e-5));
            Assert.That(mind.Arousal, Is.GreaterThan(0f));
        }

        [Test]
        public void AvoidingAKnownCue_IsCounted()
        {
            var sys = new TriggerSystem(new[] { Def(new TriggerOutcome { response = "Fog" }) }, 1);
            sys.RecordAvoided("raised-voices");
            Assert.That(sys.Avoided("voices"), Is.EqualTo(1));
        }
    }

    public class TestimonySourceTests
    {
        static List<string> Validate(LivedExperience le)
        {
            var t = new TraditionEntry { id = "t", openness = "OpenToAll", opennessNote = "n", references = new List<string> { "r" } };
            t.livedExperiences.Add(le);
            return DataValidator.Validate(new DataValidator.Bundle { Traditions = new TraditionData { traditions = new List<TraditionEntry> { t } } });
        }

        [Test]
        public void PublicConsensus_NeedsTwoSources()
        {
            var le = new LivedExperience { id = "x", status = "approved", sourceType = "public-consensus", text = "Paraphrase",
                sources = new List<TestimonySource> { new TestimonySource { url = "https://example.org/a" } } };
            Assert.That(Validate(le), Has.Some.Contains("at least 2 independent sources"));
            le.sources.Add(new TestimonySource { url = "https://example.org/b" });
            Assert.That(Validate(le).Where(i => i.Contains("lived experience")), Is.Empty);
        }

        [Test]
        public void PublicStream_NeedsAName()
        {
            var le = new LivedExperience { id = "x", status = "approved", sourceType = "public-stream", text = "Paraphrase",
                sources = new List<TestimonySource> { new TestimonySource { title = "a" }, new TestimonySource { title = "b" } } };
            Assert.That(Validate(le), Has.Some.Contains("streamName"));
        }
    }
}
