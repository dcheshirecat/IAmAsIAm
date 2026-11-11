using System;
using System.Collections.Generic;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Safety;
using UnityEngine;
using UnityEngine.Events;

namespace IAmAsIAm.Runtime.PluralSystem
{
    /// <summary>
    /// Per-member embodiment: how the world feels when this member is fronting.
    /// Author one per playable member and match it to the member id from system_members.json.
    /// </summary>
    [Serializable]
    public sealed class MemberEmbodiment
    {
        public string memberId;
        [Tooltip("Eye height in metres. A little part sees the world from lower down.")]
        public float eyeHeight = 1.65f;
        public float walkSpeed = 2.2f;
        [Tooltip("Optional visual (hands, sleeves) shown in first person.")]
        public GameObject firstPersonVisual;
        [Tooltip("Optional footstep, breath and foley set for this member.")]
        public AudioClip[] footsteps;
    }

    /// <summary>
    /// Applies switches to the player. Voluntary switches happen right away. Involuntary
    /// (story) switches go through time loss, and pause to ask first if the player has turned
    /// on "Ask before involuntary switches".
    /// </summary>
    public sealed class FrontingController : MonoBehaviour
    {
        [SerializeField] Transform eyeAnchor;
        [SerializeField] List<MemberEmbodiment> embodiments = new List<MemberEmbodiment>();

        [Header("Events (hook up UI, audio, timeline)")]
        public UnityEvent<string> onFrontChanged;
        /// <summary>Raised for an involuntary switch when the comfort setting asks first. UI must call
        /// <see cref="ConfirmPendingSwitch"/> or <see cref="SkipPendingSwitch"/>.</summary>
        public UnityEvent<string> onInvoluntarySwitchPending;
        /// <summary>Raised when time loss begins. A scene transition system cuts or fades here.</summary>
        public UnityEvent<bool> onTimeLossBegins;

        string _pendingId;

        public MemberEmbodiment Current { get; private set; }
        public float WalkSpeed => Current?.walkSpeed ?? 2.2f;

        void OnEnable()
        {
            if (GameServices.IsReady) Hook();
            else GameServices.Ready += Hook;
        }

        void OnDisable()
        {
            GameServices.Ready -= Hook;
            if (GameServices.Roster != null) GameServices.Roster.FrontChanged -= OnFrontChanged;
        }

        void Hook()
        {
            GameServices.Ready -= Hook;
            GameServices.Roster.FrontChanged += OnFrontChanged;
            Apply(GameServices.Roster.FrontingId);
        }

        /// <summary>
        /// Story-driven switch entry point (called from dialogue or Timeline).
        /// </summary>
        public void StoryForcesSwitch(string memberId)
        {
            if (GameServices.Comfort.askBeforeInvoluntarySwitches)
            {
                _pendingId = memberId;
                onInvoluntarySwitchPending?.Invoke(memberId);
                return;
            }
            DoInvoluntarySwitch(memberId);
        }

        public void ConfirmPendingSwitch()
        {
            if (_pendingId == null) return;
            DoInvoluntarySwitch(_pendingId);
            _pendingId = null;
        }

        /// <summary>The player chose to skip. Narrative should show a short summary instead.</summary>
        public void SkipPendingSwitch() => _pendingId = null;

        void DoInvoluntarySwitch(string memberId)
        {
            onTimeLossBegins?.Invoke(GameServices.Comfort.softTimeLossTransitions);
            GameServices.Mind.SetExperience(DissociativeExperience.TimeLoss, 1f);
            GameServices.Roster.ForceSwitch(memberId);
        }

        void OnFrontChanged(FrontChange change) => Apply(change.NewId);

        void Apply(string memberId)
        {
            foreach (var e in embodiments)
                if (e.firstPersonVisual != null) e.firstPersonVisual.SetActive(false);

            Current = embodiments.Find(e => e.memberId == memberId);
            if (Current != null)
            {
                if (eyeAnchor != null)
                {
                    var p = eyeAnchor.localPosition;
                    eyeAnchor.localPosition = new Vector3(p.x, Current.eyeHeight, p.z);
                }
                if (Current.firstPersonVisual != null) Current.firstPersonVisual.SetActive(true);
            }

            onFrontChanged?.Invoke(memberId);
        }
    }
}
