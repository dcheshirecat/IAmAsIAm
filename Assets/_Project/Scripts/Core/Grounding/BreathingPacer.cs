using System;

namespace IAmAsIAm.Core.Grounding
{
    public enum BreathPhase { Inhale, HoldIn, Exhale, HoldOut }

    /// <summary>
    /// A breathing rhythm. Phases with zero duration are skipped.
    /// See TherapeuticSkillsReference.md §3.
    /// </summary>
    [Serializable]
    public struct BreathingPattern
    {
        public string id;
        public string displayName;
        public float inhaleSeconds;
        public float holdInSeconds;
        public float exhaleSeconds;
        public float holdOutSeconds;

        public bool HasHolds => holdInSeconds > 0f || holdOutSeconds > 0f;
        public float CycleSeconds => inhaleSeconds + holdInSeconds + exhaleSeconds + holdOutSeconds;
        public float BreathsPerMinute => CycleSeconds <= 0f ? 0f : 60f / CycleSeconds;

        public BreathingPattern(string id, string displayName, float inhale, float holdIn, float exhale, float holdOut)
        {
            this.id = id;
            this.displayName = displayName;
            inhaleSeconds = inhale;
            holdInSeconds = holdIn;
            exhaleSeconds = exhale;
            holdOutSeconds = holdOut;
        }

        /// <summary>DBT TIPP "paced breathing": exhale longer than inhale, no holds. The game's default.</summary>
        public static BreathingPattern Paced => new BreathingPattern("paced", "Paced breathing", 4f, 0f, 6f, 0f);

        /// <summary>Box breathing. Opt-in only, because breath holds can distress some trauma survivors.</summary>
        public static BreathingPattern Box => new BreathingPattern("box", "Box breathing", 4f, 4f, 4f, 4f);

        /// <summary>A gentler on-ramp for players who find 4/6 too long.</summary>
        public static BreathingPattern Gentle => new BreathingPattern("gentle", "Gentle breathing", 3f, 0f, 4f, 0f);

        /// <summary>
        /// Returns a copy with holds removed if the player has disabled them (Comfort setting).
        /// Hold time is folded into the exhale so the rhythm stays slow.
        /// </summary>
        public BreathingPattern WithoutHolds()
        {
            if (!HasHolds) return this;
            return new BreathingPattern(id + "-noholds", displayName, inhaleSeconds, 0f,
                exhaleSeconds + holdOutSeconds, 0f);
        }
    }

    /// <summary>
    /// Pure timing logic for a breathing exercise. A view (UI circle, controller rumble, audio
    /// swell) reads <see cref="Phase"/> and <see cref="PhaseProgress"/> each frame.
    /// Ticks with unscaled time, so it works while the game is paused in the Anchor menu.
    /// </summary>
    public sealed class BreathingPacer
    {
        public BreathingPattern Pattern { get; }
        public BreathPhase Phase { get; private set; }
        public float PhaseElapsed { get; private set; }
        public int CompletedCycles { get; private set; }
        public int TargetCycles { get; }
        public bool IsComplete => TargetCycles > 0 && CompletedCycles >= TargetCycles;

        /// <summary>0..1 through the current phase.</summary>
        public float PhaseProgress
        {
            get
            {
                float d = Duration(Phase);
                return d <= 0f ? 1f : Math.Min(1f, PhaseElapsed / d);
            }
        }

        /// <summary>
        /// 0 (empty lungs) .. 1 (full). Handy for driving the scale of a breathing circle.
        /// Uses an ease-in-out curve so it feels like breath, not a metronome.
        /// </summary>
        public float Fullness
        {
            get
            {
                switch (Phase)
                {
                    case BreathPhase.Inhale: return Ease(PhaseProgress);
                    case BreathPhase.HoldIn: return 1f;
                    case BreathPhase.Exhale: return 1f - Ease(PhaseProgress);
                    default: return 0f;
                }
            }
        }

        /// <summary>0..1 overall completion, for <c>GroundingOutcome.Completion</c>.</summary>
        public float Completion => TargetCycles <= 0 ? 0f : Math.Min(1f, CompletedCycles / (float)TargetCycles);

        public event Action<BreathPhase> PhaseChanged;
        public event Action<int> CycleCompleted;

        /// <param name="targetCycles">0 means open-ended (Practice Mode).</param>
        public BreathingPacer(BreathingPattern pattern, int targetCycles = 6)
        {
            if (pattern.CycleSeconds <= 0f) throw new ArgumentException("Breathing pattern must have a positive duration.");
            Pattern = pattern;
            TargetCycles = Math.Max(0, targetCycles);
            Phase = BreathPhase.Inhale;
        }

        public void Tick(float deltaTime)
        {
            if (deltaTime <= 0f || IsComplete) return;
            PhaseElapsed += deltaTime;

            // Loop in case of a large delta (e.g. a frame hitch). Never skip a cycle count.
            while (PhaseElapsed >= Duration(Phase) && !IsComplete)
            {
                PhaseElapsed -= Duration(Phase);
                Advance();
            }
        }

        /// <summary>Moves to the next phase with a positive duration, counting a cycle on each wrap.</summary>
        void Advance()
        {
            int i = (int)Phase;
            for (int step = 0; step < 4; step++)
            {
                i++;
                if (i > (int)BreathPhase.HoldOut)
                {
                    i = 0;
                    CompletedCycles++;
                    CycleCompleted?.Invoke(CompletedCycles);
                    if (IsComplete)
                    {
                        Phase = BreathPhase.Inhale;
                        PhaseElapsed = 0f;
                        PhaseChanged?.Invoke(Phase);
                        return;
                    }
                }
                if (Duration((BreathPhase)i) > 0f) break;
            }

            Phase = (BreathPhase)i;
            PhaseChanged?.Invoke(Phase);
        }

        float Duration(BreathPhase phase)
        {
            switch (phase)
            {
                case BreathPhase.Inhale: return Pattern.inhaleSeconds;
                case BreathPhase.HoldIn: return Pattern.holdInSeconds;
                case BreathPhase.Exhale: return Pattern.exhaleSeconds;
                default: return Pattern.holdOutSeconds;
            }
        }

        static float Ease(float t) => t * t * (3f - 2f * t);
    }
}
