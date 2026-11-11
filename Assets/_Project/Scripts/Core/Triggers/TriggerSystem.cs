using System;
using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;

namespace IAmAsIAm.Core.Triggers
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  Triggers (owner's design): the character has triggers. The player learns them
    //  over time and tries to avoid them. When one fires it STARTS dissociation
    //  (depersonalization, derealization or fog), and the player has NO control over
    //  which. It only leads to a full SWITCH if the character can't get away from it
    //  for an extended time (owner, 2026-11-11). Triggers are sensory or situational
    //  cues; the game never shows or explains the trauma behind them.
    //  The trigger list itself is authored with the owner in Resources/Data/triggers.json.
    // ─────────────────────────────────────────────────────────────────────────────

    public enum TriggerResponse
    {
        Depersonalization,
        Derealization,
        Fog,
        /// <summary>
        /// An involuntary switch. It never comes straight from a cue: only from being unable to get
        /// away for <see cref="TriggerDefinition.switchAfterSeconds"/> (or a "trapped" story moment).
        /// Who comes forward isn't the player's choice.
        /// </summary>
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
        /// <summary>Seconds of being unable to get away before a full switch happens.</summary>
        public float switchAfterSeconds = 20f;
        /// <summary>Who comes forward on a switch: a member id, or "any" (any member able to front, picked at random).</summary>
        public string switchTo = "any";
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
        /// <summary>True when this is the switch after sustained exposure, not the first reaction.</summary>
        public bool Escalated;
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
        readonly Dictionary<string, float> _exposure = new Dictionary<string, float>();
        readonly HashSet<string> _switchedThisExposure = new HashSet<string>();
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
        /// It starts dissociation; the response is chosen at random by weight, and the player has no say.
        /// </summary>
        public TriggerActivation Encounter(string cue, SystemRoster roster)
        {
            var def = FindFor(cue, roster);
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
                Response = outcome.Response == TriggerResponse.Switch ? TriggerResponse.Depersonalization : outcome.Response,
                Intensity = Math.Max(0f, Math.Min(1f, outcome.intensity)),
                KnowledgeAfter = after,
            };
            Fired?.Invoke(activation);
            return activation;
        }

        /// <summary>
        /// The cue is still there and the character can't get away (call every frame while it lasts).
        /// The first call is the encounter. After the trigger's switchAfterSeconds, it becomes a full switch,
        /// once per exposure. Returns any activation that happened this call.
        /// </summary>
        public TriggerActivation Expose(string cue, SystemRoster roster, float deltaSeconds)
        {
            var def = FindFor(cue, roster);
            if (def == null) return null;

            if (!_exposure.TryGetValue(def.id, out float time))
            {
                _exposure[def.id] = 0f;
                return Encounter(cue, roster);
            }

            time += Math.Max(0f, deltaSeconds);
            _exposure[def.id] = time;
            if (time < def.switchAfterSeconds || _switchedThisExposure.Contains(def.id)) return null;
            _switchedThisExposure.Add(def.id);
            return Escalate(def, cue, roster);
        }

        /// <summary>The character got away. Exposure resets.</summary>
        public void EndExposure(string cue)
        {
            foreach (var d in _defs.Where(d => d.cues.Contains(cue)))
            {
                _exposure.Remove(d.id);
                _switchedThisExposure.Remove(d.id);
            }
        }

        /// <summary>
        /// A story moment where the character can't leave (e.g. trapped at a wedding through the vows):
        /// the encounter and the switch both happen.
        /// </summary>
        public TriggerActivation Trapped(string cue, SystemRoster roster)
        {
            var def = FindFor(cue, roster);
            if (def == null) return null;
            Encounter(cue, roster);
            return Escalate(def, cue, roster);
        }

        /// <summary>Seconds of exposure so far for the trigger behind a cue (0 if none).</summary>
        public float ExposureSeconds(string cue)
        {
            var def = _defs.FirstOrDefault(d => d.cues.Contains(cue));
            return def != null && _exposure.TryGetValue(def.id, out var t) ? t : 0f;
        }

        TriggerActivation Escalate(TriggerDefinition def, string cue, SystemRoster roster)
        {
            var activation = new TriggerActivation
            {
                Trigger = def,
                Cue = cue,
                Response = TriggerResponse.Switch,
                Intensity = 1f,
                KnowledgeAfter = Knowledge(def.id),
                Escalated = true,
                SwitchToId = ResolveSwitchTarget(def.switchTo, roster),
            };
            // If no one else can come forward, the body still reacts: deep depersonalization instead.
            if (activation.SwitchToId == null) activation.Response = TriggerResponse.Depersonalization;
            Fired?.Invoke(activation);
            return activation;
        }

        TriggerDefinition FindFor(string cue, SystemRoster roster)
        {
            string fronting = roster?.FrontingId;
            return _defs.FirstOrDefault(d => d.cues.Contains(cue) &&
                (d.affectsMembers.Count == 0 || (fronting != null && d.affectsMembers.Contains(fronting))));
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
