using System;

namespace IAmAsIAm.Core.Mind
{
    /// <summary>
    /// Where the system currently sits relative to its window of tolerance (Siegel, 1999).
    /// </summary>
    public enum ArousalZone
    {
        /// <summary>Numb, foggy, far away, shutdown.</summary>
        Hypoaroused,
        /// <summary>Present: able to think and feel at the same time.</summary>
        WithinWindow,
        /// <summary>Panic, racing thoughts, fight/flight.</summary>
        Hyperaroused,
    }

    /// <summary>
    /// The dissociative experiences the game represents. Several can be present at once.
    /// See GameDesignDocument.md §3 for how each one is represented.
    /// </summary>
    public enum DissociativeExperience
    {
        /// <summary>General hypoarousal haze. Usually derived from arousal, not scripted.</summary>
        Fog = 0,
        /// <summary>Feeling outside one's body, robotic or unreal to oneself. Represented symbolically.</summary>
        Depersonalization = 1,
        /// <summary>The world feels unreal, flat or dreamlike. Represented symbolically.</summary>
        Derealization = 2,
        /// <summary>Another part's perception or feelings bleeding through. Represented through characters.</summary>
        CoConsciousness = 3,
        /// <summary>Time that cannot be accounted for. Recovered gently through clues.</summary>
        TimeLoss = 4,
    }

    /// <summary>Which arousal direction a grounding skill is best suited to.</summary>
    public enum SkillDirection
    {
        /// <summary>Activating/orienting: best for numbness and shutdown.</summary>
        RaisesArousal,
        /// <summary>Calming/settling: best for panic and overwhelm.</summary>
        LowersArousal,
        /// <summary>Works in either direction (e.g. 5-4-3-2-1, orienting to the present).</summary>
        Either,
    }

    /// <summary>The result of practising a grounding skill, to be applied to a <see cref="MindModel"/>.</summary>
    [Serializable]
    public struct GroundingOutcome
    {
        public SkillDirection Direction;

        /// <summary>0..1: how fully the skill was completed.</summary>
        public float Completion;

        /// <summary>Which experiences this skill specifically helps with. Null means all.</summary>
        public DissociativeExperience[] Eases;

        public GroundingOutcome(SkillDirection direction, float completion, params DissociativeExperience[] eases)
        {
            Direction = direction;
            Completion = completion;
            Eases = eases;
        }
    }
}
