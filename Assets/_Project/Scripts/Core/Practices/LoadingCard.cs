using System;
using System.Collections.Generic;

namespace IAmAsIAm.Core.Practices
{
    /// <summary>
    /// Picks the card for the loading screen (owner, 2026-11-12: "an animated loading screen with a random tarot card
    /// each time"). Never the same card twice in a row. Reversed only if the player allows reversals.
    /// </summary>
    public sealed class LoadingCardPicker
    {
        readonly List<TarotCardEntry> _cards;
        readonly Random _rng;

        public LoadingCardPicker(IEnumerable<TarotCardEntry> cards, int seed)
        {
            _cards = new List<TarotCardEntry>();
            if (cards != null) foreach (var c in cards) if (c != null) _cards.Add(c);
            _rng = new Random(seed);
        }

        public int Count => _cards.Count;

        /// <summary>A random card that isn't <paramref name="lastCardId"/> (unless it's the only card).</summary>
        public TarotDraw Pick(string lastCardId, bool allowReversals)
        {
            if (_cards.Count == 0) throw new InvalidOperationException("No tarot cards to show.");
            var pool = _cards.Count > 1 ? _cards.FindAll(c => c.id != lastCardId) : _cards;
            if (pool.Count == 0) pool = _cards;
            var card = pool[_rng.Next(pool.Count)];
            bool reversed = allowReversals && _rng.NextDouble() < 0.5;
            return new TarotDraw(card, reversed);
        }
    }

    /// <summary>How the loading card looks at a moment in time. All values are 0..1 except OffsetY (reference pixels).</summary>
    public readonly struct LoadingCardPose
    {
        /// <summary>The whole card's opacity (fades in, and out when loading ends).</summary>
        public readonly float Alpha;
        /// <summary>Horizontal scale, for the flip (the card turns edge-on, then opens to the face).</summary>
        public readonly float ScaleX;
        public readonly float OffsetY;
        public readonly float BackAlpha;
        public readonly float FaceAlpha;
        /// <summary>The soft glow behind the card. A slow breath, never a flash.</summary>
        public readonly float Glow;

        public LoadingCardPose(float alpha, float scaleX, float offsetY, float backAlpha, float faceAlpha, float glow)
        {
            Alpha = alpha; ScaleX = scaleX; OffsetY = offsetY; BackAlpha = backAlpha; FaceAlpha = faceAlpha; Glow = glow;
        }

        public bool FaceShowing => FaceAlpha >= 0.5f;
    }

    /// <summary>
    /// The loading card's animation: it rises in face down, turns over, then floats gently while the game loads.
    /// With Reduce motion on, it doesn't move: the back cross-fades to the face and the glow stays still.
    /// Nothing flashes (ContentAndSafetyGuidelines: no flashing, ever).
    /// </summary>
    public static class LoadingCardAnimation
    {
        public const float AppearSeconds = 0.45f;
        public const float FlipStart = 0.7f;
        public const float FlipSeconds = 0.7f;
        /// <summary>The loading screen stays at least this long, so the card can be seen and its prompt read.</summary>
        public const float MinimumSeconds = 4f;
        /// <summary>The first one, when the game starts, stays longer (owner: it was too short): time to read the card and its prompt.</summary>
        public const float StartupMinimumSeconds = 7f;
        public const float FadeOutSeconds = 0.4f;
        /// <summary>One slow breath of the glow and float, in seconds.</summary>
        public const float BreathSeconds = 4.5f;
        public const float RiseFrom = -60f;
        public const float FloatHeight = 10f;

        /// <param name="t">Seconds since the loading screen appeared.</param>
        /// <param name="fadeOut">0..1, how far the closing fade has gone (0 while still loading).</param>
        public static LoadingCardPose Evaluate(float t, bool reduceMotion, float fadeOut = 0f)
        {
            if (t < 0f) t = 0f;
            float appear = Clamp01(t / AppearSeconds);
            float alpha = SmoothStep(appear) * (1f - Clamp01(fadeOut));
            float flip = Clamp01((t - FlipStart) / FlipSeconds);
            float breath = (float)Math.Sin(2.0 * Math.PI * t / BreathSeconds); // -1..1

            if (reduceMotion)
                return new LoadingCardPose(alpha, 1f, 0f, 1f - SmoothStep(flip), SmoothStep(flip), 0.35f);

            // Edge-on halfway through the flip: back before it, face after.
            float scaleX = Math.Abs((float)Math.Cos(Math.PI * SmoothStep(flip)));
            bool face = flip >= 0.5f;
            float rise = RiseFrom * (1f - SmoothStep(appear));
            float drift = flip >= 1f ? FloatHeight * breath * SmoothStep(Clamp01((t - FlipStart - FlipSeconds) / 1f)) : 0f;
            float glow = 0.3f + 0.12f * breath;
            return new LoadingCardPose(alpha, scaleX, rise + drift, face ? 0f : 1f, face ? 1f : 0f, glow);
        }

        /// <summary>The "Loading" dots: 0..3, one more every half second. Static (3) with Reduce motion.</summary>
        public static int Dots(float t, bool reduceMotion) => reduceMotion ? 3 : (int)(Math.Max(0f, t) / 0.5f) % 4;

        /// <summary>Roman numeral for a Major Arcana number (0 is the Fool's "0").</summary>
        public static string Numeral(int n)
        {
            if (n <= 0) return "0";
            var map = new (int v, string s)[] { (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I") };
            var sb = new System.Text.StringBuilder();
            foreach (var (v, s) in map) while (n >= v) { sb.Append(s); n -= v; }
            return sb.ToString();
        }

        static float Clamp01(float x) => x < 0f ? 0f : x > 1f ? 1f : x;
        static float SmoothStep(float x) { x = Clamp01(x); return x * x * (3f - 2f * x); }
    }
}
