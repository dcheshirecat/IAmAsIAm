using System;
using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.Devices;
using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Devices
{
    /// <summary>
    /// Maya's phone (texts), and the family computer or laptop (email). Built in code, like the other screens.
    /// - The Phone button (top, beside Saves) appears once she has a phone; with a laptop it also shows email.
    /// - The family computer stays at home: the story opens it (# device: computer) when she sits at it.
    /// - An incoming message shows a short banner. Messages written during lost time arrive silently and look
    ///   exactly like her own (owner: never pointed out).
    /// Placeholder look: final UI art replaces these panels.
    /// </summary>
    public sealed class DeviceScreen : MonoBehaviour
    {
        static readonly Color TextColor = new Color(0.95f, 0.92f, 0.86f);
        static readonly Color Dim = new Color(0.72f, 0.68f, 0.64f);
        static readonly Color PhoneColor = new Color(0.07f, 0.07f, 0.09f, 0.98f);
        static readonly Color ButtonColor = new Color(0.28f, 0.22f, 0.30f, 0.95f);
        static readonly Color QuietButtonColor = new Color(0.2f, 0.18f, 0.22f, 0.95f);
        static readonly Color TheirBubble = new Color(0.22f, 0.22f, 0.26f, 1f);
        static readonly Color MyBubble = new Color(0.30f, 0.38f, 0.55f, 1f);

        /// <summary>Above the story (0), below the saves screen (50), the loading card (100) and the Anchor (200).</summary>
        const int SortingOrder = 40;
        const int PageSize = 6;

        public static bool IsOpen { get; private set; }

        /// <summary>Set by the story screen: true while the player shouldn't open the phone (dialogue, the Anchor...).</summary>
        public Func<bool> Blocked = () => false;

        Font _font;
        GameObject _phoneButton, _panel, _banner;
        Text _phoneLabel, _title, _bannerText;
        RectTransform _tabs, _list, _footer;
        float _bannerHideAt;
        Action _onClosed;
        DeviceKind _openOn;
        MessageChannel _channel;
        Dictionary<string, string> _names;

        IAmAsIAm.Core.Devices.Devices D => GameServices.Devices;

        void Awake()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _names = LoadNames();
            Build();
        }

        void OnEnable() => GameServices.Devices.Received += OnReceived;
        void OnDisable()
        {
            if (GameServices.Devices != null) GameServices.Devices.Received -= OnReceived;
            IsOpen = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => IsOpen = false;

        void Update()
        {
            var d = D;
            bool canOpen = d != null && d.CanText && !IsOpen && !Blocked()
                           && Save.SaveService.ActiveSlot != 0 && !Save.SaveSlotsScreen.IsOpen && !Loading.LoadingScreen.IsShowing;
            _phoneButton.SetActive(canOpen);
            if (canOpen)
            {
                int unread = d.UnreadCount(MessageChannel.Text) + (d.CanEmailAnywhere ? d.UnreadCount(MessageChannel.Email) : 0);
                _phoneLabel.text = unread > 0 ? $"Phone ({unread})" : "Phone";
            }
            if (_banner.activeSelf && Time.unscaledTime > _bannerHideAt) _banner.SetActive(false);
        }

        void OnReceived(DeviceMessage m)
        {
            var d = D;
            if (d == null || !d.CanSee(m.Channel)) return;
            // Email on the family computer is only seen when she sits at it: no banner for that.
            if (m.Channel == MessageChannel.Email && !d.CanEmailAnywhere) return;
            string who = Name(m.contactId);
            _bannerText.text = m.Channel == MessageChannel.Text ? $"<b>{who}</b>  {Shorten(m.body, 60)}" : $"<b>Email from {who}</b>  {Shorten(m.subject, 50)}";
            _banner.SetActive(true);
            _bannerHideAt = Time.unscaledTime + 3.5f;
        }

        // ── Opening ──────────────────────────────────────────────────────────────

        /// <summary>Opens a device. <paramref name="onClosed"/> runs when the player closes it (the story waits for it).</summary>
        public void Open(DeviceKind kind, Action onClosed = null)
        {
            var d = D;
            if (d == null) { onClosed?.Invoke(); return; }
            d.Gain(kind); // A story that sits her at a device means she has it.
            _openOn = kind;
            _onClosed = onClosed;
            IsOpen = true;
            _panel.SetActive(true);
            _banner.SetActive(false);
            ShowList(kind == DeviceKind.Phone ? MessageChannel.Text : MessageChannel.Email);
        }

        void Close()
        {
            _panel.SetActive(false);
            IsOpen = false;
            var done = _onClosed;
            _onClosed = null;
            done?.Invoke();
        }

        IEnumerable<MessageChannel> Channels()
        {
            var d = D;
            if (_openOn == DeviceKind.Phone)
            {
                yield return MessageChannel.Text;
                if (d.CanEmailAnywhere) yield return MessageChannel.Email;
            }
            else yield return MessageChannel.Email;
        }

        string DeviceName() => _openOn switch
        {
            DeviceKind.Computer => "The family computer",
            DeviceKind.Laptop => "Laptop",
            _ => "Phone",
        };

        // ── The thread list ──────────────────────────────────────────────────────

        void ShowList(MessageChannel channel, int page = 0)
        {
            _channel = channel;
            Clear(_tabs); Clear(_list); Clear(_footer);
            _title.text = DeviceName() + (channel == MessageChannel.Email ? ": Email" : ": Messages");

            var channels = Channels().ToList();
            if (channels.Count > 1)
                foreach (var c in channels)
                {
                    var ch = c;
                    Button(_tabs, c == MessageChannel.Text ? "Messages" : "Email", () => ShowList(ch), c == channel ? ButtonColor : QuietButtonColor, 90);
                }

            var threads = D.Threads(channel);
            if (threads.Count == 0) Label(_list, "Nothing here yet.", 42, FontStyle.Italic, Dim, 90);
            foreach (var t in threads.Skip(page * PageSize).Take(PageSize))
            {
                var thread = t;
                var latest = t.Latest;
                string preview = channel == MessageChannel.Email ? (string.IsNullOrEmpty(latest.subject) ? "(no subject)" : latest.subject) : latest.body;
                string mark = t.Unread > 0 ? "●  " : "";
                Button(_list, $"{mark}<b>{Name(t.ContactId)}</b>\n<color=#bfb6a8>{Shorten(preview, 48)}</color>", () => ShowThread(thread.ContactId, channel), QuietButtonColor, 130, TextAnchor.MiddleLeft);
            }

            if (page > 0) Button(_footer, "Newer", () => ShowList(channel, page - 1), QuietButtonColor, 100);
            if ((page + 1) * PageSize < threads.Count) Button(_footer, "Older", () => ShowList(channel, page + 1), QuietButtonColor, 100);
            Button(_footer, "Close", Close, ButtonColor, 100);
        }

        // ── One conversation / correspondent ─────────────────────────────────────

        void ShowThread(string contactId, MessageChannel channel, int page = 0)
        {
            Clear(_tabs); Clear(_list); Clear(_footer);
            D.MarkRead(channel, contactId);
            _title.text = Name(contactId);

            var thread = D.Threads(channel).FirstOrDefault(t => t.ContactId == contactId);
            var all = thread?.Messages ?? new List<DeviceMessage>();
            int perPage = channel == MessageChannel.Email ? 1 : PageSize;
            // Newest at the bottom; page 0 = the newest ones.
            int end = Math.Max(0, all.Count - page * perPage);
            int start = Math.Max(0, end - perPage);
            for (int i = start; i < end; i++)
            {
                var m = all[i];
                if (channel == MessageChannel.Email)
                {
                    string from = m.outgoing ? "To " + Name(contactId) : "From " + Name(contactId);
                    string subject = string.IsNullOrEmpty(m.subject) ? "(no subject)" : m.subject;
                    Label(_list, $"<b>{subject}</b>\n<color=#bfb6a8>{from} · Day {Math.Max(1, m.day)}</color>\n\n{m.body}", 40, FontStyle.Normal, TextColor, 620, TextAnchor.UpperLeft);
                }
                else Bubble(_list, m.body, m.outgoing);
            }

            if (start > 0) Button(_footer, "Earlier", () => ShowThread(contactId, channel, page + 1), QuietButtonColor, 100);
            if (page > 0) Button(_footer, "Later", () => ShowThread(contactId, channel, page - 1), QuietButtonColor, 100);
            Button(_footer, "Back", () => ShowList(channel), ButtonColor, 100);
        }

        void Bubble(Transform parent, string text, bool mine)
        {
            var row = new GameObject("Row", typeof(RectTransform), typeof(LayoutElement), typeof(HorizontalLayoutGroup));
            row.transform.SetParent(parent, false);
            row.GetComponent<LayoutElement>().preferredHeight = 110;
            var h = row.GetComponent<HorizontalLayoutGroup>();
            h.childAlignment = mine ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
            h.childControlWidth = false; h.childControlHeight = true; h.childForceExpandWidth = false; h.childForceExpandHeight = true;

            var bubble = new GameObject("Bubble", typeof(RectTransform), typeof(Image));
            bubble.transform.SetParent(row.transform, false);
            bubble.GetComponent<Image>().color = mine ? MyBubble : TheirBubble;
            bubble.GetComponent<Image>().raycastTarget = false;
            bubble.GetComponent<RectTransform>().sizeDelta = new Vector2(900, 100);
            var t = MakeText(bubble.transform, text, 38, FontStyle.Normal, TextColor, TextAnchor.MiddleLeft);
            var rt = t.rectTransform;
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(24, 8); rt.offsetMax = new Vector2(-24, -8);
            t.resizeTextForBestFit = true; t.resizeTextMinSize = 24; t.resizeTextMaxSize = 38;
        }

        // ── Building the UI ──────────────────────────────────────────────────────

        void Build()
        {
            var canvasGo = new GameObject("Device Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2732, 2048);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;
            var root = canvasGo.transform;

            // The Phone button: just left of Saves (Saves 0.73-0.85, Anchor 0.86-0.98).
            _phoneButton = Panel(root, "Phone Button", QuietButtonColor, new Vector2(0.60f, 0.9f), new Vector2(0.72f, 0.98f));
            _phoneButton.AddComponent<Button>().onClick.AddListener(() => Open(DeviceKind.Phone));
            _phoneLabel = MakeText(_phoneButton.transform, "Phone", 44, FontStyle.Bold, TextColor, TextAnchor.MiddleCenter);
            Stretch(_phoneLabel.rectTransform);
            _phoneButton.SetActive(false);

            // A new-message banner (top middle). It never takes touches.
            _banner = Panel(root, "Message Banner", new Color(0.12f, 0.12f, 0.15f, 0.95f), new Vector2(0.3f, 0.78f), new Vector2(0.7f, 0.86f));
            _banner.GetComponent<Image>().raycastTarget = false;
            _bannerText = MakeText(_banner.transform, "", 38, FontStyle.Normal, TextColor, TextAnchor.MiddleLeft);
            Stretch(_bannerText.rectTransform, 24);
            _banner.SetActive(false);

            // The device itself.
            _panel = Panel(root, "Device", PhoneColor, new Vector2(0.27f, 0.04f), new Vector2(0.73f, 0.88f));
            _title = MakeText(_panel.transform, "", 52, FontStyle.Bold, TextColor, TextAnchor.MiddleCenter);
            Place(_title.rectTransform, new Vector2(0.04f, 0.9f), new Vector2(0.96f, 0.98f));
            _tabs = Column(_panel.transform, "Tabs", new Vector2(0.06f, 0.82f), new Vector2(0.94f, 0.89f), horizontal: true);
            _list = Column(_panel.transform, "List", new Vector2(0.05f, 0.14f), new Vector2(0.95f, 0.81f), horizontal: false);
            _footer = Column(_panel.transform, "Footer", new Vector2(0.06f, 0.02f), new Vector2(0.94f, 0.12f), horizontal: true);
            _panel.SetActive(false);
        }

        RectTransform Column(Transform parent, string name, Vector2 min, Vector2 max, bool horizontal)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            Place(rt, min, max);
            HorizontalOrVerticalLayoutGroup g = horizontal ? go.AddComponent<HorizontalLayoutGroup>() : (HorizontalOrVerticalLayoutGroup)go.AddComponent<VerticalLayoutGroup>();
            g.spacing = horizontal ? 24 : 14;
            g.childControlWidth = true; g.childForceExpandWidth = true;
            g.childControlHeight = !horizontal; g.childForceExpandHeight = false;
            g.childAlignment = horizontal ? TextAnchor.MiddleCenter : TextAnchor.UpperCenter;
            return rt;
        }

        GameObject Panel(Transform parent, string name, Color color, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            Place(go.GetComponent<RectTransform>(), min, max);
            go.GetComponent<Image>().color = color;
            return go;
        }

        void Button(Transform parent, string label, Action onClick, Color color, float height, TextAnchor align = TextAnchor.MiddleCenter)
        {
            var go = new GameObject("Button", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            go.GetComponent<Image>().color = color;
            go.GetComponent<LayoutElement>().preferredHeight = height;
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height);
            go.GetComponent<Button>().onClick.AddListener(() => onClick());
            var t = MakeText(go.transform, label, 40, FontStyle.Normal, TextColor, align);
            Stretch(t.rectTransform, 24);
        }

        void Label(Transform parent, string text, int size, FontStyle style, Color color, float height, TextAnchor align = TextAnchor.MiddleCenter)
        {
            var go = new GameObject("Label", typeof(RectTransform), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            go.GetComponent<LayoutElement>().preferredHeight = height;
            var t = MakeText(go.transform, text, size, style, color, align);
            Stretch(t.rectTransform, 12);
        }

        Text MakeText(Transform parent, string text, int size, FontStyle style, Color color, TextAnchor align)
        {
            var go = new GameObject("Text", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var t = go.GetComponent<Text>();
            t.font = _font; t.fontSize = size; t.fontStyle = style; t.color = color; t.alignment = align;
            t.supportRichText = true; t.raycastTarget = false;
            t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Truncate;
            t.text = text;
            return t;
        }

        static void Place(RectTransform rt, Vector2 min, Vector2 max)
        {
            rt.anchorMin = min; rt.anchorMax = max; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        }

        static void Stretch(RectTransform rt, float pad = 0)
        {
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(pad, 0); rt.offsetMax = new Vector2(-pad, 0);
        }

        static void Clear(Transform t)
        {
            for (int i = t.childCount - 1; i >= 0; i--)
            {
                var child = t.GetChild(i);
                child.SetParent(null, false); // Leave the layout now; Destroy happens at frame end.
                Destroy(child.gameObject);
            }
        }

        static string Shorten(string s, int max) => string.IsNullOrEmpty(s) ? "" : s.Length <= max ? s : s.Substring(0, max - 1) + "…";

        string Name(string contactId) => _names.TryGetValue(contactId ?? "", out var n) ? n : contactId;

        [Serializable] sealed class PeopleFile { public List<PersonName> people = new List<PersonName>(); }
        [Serializable] sealed class PersonName { public string id; public string name; }

        static Dictionary<string, string> LoadNames()
        {
            var d = new Dictionary<string, string>();
            foreach (var p in DataLoader.Load<PeopleFile>("people").people ?? new List<PersonName>())
                if (!string.IsNullOrEmpty(p.id)) d[p.id] = p.name;
            return d;
        }
    }
}
