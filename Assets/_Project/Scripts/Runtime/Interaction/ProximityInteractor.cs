using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IAmAsIAm.Runtime.Interaction
{
    /// <summary>
    /// Third-person interaction: the nearest interactable in front of the character, within reach,
    /// becomes the target. E / Enter / gamepad A (or the on-screen Use button) uses it.
    /// The story screen shows the prompt ("Talk", "Look", "Draw a card").
    /// </summary>
    public sealed class ProximityInteractor : MonoBehaviour
    {
        [SerializeField] float reach = 1.8f;
        [Tooltip("How far around the facing direction counts as 'in front' (degrees either side).")]
        [SerializeField] float frontAngle = 75f;

        InputAction _use;
        readonly Collider[] _hits = new Collider[16];

        public IInteractable Target { get; private set; }
        public Component TargetComponent { get; private set; }
        public event Action<IInteractable> Used;

        void Awake()
        {
            _use = new InputAction("Use", binding: "<Keyboard>/e");
            _use.AddBinding("<Gamepad>/buttonSouth");
            _use.performed += _ => Use();
        }

        void OnEnable() => _use.Enable();
        void OnDisable() => _use.Disable();

        void Update()
        {
            Target = null;
            TargetComponent = null;
            if (Player.PlayerMotor.Blocked) return;

            int n = Physics.OverlapSphereNonAlloc(transform.position + Vector3.up, reach, _hits, ~0, QueryTriggerInteraction.Collide);
            float best = float.MaxValue;
            for (int i = 0; i < n; i++)
            {
                var candidate = _hits[i].GetComponentInParent<IInteractable>();
                if (candidate == null || !candidate.CanInteract) continue;
                var comp = (Component)candidate;
                Vector3 to = comp.transform.position - transform.position;
                to.y = 0f;
                if (to.sqrMagnitude > 0.01f && Vector3.Angle(transform.forward, to) > frontAngle) continue;
                if (to.sqrMagnitude < best) { best = to.sqrMagnitude; Target = candidate; TargetComponent = comp; }
            }
        }

        /// <summary>Also called by the on-screen Use button.</summary>
        public void Use()
        {
            if (Player.PlayerMotor.Blocked || Time.timeScale <= 0f || Target == null) return;
            var t = Target;
            t.Interact();
            Used?.Invoke(t);
        }
    }
}
