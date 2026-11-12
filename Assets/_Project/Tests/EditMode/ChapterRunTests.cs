using System.Linq;
using IAmAsIAm.Core.Story;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    /// <summary>The owner's replay rule: alters who front during a FULL chapter run become starting alters for it.</summary>
    public class ChapterRunTests
    {
        [Test]
        public void FinishingAChapter_UnlocksEveryoneWhoFronted()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.RecordFront("lily");
            runs.RecordFront("nina");
            var fresh = runs.Complete();
            Assert.That(fresh.OrderBy(x => x).ToArray(), Is.EqualTo(new[] { "lily", "nina" }));
            Assert.That(runs.StartersFor("prologue-quaker", "maya"), Is.EqualTo(new[] { "maya", "lily", "nina" }));
            Assert.That(runs.StartersFor("prelude-wicca", "maya"), Is.EqualTo(new[] { "maya" }), "Unlocks are per chapter.");
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
    }
}
