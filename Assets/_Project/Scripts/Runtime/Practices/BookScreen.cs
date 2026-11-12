using System;
using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.Practices;
using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Practices
{
    /// <summary>
    /// Maya's Book of Shadows (owner, 2026-11-12): Tanya gives it at Yule, December 2017, and from then on it's an icon on the
    /// side of the screen, under the phone, usable any time (not over the Anchor or an interactive moment). Tabs: Practices
    /// (the steps of each one she has learned, and who taught her), Readings, Wheel of the Year, Dreams, Notes.
    /// Only Maya and Izzy practise Wicca (owner): with anyone else fronting the Book stays shut, with a line in their voice.
    /// Entries are in someone's hand: Izzy's look different from Maya's, but nobody's name is ever shown (owner: lost time is
    /// never pointed out). Built in code; placeholder look until the owner's art.
    /// </summary>
    public sealed class BookScreen : MonoBehaviour
    {
        static readonly Color Ink = new Color(0.20f, 0.14f, 0.10f);
        static readonly Color Faded = new Color(0.45f, 0.38f, 0.30f);
        static readonly Color Page = new Color(0.93f, 0.88f, 0.78f, 0.99f);
        static readonly Color Cover = new Color(0.22f, 0.16f, 0.24f, 0.99f);
        static readonly Color Tab = new Color(0.80f, 0.72f, 0.60f, 1f);
        static readonly Color TabOn = new Color(0.62f, 0.50f, 0.66f, 1f);
        static readonly Color Light = new Color(0.95f, 0.92f, 0.86f);

        const int SortingOrder = 40;
        const int PerPage = 4;

        public static bool IsOpen { get; private set; }

        /// <summary>Set by the story screen: true while the Book shouldn't open (the Anchor, an interactive moment).</summary>
        public Func<bool> Blocked = () => false;
        /// <summary>Set by the story screen: false while replaying a chapter from before Tanya gave it.</summary>
        public Func<bool> Available = () => true;

        Font _font;
        GameObject _icon, _panel, _note;
        Text _title, _noteText;
        RectTransform _tabs, _list, _footer;
        float _noteHideAt;
        BookTab _tab = BookTab.Practices;
        Dictionary<string, string> _names;

        static BookOfShadows B => GameServices.Book;

        void Awake()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _names = LoadNames();
            Build();
        }

        void OnDisable() => IsOpen = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => IsOpen = false;

        void Update()
        {
            var b = B;
            bool show = b != null && b.Has && Available() && !IsOpen && !Blocked() && !Devices.DeviceScreen.IsOpen
                        && Save.SaveService.ActiveSlot != 0 && !Save.SaveSlotsScreen.IsOpen && !Loading.LoadingScreen.IsShowing;
            _icon.SetActive(show);
            if (_note.activeSelf && Time.unscaledTime > _noteHideAt) _note.SetActive(false);
        }

        // ── Opening ──────────────────────────────────────────────────────────────

        public void Open()
        {
            var b = B;
            if (b == null || !b.Has) return;
            string front = GameServices.Roster?.FrontingId ?? "maya";
            if (!b.Practises(front))
            {
                // Owner: the others keep their own faith and leave the Wiccan tools alone. The Book stays shut.
                var def = GameServices.Roster?.GetDefinition(front);
                _noteText.text = $"<b>{(def != null ? def.displayName : front)}:</b> {b.RefusalLine(front)}";
                _note.SetActive(true);
                _noteHideAt = Time.unscaledTime + 3f;
                return;
            }
            IsOpen = true;
            _panel.SetActive(true);
            Show(_tab);
        }

        void Close()
        {
            _panel.SetActive(false);
            IsOpen = false;
        }

        // ── Pages ────────────────────────────────────────────────────────────────

        void Show(BookTab tab, int page = 0)
        {
            _tab = tab;
            Clear(_tabs); Clear(_list); Clear(_footer);
            _title.text = TabTitle(tab);
            foreach (BookTab t in Enum.GetValues(typeof(BookTab)))
            {
                var tt = t;
                Button(_tabs, TabName(t), () => Show(tt), t == tab ? TabOn : Tab, 90, 34);
            }

            int count;
            if (tab == BookTab.Practices) count = ShowPractices(page);
            else if (tab == BookTab.Shelf) count = ShowShelf(page);
            else count = ShowEntries(tab, page);

            if (page > 0) Button(_footer, "Back a page", () => Show(tab, page - 1), Tab, 100);
            if ((page + 1) * PerPage < count) Button(_footer, "Next page", () => Show(tab, page + 1), Tab, 100);
            if (tab != BookTab.Practices && tab != BookTab.Shelf) Button(_footer, "Write", () => ShowWrite(tab), TabOn, 100);
            Button(_footer, "Close the book", Close, TabOn, 100);
        }

        int ShowPractices(int page)
        {
            var learned = B.LearnedPractices();
            if (learned.Count == 0) Label(_list, "Nothing written here yet. Tanya says the first page fills itself.", 40, FontStyle.Italic, Faded, 140);
            foreach (var (p, l) in learned.Skip(page * PerPage).Take(PerPage))
            {
                string who = string.IsNullOrEmpty(l.teacherId) ? "" : $"   <i>learned from {Name(l.teacherId)}</i>";
                string steps = string.Join("\n", p.steps.Select((s, i) => $"{i + 1}. {s}"));
                Label(_list, $"<b>{p.name}</b>{who}\n{steps}", 34, FontStyle.Normal, Ink, 60 + 46 * (p.steps.Count + 1), TextAnchor.UpperLeft);
            }
            return learned.Count;
        }

        /// <summary>
        /// The herbs and stones she has (owner: gained from different people through the game): what each is for in a
        /// charm bag, its folklore or history, and who gave it. Stones' meanings are modern crystal lore, and say so.
        /// </summary>
        int ShowShelf(int page)
        {
            var items = B.Ingredients;
            var lib = GameServices.Practices;
            if (items.Count == 0 || lib == null) { Label(_list, "Nothing yet. People give you things along the way: a sprig, a stone. Keep them here.", 40, FontStyle.Italic, Faded, 140); return 0; }
            foreach (var it in items.Skip(page * PerPage).Take(PerPage))
            {
                string from = string.IsNullOrEmpty(it.giverId) ? "" : $"   <i>from {Name(it.giverId)}</i>";
                string text;
                if (it.kind == "herb")
                {
                    var h = lib.Herbs.FirstOrDefault(x => x.id == it.id);
                    if (h == null) continue;
                    text = $"<b>{h.commonName}</b>{from}\n<i>{h.charmMeaning}</i>\n{h.folklore}";
                }
                else
                {
                    var c = lib.Crystals.FirstOrDefault(x => x.id == it.id);
                    if (c == null) continue;
                    text = $"<b>{c.name}</b>{from}\n<i>{c.charmMeaning}</i> <color=#7a6a58>(crystal lore, not proven)</color>\n{c.history}";
                }
                Label(_list, text, 32, FontStyle.Normal, Ink, 250, TextAnchor.UpperLeft);
            }
            return items.Count;
        }

        int ShowEntries(BookTab tab, int page)
        {
            var entries = B.EntriesIn(tab);
            if (entries.Count == 0) Label(_list, EmptyLine(tab), 40, FontStyle.Italic, Faded, 140);
            foreach (var e in entries.Skip(page * PerPage).Take(PerPage))
            {
                // Each entry in its writer's hand. Nobody is named (owner: lost time is never pointed out).
                var (style, colour) = Hand(e.memberId);
                string title = string.IsNullOrEmpty(e.title) ? "" : $"<b>{e.title}</b>\n";
                Label(_list, $"<color=#7a6a58>Day {Math.Max(1, e.day)}</color>\n{title}{e.text}", 36, style, colour, 190, TextAnchor.UpperLeft);
            }
            return entries.Count;
        }

        /// <summary>Writing in the Book, in whoever is fronting's hand (Maya or Izzy: no one else opens it).</summary>
        void ShowWrite(BookTab tab)
        {
            Clear(_tabs); Clear(_list); Clear(_footer);
            string me = GameServices.Roster?.FrontingId ?? "maya";
            _title.text = TabTitle(tab);

            var titleField = Field(_list, "A title (or leave it blank)", 110, InputField.LineType.SingleLine, 80);
            var body = Field(_list, tab == BookTab.Dreams ? "What did you dream?" : "Write it down...", 520, InputField.LineType.MultiLineNewline, BookOfShadows.MaxEntryLength);

            Button(_footer, "Keep it", () =>
            {
                B.Write(tab, me, titleField.text, body.text, GameServices.InGameDay, GameServices.ChapterRuns?.CurrentChapterId ?? "", byPlayer: true);
                Show(tab);
            }, TabOn, 100);
            Button(_footer, "Not now", () => Show(tab), Tab, 100);
            body.ActivateInputField();
        }

        static string TabName(BookTab t) => t switch
        {
            BookTab.Practices => "Practices",
            BookTab.Readings => "Readings",
            BookTab.Wheel => "The Wheel",
            BookTab.Dreams => "Dreams",
            BookTab.Shelf => "Herbs & stones",
            _ => "Notes",
        };

        static string TabTitle(BookTab t) => t switch
        {
            BookTab.Practices => "Book of Shadows: practices",
            BookTab.Readings => "Book of Shadows: readings",
            BookTab.Wheel => "Book of Shadows: the Wheel of the Year",
            BookTab.Dreams => "Book of Shadows: dreams",
            BookTab.Shelf => "Book of Shadows: herbs and stones",
            _ => "Book of Shadows: notes",
        };

        static string EmptyLine(BookTab t) => t switch
        {
            BookTab.Readings => "No readings yet. Each one goes here: the cards, and what you saw in them.",
            BookTab.Wheel => "Samhain, Yule, Imbolc, Ostara, Beltane, Litha, Lammas, Mabon. Each one you keep goes here.",
            BookTab.Dreams => "Keep the book by the bed. Even one word.",
            _ => "Anything at all.",
        };

        /// <summary>Handwriting: Maya's is plain; Izzy's is looser (italic, a little warmer). Never a name.</summary>
        static (FontStyle, Color) Hand(string memberId) => memberId switch
        {
            "maya" => (FontStyle.Normal, Ink),
            "izzy" => (FontStyle.Italic, new Color(0.33f, 0.16f, 0.30f)),
            _ => (FontStyle.BoldAndItalic, new Color(0.18f, 0.22f, 0.30f)),
        };

        // ── Building the UI ──────────────────────────────────────────────────────

        void Build()
        {
            var canvasGo = new GameObject("Book Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
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

            // The icon: right edge, under the phone and laptop, above the Use button.
            _icon = new GameObject("Book Icon", typeof(RectTransform), typeof(Image), typeof(Button));
            _icon.transform.SetParent(root, false);
            var rt = _icon.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.985f, 0.30f);
            rt.pivot = new Vector2(1f, 0.5f);
            rt.sizeDelta = new Vector2(230, 300);
            _icon.GetComponent<Image>().color = new Color(0.16f, 0.14f, 0.18f, 0.82f);
            _icon.GetComponent<Button>().onClick.AddListener(Open);
            var pic = new GameObject("Icon", typeof(RectTransform), typeof(Image));
            pic.transform.SetParent(_icon.transform, false);
            var prt = pic.GetComponent<RectTransform>();
            prt.anchorMin = new Vector2(0.1f, 0.3f); prt.anchorMax = new Vector2(0.9f, 0.95f);
            prt.offsetMin = prt.offsetMax = Vector2.zero;
            var img = pic.GetComponent<Image>();
            img.sprite = BookSprite();
            img.preserveAspect = true;
            img.raycastTarget = false;
            var label = MakeText(_icon.transform, "Book", 36, FontStyle.Bold, Light, TextAnchor.MiddleCenter);
            Place(label.rectTransform, new Vector2(0f, 0.02f), new Vector2(1f, 0.3f));
            _icon.SetActive(false);

            // The refusal note (someone else is fronting): a short line, never takes touches.
            _note = Panel(root, "Book Note", new Color(0.12f, 0.12f, 0.15f, 0.95f), new Vector2(0.3f, 0.70f), new Vector2(0.7f, 0.78f));
            _note.GetComponent<Image>().raycastTarget = false;
            _noteText = MakeText(_note.transform, "", 38, FontStyle.Normal, Light, TextAnchor.MiddleCenter);
            Stretch(_noteText.rectTransform, 24);
            _note.SetActive(false);

            // The open book: a cover with a page inside it.
            _panel = Panel(root, "Book of Shadows", Cover, new Vector2(0.18f, 0.04f), new Vector2(0.82f, 0.90f));
            var page = Panel(_panel.transform, "Page", Page, new Vector2(0.025f, 0.025f), new Vector2(0.975f, 0.975f));
            _title = MakeText(page.transform, "", 50, FontStyle.Bold, Ink, TextAnchor.MiddleCenter);
            Place(_title.rectTransform, new Vector2(0.04f, 0.9f), new Vector2(0.96f, 0.98f));
            _tabs = Column(page.transform, "Tabs", new Vector2(0.04f, 0.82f), new Vector2(0.96f, 0.89f), horizontal: true);
            _list = Column(page.transform, "Entries", new Vector2(0.06f, 0.14f), new Vector2(0.94f, 0.80f), horizontal: false);
            _footer = Column(page.transform, "Footer", new Vector2(0.06f, 0.02f), new Vector2(0.94f, 0.12f), horizontal: true);
            _panel.SetActive(false);
        }

        static Sprite _bookIcon;

        /// <summary>A closed book with a crescent moon on the cover and a ribbon (art can replace it).</summary>
        static Sprite BookSprite()
        {
            if (_bookIcon != null) return _bookIcon;
            const int size = 128;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[size * size];
            var on = new Color32(240, 232, 220, 255);
            void Fill(int x0, int y0, int x1, int y1) { for (int y = y0; y <= y1; y++) for (int x = x0; x <= x1; x++) px[y * size + x] = on; }
            void Frame(int x0, int y0, int x1, int y1, int w) { Fill(x0, y0, x1, y0 + w); Fill(x0, y1 - w, x1, y1); Fill(x0, y0, x0 + w, y1); Fill(x1 - w, y0, x1, y1); }
            Frame(28, 10, 100, 118, 6);   // the cover
            Fill(36, 10, 40, 118);        // the spine line
            Fill(78, 0, 84, 30);          // the ribbon
            // A small crescent moon on the cover.
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float a = (x - 68f) * (x - 68f) + (y - 70f) * (y - 70f), b = (x - 76f) * (x - 76f) + (y - 74f) * (y - 74f);
                    if (a < 18f * 18f && b > 15f * 15f) px[y * size + x] = on;
                }
            tex.SetPixels32(px);
            tex.Apply(false, true);
            _bookIcon = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
            return _bookIcon;
        }

        InputField Field(Transform parent, string hint, float height, InputField.LineType lines, int limit)
        {
            var go = new GameObject("Field", typeof(RectTransform), typeof(Image), typeof(InputField), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            go.GetComponent<LayoutElement>().preferredHeight = height;
            go.GetComponent<Image>().color = new Color(0.98f, 0.95f, 0.88f);
            var text = MakeText(go.transform, "", 38, FontStyle.Normal, Ink, TextAnchor.UpperLeft);
            Place(text.rectTransform, Vector2.zero, Vector2.one);
            text.rectTransform.offsetMin = new Vector2(24, 16); text.rectTransform.offsetMax = new Vector2(-24, -16);
            text.supportRichText = false;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            var ph = MakeText(go.transform, hint, 38, FontStyle.Italic, new Color(Ink.r, Ink.g, Ink.b, 0.45f), TextAnchor.UpperLeft);
            Place(ph.rectTransform, Vector2.zero, Vector2.one);
            ph.rectTransform.offsetMin = new Vector2(24, 16); ph.rectTransform.offsetMax = new Vector2(-24, -16);
            var input = go.GetComponent<InputField>();
            input.textComponent = text;
            input.placeholder = ph;
            input.lineType = lines;
            input.characterLimit = limit;
            return input;
        }

        RectTransform Column(Transform parent, string name, Vector2 min, Vector2 max, bool horizontal)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            Place(rt, min, max);
            HorizontalOrVerticalLayoutGroup g = horizontal ? go.AddComponent<HorizontalLayoutGroup>() : (HorizontalOrVerticalLayoutGroup)go.AddComponent<VerticalLayoutGroup>();
            g.spacing = horizontal ? 18 : 12;
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

        void Button(Transform parent, string label, Action onClick, Color color, float height, int size = 40)
        {
            var go = new GameObject("Button", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            go.GetComponent<Image>().color = color;
            go.GetComponent<LayoutElement>().preferredHeight = height;
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height);
            go.GetComponent<Button>().onClick.AddListener(() => onClick());
            var t = MakeText(go.transform, label, size, FontStyle.Normal, Ink, TextAnchor.MiddleCenter);
            Stretch(t.rectTransform, 12);
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
                child.SetParent(null, false);
                Destroy(child.gameObject);
            }
        }

        string Name(string id) => _names.TryGetValue(id ?? "", out var n) ? n : id;

        [Serializable] internal sealed class PeopleFile { public List<PersonName> people = new List<PersonName>(); }
        [Serializable] internal sealed class PersonName { public string id; public string name; }

        /// <summary>People's display names by id (people.json). Also used by the ritual screen ("from Selene").</summary>
        internal static Dictionary<string, string> LoadNames()
        {
            var d = new Dictionary<string, string>();
            foreach (var p in DataLoader.Load<PeopleFile>("people").people ?? new List<PersonName>())
                if (!string.IsNullOrEmpty(p.id)) d[p.id] = p.name;
            return d;
        }
    }
}
