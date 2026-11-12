using System;
using IAmAsIAm.Core.Story;
using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Story
{
    /// <summary>
    /// The introduction before each chapter (owner: "an introduction, a loading screen with a quote, before each chapter").
    /// A dark screen: the chapter's number and name, where and when, then its quote (chapters.json introQuote / introQuoteBy)
    /// fading in line by line, with one small warm light slowly rising behind it. After a few seconds "Tap to begin"; it also
    /// carries on by itself after a while. Shown every time a chapter begins (first play and replays), never when a save
    /// resumes mid-chapter. Reduce motion: the light stays still (the fades stay). Built in code; placeholder look.
    /// </summary>
    public sealed class ChapterIntro : MonoBehaviour
    {
        /// <summary>Above the loading screen (100), under the Anchor (200).</summary>
        const int SortingOrder = 110;
        const float TapAfterSeconds = 3.5f;
        const float AutoAfterSeconds = 14f;
        const float FadeOutSeconds = 0.9f;

        static readonly Color Background = new Color(0.05f, 0.045f, 0.07f, 1f);
        static readonly Color TextColor = new Color(0.95f, 0.92f, 0.86f);
        static readonly Color Dim = new Color(0.72f, 0.68f, 0.64f);
        static readonly Color Warm = new Color(1f, 0.84f, 0.55f);

        static ChapterIntro _instance;
        public static bool IsShowing => _instance != null && _instance._root.activeSelf;

        Font _font;
        GameObject _root;
        CanvasGroup _group, _numberGroup, _nameGroup, _settingGroup, _quoteGroup, _byGroup, _tapGroup;
        Text _number, _name, _setting, _quote, _by;
        RectTransform _light;
        Image _lightImage;
        float _shownAt, _fadeAt = -1f;
        Action _done;

        /// <summary>Shows the chapter's introduction; <paramref name="done"/> runs when it has faded away.</summary>
        public static void Show(ChapterDefinition chapter, Action done)
        {
            if (chapter == null) { done?.Invoke(); return; }
            if (_instance == null) new GameObject("Chapter Intro").AddComponent<ChapterIntro>();
            _instance.ShowInternal(chapter, done);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => _instance = null;

        void Awake()
        {
            if (_instance != null && _instance != this) { Destroy(gameObject); return; }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Build();
            _root.SetActive(false);
        }

        void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }

        /// <summary>"Chapter 6: Sikhism (title to come)" → ("Chapter 6", "Sikhism"); "Prologue: This Little Light" → ("Prologue", "This Little Light").</summary>
        public static (string number, string name) SplitTitle(string title)
        {
            string t = (title ?? "").Replace("(title to come)", "").Trim();
            int colon = t.IndexOf(':');
            if (colon < 0) return (t, "");
            return (t.Substring(0, colon).Trim(), t.Substring(colon + 1).Trim());
        }

        void ShowInternal(ChapterDefinition c, Action done)
        {
            var (number, name) = SplitTitle(c.title);
            _number.text = number.ToUpperInvariant();
            _name.text = name;
            _setting.text = c.setting ?? "";
            _quote.text = string.IsNullOrWhiteSpace(c.introQuote) ? "" : "“" + c.introQuote.Trim() + "”";
            _by.text = c.introQuoteBy ?? "";
            _done = done;
            _shownAt = Time.unscaledTime;
            _fadeAt = -1f;
            _root.SetActive(true);
            Apply(0f);
        }

        void Update()
        {
            if (!_root.activeSelf) return;
            float t = Time.unscaledTime - _shownAt;
            if (_fadeAt < 0f && t >= AutoAfterSeconds) _fadeAt = Time.unscaledTime;
            Apply(t);
            if (_fadeAt >= 0f && Time.unscaledTime - _fadeAt >= FadeOutSeconds)
            {
                _root.SetActive(false);
                var d = _done;
                _done = null;
                d?.Invoke();
            }
        }

        void Tap()
        {
            if (_fadeAt >= 0f) return;
            if (Time.unscaledTime - _shownAt >= TapAfterSeconds) _fadeAt = Time.unscaledTime;
        }

        static float Ramp(float t, float start, float seconds) => Mathf.Clamp01((t - start) / seconds);

        void Apply(float t)
        {
            bool still = GameServices.Comfort != null && GameServices.Comfort.reduceMotion;
            _group.alpha = _fadeAt < 0f ? 1f : 1f - Mathf.Clamp01((Time.unscaledTime - _fadeAt) / FadeOutSeconds);
            _numberGroup.alpha = Ramp(t, 0.3f, 1f);
            _nameGroup.alpha = Ramp(t, 0.8f, 1.2f);
            _settingGroup.alpha = Ramp(t, 1.4f, 1f) * 0.85f;
            _quoteGroup.alpha = Ramp(t, 2.0f, 1.6f);
            _byGroup.alpha = Ramp(t, 3.0f, 1.2f);
            _tapGroup.alpha = _fadeAt >= 0f ? 0f : Ramp(t, TapAfterSeconds, 1f) * (still ? 0.7f : 0.45f + 0.25f * Mathf.Sin(t * 2f));

            // A little light, rising slowly behind the words and breathing (still with Reduce motion).
            float rise = still ? 0f : Mathf.Min(t, AutoAfterSeconds) * 14f;
            float breathe = still ? 1f : 1f + 0.06f * Mathf.Sin(t * 1.3f);
            _light.anchoredPosition = new Vector2(0f, -260f + rise);
            _light.localScale = Vector3.one * breathe;
            var c = Warm; c.a = 0.22f * Ramp(t, 0f, 2f);
            _lightImage.color = c;
        }

        // ── Building the UI ──────────────────────────────────────────────────────

        void Build()
        {
            var canvasGo = new GameObject("Chapter Intro Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(CanvasGroup));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2732, 2048);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;
            _root = canvasGo;
            _group = canvasGo.GetComponent<CanvasGroup>();
            var root = canvasGo.transform;

            // The background takes every tap (tap to begin, once the words are up).
            var bg = new GameObject("Background", typeof(RectTransform), typeof(Image), typeof(Button));
            bg.transform.SetParent(root, false);
            Stretch(bg.GetComponent<RectTransform>(), Vector2.zero, Vector2.one);
            bg.GetComponent<Image>().color = Background;
            bg.GetComponent<Button>().transition = Selectable.Transition.None;
            bg.GetComponent<Button>().onClick.AddListener(Tap);

            var light = new GameObject("Light", typeof(RectTransform), typeof(Image));
            light.transform.SetParent(root, false);
            _light = light.GetComponent<RectTransform>();
            _light.anchorMin = _light.anchorMax = new Vector2(0.5f, 0.5f);
            _light.sizeDelta = new Vector2(1400, 1400);
            _lightImage = light.GetComponent<Image>();
            _lightImage.sprite = SoftCircle();
            _lightImage.raycastTarget = false;

            _number = Line(root, "Number", 46, FontStyle.Bold, Dim, new Vector2(0.15f, 0.80f), new Vector2(0.85f, 0.86f), out _numberGroup);
            _name = Line(root, "Name", 96, FontStyle.Bold, TextColor, new Vector2(0.1f, 0.68f), new Vector2(0.9f, 0.80f), out _nameGroup);
            _setting = Line(root, "Setting", 40, FontStyle.Italic, Dim, new Vector2(0.15f, 0.62f), new Vector2(0.85f, 0.68f), out _settingGroup);
            _quote = Line(root, "Quote", 62, FontStyle.Italic, TextColor, new Vector2(0.16f, 0.30f), new Vector2(0.84f, 0.58f), out _quoteGroup);
            _by = Line(root, "By", 40, FontStyle.Normal, Dim, new Vector2(0.2f, 0.22f), new Vector2(0.8f, 0.29f), out _byGroup);
            Line(root, "Tap", 40, FontStyle.Normal, TextColor, new Vector2(0.3f, 0.06f), new Vector2(0.7f, 0.12f), out _tapGroup).text = "Tap to begin";
        }

        Text Line(Transform parent, string name, int size, FontStyle style, Color color, Vector2 min, Vector2 max, out CanvasGroup group)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasGroup), typeof(Text));
            go.transform.SetParent(parent, false);
            Stretch(go.GetComponent<RectTransform>(), min, max);
            group = go.GetComponent<CanvasGroup>();
            group.blocksRaycasts = false;
            var t = go.GetComponent<Text>();
            t.font = _font; t.fontSize = size; t.fontStyle = style; t.color = color;
            t.alignment = TextAnchor.MiddleCenter; t.supportRichText = false; t.raycastTarget = false;
            t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Overflow;
            t.lineSpacing = 1.15f;
            return t;
        }

        static void Stretch(RectTransform rt, Vector2 min, Vector2 max)
        {
            rt.anchorMin = min; rt.anchorMax = max;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
        }

        static Sprite _soft;

        static Sprite SoftCircle()
        {
            if (_soft != null) return _soft;
            const int size = 128;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[size * size];
            float r = size / 2f;
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float d = Mathf.Sqrt((x + 0.5f - r) * (x + 0.5f - r) + (y + 0.5f - r) * (y + 0.5f - r)) / r;
                    float a = Mathf.Clamp01(1f - d);
                    px[y * size + x] = new Color32(255, 255, 255, (byte)(255 * a * a));
                }
            tex.SetPixels32(px);
            tex.Apply(false, true);
            _soft = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
            return _soft;
        }
    }
}
