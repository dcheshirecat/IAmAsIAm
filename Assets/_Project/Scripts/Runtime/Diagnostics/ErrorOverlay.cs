using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Diagnostics
{
    /// <summary>
    /// For test builds on a phone, where there's no console: errors are shown on screen (bottom left) so the
    /// owner can screenshot them and send them to Claude. Also shows the version (bottom right), so it's clear
    /// which build is installed. Made automatically before the first scene loads; needs nothing in the scene.
    /// Tap the panel to hide it. Turn it off for release builds with <see cref="Enabled"/>.
    /// </summary>
    public sealed class ErrorOverlay : MonoBehaviour
    {
        public static bool Enabled = true;
        const int MaxErrors = 4;
        const int SortingOrder = 300;

        static ErrorOverlay _instance;
        readonly StringBuilder _text = new StringBuilder();
        int _count;
        GameObject _panel;
        Text _body;
        bool _dirty;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Create()
        {
            if (!Enabled || _instance != null) return;
            var go = new GameObject("Error Overlay");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<ErrorOverlay>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => _instance = null;

        void Awake()
        {
            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            var canvasGo = new GameObject("Error Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2732, 2048);
            scaler.matchWidthOrHeight = 1f;

            var version = Label(canvasGo.transform, font, 30, new Color(1, 1, 1, 0.45f), new Vector2(0.72f, 0f), new Vector2(0.995f, 0.03f));
            version.alignment = TextAnchor.LowerRight;
            version.text = $"v{Application.version}";

            // Bottom left, clear of the Anchor (top right) and the Use button (bottom right).
            _panel = new GameObject("Errors", typeof(RectTransform), typeof(Image), typeof(Button));
            _panel.transform.SetParent(canvasGo.transform, false);
            var rt = _panel.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.01f, 0.01f); rt.anchorMax = new Vector2(0.62f, 0.3f);
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            _panel.GetComponent<Image>().color = new Color(0.25f, 0.05f, 0.07f, 0.92f);
            _panel.GetComponent<Button>().onClick.AddListener(() => _panel.SetActive(false));
            _body = Label(_panel.transform, font, 30, Color.white, Vector2.zero, Vector2.one);
            _body.alignment = TextAnchor.UpperLeft;
            var brt = _body.rectTransform;
            brt.offsetMin = new Vector2(24, 16); brt.offsetMax = new Vector2(-24, -16);
            _panel.SetActive(false);

            Application.logMessageReceivedThreaded += OnLog;
        }

        void OnDestroy() => Application.logMessageReceivedThreaded -= OnLog;

        void OnLog(string message, string stack, LogType type)
        {
            if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert) return;
            lock (_text)
            {
                _count++;
                if (_count > MaxErrors) { _dirty = true; return; }
                string where = string.IsNullOrEmpty(stack) ? "" : "\n   " + stack.Split('\n')[0].Trim();
                if (message.Length > 300) message = message.Substring(0, 300) + "…";
                _text.Append("• ").Append(message).Append(where).Append('\n');
                _dirty = true;
            }
        }

        void Update()
        {
            if (!_dirty) return;
            lock (_text)
            {
                _dirty = false;
                string more = _count > MaxErrors ? $"(+{_count - MaxErrors} more)\n" : "";
                _body.text = $"<b>Something went wrong ({_count}). Screenshot this for Claude. Tap to hide.</b>\n{more}{_text}";
            }
            _panel.SetActive(true);
        }

        static Text Label(Transform parent, Font font, int size, Color color, Vector2 min, Vector2 max)
        {
            var go = new GameObject("Text", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = min; rt.anchorMax = max; rt.offsetMin = rt.offsetMax = Vector2.zero;
            var t = go.GetComponent<Text>();
            t.font = font; t.fontSize = size; t.color = color; t.raycastTarget = false; t.supportRichText = true;
            t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Truncate;
            return t;
        }
    }
}
