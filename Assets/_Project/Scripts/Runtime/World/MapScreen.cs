using System;
using System.Collections.Generic;
using IAmAsIAm.Core.Story;
using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.World
{
    /// <summary>
    /// The city map (owner, 2026-11-12: "Map, please, that is gained when she leaves for college"). From college on, getting
    /// from one scene to the next opens the map of that city: the places she has been are marked, and the next one (where the
    /// story is heading) glows; tapping it goes there. The story guides: only the next place can be chosen.
    ///
    /// Map art: <c>Assets/_Project/Resources/Maps/map_&lt;city-id&gt;.png</c> (4:3, e.g. 2048 × 1536). Without it, a plain
    /// placeholder map is drawn in code. Pin positions are in places.json (x, y from the bottom left, 0..1).
    /// </summary>
    public sealed class MapScreen : MonoBehaviour
    {
        const int SortingOrder = 45; // Above the story (and the phone at 40), under the Anchor (200).
        static readonly Color TextColor = new Color(0.95f, 0.92f, 0.86f);
        static readonly Color ButtonColor = new Color(0.28f, 0.22f, 0.30f, 0.95f);

        public static bool IsOpen { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => IsOpen = false;

        Font _font;
        GameObject _panel;
        Text _title, _hint;
        RawImage _map;
        RectTransform _pins;
        Action _onGo, _onBack;
        RectTransform _destination;

        void Awake()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Build();
        }

        void OnDisable() => IsOpen = false;

        /// <summary>Opens the map of <paramref name="cityId"/> with <paramref name="destinationId"/> to go to.</summary>
        public void Show(PlaceData data, string cityId, string destinationId, ICollection<string> visited, Action onGo, Action onBack)
        {
            _onGo = onGo;
            _onBack = onBack;
            var city = data.City(cityId);
            _title.text = city != null ? city.name : cityId;
            var dest = data.Find(destinationId);
            _hint.text = dest != null ? $"Tap <b>{Building(dest.name)}</b> to go there." : "";

            var art = Resources.Load<Texture2D>("Maps/map_" + cityId);
            _map.texture = art != null ? art : Placeholder(cityId);

            for (int i = _pins.childCount - 1; i >= 0; i--) Destroy(_pins.GetChild(i).gameObject);
            _destination = null;
            // One pin per building: places that share a spot (the museum's hall, office and gallery) are one pin.
            var done = new HashSet<string>();
            foreach (var p in data.places)
            {
                if (p.city != cityId) continue;
                string spot = $"{p.x:0.00},{p.y:0.00}";
                bool isDest = p.id == destinationId;
                bool seen = visited.Contains(p.id);
                if (!isDest && (!seen || done.Contains(spot))) continue;
                if (isDest || !done.Contains(spot)) done.Add(spot);
                Pin(p, isDest);
            }
            if (_destination != null) _destination.SetAsLastSibling();

            _panel.SetActive(true);
            IsOpen = true;
        }

        void Close()
        {
            _panel.SetActive(false);
            IsOpen = false;
        }

        void Update()
        {
            if (!IsOpen || _destination == null) return;
            // The next place breathes gently (slow, no flashing).
            float s = 1f + 0.08f * Mathf.Sin(Time.unscaledTime * 2f);
            _destination.localScale = new Vector3(s, s, 1f);
        }

        static string Building(string name)
        {
            int colon = name.IndexOf(':');
            return colon > 0 ? name.Substring(0, colon) : name;
        }

        void Pin(PlaceDefinition p, bool destination)
        {
            var go = new GameObject("Pin: " + p.id, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(_pins, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(Mathf.Clamp01(p.x), Mathf.Clamp01(p.y));
            rt.sizeDelta = destination ? new Vector2(560, 150) : new Vector2(420, 96);
            var img = go.GetComponent<Image>();
            img.color = destination ? new Color(0.98f, 0.82f, 0.45f, 0.97f) : new Color(0.2f, 0.18f, 0.22f, 0.75f);
            img.raycastTarget = destination;
            if (destination)
            {
                go.AddComponent<Button>().onClick.AddListener(() => { Close(); var go2 = _onGo; _onGo = null; go2?.Invoke(); });
                _destination = rt;
            }
            var label = MakeText(go.transform, Building(p.name), destination ? 46 : 34, destination ? FontStyle.Bold : FontStyle.Normal,
                destination ? new Color(0.15f, 0.1f, 0.08f) : TextColor);
            Stretch(label.rectTransform);
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

            _panel = new GameObject("Map", typeof(RectTransform), typeof(Image));
            _panel.transform.SetParent(canvasGo.transform, false);
            Place(_panel.GetComponent<RectTransform>(), new Vector2(0.08f, 0.05f), new Vector2(0.84f, 0.88f));
            _panel.GetComponent<Image>().color = new Color(0.1f, 0.09f, 0.12f, 0.96f);

            _title = MakeText(_panel.transform, "", 64, FontStyle.Bold, TextColor);
            Place(_title.rectTransform, new Vector2(0.03f, 0.9f), new Vector2(0.97f, 0.99f));

            var mapGo = new GameObject("Map Picture", typeof(RectTransform), typeof(RawImage));
            mapGo.transform.SetParent(_panel.transform, false);
            Place(mapGo.GetComponent<RectTransform>(), new Vector2(0.03f, 0.14f), new Vector2(0.97f, 0.89f));
            _map = mapGo.GetComponent<RawImage>();
            _map.raycastTarget = false;

            var pins = new GameObject("Pins", typeof(RectTransform));
            pins.transform.SetParent(mapGo.transform, false);
            _pins = pins.GetComponent<RectTransform>();
            Stretch(_pins);

            _hint = MakeText(_panel.transform, "", 44, FontStyle.Italic, TextColor);
            Place(_hint.rectTransform, new Vector2(0.03f, 0.02f), new Vector2(0.7f, 0.12f));

            var back = new GameObject("Back", typeof(RectTransform), typeof(Image), typeof(Button));
            back.transform.SetParent(_panel.transform, false);
            Place(back.GetComponent<RectTransform>(), new Vector2(0.74f, 0.025f), new Vector2(0.97f, 0.115f));
            back.GetComponent<Image>().color = ButtonColor;
            back.GetComponent<Button>().onClick.AddListener(() => { Close(); var b = _onBack; _onBack = null; b?.Invoke(); });
            var backLabel = MakeText(back.transform, "Not yet", 44, FontStyle.Bold, TextColor);
            Stretch(backLabel.rectTransform);

            _panel.SetActive(false);
        }

        static readonly Dictionary<string, Texture2D> Drawn = new Dictionary<string, Texture2D>();

        /// <summary>A plain paper map until the owner's art arrives: a street grid, a park, and Lake Michigan for Chicago.</summary>
        static Texture2D Placeholder(string cityId)
        {
            if (Drawn.TryGetValue(cityId ?? "", out var cached) && cached != null) return cached;
            const int w = 512, h = 384;
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[w * h];
            var paper = new Color32(226, 214, 190, 255);
            var street = new Color32(198, 184, 158, 255);
            var avenue = new Color32(176, 160, 132, 255);
            var water = new Color32(150, 178, 196, 255);
            var park = new Color32(176, 196, 150, 255);
            var rng = new System.Random((cityId ?? "").GetHashCode());
            for (int i = 0; i < px.Length; i++) px[i] = paper;
            void Fill(int x0, int y0, int x1, int y1, Color32 c)
            {
                for (int y = Mathf.Max(0, y0); y < Mathf.Min(h, y1); y++)
                    for (int x = Mathf.Max(0, x0); x < Mathf.Min(w, x1); x++) px[y * w + x] = c;
            }
            int px0 = rng.Next(60, 300), py0 = rng.Next(40, 240);
            Fill(px0, py0, px0 + 90, py0 + 60, park);
            for (int x = 20; x < w; x += 40) Fill(x, 0, x + 3, h, street);
            for (int y = 16; y < h; y += 36) Fill(0, y, w, y + 3, street);
            Fill(0, h / 2, w, h / 2 + 7, avenue);
            Fill(w / 3, 0, w / 3 + 7, h, avenue);
            if (cityId == "chicago") Fill(w - 70, 0, w, h, water); // Lake Michigan, to the east.
            tex.SetPixels32(px);
            tex.Apply(false, true);
            Drawn[cityId ?? ""] = tex;
            return tex;
        }

        Text MakeText(Transform parent, string text, int size, FontStyle style, Color color)
        {
            var go = new GameObject("Text", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var t = go.GetComponent<Text>();
            t.font = _font; t.fontSize = size; t.fontStyle = style; t.color = color; t.alignment = TextAnchor.MiddleCenter;
            t.supportRichText = true; t.raycastTarget = false;
            t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Overflow;
            t.text = text;
            return t;
        }

        static void Place(RectTransform rt, Vector2 min, Vector2 max)
        {
            rt.anchorMin = min; rt.anchorMax = max; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        }

        static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        }
    }
}
