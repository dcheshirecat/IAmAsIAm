using UnityEngine;
using UnityEngine.InputSystem;

namespace IAmAsIAm.Runtime.Interaction
{
    /// <summary>
    /// A gaze or reticle interactor. Works with mouse and gamepad, because it raycasts from the
    /// camera centre. Exposes the current target for a prompt UI.
    /// </summary>
    public sealed class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] Camera viewCamera;
        [SerializeField] InputActionReference interactAction;
        [SerializeField] float reach = 2.2f;
        [SerializeField] LayerMask mask = ~0;

        public IInteractable Target { get; private set; }

        void OnEnable()
        {
            if (interactAction == null) return;
            interactAction.action.performed += OnInteract;
            interactAction.action.Enable();
        }

        void OnDisable()
        {
            if (interactAction != null) interactAction.action.performed -= OnInteract;
        }

        void Update()
        {
            Target = null;
            if (viewCamera == null) return;
            var ray = new Ray(viewCamera.transform.position, viewCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, reach, mask, QueryTriggerInteraction.Collide))
            {
                var candidate = hit.collider.GetComponentInParent<IInteractable>();
                if (candidate != null && candidate.CanInteract) Target = candidate;
            }
        }

        void OnInteract(InputAction.CallbackContext _)
        {
            if (Time.timeScale > 0f) Target?.Interact();
        }
    }
}
