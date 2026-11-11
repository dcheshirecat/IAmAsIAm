using System;
using System.Collections.Generic;

namespace IAmAsIAm.Core.Skills
{
    /// <summary>Common cognitive distortions (Burns, 1980; Beck).</summary>
    public enum CognitiveDistortion
    {
        AllOrNothing,
        Overgeneralization,
        MentalFilter,
        DisqualifyingThePositive,
        MindReading,
        FortuneTelling,
        Catastrophizing,
        Minimization,
        EmotionalReasoning,
        ShouldStatements,
        Labeling,
        Personalization,
        Blame,
    }

    public static class CognitiveDistortionInfo
    {
        /// <summary>Plain-language descriptions with an example in the voice of the game's world.</summary>
        public static string Describe(CognitiveDistortion d)
        {
            switch (d)
            {
                case CognitiveDistortion.AllOrNothing: return "Seeing things in black and white. \"If I lost time today, I've ruined everything.\"";
                case CognitiveDistortion.Overgeneralization: return "One event becomes a pattern. \"I always mess this up.\"";
                case CognitiveDistortion.MentalFilter: return "Only noticing the bad details. Ten kind things fade behind one hard one.";
                case CognitiveDistortion.DisqualifyingThePositive: return "Good things \"don't count\". \"They only helped because they had to.\"";
                case CognitiveDistortion.MindReading: return "Assuming you know what others think. \"They think I'm faking.\"";
                case CognitiveDistortion.FortuneTelling: return "Predicting things will go badly. \"Therapy will never work for us.\"";
                case CognitiveDistortion.Catastrophizing: return "Expecting the worst possible outcome. \"If I switch at work, I'll lose everything.\"";
                case CognitiveDistortion.Minimization: return "Shrinking your own needs or progress. \"It wasn't that bad. I should be over it.\"";
                case CognitiveDistortion.EmotionalReasoning: return "Feeling it means it's true. \"I feel unsafe, so I must be in danger.\"";
                case CognitiveDistortion.ShouldStatements: return "Rigid rules about how you must be. \"I should be able to remember.\"";
                case CognitiveDistortion.Labeling: return "Turning one moment into an identity. \"I'm broken.\"";
                case CognitiveDistortion.Personalization: return "Taking responsibility for things outside your control.";
                case CognitiveDistortion.Blame: return "Placing all responsibility on yourself or on one other person.";
                default: return d.ToString();
            }
        }
    }

    /// <summary>
    /// A Beck-style thought record (CBT). Used as a journaling mechanic: the player fills it in,
    /// and the system's parts can contribute evidence. This is also a cooperation beat.
    ///
    /// Re-rating usually shows a modest drop. The game celebrates any shift, including none:
    /// "Noticing the thought is the skill."
    /// </summary>
    [Serializable]
    public sealed class ThoughtRecord
    {
        public string situation;
        public string automaticThought;
        public string emotion;
        /// <summary>0..100, as is standard in thought records.</summary>
        public int intensityBefore = -1;
        public List<CognitiveDistortion> distortions = new List<CognitiveDistortion>();
        public List<string> evidenceFor = new List<string>();
        public List<string> evidenceAgainst = new List<string>();
        /// <summary>Which parts contributed evidence (member ids).</summary>
        public List<string> contributorIds = new List<string>();
        public string balancedThought;
        public int intensityAfter = -1;

        public enum Step { Situation, AutomaticThought, Emotion, Distortions, Evidence, BalancedThought, Rerate, Done }

        /// <summary>The next step that still needs input. Distortions and evidence are encouraged but optional.</summary>
        public Step NextStep
        {
            get
            {
                if (string.IsNullOrWhiteSpace(situation)) return Step.Situation;
                if (string.IsNullOrWhiteSpace(automaticThought)) return Step.AutomaticThought;
                if (string.IsNullOrWhiteSpace(emotion) || intensityBefore < 0) return Step.Emotion;
                if (string.IsNullOrWhiteSpace(balancedThought)) return Step.BalancedThought;
                if (intensityAfter < 0) return Step.Rerate;
                return Step.Done;
            }
        }

        public bool IsComplete => NextStep == Step.Done;

        /// <summary>Positive means the emotion eased. Null until both ratings exist.</summary>
        public int? IntensityShift => intensityBefore >= 0 && intensityAfter >= 0 ? intensityBefore - intensityAfter : (int?)null;

        public void RateBefore(string emotionName, int intensity)
        {
            emotion = emotionName;
            intensityBefore = ClampRating(intensity);
        }

        public void RateAfter(int intensity) => intensityAfter = ClampRating(intensity);

        public void AddEvidence(bool supportsThought, string evidence, string contributorId = null)
        {
            if (string.IsNullOrWhiteSpace(evidence)) return;
            (supportsThought ? evidenceFor : evidenceAgainst).Add(evidence.Trim());
            if (contributorId != null && !contributorIds.Contains(contributorId)) contributorIds.Add(contributorId);
        }

        static int ClampRating(int v) => v < 0 ? 0 : v > 100 ? 100 : v;
    }
}
