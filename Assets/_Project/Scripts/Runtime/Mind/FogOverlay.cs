using UnityEngine;
using UnityEngine.UI;

namespace IAmAsIAm.Runtime.Mind
{
    /// <summary>
    /// Fog whenever the character dissociates in any way (owner, 2026-11-12): depersonalization, derealization, fog, losing
    /// time. Two soft, slowly drifting layers of mist over the screen, plus fog in the 3D world, both thickening with the
    /// strongest dissociation (1 - Presence). Co-consciousness alone doesn't fog (it's shared presence, not less presence).
    ///
    /// Comfort and safety:
    ///  * the amount passes through ComfortSettings.CapDistortion;
    ///  * it never changes faster than <see cref="MinTransitionSeconds"/> (no flashes);
    ///  * with Reduce motion on, the mist doesn't drift.
    /// The mist texture is made in code (tileable noise), so no art is needed; art can replace it later.
    /// </summary>
    public sealed class FogOverlay : MonoBehaviour
    {
        /// <summary>Seconds for the fog to go from none to full. Keep at 1.5 or more (photosensitivity).</summary>
        public const float MinTransitionSeconds = 2.5f;

        static readonly Color MistColor = new Color(0.82f, 0.83f, 0.86f);
        static readonly Color WorldFogColor = new Color(0.62f, 0.63f, 0.66f);

        RawImage _near, _far;
        Image _wash;
        float _amount;
        bool _sceneFogWas;
        Color _sceneFogColorWas;
        float _sceneFogDensityWas;
        FogMode _sceneFogModeWas;

        /// <summary>The fog right now, 0..1 (smoothed and capped). Other effects can follow it.</summary>
        public float Amount => _amount;

        /// <summary>Builds the overlay as a full-screen child of <paramref name="canvasRoot"/> that never takes touches.</summary>
        public static FogOverlay Create(RectTransform canvasRoot)
        {
            var go = new GameObject("Fog", typeof(RectTransform));
            go.transform.SetParent(canvasRoot, false);
            Stretch(go.GetComponent<RectTransform>());
            var fog = go.AddComponent<FogOverlay>();
            fog.Build();
            return fog;
        }

        void Build()
        {
            var tex = MakeMistTexture(256, 1234);

            // A flat wash first (so even still screenshots look hazy), then two drifting mist layers.
            _wash = new GameObject("Wash", typeof(RectTransform), typeof(Image)).GetComponent<Image>();
            _wash.transform.SetParent(transform, false);
            Stretch(_wash.rectTransform);
            _wash.color = new Color(MistColor.r, MistColor.g, MistColor.b, 0f);
            _wash.raycastTarget = false;

            _far = Layer("Mist (far)", tex, new Rect(0, 0, 1.6f, 1.2f));
            _near = Layer("Mist (near)", tex, new Rect(0.37f, 0.21f, 1.0f, 0.75f));
        }

        RawImage Layer(string name, Texture tex, Rect uv)
        {
            var img = new GameObject(name, typeof(RectTransform), typeof(RawImage)).GetComponent<RawImage>();
            img.transform.SetParent(transform, false);
            Stretch(img.rectTransform);
            img.texture = tex;
            img.uvRect = uv;
            img.color = new Color(MistColor.r, MistColor.g, MistColor.b, 0f);
            img.raycastTarget = false;
            return img;
        }

        void OnEnable()
        {
            _sceneFogWas = RenderSettings.fog;
            _sceneFogColorWas = RenderSettings.fogColor;
            _sceneFogDensityWas = RenderSettings.fogDensity;
            _sceneFogModeWas = RenderSettings.fogMode;
        }

        void OnDisable()
        {
            RenderSettings.fog = _sceneFogWas;
            RenderSettings.fogColor = _sceneFogColorWas;
            RenderSettings.fogDensity = _sceneFogDensityWas;
            RenderSettings.fogMode = _sceneFogModeWas;
        }

        void Update()
        {
            var mind = GameServices.Mind;
            var comfort = GameServices.Comfort;
            float target = mind == null ? 0f : comfort.CapDistortion(Mathf.Clamp01(1f - mind.Presence));
            _amount = Mathf.MoveTowards(_amount, target, Time.unscaledDeltaTime / MinTransitionSeconds);

            // Gentle curve: a little dissociation is a light haze, a lot is thick fog (never fully opaque: the Anchor and
            // the dialogue must stay readable, and they're drawn above this anyway).
            float a = Mathf.SmoothStep(0f, 1f, _amount);
            SetAlpha(_wash, a * 0.22f);
            SetAlpha(_far, a * 0.45f);
            SetAlpha(_near, a * 0.55f);

            if (!comfort.reduceMotion && a > 0.001f)
            {
                float t = Time.unscaledDeltaTime;
                Drift(_far, 0.006f * t, 0.002f * t);
                Drift(_near, -0.011f * t, 0.004f * t);
            }

            // The 3D world fogs too.
            if (a > 0.001f)
            {
                RenderSettings.fog = true;
                RenderSettings.fogMode = FogMode.ExponentialSquared;
                RenderSettings.fogColor = WorldFogColor;
                RenderSettings.fogDensity = Mathf.Lerp(0f, 0.22f, a);
            }
            else if (RenderSettings.fog && !_sceneFogWas)
            {
                RenderSettings.fog = false;
            }
        }

        static void SetAlpha(Graphic g, float alpha)
        {
            var c = g.color;
            c.a = alpha;
            g.color = c;
        }

        static void Drift(RawImage img, float dx, float dy)
        {
            var r = img.uvRect;
            r.x = Mathf.Repeat(r.x + dx, 1f);
            r.y = Mathf.Repeat(r.y + dy, 1f);
            img.uvRect = r;
        }

        static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        }

        /// <summary>
        /// Soft, tileable mist: a few octaves of Perlin noise, blended with shifted copies of itself so the edges wrap without
        /// a seam. White, with the noise in the alpha channel.
        /// </summary>
        static Texture2D MakeMistTexture(int size, int seed)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Bilinear,
                name = "Mist (generated)",
            };
            float ox = 100 + seed % 97, oy = 100 + seed % 89; // Positive, so x - size stays in one smooth region.
            float Noise(float x, float y)
            {
                float v = 0f, amp = 0.55f, freq = 3f / size;
                for (int o = 0; o < 4; o++)
                {
                    v += amp * Mathf.PerlinNoise(ox + x * freq, oy + y * freq);
                    amp *= 0.5f; freq *= 2f;
                }
                return v;
            }

            var pixels = new Color32[size * size];
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float u = (float)x / size, v = (float)y / size;
                    // Seamless: blend the four wrapped copies by position.
                    float n = Noise(x, y) * (1 - u) * (1 - v)
                            + Noise(x - size, y) * u * (1 - v)
                            + Noise(x, y - size) * (1 - u) * v
                            + Noise(x - size, y - size) * u * v;
                    float alpha = Mathf.Clamp01((n - 0.35f) * 1.8f);
                    pixels[y * size + x] = new Color32(255, 255, 255, (byte)(alpha * 255));
                }
            tex.SetPixels32(pixels);
            tex.Apply(false, true);
            return tex;
        }
    }
}
