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
        /// <summary>The phone's Maps tab (owner: the map lives on Maya's phone). Set by the story screen.</summary>
        public Func<bool> MapAvailable = () => false;
        public Action ShowMap;
        /// <summary>
        /// SystemLog (owner: unlocked at the end of Chapter 4) is on the phone once gained, but not while replaying a chapter from
        /// before it. Set by the story screen.
        /// </summary>
        public Func<bool> SystemLogAvailable = () => true;

        Font _font;
        GameObject _phoneButton, _laptopButton, _panel, _banner;
        Text _phoneLabel, _laptopLabel, _title, _bannerText;
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
            // Owner: the phone and the laptop are always-usable icons on the side of the screen (once she has them),
            // not only something the story offers.
            bool usable = d != null && !IsOpen && !Blocked() && !IAmAsIAm.Runtime.Practices.BookScreen.IsOpen
                          && Save.SaveService.ActiveSlot != 0 && !Save.SaveSlotsScreen.IsOpen && !Loading.LoadingScreen.IsShowing;
            bool phone = usable && d.CanText, laptop = usable && d.Has(DeviceKind.Laptop);
            _phoneButton.SetActive(phone);
            _laptopButton.SetActive(laptop);
            if (phone)
            {
                int unread = d.UnreadCount(MessageChannel.Text);
                _phoneLabel.text = unread > 0 ? $"Phone ({unread})" : "Phone";
            }
            if (laptop)
            {
                int unread = d.UnreadCount(MessageChannel.Email);
                _laptopLabel.text = unread > 0 ? $"Laptop ({unread})" : "Laptop";
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
            // Maps, on the phone, when she's somewhere with a map (not while the story is waiting on this phone moment).
            bool maps = _openOn == DeviceKind.Phone && _onClosed == null && ShowMap != null && MapAvailable();
            if (channels.Count > 1 || maps || LogTab())
                foreach (var c in channels)
                {
                    var ch = c;
                    Button(_tabs, c == MessageChannel.Text ? "Messages" : "Email", () => ShowList(ch), c == channel ? ButtonColor : QuietButtonColor, 90);
                }
            if (maps) Button(_tabs, "Maps", () => { Close(); ShowMap(); }, QuietButtonColor, 90);
            if (LogTab()) Button(_tabs, "SystemLog", () => ShowLog(), QuietButtonColor, 90);

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

        // ── SystemLog: notes the system leaves for each other (owner) ────────────

        bool LogTab() => _openOn == DeviceKind.Phone && D.HasApp(IAmAsIAm.Core.Devices.Devices.SystemLogApp) && SystemLogAvailable();

        /// <summary>The notes, newest first, each signed by who wrote it in their colour; and a way to write one.</summary>
        void ShowLog(int page = 0)
        {
            Clear(_tabs); Clear(_list); Clear(_footer);
            _title.text = "SystemLog";
            var notes = D.SystemLog.Reverse().ToList();
            if (notes.Count == 0) Label(_list, "No notes yet. Leave one for whoever comes next.", 40, FontStyle.Italic, Dim, 120);
            const int perPage = 5;
            foreach (var n in notes.Skip(page * perPage).Take(perPage))
            {
                var def = GameServices.Roster?.GetDefinition(n.memberId);
                string who = def != null ? def.displayName : n.memberId;
                string colour = def != null && !string.IsNullOrEmpty(def.colorHex) ? def.colorHex : "#bfb6a8";
                Label(_list, $"<color={colour}><b>{who}</b></color>  <color=#bfb6a8>Day {Math.Max(1, n.day)}</color>\n{n.text}", 38, FontStyle.Normal, TextColor, 150, TextAnchor.UpperLeft);
            }
            if (page > 0) Button(_footer, "Newer", () => ShowLog(page - 1), QuietButtonColor, 100);
            if ((page + 1) * perPage < notes.Count) Button(_footer, "Older", () => ShowLog(page + 1), QuietButtonColor, 100);
            Button(_footer, "Write", ShowWrite, ButtonColor, 100);
            Button(_footer, "Back", () => ShowList(MessageChannel.Text), QuietButtonColor, 100);
        }

        /// <summary>Writing a note, signed by whoever is fronting right now.</summary>
        void ShowWrite()
        {
            Clear(_tabs); Clear(_list); Clear(_footer);
            var roster = GameServices.Roster;
            string me = roster?.FrontingId ?? "maya";
            var def = roster?.GetDefinition(me);
            _title.text = "SystemLog: a note from " + (def != null ? def.displayName : me);

            var go = new GameObject("Note", typeof(RectTransform), typeof(Image), typeof(InputField), typeof(LayoutElement));
            go.transform.SetParent(_list, false);
            go.GetComponent<LayoutElement>().preferredHeight = 420;
            go.GetComponent<Image>().color = new Color(0.95f, 0.93f, 0.9f);
            var text = MakeText(go.transform, "", 40, FontStyle.Normal, new Color(0.1f, 0.1f, 0.12f), TextAnchor.UpperLeft);
            Place(text.rectTransform, Vector2.zero, Vector2.one);
            text.rectTransform.offsetMin = new Vector2(24, 20); text.rectTransform.offsetMax = new Vector2(-24, -20);
            text.supportRichText = false;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            var hint = MakeText(go.transform, "Write a note for the others...", 40, FontStyle.Italic, new Color(0.1f, 0.1f, 0.12f, 0.45f), TextAnchor.UpperLeft);
            Place(hint.rectTransform, Vector2.zero, Vector2.one);
            hint.rectTransform.offsetMin = new Vector2(24, 20); hint.rectTransform.offsetMax = new Vector2(-24, -20);
            var input = go.GetComponent<InputField>();
            input.textComponent = text;
            input.placeholder = hint;
            input.lineType = InputField.LineType.MultiLineNewline;
            input.characterLimit = IAmAsIAm.Core.Devices.Devices.MaxLogLength;

            Button(_footer, "Post", () =>
            {
                D.Log(me, input.text, GameServices.InGameDay, GameServices.ChapterRuns?.CurrentChapterId ?? "", byPlayer: true);
                ShowLog();
            }, ButtonColor, 100);
            Button(_footer, "Cancel", () => ShowLog(), QuietButtonColor, 100);
            input.ActivateInputField();
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

            // The phone and laptop icons: the right edge of the screen, above the Use button.
            _phoneButton = SideIcon(root, "Phone Icon", phone: true, y: 0.68f, out _phoneLabel);
            _phoneButton.GetComponent<Button>().onClick.AddListener(() => Open(DeviceKind.Phone));
            _laptopButton = SideIcon(root, "Laptop Icon", phone: false, y: 0.49f, out _laptopLabel);
            _laptopButton.GetComponent<Button>().onClick.AddListener(() => Open(DeviceKind.Laptop));

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

        /// <summary>A round-cornered tile on the right edge with a drawn icon and a name under it (with unread counts).</summary>
        GameObject SideIcon(Transform root, string name, bool phone, float y, out Text label)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(root, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.985f, y);
            rt.pivot = new Vector2(1f, 0.5f);
            rt.sizeDelta = new Vector2(230, 300);
            go.GetComponent<Image>().color = new Color(0.16f, 0.14f, 0.18f, 0.82f);

            var icon = new GameObject("Icon", typeof(RectTransform), typeof(Image));
            icon.transform.SetParent(go.transform, false);
            var irt = icon.GetComponent<RectTransform>();
            irt.anchorMin = new Vector2(0.1f, 0.3f); irt.anchorMax = new Vector2(0.9f, 0.95f);
            irt.offsetMin = irt.offsetMax = Vector2.zero;
            var img = icon.GetComponent<Image>();
            img.sprite = IconSprite(phone);
            img.preserveAspect = true;
            img.raycastTarget = false;

            label = MakeText(go.transform, phone ? "Phone" : "Laptop", 36, FontStyle.Bold, TextColor, TextAnchor.MiddleCenter);
            Place(label.rectTransform, new Vector2(0f, 0.02f), new Vector2(1f, 0.3f));
            go.SetActive(false);
            return go;
        }

        static Sprite _phoneIcon, _laptopIcon;

        /// <summary>Simple line icons made in code (art can replace them later): a phone, or a laptop.</summary>
        static Sprite IconSprite(bool phone)
        {
            if (phone && _phoneIcon != null) return _phoneIcon;
            if (!phone && _laptopIcon != null) return _laptopIcon;
            const int size = 128;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[size * size];
            var on = new Color32(240, 232, 220, 255);
            void Fill(int x0, int y0, int x1, int y1) { for (int y = y0; y <= y1; y++) for (int x = x0; x <= x1; x++) px[y * size + x] = on; }
            void Frame(int x0, int y0, int x1, int y1, int w) { Fill(x0, y0, x1, y0 + w); Fill(x0, y1 - w, x1, y1); Fill(x0, y0, x0 + w, y1); Fill(x1 - w, y0, x1, y1); }
            if (phone)
            {
                Frame(36, 8, 92, 120, 6);          // the body
                Fill(56, 14, 72, 18);              // the home bar
                Fill(56, 110, 72, 113);            // the speaker
            }
            else
            {
                Frame(22, 44, 106, 108, 6);        // the screen
                Fill(8, 30, 120, 40);              // the keyboard base
                Fill(52, 34, 76, 36);              // the hinge notch
            }
            tex.SetPixels32(px);
            tex.Apply(false, true);
            var sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
            if (phone) _phoneIcon = sprite; else _laptopIcon = sprite;
            return sprite;
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
