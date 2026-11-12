using System;
using System.Collections.Generic;
using IAmAsIAm.Core.Save;
using IAmAsIAm.Runtime.Story;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Save
{
    /// <summary>
    /// The save slots (owner, 2026-11-12), built in code like the story screen.
    ///  * At start-up it's the title screen: Continue or Delete a save, or start a New game in an empty slot.
    ///  * In the game, the "Saves" button (top right, beside the Anchor) opens it: save now, save into another slot,
    ///    load another slot, start a new game in an empty slot, or go back to the title screen.
    /// Each slot is a separate game; switching slots saves the current one first (Save.GameSession).
    /// It autosaves the active slot whenever a conversation or scene ends, and when the app is paused or closed,
    /// never mid-conversation. Every question is asked on this screen (no system pop-ups).
    /// Placeholder look: mockup_title.png replaces it (docs/ART_LIST.md).
    /// </summary>
    public sealed class SaveSlotsScreen : MonoBehaviour
    {
        [SerializeField] StoryRunner runner;
        [SerializeField] StoryScreen storyScreen;

        const int SortingOrder = 50;
        static readonly Color TextColor = new Color(0.95f, 0.92f, 0.86f);
        static readonly Color Dim = new Color(0.95f, 0.92f, 0.86f, 0.65f);
        static readonly Color TitleBackground = new Color(0.07f, 0.06f, 0.09f, 1f);
        static readonly Color MenuBackground = new Color(0.04f, 0.03f, 0.05f, 0.88f);
        static readonly Color CardColor = new Color(0.14f, 0.12f, 0.17f, 0.96f);
        static readonly Color ActiveCardColor = new Color(0.20f, 0.16f, 0.25f, 0.98f);
        static readonly Color ButtonColor = new Color(0.28f, 0.22f, 0.30f, 0.95f);
        static readonly Color QuietButtonColor = new Color(0.20f, 0.18f, 0.22f, 0.95f);

        enum Mode { Title, InGame }

        static SaveSlotsScreen _open;
        /// <summary>True while the title screen or the saves menu is up.</summary>
        public static bool IsOpen => _open != null;

        Font _font;
        GameObject _canvasGo, _panel, _menuButton;
        Image _background;
        Text _heading, _message;
        RectTransform _content, _footer;
        Mode _mode;
        float _previousTimeScale = 1f;

        void Awake()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Build();
            _panel.SetActive(false);
        }

        void OnEnable()
        {
            if (runner != null) runner.Ended += OnStoryEnded;
        }

        void OnDisable()
        {
            if (runner != null) runner.Ended -= OnStoryEnded;
            if (_open == this) _open = null;
        }

        void Start()
        {
            var request = GameSession.TakePending();
            if (request == null) { OpenTitle(); return; }

            // The scene was reloaded to switch games; the loading screen is up and hides by itself.
            bool ok = request.NewGame ? NewGameInPlace(request.Slot) : SaveService.TryLoad(request.Slot, runner);
            if (!ok) OpenTitle($"Save {request.Slot} couldn't be opened.");
        }

        void Update()
        {
            bool inGame = SaveService.ActiveSlot != 0;
            if (inGame && !IsOpen) GameServices.PlaySeconds += Time.unscaledDeltaTime;
            bool storyBusy = (storyScreen != null && storyScreen.IsBusy) || (runner != null && runner.IsPlaying);
            _menuButton.SetActive(inGame && !IsOpen && !storyBusy && !Loading.LoadingScreen.IsShowing);
        }

        void OnStoryEnded() => SaveService.Autosave(runner);

        void OnApplicationPause(bool paused)
        {
            if (paused) SaveService.Autosave(runner); // Android: the app may be closed from the background.
        }

        void OnApplicationQuit() => SaveService.Autosave(runner);

        // ── Opening and closing ───────────────────────────────────────────────────

        void OpenTitle(string message = null)
        {
            _mode = Mode.Title;
            Open();
            _background.color = TitleBackground;
            _heading.text = "I Am As I Am";
            ShowSlots(message);
        }

        void OpenMenu()
        {
            if (IsOpen || SaveService.ActiveSlot == 0) return;
            _mode = Mode.InGame;
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            Open();
            _background.color = MenuBackground;
            _heading.text = "Saves";
            ShowSlots(null);
        }

        void Open()
        {
            _open = this;
            _panel.SetActive(true);
        }

        void Close()
        {
            _panel.SetActive(false);
            if (_open == this) _open = null;
            if (_mode == Mode.InGame) Time.timeScale = _previousTimeScale <= 0f ? 1f : _previousTimeScale;
        }

        // ── The slot list ─────────────────────────────────────────────────────────

        void ShowSlots(string message)
        {
            Clear(_content);
            Clear(_footer);
            _message.text = message ?? (_mode == Mode.Title ? "Choose a save. Each one is its own game." : "The game saves by itself whenever a scene ends.");

            int lastPlayed = GameServices.Preferences?.lastSlot ?? 0;
            GameObject first = null;
            foreach (var slot in SaveService.Slots.List())
            {
                var b = SlotCard(slot, lastPlayed);
                if (first == null) first = b;
            }

            if (_mode == Mode.InGame)
            {
                var back = Button(_footer, "Back to the game", Close, 110, ButtonColor);
                Button(_footer, "Title screen", () => Confirm("Go back to the title screen? Your game is saved first.", "Title screen", () =>
                {
                    Close();
                    GameSession.ToTitle();
                }), 110, QuietButtonColor);
                Select(back);
            }
            else Select(first);
        }

        /// <summary>One slot's card. Returns its first button, for gamepad selection.</summary>
        GameObject SlotCard(SaveSlotSummary slot, int lastPlayed)
        {
            bool active = slot.Slot == SaveService.ActiveSlot;
            var card = Panel(_content, $"Slot {slot.Slot}", active ? ActiveCardColor : CardColor, Vector2.zero, Vector2.one);
            var vlg = card.AddComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(40, 40, 40, 40);
            vlg.spacing = 24;
            vlg.childControlHeight = true; vlg.childControlWidth = true;
            vlg.childForceExpandHeight = false; vlg.childForceExpandWidth = true;

            string tag = active ? "  ·  playing now" : _mode == Mode.Title && slot.Slot == lastPlayed && slot.CanLoad ? "  ·  last played" : "";
            FlowLabel(card.transform, $"Save {slot.Slot}<size=40><color=#d8c9a6>{tag}</color></size>", 64, FontStyle.Bold, TextColor);
            FlowLabel(card.transform, Details(slot), 46, FontStyle.Normal, slot.CanLoad ? TextColor : Dim);
            Spacer(card.transform);

            var buttons = new List<GameObject>();
            void Add(string label, Action onClick, Color color) => buttons.Add(Button(card.transform, label, onClick, 110, color));
            int n = slot.Slot;

            if (_mode == Mode.Title)
            {
                if (slot.CanLoad)
                {
                    Add("Continue", () => StartFromTitle(n, newGame: false), ButtonColor);
                    Add("Delete", () => AskDelete(n), QuietButtonColor);
                }
                else if (slot.IsEmpty) Add("New game", () => StartFromTitle(n, newGame: true), ButtonColor);
                else Add("Delete", () => AskDelete(n), QuietButtonColor);
            }
            else if (active)
            {
                Add("Save now", () => { SaveService.Save(n, runner); ShowSlots("Saved."); }, ButtonColor);
            }
            else if (slot.CanLoad)
            {
                Add("Load", () => Confirm($"Load Save {n}? Your game in Save {SaveService.ActiveSlot} is saved first.", "Load",
                    () => { Close(); GameSession.LoadSlot(n); }), ButtonColor);
                Add("Save here", () => Confirm($"Save this game over Save {n}? What's in Save {n} now will be lost.", "Save over it",
                    () => { SaveService.Save(n, runner); ShowSlots($"Saved in Save {n}. You're now playing Save {n}."); }), QuietButtonColor);
                Add("Delete", () => AskDelete(n), QuietButtonColor);
            }
            else if (slot.IsEmpty)
            {
                Add("Save here", () => { SaveService.Save(n, runner); ShowSlots($"Saved in Save {n}. You're now playing Save {n}."); }, ButtonColor);
                Add("New game", () => Confirm($"Start a new game in Save {n}? Your game in Save {SaveService.ActiveSlot} is saved first.", "New game",
                    () => { Close(); GameSession.NewGame(n); }), QuietButtonColor);
            }
            else Add("Delete", () => AskDelete(n), QuietButtonColor);

            return buttons.Count > 0 ? buttons[0] : null;
        }

        static string Details(SaveSlotSummary slot)
        {
            switch (slot.Status)
            {
                case SaveSlotStatus.Empty: return "Empty";
                case SaveSlotStatus.Unreadable: return "This save can't be read. It's kept as it is unless you delete it.";
                case SaveSlotStatus.TooNew: return "This save was made by a newer version of the game. Update the game to play it.";
            }
            var h = slot.Header;
            string who = string.IsNullOrEmpty(h.frontingName) ? h.frontingId : h.frontingName;
            string when = h.savedAtUtcTicks > 0 ? new DateTime(h.savedAtUtcTicks, DateTimeKind.Utc).ToLocalTime().ToString("d MMM yyyy, HH:mm") : "";
            var lines = new List<string> { "<b>" + SaveSlotText.Place(h) + "</b>" };
            if (!string.IsNullOrEmpty(who)) lines.Add("Fronting: " + who);
            lines.Add($"Day {Math.Max(1, h.inGameDay)}  ·  played {SaveSlotText.PlayTime(h.playSeconds)}");
            if (when != "") lines.Add("<color=#bfb6a8>Saved " + when + "</color>");
            return string.Join("\n", lines);
        }

        // ── Actions ───────────────────────────────────────────────────────────────

        void StartFromTitle(int slot, bool newGame)
        {
            Close();
            GameSession.BeginFromTitle(
                () => newGame ? NewGameInPlace(slot) : SaveService.TryLoad(slot, runner),
                () => OpenTitle($"Save {slot} couldn't be opened."));
        }

        /// <summary>The state is fresh here (title screen or a reloaded scene): just claim the slot.</summary>
        static bool NewGameInPlace(int slot)
        {
            SaveService.BeginNewGame(slot);
            return SaveService.ActiveSlot == slot;
        }

        void AskDelete(int slot) => Confirm($"Delete Save {slot}? This can't be undone.", "Delete it", () =>
        {
            SaveService.Delete(slot);
            ShowSlots($"Save {slot} was deleted.");
        });

        /// <summary>A question asked on this screen, never in a system pop-up.</summary>
        void Confirm(string question, string yes, Action onYes)
        {
            Clear(_content);
            Clear(_footer);
            _message.text = "";
            var box = Panel(_content, "Question", CardColor, Vector2.zero, Vector2.one);
            var vlg = box.AddComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(80, 80, 80, 80);
            vlg.spacing = 40;
            vlg.childControlHeight = true; vlg.childControlWidth = true;
            vlg.childForceExpandHeight = false; vlg.childForceExpandWidth = true;
            FlowLabel(box.transform, question, 54, FontStyle.Normal, TextColor).alignment = TextAnchor.MiddleCenter;
            Spacer(box.transform);
            var keep = Button(_footer, "Cancel", () => ShowSlots(null), 110, ButtonColor);
            Button(_footer, yes, onYes, 110, QuietButtonColor);
            Select(keep); // The safe answer is the default.
        }

        // ── Building the UI ───────────────────────────────────────────────────────

        void Build()
        {
            _canvasGo = new GameObject("Saves Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            _canvasGo.transform.SetParent(transform, false);
            var canvas = _canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            var scaler = _canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(2732, 2048);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;
            var root = _canvasGo.transform;

            // The in-game "Saves" button, just left of the Anchor button.
            _menuButton = Button(root, "Saves", OpenMenu, 0, QuietButtonColor);
            Stretch(_menuButton.GetComponent<RectTransform>(), new Vector2(0.73f, 0.9f), new Vector2(0.85f, 0.98f));
            Destroy(_menuButton.GetComponent<LayoutElement>());
            _menuButton.SetActive(false);

            _panel = Panel(root, "Saves", TitleBackground, Vector2.zero, Vector2.one);
            _background = _panel.GetComponent<Image>();
            // Leave the top-right corner to the Anchor button (it's drawn above this screen).
            _heading = Label(_panel.transform, "Heading", 96, FontStyle.Bold, TextColor, new Vector2(0.15f, 0.84f), new Vector2(0.85f, 0.96f));
            _message = Label(_panel.transform, "Message", 48, FontStyle.Italic, Dim, new Vector2(0.1f, 0.77f), new Vector2(0.9f, 0.84f));

            _content = Panel(_panel.transform, "Slots", Color.clear, new Vector2(0.06f, 0.18f), new Vector2(0.94f, 0.75f)).GetComponent<RectTransform>();
            _content.GetComponent<Image>().raycastTarget = false;
            var hlg = _content.gameObject.AddComponent<HorizontalLayoutGroup>();
            hlg.spacing = 48;
            hlg.childControlWidth = true; hlg.childControlHeight = true;
            hlg.childForceExpandWidth = true; hlg.childForceExpandHeight = true;

            _footer = Panel(_panel.transform, "Footer", Color.clear, new Vector2(0.25f, 0.04f), new Vector2(0.75f, 0.14f)).GetComponent<RectTransform>();
            _footer.GetComponent<Image>().raycastTarget = false;
            var fhlg = _footer.gameObject.AddComponent<HorizontalLayoutGroup>();
            fhlg.spacing = 40; fhlg.childAlignment = TextAnchor.MiddleCenter;
            fhlg.childControlWidth = true; fhlg.childForceExpandWidth = true; fhlg.childControlHeight = false;
        }

        static void Select(GameObject go)
        {
            if (go != null && EventSystem.current != null) EventSystem.current.SetSelectedGameObject(go);
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

        /// <summary>A label inside a layout group (it takes the height its text needs).</summary>
        Text FlowLabel(Transform parent, string text, int size, FontStyle style, Color color)
        {
            var t = Label(parent, "Text", size, style, color, Vector2.zero, Vector2.one);
            t.alignment = TextAnchor.UpperLeft;
            t.text = text;
            return t;
        }

        static void Spacer(Transform parent)
        {
            var go = new GameObject("Spacer", typeof(RectTransform), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            go.GetComponent<LayoutElement>().flexibleHeight = 1f;
        }

        GameObject Button(Transform parent, string label, Action onClick, float height, Color color)
        {
            var go = new GameObject("Button: " + label, typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            go.GetComponent<Image>().color = color;
            go.GetComponent<LayoutElement>().preferredHeight = height;
            go.GetComponent<LayoutElement>().minHeight = height;
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height);
            go.GetComponent<Button>().onClick.AddListener(() => onClick());
            var t = Label(go.transform, "Label", 46, FontStyle.Normal, TextColor, Vector2.zero, Vector2.one);
            t.text = label;
            return go;
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => _open = null;
    }
}
