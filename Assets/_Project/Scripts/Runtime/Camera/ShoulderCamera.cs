using IAmAsIAm.Core.Mind;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IAmAsIAm.Runtime.CameraRig
{
    /// <summary>
    /// The owner's chosen camera (2026-11-11): close third person, behind and over the character's
    /// shoulder, in the style of Pokémon Legends: Z-A. The player orbits it with the right stick, the
    /// mouse (while the right button is held, or always if lockMouseLook), or by dragging on the right
    /// half of a touch screen (TouchControls feeds <see cref="AddLook"/>).
    ///
    /// Dissociation: depersonalization slowly pulls the camera back and up, out of the body, as if
    /// watching yourself. Grounding brings it back in. This respects the "Disable camera drift"
    /// comfort setting. Walls pull the camera in so it never clips through them.
    /// In conversations it eases closer and lower, framing the character and who they're talking to.
    /// </summary>
    public sealed class ShoulderCamera : MonoBehaviour
    {
        [Header("Follow")]
        [SerializeField] Transform target;
        [Tooltip("Point on the character the camera looks past (about shoulder/head height).")]
        [SerializeField] float pivotHeight = 1.45f;
        [Tooltip("Metres behind the pivot. Z-A style: close.")]
        [SerializeField] float distance = 2.3f;
        [Tooltip("Metres to the right of the character: the over-the-shoulder offset.")]
        [SerializeField] float shoulderOffset = 0.4f;
        [SerializeField] float followSharpness = 12f;

        [Header("Orbit")]
        [SerializeField] float stickDegreesPerSecond = 160f;
        [SerializeField] float mouseDegreesPerPixel = 0.15f;
        [SerializeField] float touchDegreesPerPixel = 0.2f;
        [SerializeField] Vector2 pitchLimits = new Vector2(-25f, 60f);
        [SerializeField] float startPitch = 12f;
        [Tooltip("When true, the mouse always orbits. When false, hold the right mouse button.")]
        [SerializeField] bool lockMouseLook;

        [Header("Collision")]
        [SerializeField] float collisionRadius = 0.2f;
        [SerializeField] LayerMask collisionMask = ~0;

        [Header("Dissociation")]
        [Tooltip("Extra distance at full depersonalization.")]
        [SerializeField] float driftDistance = 2.8f;
        [Tooltip("Extra height at full depersonalization.")]
        [SerializeField] float driftHeight = 1.4f;

        [Header("Conversation")]
        [SerializeField] float talkDistance = 1.6f;
        [SerializeField] float talkPivotHeight = 1.5f;

        float _yaw, _pitch, _drift, _talk;
        Vector3 _pivot;
        Transform _talkPartner;
        InputAction _stickLook, _mouseLook, _mouseHold;

        public Transform Target { get => target; set { target = value; SnapBehind(); } }

        /// <summary>The flat forward direction of the camera, for camera-relative movement.</summary>
        public Vector3 FlatForward => Quaternion.Euler(0f, _yaw, 0f) * Vector3.forward;
        public Vector3 FlatRight => Quaternion.Euler(0f, _yaw, 0f) * Vector3.right;

        void Awake()
        {
            _stickLook = new InputAction("Look (stick)", binding: "<Gamepad>/rightStick");
            _mouseLook = new InputAction("Look (mouse)", binding: "<Mouse>/delta");
            _mouseHold = new InputAction("Look (hold)", binding: "<Mouse>/rightButton");
            _pitch = startPitch;
            SnapBehind();
        }

        void OnEnable() { _stickLook.Enable(); _mouseLook.Enable(); _mouseHold.Enable(); }
        void OnDisable() { _stickLook.Disable(); _mouseLook.Disable(); _mouseHold.Disable(); }

        /// <summary>Touch drag look (pixels). Called by TouchControls.</summary>
        public void AddLook(Vector2 pixels) => Rotate(pixels * touchDegreesPerPixel);

        /// <summary>Frame a conversation with someone. Pass null when it ends.</summary>
        public void SetTalkPartner(Transform partner) => _talkPartner = partner;

        public void SnapBehind()
        {
            if (target == null) return;
            _yaw = target.eulerAngles.y;
            _pivot = target.position + Vector3.up * pivotHeight;
        }

        void LateUpdate()
        {
            if (target == null) return;
            float dt = Time.unscaledDeltaTime;

            if (Time.timeScale > 0f)
            {
                Rotate(_stickLook.ReadValue<Vector2>() * stickDegreesPerSecond * dt);
                if (lockMouseLook || _mouseHold.IsPressed()) Rotate(_mouseLook.ReadValue<Vector2>() * mouseDegreesPerPixel);
            }

            // Depersonalization drift, smoothed and comfort-capped.
            var mind = GameServices.Mind;
            float targetDrift = 0f;
            if (mind != null && !GameServices.Comfort.disableCameraDrift)
                targetDrift = GameServices.Comfort.CapDistortion(mind.GetIntensity(DissociativeExperience.Depersonalization));
            _drift = Mathf.MoveTowards(_drift, targetDrift, dt / 3f); // Slow: felt before it's seen.
            _talk = Mathf.MoveTowards(_talk, _talkPartner != null ? 1f : 0f, dt * 2f);

            // Where the camera looks past: the character, or between the two people talking.
            Vector3 focus = target.position + Vector3.up * Mathf.Lerp(pivotHeight, talkPivotHeight, _talk);
            if (_talkPartner != null)
                focus = Vector3.Lerp(focus, (target.position + _talkPartner.position) * 0.5f + Vector3.up * talkPivotHeight, _talk * 0.5f);
            _pivot = Vector3.Lerp(_pivot, focus, 1f - Mathf.Exp(-followSharpness * dt));

            var rotation = Quaternion.Euler(_pitch + _drift * 12f, _yaw, 0f);
            float dist = Mathf.Lerp(distance, talkDistance, _talk) + _drift * driftDistance;
            Vector3 shoulder = rotation * Vector3.right * shoulderOffset * (1f - _drift);
            Vector3 pivot = _pivot + Vector3.up * (_drift * driftHeight) + shoulder;
            Vector3 wanted = pivot - rotation * Vector3.forward * dist;

            // Pull in in front of walls.
            Vector3 dir = wanted - pivot;
            if (Physics.SphereCast(pivot, collisionRadius, dir.normalized, out var hit, dir.magnitude, collisionMask, QueryTriggerInteraction.Ignore)
                && !hit.transform.IsChildOf(target))
                wanted = pivot + dir.normalized * Mathf.Max(0.3f, hit.distance);

            transform.SetPositionAndRotation(wanted, rotation);
        }

        void Rotate(Vector2 degrees)
        {
            _yaw += degrees.x;
            _pitch = Mathf.Clamp(_pitch - degrees.y, pitchLimits.x, pitchLimits.y);
        }
    }
}
