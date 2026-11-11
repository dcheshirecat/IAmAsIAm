using System;
using IAmAsIAm.Core.Triggers;
using IAmAsIAm.Runtime.PluralSystem;
using UnityEngine;

namespace IAmAsIAm.Runtime.Triggers
{
    /// <summary>
    /// The one place a trigger cue is met, whether from the story ("# cue:") or from walking into a
    /// <see cref="CueZone"/>. It applies what happens: the player doesn't choose.
    /// </summary>
    public static class TriggerRunner
    {
        public static event Action<TriggerActivation> Fired;

        public static TriggerActivation Encounter(string cue, FrontingController fronting = null)
        {
            var triggers = GameServices.Triggers;
            if (triggers == null || string.IsNullOrEmpty(cue)) return null;
            var activation = triggers.Encounter(cue, GameServices.Roster);
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
