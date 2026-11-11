using UnityEngine;

namespace IAmAsIAm.Runtime.Mind
{
    /// <summary>
    /// Depersonalization: the camera drifts out of the body, from first person to over the
    /// shoulder to a distant observer. Grounding pulls it back in.
    ///
    /// The camera is a child of the player. Author two empty transforms: one at eye height
    /// (first person) and one behind, above and to the side (the "observer"). The rig blends
    /// along a curve between them. Disabled entirely by the "Disable camera drift" comfort setting.
    /// </summary>
    public sealed class DepersonalizationCameraRig : MonoBehaviour
    {
        [SerializeField] DissociationPresentationDriver driver;
        [SerializeField] Transform cameraTransform;
        [SerializeField] Transform firstPersonAnchor;
        [SerializeField] Transform observerAnchor;
        [Tooltip("How intensity maps to distance. Start gently; the drift should be felt before it's seen.")]
        [SerializeField] AnimationCurve driftCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [Tooltip("At full drift, the player's own body becomes visible on this layer (the 'watching yourself' effect).")]
        [SerializeField] GameObject playerBodyVisual;
        [SerializeField, Range(0f, 1f)] float showBodyAbove = 0.25f;

        void LateUpdate()
        {
            if (driver == null || cameraTransform == null || firstPersonAnchor == null || observerAnchor == null) return;

            float t = GameServices.Comfort.disableCameraDrift ? 0f : driftCurve.Evaluate(driver.Depersonalization);
            cameraTransform.SetPositionAndRotation(
                Vector3.Lerp(firstPersonAnchor.position, observerAnchor.position, t),
                Quaternion.Slerp(firstPersonAnchor.rotation, observerAnchor.rotation, t));

            if (playerBodyVisual != null) playerBodyVisual.SetActive(t > showBodyAbove);
        }
    }
}
