using System;
using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.Grounding;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Safety;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Triggers;
using IAmAsIAm.Runtime.CameraRig;
using IAmAsIAm.Runtime.Interaction;
using IAmAsIAm.Runtime.Player;
using IAmAsIAm.Runtime.Triggers;
using IAmAsIAm.Runtime.World;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
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
        [Tooltip("Optional knot to play as soon as the scene starts (e.g. a chapter opening). Empty = free roam.")]
        [SerializeField] string startKnot = "";
        [SerializeField] ProximityInteractor interactor;
        [SerializeField] ShoulderCamera cameraRig;

        static readonly Color TextColor = new Color(0.95f, 0.92f, 0.86f);
        static readonly Color PanelColor = new Color(0.10f, 0.09f, 0.12f, 0.88f);
        static readonly Color ButtonColor = new Color(0.28f, 0.22f, 0.30f, 0.95f);

        Font _font;
        RectTransform _root;
        Text _speaker, _line, _frontLabel, _triggerText, _actionTitle, _actionBody, _anchorBody;
        GameObject _dialogue, _choices, _action, _anchor, _triggerPanel, _tap, _touch;
        Text _prompt, _objective, _objectiveCard;
        float _objectiveCardUntil;
        RectTransform _actionButtons, _breathCircle;
        IAmAsIAm.Runtime.Mind.FogOverlay _fog;
        GameObject _chaptersButton;
        bool _wantChapterList, _offeredStart;
        InputAction _advance, _anchorKey;
        bool _anchorOpen;
        float _previousTimeScale = 1f, _triggerHideAt;
        int _lineShownFrame = -1;

        /// <summary>True while dialogue, a choice, an interactive moment or the Anchor is up (the saves menu waits).</summary>
        public bool IsBusy { get; private set; }
        /// <summary>Above the loading screen (100) and the saves screen (50): the Anchor is always reachable.</summary>
        const int AnchorSortingOrder = 200;

        BreathingPacer _pacer;
        Action _onPacerTick;
        IAmAsIAm.Runtime.Devices.DeviceScreen _devices;
        IAmAsIAm.Runtime.Practices.BookScreen _book;
        IAmAsIAm.Runtime.Practices.RitualScreen _ritual;
        StoryWorld _world;
        MapScreen _map;
        ChapterTreeScreen _tree;
        IAmAsIAm.Runtime.Audio.VoicePlayer _voice;
        IAmAsIAm.Core.Story.PlaceData _places;
        readonly HashSet<string> _visited = new HashSet<string>();
        /// <summary>The last thing used in the world ("Go" means she just walked out: no second walk to get there).</summary>
        string _lastWorldVerb = "";
        GameObject _pocket;
        /// <summary>True while the story's explore choices are out in the world: the character can walk to them.</summary>
        bool _exploring;

        void Awake()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            BuildCanvas();
            // Maya's phone / computer / laptop (texts and email). Its own canvas, under the Anchor.
            _devices = gameObject.AddComponent<IAmAsIAm.Runtime.Devices.DeviceScreen>();
            // The phone and laptop are always usable (owner), except over the Anchor or an interactive moment.
            _devices.Blocked = () => _anchorOpen || (_action != null && _action.activeSelf) || IAmAsIAm.Runtime.Practices.RitualScreen.IsOpen;
            // The Book of Shadows (owner: Tanya gives it at Yule, Dec 2017): an icon under the phone, usable any time.
            _book = gameObject.AddComponent<IAmAsIAm.Runtime.Practices.BookScreen>();
            _book.Blocked = () => _anchorOpen || (_action != null && _action.activeSelf) || IAmAsIAm.Runtime.Practices.RitualScreen.IsOpen;
            // Rituals (owner: they feel like rituals, not games), at the altar or when the story calls for one.
            _ritual = gameObject.AddComponent<IAmAsIAm.Runtime.Practices.RitualScreen>();
            // The charm bag in her pocket (owner: hold it to ground; alters react to it): shows while she's drifting.
            // The moon tonight (owner: a moon phase always shown), from the story's date.
            var moon = gameObject.AddComponent<IAmAsIAm.Runtime.Practices.MoonBadge>();
            moon.Visible = () => IAmAsIAm.Runtime.Save.SaveService.ActiveSlot != 0;
            var charmBag = gameObject.AddComponent<IAmAsIAm.Runtime.Practices.CharmBagButton>();
            charmBag.Blocked = () => _anchorOpen || (_action != null && _action.activeSelf) || ChapterTreeScreen.IsOpen || MapScreen.IsOpen;
            // Not while replaying a chapter from before she had it (the Prologue and the Prelude).
            _book.Available = () => !runner.IsPlaying
                || (GameServices.Chapter(runner.ChapterId)?.number ?? int.MaxValue) > (GameServices.Chapter("prelude-wicca")?.number ?? -1);
            // The places the story happens in, to walk around and explore (owner).
            _world = gameObject.AddComponent<StoryWorld>();
            _world.Init(cameraRig != null ? cameraRig.Target : null, () => { if (cameraRig != null) cameraRig.SnapBehind(); });
            _map = gameObject.AddComponent<MapScreen>();
            _tree = gameObject.AddComponent<ChapterTreeScreen>();
            // The owner's recorded voice lines (never generated): played when their line shows, if the file is there.
            _voice = gameObject.AddComponent<IAmAsIAm.Runtime.Audio.VoicePlayer>();
            _places = DataLoader.Load<IAmAsIAm.Core.Story.PlaceData>("places");
            // Maya's altar in each of her homes (owner): usable while exploring there.
            _world.HasAltar = id => _places?.Find(id)?.altar ?? false;
            _world.AltarUsable = () => _exploring && !_anchorOpen && !IAmAsIAm.Runtime.Practices.RitualScreen.IsOpen
                && !IAmAsIAm.Runtime.Devices.DeviceScreen.IsOpen && !IAmAsIAm.Runtime.Practices.BookScreen.IsOpen;
            _world.AltarUsed = OpenAltar;
            // The phone's Maps tab: the city she's in, just to look (from Chapter 1 on).
            _devices.MapAvailable = () => !string.IsNullOrEmpty(_lastCity) && !(GameServices.Chapter(runner.ChapterId)?.doorToDoor ?? true) && runner.IsPlaying;
            // SystemLog (owner: from the end of Chapter 4): not while replaying a chapter from before it.
            _devices.SystemLogAvailable = () => !runner.IsPlaying
                || (GameServices.Chapter(runner.ChapterId)?.number ?? int.MaxValue) >= (GameServices.Chapter("ch4-islam")?.number ?? 0);
            _devices.ShowMap = () => _map.Show(_places, _lastCity, _world.Place, null, _visited, null, null);

            _advance = new InputAction("Advance", binding: "<Keyboard>/space");
            _advance.AddBinding("<Keyboard>/enter");
            _advance.AddBinding("<Gamepad>/buttonSouth");
            // Ignore the press that opened this line (Use and Advance share gamepad A).
            _advance.performed += _ => { if (!_anchorOpen && !IAmAsIAm.Runtime.Devices.DeviceScreen.IsOpen && !IAmAsIAm.Runtime.Practices.BookScreen.IsOpen && !IAmAsIAm.Runtime.Practices.RitualScreen.IsOpen && _dialogue.activeSelf && !_choices.activeSelf && Time.frameCount > _lineShownFrame) runner.Next(); };

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
            TriggerRunner.Fired += ShowTrigger;
            runner.Started += OnStoryStarted;
            runner.Ended += OnStoryEnded;
            runner.ObjectiveChanged += ShowObjective;
            runner.PlaceChanged += OnPlace;
            IAmAsIAm.Runtime.Save.SaveService.Loaded += UpdateFrontLabel;
        }

        void OnDisable()
        {
            _advance.Disable();
            _anchorKey.Disable();
            runner.LineReady -= ShowLine;
            runner.ChoicesReady -= ShowChoices;
            runner.ActionRequested -= RunAction;
            TriggerRunner.Fired -= ShowTrigger;
            runner.Started -= OnStoryStarted;
            runner.Ended -= OnStoryEnded;
            runner.ObjectiveChanged -= ShowObjective;
            runner.PlaceChanged -= OnPlace;
            IAmAsIAm.Runtime.Save.SaveService.Loaded -= UpdateFrontLabel;
            PlayerMotor.Blocked = false;
        }

        void OnStoryStarted(Transform partner)
        {
            if (cameraRig != null) cameraRig.SetTalkPartner(partner);
            if (partner != null && cameraRig != null && cameraRig.Target != null)
            {
                // The person turns to face the character.
                var to = cameraRig.Target.position - partner.position;
                to.y = 0f;
                if (to.sqrMagnitude > 0.01f) partner.rotation = Quaternion.LookRotation(to);
            }
        }

        string _lastCity;

        void OnPlace(string placeId)
        {
            _visited.Add(placeId);
            var city = _places?.Find(placeId)?.city;
            if (!string.IsNullOrEmpty(city)) _lastCity = city;
            if (_world != null) _world.Enter(placeId);
        }

        // ── Getting from scene to scene (owner: walk there, guided; the map from college on) ──

        void RunTravel(string destination)
        {
            string name = _places.NameOf(destination);
            ShowObjective("Head to " + name);
            if (_lastWorldVerb == "Go") { _lastWorldVerb = ""; Depart(destination); return; } // She's already on her way.
            _exploring = true;
            _world.ShowExit(name, () => { StopExploring(); Depart(destination); });
        }

        void Depart(string destination)
        {
            // Owner: the Prologue and the Prelude are door to door; from Chapter 1 the map on Maya's phone opens.
            var place = _places.Find(destination);
            bool doorToDoor = GameServices.Chapter(runner.ChapterId)?.doorToDoor ?? false;
            if (place != null && !string.IsNullOrEmpty(place.city) && !doorToDoor)
                _map.Show(_places, place.city, _world.Place, destination, _visited, () => Arrive(destination), () => RunTravel(destination));
            else Arrive(destination);
        }

        void Arrive(string destination)
        {
            OnPlace(destination);
            ShowObjective(runner.Objective);
            runner.CompleteAction();
        }

        void OnStoryEnded()
        {
            _dialogue.SetActive(false);
            _choices.SetActive(false);
            StopExploring();
            if (_voice != null) _voice.Stop();
            if (_world != null) _world.Leave();
            // Sharing time ends with the story.
            var roster = GameServices.Roster;
            if (roster != null) foreach (var id in roster.CoConscious.ToList()) roster.SetCoConscious(id, false);
            if (cameraRig != null) cameraRig.SetTalkPartner(null);
        }

        void Start()
        {
            if (GameServices.Roster != null)
            {
                GameServices.Roster.FrontChanged += _ => UpdateFrontLabel();
                GameServices.Roster.CoConsciousChanged += (_, __) => UpdateFrontLabel();
            }
            UpdateFrontLabel();
            if (!string.IsNullOrEmpty(startKnot)) runner.Play(startKnot);
        }

        void Update()
        {
            EnsureEventSystem();

            // A save from the middle of a chapter (owner: "Continue" used to start over): pick it back up at its last choice.
            var resume = IAmAsIAm.Runtime.Save.SaveService.PendingResume;
            if (resume != null && GameServices.IsReady && !runner.IsPlaying)
            {
                IAmAsIAm.Runtime.Save.SaveService.PendingResume = null;
                if (!runner.Resume(resume)) Debug.LogWarning("[IAmAsIAm] Couldn't pick the chapter back up; open it from the chapter tree.");
            }

            if (_pacer != null)
            {
                _pacer.Tick(Time.unscaledDeltaTime);
                if (_breathCircle != null) _breathCircle.localScale = Vector3.one * Mathf.Lerp(0.55f, 1.1f, _pacer.Fullness);
                _onPacerTick?.Invoke();
            }

            if (_triggerPanel.activeSelf && Time.unscaledTime > _triggerHideAt) _triggerPanel.SetActive(false);

            bool busy = _anchorOpen || _dialogue.activeSelf || _choices.activeSelf || _action.activeSelf || (runner.IsPlaying && !_exploring)
                        || IAmAsIAm.Runtime.Devices.DeviceScreen.IsOpen || IAmAsIAm.Runtime.Practices.BookScreen.IsOpen || IAmAsIAm.Runtime.Practices.RitualScreen.IsOpen || MapScreen.IsOpen || ChapterTreeScreen.IsOpen;
            // Saves, the phone and the chapter list still wait for the story, even while exploring.
            IsBusy = busy || runner.IsPlaying;
            busy |= IAmAsIAm.Runtime.Save.SaveSlotsScreen.IsOpen || IAmAsIAm.Runtime.Loading.LoadingScreen.IsShowing;
            PlayerMotor.Blocked = busy;
            _tap.SetActive(_dialogue.activeSelf && !_choices.activeSelf && !_anchorOpen);
            if (_touch != null) _touch.SetActive(!busy);

            // Chapters (owner: the story is in the game). The list opens by itself when a new game starts and after a chapter
            // ends; the Chapters button opens it any time nothing else is going on.
            bool inGame = IAmAsIAm.Runtime.Save.SaveService.ActiveSlot != 0;
            _chaptersButton.SetActive(inGame && !busy && !runner.IsPlaying);
            var runs = GameServices.ChapterRuns;
            if (inGame && !busy && runs != null && !runs.InChapter)
            {
                if (!_offeredStart)
                {
                    _offeredStart = true;
                    if (!(GameServices.Chapters?.chapters ?? new List<IAmAsIAm.Core.Story.ChapterDefinition>()).Any(c => runs.HasFinished(c.id)))
                        _wantChapterList = true;
                }
                if (_wantChapterList) { _wantChapterList = false; OpenChapterList(); }
            }

            // The goal line steps aside while a list of dialogue choices is up (they share the top of the screen).
            if (_objective != null) _objective.enabled = !_choices.activeSelf;
            if (_objectiveCard != null)
            {
                // Fades out over the last second; nothing moves (fine with Reduce motion).
                float left = _objectiveCardUntil - Time.unscaledTime;
                var c = _objectiveCard.color;
                c.a = Mathf.Clamp01(left);
                _objectiveCard.color = c;
                _objectiveCard.enabled = left > 0f && !_anchorOpen;
            }

            var usable = busy || interactor == null ? null : interactor.Target;
            _prompt.text = usable == null ? "" : (Touchscreen.current != null ? "" : "E  ") + usable.Prompt;
        }

        /// <summary>
        /// The goal line (owner: bigger, in the middle of the screen). A new goal shows large in the middle for a few seconds,
        /// then stays at the top centre.
        /// </summary>
        void ShowObjective(string text)
        {
            if (_objective == null) return;
            bool changed = _objective.text != text;
            _objective.text = text;
            if (changed && !string.IsNullOrEmpty(text) && _objectiveCard != null)
            {
                _objectiveCard.text = text;
                _objectiveCardUntil = Time.unscaledTime + 4f;
            }
        }

        // ── Dialogue ──────────────────────────────────────────────────────────────

        void ShowLine(StoryLine line)
        {
            _choices.SetActive(false);
            StopExploring();
            _dialogue.SetActive(true);
            _lineShownFrame = Time.frameCount;
            _speaker.text = line.Speaker == null ? "" : line.Inner ? line.Speaker + " (inside)" : line.Remote ? line.Speaker + " (on video)" : line.Speaker;
            // Whoever speaks is standing here, and the camera turns to them (not the system's own voices: that's the body).
            if (line.Speaker != null && !line.Inner && !line.Remote && _world != null && !IsSystemMember(line.Speaker))
            {
                var person = _world.Person(line.Speaker);
                if (person != null) OnStoryStarted(person);
            }
            _line.fontStyle = line.Inner ? FontStyle.Italic : FontStyle.Normal;
            _line.text = line.Text;
            if (_voice != null) _voice.Play(line);
        }

        void ShowChoices(List<Choice> choices)
        {
            StopExploring();
            _lastWorldVerb = "";
            if (TryExplore(choices)) return;
            Clear(_choices.transform);
            _choices.SetActive(true);
            for (int i = 0; i < choices.Count; i++)
            {
                int index = i;
                Button(_choices.transform, choices[i].text, () => { _choices.SetActive(false); runner.Choose(index); }, 120);
            }
        }

        // ── Exploring (owner: explore the world, not just dialogue) ──────────────

        static readonly System.Text.RegularExpressions.Regex WorldChoice =
            new System.Text.RegularExpressions.Regex(@"^(Look|Use|Talk|Go):\s*(.+?)(?:\s*\(([^()]*)\))?\s*$");

        /// <summary>Things the character carries (her phone, her keys, Mira's stone): a button, not a place in the world.</summary>
        static bool Carried(string target)
        {
            string t = target.ToLowerInvariant();
            return t.StartsWith("your ") || t.StartsWith("mira's stone") || t.StartsWith("mira's napkin");
        }

        /// <summary>
        /// When the choices are things to explore (Look / Use / Talk / Go, each at a different thing), they go out into the
        /// place: the character walks to them. Things she carries and "Wait" become small buttons at the bottom. Anything
        /// else (what to say, how to answer) stays a list of choices on screen.
        /// </summary>
        bool TryExplore(List<Choice> choices)
        {
            if (_world == null || string.IsNullOrEmpty(_world.Place)) return false;
            var offers = new List<StoryWorld.Offer>();
            var pocket = new List<(int index, string text)>();
            var seen = new HashSet<string>();
            for (int i = 0; i < choices.Count; i++)
            {
                string text = (choices[i].text ?? "").Trim();
                var m = WorldChoice.Match(text);
                if (m.Success)
                {
                    string verb = m.Groups[1].Value, target = m.Groups[2].Value.Trim(), detail = m.Groups[3].Value.Trim();
                    // Things she carries, and saying something ("Talk: Say Mira's name"), are buttons, not places to walk to.
                    if (Carried(target) || (verb == "Talk" && target.StartsWith("Say "))) { pocket.Add((i, text)); continue; }
                    string key = target.ToLowerInvariant();
                    if (key == "mom" || key == "mommy") key = "sarah";
                    if (!seen.Add(key)) return false; // Two ways to answer the same person: a dialogue choice.
                    offers.Add(new StoryWorld.Offer { index = i, verb = verb, target = target, label = detail.Length > 0 ? $"{target} ({detail})" : target });
                }
                else if (text.StartsWith("Wait")) pocket.Add((i, text));
                else return false;
            }
            if (offers.Count == 0) return false;

            _dialogue.SetActive(false);
            _choices.SetActive(false);
            _exploring = true;
            if (cameraRig != null) cameraRig.SetTalkPartner(null);
            _world.ShowOffers(offers, ChooseFromWorld);

            Clear(_pocket.transform);
            foreach (var p in pocket)
            {
                int index = p.index;
                Button(_pocket.transform, p.text, () => ChooseFromWorld(index, null, ""), 130);
            }
            _pocket.SetActive(pocket.Count > 0);
            return true;
        }

        void ChooseFromWorld(int index, Transform spot, string verb)
        {
            if (!_exploring || _anchorOpen) return;
            StopExploring();
            _lastWorldVerb = verb ?? "";
            if (verb == "Talk" && spot != null && cameraRig != null)
            {
                cameraRig.SetTalkPartner(spot);
                OnStoryStarted(spot); // They turn to face the character.
            }
            runner.Choose(index);
        }

        static bool IsSystemMember(string name)
        {
            var roster = GameServices.Roster;
            if (roster == null) return false;
            foreach (var m in roster.Members)
                if (string.Equals(m.displayName, name, StringComparison.OrdinalIgnoreCase) || string.Equals(m.id, name, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        void StopExploring()
        {
            _exploring = false;
            if (_world != null) _world.Dim();
            if (_pocket != null) _pocket.SetActive(false);
        }

        void UpdateFrontLabel()
        {
            var roster = GameServices.Roster;
            if (roster == null || _frontLabel == null) return;
            var def = roster.GetDefinition(roster.FrontingId);
            _frontLabel.text = def != null ? def.displayName : roster.FrontingId;
            // Sharing time (co-consciousness): "Maya + Izzy", the others in their own colours.
            foreach (var id in roster.CoConscious)
            {
                var other = roster.GetDefinition(id);
                if (other == null) continue;
                _frontLabel.text += $" <color={other.colorHex}>+ {other.displayName}</color>";
            }
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
                case "travel": RunTravel(action.Value); break;
                case "device":
                    if (Enum.TryParse<IAmAsIAm.Core.Devices.DeviceKind>(action.Value, true, out var kind)) _devices.Open(kind, Finish);
                    else Finish();
                    break;
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

            // The craft's rituals (owner: small steps that feel like rituals): # ritual: candle, # ritual: grounding-centering,
            // runes, charm-bag, cleansing [hard], circle-casting, and # ritual: tarot on its own (one card). A spread (# ritual: tarot three-card) is read below.
            if (IAmAsIAm.Core.Practices.RitualSession.IsSupported(kind) && (kind != "tarot" || arg.Length == 0))
            {
                string who = GameServices.Roster?.FrontingId ?? "maya";
                // # ritual: cleansing hard = the moment stirs something up (after the breakup).
                _ritual.Run(kind, who, r => runner.CompleteAction(r?.Vars ?? new Dictionary<string, string>()), variant: arg);
                return;
            }

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

        /// <summary>
        /// Maya's altar at home. Maya and Izzy (the ones who practise Wicca, owner) get the practice wheel; anyone else
        /// leaves the tools alone, with a line in their own voice.
        /// </summary>
        void OpenAltar()
        {
            string front = GameServices.Roster?.FrontingId ?? "maya";
            var book = GameServices.Book;
            if (book != null && !book.Practises(front)) { _ritual.ShowLine(MemberName(front), book.RefusalLine(front), null); return; }
            _ritual.OpenWheel(null);
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
            var runs = GameServices.ChapterRuns;
            string chapterId = runs != null && runs.InChapter ? runs.CurrentChapterId : null;
            var choices = (GameServices.Choices?.Records ?? new List<IAmAsIAm.Core.Story.ChoiceRecord>())
                .Where(c => chapterId == null || c.chapterId == chapterId).ToList();
            string body = choices.Count == 0 ? "No choices recorded yet." : string.Join("\n", choices.Select(c => "• " + c.summary));

            // Finishing the chapter in full unlocks its one set alter (owner: never random). Replays can start as anyone unlocked.
            if (chapterId != null)
            {
                foreach (var id in runs.Complete(GameServices.Chapter(chapterId)?.unlocksMemberId))
                    body += $"\n\n<b>{MemberName(id)}</b> came forward. You can now replay the chapters you've finished as {MemberName(id)}.";
                // An app on her phone (owner: SystemLog at the end of Chapter 4).
                string app = GameServices.Chapter(chapterId)?.unlocksAppId;
                var devices = GameServices.Devices;
                if (!string.IsNullOrEmpty(app) && devices != null && !devices.HasApp(app))
                {
                    devices.GainApp(app);
                    if (app == IAmAsIAm.Core.Devices.Devices.SystemLogApp)
                        body += "\n\n<b>SystemLog</b> is on Maya's phone now: a place for everyone to leave each other notes.";
                }
            }
            OpenAction("Your choices", body, ("Continue", () => { Finish(); _wantChapterList = true; }));
        }

        // ── Starting a chapter: content warnings, who to start as, or a summary instead ──

        /// <summary>Opens a chapter's start screen. Content warnings are always shown first (owner's rule).</summary>
        public void OpenChapter(string chapterId) => OpenChapter(chapterId, null);

        /// <summary>The start screen for a chapter; with <paramref name="asMember"/>, only "Begin as" them (from the tree).</summary>
        public void OpenChapter(string chapterId, string asMember)
        {
            var ch = GameServices.Chapter(chapterId);
            if (ch == null || runner.IsPlaying || _action.activeSelf) return;

            string warnings = ch.contentTags == null || ch.contentTags.Count == 0
                ? "None listed."
                : string.Join("\n", ch.contentTags.Select(t => "• " + ContentTags.Label(t)));
            string setting = string.IsNullOrEmpty(ch.setting) ? "" : $"<i>{ch.setting}</i>\n\n";
            string body = $"{setting}<b>Content warnings</b>\n{warnings}\n\nYou can open the Anchor at any time.";

            var buttons = new List<(string, Action)>();
            IReadOnlyList<string> starters = GameServices.ChapterRuns?.StartersFor(ch.id, ch.startingMemberId) ?? new List<string>();
            if (!string.IsNullOrEmpty(asMember)) starters = new List<string> { asMember };
            if (starters.Count == 0) buttons.Add(("Begin", () => BeginChapter(ch, null)));
            foreach (var id in starters)
            {
                string starter = id;
                buttons.Add(($"Begin as {MemberName(starter)}", () => BeginChapter(ch, starter)));
            }
            buttons.Add(("Skip and read a summary", () => OpenAction(ch.title, "<b>Summary</b>\n" + ch.summary, ("Close", CloseAction))));
            buttons.Add(("Back", () => { CloseAction(); OpenChapterList(); }));
            OpenAction(ch.title, body, buttons.ToArray());
        }

        /// <summary>
        /// The chapter list: begin the next chapter (in order), or replay one already finished (as anyone unlocked).
        /// Chapters open in order: each one after the last finished.
        /// </summary>
        public void OpenChapterList(int replayPage = -1)
        {
            // The hub (owner): a tree that grows up from the Prologue, with a leaf for every alter who can replay a chapter.
            if (replayPage < 0 && _tree != null)
            {
                if (runner.IsPlaying) return;
                var chapters = GameServices.Chapters?.chapters ?? new List<IAmAsIAm.Core.Story.ChapterDefinition>();
                _tree.Show(chapters, GameServices.ChapterRuns, id =>
                {
                    var def = GameServices.Roster?.GetDefinition(id);
                    Color c = new Color(0.8f, 0.8f, 0.8f);
                    if (def != null && !string.IsNullOrEmpty(def.colorHex)) ColorUtility.TryParseHtmlString(def.colorHex, out c);
                    return (def != null ? def.displayName : id, c);
                }, TestJumps, (chapterId, member) => OpenChapter(chapterId, member), null);
                return;
            }
            if (runner.IsPlaying) return;
            var all = GameServices.Chapters?.chapters ?? new List<IAmAsIAm.Core.Story.ChapterDefinition>();
            var runs = GameServices.ChapterRuns;
            var finished = all.Where(c => runs != null && runs.HasFinished(c.id)).ToList();
            var next = all.FirstOrDefault(c => runs == null || !runs.HasFinished(c.id));

            if (replayPage >= 0)
            {
                const int perPage = 3;
                var buttons = new List<(string, Action)>();
                foreach (var c in finished.Skip(replayPage * perPage).Take(perPage))
                {
                    var ch = c;
                    buttons.Add((ShortTitle(ch), () => { CloseAction(); OpenChapter(ch.id); }));
                }
                if ((replayPage + 1) * perPage < finished.Count) buttons.Add(("More", () => OpenChapterList(replayPage + 1)));
                buttons.Add(("Back", () => OpenChapterList()));
                OpenAction("Replay a chapter", "Finished chapters can be played again, starting as anyone who has come forward.", buttons.ToArray());
                return;
            }

            string body = finished.Count == 0
                ? "Your story starts here."
                : "Finished:\n" + string.Join("\n", finished.Select(c => "• " + ShortTitle(c)));
            var list = new List<(string, Action)>();
            if (next != null) list.Add(($"Begin: {ShortTitle(next)}", () => { CloseAction(); OpenChapter(next.id); }));
            if (finished.Count > 0) list.Add(("Replay a chapter", () => OpenChapterList(0)));
            if (TestJumps) list.Add(("Test: any chapter", () => OpenTestChapterList(0)));
            list.Add(("Not now", CloseAction));
            OpenAction("Chapters", body, list.ToArray());
        }

        /// <summary>
        /// Test builds only: start ANY chapter, so the owner can test a new scene without replaying everything before it.
        /// Every build is a test build for now; set this to false for a release.
        /// </summary>
        const bool TestJumps = true;

        void OpenTestChapterList(int page)
        {
            const int perPage = 2;
            var all = GameServices.Chapters?.chapters ?? new List<IAmAsIAm.Core.Story.ChapterDefinition>();
            var buttons = new List<(string, Action)>();
            foreach (var c in all.Skip(page * perPage).Take(perPage))
            {
                var ch = c;
                buttons.Add((ShortTitle(ch), () => { CloseAction(); OpenChapter(ch.id); }));
            }
            if ((page + 1) * perPage < all.Count) buttons.Add(("More", () => OpenTestChapterList(page + 1)));
            buttons.Add(("Back", () => OpenChapterList()));
            OpenAction("Test: any chapter", "For testing: start any chapter, even one you haven't reached. (Not in the finished game.)", buttons.ToArray());
        }

        static string ShortTitle(IAmAsIAm.Core.Story.ChapterDefinition c) =>
            string.IsNullOrEmpty(c.title) ? c.id : c.title.Replace(" (title to come)", "");

        void BeginChapter(IAmAsIAm.Core.Story.ChapterDefinition ch, string starterId)
        {
            CloseAction();
            var roster = GameServices.Roster;
            string starter = starterId ?? roster?.FrontingId;
            GameServices.ChapterRuns?.Begin(ch.id, starter);
            if (roster != null && !string.IsNullOrEmpty(starterId) && roster.FrontingId != starterId) roster.ForceSwitch(starterId);
            runner.Play(ch.inkKnot, null, ch.id);
        }

        static string MemberName(string id) => GameServices.Roster?.GetDefinition(id)?.displayName ?? id;

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
            EnsureEventSystem();

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
            // Fog whenever the character dissociates (owner), under everything else.
            _fog = IAmAsIAm.Runtime.Mind.FogOverlay.Create(_root);

            // Tap anywhere to continue.
            if (ShowTouchControls()) BuildTouchControls();

            _tap = Panel(_root, "Tap", new Color(0, 0, 0, 0), Vector2.zero, Vector2.one);
            _tap.AddComponent<Button>().onClick.AddListener(() => { if (!_anchorOpen && !_choices.activeSelf && !_action.activeSelf) runner.Next(); });
            _tap.SetActive(false);

            // Things she carries, and "Wait", while exploring: small buttons, bottom middle (above the look pad).
            _pocket = Panel(_root, "Pocket", new Color(0, 0, 0, 0), new Vector2(0.3f, 0.03f), new Vector2(0.72f, 0.14f));
            var plg = _pocket.AddComponent<HorizontalLayoutGroup>();
            plg.spacing = 24; plg.childControlWidth = true; plg.childForceExpandWidth = true; plg.childControlHeight = false;
            plg.childAlignment = TextAnchor.LowerCenter;
            _pocket.SetActive(false);

            _prompt = Label(_root, "Prompt", 54, FontStyle.Bold, new Vector2(0.3f, 0.34f), new Vector2(0.7f, 0.42f));
            _prompt.alignment = TextAnchor.MiddleCenter;

            _dialogue = Panel(_root, "Dialogue", PanelColor, new Vector2(0.05f, 0.03f), new Vector2(0.95f, 0.33f));
            _dialogue.GetComponent<Image>().raycastTarget = false;
            _speaker = Label(_dialogue.transform, "Speaker", 56, FontStyle.Bold, new Vector2(0.03f, 0.75f), new Vector2(0.97f, 0.95f));
            _line = Label(_dialogue.transform, "Line", 54, FontStyle.Normal, new Vector2(0.03f, 0.08f), new Vector2(0.97f, 0.75f));
            // Hidden until someone speaks. (It used to start visible, which made the game think a conversation was
            // always going on: the player couldn't move, the touch controls never appeared, and an invisible
            // "tap to continue" layer swallowed every touch.)
            _dialogue.SetActive(false);

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

            // The Chapters button: top, left of Phone (Phone 0.60-0.72, Saves 0.73-0.85, Anchor 0.86-0.98).
            _chaptersButton = Panel(_root, "Chapters Button", ButtonColor, new Vector2(0.47f, 0.9f), new Vector2(0.59f, 0.98f));
            _chaptersButton.AddComponent<Button>().onClick.AddListener(() => OpenChapterList());
            var chaptersLabel = Label(_chaptersButton.transform, "Label", 44, FontStyle.Bold, Vector2.zero, Vector2.one);
            chaptersLabel.alignment = TextAnchor.MiddleCenter;
            chaptersLabel.text = "Chapters";
            _chaptersButton.SetActive(false);

            // The goal line (# objective:), under who's fronting: the main way the game points the way.
            // Owner: bigger and in the middle of the screen. Top centre (under the top buttons), with a dark edge to read on anything.
            _objective = Label(_root, "Objective", 64, FontStyle.Bold, new Vector2(0.18f, 0.79f), new Vector2(0.82f, 0.89f));
            _objective.alignment = TextAnchor.MiddleCenter;
            Shadowed(_objective);
            // A new goal first shows large in the middle of the screen, then fades (it stays at the top).
            _objectiveCard = Label(_root, "Objective (new)", 88, FontStyle.Bold, new Vector2(0.08f, 0.46f), new Vector2(0.92f, 0.64f));
            _objectiveCard.alignment = TextAnchor.MiddleCenter;
            _objectiveCard.enabled = false;
            Shadowed(_objectiveCard);

            // The Anchor button and panel: top right, on their own canvas above every other screen.
            var anchorLayer = new GameObject("Anchor Layer", typeof(RectTransform), typeof(Canvas), typeof(GraphicRaycaster));
            anchorLayer.transform.SetParent(_root, false);
            var layerRt = anchorLayer.GetComponent<RectTransform>();
            layerRt.anchorMin = Vector2.zero; layerRt.anchorMax = Vector2.one; layerRt.offsetMin = Vector2.zero; layerRt.offsetMax = Vector2.zero;
            var anchorCanvas = anchorLayer.GetComponent<Canvas>();
            anchorCanvas.overrideSorting = true;
            anchorCanvas.sortingOrder = AnchorSortingOrder;

            var anchorButton = Panel(anchorLayer.transform, "Anchor Button", ButtonColor, new Vector2(0.86f, 0.9f), new Vector2(0.98f, 0.98f));
            anchorButton.AddComponent<Button>().onClick.AddListener(ToggleAnchor);
            Label(anchorButton.transform, "Label", 48, FontStyle.Bold, Vector2.zero, Vector2.one).alignment = TextAnchor.MiddleCenter;
            anchorButton.GetComponentInChildren<Text>().text = "Anchor";

            _anchor = Panel(anchorLayer.transform, "Anchor", new Color(0.12f, 0.14f, 0.13f, 0.97f), new Vector2(0.1f, 0.06f), new Vector2(0.84f, 0.88f));
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

            // The breathing circle is used by the Anchor too, so it sits on the Anchor layer, above the Anchor panel.
            _breathCircle.SetParent(anchorLayer.transform, false);
            _breathCircle.SetAsLastSibling();
        }

        /// <summary>
        /// Every tap and button press goes through the EventSystem: without one, nothing on screen responds.
        /// It's kept for the whole run (it used to belong to the scene, and switching save slots reloads the
        /// scene: the new scene could find the old EventSystem just before it was destroyed, and end up with none).
        /// Update checks every frame and makes a new one if it's ever missing.
        /// </summary>
        static void EnsureEventSystem()
        {
            if (EventSystem.current != null && EventSystem.current.isActiveAndEnabled) return;
            var existing = FindAnyObjectByType<EventSystem>();
            if (existing != null && existing.isActiveAndEnabled && existing.GetComponent<BaseInputModule>() != null) return;
            var es = new GameObject("EventSystem (kept)", typeof(EventSystem), typeof(InputSystemUIInputModule));
            es.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();
            DontDestroyOnLoad(es);
        }

        static bool ShowTouchControls() => Application.isMobilePlatform || Touchscreen.current != null;

        /// <summary>
        /// Phone controls: a joystick bottom left (drives the virtual gamepad's left stick, so walking code
        /// is shared), a Use button bottom right, and dragging anywhere else on the right half orbits the camera.
        /// </summary>
        void BuildTouchControls()
        {
            _touch = new GameObject("Touch Controls", typeof(RectTransform));
            _touch.transform.SetParent(_root, false);
            var rt = _touch.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;

            var look = Panel(_touch.transform, "Look Pad", new Color(0, 0, 0, 0), new Vector2(0.45f, 0f), new Vector2(1f, 0.88f));
            look.AddComponent<LookPad>().cameraRig = cameraRig;

            // Round (owner): a disc with a brighter rim, and a round knob.
            var ring = Panel(_touch.transform, "Stick Ring", new Color(1, 1, 1, 0.35f), new Vector2(0.04f, 0.06f), new Vector2(0.04f, 0.06f));
            ring.GetComponent<Image>().sprite = Disc(rim: true);
            var ringRt = ring.GetComponent<RectTransform>();
            ringRt.pivot = Vector2.zero; ringRt.sizeDelta = new Vector2(480, 480);
            var knob = Panel(ring.transform, "Stick", new Color(1, 1, 1, 0.55f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            knob.GetComponent<Image>().sprite = Disc(rim: false);
            knob.GetComponent<RectTransform>().sizeDelta = new Vector2(260, 260); // Big enough to find with a thumb.
            knob.SetActive(false); // Set the control path while inactive, then enable.
            var stick = knob.AddComponent<OnScreenStick>();
            stick.controlPath = "<Gamepad>/leftStick";
            stick.movementRange = 150;
            knob.SetActive(true);

            var use = Panel(_touch.transform, "Use", ButtonColor, new Vector2(0.86f, 0.08f), new Vector2(0.86f, 0.08f));
            var useRt = use.GetComponent<RectTransform>();
            useRt.sizeDelta = new Vector2(300, 300);
            use.AddComponent<Button>().onClick.AddListener(() => { if (interactor != null) interactor.Use(); });
            var useLabel = Label(use.transform, "Label", 52, FontStyle.Bold, Vector2.zero, Vector2.one);
            useLabel.alignment = TextAnchor.MiddleCenter;
            useLabel.text = "Use";
        }

        static Sprite _disc, _ring;

        /// <summary>
        /// A round sprite made in code (smooth edge). With <paramref name="rim"/>: a faint fill with a brighter ring around the
        /// edge (the joystick's base); without: a solid disc (the knob).
        /// </summary>
        static Sprite Disc(bool rim)
        {
            if (rim && _ring != null) return _ring;
            if (!rim && _disc != null) return _disc;
            const int size = 256;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp, name = rim ? "Stick Ring" : "Stick Knob" };
            var px = new Color32[size * size];
            float r = size / 2f;
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float d = Mathf.Sqrt((x + 0.5f - r) * (x + 0.5f - r) + (y + 0.5f - r) * (y + 0.5f - r));
                    float edge = Mathf.Clamp01(r - d);              // 1 inside, a 1-pixel soft edge
                    float a = edge;
                    if (rim) a *= d > r - 10f ? 1f : 0.35f;         // brighter rim, faint fill
                    px[y * size + x] = new Color32(255, 255, 255, (byte)(a * 255));
                }
            tex.SetPixels32(px);
            tex.Apply();
            var sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
            if (rim) _ring = sprite; else _disc = sprite;
            return sprite;
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

        /// <summary>A dark edge so a line reads over any scene.</summary>
        static void Shadowed(Text t)
        {
            var o = t.gameObject.AddComponent<Outline>();
            o.effectColor = new Color(0f, 0f, 0f, 0.85f);
            o.effectDistance = new Vector2(3f, -3f);
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
