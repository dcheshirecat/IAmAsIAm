using System;
using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Story;
using IAmAsIAm.Core.Triggers;
using IAmAsIAm.Runtime.PluralSystem;
using IAmAsIAm.Runtime.Triggers;
using Ink.Runtime;
using UnityEngine;

namespace IAmAsIAm.Runtime.Story
{
    /// <summary>One line of story, parsed for the dialogue view.</summary>
    public sealed class StoryLine
    {
        /// <summary>Speaker name, or null for narration.</summary>
        public string Speaker;
        /// <summary>True for "Name (inside): ..." (an inner voice / co-consciousness).</summary>
        public bool Inner;
        /// <summary>True for "Name (on video): ..." or "Name (on the phone): ...": someone on a call, so not standing in the place.</summary>
        public bool Remote;
        /// <summary>How a remote speaker is there: "on video" or "on the phone".</summary>
        public string Via;
        public string Text;
        public List<string> Tags = new List<string>();
    }

    /// <summary>
    /// An interactive moment the story hands to the game (from a tag), e.g. "exercise" / "five-senses".
    /// The story waits until the view calls <see cref="StoryRunner.CompleteAction"/>.
    /// </summary>
    public sealed class StoryAction
    {
        public string Kind;
        public string Value;
    }

    /// <summary>
    /// Runs the Ink story (Assets/_Project/Story/main.ink, compiled to main.json by the Ink package).
    /// It binds the game functions declared in game_api.ink, turns tags into actions, and feeds a view.
    /// </summary>
    public sealed class StoryRunner : MonoBehaviour
    {
        static readonly HashSet<string> ActionTags = new HashSet<string> { "exercise", "ritual", "explore", "chapter-end", "device" };

        [SerializeField] TextAsset compiledStory;
        [SerializeField] FrontingController fronting;

        Ink.Runtime.Story _story;
        StoryAction _pending;
        bool _announced;
        string _chapterId = "";
        /// <summary>The chapter being played (chapters.json id), or empty.</summary>
        public string ChapterId => _chapterId;

        public event Action<StoryLine> LineReady;
        public event Action<List<Choice>> ChoicesReady;
        public event Action<StoryAction> ActionRequested;
        /// <summary>A content note from a "# cw:" tag. The view may offer to skip.</summary>
        public event Action<string> ContentNote;
        /// <summary>A conversation or look-at began; partner is the person talked to (or null for objects).</summary>
        public event Action<Transform> Started;
        public event Action Ended;
        /// <summary>The goal line changed (# objective:). Empty clears it.</summary>
        public event Action<string> ObjectiveChanged;
        /// <summary>A <c># place:</c> tag: the character is now somewhere else (the world builds it to explore).</summary>
        public event Action<string> PlaceChanged;
        /// <summary>Where the story is right now (the last <c># place:</c>), or null outside a story.</summary>
        public string Place { get; private set; }
        public string Objective { get; private set; } = "";

        public bool IsWaitingForAction => _pending != null;
        public bool IsPlaying { get; private set; }
        public Ink.Runtime.Story Story => _story;

        /// <summary>Loads the compiled story once. Story state (variables, visits) persists between plays.</summary>
        public bool EnsureLoaded()
        {
            if (_story != null) return true;
            if (compiledStory == null)
            {
                Debug.LogError("[IAmAsIAm] StoryRunner has no compiled story. Run IAmAsIAm → Compile Story, then Rebuild Boot Scene.");
                return false;
            }
            _story = new Ink.Runtime.Story(compiledStory.text);
            _story.onError += (msg, type) =>
            {
                if (type == Ink.ErrorType.Error) Debug.LogError("[Ink] " + msg);
                else Debug.LogWarning("[Ink] " + msg);
            };
            Bind();
            return true;
        }

        /// <summary>Plays a knot: a conversation, a look at something, a scene. Null plays from the story's start.</summary>
        public void Play(string knot, Transform partner = null, string chapterId = null)
        {
            if (!EnsureLoaded() || IsPlaying) return;
            if (chapterId != null) _chapterId = chapterId;
            SyncFronting();
            if (!string.IsNullOrEmpty(knot)) _story.ChoosePathString(knot);
            IsPlaying = true;
            _cutNext = true;
            Started?.Invoke(partner);
            Next();
        }

