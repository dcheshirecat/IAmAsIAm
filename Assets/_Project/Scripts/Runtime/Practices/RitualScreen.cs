using System;
using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.Practices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Practices
{
    /// <summary>
    /// A ritual at Maya's altar, or one the story calls for (# ritual: candle). Owner: small hands-on steps that feel like
    /// rituals, not games: slow and quiet, never timed, no score, no failing, and "Not today" is always there.
    /// It shows one <see cref="RitualSession"/> step at a time: a breath, a hold, a tap, a choice, or a few words.
    /// When it's done, what she did goes in the Book of Shadows, a little grounding is applied, and the caller gets the result.
    /// Also the altar's "practice wheel": what the one fronting knows how to do. Built in code; placeholder look.
    /// </summary>
    public sealed class RitualScreen : MonoBehaviour
    {
        static readonly Color Ink = new Color(0.95f, 0.92f, 0.86f);
        static readonly Color Faded = new Color(0.72f, 0.68f, 0.62f);
        static readonly Color Veil = new Color(0.04f, 0.03f, 0.06f, 0.82f);
        static readonly Color Cloth = new Color(0.16f, 0.12f, 0.20f, 0.97f);
        static readonly Color ButtonColour = new Color(0.36f, 0.27f, 0.40f, 1f);
        static readonly Color Quiet = new Color(0.22f, 0.18f, 0.25f, 1f);
        static readonly Color Warm = new Color(1f, 0.84f, 0.55f, 1f);

        /// <summary>Above the phone and the Book (40), below the saves screen (50), the loading card (100) and the Anchor (200).</summary>
        const int SortingOrder = 45;

        public static bool IsOpen { get; private set; }

        Font _font;
        GameObject _root, _breath;
        Text _title, _phase, _body;
        RectTransform _actions;
        Image _holdFill;
        RitualSession _session;
        Action<RitualResult> _done;
        float _held, _breathStart;
        bool _holding;
        float _depersonalizationWas = -1f;

        CraftData _craft;

        void Awake()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _craft = DataLoader.Load<CraftData>("craft");
            Build();
        }

        void OnDisable() => IsOpen = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => IsOpen = false;

        // ── The practice wheel (at the altar) ────────────────────────────────────

        /// <summary>
        /// The altar's choices for whoever is fronting: the practices they've learned that can be done here. Calls
        /// <paramref name="closed"/> when the player leaves the altar (after a ritual, or "Not today").
        /// </summary>
        public void OpenWheel(Action closed)
        {
            var book = GameServices.Book;
            string front = GameServices.Roster?.FrontingId ?? "maya";
            Show();
            _title.text = "The altar";
            _phase.text = "";
            Clear(_actions);
            _breath.SetActive(false);
            var known = RitualSession.Supported.Where(id => book != null && book.HasLearned(id)).ToList();
            // Grounding is always there: it's the first thing Tanya teaches, and it's never wrong to start with it.
            if (!known.Contains("grounding-centering")) known.Insert(0, "grounding-centering");
            _body.text = "What would you like to do?";
            foreach (var id in known)
            {
                string practice = id;
                Button(_actions, book?.Practice(id)?.name ?? id, () => Run(practice, front, _ => closed?.Invoke()), ButtonColour);
            }
            Button(_actions, "Not today", () => { Hide(); closed?.Invoke(); }, Quiet);
        }

        /// <summary>
        /// For someone who doesn't practise Wicca (owner: they keep their own faith and leave the tools alone): one line in
        /// their voice at the altar, then back. (Their own small practices come later: docs/WiccanPractice.md step 6.)
        /// </summary>
        public void ShowLine(string who, string line, Action closed)
        {
            Show();
            _title.text = "The altar";
            _phase.text = "";
            _breath.SetActive(false);
            Clear(_actions);
            _body.text = $"<b>{who}:</b> {line}";
            Button(_actions, "Leave it", () => { Hide(); closed?.Invoke(); }, Quiet);
        }

        // ── A ritual ─────────────────────────────────────────────────────────────

        /// <summary>Runs one ritual. <paramref name="done"/> gets the result (null if she stopped: "Not today").</summary>
        public void Run(string practiceId, string memberId, Action<RitualResult> done)
        {
            if (!RitualSession.IsSupported(practiceId)) { done?.Invoke(null); return; }
            _session = new RitualSession(practiceId, memberId, _craft, GameServices.Practices, Environment.TickCount,
                GameServices.Comfort?.allowTarotReversals ?? false);
            _done = done;
            Show();
            _title.text = _session.Title;
            ShowStep();
        }

        void ShowStep()
        {
            Clear(_actions);
            _holding = false;
            _held = 0f;
            _holdFill = null;
            var step = _session.Current;
            if (step == null) { Finish(); return; }
            _phase.text = step.Phase switch
            {
                RitualPhase.Arrive => "Arrive",
                RitualPhase.Prepare => "Prepare",
                RitualPhase.Moment => "",
                RitualPhase.Reflect => "Reflect",
                _ => "Close",
            };
            _body.text = step.Text;

            // Owner: practice can sometimes be hard. A card that stirs something up starts a gentle depersonalization.
            if (step.Phase == RitualPhase.Moment && _session.Result != null && _session.Result.StirredUp && _depersonalizationWas < 0f)
            {
                var mind = GameServices.Mind;
                if (mind != null)
                {
                    _depersonalizationWas = mind.GetIntensity(DissociativeExperience.Depersonalization);
                    mind.SetExperience(DissociativeExperience.Depersonalization, Mathf.Max(_depersonalizationWas, 0.3f));
                }
            }

            _breath.SetActive(step.Kind == RitualStepKind.Breathe);
            _breathStart = Time.unscaledTime;
            switch (step.Kind)
            {
                case RitualStepKind.Choose:
                    foreach (var o in step.Options)
                    {
                        string id = o.Id;
                        Button(_actions, o.Label, () => { _session.Answer(id); ShowStep(); }, ButtonColour);
                    }
                    break;
                case RitualStepKind.Write:
                    var field = Field(_actions, step.MaxLength > 0 && step.MaxLength <= 30 ? "One word..." : "Write it down...", step.MaxLength > 0 && step.MaxLength <= 30 ? 110 : 360, step.MaxLength);
                    Button(_actions, step.Action, () => { _session.Answer(field.text); ShowStep(); }, ButtonColour);
                    field.ActivateInputField();
                    break;
                case RitualStepKind.Hold:
                    HoldButton(_actions, step.Action);
                    break;
                default:
                    Button(_actions, step.Action, () => { _session.Answer(); ShowStep(); }, ButtonColour);
                    break;
            }
            Button(_actions, "Not today", LeaveEarly, Quiet, 90, 34);
        }

        void Update()
        {
            if (!IsOpen) return;
            bool reduce = GameServices.Comfort?.reduceMotion ?? false;
            if (_breath.activeSelf)
            {
                // One slow breath: about ten seconds in and out; never fast. With Reduce motion it stays still.
                float t = (Time.unscaledTime - _breathStart) * Mathf.PI * 2f / 10f;
                float s = reduce ? 0.85f : Mathf.Lerp(0.6f, 1.1f, 0.5f - 0.5f * Mathf.Cos(t));
                _breath.transform.localScale = Vector3.one * s;
            }
            var step = _session?.Current;
            if (step != null && step.Kind == RitualStepKind.Hold && _holdFill != null)
            {
                // Keyboard / gamepad can hold too (Space, E, or A).
                bool key = (Keyboard.current != null && (Keyboard.current.spaceKey.isPressed || Keyboard.current.eKey.isPressed))
                           || (Gamepad.current != null && Gamepad.current.buttonSouth.isPressed);
                // Letting go early only pauses: nothing is lost, nothing fails.
                if (_holding || key) _held += Time.unscaledDeltaTime;
                float need = Mathf.Max(0.5f, step.Seconds);
                _holdFill.fillAmount = Mathf.Clamp01(_held / need);
                if (_held >= need) { _session.Answer(); ShowStep(); }
            }
        }

        void LeaveEarly()
        {
            _session?.LeaveEarly();
            Finish();
        }

        void Finish()
        {
            var result = _session != null && !_session.LeftEarly ? _session.Result : null;
            if (_depersonalizationWas >= 0f)
            {
                GameServices.Mind?.SetExperience(DissociativeExperience.Depersonalization, _depersonalizationWas);
                _depersonalizationWas = -1f;
            }
            if (result != null) Apply(result);
            _session = null;
            Hide();
            var done = _done;
            _done = null;
            done?.Invoke(result);
        }

        /// <summary>What a finished ritual leaves behind: the Book entry, a little grounding, the practice remembered.</summary>
        static void Apply(RitualResult r)
        {
            var book = GameServices.Book;
            if (book != null)
            {
                book.Learn(r.PracticeId, "", GameServices.InGameDay, GameServices.ChapterRuns?.CurrentChapterId ?? "");
                if (book.Has && !string.IsNullOrWhiteSpace(r.Text))
                    book.Write(r.Tab, r.MemberId, r.Title, r.Text, GameServices.InGameDay, GameServices.ChapterRuns?.CurrentChapterId ?? "", byPlayer: true);
            }
            if (r.Grounding > 0f) GameServices.Mind?.Nudge(-r.Grounding);
            GameServices.Mind?.RecordPractice();
        }

        void Show()
        {
            IsOpen = true;
            _root.SetActive(true);
        }

        void Hide()
        {
            _root.SetActive(false);
            IsOpen = false;
        }

        // ── Building the UI ──────────────────────────────────────────────────────

        void Build()
        {
            var canvasGo = new GameObject("Ritual Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2732, 2048);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;

            // A dim veil over the world (the camera stays on the altar), and a cloth in the middle.
            _root = Panel(canvasGo.transform, "Ritual", Veil, Vector2.zero, Vector2.one);
            var cloth = Panel(_root.transform, "Cloth", Cloth, new Vector2(0.24f, 0.08f), new Vector2(0.76f, 0.92f));

            _title = MakeText(cloth.transform, "", 56, FontStyle.Bold, Warm, TextAnchor.MiddleCenter);
            Place(_title.rectTransform, new Vector2(0.05f, 0.89f), new Vector2(0.95f, 0.97f));
            _phase = MakeText(cloth.transform, "", 34, FontStyle.Italic, Faded, TextAnchor.MiddleCenter);
            Place(_phase.rectTransform, new Vector2(0.05f, 0.84f), new Vector2(0.95f, 0.89f));

            // The breathing circle (a soft disc), shown for the "arrive" breath.
            _breath = new GameObject("Breath", typeof(RectTransform), typeof(Image));
            _breath.transform.SetParent(cloth.transform, false);
            var brt = _breath.GetComponent<RectTransform>();
            brt.anchorMin = brt.anchorMax = new Vector2(0.5f, 0.70f);
            brt.sizeDelta = new Vector2(300, 300);
            var bimg = _breath.GetComponent<Image>();
            bimg.sprite = Disc();
            bimg.color = new Color(Warm.r, Warm.g, Warm.b, 0.35f);
            bimg.raycastTarget = false;
            _breath.SetActive(false);

            _body = MakeText(cloth.transform, "", 44, FontStyle.Normal, Ink, TextAnchor.MiddleCenter);
            Place(_body.rectTransform, new Vector2(0.08f, 0.50f), new Vector2(0.92f, 0.82f));
            _body.verticalOverflow = VerticalWrapMode.Overflow;

            var go = new GameObject("Actions", typeof(RectTransform));
            go.transform.SetParent(cloth.transform, false);
            _actions = go.GetComponent<RectTransform>();
            Place(_actions, new Vector2(0.12f, 0.04f), new Vector2(0.88f, 0.48f));
            var g = go.AddComponent<VerticalLayoutGroup>();
            g.spacing = 16;
            g.childControlWidth = true; g.childForceExpandWidth = true;
            g.childControlHeight = true; g.childForceExpandHeight = false;
            g.childAlignment = TextAnchor.LowerCenter;

            _root.SetActive(false);
        }

        void HoldButton(Transform parent, string label)
        {
            var go = new GameObject("Hold", typeof(RectTransform), typeof(Image), typeof(LayoutElement), typeof(RitualHoldArea));
            go.transform.SetParent(parent, false);
            go.GetComponent<Image>().color = ButtonColour;
            go.GetComponent<LayoutElement>().preferredHeight = 150;
            var area = go.GetComponent<RitualHoldArea>();
            area.Changed = down => _holding = down;

            var fill = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            fill.transform.SetParent(go.transform, false);
            Place(fill.GetComponent<RectTransform>(), Vector2.zero, Vector2.one);
            _holdFill = fill.GetComponent<Image>();
            _holdFill.sprite = Square();
            _holdFill.type = Image.Type.Filled;
            _holdFill.fillMethod = Image.FillMethod.Horizontal;
            _holdFill.fillAmount = 0f;
            _holdFill.color = new Color(Warm.r, Warm.g, Warm.b, 0.35f);
            _holdFill.raycastTarget = false;

            var t = MakeText(go.transform, label + "  (hold)", 44, FontStyle.Normal, Ink, TextAnchor.MiddleCenter);
            Stretch(t.rectTransform, 16);
        }

        InputField Field(Transform parent, string hint, float height, int limit)
        {
            var go = new GameObject("Field", typeof(RectTransform), typeof(Image), typeof(InputField), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            go.GetComponent<LayoutElement>().preferredHeight = height;
            go.GetComponent<Image>().color = new Color(0.95f, 0.92f, 0.86f);
            var dark = new Color(0.12f, 0.1f, 0.12f);
            var text = MakeText(go.transform, "", 40, FontStyle.Normal, dark, TextAnchor.UpperLeft);
            Place(text.rectTransform, Vector2.zero, Vector2.one);
            text.rectTransform.offsetMin = new Vector2(24, 14); text.rectTransform.offsetMax = new Vector2(-24, -14);
            text.supportRichText = false;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            var ph = MakeText(go.transform, hint, 40, FontStyle.Italic, new Color(dark.r, dark.g, dark.b, 0.45f), TextAnchor.UpperLeft);
            Place(ph.rectTransform, Vector2.zero, Vector2.one);
            ph.rectTransform.offsetMin = new Vector2(24, 14); ph.rectTransform.offsetMax = new Vector2(-24, -14);
            var input = go.GetComponent<InputField>();
            input.textComponent = text;
            input.placeholder = ph;
            input.lineType = height > 120 ? InputField.LineType.MultiLineNewline : InputField.LineType.SingleLine;
            if (limit > 0) input.characterLimit = limit;
            return input;
        }

        GameObject Panel(Transform parent, string name, Color color, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            Place(go.GetComponent<RectTransform>(), min, max);
            go.GetComponent<Image>().color = color;
            return go;
        }

        void Button(Transform parent, string label, Action onClick, Color color, float height = 120, int size = 42)
        {
            var go = new GameObject("Button", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            go.GetComponent<Image>().color = color;
            go.GetComponent<LayoutElement>().preferredHeight = height;
            go.GetComponent<Button>().onClick.AddListener(() => onClick());
            var t = MakeText(go.transform, label, size, FontStyle.Normal, Ink, TextAnchor.MiddleCenter);
            Stretch(t.rectTransform, 16);
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
                    px[y * size + x] = new Color32(255, 255, 255, (byte)(255 * Mathf.Clamp01((1f - d) * 3f)));
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
