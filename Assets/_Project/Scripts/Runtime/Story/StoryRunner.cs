using System;
using System.Collections.Generic;
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
        static readonly HashSet<string> ActionTags = new HashSet<string> { "exercise", "ritual", "explore", "chapter-end" };

        [SerializeField] TextAsset compiledStory;
        [SerializeField] FrontingController fronting;

        Ink.Runtime.Story _story;
        StoryAction _pending;
        bool _announced;
        string _chapterId = "";

        public event Action<StoryLine> LineReady;
        public event Action<List<Choice>> ChoicesReady;
        public event Action<StoryAction> ActionRequested;
        /// <summary>A content note from a "# cw:" tag. The view may offer to skip.</summary>
        public event Action<string> ContentNote;
        /// <summary>A conversation or look-at began; partner is the person talked to (or null for objects).</summary>
        public event Action<Transform> Started;
        public event Action Ended;

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

            if (_story.currentChoices.Count > 0) ChoicesReady?.Invoke(_story.currentChoices);
            else { IsPlaying = false; Ended?.Invoke(); }
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

        public string SaveState() => _story?.state.ToJson();
        public void LoadState(string json) { if (_story != null && !string.IsNullOrEmpty(json)) _story.state.LoadJson(json); }

        void HandleTags(List<string> tags)
        {
            foreach (var raw in tags)
            {
                int colon = raw.IndexOf(':');
                string key = (colon < 0 ? raw : raw.Substring(0, colon)).Trim();
                string value = colon < 0 ? "" : raw.Substring(colon + 1).Trim();

                if (key == "cw") ContentNote?.Invoke(value);
                else if (key == "cue") EncounterCue(value);
                else if (ActionTags.Contains(key)) _pending = new StoryAction { Kind = key, Value = value };
                // "shot", "place" and "front" are read by the view and camera from line.Tags.
            }
        }

        /// <summary>The character meets a trigger cue. What happens isn't the player's choice.</summary>
        void EncounterCue(string cue)
        {
            if (TriggerRunner.Encounter(cue, fronting) != null) SyncFronting();
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
            _story.BindExternalFunctionGeneral("card_prompt", a => GameServices.Practices?.GetCard(S(a[0]))?.reflectionPrompt ?? "");
        }
    }
}
