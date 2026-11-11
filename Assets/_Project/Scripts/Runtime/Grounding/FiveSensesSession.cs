using IAmAsIAm.Core.Grounding;
using UnityEngine;
using UnityEngine.Events;

namespace IAmAsIAm.Runtime.Grounding
{
    /// <summary>
    /// Runs a 5-4-3-2-1 exercise in the scene. While it's active, every <see cref="SenseAnchor"/>
    /// that matches the current sense becomes interactable, and each one noticed lifts the fog a little.
    /// </summary>
    public sealed class FiveSensesSession : MonoBehaviour
    {
        public static FiveSensesSession Active { get; private set; }

        [Tooltip("How much each noticed item eases the mind, as a fraction of a full completion.")]
        [SerializeField, Range(0f, 0.2f)] float easePerItem = 0.06f;

        public UnityEvent<string> onPrompt;
        public UnityEvent onComplete;

        FiveSensesExercise _exercise;

        public Sense CurrentSense => _exercise?.CurrentSense ?? Sense.See;
        public bool IsRunning => _exercise != null && !_exercise.IsComplete;

        public void Begin()
        {
            _exercise = new FiveSensesExercise();
            _exercise.StepStarted += s => onPrompt?.Invoke(PromptFor(s, _exercise.CurrentTarget));
            _exercise.Noticed += OnNoticed;
            _exercise.Completed += OnCompleted;
            Active = this;
            onPrompt?.Invoke(PromptFor(_exercise.CurrentSense, _exercise.CurrentTarget));
        }

        public bool TryNotice(Sense sense, string id) => _exercise != null && _exercise.Notice(sense, id);

        public void SkipSense() => _exercise?.SkipCurrentSense();

        void OnNoticed(Sense sense, int count)
        {
            var partial = _exercise.ToOutcome();
            partial.Completion = easePerItem;
            GameServices.Mind?.ApplyGrounding(partial);
        }

        void OnCompleted()
        {
            GameServices.Skills?.RecordPractice("five-senses");
            GameServices.Mind?.RecordPractice();
            Active = null;
            onComplete?.Invoke();
        }

        void OnDisable()
        {
            if (Active == this) Active = null;
        }

        static string PromptFor(Sense s, int n) => s switch
        {
            Sense.See => $"Find {n} things you can see.",
            Sense.Touch => $"Find {n} things you can touch.",
            Sense.Hear => $"Listen for {n} sounds.",
            Sense.Smell => $"Find {n} things you can smell.",
            _ => $"Notice {n} thing you can taste.",
        };
    }
}
