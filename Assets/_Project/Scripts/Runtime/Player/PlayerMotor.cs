using IAmAsIAm.Core.Mind;
using IAmAsIAm.Runtime.CameraRig;
using IAmAsIAm.Runtime.PluralSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IAmAsIAm.Runtime.Player
{
    /// <summary>
    /// Third-person walking, relative to the camera (left stick, WASD or arrows, or the on-screen
    /// joystick, which drives the virtual gamepad's left stick). The character turns to face where
    /// it walks. Movement stops while a conversation, an exercise or the Anchor is open.
    /// Fog (hypoarousal) slows the walk a little, as the GDD describes.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        [SerializeField] ShoulderCamera cameraRig;
        [SerializeField] FrontingController fronting;
        [SerializeField] float walkSpeed = 2.2f;
        [SerializeField] float runMultiplier = 1.7f;
        [SerializeField] float turnSharpness = 12f;
        [SerializeField] float gravity = -18f;

        CharacterController _controller;
        InputAction _move, _run;
        float _verticalSpeed;

        /// <summary>Set by the story screen: no walking while it's busy.</summary>
        public static bool Blocked { get; set; }

        /// <summary>0..1 how fast the character is moving, for the animator.</summary>
        public float SpeedNormalized { get; private set; }

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _move = new InputAction("Move", binding: "<Gamepad>/leftStick");
            _move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
            _move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow").With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow").With("Right", "<Keyboard>/rightArrow");
            _run = new InputAction("Run", binding: "<Keyboard>/leftShift");
            _run.AddBinding("<Gamepad>/leftTrigger");
        }

        void OnEnable() { _move.Enable(); _run.Enable(); }
        void OnDisable() { _move.Disable(); _run.Disable(); }

        void Update()
        {
            Vector2 input = Blocked || Time.timeScale <= 0f ? Vector2.zero : Vector2.ClampMagnitude(_move.ReadValue<Vector2>(), 1f);

            Vector3 forward = cameraRig != null ? cameraRig.FlatForward : Vector3.forward;
            Vector3 right = cameraRig != null ? cameraRig.FlatRight : Vector3.right;
            Vector3 wish = forward * input.y + right * input.x;

            float speed = fronting != null && fronting.Current != null ? fronting.WalkSpeed : walkSpeed;
            if (_run.IsPressed()) speed *= runMultiplier;
            var mind = GameServices.Mind;
            if (mind != null) speed *= 1f - 0.35f * mind.GetIntensity(DissociativeExperience.Fog);

            if (wish.sqrMagnitude > 0.0001f)
            {
                var face = Quaternion.LookRotation(wish.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, face, 1f - Mathf.Exp(-turnSharpness * Time.deltaTime));
            }

            _verticalSpeed = _controller.isGrounded ? -1f : _verticalSpeed + gravity * Time.deltaTime;
            _controller.Move((wish * speed + Vector3.up * _verticalSpeed) * Time.deltaTime);
            SpeedNormalized = wish.magnitude * (_run.IsPressed() ? 1f : 0.6f);
        }
    }
}
