using System;
using System.Collections.Generic;
using IAmAsIAm.Core.Practices;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    public sealed class WheelOfTheYearTests
    {
        [Test]
        public void Moon_MatchesKnownDates()
        {
            Assert.That(Moon.IsFull(new DateTime(2024, 1, 25)), Is.True, "full moon 25 Jan 2024");
            Assert.That(Moon.IsNew(new DateTime(2024, 1, 11)), Is.True, "new moon 11 Jan 2024");
            Assert.That(Moon.IsFull(new DateTime(2018, 3, 31)), Is.True, "the blue moon, 31 Mar 2018");
            Assert.That(Moon.Phase(new DateTime(2024, 1, 18)), Is.EqualTo(MoonPhase.FirstQuarter));
            Assert.That(Moon.IsFull(new DateTime(2024, 1, 11)), Is.False);
        }

        static List<SabbatDef> Sabbats() => new List<SabbatDef>
        {
            new SabbatDef { id = "yule", month = 12, day = 21, tanyaText = "Longest night." },
            new SabbatDef { id = "imbolc", month = 2, day = 1, tanyaText = "Imbolc." },
            new SabbatDef { id = "samhain", month = 10, day = 31, tanyaText = "Samhain." },
        };

        [Test]
        public void Sabbat_IsFoundNearItsDate_NearItsDate()
        {
            Assert.That(WheelOfTheYear.Near(new DateTime(2018, 12, 28), Sabbats())?.id, Is.EqualTo("yule"));
            Assert.That(WheelOfTheYear.Near(new DateTime(2024, 2, 3), Sabbats())?.id, Is.EqualTo("imbolc"));
            Assert.That(WheelOfTheYear.Near(new DateTime(2024, 7, 3), Sabbats()), Is.Null);
        }

        [Test]
        public void Tanya_TextsEachSabbatOnce_OnlyAfterTheBook()
        {
            var book = new BookOfShadows(new CraftData { sabbats = Sabbats() });
            Assert.That(book.SetDate(new DateTime(2016, 10, 31)), Is.Null, "no Book yet: no text");
            book.Give("tanya", 1);
            Assert.That(book.SetDate(new DateTime(2019, 10, 30))?.id, Is.EqualTo("samhain"));
            Assert.That(book.SetDate(new DateTime(2019, 10, 31)), Is.Null, "once a year");
            Assert.That(book.SetDate(new DateTime(2020, 10, 31))?.id, Is.EqualTo("samhain"), "again next year");
            Assert.That(book.SetDate(new DateTime(2020, 12, 21))?.id, Is.EqualTo("yule"));
            Assert.That(book.SetDate(new DateTime(2020, 12, 26)), Is.Null, "the same Yule");

            var copy = new BookOfShadows(new CraftData { sabbats = Sabbats() });
            copy.Restore(book.Export());
            Assert.That(copy.Date, Is.EqualTo(new DateTime(2020, 12, 26)));
            Assert.That(copy.SetDate(new DateTime(2020, 12, 22)), Is.Null, "remembered across saves");
        }

        [Test]
        public void SabbatRitual_UsesItsDay_AndGoesOnTheWheel()
        {
            var craft = new CraftData { sabbats = new List<SabbatDef> { new SabbatDef { id = "mabon", name = "Mabon", prepare = new List<string> { "Apples." }, moment = "Balance.", reflect = "Let go of?", close = "Thanks." } } };
            var s = new RitualSession("sabbat", "izzy", craft, null, 1, false, variant: "mabon");
            int guard = 0;
            while (!s.IsDone && guard++ < 30) s.Answer(s.Current.Kind == RitualStepKind.Choose ? s.Current.Options[0].Id : "");
            Assert.That(s.Result.Tab, Is.EqualTo(BookTab.Wheel));
            Assert.That(s.Result.Title, Is.EqualTo("Mabon"));
        }
    }
}
