using IAmAsIAm.Runtime.Interaction;
using UnityEngine;

namespace IAmAsIAm.Runtime.Story
{
    /// <summary>
    /// Something in the world that plays an Ink knot when used: a person to talk to, an object to look at,
    /// an altar to draw a card at. For people, the camera frames the conversation.
    /// </summary>
    public sealed class StoryInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] string prompt = "Look";
        [Tooltip("The Ink knot (or knot.stitch) to play.")]
        [SerializeField] string knot;
        [Tooltip("For people: the camera frames this conversation, and they turn to face the character.")]
        [SerializeField] bool isPerson;
        [SerializeField] bool onceOnly;

        bool _used;

        public string Prompt => prompt;
        public bool CanInteract => !(onceOnly && _used) && !string.IsNullOrEmpty(knot);

        public void Interact()
        {
            var runner = FindAnyObjectByType<StoryRunner>();
            if (runner == null) return;
            _used = true;
            runner.Play(knot, isPerson ? transform : null);
        }
    }
}
