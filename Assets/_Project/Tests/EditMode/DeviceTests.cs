using System.Linq;
using IAmAsIAm.Core.Devices;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    public sealed class DeviceTests
    {
        [Test]
        public void Devices_FollowTheOwnersAges()
        {
            var d = new Devices();
            d.SetAge(11);
            Assert.That(d.CanEmail, Is.False);
            Assert.That(d.CanText, Is.False);

            d.SetAge(12);
            Assert.That(d.Has(DeviceKind.Computer), Is.True);
            Assert.That(d.CanEmail, Is.True);
            Assert.That(d.CanEmailAnywhere, Is.False, "the family computer stays at home");
            Assert.That(d.CanText, Is.False);

            d.SetAge(18);
            Assert.That(d.CanText, Is.True);
            Assert.That(d.CanEmailAnywhere, Is.True);
        }

        [Test]
        public void SetAge_IsExact_SoReplayingAnEarlierChapterTakesThePhoneAway()
        {
            var d = new Devices();
            d.SetAge(21);
            d.SetAge(15);
            Assert.That(d.CanText, Is.False);
            Assert.That(d.Has(DeviceKind.Computer), Is.True);
        }

        [Test]
        public void MessagesSentDuringLostTime_LookLikeAnyOtherSentMessage()
        {
            var d = new Devices();
            d.SetAge(18);
            int heard = 0;
            d.Received += _ => heard++;

            var mine = d.Send(MessageChannel.Text, "selene", "see you in class", 3);
            var away = d.SentWhileAway("izzy", MessageChannel.Text, "selene", "full moon tues!!", 4);

            Assert.That(heard, Is.EqualTo(0), "no notification for either");
            Assert.That(away.outgoing, Is.EqualTo(mine.outgoing));
            Assert.That(away.read, Is.EqualTo(mine.read));
            Assert.That(d.UnreadCount(MessageChannel.Text), Is.EqualTo(0));
            Assert.That(away.writtenById, Is.EqualTo("izzy"), "kept for the story only");
        }

        [Test]
        public void IncomingMessages_AreUnreadUntilOpened_AndNotify()
        {
            var d = new Devices();
            d.SetAge(21);
            DeviceMessage got = null;
            d.Received += m => got = m;
            d.Receive(MessageChannel.Text, "tanya", "Thursday. 7. Ground first.", 5);
            d.Receive(MessageChannel.Email, "tanya", "Notes from tonight are attached.", 5, "Intro to the Craft, week 1");

            Assert.That(got.channel, Is.EqualTo("Email"));
            Assert.That(d.UnreadCount(MessageChannel.Text), Is.EqualTo(1));
            d.MarkRead(MessageChannel.Text, "tanya");
            Assert.That(d.UnreadCount(MessageChannel.Text), Is.EqualTo(0));
            Assert.That(d.UnreadCount(MessageChannel.Email), Is.EqualTo(1));
        }

        [Test]
        public void Texts_CantBeSeenWithoutAPhone()
        {
            var d = new Devices();
            d.SetAge(21);
            d.Receive(MessageChannel.Text, "selene", "hi", 1);
            d.SetAge(15);
            Assert.That(d.UnreadCount(MessageChannel.Text), Is.EqualTo(0));
        }

        [Test]
        public void Threads_NewestFirst_MessagesOldestFirst()
        {
            var d = new Devices();
            d.SetAge(18);
            d.Receive(MessageChannel.Text, "selene", "1", 1);
            d.Receive(MessageChannel.Text, "tanya", "2", 1);
            d.Send(MessageChannel.Text, "selene", "3", 1);
            var threads = d.Threads(MessageChannel.Text);
            Assert.That(threads.Select(t => t.ContactId), Is.EqualTo(new[] { "selene", "tanya" }));
            Assert.That(threads[0].Messages.Select(m => m.body), Is.EqualTo(new[] { "1", "3" }));
        }

        [Test]
        public void ExportRestore_RoundTrips_AndKeepsIdsUnique()
        {
            var d = new Devices();
            d.SetAge(18);
            d.Receive(MessageChannel.Text, "selene", "a", 1);
            d.SentWhileAway("izzy", MessageChannel.Email, "tanya", "b", 2, "re: classes");
            var state = d.Export();

            var e = new Devices();
            e.Restore(state);
            Assert.That(e.Age, Is.EqualTo(18));
            Assert.That(e.CanText, Is.True);
            Assert.That(e.Messages.Count, Is.EqualTo(2));
            var next = e.Send(MessageChannel.Text, "selene", "c", 3);
            Assert.That(next.id, Is.GreaterThan(state.messages.Max(m => m.id)));
        }

        [Test]
        public void Restore_FromAnOldSave_IsEmpty()
        {
            var d = new Devices();
            d.Restore(null);
            Assert.That(d.Age, Is.EqualTo(0));
            Assert.That(d.Messages.Count, Is.EqualTo(0));
            Assert.That(d.CanEmail, Is.False);
        }

        [Test]
        public void SystemLog_IsSigned_TrimmedAndSaved()
        {
            var d = new Devices();
            Assert.That(d.HasApp(Devices.SystemLogApp), Is.False);
            d.GainApp(Devices.SystemLogApp);
            Assert.That(d.HasApp(Devices.SystemLogApp), Is.True);
            Assert.That(d.Log("izzy", "   ", 1), Is.Null, "blank notes are ignored");
            var a = d.Log("izzy", "  bought oat milk  ", 1, "ch4-islam");
            var b = d.Log("maya", new string('x', Devices.MaxLogLength + 50), 2, "", byPlayer: true);
            Assert.That(a.text, Is.EqualTo("bought oat milk"));
            Assert.That(a.memberId, Is.EqualTo("izzy"));
            Assert.That(b.text.Length, Is.EqualTo(Devices.MaxLogLength));
            Assert.That(b.byPlayer, Is.True);

            var e = new Devices();
            e.Restore(d.Export());
            Assert.That(e.HasApp(Devices.SystemLogApp), Is.True);
            Assert.That(e.SystemLog.Count, Is.EqualTo(2));
            var c = e.Log("heather", "drink water", 3);
            Assert.That(c.id, Is.GreaterThan(b.id), "ids stay unique after loading");
            var m = e.Send(MessageChannel.Text, "selene", "hi", 3);
            Assert.That(m.id, Is.GreaterThan(c.id));
        }
    }
}