        /// <summary>Advance: the view calls this when the player taps to continue.</summary>
        public void Next()
        {
            if (_story == null) return;

            // A line that carried an action tag has been read: now start the interactive moment.
            if (_pending != null)
            {
                if (!_announced) { _announced = true; ActionRequested?.Invoke(_pending); }
                return;
            }

            if (_story.canContinue)
            {
                var line = Parse(_story.Continue(), _story.currentTags);
                HandleTags(line.Tags);
                if (!string.IsNullOrWhiteSpace(line.Text)) LineReady?.Invoke(line);
                else Next(); // Tag-only line: move straight on (or straight into its action).
                return;
            }

            if (_story.currentChoices.Count > 0)
            {
                // A safe point to save: waiting at a choice, nothing half-done.
                if (_pending == null)
                {
                    Checkpoint = new StoryCheckpoint
                    {
                        Ink = _story.state.ToJson(), Place = Place, Objective = Objective, ChapterId = _chapterId,
                        Path = _story.currentChoices[0].sourcePath ?? _story.state.currentPathString ?? "",
                    };
                    CheckpointReached?.Invoke();
                }
                ChoicesReady?.Invoke(_story.currentChoices);
            }
            else { IsPlaying = false; Place = null; Checkpoint = null; Ended?.Invoke(); }
        }

        public void Choose(int index)
        {
            if (_story == null || index < 0 || index >= _story.currentChoices.Count) return;
            _story.ChooseChoiceIndex(index);
            Next();
        }

        /// <summary>The interactive moment is done. Optional story variables carry its results (e.g. a tarot draw).</summary>
        public void CompleteAction(IDictionary<string, string> results = null)
        {
            if (_pending == null) return;
            _pending = null;
            _announced = false;
            if (results != null)
                foreach (var kv in results)
                    if (_story.variablesState.GlobalVariableExistsWithName(kv.Key)) _story.variablesState[kv.Key] = kv.Value;
            Next();
        }

        /// <summary>
        /// A safe point to save in the middle of a chapter: the story is waiting at a choice, nothing half-done. Taken every
        /// time choices appear, so a chapter can be picked up again after the app closes (owner: "Continue" started over).
        /// </summary>
        public sealed class StoryCheckpoint
        {
            public string Ink;
            public string Place;
            public string Objective;
            public string ChapterId;
            public string Path;
        }

        public StoryCheckpoint Checkpoint { get; private set; }
        /// <summary>Raised when a new checkpoint is taken (the save screen writes it to the active slot).</summary>
        public event Action CheckpointReached;

        /// <summary>
        /// Picks a chapter back up from a save: the story's state is put back at its checkpoint, the place is rebuilt and the
        /// goal line shown, and the choices come up again. If the story changed in an update and the exact spot is gone, the
        /// chapter part starts again from its beginning.
        /// </summary>
        public bool Resume(StoryCheckpoint cp)
        {
            if (cp == null || !EnsureLoaded() || IsPlaying) return false;
            bool exact = false;
            try { _story.state.LoadJson(cp.Ink); exact = _story.currentChoices.Count > 0; }
            catch (Exception e) { Debug.LogWarning($"[IAmAsIAm] The saved story position couldn't be restored exactly: {e.Message}"); }
            if (!exact)
            {
                string part = string.IsNullOrEmpty(cp.Path) ? "" : cp.Path.Split('.')[0];
                if (string.IsNullOrEmpty(part) || _story.KnotContainerWithName(part) == null) return false;
                _story.ChoosePathString(part);
            }
            _chapterId = cp.ChapterId;
            SyncFronting();
            IsPlaying = true;
            Started?.Invoke(null);
            if (exact)
            {
                Place = null;
                _cutNext = true;
                if (!string.IsNullOrEmpty(cp.Place)) GoTo(cp.Place);
                Objective = cp.Objective ?? "";
                ObjectiveChanged?.Invoke(Objective);
            }
            else { Place = null; _cutNext = true; }
            Next();
            return true;
        }

        public string SaveState() => _story?.state.ToJson();
        public void LoadState(string json) { if (!string.IsNullOrEmpty(json) && EnsureLoaded()) _story.state.LoadJson(json); }

        void HandleTags(List<string> tags)
        {
            foreach (var raw in tags)
            {
                int colon = raw.IndexOf(':');
                string key = (colon < 0 ? raw : raw.Substring(0, colon)).Trim();
                string value = colon < 0 ? "" : raw.Substring(colon + 1).Trim();

                if (key == "cw") ContentNote?.Invoke(value);
                else if (key == "cue") EncounterCue(value, trapped: false);
                else if (key == "cue-held") EncounterCue(value, trapped: true);
                else if (key == "switch") ScriptedSwitch(value);
                else if (key == "objective") { Objective = value; ObjectiveChanged?.Invoke(value); }
                else if (key == "maybe-switch") MaybeSwitch(value);
                else if (key == "place" && value.Length > 0 && value != Place) GoTo(value);
                else if (ActionTags.Contains(key))
                {
                    // Two moments on one line: the walk there becomes a cut, so the other moment still happens.
                    if (_pending != null && _pending.Kind == "travel") PlaceChanged?.Invoke(_pending.Value);
                    _pending = new StoryAction { Kind = key, Value = value };
                }
                // "shot" and "front" are read by the view and camera from line.Tags.
            }
        }

