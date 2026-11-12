using System;
using System.Collections.Generic;
using IAmAsIAm.Core.Story;
using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.World
{
    /// <summary>
    /// The map on Maya's phone (owner, 2026-11-12: "Phone map"). From Chapter 1 on (the Prologue and the Prelude are door to
    /// door), getting from one scene to the next opens it like a map app: a blue dot where she is, the places she's been,
    /// the next place as a red pin with a blue route to it, and a Go button. The story guides: only the next place can be
    /// chosen. It also opens from the phone's Maps tab, just to look (no destination).
    ///
    /// Map art: <c>Assets/_Project/Resources/Maps/map_&lt;city-id&gt;.png</c>, portrait 3:4 (e.g. 1536 × 2048), in a map-app
    /// style. Without it a plain map-app placeholder is drawn in code. Pin positions are in places.json (x, y from the
    /// bottom left, 0..1).
    /// </summary>
    public sealed class MapScreen : MonoBehaviour
    {
        const int SortingOrder = 45; // Above the story (and the phone at 40), under the Anchor (200).
        const float MapW = 860f, MapH = 1180f;
        static readonly Color Ink = new Color(0.13f, 0.13f, 0.15f);
        static readonly Color Blue = new Color(0.26f, 0.52f, 0.96f);
        static readonly Color Red = new Color(0.86f, 0.27f, 0.22f);

        public static bool IsOpen { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => IsOpen = false;

        Font _font;
        GameObject _panel, _goButton;
        Text _search, _cardTitle, _cardText, _backLabel;
        RawImage _map;
        RectTransform _marks;
        Action _onGo, _onBack;
        RectTransform _pulse;

        void Awake()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Build();
        }

        void OnDisable() => IsOpen = false;

        /// <summary>
        /// Opens the map of <paramref name="cityId"/>. With a <paramref name="destinationId"/>: directions there, and Go calls
        /// <paramref name="onGo"/>. Without: just looking (the phone's Maps tab).
        /// </summary>
        public void Show(PlaceData data, string cityId, string currentId, string destinationId, ICollection<string> visited,
            Action onGo, Action onBack)
        {
            _onGo = onGo;
            _onBack = onBack;
            var city = data.City(cityId);
            string cityName = city != null ? city.name : cityId;
            var dest = string.IsNullOrEmpty(destinationId) ? null : data.Find(destinationId);
            var here = string.IsNullOrEmpty(currentId) ? null : data.Find(currentId);
            if (here != null && here.city != cityId) here = null;

            _search.text = dest != null ? "Directions to " + Building(dest.name) : "Search " + cityName;
            _cardTitle.text = dest != null ? Building(dest.name) : cityName;
            _cardText.text = dest != null
                ? (here != null ? $"From {Building(here.name)}" : cityName)
                : (here != null ? $"You're at {Building(here.name)}" : "");
            _goButton.SetActive(dest != null);
            _backLabel.text = dest != null ? "Not yet" : "Close";

            var art = Resources.Load<Texture2D>("Maps/map_" + cityId);
            _map.texture = art != null ? art : Placeholder(cityId);

            for (int i = _marks.childCount - 1; i >= 0; i--) Destroy(_marks.GetChild(i).gameObject);
            _pulse = null;

            // One mark per building: places sharing a spot (the museum's hall, office and gallery) are one mark.
            var done = new HashSet<string>();
            foreach (var p in data.places)
            {
                if (p.city != cityId || p == dest || (here != null && p == here)) continue;
                string spot = $"{p.x:0.00},{p.y:0.00}";
                if (!visited.Contains(p.id) || done.Contains(spot)) continue;
                if (dest != null && SameSpot(p, dest)) continue;
                if (here != null && SameSpot(p, here)) continue;
                done.Add(spot);
                Dot(p, new Color(0.45f, 0.45f, 0.5f), 34f, Building(p.name), 30);
            }
            if (here != null && dest != null && !SameSpot(here, dest)) Route(At(here), At(dest));
            if (dest != null) Pin(dest);
            if (here != null) _pulse = Dot(here, Blue, 46f, null, 0);

            _panel.SetActive(true);
            IsOpen = true;
        }

        static bool SameSpot(PlaceDefinition a, PlaceDefinition b) => Mathf.Abs(a.x - b.x) < 0.005f && Mathf.Abs(a.y - b.y) < 0.005f;

        void Close()
        {
            _panel.SetActive(false);
            IsOpen = false;
        }

        void Update()
        {
            if (!IsOpen || _pulse == null) return;
            // "You are here" breathes gently (slow, no flashing).
            float s = 1f + 0.12f * Mathf.Sin(Time.unscaledTime * 2f);
            _pulse.localScale = new Vector3(s, s, 1f);
        }

        static string Building(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";
            int colon = name.IndexOf(':');
            return colon > 0 ? name.Substring(0, colon) : name;
        }

        static Vector2 At(PlaceDefinition p) => new Vector2((Mathf.Clamp01(p.x) - 0.5f) * MapW, (Mathf.Clamp01(p.y) - 0.5f) * MapH);

        RectTransform Dot(PlaceDefinition p, Color colour, float size, string label, int fontSize)
        {
            var go = new GameObject("Mark: " + p.id, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(_marks, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = At(p);
            rt.sizeDelta = new Vector2(size, size);
            var img = go.GetComponent<Image>();
            img.sprite = Circle();
            img.color = colour;
            img.raycastTarget = false;
            if (!string.IsNullOrEmpty(label))
            {
                var t = MakeText(go.transform, label, fontSize, FontStyle.Normal, Ink, TextAnchor.MiddleLeft);
                var trt = t.rectTransform;
                trt.anchorMin = trt.anchorMax = new Vector2(1f, 0.5f);
                trt.pivot = new Vector2(0f, 0.5f);
                trt.anchoredPosition = new Vector2(10f, 0f);
                trt.sizeDelta = new Vector2(320f, 80f);
            }
            return rt;
        }

        void Pin(PlaceDefinition p)
        {
            var head = Dot(p, Red, 64f, null, 0);
            head.anchoredPosition += new Vector2(0f, 46f);
            var inner = new GameObject("Hole", typeof(RectTransform), typeof(Image));
            inner.transform.SetParent(head, false);
            var irt = inner.GetComponent<RectTransform>();
            irt.anchorMin = irt.anchorMax = new Vector2(0.5f, 0.5f);
            irt.sizeDelta = new Vector2(24f, 24f);
            inner.GetComponent<Image>().sprite = Circle();
            inner.GetComponent<Image>().raycastTarget = false;
            var stem = new GameObject("Stem", typeof(RectTransform), typeof(Image));
            stem.transform.SetParent(_marks, false);
            var srt = stem.GetComponent<RectTransform>();
            srt.anchorMin = srt.anchorMax = new Vector2(0.5f, 0.5f);
            srt.anchoredPosition = At(p) + new Vector2(0f, 12f);
            srt.sizeDelta = new Vector2(8f, 34f);
            stem.GetComponent<Image>().color = Red;
            stem.GetComponent<Image>().raycastTarget = false;
            head.SetAsLastSibling();
            var label = MakeText(head, Building(p.name), 34, FontStyle.Bold, Ink, TextAnchor.MiddleCenter);
            var lrt = label.rectTransform;
            lrt.anchorMin = lrt.anchorMax = new Vector2(0.5f, 1f);
            lrt.pivot = new Vector2(0.5f, 0f);
            lrt.anchoredPosition = new Vector2(0f, 6f);
            lrt.sizeDelta = new Vector2(420f, 60f);
        }

        void Route(Vector2 from, Vector2 to)
        {
            var go = new GameObject("Route", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(_marks, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0f, 0.5f);
            rt.anchoredPosition = from;
            var d = to - from;
            rt.sizeDelta = new Vector2(d.magnitude, 14f);
            rt.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg);
            var img = go.GetComponent<Image>();
            img.color = new Color(Blue.r, Blue.g, Blue.b, 0.85f);
            img.raycastTarget = false;
        }

        void Build()
        {
            var canvasGo = new GameObject("Map Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2732, 2048);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;

            // A dim backdrop, then the phone in the middle of the screen.
            _panel = Box(canvasGo.transform, "Map (phone)", new Color(0, 0, 0, 0.45f), Vector2.zero, Vector2.one);
            var phone = Fixed(_panel.transform, "Phone", new Color(0.08f, 0.08f, 0.09f), new Vector2(980, 1940));
            var screen = Fixed(phone.transform, "Screen", new Color(0.93f, 0.92f, 0.89f), new Vector2(900, 1840));

            var mapGo = new GameObject("Map Picture", typeof(RectTransform), typeof(RawImage));
            mapGo.transform.SetParent(screen.transform, false);
            var mrt = mapGo.GetComponent<RectTransform>();
            mrt.anchorMin = mrt.anchorMax = new Vector2(0.5f, 0.5f);
            mrt.anchoredPosition = new Vector2(0f, 40f);
            mrt.sizeDelta = new Vector2(MapW, MapH);
            _map = mapGo.GetComponent<RawImage>();
            _map.raycastTarget = false;
            var marks = new GameObject("Marks", typeof(RectTransform));
            marks.transform.SetParent(mapGo.transform, false);
            _marks = marks.GetComponent<RectTransform>();
            _marks.anchorMin = Vector2.zero; _marks.anchorMax = Vector2.one;
            _marks.offsetMin = Vector2.zero; _marks.offsetMax = Vector2.zero;

            // The search / directions bar at the top.
            var bar = Box(screen.transform, "Search Bar", Color.white, new Vector2(0.04f, 0.9f), new Vector2(0.96f, 0.97f));
            _search = MakeText(bar.transform, "", 40, FontStyle.Normal, new Color(0.35f, 0.35f, 0.38f), TextAnchor.MiddleLeft);
            Inset(_search.rectTransform, 30f);

            // The card at the bottom: where, from where, Go / Not yet.
            var card = Box(screen.transform, "Card", Color.white, new Vector2(0f, 0f), new Vector2(1f, 0.2f));
            _cardTitle = MakeText(card.transform, "", 48, FontStyle.Bold, Ink, TextAnchor.UpperLeft);
            Anchor(_cardTitle.rectTransform, new Vector2(0.05f, 0.55f), new Vector2(0.95f, 0.92f));
            _cardText = MakeText(card.transform, "", 34, FontStyle.Normal, new Color(0.4f, 0.4f, 0.43f), TextAnchor.UpperLeft);
            Anchor(_cardText.rectTransform, new Vector2(0.05f, 0.38f), new Vector2(0.95f, 0.58f));

            _goButton = Box(card.transform, "Go", Blue, new Vector2(0.05f, 0.07f), new Vector2(0.55f, 0.34f));
            _goButton.AddComponent<Button>().onClick.AddListener(() => { Close(); var go = _onGo; _onGo = null; go?.Invoke(); });
            Inset(MakeText(_goButton.transform, "Go", 44, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter).rectTransform, 0f);

            var back = Box(card.transform, "Back", new Color(0.9f, 0.9f, 0.92f), new Vector2(0.6f, 0.07f), new Vector2(0.95f, 0.34f));
            back.AddComponent<Button>().onClick.AddListener(() => { Close(); var b = _onBack; _onBack = null; b?.Invoke(); });
            _backLabel = MakeText(back.transform, "Not yet", 40, FontStyle.Bold, Ink, TextAnchor.MiddleCenter);
            Inset(_backLabel.rectTransform, 0f);

            _panel.SetActive(false);
        }

        // ── Drawing ──────────────────────────────────────────────────────────

        static readonly Dictionary<string, Texture2D> Drawn = new Dictionary<string, Texture2D>();
        static Sprite _circle;

        static Sprite Circle()
        {
            if (_circle != null) return _circle;
            const int n = 64;
            var tex = new Texture2D(n, n, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[n * n];
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    float d = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(n / 2f, n / 2f));
                    float a = Mathf.Clamp01(n / 2f - d);
                    px[y * n + x] = new Color32(255, 255, 255, (byte)(a * 255));
                }
            tex.SetPixels32(px);
            tex.Apply(false, true);
            _circle = Sprite.Create(tex, new Rect(0, 0, n, n), new Vector2(0.5f, 0.5f), 100f);
            return _circle;
        }

        /// <summary>A map-app placeholder until the owner's art arrives: white streets, yellow main roads, parks, and Lake Michigan.</summary>
        static Texture2D Placeholder(string cityId)
        {
            if (Drawn.TryGetValue(cityId ?? "", out var cached) && cached != null) return cached;
            const int w = 384, h = 528;
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[w * h];
            var ground = new Color32(236, 233, 226, 255);
            var street = new Color32(255, 255, 255, 255);
            var road = new Color32(250, 222, 140, 255);
            var water = new Color32(170, 205, 240, 255);
            var park = new Color32(196, 230, 184, 255);
            var rng = new System.Random((cityId ?? "").GetHashCode());
            for (int i = 0; i < px.Length; i++) px[i] = ground;
            void Fill(int x0, int y0, int x1, int y1, Color32 c)
            {
                for (int y = Mathf.Max(0, y0); y < Mathf.Min(h, y1); y++)
                    for (int x = Mathf.Max(0, x0); x < Mathf.Min(w, x1); x++) px[y * w + x] = c;
            }
            for (int k = 0; k < 3; k++)
            {
                int x0 = rng.Next(10, w - 90), y0 = rng.Next(10, h - 90);
                Fill(x0, y0, x0 + rng.Next(40, 90), y0 + rng.Next(40, 90), park);
            }
            for (int x = 14; x < w; x += 30) Fill(x, 0, x + 4, h, street);
            for (int y = 10; y < h; y += 30) Fill(0, y, w, y + 4, street);
            Fill(0, h / 2, w, h / 2 + 8, road);
            Fill(w / 3, 0, w / 3 + 8, h, road);
            Fill(0, h / 5, w, h / 5 + 6, road);
            if (cityId == "chicago") Fill(w - 52, 0, w, h, water); // Lake Michigan, to the east.
            tex.SetPixels32(px);
            tex.Apply(false, true);
            Drawn[cityId ?? ""] = tex;
            return tex;
        }

        GameObject Box(Transform parent, string name, Color colour, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            Anchor(go.GetComponent<RectTransform>(), min, max);
            go.GetComponent<Image>().color = colour;
            return go;
        }

        GameObject Fixed(Transform parent, string name, Color colour, Vector2 size)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = size;
            go.GetComponent<Image>().color = colour;
            return go;
        }

        Text MakeText(Transform parent, string text, int size, FontStyle style, Color color, TextAnchor align)
        {
            var go = new GameObject("Text", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var t = go.GetComponent<Text>();
            t.font = _font; t.fontSize = size; t.fontStyle = style; t.color = color; t.alignment = align;
            t.supportRichText = true; t.raycastTarget = false;
            t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Overflow;
            t.text = text;
            return t;
        }

        static void Anchor(RectTransform rt, Vector2 min, Vector2 max)
        {
            rt.anchorMin = min; rt.anchorMax = max; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        }

        static void Inset(RectTransform rt, float pad)
        {
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(pad, 0f); rt.offsetMax = new Vector2(-pad, 0f);
        }
    }
}
