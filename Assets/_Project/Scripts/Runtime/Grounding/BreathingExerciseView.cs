using IAmAsIAm.Core.Grounding;
using IAmAsIAm.Core.Mind;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace IAmAsIAm.Runtime.Grounding
{
    /// <summary>
    /// A breathing guide: a softly scaling circle or lantern, a phase label, and an optional gentle
    /// controller rumble on the inhale, which gives a non-visual cue.
    /// Runs on unscaled time, so it works inside the paused Anchor menu.
    /// </summary>
    public sealed class BreathingExerciseView : MonoBehaviour
    {
        public enum PatternChoice { Paced, Gentle, Box }

        [SerializeField] PatternChoice pattern = PatternChoice.Paced;
        [Tooltip("0 = open-ended (Practice Mode / Anchor menu).")]
        [SerializeField] int targetCycles = 6;
        [SerializeField] Transform breathShape;
        [SerializeField] Vector2 scaleRange = new Vector2(0.6f, 1.15f);
        [SerializeField] TMP_Text phaseLabel;
        [SerializeField] bool hapticCue = true;
        [SerializeField, Range(0f, 0.3f)] float hapticStrength = 0.08f;
        [Tooltip("Apply the result to the mind model when done (off in Practice Mode).")]
        [SerializeField] bool affectsStory = true;

        public UnityEvent onComplete;

        BreathingPacer _pacer;

        public void Begin()
        {
            var p = pattern switch
            {
                PatternChoice.Box => BreathingPattern.Box,
                PatternChoice.Gentle => BreathingPattern.Gentle,
                _ => BreathingPattern.Paced,
            };
            if (!GameServices.Comfort.allowBreathHolds) p = p.WithoutHolds();

            _pacer = new BreathingPacer(p, targetCycles);
            _pacer.PhaseChanged += UpdateLabel;
            UpdateLabel(_pacer.Phase);
            enabled = true;
        }

        public void Stop()
        {
            StopHaptics();
            if (_pacer != null && _pacer.CompletedCycles > 0) Finish();
            _pacer = null;
        }

        void OnEnable()
        {
            if (_pacer == null) Begin();
        }

        void OnDisable() => StopHaptics();

        void Update()
        {
            if (_pacer == null) return;
            _pacer.Tick(Time.unscaledDeltaTime);

            if (breathShape != null)
                breathShape.localScale = Vector3.one * Mathf.Lerp(scaleRange.x, scaleRange.y, _pacer.Fullness);

            if (hapticCue && Gamepad.current != null)
            {
                float rumble = _pacer.Phase == BreathPhase.Inhale ? hapticStrength * _pacer.PhaseProgress : 0f;
                Gamepad.current.SetMotorSpeeds(rumble * 0.5f, rumble);
            }

            if (_pacer.IsComplete)
            {
                Finish();
                _pacer = null;
                enabled = false;
            }
        }

        void Finish()
        {
            StopHaptics();
            string skillId = pattern == PatternChoice.Box ? "box-breathing" : "paced-breathing";
            GameServices.Skills?.RecordPractice(skillId);
            if (affectsStory && GameServices.Mind != null)
            {
                GameServices.Mind.ApplyGrounding(new GroundingOutcome(SkillDirection.LowersArousal, _pacer.Completion));
                GameServices.Mind.RecordPractice();
            }
            onComplete?.Invoke();
        }

        void UpdateLabel(BreathPhase phase)
        {
            if (phaseLabel == null) return;
            phaseLabel.text = phase switch
            {
                BreathPhase.Inhale => "Breathe in…",
                BreathPhase.HoldIn => "Hold gently…",
                BreathPhase.Exhale => "And slowly out…",
                _ => "Rest…",
            };
        }

        static void StopHaptics() => Gamepad.current?.SetMotorSpeeds(0f, 0f);
    }
}