        /// <summary>
        /// The story moves somewhere else (owner: "Maya should be able to wander, guided by the story, to get from scene to
        /// scene within a part"). Within a part she walks there: the story waits for a "travel" moment (a way out of the
        /// place, then the map from college on). The first place of a run, the first place of each part (set_age marks a
        /// part's start: a jump in time) and the black screen are cuts: she's simply there.
        /// </summary>
        void GoTo(string place)
        {
            bool cut = _cutNext || Place == null || Place == "black" || place == "black" || _pending != null;
            Place = place;
            _cutNext = false;
            if (cut) PlaceChanged?.Invoke(place);
            else _pending = new StoryAction { Kind = "travel", Value = place };
        }

        bool _cutNext = true;

        /// <summary>The character meets a trigger cue. What happens isn't the player's choice.</summary>
        void EncounterCue(string cue, bool trapped)
        {
            var a = trapped ? TriggerRunner.Trapped(cue, fronting) : TriggerRunner.Encounter(cue, fronting);
            if (a != null) SyncFronting();
        }

        /// <summary>A switch the story itself calls for (e.g. Nina at the end of the prologue). Never the player's choice.</summary>
        void ScriptedSwitch(string memberId)
        {
            var roster = GameServices.Roster;
            if (roster == null || string.IsNullOrEmpty(memberId)) return;
            try { roster.ForceSwitch(memberId); }
            catch (ArgumentException) { Debug.LogWarning($"[IAmAsIAm] # switch: unknown member '{memberId}'."); return; }
            SyncFronting();
        }

        static readonly System.Random Chance = new System.Random();

        /// <summary>
        /// Owner's rule: once another alter's version of a chapter is unlocked, a switch can always happen unexpectedly.
        /// Here the story marks moments where it may: a chance (default 15%) of switching to an unlocked alter.
        /// </summary>
        void MaybeSwitch(string value)
        {
            var roster = GameServices.Roster;
            var runs = GameServices.ChapterRuns;
            if (roster == null || runs == null) return;
            var pool = new List<string>();
            // Owner: only the host or an alter already unlocked.
            foreach (var m in roster.Members) if (m.id != roster.FrontingId && m.canFront && runs.CanSwitchTo(m.id)) pool.Add(m.id);
            if (pool.Count == 0) return;
            int percent = int.TryParse(value, out int p) ? Mathf.Clamp(p, 1, 100) : 15;
            if (Chance.Next(100) >= percent) return;
            ScriptedSwitch(pool[Chance.Next(pool.Count)]);
        }

        void SyncFronting()
        {
            if (_story != null && GameServices.Roster != null && _story.variablesState.GlobalVariableExistsWithName("fronting"))
                _story.variablesState["fronting"] = GameServices.Roster.FrontingId;
        }

        static StoryLine Parse(string text, List<string> tags)
        {
            var line = new StoryLine { Text = (text ?? "").Trim(), Tags = new List<string>(tags) };
            int colon = line.Text.IndexOf(": ", StringComparison.Ordinal);
            // A speaker prefix is short and has no sentence punctuation before the colon.
            if (colon > 0 && colon <= 32 && line.Text.Substring(0, colon).IndexOfAny(new[] { '.', '!', '?', '"' }) < 0)
            {
                string name = line.Text.Substring(0, colon);
                if (name.EndsWith(" (inside)")) { line.Inner = true; name = name.Substring(0, name.Length - 9); }
                else if (name.EndsWith(" (on video)")) { line.Remote = true; line.Via = "on video"; name = name.Substring(0, name.Length - 11); }
                else if (name.EndsWith(" (on the phone)")) { line.Remote = true; line.Via = "on the phone"; name = name.Substring(0, name.Length - 15); }
                line.Speaker = name;
                line.Text = line.Text.Substring(colon + 2);
            }
            return line;
        }

