using IAmAsIAm.Core.Mind;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

namespace IAmAsIAm.Runtime.Mind
{
    /// <summary>
    /// Translates the mind state into how the world looks and sounds. Each experience has its own
    /// URP Volume (authored by art), blended by weight.
    ///
    /// Rules:
    ///  * Every intensity passes through <c>ComfortSettings.CapDistortion</c>.
    ///  * Changes are smoothed and never faster than <see cref="minTransitionSeconds"/>, which
    ///    avoids photosensitivity triggers.
    ///  * The "grounded" volume is the inverse: warm, rich and present.
    /// </summary>
    public sealed class DissociationPresentationDriver : MonoBehaviour
    {
        [Header("Post-processing volumes (global, priority above base)")]
        [SerializeField] Volume groundedVolume;
        [SerializeField] Volume fogVolume;
        [SerializeField] Volume derealizationVolume;
        [SerializeField] Volume depersonalizationVolume;
        [SerializeField] Volume coConsciousnessVolume;

        [Header("Audio")]
        [SerializeField] AudioMixer mixer;
        [Tooltip("Exposed mixer parameter: 0 = clear, 1 = muffled. Map it to a low-pass cutoff in the mixer.")]
        [SerializeField] string muffleParameter = "Muffle";
        [Tooltip("Exposed mixer parameter for the co-conscious voice layer volume (dB).")]
        [SerializeField] string innerVoiceParameter = "InnerVoiceVolume";

        [Header("Feel")]
        [Tooltip("Minimum seconds for any effect to go from 0 to 1. Keep it ≥ 1.5 for photosensitivity safety.")]
        [SerializeField, Min(1.5f)] float minTransitionSeconds = 2f;

        float _fog, _dereal, _depers, _cocon;

        /// <summary>Smoothed, comfort-capped depersonalization amount, for the camera rig to read.</summary>
        public float Depersonalization => _depers;

        void Update()
        {
            var mind = GameServices.Mind;
            if (mind == null) return;
            var comfort = GameServices.Comfort;

            float step = Time.unscaledDeltaTime / minTransitionSeconds;
            _fog = Mathf.MoveTowards(_fog, comfort.CapDistortion(mind.GetIntensity(DissociativeExperience.Fog)), step);
            _dereal = Mathf.MoveTowards(_dereal, comfort.CapDistortion(mind.GetIntensity(DissociativeExperience.Derealization)), step);
            _depers = Mathf.MoveTowards(_depers, comfort.CapDistortion(mind.GetIntensity(DissociativeExperience.Depersonalization)), step);
            _cocon = Mathf.MoveTowards(_cocon, comfort.CapDistortion(mind.GetIntensity(DissociativeExperience.CoConsciousness)), step);

            SetWeight(fogVolume, _fog);
            SetWeight(derealizationVolume, _dereal);
            SetWeight(depersonalizationVolume, _depers);
            SetWeight(coConsciousnessVolume, _cocon);
            SetWeight(groundedVolume, mind.Presence);

            if (mixer != null)
            {
                float muffle = comfort.disableAudioDistortion ? 0f : Mathf.Max(_fog, _dereal * 0.6f, _depers * 0.4f);
                mixer.SetFloat(muffleParameter, muffle);
                // Inner voices fade in from -40 dB to 0 dB with co-consciousness.
                mixer.SetFloat(innerVoiceParameter, Mathf.Lerp(-40f, 0f, _cocon));
            }
        }

        static void SetWeight(Volume v, float w)
        {
            if (v != null) v.weight = w;
        }
    }
}
