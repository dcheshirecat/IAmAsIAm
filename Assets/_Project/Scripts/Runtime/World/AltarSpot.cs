using System;
using IAmAsIAm.Runtime.Interaction;
using UnityEngine;

namespace IAmAsIAm.Runtime.World
{
    /// <summary>
    /// Maya's altar at home (owner: an altar in every home she lives in, usable whenever she's there). It doesn't glow:
    /// the glow is the story pointing the way, and the altar is just always there. Walk up and Use.
    /// </summary>
    public sealed class AltarSpot : MonoBehaviour, IInteractable
    {
        public Func<bool> Usable = () => false;
        public Action Used;

        public string Prompt => "Use: Your altar";
        public bool CanInteract => Usable != null && Usable();
        public void Interact() => Used?.Invoke();
    }
}
