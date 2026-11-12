using System;
using IAmAsIAm.Core.Practices;
using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Practices
{
    /// <summary>
    /// The moon tonight (owner: a moon phase always shown). A small moon at the top left, under the fronting label, drawn in
    /// the right phase for the story's date (Ink set_date), with its name under it ("Full moon"). On a full or new moon the
    /// altar offers the esbat. Hidden on the title screen and before the story has a date. Placeholder art drawn in code
    /// (art: ui_moon_<phase>).
    /// </summary>
    public sealed class MoonBadge : MonoBehaviour
    {
        const int SortingOrder = 40;
        const int TexSize = 96;

        /// <summary>Set by the story screen: whether a game is being played (not the title screen).</summary>
        public Func<bool> Visible = () => true;

        Font _font;
        GameObject _root;
        Image _moon;
        Text _label;
        Texture2D _tex;
        DateTime? _shown;

        void Awake()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Build();
        }

        void Update()
        {
            var date = GameServices.Book?.Date;
            bool show = date.HasValue && Visible();
            if (_root.activeSelf != show) _root.SetActive(show);
            if (!show || _shown == date) return;
            _shown = date;
            Draw(Moon.Age(date.Value));
            _label.text = Moon.Name(Moon.Phase(date.Value));
        }

        /// <summary>Lit where the sun reaches: right side while waxing, left while waning (as seen from the north).</summary>
        void Draw(double age)
        {
            var px = new Color32[TexSize * TexSize];
            float r = TexSize / 2f;
            double c = Math.Cos(2 * Math.PI * age);
            var lit = new Color32(246, 240, 214, 255);
            var dark = new Color32(60, 62, 82, 255);
            for (int y = 0; y < TexSize; y++)
                for (int x = 0; x < TexSize; x++)
                {
                    float nx = (x + 0.5f - r) / r, ny = (y + 0.5f - r) / r;
                    float d2 = nx * nx + ny * ny;
                    if (d2 > 1f) { px[y * TexSize + x] = new Color32(0, 0, 0, 0); continue; }
                    double edge = c * Math.Sqrt(1 - ny * ny);
                    bool on = age < 0.5 ? nx > edge : nx < -edge;
                    var col = on ? lit : dark;
                    byte a = (byte)(255 * Mathf.Clamp01((1f - Mathf.Sqrt(d2)) * r / 1.5f));
                    px[y * TexSize + x] = new Color32(col.r, col.g, col.b, a);
                }
            _tex.SetPixels32(px);
            _tex.Apply(false);
        }

        void Build()
        {
            var canvasGo = new GameObject("Moon Canvas", typeof(Canvas), typeof(CanvasScaler));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2732, 2048);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;

            _root = new GameObject("Moon", typeof(RectTransform));
            _root.transform.SetParent(canvasGo.transform, false);
            var rt = _root.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.02f, 0.80f);
            rt.pivot = new Vector2(0f, 1f);
            rt.sizeDelta = new Vector2(260, 170);

            _tex = new Texture2D(TexSize, TexSize, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var moonGo = new GameObject("Phase", typeof(RectTransform), typeof(Image));
            moonGo.transform.SetParent(_root.transform, false);
            var mrt = moonGo.GetComponent<RectTransform>();
            mrt.anchorMin = mrt.anchorMax = new Vector2(0f, 1f);
            mrt.pivot = new Vector2(0f, 1f);
            mrt.anchoredPosition = Vector2.zero;
            mrt.sizeDelta = new Vector2(110, 110);
            _moon = moonGo.GetComponent<Image>();
            _moon.sprite = Sprite.Create(_tex, new Rect(0, 0, TexSize, TexSize), new Vector2(0.5f, 0.5f), 100f);
            _moon.raycastTarget = false;

            var labelGo = new GameObject("Name", typeof(RectTransform), typeof(Text), typeof(Outline));
            labelGo.transform.SetParent(_root.transform, false);
            var lrt = labelGo.GetComponent<RectTransform>();
            lrt.anchorMin = new Vector2(0f, 0f); lrt.anchorMax = new Vector2(1f, 0f);
            lrt.pivot = new Vector2(0f, 0f);
            lrt.sizeDelta = new Vector2(0, 50);
            _label = labelGo.GetComponent<Text>();
            _label.font = _font; _label.fontSize = 30; _label.color = new Color(0.95f, 0.93f, 0.86f);
            _label.alignment = TextAnchor.LowerLeft; _label.raycastTarget = false;
            _label.horizontalOverflow = HorizontalWrapMode.Overflow;
            labelGo.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f, 0.7f);
            _root.SetActive(false);
        }
    }
}
