using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Save;
using IAmAsIAm.Core.Story;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    /// <summary>JsonUtility in Unity, System.Text.Json in the standalone harness (same as RealDataTests).</summary>
    sealed class TestJson : ISaveSerializer
    {
#if UNITY_5_3_OR_NEWER
        public string ToJson(object value) => UnityEngine.JsonUtility.ToJson(value);
        public T FromJson<T>(string json) => UnityEngine.JsonUtility.FromJson<T>(json);
#else
        static readonly System.Text.Json.JsonSerializerOptions Options = new System.Text.Json.JsonSerializerOptions { IncludeFields = true };
        public string ToJson(object value) => System.Text.Json.JsonSerializer.Serialize(value, value.GetType(), Options);
        public T FromJson<T>(string json) => System.Text.Json.JsonSerializer.Deserialize<T>(json, Options);
#endif
    }

    public class SaveSlotTests
    {
        static SaveData Game(string chapter, string front, long ticks, int day = 1) => new SaveData
        {
            chapterId = chapter,
            inGameDay = day,
            frontingId = front,
            header = new SaveSlotHeader { chapterId = chapter, chapterTitle = "Title of " + chapter, frontingId = front, inGameDay = day, savedAtUtcTicks = ticks },
        };

        [Test]
        public void NewStorageHasOnlyEmptySlots()
        {
            var slots = new SaveSlots(new MemorySaveStorage(), new TestJson());
            Assert.That(slots.List().Count, Is.EqualTo(3));
            Assert.That(slots.List().All(s => s.IsEmpty));
            Assert.That(slots.MostRecent(), Is.EqualTo(0));
            Assert.That(slots.FirstEmpty(), Is.EqualTo(1));
        }

        [Test]
        public void EachSlotIsItsOwnGame()
        {
            var slots = new SaveSlots(new MemorySaveStorage(), new TestJson());
            var a = Game("prologue-quaker", "maya", 100);
            a.chapterUnlocks.Add(new ChapterUnlock { chapterId = "prologue-quaker", memberIds = new List<string> { "nina" } });
            slots.Write(1, a);
            slots.Write(2, Game("prelude-wicca", "nina", 200));

            Assert.That(slots.TryRead(1, out var one));
            Assert.That(slots.TryRead(2, out var two));
            Assert.That(one.frontingId, Is.EqualTo("maya"));
            Assert.That(one.chapterUnlocks.Single().memberIds, Is.EqualTo(new[] { "nina" }));
            Assert.That(two.chapterUnlocks, Is.Empty, "Unlocks never carry across saves.");
            Assert.That(slots.FirstEmpty(), Is.EqualTo(3));
        }

        [Test]
        public void SummaryShowsTheHeaderAndMostRecentWins()
        {
            var slots = new SaveSlots(new MemorySaveStorage(), new TestJson());
            slots.Write(1, Game("prologue-quaker", "maya", 500, day: 3));
            slots.Write(3, Game("prelude-wicca", "nina", 900));
            var s1 = slots.Describe(1);
            Assert.That(s1.CanLoad);
            Assert.That(s1.Header.slot, Is.EqualTo(1));
            Assert.That(s1.Header.inGameDay, Is.EqualTo(3));
            Assert.That(SaveSlotText.Place(s1.Header), Is.EqualTo("Title of prologue-quaker"));
            Assert.That(slots.MostRecent(), Is.EqualTo(3));
        }

        [Test]
        public void MissingHeaderIsRebuiltFromTheSave()
        {
            var storage = new MemorySaveStorage();
            var slots = new SaveSlots(storage, new TestJson());
            slots.Write(2, Game("prologue-quaker", "maya", 42));
            storage.Delete(SaveSlots.HeaderKey(2));
            var s = slots.Describe(2);
            Assert.That(s.CanLoad);
            Assert.That(s.Header.frontingId, Is.EqualTo("maya"));
            Assert.That(s.Header.slot, Is.EqualTo(2));
        }

        [Test]
        public void BrokenOrNewerSavesAreNeverLoaded()
        {
            var storage = new MemorySaveStorage();
            var slots = new SaveSlots(storage, new TestJson());
            storage.Write(SaveSlots.DataKey(1), "{ this is not json");
            Assert.That(slots.Describe(1).Status, Is.EqualTo(SaveSlotStatus.Unreadable));
            Assert.That(slots.TryRead(1, out _), Is.False);
            Assert.That(slots.FirstEmpty(), Is.EqualTo(2), "An unreadable slot isn't empty: it's never overwritten by New game.");

            var future = Game("prologue-quaker", "maya", 1);
            storage.Write(SaveSlots.DataKey(2), new TestJson().ToJson(future).Replace("\"version\":" + SaveData.CurrentVersion, "\"version\":99"));
            Assert.That(slots.Describe(2).Status, Is.EqualTo(SaveSlotStatus.TooNew));
            Assert.That(slots.TryRead(2, out _), Is.False);
        }

        [Test]
        public void DeleteEmptiesTheSlot()
        {
            var storage = new MemorySaveStorage();
            var slots = new SaveSlots(storage, new TestJson());
            slots.Write(1, Game("prologue-quaker", "maya", 1));
            slots.Delete(1);
            Assert.That(slots.Describe(1).IsEmpty);
            Assert.That(storage.Exists(SaveSlots.HeaderKey(1)), Is.False);
        }

        [Test]
        public void SlotNumbersAreChecked()
        {
            var slots = new SaveSlots(new MemorySaveStorage(), new TestJson());
            Assert.Throws<System.ArgumentOutOfRangeException>(() => slots.Describe(0));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => slots.Write(4, new SaveData()));
        }

        [Test]
        public void PlayTimeReadsNaturally()
        {
            Assert.That(SaveSlotText.PlayTime(20), Is.EqualTo("under a minute"));
            Assert.That(SaveSlotText.PlayTime(12 * 60 + 5), Is.EqualTo("12 min"));
            Assert.That(SaveSlotText.PlayTime(65 * 60), Is.EqualTo("1 h 05 min"));
            Assert.That(SaveSlotText.Place(new SaveSlotHeader()), Is.EqualTo("Between chapters"));
        }

        [Test]
        public void ChapterRunInProgressSurvivesASave()
        {
            var runs = new ChapterRuns();
            runs.Begin("prologue-quaker", "maya");
            runs.RecordFront("nina");
            var saved = runs.ExportRun();

            var loaded = new ChapterRuns();
            loaded.RestoreRun(saved);
            Assert.That(loaded.CurrentChapterId, Is.EqualTo("prologue-quaker"));
            Assert.That(loaded.StartedAsId, Is.EqualTo("maya"));
            Assert.That(loaded.Complete("nina"), Is.EqualTo(new[] { "nina" }));

            var none = new ChapterRuns().ExportRun();
            loaded.RestoreRun(none);
            Assert.That(loaded.InChapter, Is.False);
        }
    }

    public class LoadingCardTests
    {
        static List<TarotCardEntry> Cards(int n) =>
            Enumerable.Range(0, n).Select(i => new TarotCardEntry { id = "c" + i, number = i, name = "Card " + i }).ToList();

        [Test]
        public void NeverTheSameCardTwiceInARow()
        {
            var picker = new LoadingCardPicker(Cards(3), 7);
            string last = null;
            for (int i = 0; i < 200; i++)
            {
                var draw = picker.Pick(last, allowReversals: false);
                Assert.That(draw.Card.id, Is.Not.EqualTo(last));
                Assert.That(draw.Reversed, Is.False);
                last = draw.Card.id;
            }
        }

        [Test]
        public void EveryCardCanComeUpAndReversalsFollowTheSetting()
        {
            var picker = new LoadingCardPicker(Cards(22), 1);
            var seen = new HashSet<string>();
            bool anyReversed = false;
            for (int i = 0; i < 2000; i++) { var d = picker.Pick(null, true); seen.Add(d.Card.id); anyReversed |= d.Reversed; }
            Assert.That(seen.Count, Is.EqualTo(22));
            Assert.That(anyReversed);
        }

        [Test]
        public void OneCardDeckStillWorks()
        {
            var picker = new LoadingCardPicker(Cards(1), 1);
            Assert.That(picker.Pick("c0", false).Card.id, Is.EqualTo("c0"));
            Assert.Throws<System.InvalidOperationException>(() => new LoadingCardPicker(null, 1).Pick(null, false));
        }

        [Test]
        public void TheCardRisesFaceDownThenTurnsOver()
        {
            var start = LoadingCardAnimation.Evaluate(0f, false);
            Assert.That(start.Alpha, Is.EqualTo(0f));
            Assert.That(start.OffsetY, Is.LessThan(0f));
            Assert.That(start.FaceShowing, Is.False);

            var edge = LoadingCardAnimation.Evaluate(LoadingCardAnimation.FlipStart + LoadingCardAnimation.FlipSeconds / 2f, false);
            Assert.That(edge.ScaleX, Is.LessThan(0.05f));

            var after = LoadingCardAnimation.Evaluate(LoadingCardAnimation.FlipStart + LoadingCardAnimation.FlipSeconds, false);
            Assert.That(after.FaceShowing);
            Assert.That(after.ScaleX, Is.EqualTo(1f).Within(0.001f));
            Assert.That(after.Alpha, Is.EqualTo(1f));
            Assert.That(LoadingCardAnimation.MinimumSeconds, Is.GreaterThan(LoadingCardAnimation.FlipStart + LoadingCardAnimation.FlipSeconds));
        }

        [Test]
        public void ReduceMotionKeepsTheCardStill()
        {
            for (float t = 0f; t < 10f; t += 0.1f)
            {
                var p = LoadingCardAnimation.Evaluate(t, true);
                Assert.That(p.OffsetY, Is.EqualTo(0f));
                Assert.That(p.ScaleX, Is.EqualTo(1f));
                Assert.That(p.Glow, Is.EqualTo(0.35f));
                Assert.That(LoadingCardAnimation.Dots(t, true), Is.EqualTo(3));
            }
            Assert.That(LoadingCardAnimation.Evaluate(5f, true).FaceShowing);
        }

        [Test]
        public void TheGlowBreathesSlowlyAndNeverFlashes()
        {
            float prev = LoadingCardAnimation.Evaluate(0f, false).Glow;
            for (float t = 0.02f; t < 20f; t += 0.02f)
            {
                float g = LoadingCardAnimation.Evaluate(t, false).Glow;
                Assert.That(g, Is.InRange(0.15f, 0.45f));
                Assert.That(System.Math.Abs(g - prev), Is.LessThan(0.01f), "No sudden jumps in brightness.");
                prev = g;
            }
        }

        [Test]
        public void FadeOutAndNumerals()
        {
            Assert.That(LoadingCardAnimation.Evaluate(5f, false, fadeOut: 1f).Alpha, Is.EqualTo(0f));
            Assert.That(LoadingCardAnimation.Numeral(0), Is.EqualTo("0"));
            Assert.That(LoadingCardAnimation.Numeral(14), Is.EqualTo("XIV"));
            Assert.That(LoadingCardAnimation.Numeral(19), Is.EqualTo("XIX"));
            Assert.That(LoadingCardAnimation.Numeral(21), Is.EqualTo("XXI"));
        }
    }
}
