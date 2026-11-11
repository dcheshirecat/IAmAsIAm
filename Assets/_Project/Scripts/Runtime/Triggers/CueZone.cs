using UnityEngine;

namespace IAmAsIAm.Runtime.Triggers
{
    /// <summary>
    /// A place in the world that carries a trigger cue: the flat above where the neighbours shout,
    /// a church whose bells ring at noon. Walking in starts dissociation. Staying in it (or being unable
    /// to leave) builds exposure, and after the trigger's switchAfterSeconds a full switch happens.
    /// Leaving resets it. Once the player knows that trigger,
    /// the zone can show a subtle sign (<see cref="knownSign"/>) so they can choose another way round.
    /// Walking around a known zone counts as avoiding it.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public sealed class CueZone : MonoBehaviour
    {
        [SerializeField] string cue;
        [Tooltip("Optional: shown once the player has learned this trigger (e.g. a faint sound-wave decal).")]
        [SerializeField] GameObject knownSign;
        [Tooltip("Seconds after leaving before the same zone can set the trigger off again.")]
        [SerializeField] float cooldown = 30f;

        float _readyAt;

        void Reset() => GetComponent<Collider>().isTrigger = true;

        void Update()
        {
            bool known = GameServices.Triggers != null && GameServices.Triggers.IsKnownCue(cue);
            if (knownSign != null && knownSign.activeSelf != known) knownSign.SetActive(known);
        }

        bool _inside;

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<Player.PlayerMotor>() == null || Time.time < _readyAt) return;
            _inside = true;
            TriggerRunner.Expose(cue, 0f); // The first exposure is the encounter.
        }

        void OnTriggerStay(Collider other)
        {
            if (_inside && other.GetComponentInParent<Player.PlayerMotor>() != null) TriggerRunner.Expose(cue, Time.deltaTime);
        }

        void OnTriggerExit(Collider other)
        {
            if (!_inside || other.GetComponentInParent<Player.PlayerMotor>() == null) return;
            _inside = false;
            _readyAt = Time.time + cooldown;
            TriggerRunner.EndExposure(cue);
        }

        /// <summary>Call from a larger "approach" trigger around the zone: leaving it without entering counts as avoiding.</summary>
        public void ApproachedAndLeft()
        {
            if (GameServices.Triggers != null && GameServices.Triggers.IsKnownCue(cue)) GameServices.Triggers.RecordAvoided(cue);
        }
    }
}
