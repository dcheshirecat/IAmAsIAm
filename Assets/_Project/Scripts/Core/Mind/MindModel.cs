using System;

namespace IAmAsIAm.Core.Mind
{
    /// <summary>
    /// The system's moment-to-moment state: arousal relative to the window of tolerance, plus
    /// the intensity of each dissociative experience.
    ///
    /// Design rules (see GameDesignDocument.md §2):
    ///  * There is no failure state. Leaving the window changes perception, never outcomes.
    ///  * Practising skills permanently widens the window. This is the main progression.
    ///  * A skill used in the "wrong" direction still helps, just less.
    /// </summary>
    public sealed class MindModel
    {
        public const float MinWindowHalfWidth = 0.2f;
        public const float MaxWindowHalfWidth = 0.75f;

        /// <summary>How strongly a mismatched skill still helps (e.g. slow breathing during shutdown).</summary>
        public const float MismatchedSkillEffectiveness = 0.35f;

        static readonly int ExperienceCount = Enum.GetValues(typeof(DissociativeExperience)).Length;

        readonly float[] _scripted = new float[ExperienceCount];

        /// <summary>-1 (deep shutdown) .. 0 (settled) .. +1 (panic).</summary>
        public float Arousal { get; private set; }

        /// <summary>Half-width of the window of tolerance around 0. It grows with practice.</summary>
        public float WindowHalfWidth { get; private set; }

        /// <summary>How quickly arousal settles back toward 0 by itself, per second.</summary>
        public float SettleRate { get; set; } = 0.02f;

        /// <summary>How quickly scripted experiences fade by themselves, per second, once the story stops holding them.</summary>
        public float ExperienceFadeRate { get; set; } = 0.01f;

        public event Action<MindModel> Changed;

        public MindModel(float initialWindowHalfWidth = 0.3f)
        {
            WindowHalfWidth = Clamp(initialWindowHalfWidth, MinWindowHalfWidth, MaxWindowHalfWidth);
        }

        public ArousalZone Zone =>
            Arousal < -WindowHalfWidth ? ArousalZone.Hypoaroused :
            Arousal > WindowHalfWidth ? ArousalZone.Hyperaroused :
            ArousalZone.WithinWindow;

        /// <summary>0 inside the window, rising to 1 at the extreme of either side.</summary>
        public float DistanceOutsideWindow
        {
            get
            {
                float over = Math.Abs(Arousal) - WindowHalfWidth;
                if (over <= 0f) return 0f;
                return Clamp01(over / (1f - WindowHalfWidth));
            }
        }

        /// <summary>
        /// Current intensity of an experience, 0..1. Fog is derived from hypoarousal as well as
        /// any scripted fog. All other experiences are set by the narrative.
        /// </summary>
        public float GetIntensity(DissociativeExperience experience)
        {
            float scripted = _scripted[(int)experience];
            if (experience == DissociativeExperience.Fog && Zone == ArousalZone.Hypoaroused)
                return Math.Max(scripted, DistanceOutsideWindow);
            return scripted;
        }

        /// <summary>1 = fully present. This is what the presentation layer reads for overall "clarity".</summary>
        public float Presence
        {
            get
            {
                float strongest = DistanceOutsideWindow;
                for (int i = 0; i < ExperienceCount; i++)
                {
                    // Co-consciousness isn't a loss of presence; it's shared presence.
                    if (i == (int)DissociativeExperience.CoConsciousness) continue;
                    strongest = Math.Max(strongest, GetIntensity((DissociativeExperience)i));
                }
                return 1f - strongest;
            }
        }

        /// <summary>A stressor or soothing event moves arousal. Positive raises it, negative lowers it.</summary>
        public void Nudge(float delta)
        {
            Arousal = Clamp(Arousal + delta, -1f, 1f);
            Changed?.Invoke(this);
        }

        /// <summary>Called by the narrative to begin, deepen or end a dissociative experience.</summary>
        public void SetExperience(DissociativeExperience experience, float intensity)
        {
            _scripted[(int)experience] = Clamp01(intensity);
            Changed?.Invoke(this);
        }

        /// <summary>Applies a practised skill: it eases experiences and moves arousal toward the window.</summary>
        public void ApplyGrounding(GroundingOutcome outcome)
        {
            float completion = Clamp01(outcome.Completion);
            if (completion <= 0f) return;

            float effectiveness = DirectionEffectiveness(outcome.Direction) * completion;

            // Move arousal toward 0 without overshooting.
            float pull = 0.6f * effectiveness;
            Arousal = Math.Abs(Arousal) <= pull ? 0f : Arousal - Math.Sign(Arousal) * pull;

            for (int i = 0; i < ExperienceCount; i++)
            {
                var exp = (DissociativeExperience)i;
                if (exp == DissociativeExperience.CoConsciousness) continue; // Not something to "fix".
                if (outcome.Eases != null && outcome.Eases.Length > 0 && Array.IndexOf(outcome.Eases, exp) < 0) continue;
                _scripted[i] = Clamp01(_scripted[i] - 0.5f * effectiveness);
            }

            Changed?.Invoke(this);
        }

        /// <summary>
        /// Practice widens the window permanently. Call after completing a skill.
        /// Gains shrink as the window approaches its maximum.
        /// </summary>
        public void RecordPractice(float amount = 0.01f)
        {
            float headroom = (MaxWindowHalfWidth - WindowHalfWidth) / (MaxWindowHalfWidth - MinWindowHalfWidth);
            WindowHalfWidth = Clamp(WindowHalfWidth + amount * headroom, MinWindowHalfWidth, MaxWindowHalfWidth);
            Changed?.Invoke(this);
        }

        /// <summary>Natural settling over time.</summary>
        public void Tick(float deltaTime, bool holdScriptedExperiences)
        {
            if (deltaTime <= 0f) return;
            bool changed = false;

            float settle = SettleRate * deltaTime;
            if (Arousal != 0f)
            {
                Arousal = Math.Abs(Arousal) <= settle ? 0f : Arousal - Math.Sign(Arousal) * settle;
                changed = true;
            }

            if (!holdScriptedExperiences)
            {
                float fade = ExperienceFadeRate * deltaTime;
                for (int i = 0; i < ExperienceCount; i++)
                {
                    if (_scripted[i] <= 0f) continue;
                    _scripted[i] = Math.Max(0f, _scripted[i] - fade);
                    changed = true;
                }
            }

            if (changed) Changed?.Invoke(this);
        }

        /// <summary>Restores a saved state.</summary>
        public void Restore(float arousal, float windowHalfWidth)
        {
            Arousal = Clamp(arousal, -1f, 1f);
            WindowHalfWidth = Clamp(windowHalfWidth, MinWindowHalfWidth, MaxWindowHalfWidth);
            Array.Clear(_scripted, 0, _scripted.Length);
            Changed?.Invoke(this);
        }

        float DirectionEffectiveness(SkillDirection direction)
        {
            // Inside the window, any skill is just practice. Direction only matters at the edges.
            if (Zone == ArousalZone.WithinWindow) return 1f;
            switch (direction)
            {
                case SkillDirection.Either: return 1f;
                case SkillDirection.LowersArousal: return Arousal > 0f ? 1f : MismatchedSkillEffectiveness;
                case SkillDirection.RaisesArousal: return Arousal < 0f ? 1f : MismatchedSkillEffectiveness;
                default: return 1f;
            }
        }

        static float Clamp(float v, float min, float max) => v < min ? min : v > max ? max : v;
        static float Clamp01(float v) => Clamp(v, 0f, 1f);
    }
}
