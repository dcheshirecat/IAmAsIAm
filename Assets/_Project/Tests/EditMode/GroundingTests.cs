using IAmAsIAm.Core.Grounding;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    public class BreathingPacerTests
    {
        [Test]
        public void PacedBreathing_HasNoHolds_AndLongerExhale()
        {
            var p = BreathingPattern.Paced;
            Assert.That(p.HasHolds, Is.False);
            Assert.That(p.exhaleSeconds, Is.GreaterThan(p.inhaleSeconds));
            Assert.That(p.BreathsPerMinute, Is.InRange(5f, 7f));
        }

        [Test]
        public void SkipsZeroLengthPhases()
        {
            var pacer = new BreathingPacer(BreathingPattern.Paced, targetCycles: 3);
            Assert.That(pacer.Phase, Is.EqualTo(BreathPhase.Inhale));
            pacer.Tick(4.01f);
            Assert.That(pacer.Phase, Is.EqualTo(BreathPhase.Exhale));
            pacer.Tick(6f);
            Assert.That(pacer.Phase, Is.EqualTo(BreathPhase.Inhale));
            Assert.That(pacer.CompletedCycles, Is.EqualTo(1));
        }

        [Test]
        public void BoxBreathing_VisitsAllFourPhases()
        {
            var pacer = new BreathingPacer(BreathingPattern.Box, targetCycles: 2);
            var seen = new System.Collections.Generic.List<BreathPhase>();
            pacer.PhaseChanged += seen.Add;
            pacer.Tick(16.01f);
            Assert.That(seen, Is.EqualTo(new[] { BreathPhase.HoldIn, BreathPhase.Exhale, BreathPhase.HoldOut, BreathPhase.Inhale }));
        }

        [Test]
        public void LargeDelta_CountsEveryCycle_AndStopsAtTarget()
        {
            var pacer = new BreathingPacer(BreathingPattern.Paced, targetCycles: 4);
            pacer.Tick(1000f);
            Assert.That(pacer.CompletedCycles, Is.EqualTo(4));
            Assert.That(pacer.IsComplete, Is.True);
            Assert.That(pacer.Completion, Is.EqualTo(1f));
        }

        [Test]
        public void OpenEnded_NeverCompletes()
        {
            var pacer = new BreathingPacer(BreathingPattern.Gentle, targetCycles: 0);
            pacer.Tick(600f);
            Assert.That(pacer.IsComplete, Is.False);
            Assert.That(pacer.CompletedCycles, Is.GreaterThan(0));
        }

        [Test]
        public void WithoutHolds_RemovesHolds_KeepsRhythmSlow()
        {
            var box = BreathingPattern.Box;
            var noHolds = box.WithoutHolds();
            Assert.That(noHolds.HasHolds, Is.False);
            Assert.That(noHolds.exhaleSeconds, Is.GreaterThan(noHolds.inhaleSeconds));
        }

        [Test]
        public void Fullness_RisesOnInhale_FallsOnExhale()
        {
            var pacer = new BreathingPacer(BreathingPattern.Paced);
            pacer.Tick(2f);
            float mid = pacer.Fullness;
            pacer.Tick(1.9f);
            Assert.That(pacer.Fullness, Is.GreaterThan(mid));
            pacer.Tick(3f);
            Assert.That(pacer.Phase, Is.EqualTo(BreathPhase.Exhale));
            Assert.That(pacer.Fullness, Is.LessThan(1f));
        }
    }

    public class FiveSensesTests
    {
        [Test]
        public void FollowsFiveFourThreeTwoOne()
        {
            var ex = new FiveSensesExercise();
            Assert.That(ex.TotalTarget, Is.EqualTo(15));
            Assert.That(ex.CurrentSense, Is.EqualTo(Sense.See));
            for (int i = 0; i < 5; i++) Assert.That(ex.Notice(Sense.See), Is.True);
            Assert.That(ex.CurrentSense, Is.EqualTo(Sense.Touch));
            Assert.That(ex.CurrentTarget, Is.EqualTo(4));
        }

        [Test]
        public void WrongSense_DoesNotCount()
        {
            var ex = new FiveSensesExercise();
            Assert.That(ex.Notice(Sense.Hear), Is.False);
            Assert.That(ex.TotalNoticed, Is.EqualTo(0));
        }

        [Test]
        public void SameSceneObject_CountsOnce()
        {
            var ex = new FiveSensesExercise();
            Assert.That(ex.Notice(Sense.See, "kettle"), Is.True);
            Assert.That(ex.Notice(Sense.See, "kettle"), Is.False);
        }

        [Test]
        public void SkippingUnavailableSenses_StillCompletesFully()
        {
            var ex = new FiveSensesExercise();
            bool completed = false;
            ex.Completed += () => completed = true;
            for (int i = 0; i < 5; i++) ex.SkipCurrentSense();
            Assert.That(completed, Is.True);
            Assert.That(ex.Completion, Is.EqualTo(1f));
        }
    }
}
