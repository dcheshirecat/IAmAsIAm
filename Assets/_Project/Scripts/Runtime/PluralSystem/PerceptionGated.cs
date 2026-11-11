using UnityEngine;

namespace IAmAsIAm.Runtime.PluralSystem
{
    /// <summary>
    /// Shows an object only when the fronting member, or any co-conscious member, has a
    /// perception trait. Examples: Bramble's glyphs on the forest trees, Pip's small paths,
    /// Sorrel's birdsong. This is how puzzles can need two parts' perceptions at once.
    /// </summary>
    public sealed class PerceptionGated : MonoBehaviour
    {
        [Tooltip("Trait id from system_members.json perceptionTraits, e.g. 'glyphs'.")]
        [SerializeField] string requiredTrait;
        [Tooltip("Objects to show or hide. If empty, toggles all child renderers and colliders.")]
        [SerializeField] GameObject[] targets;
        [Tooltip("Seconds to fade in, for renderers whose material exposes _Reveal (0..1).")]
        [SerializeField] float revealSeconds = 1.5f;

        static readonly int RevealId = Shader.PropertyToID("_Reveal");
        Renderer[] _renderers;
        Collider[] _colliders;
        MaterialPropertyBlock _mpb;
        float _reveal;

        void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>(true);
            _colliders = GetComponentsInChildren<Collider>(true);
            _mpb = new MaterialPropertyBlock();
        }

        void Update()
        {
            var roster = GameServices.Roster;
            bool visible = roster != null && roster.CanPerceive(requiredTrait);
            _reveal = Mathf.MoveTowards(_reveal, visible ? 1f : 0f, Time.deltaTime / Mathf.Max(0.01f, revealSeconds));

            if (targets != null && targets.Length > 0)
            {
                foreach (var t in targets) if (t != null && t.activeSelf != visible) t.SetActive(visible);
                return;
            }

            foreach (var r in _renderers)
            {
                r.enabled = _reveal > 0.001f;
                r.GetPropertyBlock(_mpb);
                _mpb.SetFloat(RevealId, _reveal);
                r.SetPropertyBlock(_mpb);
            }
            foreach (var c in _colliders) c.enabled = visible;
        }
    }
}
