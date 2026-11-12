using IAmAsIAm.Runtime.Story;
using UnityEngine;

namespace IAmAsIAm.Runtime.Audio
{
    /// <summary>
    /// Plays the owner's recorded voice lines (owner: every voice is their own recording; never generated). A spoken line in
    /// the story carries <c># vo: &lt;chapter&gt;_&lt;speaker&gt;_&lt;0000&gt;</c>; its file is
    /// <c>Assets/_Project/Resources/Voice/&lt;chapter&gt;/vo_&lt;chapter&gt;_&lt;speaker&gt;_&lt;0000&gt;.mp3</c>. A line without a file
    /// stays text only, so lines can arrive one at a time. The next line (or the end of the story) stops the last one.
    ///
    /// Inner voices ("Name (inside)") sound a little closed-in and quieter; while Maya is far away (dissociating) every voice is
    /// a little muffled. Both follow the comfort setting "no audio distortion".
    /// </summary>
    public sealed class VoicePlayer : MonoBehaviour
    {
        AudioSource _source;
        AudioLowPassFilter _filter;

        void Awake()
        {
            // The filter goes on the same object as the source (Unity applies it to that source's sound).
            var go = new GameObject("Voice");
            go.transform.SetParent(transform, false);
            _source = go.AddComponent<AudioSource>();
            _source.playOnAwake = false;
            _source.spatialBlend = 0f;
            _filter = go.AddComponent<AudioLowPassFilter>();
            _filter.cutoffFrequency = 22000f;
        }

        /// <summary>Plays the line's recording if there is one; otherwise just stops the last one.</summary>
        public void Play(StoryLine line)
        {
            Stop();
            string id = VoiceId(line);
            if (id == null) return;
            int underscore = id.IndexOf('_');
            if (underscore <= 0) return;
            var clip = Resources.Load<AudioClip>($"Voice/{id.Substring(0, underscore)}/vo_{id}");
            if (clip == null) return;

            bool calm = GameServices.Comfort != null && GameServices.Comfort.disableAudioDistortion;
            float far = GameServices.Mind == null ? 0f : Mathf.Clamp01(1f - GameServices.Mind.Presence);
            if (GameServices.Comfort != null) far = GameServices.Comfort.CapDistortion(far);
            float cutoff = 22000f;
            if (!calm)
            {
                if (line.Inner) cutoff = 4200f;            // Inside: closer, softer.
                cutoff = Mathf.Min(cutoff, Mathf.Lerp(22000f, 2500f, far)); // Far away: muffled.
            }
            _filter.cutoffFrequency = cutoff;
            _source.volume = line.Inner ? 0.85f : 1f;
            _source.clip = clip;
            _source.Play();
        }

        public void Stop()
        {
            if (_source != null && _source.isPlaying) _source.Stop();
        }

        /// <summary>The "# vo:" tag's value, or null.</summary>
        static string VoiceId(StoryLine line)
        {
            if (line?.Tags == null) return null;
            foreach (var raw in line.Tags)
            {
                var t = raw.Trim();
                if (!t.StartsWith("vo:")) continue;
                var v = t.Substring(3).Trim();
                return v.Length > 0 ? v : null;
            }
            return null;
        }
    }
}
