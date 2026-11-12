using System.Linq;
using IAmAsIAm.Core.Story;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    /// <summary>
    /// The owner's chapter rules: the first playthrough starts as the chapter's set alter; finishing a chapter unlocks its one set
    /// alter (never random); finished chapters can be replayed as anyone unlocked; triggers only switch to the host or unlocked alters.
    /// </summary>
    public class ChapterRunTests
    {
        [Test]
        public void FirstPlaythrough_OnlyTheStartingAlter()
        {
            var runs = new ChapterRuns();
            Assert.That(runs.StartersFor("prelude-wicca", "maya"), Is.EqualTo(new[] { "maya" }));
            Assert.That(runs.StartersFor("ch1-5-axel", "axel"), Is.EqualTo(new[] { "axel" }));
        }

        [Test]
        public void Finishing_UnlocksTheChaptersSetAlter_NotWhoeverFronted()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.RecordFront("lily"); // A triggered switch partway: doesn't decide the unlock.
            Assert.That(runs.Complete("nina"), Is.EqualTo(new[] { "nina" }));
            Assert.That(runs.IsUnlockedAnywhere("lily"), Is.False);
            Assert.That(runs.IsUnlockedAnywhere("nina"), Is.True);
        }

        [Test]
        public void FinishedChapters_ReplayAsAnyoneUnlocked_EvenFromOtherChapters()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.Complete("nina");
            runs.Begin("prelude-wicca", "maya");
            Assert.That(runs.StartersFor("prelude-wicca", "maya"), Is.EqualTo(new[] { "maya" }), "Not finished yet: Maya only.");
            runs.Complete("izzy");

            Assert.That(runs.StartersFor("prelude-wicca", "maya"), Is.EqualTo(new[] { "maya", "izzy", "nina" }));
            Assert.That(runs.StartersFor("prologue-quaker", "maya"), Is.EqualTo(new[] { "maya", "izzy", "nina" }));
            Assert.That(runs.StartersFor("ch1-christianity", "maya"), Is.EqualTo(new[] { "maya" }), "Not played yet.");
        }

        [Test]
        public void ChapterOnePointFive_IsAxelsFirst_AndUnlocksHim()
        {
            var runs = new ChapterRuns();
            runs.Begin("ch1-5-axel", "axel");
            runs.RecordFront("maya"); // Maya comes back at the end.
            Assert.That(runs.Complete("axel"), Is.EqualTo(new[] { "axel" }));
            Assert.That(runs.StartersFor("ch1-5-axel", "axel"), Is.EqualTo(new[] { "axel", "maya" }));
        }

        [Test]
        public void Replaying_DoesntUnlockAgain()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.Complete("nina");
            runs.Begin("prologue-quaker", "nina");
            Assert.That(runs.Complete("nina"), Is.Empty);
        }

        [Test]
        public void Triggers_OnlySwitchToTheHostOrUnlockedAlters()
        {
            var runs = new ChapterRuns();
            Assert.That(runs.CanSwitchTo("maya"), Is.True);
            Assert.That(runs.CanSwitchTo("heather"), Is.False, "Heather can't come forward in the Prelude (owner's example).");
            runs.Begin("prologue-quaker", "maya");
            runs.Complete("nina");
            Assert.That(runs.CanSwitchTo("nina"), Is.True);
            Assert.That(runs.CanSwitchTo("heather"), Is.False);
        }

        [Test]
        public void LeavingEarly_UnlocksNothing()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.Abandon();
            Assert.That(runs.HasFinished("prologue-quaker"), Is.False);
            Assert.That(runs.Complete("nina"), Is.Empty, "Not in a chapter: nothing happens.");
            Assert.That(runs.IsUnlockedAnywhere("nina"), Is.False);
        }

        [Test]
        public void Unlocks_SurviveSaveAndRestore()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.Complete("nina");
            var again = new ChapterRuns();
            again.Restore(runs.Export());
            Assert.That(again.HasFinished("prologue-quaker"), Is.True);
            Assert.That(again.StartersFor("prologue-quaker", "maya"), Is.EqualTo(new[] { "maya", "nina" }));
        }

        [Test]
        public void AChapterWithNoSetAlter_StillCountsAsFinished()
        {
            var runs = new ChapterRuns();
            runs.Begin("ch2-judaism", "maya");
            Assert.That(runs.Complete(), Is.Empty);
            Assert.That(runs.HasFinished("ch2-judaism"), Is.True);
        }

        [Test]
        public void PlayedAs_RecordsWhoFinishedEachChapter_AndIsSaved()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.Complete("nina");
            runs.Begin("prologue-quaker", "nina");
            runs.Abandon();
            Assert.That(runs.PlayedAs("prologue-quaker"), Is.EquivalentTo(new[] { "maya" }), "an abandoned run isn't a branch");
            runs.Begin("prologue-quaker", "nina");
            runs.Complete("nina");
            Assert.That(runs.PlayedAs("prologue-quaker"), Is.EquivalentTo(new[] { "maya", "nina" }));

            var loaded = new ChapterRuns();
            loaded.Restore(runs.Export());
            Assert.That(loaded.PlayedAs("prologue-quaker"), Is.EquivalentTo(new[] { "maya", "nina" }));
            Assert.That(loaded.PlayedAs("prelude-wicca"), Is.Empty);
        }
    }
}
