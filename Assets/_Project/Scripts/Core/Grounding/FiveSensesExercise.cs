using System;
using System.Collections.Generic;
using IAmAsIAm.Core.Mind;

namespace IAmAsIAm.Core.Grounding
{
    public enum Sense { See, Touch, Hear, Smell, Taste }

    /// <summary>
    /// The 5-4-3-2-1 grounding exercise.
    ///
    /// It runs in two modes, both using this same model:
    ///  * In-story: the player finds objects in the scene tagged with a sense. Each one lifts some fog.
    ///  * Practice Mode: the game prompts the player to notice real things around them and simply
    ///    confirm when they have. Nothing is recorded but the count.
    ///
    /// Senses can be swapped or skipped. Not everyone can smell or taste on demand, and that's fine.
    /// </summary>
    public sealed class FiveSensesExercise
    {
        public static readonly IReadOnlyList<(Sense sense, int count)> DefaultSteps = new[]
        {
            (Sense.See, 5), (Sense.Touch, 4), (Sense.Hear, 3), (Sense.Smell, 2), (Sense.Taste, 1),
        };

        readonly List<(Sense sense, int count)> _steps;
        readonly HashSet<string> _noticedIds = new HashSet<string>();

        public int StepIndex { get; private set; }
        public int NoticedInStep { get; private set; }
        public bool IsComplete => StepIndex >= _steps.Count;

        public Sense CurrentSense => IsComplete ? _steps[_steps.Count - 1].sense : _steps[StepIndex].sense;
        public int CurrentTarget => IsComplete ? 0 : _steps[StepIndex].count;

        public int TotalTarget { get; }
        public int TotalNoticed { get; private set; }
        public float Completion => TotalTarget == 0 ? 1f : (float)TotalNoticed / TotalTarget;

        public event Action<Sense, int> Noticed;
        public event Action<Sense> StepStarted;
        public event Action Completed;

        public FiveSensesExercise(IEnumerable<(Sense sense, int count)> steps = null)
        {
            _steps = new List<(Sense, int)>(steps ?? DefaultSteps);
            foreach (var s in _steps) TotalTarget += s.count;
        }

        /// <summary>
        /// Registers a noticed thing for the current sense.
        /// </summary>
        /// <param name="sense">The sense of the noticed thing. It must match the current step.</param>
        /// <param name="uniqueId">Scene object id. Pass null in Practice Mode (real-world items).</param>
        /// <returns>True if it counted.</returns>
        public bool Notice(Sense sense, string uniqueId = null)
        {
            if (IsComplete || sense != CurrentSense) return false;
            if (uniqueId != null && !_noticedIds.Add(uniqueId)) return false;

            NoticedInStep++;
            TotalNoticed++;
            Noticed?.Invoke(sense, NoticedInStep);

            if (NoticedInStep >= CurrentTarget) NextStep();
            return true;
        }

        /// <summary>Skip a sense that isn't available right now. The remaining count isn't held against anyone.</summary>
        public void SkipCurrentSense()
        {
            if (IsComplete) return;
            // Credit the skipped items so completion reflects "done with what I could".
            TotalNoticed += CurrentTarget - NoticedInStep;
            NextStep();
        }

        void NextStep()
        {
            StepIndex++;
            NoticedInStep = 0;
            if (IsComplete) Completed?.Invoke();
            else StepStarted?.Invoke(CurrentSense);
        }

        public GroundingOutcome ToOutcome() => new GroundingOutcome(
            SkillDirection.Either, Completion,
            DissociativeExperience.Derealization,
            DissociativeExperience.Fog,
            DissociativeExperience.Depersonalization);
    }
}
