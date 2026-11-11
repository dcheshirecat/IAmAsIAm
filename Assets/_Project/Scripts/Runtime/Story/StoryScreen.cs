using System;
using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.Grounding;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Triggers;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Story
{
    /// <summary>
    /// The story screen, built in code (no prefabs) so the game runs straight from a generated scene,
    /// including on Android. It shows dialogue and choices, runs the interactive moments the story
    /// asks for (grounding exercises, tarot, crafting, exploring a tradition), shows trigger moments,
    /// and keeps the Anchor button on screen at all times.
    /// Placeholder look: final UI art replaces these panels (see docs/ART_LIST.md, UI).
    /// The landscape reference canvas is 2732 x 2048, scaled by height (like Seven Witches' PC layout).
    /// </summary>
    public sealed class StoryScreen : MonoBehaviour
    {
        [SerializeField] StoryRunner runner;
        [SerializeField] string startKnot = "";

        static readonly Color TextColor = new Color(0.95f, 0.92f, 0.86f);
        static readonly Color PanelColor = new Color(0.10f, 0.09f, 0.12f, 0.88f);
        static readonly Color ButtonColor = new Color(0.28f, 0.22f, 0.30f, 0.95f);

        Font _font;
        RectTransform _root;
        Text _speaker, _line, _frontLabel, _triggerText, _actionTitle, _actionBody, _anchorBody;
        GameObject _dialogue, _choices, _action, _anchor, _triggerPanel;
        RectTransform _actionButtons, _breathCircle;
        Image _fog;
        InputAction _advance, _anchorKey;
        bool _anchorOpen;
        float _previousTimeScale = 1f, _triggerHideAt;

        BreathingPacer _pacer;
        Action _onPacerTick;

        void Awake()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            BuildCanvas();

            _advance = new InputAction("Advance", binding: "<Keyboard>/space");
            _advance.AddBinding("<Keyboard>/enter");
            _advance.AddBinding("<Gamepad>/buttonSouth");
            _advance.performed += _ => { if (!_anchorOpen && _dialogue.activeSelf && !_choices.activeSelf) runner.Next(); };

            // Esc is also the Android back button in the Input System.
            _anchorKey = new InputAction("Anchor", binding: "<Keyboard>/escape");
            _anchorKey.AddBinding("<Gamepad>/start");
            _anchorKey.performed += _ => ToggleAnchor();
        }

        void OnEnable()
        {
            _advance.Enable();
            _anchorKey.Enable();
            runner.LineReady += ShowLine;
            runner.ChoicesReady += ShowChoices;
            runner.ActionRequested += RunAction;
            runner.TriggerFired += ShowTrigger;
            runner.Ended += () => ShowLine(new StoryLine { Text = "(The end of what's written so far.)" });
        }

        void OnDisable()
        {
            _advance.Disable();
            _anchorKey.Disable();
            runner.LineReady -= ShowLine;
            runner.ChoicesReady -= ShowChoices;
            runner.ActionRequested -= RunAction;
            runner.TriggerFired -= ShowTrigger;
        }

        void Start()
        {
            if (GameServices.Roster != null) GameServices.Roster.FrontChanged += _ => UpdateFrontLabel();
            UpdateFrontLabel();
            runner.Begin(startKnot);
        }

        void Update()
        {
            var mind = GameServices.Mind;
            if (mind != null && _fog != null)
            {
                // A simple stand-in for the URP dissociation volumes: the screen greys as presence drops.
                float target = GameServices.Comfort.CapDistortion(1f - mind.Presence) * 0.55f;
                var c = _fog.color;
                c.a = Mathf.MoveTowards(c.a, target, Time.unscaledDeltaTime / 2f);
                _fog.color = c;
            }

            if (_pacer != null)
            {
                _pacer.Tick(Time.unscaledDeltaTime);
                if (_breathCircle != null) _breathCircle.localScale = Vector3.one * Mathf.Lerp(0.55f, 1.1f, _pacer.Fullness);
                _onPacerTick?.Invoke();
            }

            if (_triggerPanel.activeSelf && Time.unscaledTime > _triggerHideAt) _triggerPanel.SetActive(false);
        }

        // ── Dialogue ──────────────────────────────────────────────────────────────

        void ShowLine(StoryLine line)
        {
            _choices.SetActive(false);
            _dialogue.SetActive(true);
            _speaker.text = line.Speaker == null ? "" : line.Inner ? line.Speaker + " (inside)" : line.Speaker;
            _line.fontStyle = line.Inner ? FontStyle.Italic : FontStyle.Normal;
            _line.text = line.Text;
        }

        void ShowChoices(List<Choice> choices)
        {
            Clear(_choices.transform);
            _choices.SetActive(true);
            for (int i = 0; i < choices.Count; i++)
            {
                int index = i;
                Button(_choices.transform, choices[i].text, () => { _choices.SetActive(false); runner.Choose(index); }, 120);
            }
        }

        void UpdateFrontLabel()
        {
            var roster = GameServices.Roster;
            if (roster == null || _frontLabel == null) return;
            var def = roster.GetDefinition(roster.FrontingId);
            _frontLabel.text = def != null ? def.displayName : roster.FrontingId;
            if (def != null && ColorUtility.TryParseHtmlString(def.colorHex, out var col)) _frontLabel.color = col;
        }

        // ── Triggers: no choice, just what happened ───────────────────────────────

        void ShowTrigger(TriggerActivation a)
        {
            _triggerPanel.SetActive(true);
            _triggerHideAt = Time.unscaledTime + 4f;
            switch (a.Response)
            {
                case TriggerResponse.Depersonalization: _triggerText.text = "You're watching from somewhere behind your own eyes."; break;
                case TriggerResponse.Derealization: _triggerText.text = "The room goes flat, like a photograph of a room."; break;
                case TriggerResponse.Fog: _triggerText.text = "Everything goes soft, and very far away."; break;
                default: _triggerText.text = "…\n\nTime has passed."; UpdateFrontLabel(); break;
            }
            if (a.KnowledgeAfter == TriggerKnowledge.Noticed) _triggerText.text += "\n\n<i>" + a.Trigger.hint + "</i>";
            if (a.KnowledgeAfter == TriggerKnowledge.Known) _triggerText.text += "\n\n<i>You know this one now: " + a.Trigger.name + ".</i>";
        }

        // ── Interactive moments ───────────────────────────────────────────────────

        void RunAction(StoryAction action)
        {
            _dialogue.SetActive(false);
            _choices.SetActive(false);
            switch (action.Kind)
            {
                case "exercise": RunExercise(action.Value); break;
                case "ritual": RunRitual(action.Value); break;
                case "explore": RunExplore(action.Value); break;
                case "chapter-end": RunRecap(); break;
                default: Finish(); break;
            }
        }

        void RunExercise(string skillId)
        {
            var skill = GameServices.Skills?.Get(skillId);
            GameServices.Skills?.Learn(skillId);

            if (skillId == "five-senses")
            {
                var ex = new FiveSensesExercise();
                void Refresh()
                {
                    if (ex.IsComplete) { Finish(ex.ToOutcome(), skillId); return; }
                    string sense = ex.CurrentSense.ToString().ToLowerInvariant();
                    OpenAction("5-4-3-2-1",
                        $"Look around wherever you really are.\n\nNotice something you can <b>{sense}</b>.\n({ex.NoticedInStep} of {ex.CurrentTarget})",
                        ("I noticed one", () => { ex.Notice(ex.CurrentSense); Refresh(); }),
                        ("Skip this sense", () => { ex.SkipCurrentSense(); Refresh(); }));
                }
                Refresh();
                return;
            }

            if (skillId == "paced-breathing" || skillId == "box-breathing")
            {
                var pattern = skillId == "box-breathing" ? BreathingPattern.Box : BreathingPattern.Paced;
                if (!GameServices.Comfort.allowBreathHolds) pattern = pattern.WithoutHolds();
                StartPacer(pattern, 6);
                OpenAction(skill?.name ?? "Breathing", "", ("I'm done", () =>
                {
                    float completion = _pacer?.Completion ?? 0f;
                    StopPacer();
                    Finish(new GroundingOutcome(SkillDirection.LowersArousal, Mathf.Max(0.3f, completion)), skillId);
                }));
                _onPacerTick = () => _actionBody.text = PhaseText(_pacer.Phase) + $"\n\n{_pacer.CompletedCycles} of {_pacer.TargetCycles}";
                return;
            }

            // Any other skill: show its real steps from the codex.
            string body = skill == null ? skillId : skill.summary + "\n\n" + string.Join("\n", skill.steps.Select((s, i) => $"{i + 1}. {s}"));
            OpenAction(skill?.name ?? skillId, body, ("Done", () =>
                Finish(new GroundingOutcome(skill?.Direction ?? SkillDirection.Either, 1f), skillId)));
        }

        void RunRitual(string value)
        {
            var parts = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var lib = GameServices.Practices;
            string kind = parts.Length > 0 ? parts[0] : "";
            string arg = parts.Length > 1 ? parts[1] : "";

            if (kind == "tarot")
            {
                var spread = GameServices.Spreads?.spreads.FirstOrDefault(s => s.id == arg);
                if (spread == null || lib.Tarot.Count == 0) { Finish(); return; }
                var deck = new TarotDeck(lib.Tarot, Environment.TickCount) { AllowReversals = GameServices.Comfort.allowTarotReversals };
                var roster = GameServices.Roster;
                var present = new[] { roster.FrontingId }.Concat(roster.CoConscious);
                var reading = TarotReader.Read(spread, deck, present);

                string body = string.Join("\n\n", reading.Cards.Select(c =>
                    $"<b>{c.PositionLabel}: {c.Draw.Card.name}{(c.Draw.Reversed ? " (reversed)" : "")}</b>\n{c.Draw.Card.reflectionPrompt}" +
                    (string.IsNullOrEmpty(c.Draw.Card.gentleNote) ? "" : $"\n<i>{c.Draw.Card.gentleNote}</i>")));
                GameServices.Journal?.Write(JournalEntryKind.TarotReflection, roster.FrontingId, GameServices.InGameDay,
                    spread.name + ": " + string.Join(", ", reading.Cards.Select(c => c.Draw.Card.name)), spread.id);
                OpenAction(spread.name, body, ("Continue", () => { CloseAction(); runner.CompleteAction(reading.ToStoryVariables()); }));
                return;
            }

            if (kind == "rune")
            {
                if (lib.Runes.Count == 0) { Finish(); return; }
                var rune = new RunePouch(lib.Runes, Environment.TickCount).Pull();
                OpenAction($"{rune.glyph}  {rune.name}", $"{rune.literalMeaning}\n\n{rune.reflectionPrompt}",
                    ("Continue", () => { CloseAction(); runner.CompleteAction(new Dictionary<string, string> { ["rune_pulled"] = rune.id }); }));
                return;
            }

            if (kind == "craft")
            {
                var charm = lib.Charms.FirstOrDefault(c => c.id == arg);
                if (charm == null) { Finish(); return; }
                var craft = CraftRitual.ForCharm(charm);
                void Step()
                {
                    if (craft.IsComplete)
                    {
                        OpenAction(charm.name, charm.sensory + "\n\n" + charm.reflectionPrompt, ("Continue", Finish));
                        return;
                    }
                    OpenAction(charm.name, $"{craft.CurrentStep}\n\n<i>One thing at a time.</i>", ("Done", () => { craft.CompleteStep(); Step(); }));
                }
                Step();
                return;
            }

            Finish();
        }

        void RunExplore(string traditionId)
        {
            var t = GameServices.Traditions?.Get(traditionId);
            if (t == null) { Finish(); return; }
            GameServices.Traditions.Explore(traditionId);
            string tries = t.tryInGame.Count == 0 ? "" : "\n\n<b>You could try:</b> " + string.Join(", ", t.tryInGame.Select(p => p.name));
            OpenAction(t.name, $"{t.overview}\n\n<i>{t.opennessNote}</i>{tries}", ("Continue", Finish));
        }

        void RunRecap()
        {
            var choices = GameServices.Choices?.Records ?? new List<IAmAsIAm.Core.Story.ChoiceRecord>();
            string body = choices.Count == 0 ? "No choices recorded yet." : string.Join("\n", choices.Select(c => "• " + c.summary));
            OpenAction("Your choices", body, ("Continue", Finish));
        }

        void Finish() { CloseAction(); runner.CompleteAction(); }

        void Finish(GroundingOutcome outcome, string skillId)
        {
            GameServices.Mind?.ApplyGrounding(outcome);
            GameServices.Mind?.RecordPractice();
            GameServices.Skills?.RecordPractice(skillId);
            Finish();
        }

        void OpenAction(string title, string body, params (string label, Action onClick)[] buttons)
        {
            _action.SetActive(true);
            _actionTitle.text = title;
            _actionBody.text = body;
            Clear(_actionButtons);
            foreach (var (label, onClick) in buttons) Button(_actionButtons, label, onClick, 110);
        }

        void CloseAction()
        {
            _action.SetActive(false);
            _breathCircle.gameObject.SetActive(false);
            _onPacerTick = null;
        }

        // ── The Anchor: always available ──────────────────────────────────────────

        void ToggleAnchor()
        {
            if (_anchorOpen) CloseAnchor();
            else OpenAnchor();
        }

        void OpenAnchor()
        {
            _anchorOpen = true;
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            _anchor.SetActive(true);
            _anchorBody.text = "You're safe to pause here for as long as you need.\nNothing in the story moves while you're here.";
        }

        void CloseAnchor()
        {
            _anchorOpen = false;
            if (!_action.activeSelf) StopPacer();
            _anchor.SetActive(false);
            Time.timeScale = _previousTimeScale <= 0f ? 1f : _previousTimeScale;
        }

        void AnchorBreathe()
        {
            var pattern = GameServices.Comfort.allowBreathHolds ? BreathingPattern.Paced : BreathingPattern.Paced.WithoutHolds();
            StartPacer(pattern, 0);
            _onPacerTick = () => _anchorBody.text = PhaseText(_pacer.Phase);
        }

        void AnchorSupport()
        {
            StopPacer();
            var data = GameServices.Crisis;
            _anchorBody.text = data == null ? "" : data.disclaimer + "\n\n" +
                string.Join("\n\n", data.resources.Select(r => $"<b>{r.name}</b>  {r.contact}\n{r.url}"));
        }

        void StartPacer(BreathingPattern pattern, int cycles)
        {
            _pacer = new BreathingPacer(pattern, cycles);
            _breathCircle.gameObject.SetActive(true);
        }

        void StopPacer()
        {
            _pacer = null;
            _onPacerTick = null;
            if (_breathCircle != null) _breathCircle.gameObject.SetActive(false);
        }

        static string PhaseText(BreathPhase p) => p switch
        {
            BreathPhase.Inhale => "Breathe in…",
            BreathPhase.HoldIn => "Hold gently…",
            BreathPhase.Exhale => "And slowly out…",
            _ => "Rest…",
        };

        // ── Building the UI ───────────────────────────────────────────────────────

        void BuildCanvas()
        {
            if (FindAnyObjectByType<EventSystem>() == null)
            {
                var es = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
                es.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();
            }

            var canvasGo = new GameObject("Story Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2732, 2048);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;
            _root = canvasGo.GetComponent<RectTransform>();

            // Fog overlay (presence), under everything else.
            _fog = Panel(_root, "Fog", new Color(0.55f, 0.56f, 0.6f, 0f), Vector2.zero, Vector2.one).GetComponent<Image>();
            _fog.raycastTarget = false;

            // Tap anywhere to continue.
            var tap = Panel(_root, "Tap", new Color(0, 0, 0, 0), Vector2.zero, Vector2.one);
            tap.AddComponent<Button>().onClick.AddListener(() => { if (!_anchorOpen && !_choices.activeSelf && !_action.activeSelf) runner.Next(); });

            _dialogue = Panel(_root, "Dialogue", PanelColor, new Vector2(0.05f, 0.03f), new Vector2(0.95f, 0.33f));
            _dialogue.GetComponent<Image>().raycastTarget = false;
            _speaker = Label(_dialogue.transform, "Speaker", 56, FontStyle.Bold, new Vector2(0.03f, 0.75f), new Vector2(0.97f, 0.95f));
            _line = Label(_dialogue.transform, "Line", 54, FontStyle.Normal, new Vector2(0.03f, 0.08f), new Vector2(0.97f, 0.75f));

            _choices = Panel(_root, "Choices", new Color(0, 0, 0, 0), new Vector2(0.2f, 0.36f), new Vector2(0.8f, 0.9f));
            var vlg = _choices.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = 24; vlg.childAlignment = TextAnchor.LowerCenter;
            vlg.childControlHeight = false; vlg.childControlWidth = true; vlg.childForceExpandHeight = false;
            _choices.SetActive(false);

            _action = Panel(_root, "Action", PanelColor, new Vector2(0.15f, 0.08f), new Vector2(0.85f, 0.92f));
            _actionTitle = Label(_action.transform, "Title", 70, FontStyle.Bold, new Vector2(0.05f, 0.86f), new Vector2(0.95f, 0.97f));
            _actionTitle.alignment = TextAnchor.MiddleCenter;
            _actionBody = Label(_action.transform, "Body", 50, FontStyle.Normal, new Vector2(0.06f, 0.22f), new Vector2(0.94f, 0.85f));
            _actionBody.alignment = TextAnchor.UpperCenter;
            _actionButtons = Panel(_action.transform, "Buttons", new Color(0, 0, 0, 0), new Vector2(0.1f, 0.03f), new Vector2(0.9f, 0.2f)).GetComponent<RectTransform>();
            var hlg = _actionButtons.gameObject.AddComponent<HorizontalLayoutGroup>();
            hlg.spacing = 30; hlg.childControlWidth = true; hlg.childForceExpandWidth = true; hlg.childControlHeight = false;
            hlg.childAlignment = TextAnchor.MiddleCenter;
            _action.SetActive(false);

            var circle = Panel(_root, "Breath", new Color(0.75f, 0.68f, 0.85f, 0.45f), new Vector2(0.5f, 0.55f), new Vector2(0.5f, 0.55f));
            _breathCircle = circle.GetComponent<RectTransform>();
            _breathCircle.sizeDelta = new Vector2(520, 520);
            circle.GetComponent<Image>().raycastTarget = false;
            circle.SetActive(false);

            _triggerPanel = Panel(_root, "Trigger", new Color(0.05f, 0.05f, 0.07f, 0.8f), new Vector2(0.2f, 0.4f), new Vector2(0.8f, 0.7f));
            _triggerPanel.GetComponent<Image>().raycastTarget = false;
            _triggerText = Label(_triggerPanel.transform, "Text", 56, FontStyle.Italic, new Vector2(0.05f, 0.05f), new Vector2(0.95f, 0.95f));
            _triggerText.alignment = TextAnchor.MiddleCenter;
            _triggerPanel.SetActive(false);

            _frontLabel = Label(_root, "Fronting", 52, FontStyle.Bold, new Vector2(0.02f, 0.92f), new Vector2(0.4f, 0.99f));

            // The Anchor button: top right, always on top.
            var anchorButton = Panel(_root, "Anchor Button", ButtonColor, new Vector2(0.86f, 0.9f), new Vector2(0.98f, 0.98f));
            anchorButton.AddComponent<Button>().onClick.AddListener(ToggleAnchor);
            Label(anchorButton.transform, "Label", 48, FontStyle.Bold, Vector2.zero, Vector2.one).alignment = TextAnchor.MiddleCenter;
            anchorButton.GetComponentInChildren<Text>().text = "Anchor";

            _anchor = Panel(_root, "Anchor", new Color(0.12f, 0.14f, 0.13f, 0.97f), new Vector2(0.1f, 0.06f), new Vector2(0.84f, 0.88f));
            var anchorTitle = Label(_anchor.transform, "Title", 72, FontStyle.Bold, new Vector2(0.05f, 0.86f), new Vector2(0.95f, 0.97f));
            anchorTitle.text = "Anchor";
            anchorTitle.alignment = TextAnchor.MiddleCenter;
            _anchorBody = Label(_anchor.transform, "Body", 48, FontStyle.Normal, new Vector2(0.06f, 0.24f), new Vector2(0.94f, 0.84f));
            _anchorBody.alignment = TextAnchor.UpperCenter;
            var anchorButtons = Panel(_anchor.transform, "Buttons", new Color(0, 0, 0, 0), new Vector2(0.05f, 0.04f), new Vector2(0.95f, 0.2f));
            var ahlg = anchorButtons.AddComponent<HorizontalLayoutGroup>();
            ahlg.spacing = 30; ahlg.childControlWidth = true; ahlg.childForceExpandWidth = true; ahlg.childControlHeight = false;
            ahlg.childAlignment = TextAnchor.MiddleCenter;
            Button(anchorButtons.transform, "Breathe", AnchorBreathe, 110);
            Button(anchorButtons.transform, "Support", AnchorSupport, 110);
            Button(anchorButtons.transform, "Return when ready", CloseAnchor, 110);
            _anchor.SetActive(false);
        }

        GameObject Panel(Transform parent, string name, Color color, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = min; rt.anchorMax = max; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
            go.GetComponent<Image>().color = color;
            return go;
        }

        Text Label(Transform parent, string name, int size, FontStyle style, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = min; rt.anchorMax = max; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
            var t = go.GetComponent<Text>();
            t.font = _font; t.fontSize = size; t.fontStyle = style; t.color = TextColor;
            t.supportRichText = true; t.raycastTarget = false;
            t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Overflow;
            return t;
        }

        void Button(Transform parent, string label, Action onClick, float height)
        {
            var go = new GameObject("Button", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            go.GetComponent<Image>().color = ButtonColor;
            go.GetComponent<LayoutElement>().preferredHeight = height;
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height);
            go.GetComponent<Button>().onClick.AddListener(() => onClick());
            var t = Label(go.transform, "Label", 46, FontStyle.Normal, Vector2.zero, Vector2.one);
            t.alignment = TextAnchor.MiddleCenter;
            t.text = label;
        }

        static void Clear(Transform t)
        {
            for (int i = t.childCount - 1; i >= 0; i--)
            {
                var child = t.GetChild(i);
                child.SetParent(null, false); // Leave the layout now; Destroy happens at frame end.
                Destroy(child.gameObject);
            }
        }
    }
}
