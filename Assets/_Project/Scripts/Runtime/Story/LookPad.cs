using IAmAsIAm.Runtime.CameraRig;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IAmAsIAm.Runtime.Story
{
    /// <summary>Drag on the right of a touch screen to orbit the shoulder camera.</summary>
    public sealed class LookPad : MonoBehaviour, IDragHandler
    {
        public ShoulderCamera cameraRig;

        public void OnDrag(PointerEventData eventData)
        {
            if (cameraRig != null) cameraRig.AddLook(eventData.delta);
        }
    }
}
