using System.Collections.Generic;
using IAmAsIAm.Core.Practices;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    public sealed class BookOfShadowsTests
    {
        static CraftData Data() => new CraftData
        {
            wiccanMemberIds = new List<string> { "maya", "izzy" },
            practices = new List<CraftPractice>
            {
                new CraftPractice { id = "tarot", name = "Tarot", steps = new List<string> { "Shuffle.", "Draw." } },
                new CraftPractice { id = "candle", name = "Candle magick", steps = new List<string> { "Light it." } },
            },
            refusals = new List<CraftRefusal> { new CraftRefusal { memberId = "jacob", line = "Not touching that." } },
        };

        [Test]
        public void Book_OpensOnlyOnceGivenAndOnlyForThoseWhoPractise()
        {
            var b = new BookOfShadows(Data());
            Assert.That(b.CanOpen("maya"), Is.False, "not before Tanya gives it");
            b.Give("tanya", 40);
            Assert.That(b.Has, Is.True);
            Assert.That(b.GivenById, Is.EqualTo("tanya"));
            Assert.That(b.CanOpen("maya"), Is.True);
            Assert.That(b.CanOpen("izzy"), Is.True);
            Assert.That(b.CanOpen("jacob"), Is.False);
            Assert.That(b.CanOpen("heather"), Is.False, "owner: Heather's candles are her own faith");
            Assert.That(b.RefusalLine("jacob"), Is.EqualTo("Not touching that."));
            Assert.That(b.RefusalLine("serena"), Is.Not.Empty);
        }

        [Test]
        public void Learning_AddsAPracticeOnceAndOnlyKnownOnes()
        {
            var b = new BookOfShadows(Data());
            Assert.That(b.Learn("tarot", "selene", 3), Is.True, "learning before she has the Book still counts");
            Assert.That(b.Learn("tarot", "tanya", 9), Is.False);
            Assert.That(b.Learn("hoodoo", "nobody", 9), Is.False, "unknown practices are refused");
            Assert.That(b.HasLearned("tarot"), Is.True);
            Assert.That(b.LearnedPractices()[0].learned.teacherId, Is.EqualTo("selene"));
        }

        [Test]
        public void Entries_AreKeptPerTabNewestFirstAndSurviveASave()
        {
            var b = new BookOfShadows(Data());
            b.Give("tanya", 1);
            Assert.That(b.Write(BookTab.Readings, "maya", "The Star", "   ", 2), Is.Null);
            b.Write(BookTab.Readings, "maya", "The Star", "Hope, after.", 2);
            b.Write(BookTab.Readings, "izzy", "Three of Cups", "PARTY", 3);
            b.Write(BookTab.Dreams, "maya", "", new string('a', 2000), 4);
            Assert.That(b.EntriesIn(BookTab.Readings)[0].memberId, Is.EqualTo("izzy"));
            Assert.That(b.EntriesIn(BookTab.Dreams)[0].text.Length, Is.EqualTo(BookOfShadows.MaxEntryLength));

            var copy = new BookOfShadows(Data());
            copy.Restore(b.Export());
            Assert.That(copy.Has, Is.True);
            Assert.That(copy.EntriesIn(BookTab.Readings).Count, Is.EqualTo(2));
            var next = copy.Write(BookTab.Notes, "maya", "", "New.", 5);
            Assert.That(next.id, Is.GreaterThan(3), "ids never repeat after loading");

            var empty = new BookOfShadows(Data());
            empty.Restore(null);
            Assert.That(empty.Has, Is.False);
        }
    }
}
