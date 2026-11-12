using System;
using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.Story;
using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Story
{
    /// <summary>
    /// The chapter hub (owner, 2026-11-12: "a hub with a tree-like chart that branches out so you can always go back and replay
    /// any chapter you've unlocked with any alter you've unlocked"). A trunk grows up from the Prologue at the bottom; each
    /// chapter is a node on it. A finished chapter branches out into one leaf per alter who can start it (everyone unlocked so
    /// far): a full leaf for someone it's been played as, a faint one for someone it hasn't yet. The next chapter glows. Later
    /// chapters are dim and closed (in test builds they can be opened too).
    /// </summary>
    public sealed class ChapterTreeScreen : MonoBehaviour
    {
        const int SortingOrder = 30; // Above the story, under the phone (40), the map (45) and the Anchor (200).
        const float RowHeight = 300f;
        static readonly Color TextColor = new Color(0.95f, 0.92f, 0.86f);
        static readonly Color Dim = new Color(0.6f, 0.57f, 0.55f);
        static readonly Color Trunk = new Color(0.45f, 0.36f, 0.28f);
        static readonly Color ButtonColor = new Color(0.28f, 0.22f, 0.30f, 0.95f);

        public static bool IsOpen { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => IsOpen = false;

        Font _font;
        GameObject _panel;
        RectTransform _content;
        ScrollRect _scroll;
        Action<string, string> _onPick;
        Action _onClose;
        float _t;
        readonly List<Image> _glowing = new List<Image>();

        void Awake()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Build();
        }

        void OnDisable() => IsOpen = false;

        /// <summary>
        /// Shows the tree. <paramref name="onPick"/> gets (chapter id, who to start as). <paramref name="allowAll"/> opens every
        /// chapter (test builds).
        /// </summary>
        public void Show(IList<ChapterDefinition> chapters, ChapterRuns runs, Func<string, (string name, Color colour)> member,
            bool allowAll, Action<string, string> onPick, Action onClose)
        {
            _onPick = onPick;
            _onClose = onClose;
            _glowing.Clear();
            for (int i = _content.childCount - 1; i >= 0; i--) Destroy(_content.GetChild(i).gameObject);

            var next = chapters.FirstOrDefault(c => runs == null || !runs.HasFinished(c.id));
            // Newest at the top, the Prologue at the bottom: the tree grows up.
            for (int i = chapters.Count - 1; i >= 0; i--)
            {
                var ch = chapters[i];
                bool finished = runs != null && runs.HasFinished(ch.id);
                bool isNext = ch == next;
                Row(ch, finished, isNext, allowAll, runs, member, i == 0, i == chapters.Count - 1);
            }

            _panel.SetActive(true);
            IsOpen = true;
            Canvas.ForceUpdateCanvases();
            _scroll.verticalNormalizedPosition = 0f; // Start at the roots.
        }

        public void Close()
        {
            _panel.SetActive(false);
            IsOpen = false;
        }

        void Update()
        {
            if (!IsOpen) return;
            _t += Time.unscaledDeltaTime;
            // The next chapter breathes slowly (no flashing).
            float a = 0.75f + 0.25f * Mathf.Sin(_t * 1.6f);
            foreach (var img in _glowing) if (img != null) { var c = img.color; c.a = a; img.color = c; }
        }

        void Row(ChapterDefinition ch, bool finished, bool isNext, bool allowAll, ChapterRuns runs,
            Func<string, (string name, Color colour)> member, bool isRoot, bool isTop)
        {
            var row = new GameObject("Row: " + ch.id, typeof(RectTransform), typeof(LayoutElement));
            row.transform.SetParent(_content, false);
            row.GetComponent<LayoutElement>().preferredHeight = RowHeight;
            var rt = row.GetComponent<RectTransform>();

            // The trunk through this row (not above the newest chapter, not below the Prologue).
            var trunk = Img(row.transform, "Trunk", Trunk, null);
            var trt = trunk.rectTransform;
            trt.anchorMin = new Vector2(0.38f, isRoot ? 0.5f : 0f);
            trt.anchorMax = new Vector2(0.38f, isTop ? 0.5f : 1f);
            trt.sizeDelta = new Vector2(22f, 0f);

            // The node.
            bool open = finished || isNext || allowAll;
            Color nodeColour = finished ? new Color(0.85f, 0.75f, 0.45f) : isNext ? new Color(0.98f, 0.86f, 0.5f) : new Color(0.35f, 0.32f, 0.3f);
            var node = Img(row.transform, "Node", nodeColour, Circle());
            var nrt = node.rectTransform;
            nrt.anchorMin = nrt.anchorMax = new Vector2(0.38f, 0.5f);
            nrt.sizeDelta = isNext ? new Vector2(150f, 150f) : new Vector2(120f, 120f);
            if (isNext) _glowing.Add(node);
            if (open && !finished)
            {
                var b = node.gameObject.AddComponent<Button>();
                string id = ch.id, starter = ch.startingMemberId;
                b.onClick.AddListener(() => Pick(id, starter));
            }

            // The title, left of the trunk.
            string title = string.IsNullOrEmpty(ch.title) ? ch.id : ch.title.Replace(" (title to come)", "");
            string state = finished ? "Played" : isNext ? "Next: tap to begin" : allowAll ? "Test: tap to begin" : "Not yet";
            var label = Txt(row.transform, $"<b>{title}</b>\n<size=34>{state}</size>", 44, open ? TextColor : Dim, TextAnchor.MiddleRight);
            Anchor(label.rectTransform, new Vector2(0.02f, 0.1f), new Vector2(0.34f, 0.9f));

            if (!finished || runs == null) return;

            // The branches: one leaf per alter who can start it. Full = played as them; faint = not yet.
            var starters = runs.StartersFor(ch.id, ch.startingMemberId);
            var played = runs.PlayedAs(ch.id);
            var branch = Img(row.transform, "Branch", Trunk, null);
            var brt = branch.rectTransform;
            brt.anchorMin = new Vector2(0.38f, 0.5f);
            brt.anchorMax = new Vector2(0.38f + Mathf.Min(0.58f, 0.075f * starters.Count + 0.02f), 0.5f);
            brt.sizeDelta = new Vector2(0f, 12f);
            for (int i = 0; i < starters.Count; i++)
            {
                string who = starters[i];
                var (name, colour) = member(who);
                bool done = played.Contains(who);
                float x = 0.43f + 0.075f * i;
                var leaf = Img(row.transform, "Leaf: " + who, new Color(colour.r, colour.g, colour.b, done ? 1f : 0.35f), Circle());
                var lrt = leaf.rectTransform;
                lrt.anchorMin = lrt.anchorMax = new Vector2(x, i % 2 == 0 ? 0.62f : 0.4f);
                lrt.sizeDelta = new Vector2(96f, 96f);
                string id = ch.id;
                leaf.gameObject.AddComponent<Button>().onClick.AddListener(() => Pick(id, who));
                var n = Txt(leaf.transform, name, 28, done ? TextColor : Dim, TextAnchor.UpperCenter);
                var nrt2 = n.rectTransform;
                nrt2.anchorMin = new Vector2(0.5f, 0f); nrt2.anchorMax = new Vector2(0.5f, 0f);
                nrt2.pivot = new Vector2(0.5f, 1f);
                nrt2.anchoredPosition = new Vector2(0f, -4f);
                nrt2.sizeDelta = new Vector2(200f, 40f);
            }
        }

        void Pick(string chapterId, string memberId)
        {
            Close();
            _onPick?.Invoke(chapterId, memberId);
        }

        void Build()
        {
            var canvasGo = new GameObject("Chapter Tree Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2732, 2048);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;

            _panel = new GameObject("Chapter Tree", typeof(RectTransform), typeof(Image));
            _panel.transform.SetParent(canvasGo.transform, false);
            Anchor(_panel.GetComponent<RectTransform>(), Vector2.zero, Vector2.one);
            _panel.GetComponent<Image>().color = new Color(0.07f, 0.07f, 0.09f, 0.97f);

            var title = Txt(_panel.transform, "Chapters", 70, TextColor, TextAnchor.MiddleCenter);
            Anchor(title.rectTransform, new Vector2(0.2f, 0.9f), new Vector2(0.8f, 0.99f));
            var hint = Txt(_panel.transform, "Tap the glowing chapter to go on, or a leaf to replay a chapter as someone who has come forward.", 36, Dim, TextAnchor.MiddleCenter);
            Anchor(hint.rectTransform, new Vector2(0.1f, 0.85f), new Vector2(0.9f, 0.9f));

            var viewport = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(RectMask2D));
            viewport.transform.SetParent(_panel.transform, false);
            Anchor(viewport.GetComponent<RectTransform>(), new Vector2(0.04f, 0.12f), new Vector2(0.96f, 0.84f));
            viewport.GetComponent<Image>().color = new Color(0, 0, 0, 0.01f); // Catches drags.

            var content = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            content.transform.SetParent(viewport.transform, false);
            _content = content.GetComponent<RectTransform>();
            _content.anchorMin = new Vector2(0f, 1f); _content.anchorMax = new Vector2(1f, 1f);
            _content.pivot = new Vector2(0.5f, 1f);
            _content.offsetMin = Vector2.zero; _content.offsetMax = Vector2.zero;
            var v = content.GetComponent<VerticalLayoutGroup>();
            v.childControlWidth = true; v.childForceExpandWidth = true; v.childControlHeight = true; v.childForceExpandHeight = false;
            v.spacing = 0;
            content.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            _scroll = _panel.AddComponent<ScrollRect>();
            _scroll.viewport = viewport.GetComponent<RectTransform>();
            _scroll.content = _content;
            _scroll.horizontal = false;
            _scroll.vertical = true;
            _scroll.movementType = ScrollRect.MovementType.Clamped;
            _scroll.scrollSensitivity = 40f;

            var close = new GameObject("Close", typeof(RectTransform), typeof(Image), typeof(Button));
            close.transform.SetParent(_panel.transform, false);
            Anchor(close.GetComponent<RectTransform>(), new Vector2(0.4f, 0.02f), new Vector2(0.6f, 0.1f));
            close.GetComponent<Image>().color = ButtonColor;
            close.GetComponent<Button>().onClick.AddListener(() => { Close(); var c = _onClose; _onClose = null; c?.Invoke(); });
            var cl = Txt(close.transform, "Not now", 46, TextColor, TextAnchor.MiddleCenter);
            Anchor(cl.rectTransform, Vector2.zero, Vector2.one);

            _panel.SetActive(false);
        }

        static Sprite _circle;

        static Sprite Circle()
        {
            if (_circle != null) return _circle;
            const int n = 96;
            var tex = new Texture2D(n, n, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[n * n];
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    float d = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(n / 2f, n / 2f));
                    px[y * n + x] = new Color32(255, 255, 255, (byte)(Mathf.Clamp01(n / 2f - d) * 255));
                }
            tex.SetPixels32(px);
            tex.Apply(false, true);
            _circle = Sprite.Create(tex, new Rect(0, 0, n, n), new Vector2(0.5f, 0.5f), 100f);
            return _circle;
        }

        Image Img(Transform parent, string name, Color colour, Sprite sprite)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var img = go.GetComponent<Image>();
            img.color = colour;
            img.sprite = sprite;
            img.raycastTarget = sprite != null; // Nodes and leaves take taps; the trunk and branches don't.
            return img;
        }

        Text Txt(Transform parent, string text, int size, Color colour, TextAnchor align)
        {
            var go = new GameObject("Text", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var t = go.GetComponent<Text>();
            t.font = _font; t.fontSize = size; t.color = colour; t.alignment = align;
            t.supportRichText = true; t.raycastTarget = false;
            t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Overflow;
            t.text = text;
            return t;
        }

        static void Anchor(RectTransform rt, Vector2 min, Vector2 max)
        {
            rt.anchorMin = min; rt.anchorMax = max; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        }
    }
}
