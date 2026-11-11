using IAmAsIAm.Core.Mind;
using UnityEngine;

namespace IAmAsIAm.Runtime.Mind
{
    /// <summary>
    /// Ticks the <see cref="MindModel"/>, and gives narrative and Timeline a simple way to drive it.
    /// Uses unscaled time, so the mind keeps settling while the Anchor menu has the game paused.
    /// </summary>
    public sealed class MindStateController : MonoBehaviour
    {
        [Tooltip("While true, scripted experiences don't fade by themselves (the scene is holding them).")]
        public bool holdScriptedExperiences;

        MindModel Mind => GameServices.Mind;

        void Update()
        {
            Mind?.Tick(Time.unscaledDeltaTime, holdScriptedExperiences);
        }

        // These are convenience hooks for UnityEvents, Timeline signals and dialogue commands.

        public void BeginDerealization(float intensity) => Mind?.SetExperience(DissociativeExperience.Derealization, intensity);
        public void BeginDepersonalization(float intensity) => Mind?.SetExperience(DissociativeExperience.Depersonalization, intensity);
        public void BeginFog(float intensity) => Mind?.SetExperience(DissociativeExperience.Fog, intensity);
        public void SetCoConsciousness(float intensity) => Mind?.SetExperience(DissociativeExperience.CoConsciousness, intensity);
        public void Stressor(float amount) => Mind?.Nudge(Mathf.Abs(amount));
        public void Shutdown(float amount) => Mind?.Nudge(-Mathf.Abs(amount));
        public void HoldExperiences(bool hold) => holdScriptedExperiences = hold;
    }
}