        void Bind()
        {
            string S(object o) => o?.ToString() ?? "";
            float F(object o) => o is int i ? i : o is float f ? f : float.TryParse(S(o), out var p) ? p : 0f;

            _story.BindExternalFunctionGeneral("set_experience", a =>
            {
                if (Enum.TryParse<DissociativeExperience>(S(a[0]), out var e)) GameServices.Mind?.SetExperience(e, F(a[1]));
                return null;
            });
            _story.BindExternalFunctionGeneral("nudge", a => { GameServices.Mind?.Nudge(F(a[0])); return null; });
            _story.BindExternalFunctionGeneral("adjust_trust", a => { GameServices.Roster?.AdjustTrust(S(a[0]), F(a[1])); return null; });
            _story.BindExternalFunctionGeneral("learn_skill", a => { GameServices.Skills?.Learn(S(a[0])); return null; });
            _story.BindExternalFunctionGeneral("record_choice", a =>
            {
                GameServices.Choices?.Record(new ChoiceRecord
                {
                    decisionId = S(a[0]), optionId = S(a[1]), summary = S(a[2]),
                    chapterId = _chapterId, madeById = GameServices.Roster?.FrontingId,
                });
                return null;
            });
            _story.BindExternalFunctionGeneral("journal", a =>
            {
                if (Enum.TryParse<JournalEntryKind>(S(a[0]), out var kind))
                    GameServices.Journal?.Write(kind, GameServices.Roster?.FrontingId, GameServices.InGameDay, S(a[1]));
                return null;
            });
            _story.BindExternalFunctionGeneral("open_gap", a =>
            {
                GameServices.Journal?.OpenGap(S(a[0]), GameServices.InGameDay, null, S(a[1]).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                return null;
            });
            _story.BindExternalFunctionGeneral("find_clue", a => { GameServices.Journal?.FindClue(S(a[0]), S(a[1])); return null; });
            _story.BindExternalFunctionGeneral("explore_tradition", a => { GameServices.Traditions?.Explore(S(a[0])); return null; });
            _story.BindExternalFunctionGeneral("card_name", a => GameServices.Practices?.GetCard(S(a[0]))?.name ?? "");
            _story.BindExternalFunctionGeneral("unlocked", a =>
            {
                string id = S(a[0]);
                return id == GameServices.Roster?.FrontingId || (GameServices.ChapterRuns?.IsUnlockedAnywhere(id) ?? false);
            });
            // Devices (owner): the family computer at 12, a phone and a laptop at 18; texts and emails.
            var devices = GameServices.Devices;
            _story.BindExternalFunctionGeneral("set_age", a =>
            {
                devices?.SetAge(Convert.ToInt32(a[0]));
                _cutNext = true;
                // A new part: whoever is here (fronting or sharing time) gets known a little better (owner: their own
                // practices open by getting to know them better and better).
                var r = GameServices.Roster;
                if (r != null && GameServices.Bonds != null)
                {
                    if (r.FrontingId != "maya") GameServices.Bonds.GetToKnow(r.FrontingId, 1);
                    foreach (var c in r.CoConscious) if (c != "maya") GameServices.Bonds.GetToKnow(c, 1);
                }
                return null;
            });
            _story.BindExternalFunctionGeneral("get_to_know", a => { GameServices.Bonds?.GetToKnow(S(a[0]), Convert.ToInt32(a[1])); return null; });
            _story.BindExternalFunctionGeneral("gain_device", a =>
            {
                if (Enum.TryParse<IAmAsIAm.Core.Devices.DeviceKind>(S(a[0]), true, out var kind)) devices?.Gain(kind);
                else Debug.LogError($"[IAmAsIAm] gain_device: unknown device '{a[0]}'");
                return null;
            });
            // The story's date (owner: a moon phase always shown). Near a sabbat, Tanya texts (owner: Tanya's sabbat texts).
            _story.BindExternalFunctionGeneral("set_date", a =>
            {
                try
                {
                    var date = new DateTime(Convert.ToInt32(a[0]), Convert.ToInt32(a[1]), Convert.ToInt32(a[2]));
                    var sabbat = GameServices.Book?.SetDate(date);
                    if (sabbat != null) devices?.Receive(IAmAsIAm.Core.Devices.MessageChannel.Text, "tanya", sabbat.tanyaText, GameServices.InGameDay);
                }
                catch (ArgumentException e) { Debug.LogError($"[IAmAsIAm] set_date: bad date ({e.Message})"); }
                return null;
            });
            _story.BindExternalFunctionGeneral("text_from", a => { devices?.Receive(IAmAsIAm.Core.Devices.MessageChannel.Text, S(a[0]), S(a[1]), GameServices.InGameDay); return null; });
            _story.BindExternalFunctionGeneral("text_to", a => { devices?.Send(IAmAsIAm.Core.Devices.MessageChannel.Text, S(a[0]), S(a[1]), GameServices.InGameDay); return null; });
            _story.BindExternalFunctionGeneral("email_from", a => { devices?.Receive(IAmAsIAm.Core.Devices.MessageChannel.Email, S(a[0]), S(a[2]), GameServices.InGameDay, S(a[1])); return null; });
            _story.BindExternalFunctionGeneral("email_to", a => { devices?.Send(IAmAsIAm.Core.Devices.MessageChannel.Email, S(a[0]), S(a[2]), GameServices.InGameDay, S(a[1])); return null; });
            _story.BindExternalFunctionGeneral("gain_app", a => { devices?.GainApp(S(a[0])); return null; });
            _story.BindExternalFunctionGeneral("system_log", a =>
            {
                devices?.Log(S(a[0]), S(a[1]), GameServices.InGameDay, _chapterId);
                return null;
            });
            _story.BindExternalFunctionGeneral("co_conscious", a =>
            {
                var roster = GameServices.Roster;
                bool present = a[1] is bool b ? b : Convert.ToInt32(a[1]) != 0;
                try { roster?.SetCoConscious(S(a[0]), present); }
                catch (ArgumentException) { Debug.LogWarning($"[IAmAsIAm] co_conscious: unknown member '{S(a[0])}'."); }
                return null;
            });
            _story.BindExternalFunctionGeneral("sent_while_away", a =>
            {
                var channel = S(a[1]).Equals("email", StringComparison.OrdinalIgnoreCase)
                    ? IAmAsIAm.Core.Devices.MessageChannel.Email : IAmAsIAm.Core.Devices.MessageChannel.Text;
                devices?.SentWhileAway(S(a[0]), channel, S(a[2]), S(a[3]), GameServices.InGameDay);
                return null;
            });
            // The Book of Shadows (owner: Tanya gives it at Yule; only Maya and Izzy practise Wicca).
            _story.BindExternalFunctionGeneral("gain_book_of_shadows", a => { GameServices.Book?.Give(S(a[0]), GameServices.InGameDay); return null; });
            _story.BindExternalFunctionGeneral("gift_charm_bag", a =>
            {
                var lib = GameServices.Practices;
                var book = GameServices.Book;
                if (book == null) return null;
                string herb = lib?.Herbs.FirstOrDefault(h => h.id == S(a[1]))?.commonName ?? S(a[1]);
                string stone = lib?.Crystals.FirstOrDefault(c => c.id == S(a[2]))?.name ?? S(a[2]);
                book.GainIngredient("herb", S(a[1]), S(a[0]), GameServices.InGameDay, _chapterId);
                book.GainIngredient("stone", S(a[2]), S(a[0]), GameServices.InGameDay, _chapterId);
                book.MakeCharmBag(herb, stone, "made for you", S(a[3]), GameServices.InGameDay);
                return null;
            });
            _story.BindExternalFunctionGeneral("honour_ancestor", a => { GameServices.Book?.Honour(S(a[0]), GameServices.InGameDay); return null; });
            _story.BindExternalFunctionGeneral("gain_herb", a => { GameServices.Book?.GainIngredient("herb", S(a[0]), S(a[1]), GameServices.InGameDay, _chapterId); return null; });
            _story.BindExternalFunctionGeneral("gain_stone", a => { GameServices.Book?.GainIngredient("stone", S(a[0]), S(a[1]), GameServices.InGameDay, _chapterId); return null; });
            _story.BindExternalFunctionGeneral("learn_practice", a =>
            {
                if (GameServices.Book?.Practice(S(a[0])) == null) Debug.LogError($"[IAmAsIAm] learn_practice: unknown practice '{a[0]}' (craft.json)");
                else GameServices.Book.Learn(S(a[0]), S(a[1]), GameServices.InGameDay, _chapterId);
                return null;
            });
            _story.BindExternalFunctionGeneral("bos_entry", a =>
            {
                if (!Enum.TryParse<IAmAsIAm.Core.Practices.BookTab>(S(a[1]), true, out var tab)) tab = IAmAsIAm.Core.Practices.BookTab.Notes;
                GameServices.Book?.Write(tab, S(a[0]), S(a[2]), S(a[3]), GameServices.InGameDay, _chapterId);
                return null;
            });
            _story.BindExternalFunctionGeneral("practised", a => GameServices.Book != null && GameServices.Book.HasLearned(S(a[0])));
            // Hidden memories (owner): only while that alter is fronting at this moment.
            _story.BindExternalFunctionGeneral("fronts", a => S(a[0]) == GameServices.Roster?.FrontingId);
            _story.BindExternalFunctionGeneral("card_prompt", a => GameServices.Practices?.GetCard(S(a[0]))?.reflectionPrompt ?? "");
        }
    }
}
