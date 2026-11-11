using System;
using System.Collections.Generic;

namespace IAmAsIAm.Core.Practices
{
    public readonly struct TarotDraw
    {
        public readonly TarotCardEntry Card;
        public readonly bool Reversed;

        public TarotDraw(TarotCardEntry card, bool reversed)
        {
            Card = card;
            Reversed = reversed;
        }

        public IReadOnlyList<string> Keywords => Reversed && Card.reversedKeywords.Count > 0 ? Card.reversedKeywords : Card.uprightKeywords;
    }

    /// <summary>
    /// Tarot used as a journaling prompt, as many practitioners use it, and never as a
    /// prediction. Draws feed <c>JournalEntryKind.TarotReflection</c>. Seeded so the story
    /// can script a draw or replay one.
    /// </summary>
    public sealed class TarotDeck
    {
        readonly List<TarotCardEntry> _cards;
        readonly List<int> _order = new List<int>();
        readonly Random _rng;
        int _cursor;

        /// <summary>Off by default (Comfort setting). Many readers don't use reversals.</summary>
        public bool AllowReversals { get; set; }

        public int Remaining => _order.Count - _cursor;

        public TarotDeck(IEnumerable<TarotCardEntry> cards, int seed)
        {
            _cards = new List<TarotCardEntry>(cards);
            if (_cards.Count == 0) throw new ArgumentException("Tarot deck needs at least one card.");
            _rng = new Random(seed);
            Shuffle();
        }

        public void Shuffle()
        {
            _order.Clear();
            for (int i = 0; i < _cards.Count; i++) _order.Add(i);
            for (int i = _order.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                (_order[i], _order[j]) = (_order[j], _order[i]);
            }
            _cursor = 0;
        }

        public TarotDraw Draw()
        {
            if (Remaining == 0) Shuffle();
            var card = _cards[_order[_cursor++]];
            bool reversed = AllowReversals && _rng.NextDouble() < 0.5;
            return new TarotDraw(card, reversed);
        }

        /// <summary>For scripted story beats: draw a specific card.</summary>
        public TarotDraw DrawSpecific(string cardId)
        {
            var card = _cards.Find(c => c.id == cardId)
                       ?? throw new ArgumentException($"No tarot card with id '{cardId}'.");
            return new TarotDraw(card, false);
        }
    }

    /// <summary>
    /// Pulling a single rune as a prompt, in the same spirit as <see cref="TarotDeck"/>.
    /// </summary>
    public sealed class RunePouch
    {
        readonly List<RuneEntry> _runes;
        readonly Random _rng;

        public RunePouch(IEnumerable<RuneEntry> runes, int seed)
        {
            _runes = new List<RuneEntry>(runes);
            if (_runes.Count == 0) throw new ArgumentException("Rune pouch is empty.");
            _rng = new Random(seed);
        }

        public RuneEntry Pull() => _runes[_rng.Next(_runes.Count)];
    }
}
