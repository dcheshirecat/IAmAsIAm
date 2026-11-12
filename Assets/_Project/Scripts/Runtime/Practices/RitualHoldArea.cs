using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IAmAsIAm.Runtime.Practices
{
    /// <summary>A ritual's "hold" button: tells the ritual screen while it's pressed (letting go early only pauses).</summary>
    public sealed class RitualHoldArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public Action<bool> Changed;
        public void OnPointerDown(PointerEventData e) => Changed?.Invoke(true);
        public void OnPointerUp(PointerEventData e) => Changed?.Invoke(false);
        public void OnPointerExit(PointerEventData e) => Changed?.Invoke(false);
    }
}
