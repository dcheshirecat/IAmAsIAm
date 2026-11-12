using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.Devices
{
    /// <summary>Maya's devices. She gains them as she grows up (owner, 2026-11-12).</summary>
    public enum DeviceKind { Computer, Phone, Laptop }

    /// <summary>Texts go to and from the phone; email is read on the family computer or the laptop.</summary>
    public enum MessageChannel { Text, Email }

    [Serializable]
    public sealed class DeviceMessage
    {
        public int id;
        public string channel;      // MessageChannel, as text (EnumText style)
        public string contactId;    // a person id from people.json
        public bool outgoing;       // sent from Maya's phone or account
        /// <summary>
        /// For outgoing messages: who in the system actually wrote it. NEVER shown to the player (owner: messages sent
        /// during lost time appear quietly, "an extra message here and there", and are never pointed out).
        /// </summary>
        public string writtenById;
        public string subject;      // email only
        public string body;
        public int day;
        public bool read;

        public MessageChannel Channel => channel == nameof(MessageChannel.Email) ? MessageChannel.Email : MessageChannel.Text;
    }

    [Serializable]
    public sealed class DevicesState
    {
        public int age;
        public List<string> owned = new List<string>();
        public List<DeviceMessage> messages = new List<DeviceMessage>();
        public int nextId = 1;
        /// <summary>Apps on the phone (e.g. "systemlog"). Empty in older saves.</summary>
        public List<string> apps = new List<string>();
        /// <summary>SystemLog: notes the system leaves for each other. Empty in older saves.</summary>
        public List<SystemLogEntry> systemLog = new List<SystemLogEntry>();
    }

    /// <summary>
    /// One note in SystemLog (owner, 2026-11-12: an app on Maya's phone, unlocked at the end of Chapter 4, that the system uses
    /// to talk between alters who pop in). Unlike messages sent during lost time, a SystemLog note IS signed: that's the
    /// point of the app.
    /// </summary>
    [Serializable]
    public sealed class SystemLogEntry
    {
        public int id;
        public string memberId;
        public string text;
        public int day;
        /// <summary>The chapter it was written in (empty outside a chapter).</summary>
        public string chapterId;
        /// <summary>True when the player typed it; false when the story wrote it.</summary>
        public bool byPlayer;
    }

    /// <summary>One conversation (texts) or one correspondent (email), newest activity first.</summary>
    public sealed class MessageThread
    {
        public string ContactId;
        public MessageChannel Channel;
        public List<DeviceMessage> Messages = new List<DeviceMessage>();
        public DeviceMessage Latest => Messages.Count == 0 ? null : Messages[Messages.Count - 1];
        public int Unread => Messages.Count(m => !m.outgoing && !m.read);
    }

    /// <summary>
    /// Maya's family computer, phone and laptop, and the texts and emails on them. Pure rules (no Unity).
    /// Owner's ages: the family computer at 12; a phone and a laptop at 18. Email works on either computer, but the
    /// family computer stays at home; texts need the phone.
    /// </summary>
    public sealed class Devices
    {
        public const int ComputerAge = 12;
        public const int PhoneAge = 18;
        public const int LaptopAge = 18;
        /// <summary>The SystemLog app's id (owner: unlocked at the end of Chapter 4).</summary>
        public const string SystemLogApp = "systemlog";
        /// <summary>Longest SystemLog note, so a note fits on the phone screen.</summary>
        public const int MaxLogLength = 500;

        readonly HashSet<string> _apps = new HashSet<string>();
        readonly List<SystemLogEntry> _log = new List<SystemLogEntry>();

        public bool HasApp(string app) => !string.IsNullOrEmpty(app) && _apps.Contains(app);
        public IReadOnlyList<SystemLogEntry> SystemLog => _log;

        /// <summary>Puts an app on the phone (from the story).</summary>
        public void GainApp(string app)
        {
            if (!string.IsNullOrEmpty(app) && _apps.Add(app)) Changed?.Invoke();
        }

        /// <summary>
        /// Adds a SystemLog note, signed by <paramref name="memberId"/>. Blank notes are ignored; long ones are cut to
        /// <see cref="MaxLogLength"/>. Returns the note, or null.
        /// </summary>
        public SystemLogEntry Log(string memberId, string text, int day, string chapterId = "", bool byPlayer = false)
        {
            if (string.IsNullOrEmpty(memberId) || string.IsNullOrWhiteSpace(text)) return null;
            text = text.Trim();
            if (text.Length > MaxLogLength) text = text.Substring(0, MaxLogLength);
            var e = new SystemLogEntry { id = _nextId++, memberId = memberId, text = text, day = day, chapterId = chapterId ?? "", byPlayer = byPlayer };
            _log.Add(e);
            Changed?.Invoke();
            return e;
        }

        readonly HashSet<DeviceKind> _owned = new HashSet<DeviceKind>();
        readonly List<DeviceMessage> _messages = new List<DeviceMessage>();
        int _nextId = 1;

        /// <summary>Maya's (the body's) age in the scene being played; 0 = not set yet.</summary>
        public int Age { get; private set; }

        /// <summary>A message came in (incoming only: messages written during lost time arrive silently).</summary>
        public event Action<DeviceMessage> Received;
        public event Action Changed;

        public IReadOnlyList<DeviceMessage> Messages => _messages;
        public bool Has(DeviceKind kind) => _owned.Contains(kind);
        public bool CanText => Has(DeviceKind.Phone);
        public bool CanEmail => Has(DeviceKind.Computer) || Has(DeviceKind.Laptop);
        /// <summary>The laptop goes everywhere; the family computer is only at home.</summary>
        public bool CanEmailAnywhere => Has(DeviceKind.Laptop);

        public static IEnumerable<DeviceKind> OwnedAtAge(int age)
        {
            if (age >= ComputerAge) yield return DeviceKind.Computer;
            if (age >= PhoneAge) yield return DeviceKind.Phone;
            if (age >= LaptopAge) yield return DeviceKind.Laptop;
        }

        /// <summary>
        /// Sets Maya's age for the scene and gives her exactly the devices she has at that age (chapters jump around in
        /// time, and replays start earlier chapters again, so this is set, not added).
        /// </summary>
        public void SetAge(int age)
        {
            Age = Math.Max(0, age);
            _owned.Clear();
            foreach (var d in OwnedAtAge(Age)) _owned.Add(d);
            Changed?.Invoke();
        }

        /// <summary>For a story moment that hands her a device early (a gift, a hand-me-down).</summary>
        public void Gain(DeviceKind kind)
        {
            if (_owned.Add(kind)) Changed?.Invoke();
        }

        public DeviceMessage Receive(MessageChannel channel, string contactId, string body, int day, string subject = "")
            => Add(channel, contactId, false, null, body, day, subject);

        /// <summary>Maya sends a message herself (a story choice).</summary>
        public DeviceMessage Send(MessageChannel channel, string contactId, string body, int day, string subject = "")
            => Add(channel, contactId, true, null, body, day, subject);

        /// <summary>
        /// A message someone else in the system sent from Maya's phone or account during lost time. It looks exactly like
        /// any other sent message: no marker, no notification. The player may notice it, or not.
        /// </summary>
        public DeviceMessage SentWhileAway(string writtenById, MessageChannel channel, string contactId, string body, int day, string subject = "")
            => Add(channel, contactId, true, writtenById, body, day, subject);

        DeviceMessage Add(MessageChannel channel, string contactId, bool outgoing, string writtenById, string body, int day, string subject)
        {
            var m = new DeviceMessage
            {
                id = _nextId++,
                channel = channel.ToString(),
                contactId = contactId ?? "",
                outgoing = outgoing,
                writtenById = writtenById ?? "",
                subject = subject ?? "",
                body = body ?? "",
                day = day,
                read = outgoing,
            };
            _messages.Add(m);
            if (!outgoing) Received?.Invoke(m);
            Changed?.Invoke();
            return m;
        }

        /// <summary>Messages the player can see right now (texts need the phone; email needs a computer).</summary>
        public bool CanSee(MessageChannel channel) => channel == MessageChannel.Text ? CanText : CanEmail;

        public int UnreadCount(MessageChannel channel) => CanSee(channel) ? _messages.Count(m => m.Channel == channel && !m.outgoing && !m.read) : 0;

        /// <summary>Threads on one channel, the most recently active first. Messages inside are oldest first.</summary>
        public List<MessageThread> Threads(MessageChannel channel)
        {
            var threads = new List<MessageThread>();
            foreach (var group in _messages.Where(m => m.Channel == channel).GroupBy(m => m.contactId))
                threads.Add(new MessageThread { ContactId = group.Key, Channel = channel, Messages = group.OrderBy(m => m.id).ToList() });
            return threads.OrderByDescending(t => t.Latest.id).ToList();
        }

        public void MarkRead(MessageChannel channel, string contactId)
        {
            bool any = false;
            foreach (var m in _messages)
                if (m.Channel == channel && m.contactId == contactId && !m.read) { m.read = true; any = true; }
            if (any) Changed?.Invoke();
        }

        public DevicesState Export() => new DevicesState
        {
            age = Age,
            owned = _owned.Select(d => d.ToString()).OrderBy(s => s).ToList(),
            messages = _messages.ToList(),
            nextId = _nextId,
            apps = _apps.OrderBy(a => a).ToList(),
            systemLog = _log.ToList(),
        };

        public void Restore(DevicesState state)
        {
            _owned.Clear();
            _messages.Clear();
            _apps.Clear();
            _log.Clear();
            Age = state?.age ?? 0;
            _nextId = 1;
            if (state != null)
            {
                foreach (var s in state.owned ?? new List<string>())
                    if (Enum.TryParse<DeviceKind>(s, out var d)) _owned.Add(d);
                _messages.AddRange(state.messages ?? new List<DeviceMessage>());
                foreach (var a in state.apps ?? new List<string>()) if (!string.IsNullOrEmpty(a)) _apps.Add(a);
                _log.AddRange(state.systemLog ?? new List<SystemLogEntry>());
                int maxId = Math.Max(_messages.Count == 0 ? 0 : _messages.Max(m => m.id), _log.Count == 0 ? 0 : _log.Max(e => e.id));
                _nextId = Math.Max(state.nextId, maxId + 1);
            }
            Changed?.Invoke();
        }
    }
}
