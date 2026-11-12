using System.Linq;
using IAmAsIAm.Core.Story;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    /// <summary>The owner's replay rule: alters who front during a FULL chapter run become starting alters for it.</summary>
    public class ChapterRunTests
    {
        [Test]
        public void FinishingAChapter_UnlocksTheFirstNewAlterWhoFronted()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.RecordFront("nina");
            Assert.That(runs.Complete(), Is.EqualTo(new[] { "nina" }));
            Assert.That(runs.StartersFor("prologue-quaker", "maya"), Is.EqualTo(new[] { "maya", "nina" }));
            Assert.That(runs.StartersFor("prelude-wicca", "maya"), Is.EqualTo(new[] { "maya" }), "Unlocks are per chapter.");
        }

        [Test]
        public void Unlocks_ArePaced_OneNewAlterPerFinishedRun()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.RecordFront("lily");
            runs.RecordFront("nina");
            Assert.That(runs.Complete(), Is.EqualTo(new[] { "lily" }), "Only the first new one.");

            runs.Begin("prologue-quaker", "lily");
            runs.RecordFront("nina");
            Assert.That(runs.Complete(), Is.EqualTo(new[] { "nina" }), "The next run can unlock the next.");
            Assert.That(runs.StartersFor("prologue-quaker", "maya"), Is.EqualTo(new[] { "maya", "lily", "nina" }));
        }

        [Test]
        public void LeavingEarly_UnlocksNothing()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.RecordFront("jacob");
            runs.Abandon();
            Assert.That(runs.StartersFor("prologue-quaker", "maya"), Is.EqualTo(new[] { "maya" }));
            runs.RecordFront("axel"); // Not in a chapter: ignored.
            Assert.That(runs.Complete(), Is.Empty);
        }

        [Test]
        public void Unlocks_SurviveSaveAndRestore()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.RecordFront("lily");
            runs.Complete();
            var again = new ChapterRuns();
            again.Restore(runs.Export());
            Assert.That(again.StartersFor("prologue-quaker", "maya"), Is.EqualTo(new[] { "maya", "lily" }));
        }

        [Test]
        public void ReplayingAsAnUnlockedAlter_CanUnlockMaya_OnlyIfSheFronts()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "lily");
            Assert.That(runs.Complete(), Is.Empty, "The starter isn't 'new'.");
        }
    
        [Test]
        public void UnlockedAnywhereFollowsFinishedRuns()
        {
            var runs = new ChapterRuns();
            Assert.That(runs.IsUnlockedAnywhere("nina"), Is.False);
            runs.Begin("prologue-quaker", "maya");
            runs.RecordFront("nina");
            Assert.That(runs.IsUnlockedAnywhere("nina"), Is.False, "Not until the chapter is finished.");
            runs.Complete();
            Assert.That(runs.IsUnlockedAnywhere("nina"));
            Assert.That(runs.IsUnlockedAnywhere("maya"), Is.False, "The default starter isn't an unlock.");
            Assert.That(runs.AllUnlocked(), Is.EqualTo(new[] { "nina" }));
        }
}
}
