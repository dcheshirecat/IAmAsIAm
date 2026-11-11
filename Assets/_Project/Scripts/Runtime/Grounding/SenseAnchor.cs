using System;
using IAmAsIAm.Core.Grounding;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Runtime.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace IAmAsIAm.Runtime.Grounding
{
    /// <summary>
    /// A scene object that can be noticed with one or more senses. Optionally linked to a
    /// practice item (herb, stone, charm) whose sensory description is shown.
    /// </summary>
    public sealed class SenseAnchor : MonoBehaviour, IInteractable
    {
        [SerializeField] string anchorId;
        [SerializeField] Sense[] senses = { Sense.See };
        [Tooltip("Optional: id of a herb, crystal or charm. Its sensory text is shown when noticed.")]
        [SerializeField] string practiceItemId;
        [SerializeField, TextArea] string fallbackDescription;

        public UnityEvent<string> onDescribed;

        public string Prompt
        {
            get
            {
                var s = FiveSensesSession.Active?.CurrentSense ?? Sense.See;
                return s switch
                {
                    Sense.Touch => "Touch",
                    Sense.Hear => "Listen",
                    Sense.Smell => "Smell",
                    Sense.Taste => "Taste",
                    _ => "Look",
                };
            }
        }

        public bool CanInteract
        {
            get
            {
                var session = FiveSensesSession.Active;
                return session != null && session.IsRunning && Array.IndexOf(senses, session.CurrentSense) >= 0;
            }
        }

        public void Interact()
        {
            var session = FiveSensesSession.Active;
            if (session == null) return;
            if (!session.TryNotice(session.CurrentSense, string.IsNullOrEmpty(anchorId) ? GetInstanceID().ToString() : anchorId)) return;

            ISensoryAnchor item = string.IsNullOrEmpty(practiceItemId) ? null : GameServices.Practices?.GetAnchor(practiceItemId);
            onDescribed?.Invoke(item != null ? item.SensoryDescription : fallbackDescription);
        }

        void OnValidate()
        {
            if (string.IsNullOrEmpty(anchorId)) anchorId = gameObject.name;
        }
    }
}
