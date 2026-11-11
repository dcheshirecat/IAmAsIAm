using System;
using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;

namespace IAmAsIAm.Core.Triggers
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  Triggers (owner's design): the character has triggers. The player learns them
    //  over time and tries to avoid them. When one fires, it causes unwanted
    //  depersonalization, derealization or a switch, and the player has NO control
    //  over which. Triggers are sensory or situational cues; the game never shows
    //  or explains the trauma behind them.
    //  The trigger list itself is authored with the owner in Resources/Data/triggers.json.
    // ─────────────────────────────────────────────────────────────────────────────

    public enum TriggerResponse
    {
        Depersonalization,
        Derealization,
        Fog,
        /// <summary>An involuntary switch. Who comes forward isn't the player's choice.</summary>
        Switch,
    }

    [Serializable]
    public sealed class TriggerOutcome
    {
        /// <summary>A <see cref="TriggerResponse"/> name.</summary>
        public string response;
        /// <summary>Relative likelihood among this trigger's outcomes.</summary>
        public float weight = 1f;
        /// <summary>0..1 intensity for perceptual responses.</summary>
        public float intensity = 0.7f;
        /// <summary>For Switch: a member id, or "any" (any member able to front, picked at random).</summary>
        public string switchTo = "any";

        public TriggerResponse Response => EnumText.Parse(response, TriggerResponse.Derealization);
    }

    [Serializable]
    public sealed class TriggerDefinition
    {
        public string id;
        /// <summary>Shown once the trigger is fully learned, e.g. "Raised voices".</summary>
        public string name;
        /// <summary>Shown after the first time, before it's understood, e.g. "Something about loud rooms…"</summary>
        public string hint;
        /// <summary>Cue ids that set it off. Scenes and Ink emit cues ("# cue: raised-voices").</summary>
        public List<string> cues = new List<string>();
        public List<TriggerOutcome> outcomes = new List<TriggerOutcome>();
        /// <summary>Encounters before the player fully understands this trigger.</summary>
        public int learnAfter = 2;
        /// <summary>Member ids this trigger affects. Empty means it affects whoever is fronting.</summary>
        public List<string> affectsMembers = new List<string>();
        /// <summary>Content tags, for content notes.</summary>
        public List<string> contentTags = new List<string>();
    }

    [Serializable]
    public sealed class TriggerData
    {
        public string sourceNote;
        public List<TriggerDefinition> triggers = new List<TriggerDefinition>();
    }

    public enum TriggerKnowledge { Unknown, Noticed, Known }

    /// <summary>What happened when a trigger fired. The Runtime applies it (FrontingController for switches).</summary>
    public sealed class TriggerActivation
    {
        public TriggerDefinition Trigger;
        public string Cue;
        public TriggerResponse Response;
        public float Intensity;
        /// <summary>For switches: who comes forward. Null if no one else can front.</summary>
        public string SwitchToId;
        public TriggerKnowledge KnowledgeAfter;
    }

    [Serializable]
    public sealed class TriggerProgress
    {
        public string id;
        public int encounters;
        public int avoided;
    }

    public sealed class TriggerSystem
    {
        readonly List<TriggerDefinition> _defs;
        readonly Dictionary<string, TriggerProgress> _progress = new Dictionary<string, TriggerProgress>();
        readonly Random _rng;

        public event Action<TriggerActivation> Fired;
        public event Action<TriggerDefinition, TriggerKnowledge> Learned;

        public IEnumerable<TriggerDefinition> All => _defs;

        public TriggerSystem(IEnumerable<TriggerDefinition> definitions, int seed)
        {
            _defs = definitions?.ToList() ?? new List<TriggerDefinition>();
            _rng = new Random(seed);
            foreach (var d in _defs) _progress[d.id] = new TriggerProgress { id = d.id };
        }

        public TriggerKnowledge Knowledge(string triggerId)
        {
            var def = _defs.Find(d => d.id == triggerId);
            if (def == null || !_progress.TryGetValue(triggerId, out var p) || p.encounters == 0) return TriggerKnowledge.Unknown;
            return p.encounters >= Math.Max(1, def.learnAfter) ? TriggerKnowledge.Known : TriggerKnowledge.Noticed;
        }

        /// <summary>Known cues: lets the UI and story mark risky options once the player has learned them.</summary>
        public bool IsKnownCue(string cue) =>
            _defs.Any(d => d.cues.Contains(cue) && Knowledge(d.id) == TriggerKnowledge.Known);

        /// <summary>
        /// The character meets a cue. Returns the activation if a trigger fires, or null.
        /// The response is chosen at random by weight; the player has no say.
        /// </summary>
        public TriggerActivation Encounter(string cue, SystemRoster roster)
        {
            string fronting = roster?.FrontingId;
            var def = _defs.FirstOrDefault(d => d.cues.Contains(cue) &&
                (d.affectsMembers.Count == 0 || (fronting != null && d.affectsMembers.Contains(fronting))));
            if (def == null || def.outcomes.Count == 0) return null;

            var before = Knowledge(def.id);
            _progress[def.id].encounters++;
            var after = Knowledge(def.id);
            if (after != before) Learned?.Invoke(def, after);

            var outcome = PickWeighted(def.outcomes);
            var activation = new TriggerActivation
            {
                Trigger = def,
                Cue = cue,
                Response = outcome.Response,
                Intensity = Math.Max(0f, Math.Min(1f, outcome.intensity)),
                KnowledgeAfter = after,
            };

            if (activation.Response == TriggerResponse.Switch)
            {
                activation.SwitchToId = ResolveSwitchTarget(outcome.switchTo, roster);
                // If no one else can come forward, the body still reacts: fall back to depersonalization.
                if (activation.SwitchToId == null) activation.Response = TriggerResponse.Depersonalization;
            }

            Fired?.Invoke(activation);
            return activation;
        }

        /// <summary>The player knowingly steered around a known cue. This is tracked so the game can acknowledge it.</summary>
        public void RecordAvoided(string cue)
        {
            foreach (var d in _defs.Where(d => d.cues.Contains(cue)))
                _progress[d.id].avoided++;
        }

        public int Avoided(string triggerId) => _progress.TryGetValue(triggerId, out var p) ? p.avoided : 0;

        /// <summary>
        /// Applies the perceptual part of an activation to the mind. Switches are applied by the
        /// Runtime FrontingController, because they involve time loss and the camera.
        /// </summary>
        public static void ApplyToMind(TriggerActivation a, MindModel mind)
        {
            if (a == null || mind == null) return;
            switch (a.Response)
            {
                case TriggerResponse.Depersonalization: mind.SetExperience(DissociativeExperience.Depersonalization, a.Intensity); break;
                case TriggerResponse.Derealization: mind.SetExperience(DissociativeExperience.Derealization, a.Intensity); break;
                case TriggerResponse.Fog: mind.Nudge(-a.Intensity); break;
                case TriggerResponse.Switch: mind.SetExperience(DissociativeExperience.TimeLoss, 1f); break;
            }
            // Every trigger also jolts arousal out of the window.
            if (a.Response != TriggerResponse.Fog) mind.Nudge(0.4f * a.Intensity);
        }

        public IEnumerable<TriggerProgress> ExportProgress() => _progress.Values;

        public void Restore(IEnumerable<TriggerProgress> progress)
        {
            if (progress == null) return;
            foreach (var p in progress)
                if (_progress.ContainsKey(p.id)) _progress[p.id] = p;
        }

        TriggerOutcome PickWeighted(List<TriggerOutcome> outcomes)
        {
            float total = outcomes.Sum(o => Math.Max(0f, o.weight));
            if (total <= 0f) return outcomes[0];
            double roll = _rng.NextDouble() * total;
            foreach (var o in outcomes)
            {
                roll -= Math.Max(0f, o.weight);
                if (roll <= 0) return o;
            }
            return outcomes[outcomes.Count - 1];
        }

        string ResolveSwitchTarget(string switchTo, SystemRoster roster)
        {
            if (roster == null) return null;
            bool Eligible(SystemMemberDefinition m) =>
                m.id != roster.FrontingId && m.canFront && roster.GetState(m.id)?.presentation == MemberPresentation.Embodied;

            if (!string.IsNullOrEmpty(switchTo) && switchTo != "any")
            {
                var target = roster.GetDefinition(switchTo);
                return target != null && Eligible(target) ? target.id : null;
            }
            var candidates = roster.Members.Where(Eligible).ToList();
            return candidates.Count == 0 ? null : candidates[_rng.Next(candidates.Count)].id;
        }
    }
}
