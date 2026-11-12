using System;
using System.Collections.Generic;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;
using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Practices
{
    /// <summary>
    /// The charm bag in Maya's pocket (owner: she carries it and can hold it to ground; alters react to it).
    /// While she's drifting (presence low) and carries it, a small pouch shows on the left edge: press and hold it, and her
    /// fingers find the knots. It eases the dissociation a little (grounding, never a stat boost). It wears out after a few
    /// holds and she can make another at the altar.
    /// Also listens for switches: Jacob sometimes takes it out of her pocket and leaves it in the altar's drawer, and Izzy
    /// sometimes puts something small in it. None of it is ever pointed out; she just finds it.
    /// Only Maya and Izzy hold it (the ones who practise Wicca, owner). Built in code; placeholder look.
    /// </summary>
    public sealed class CharmBagButton : MonoBehaviour
    {
        static readonly Color Ink = new Color(0.95f, 0.92f, 0.86f);
        static readonly Color Pouch = new Color(0.42f, 0.30f, 0.46f, 1f);
        static readonly Color Warm = new Color(1f, 0.84f, 0.55f, 1f);

        /// <summary>The same layer as the phone and the Book (40), under the ritual screen and the Anchor.</summary>
        const int SortingOrder = 40;
        /// <summary>Shown while presence is below this (she's drifting).</summary>
        const float ShowBelowPresence = 0.85f;
        const float HoldSeconds = 2.5f;

        /// <summary>Set by the story screen: true while something else is up (the Anchor, an interactive moment).</summary>
        public Func<bool> Blocked = () => false;

        Font _font;
        GameObject _button, _note;
        Text _noteText;
        Image _fill;
        bool _holding;
        float _held, _noteUntil;
        SystemRoster _roster;
        readonly System.Random _rng = new System.Random();

        void Awake()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Build();
        }

        void OnDestroy()
        {
            if (_roster != null) _roster.FrontChanged -= OnFrontChanged;
        }

        void OnFrontChanged(FrontChange c) => GameServices.Book?.OnFrontChanged(c.NewId, _rng.NextDouble(), _rng.Next());

        void Update()
        {
            // The roster can be made again (a new slot): follow whichever one is current.
            if (!ReferenceEquals(_roster, GameServices.Roster))
            {
                if (_roster != null) _roster.FrontChanged -= OnFrontChanged;
                _roster = GameServices.Roster;
                if (_roster != null) _roster.FrontChanged += OnFrontChanged;
            }

            var book = GameServices.Book;
            var mind = GameServices.Mind;
            string front = _roster?.FrontingId ?? "maya";
            bool show = book != null && mind != null && book.CanHoldCharmBag(front) && mind.Presence < ShowBelowPresence
                        && !Blocked() && !RitualScreen.IsOpen && !BookScreen.IsOpen && !Devices.DeviceScreen.IsOpen;
            if (_button.activeSelf != show) { _button.SetActive(show); _holding = false; _held = 0f; }
            if (show)
            {
                // Letting go only pauses; nothing is lost.
                if (_holding) _held += Time.unscaledDeltaTime;
                _fill.fillAmount = Mathf.Clamp01(_held / HoldSeconds);
                if (_held >= HoldSeconds) Hold();
            }
            if (_note.activeSelf && Time.unscaledTime > _noteUntil) _note.SetActive(false);
        }

        void Hold()
        {
            _held = 0f;
            _holding = false;
            var book = GameServices.Book;
            var bag = book.CharmBag;
            string what = $"{bag.herb} and {bag.stone.ToLowerInvariant()}, through the cloth. Your fingers find the three knots.";
            var (found, apart) = book.HoldCharmBag();
            var lines = new List<string> { what };
            foreach (var thing in found) lines.Add($"There's something in it you didn't put there: {thing}.");
            if (apart) lines.Add("The knots have come loose. It's done what it could. You can make another at the altar.");
            GameServices.Mind?.ApplyGrounding(new GroundingOutcome(SkillDirection.Either, 0.5f));
            GameServices.Mind?.RecordPractice();
            _noteText.text = string.Join("\n\n", lines);
            _note.SetActive(true);
            _noteUntil = Time.unscaledTime + 4f + 2f * lines.Count;
        }

        // ── Building the UI ──────────────────────────────────────────────────────

        void Build()
        {
            var canvasGo = new GameObject("Charm Bag Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2732, 2048);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;

            // Left edge, above the joystick.
            _button = new GameObject("Charm Bag", typeof(RectTransform), typeof(Image), typeof(RitualHoldArea));
            _button.transform.SetParent(canvasGo.transform, false);
            var rt = _button.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.015f, 0.56f);
            rt.pivot = new Vector2(0f, 0.5f);
            rt.sizeDelta = new Vector2(230, 270);
            _button.GetComponent<Image>().color = new Color(0.16f, 0.14f, 0.18f, 0.82f);
            _button.GetComponent<RitualHoldArea>().Changed = down => _holding = down;

            var fill = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            fill.transform.SetParent(_button.transform, false);
            var frt = fill.GetComponent<RectTransform>();
            frt.anchorMin = Vector2.zero; frt.anchorMax = Vector2.one; frt.offsetMin = frt.offsetMax = Vector2.zero;
            _fill = fill.GetComponent<Image>();
            _fill.sprite = Square();
            _fill.type = Image.Type.Filled;
            _fill.fillMethod = Image.FillMethod.Vertical;
            _fill.fillAmount = 0f;
            _fill.color = new Color(Warm.r, Warm.g, Warm.b, 0.35f);
            _fill.raycastTarget = false;

            // The pouch: a soft round shape with a tie at the top (placeholder; art: ui_icon_charm_bag).
            var pouch = new GameObject("Pouch", typeof(RectTransform), typeof(Image));
            pouch.transform.SetParent(_button.transform, false);
            var prt = pouch.GetComponent<RectTransform>();
            prt.anchorMin = new Vector2(0.2f, 0.32f); prt.anchorMax = new Vector2(0.8f, 0.82f);
            prt.offsetMin = prt.offsetMax = Vector2.zero;
            var pimg = pouch.GetComponent<Image>();
            pimg.sprite = Disc();
            pimg.color = Pouch;
            pimg.raycastTarget = false;
            var tie = new GameObject("Tie", typeof(RectTransform), typeof(Image));
            tie.transform.SetParent(_button.transform, false);
            var trt = tie.GetComponent<RectTransform>();
            trt.anchorMin = new Vector2(0.38f, 0.80f); trt.anchorMax = new Vector2(0.62f, 0.90f);
            trt.offsetMin = trt.offsetMax = Vector2.zero;
            tie.GetComponent<Image>().color = Warm;
            tie.GetComponent<Image>().raycastTarget = false;

            var label = MakeText(_button.transform, "Hold it", 34, FontStyle.Bold, Ink, TextAnchor.MiddleCenter);
            var lrt = label.rectTransform;
            lrt.anchorMin = new Vector2(0f, 0.02f); lrt.anchorMax = new Vector2(1f, 0.30f);
            lrt.offsetMin = lrt.offsetMax = Vector2.zero;
            _button.SetActive(false);

            // What her fingers find (beside the pouch, for a few seconds). Never takes touches.
            _note = new GameObject("Charm Bag Note", typeof(RectTransform), typeof(Image));
            _note.transform.SetParent(canvasGo.transform, false);
            var nrt = _note.GetComponent<RectTransform>();
            nrt.anchorMin = new Vector2(0.11f, 0.46f); nrt.anchorMax = new Vector2(0.48f, 0.68f);
            nrt.offsetMin = nrt.offsetMax = Vector2.zero;
            _note.GetComponent<Image>().color = new Color(0.10f, 0.08f, 0.12f, 0.88f);
            _note.GetComponent<Image>().raycastTarget = false;
            _noteText = MakeText(_note.transform, "", 36, FontStyle.Italic, Ink, TextAnchor.MiddleLeft);
            var ntr = _noteText.rectTransform;
            ntr.anchorMin = Vector2.zero; ntr.anchorMax = Vector2.one;
            ntr.offsetMin = new Vector2(28, 16); ntr.offsetMax = new Vector2(-28, -16);
            _note.SetActive(false);
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

        static Sprite _disc, _square;

        static Sprite Disc()
        {
            if (_disc != null) return _disc;
            const int size = 128;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[size * size];
            float r = size / 2f;
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float d = Mathf.Sqrt((x + 0.5f - r) * (x + 0.5f - r) + (y + 0.5f - r) * (y + 0.5f - r)) / r;
                    px[y * size + x] = new Color32(255, 255, 255, (byte)(255 * Mathf.Clamp01((1f - d) * 8f)));
                }
            tex.SetPixels32(px);
            tex.Apply(false, true);
            _disc = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
            return _disc;
        }

        static Sprite Square()
        {
            if (_square != null) return _square;
            var tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);
            var px = new Color32[16];
            for (int i = 0; i < 16; i++) px[i] = new Color32(255, 255, 255, 255);
            tex.SetPixels32(px);
            tex.Apply(false, true);
            _square = Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 100f);
            return _square;
        }
    }
}
