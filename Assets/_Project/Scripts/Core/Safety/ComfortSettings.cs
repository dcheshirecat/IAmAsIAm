using System;
using System.Collections.Generic;

namespace IAmAsIAm.Core.Safety
{
    /// <summary>
    /// Content tags shown as content notes before chapters and scenes.
    /// See ContentAndSafetyGuidelines.md §3. Add new tags here and in the content-note UI table.
    /// </summary>
    public static class ContentTags
    {
        public const string TimeLoss = "time-loss";
        public const string Depersonalization = "depersonalization";
        public const string Derealization = "derealization";
        public const string InvoluntarySwitch = "involuntary-switch";
        public const string TraumaReference = "trauma-reference";
        public const string ChildhoodReference = "childhood-reference";
        public const string MedicalSetting = "medical-setting";
        public const string RaisedVoices = "raised-voices";
        public const string SelfHarmReference = "self-harm-reference";
        public const string Panic = "panic";
        /// <summary>Scenes exploring religion with the teacher. Skippable for anyone with religious trauma.</summary>
        public const string ReligiousContent = "religious-content";

        public static readonly IReadOnlyList<string> All = new[]
        {
            TimeLoss, Depersonalization, Derealization, InvoluntarySwitch, TraumaReference,
            ChildhoodReference, MedicalSetting, RaisedVoices, SelfHarmReference, Panic, ReligiousContent,
        };
    }

    /// <summary>
    /// Player comfort options. Every presentation system must read these before applying effects.
    /// Defaults follow ContentAndSafetyGuidelines.md §6.
    /// </summary>
    [Serializable]
    public sealed class ComfortSettings
    {
        /// <summary>0..1 cap applied to every dissociation post-process and distortion.</summary>
        public float maxVisualDistortion = 1f;
        public bool disableCameraDrift;
        public bool disableAudioDistortion;
        public bool reduceMotion;
        /// <summary>When true, story-driven switches pause and ask first (and offer to skip with a summary).</summary>
        public bool askBeforeInvoluntarySwitches;
        public bool showContentNotes = true;
        /// <summary>Tags the player has chosen to skip. Skipped scenes are replaced with a short summary.</summary>
        public List<string> skippedTags = new List<string>();
        public bool allowTarotReversals;
        public bool allowBreathHolds;
        /// <summary>Slow fade instead of a hard cut on time loss.</summary>
        public bool softTimeLossTransitions;
        /// <summary>Characters per second. 0 means instant.</summary>
        public float textSpeed = 40f;

        public bool ShouldSkip(IEnumerable<string> sceneTags)
        {
            if (sceneTags == null) return false;
            foreach (var t in sceneTags) if (skippedTags.Contains(t)) return true;
            return false;
        }

        /// <summary>Applies the distortion cap. Every effect intensity should pass through this.</summary>
        public float CapDistortion(float intensity)
        {
            float cap = maxVisualDistortion < 0f ? 0f : maxVisualDistortion > 1f ? 1f : maxVisualDistortion;
            if (reduceMotion) cap *= 0.5f;
            return Math.Min(intensity, cap);
        }
    }

    [Serializable]
    public sealed class CrisisResource
    {
        /// <summary>ISO 3166 country/region code, or "INTL".</summary>
        public string region;
        public string name;
        public string contact;
        public string url;
        public string notes;
    }

    [Serializable]
    public sealed class CrisisResourceData
    {
        public string lastVerified;
        public string disclaimer;
        public List<CrisisResource> resources = new List<CrisisResource>();
    }
}
