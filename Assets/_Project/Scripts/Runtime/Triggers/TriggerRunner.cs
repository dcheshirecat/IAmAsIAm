using System;
using IAmAsIAm.Core.Triggers;
using IAmAsIAm.Runtime.PluralSystem;
using UnityEngine;

namespace IAmAsIAm.Runtime.Triggers
{
    /// <summary>
    /// The one place a trigger cue is met, whether from the story ("# cue:" / "# cue-held:") or from a
    /// <see cref="CueZone"/>. It applies what happens: the player doesn't choose. A cue starts dissociation;
    /// only being unable to get away for long enough leads to a switch.
    /// </summary>
    public static class TriggerRunner
    {
        public static event Action<TriggerActivation> Fired;

        /// <summary>A brief encounter: dissociation starts; no switch.</summary>
        public static TriggerActivation Encounter(string cue, FrontingController fronting = null) =>
            Apply(GameServices.Triggers?.Encounter(cue, GameServices.Roster), fronting);

        /// <summary>The cue continues and the character can't get away: call every frame. A switch follows if it lasts.</summary>
        public static TriggerActivation Expose(string cue, float deltaSeconds, FrontingController fronting = null) =>
            Apply(GameServices.Triggers?.Expose(cue, GameServices.Roster, deltaSeconds), fronting);

        /// <summary>The character got away: exposure resets.</summary>
        public static void EndExposure(string cue) => GameServices.Triggers?.EndExposure(cue);

        /// <summary>A story moment with no way out: dissociation, then a switch.</summary>
        public static TriggerActivation Trapped(string cue, FrontingController fronting = null) =>
            Apply(GameServices.Triggers?.Trapped(cue, GameServices.Roster), fronting);

        static TriggerActivation Apply(TriggerActivation activation, FrontingController fronting)
        {
            if (activation == null) return null;
            TriggerSystem.ApplyToMind(activation, GameServices.Mind);
            if (activation.Response == TriggerResponse.Switch && activation.SwitchToId != null)
            {
                if (fronting == null) fronting = UnityEngine.Object.FindAnyObjectByType<FrontingController>();
                if (fronting != null) fronting.StoryForcesSwitch(activation.SwitchToId);
                else GameServices.Roster.ForceSwitch(activation.SwitchToId);
            }
            Fired?.Invoke(activation);
            return activation;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => Fired = null;
    }
}
