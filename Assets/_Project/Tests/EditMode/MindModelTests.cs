using IAmAsIAm.Core.Mind;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    public class MindModelTests
    {
        [Test]
        public void StartsSettledAndPresent()
        {
            var mind = new MindModel();
            Assert.That(mind.Zone, Is.EqualTo(ArousalZone.WithinWindow));
            Assert.That(mind.Presence, Is.EqualTo(1f));
        }

        [Test]
        public void Hypoarousal_ProducesFog_WithoutBeingScripted()
        {
            var mind = new MindModel(0.3f);
            mind.Nudge(-0.8f);
            Assert.That(mind.Zone, Is.EqualTo(ArousalZone.Hypoaroused));
            Assert.That(mind.GetIntensity(DissociativeExperience.Fog), Is.GreaterThan(0f));
            Assert.That(mind.Presence, Is.LessThan(1f));
        }

        [Test]
        public void Hyperarousal_DoesNotProduceFog()
        {
            var mind = new MindModel(0.3f);
            mind.Nudge(0.8f);
            Assert.That(mind.GetIntensity(DissociativeExperience.Fog), Is.EqualTo(0f));
        }

        [Test]
        public void MatchedSkill_HelpsMoreThanMismatchedSkill()
        {
            var matched = new MindModel(0.3f);
            var mismatched = new MindModel(0.3f);
            matched.Nudge(-0.9f);
            mismatched.Nudge(-0.9f);

            // Shutdown: activating skills fit; slow calming breath fits less well.
            matched.ApplyGrounding(new GroundingOutcome(SkillDirection.RaisesArousal, 1f));
            mismatched.ApplyGrounding(new GroundingOutcome(SkillDirection.LowersArousal, 1f));

            Assert.That(matched.Arousal, Is.GreaterThan(mismatched.Arousal));
            Assert.That(mismatched.Arousal, Is.GreaterThan(-0.9f), "A mismatched skill should still help a little.");
        }

        [Test]
        public void Grounding_NeverOvershootsPastSettled()
        {
            var mind = new MindModel();
            mind.Nudge(0.1f);
            mind.ApplyGrounding(new GroundingOutcome(SkillDirection.LowersArousal, 1f));
            Assert.That(mind.Arousal, Is.EqualTo(0f));
        }

        [Test]
        public void Grounding_EasesOnlyTargetedExperiences()
        {
            var mind = new MindModel();
            mind.SetExperience(DissociativeExperience.Derealization, 0.8f);
            mind.SetExperience(DissociativeExperience.Depersonalization, 0.8f);

            mind.ApplyGrounding(new GroundingOutcome(SkillDirection.Either, 1f, DissociativeExperience.Derealization));

            Assert.That(mind.GetIntensity(DissociativeExperience.Derealization), Is.LessThan(0.8f));
            Assert.That(mind.GetIntensity(DissociativeExperience.Depersonalization), Is.EqualTo(0.8f));
        }

        [Test]
        public void CoConsciousness_IsNotTreatedAsLossOfPresence_OrSomethingToFix()
        {
            var mind = new MindModel();
            mind.SetExperience(DissociativeExperience.CoConsciousness, 1f);
            Assert.That(mind.Presence, Is.EqualTo(1f));

            mind.ApplyGrounding(new GroundingOutcome(SkillDirection.Either, 1f));
            Assert.That(mind.GetIntensity(DissociativeExperience.CoConsciousness), Is.EqualTo(1f));
        }

        [Test]
        public void Practice_WidensWindow_WithDiminishingReturns_UpToMax()
        {
            var mind = new MindModel(0.3f);
            float before = mind.WindowHalfWidth;
            mind.RecordPractice(0.1f);
            float firstGain = mind.WindowHalfWidth - before;
            Assert.That(firstGain, Is.GreaterThan(0f));

            for (int i = 0; i < 1000; i++) mind.RecordPractice(0.1f);
            Assert.That(mind.WindowHalfWidth, Is.LessThanOrEqualTo(MindModel.MaxWindowHalfWidth));

            float nearMax = mind.WindowHalfWidth;
            mind.RecordPractice(0.1f);
            Assert.That(mind.WindowHalfWidth - nearMax, Is.LessThan(firstGain));
        }

        [Test]
        public void Tick_SettlesArousal_AndFadesUnheldExperiences()
        {
            var mind = new MindModel();
            mind.Nudge(0.5f);
            mind.SetExperience(DissociativeExperience.Derealization, 0.5f);

            mind.Tick(5f, holdScriptedExperiences: true);
            Assert.That(mind.Arousal, Is.LessThan(0.5f));
            Assert.That(mind.GetIntensity(DissociativeExperience.Derealization), Is.EqualTo(0.5f));

            mind.Tick(5f, holdScriptedExperiences: false);
            Assert.That(mind.GetIntensity(DissociativeExperience.Derealization), Is.LessThan(0.5f));
        }
    }
}
