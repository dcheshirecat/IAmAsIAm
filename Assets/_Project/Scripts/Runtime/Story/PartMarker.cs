using System;
using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Story
{
    /// <summary>
    /// The marker at the start of each chapter part (owner: "Maya, age 18" or "???, age 14"): who you're playing and how old
    /// they are, with the month and year under it, over a dimmed screen. The story waits for it: about three seconds, or a tap.
    /// Text from Core.Story.PartMarkerText. Fades only (fine with Reduce motion). Built in code; placeholder look.
    /// </summary>
    public sealed class PartMarker : MonoBehaviour
    {
        /// <summary>Over the story's UI and the phone (40), under the chapter intro (110) and the Anchor (200).</summary>
        const int SortingOrder = 90;
        const float Seconds = 3.2f;
        const float TapAfter = 0.8f;
        const float Fade = 0.6f;

        static readonly Color TextColor = new Color(0.95f, 0.92f, 0.86f);
        static readonly Color Dim = new Color(0.75f, 0.71f, 0.66f);

        static PartMarker _instance;
        public static bool IsShowing => _instance != null && _instance._root.activeSelf;

        GameObject _root;
        CanvasGroup _group;
        Text _who, _when;
        float _shownAt, _endAt = -1f;
        Action _done;

        /// <summary>Shows "<paramref name="who"/>" over "<paramref name="when"/>"; <paramref name="done"/> runs as it fades.</summary>
        public static void Show(string who, string when, Color colour, Action done)
        {
            if (_instance == null) new GameObject("Part Marker").AddComponent<PartMarker>();
            _instance.ShowInternal(who, when, colour, done);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => _instance = null;

        void Awake()
        {
            if (_instance != null && _instance != this) { Destroy(gameObject); return; }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Build();
            _root.SetActive(false);
        }

        void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }

        void ShowInternal(string who, string when, Color colour, Action done)
        {
            // Another marker still up: finish it first so its story step isn't lost.
            if (_root.activeSelf) { var prev = _done; _done = null; prev?.Invoke(); }
            _who.text = who ?? "";
            _who.color = colour;
            _when.text = when ?? "";
            _done = done;
            _shownAt = Time.unscaledTime;
            _endAt = -1f;
            _group.alpha = 0f;
            _root.SetActive(true);
        }

        void Update()
        {
            if (!_root.activeSelf) return;
            float t = Time.unscaledTime - _shownAt;
            if (_endAt < 0f && t >= Seconds) _endAt = Time.unscaledTime;
            float a = Mathf.Clamp01(t / Fade);
            if (_endAt >= 0f) a = Mathf.Min(a, 1f - Mathf.Clamp01((Time.unscaledTime - _endAt) / Fade));
            _group.alpha = a;
            if (_endAt >= 0f && Time.unscaledTime - _endAt >= Fade)
            {
                _root.SetActive(false);
                var d = _done;
                _done = null;
                d?.Invoke();
            }
        }

        void Tap()
        {
            if (_endAt < 0f && Time.unscaledTime - _shownAt >= TapAfter) _endAt = Time.unscaledTime;
        }

        void Build()
        {
            var canvasGo = new GameObject("Part Marker Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(CanvasGroup));
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

            var bg = new GameObject("Dim", typeof(RectTransform), typeof(Image), typeof(Button));
            bg.transform.SetParent(canvasGo.transform, false);
            var brt = bg.GetComponent<RectTransform>();
            brt.anchorMin = Vector2.zero; brt.anchorMax = Vector2.one; brt.offsetMin = brt.offsetMax = Vector2.zero;
            bg.GetComponent<Image>().color = new Color(0.04f, 0.035f, 0.06f, 0.88f);
            bg.GetComponent<Button>().transition = Selectable.Transition.None;
            bg.GetComponent<Button>().onClick.AddListener(Tap);

            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _who = Label(canvasGo.transform, font, 110, FontStyle.Bold, TextColor, new Vector2(0.1f, 0.48f), new Vector2(0.9f, 0.62f));
            _when = Label(canvasGo.transform, font, 52, FontStyle.Italic, Dim, new Vector2(0.1f, 0.40f), new Vector2(0.9f, 0.48f));
        }

        static Text Label(Transform parent, Font font, int size, FontStyle style, Color color, Vector2 min, Vector2 max)
        {
            var go = new GameObject("Text", typeof(RectTransform), typeof(Text), typeof(Outline));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = min; rt.anchorMax = max; rt.offsetMin = rt.offsetMax = Vector2.zero;
            var t = go.GetComponent<Text>();
            t.font = font; t.fontSize = size; t.fontStyle = style; t.color = color;
            t.alignment = TextAnchor.MiddleCenter; t.raycastTarget = false; t.supportRichText = false;
            t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Overflow;
            go.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f, 0.6f);
            return t;
        }
    }
}
