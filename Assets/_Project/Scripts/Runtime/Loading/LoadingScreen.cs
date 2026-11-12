using System.Collections;
using System.Collections.Generic;
using IAmAsIAm.Core.Practices;
using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Loading
{
    /// <summary>
    /// The loading screen (owner, 2026-11-12): each time, a random Major Arcana card rises face down, turns over and
    /// floats gently while the game loads, with the card's reflection prompt beneath it. Shown at start-up and
    /// whenever a save is loaded or a new game begins. Built in code; it lives across scene loads.
    ///
    /// Art: the card back and faces come from Art/Tarot (tarot_back.png, tarot_&lt;card-id&gt;.png), wired in by
    /// IAmAsIAm → Rebuild Boot Scene. Until they exist, the cards are drawn as plain placeholder panels.
    /// Comfort: Reduce motion keeps the card still (a cross-fade instead of the turn). Reversals follow the
    /// tarot reversal setting. The Anchor button stays above this screen (StoryScreen draws it on top).
    /// </summary>
    [DefaultExecutionOrder(-900)]
    public sealed class LoadingScreen : MonoBehaviour
    {
        [SerializeField, Tooltip("Show the card while the game starts up.")]
        bool showOnBoot = true;
        [SerializeField] Sprite cardBack;
        [SerializeField, Tooltip("Card ids matching Face Sprites, filled in by Rebuild Boot Scene from Art/Tarot.")]
        List<string> faceIds = new List<string>();
        [SerializeField] List<Sprite> faceSprites = new List<Sprite>();

        const int SortingOrder = 100;
        static readonly Color Background = new Color(0.06f, 0.05f, 0.08f, 1f);
        static readonly Color TextColor = new Color(0.95f, 0.92f, 0.86f);
        static readonly Color InkColor = new Color(0.22f, 0.17f, 0.24f);
        static readonly Color BackColor = new Color(0.22f, 0.16f, 0.34f);
        static readonly Color BackTrim = new Color(0.78f, 0.66f, 0.42f);
        static readonly Color FaceColor = new Color(0.93f, 0.88f, 0.78f);
        static readonly Color GlowColor = new Color(0.78f, 0.68f, 0.95f);
        const float CardHeight = 1000f;
        const float CardWidth = CardHeight * 2.75f / 4.75f; // Standard tarot proportions (ART_SIZES.md).
        const float CardCentreY = 140f;

        static LoadingScreen _instance;
        static string _lastCardId;
        static bool _creatingOnDemand;

        CanvasGroup _group, _card, _back, _face, _promptGroup;
        RectTransform _cardRt;
        Image _glow, _faceArt;
        Text _numeral, _name, _keywords, _prompt, _loading;
        GameObject _placeholderFace;
        Font _font;
        List<TarotCardEntry> _cards;
        LoadingCardPicker _picker;

        bool _showing, _hideRequested;
        float _shownAt, _fadeStartedAt = -1f;

        public static bool IsShowing => _instance != null && _instance._showing;

        static LoadingScreen Instance
        {
            get
            {
                if (_instance == null)
                {
                    _creatingOnDemand = true; // Made on demand (no boot scene object): the caller shows and hides it.
                    new GameObject("Loading Screen").AddComponent<LoadingScreen>();
                    _creatingOnDemand = false;
                }
                return _instance;
            }
        }

        /// <summary>Shows the screen with a new card. If it's already up, the same card stays.</summary>
        public static void Show() => Instance.ShowInternal();

        /// <summary>Hides it once the card has been up long enough to see (<see cref="LoadingCardAnimation.MinimumSeconds"/>).</summary>
        public static void Hide() { if (_instance != null) _instance._hideRequested = true; }

        /// <summary>Runs a coroutine on the loading screen, which survives scene loads.</summary>
        public static Coroutine Run(IEnumerator routine) => Instance.StartCoroutine(routine);

        void Awake()
        {
            if (_instance != null && _instance != this) { Destroy(gameObject); return; }
            _instance = this;
            if (_creatingOnDemand) showOnBoot = false;
            DontDestroyOnLoad(gameObject);
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _cards = DataLoader.Load<TarotData>("tarot_major").cards ?? new List<TarotCardEntry>();
            _picker = new LoadingCardPicker(_cards, System.Environment.TickCount);
            Build();
            _group.gameObject.SetActive(false);
            if (showOnBoot) ShowInternal();
        }

        void Start()
        {
            // Start-up: hide once the game's data is loaded. A restart (Save.GameSession) hides it itself.
            if (_instance == this && _showing && showOnBoot) StartCoroutine(HideWhenReady());
        }

        IEnumerator HideWhenReady()
        {
            while (!GameServices.IsReady) yield return null;
            Hide();
        }

        void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }

        void ShowInternal()
        {
            _hideRequested = false;
            if (_showing && _fadeStartedAt < 0f) return;
            _showing = true;
            _fadeStartedAt = -1f;
            _shownAt = Time.unscaledTime;
            _group.gameObject.SetActive(true);
            _group.alpha = 1f;
            Deal();
            Apply(0f);
        }

        void Update()
        {
            if (!_showing) return;
            float t = Time.unscaledTime - _shownAt;

            if (_hideRequested && _fadeStartedAt < 0f && t >= LoadingCardAnimation.MinimumSeconds) _fadeStartedAt = Time.unscaledTime;
            float fade = _fadeStartedAt < 0f ? 0f : Mathf.Clamp01((Time.unscaledTime - _fadeStartedAt) / LoadingCardAnimation.FadeOutSeconds);

            Apply(t, fade);
            if (fade >= 1f)
            {
                _showing = false;
                _hideRequested = false;
                _fadeStartedAt = -1f;
                _group.gameObject.SetActive(false);
            }
        }

        void Apply(float t, float fade = 0f)
        {
            bool still = GameServices.Comfort != null && GameServices.Comfort.reduceMotion;
            var pose = LoadingCardAnimation.Evaluate(t, still);
            _group.alpha = 1f - fade;
            _card.alpha = pose.Alpha;
            _cardRt.localScale = new Vector3(pose.ScaleX, 1f, 1f);
            _cardRt.anchoredPosition = new Vector2(0f, CardCentreY + pose.OffsetY);
            _back.alpha = pose.BackAlpha;
            _face.alpha = pose.FaceAlpha;
            var g = GlowColor; g.a = pose.Glow * pose.Alpha;
            _glow.color = g;
            _promptGroup.alpha = Mathf.Clamp01((t - LoadingCardAnimation.FlipStart - LoadingCardAnimation.FlipSeconds) / 0.6f);
            _loading.text = "Loading" + new string('.', LoadingCardAnimation.Dots(t, still));
        }

        /// <summary>Picks this time's card and fills in the face.</summary>
        void Deal()
        {
            if (_cards.Count == 0) { _face.gameObject.SetActive(false); _prompt.text = ""; return; }
            var prefs = GameServices.Preferences;
            string last = _lastCardId ?? prefs?.lastLoadingCardId;
            bool reversals = GameServices.Comfort != null && GameServices.Comfort.allowTarotReversals;
            var draw = _picker.Pick(last, reversals);
            _lastCardId = draw.Card.id;
            if (prefs != null) prefs.lastLoadingCardId = draw.Card.id; // Written to disk with the next preferences save.

            var art = FaceFor(draw.Card.id);
            _faceArt.gameObject.SetActive(art != null);
            _placeholderFace.SetActive(art == null);
            if (art != null)
            {
                _faceArt.sprite = art;
                _faceArt.rectTransform.localEulerAngles = new Vector3(0, 0, draw.Reversed ? 180f : 0f);
            }
            _numeral.text = LoadingCardAnimation.Numeral(draw.Card.number);
            _name.text = draw.Card.name + (draw.Reversed ? "\n<size=40><i>(reversed)</i></size>" : "");
            _keywords.text = string.Join("\n", draw.Keywords);

            string note = string.IsNullOrEmpty(draw.Card.gentleNote) ? "" : $"\n<size=40><i>{draw.Card.gentleNote}</i></size>";
            _prompt.text = $"<b>{draw.Card.name}{(draw.Reversed ? " (reversed)" : "")}</b>\n{draw.Card.reflectionPrompt}{note}";
        }

        Sprite FaceFor(string id)
        {
            int i = faceIds.IndexOf(id);
            return i >= 0 && i < faceSprites.Count ? faceSprites[i] : null;
        }

        // ── Building the UI ───────────────────────────────────────────────────────

        void Build()
        {
            var canvasGo = new GameObject("Loading Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(CanvasGroup));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2732, 2048);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;
            _group = canvasGo.GetComponent<CanvasGroup>();
            var root = canvasGo.transform;

            Panel(root, "Background", Background, Vector2.zero, Vector2.one); // Also blocks taps while loading.

            // The glow: a soft round light behind the card.
            var glow = Panel(root, "Glow", GlowColor, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            _glow = glow.GetComponent<Image>();
            _glow.sprite = SoftCircle();
            _glow.raycastTarget = false;
            var glowRt = glow.GetComponent<RectTransform>();
            glowRt.sizeDelta = new Vector2(CardHeight * 1.5f, CardHeight * 1.5f);
            glowRt.anchoredPosition = new Vector2(0, CardCentreY);

            // The card.
            var card = new GameObject("Card", typeof(RectTransform), typeof(CanvasGroup));
            card.transform.SetParent(root, false);
            _cardRt = card.GetComponent<RectTransform>();
            _cardRt.anchorMin = _cardRt.anchorMax = new Vector2(0.5f, 0.5f);
            _cardRt.sizeDelta = new Vector2(CardWidth, CardHeight);
            _card = card.GetComponent<CanvasGroup>();
            _card.blocksRaycasts = false;

            // Back: the artist's tarot_back.png, or a placeholder (violet with a gold border and a diamond).
            var back = Layer(card.transform, "Back");
            _back = back.GetComponent<CanvasGroup>();
            if (cardBack != null)
            {
                var img = Panel(back.transform, "Art", Color.white, Vector2.zero, Vector2.one).GetComponent<Image>();
                img.sprite = cardBack; img.preserveAspect = true;
            }
            else
            {
                Panel(back.transform, "Trim", BackTrim, Vector2.zero, Vector2.one);
                Panel(back.transform, "Field", BackColor, new Vector2(0.04f, 0.025f), new Vector2(0.96f, 0.975f));
                Panel(back.transform, "Inner Trim", BackTrim, new Vector2(0.1f, 0.06f), new Vector2(0.9f, 0.94f));
                Panel(back.transform, "Inner Field", BackColor, new Vector2(0.115f, 0.068f), new Vector2(0.885f, 0.932f));
                var diamond = Panel(back.transform, "Diamond", BackTrim, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
                var drt = diamond.GetComponent<RectTransform>();
                drt.sizeDelta = new Vector2(150, 150);
                drt.localEulerAngles = new Vector3(0, 0, 45f);
                var hole = Panel(diamond.transform, "Hole", BackColor, new Vector2(0.18f, 0.18f), new Vector2(0.82f, 0.82f));
                hole.GetComponent<Image>().raycastTarget = false;
            }

            // Face: the artist's tarot_<card-id>.png, or a placeholder with the numeral, keywords and name.
            var face = Layer(card.transform, "Face");
            _face = face.GetComponent<CanvasGroup>();
            _faceArt = Panel(face.transform, "Art", Color.white, Vector2.zero, Vector2.one).GetComponent<Image>();
            _faceArt.preserveAspect = true;
            _placeholderFace = new GameObject("Placeholder", typeof(RectTransform));
            _placeholderFace.transform.SetParent(face.transform, false);
            Stretch(_placeholderFace.GetComponent<RectTransform>(), Vector2.zero, Vector2.one);
            Panel(_placeholderFace.transform, "Trim", InkColor, Vector2.zero, Vector2.one);
            Panel(_placeholderFace.transform, "Paper", FaceColor, new Vector2(0.03f, 0.018f), new Vector2(0.97f, 0.982f));
            Panel(_placeholderFace.transform, "Frame", InkColor, new Vector2(0.08f, 0.05f), new Vector2(0.92f, 0.95f));
            Panel(_placeholderFace.transform, "Inner", FaceColor, new Vector2(0.09f, 0.056f), new Vector2(0.91f, 0.944f));
            _numeral = Label(_placeholderFace.transform, "Numeral", 72, FontStyle.Bold, InkColor, new Vector2(0.1f, 0.83f), new Vector2(0.9f, 0.93f));
            _keywords = Label(_placeholderFace.transform, "Keywords", 46, FontStyle.Italic, InkColor, new Vector2(0.12f, 0.3f), new Vector2(0.88f, 0.78f));
            _name = Label(_placeholderFace.transform, "Name", 62, FontStyle.Bold, InkColor, new Vector2(0.1f, 0.07f), new Vector2(0.9f, 0.24f));

            // Under the card: its reflection prompt (tarot as reflection, never prediction), then "Loading…".
            var prompt = new GameObject("Prompt", typeof(RectTransform), typeof(CanvasGroup));
            prompt.transform.SetParent(root, false);
            Stretch(prompt.GetComponent<RectTransform>(), new Vector2(0.18f, 0.06f), new Vector2(0.82f, 0.22f));
            _promptGroup = prompt.GetComponent<CanvasGroup>();
            _prompt = Label(prompt.transform, "Text", 50, FontStyle.Normal, TextColor, Vector2.zero, Vector2.one);
            _loading = Label(root, "Loading", 44, FontStyle.Normal, new Color(TextColor.r, TextColor.g, TextColor.b, 0.6f),
                new Vector2(0.4f, 0.015f), new Vector2(0.6f, 0.06f));
        }

        static GameObject Layer(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasGroup));
            go.transform.SetParent(parent, false);
            Stretch(go.GetComponent<RectTransform>(), Vector2.zero, Vector2.one);
            go.GetComponent<CanvasGroup>().blocksRaycasts = false;
            return go;
        }

        static void Stretch(RectTransform rt, Vector2 min, Vector2 max)
        {
            rt.anchorMin = min; rt.anchorMax = max; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        }

        static GameObject Panel(Transform parent, string name, Color color, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            Stretch(go.GetComponent<RectTransform>(), min, max);
            go.GetComponent<Image>().color = color;
            return go;
        }

        Text Label(Transform parent, string name, int size, FontStyle style, Color color, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            Stretch(go.GetComponent<RectTransform>(), min, max);
            var t = go.GetComponent<Text>();
            t.font = _font; t.fontSize = size; t.fontStyle = style; t.color = color;
            t.alignment = TextAnchor.MiddleCenter;
            t.supportRichText = true; t.raycastTarget = false;
            t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Overflow;
            return t;
        }

        /// <summary>A round, soft-edged white sprite for the glow (made once, in code).</summary>
        static Sprite _softCircle;
        static Sprite SoftCircle()
        {
            if (_softCircle != null) return _softCircle;
            const int size = 128;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp, name = "Loading Glow" };
            var px = new Color32[size * size];
            float r = size / 2f;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float d = Mathf.Sqrt((x + 0.5f - r) * (x + 0.5f - r) + (y + 0.5f - r) * (y + 0.5f - r)) / r;
                float a = Mathf.Clamp01(1f - d);
                px[y * size + x] = new Color32(255, 255, 255, (byte)(a * a * 255));
            }
            tex.SetPixels32(px);
            tex.Apply();
            _softCircle = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
            return _softCircle;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            _instance = null;
            _lastCardId = null;
            _creatingOnDemand = false;
            _softCircle = null;
        }
    }
}
