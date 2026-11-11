using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.Practices
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  Rituals: the open practices the character DOES in the story. Tarot readings,
    //  rune casting, charm crafting and scent sachets are playable actions. Their
    //  results are written into the story (Ink variables), so a card drawn can open
    //  a conversation, give a part a way in, or colour a scene. They still never
    //  change stats; meaning, not mechanics.
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>A tarot spread. Authored in Resources/Data/tarot_spreads.json.</summary>
    [Serializable]
    public sealed class TarotSpreadDefinition
    {
        public string id;
        public string name;
        public string description;
        public List<SpreadPosition> positions = new List<SpreadPosition>();
        /// <summary>When true, each present part draws one card (the system's own spread). The position list is ignored.</summary>
        public bool onePerPart;
    }

    [Serializable]
    public sealed class SpreadPosition
    {
        public string id;
        public string label;
        /// <summary>Gentle question for this position, combined with the card's own prompt.</summary>
        public string question;
    }

    [Serializable]
    public sealed class TarotSpreadData
    {
        public List<TarotSpreadDefinition> spreads = new List<TarotSpreadDefinition>();
    }

    public sealed class ReadingCard
    {
        public string PositionId;
        public string PositionLabel;
        public string Question;
        /// <summary>For part-spreads: the member who drew this card.</summary>
        public string DrawnById;
        public TarotDraw Draw;
    }

    /// <summary>A completed reading. It feeds the story and the Inner Journal.</summary>
    public sealed class TarotReading
    {
        public string SpreadId;
        public List<ReadingCard> Cards = new List<ReadingCard>();

        /// <summary>
        /// Variables for the story, e.g. tarot_spread = "three-card", tarot_present = "major-17-star".
        /// Ink declares matching VARs; the runner copies these in.
        /// </summary>
        public Dictionary<string, string> ToStoryVariables()
        {
            var vars = new Dictionary<string, string> { ["tarot_spread"] = SpreadId };
            foreach (var c in Cards)
            {
                string key = c.DrawnById != null ? "tarot_part_" + c.DrawnById : "tarot_" + c.PositionId;
                vars[key.Replace('-', '_')] = c.Draw.Card.id;
            }
            return vars;
        }
    }

    public static class TarotReader
    {
        /// <summary>Lays out a spread. For part-spreads, pass the member ids that are present (fronting + co-conscious).</summary>
        public static TarotReading Read(TarotSpreadDefinition spread, TarotDeck deck, IEnumerable<string> presentMemberIds = null)
        {
            if (spread == null) throw new ArgumentNullException(nameof(spread));
            var reading = new TarotReading { SpreadId = spread.id };

            if (spread.onePerPart)
            {
                foreach (var member in presentMemberIds ?? Enumerable.Empty<string>())
                    reading.Cards.Add(new ReadingCard
                    {
                        PositionId = member,
                        PositionLabel = member,
                        Question = "What does this card say about how they're doing?",
                        DrawnById = member,
                        Draw = deck.Draw(),
                    });
                return reading;
            }

            foreach (var pos in spread.positions)
                reading.Cards.Add(new ReadingCard
                {
                    PositionId = pos.id,
                    PositionLabel = pos.label,
                    Question = pos.question,
                    Draw = deck.Draw(),
                });
            return reading;
        }
    }

    /// <summary>
    /// A step-by-step crafting ritual (a charm from charms.json, or a scent sachet).
    /// Each step is one mindful action: "one thing in the moment". There's no failing it,
    /// and stopping midway is allowed; it can be picked up again.
    /// </summary>
    public sealed class CraftRitual
    {
        readonly IReadOnlyList<string> _steps;

        public string ItemId { get; }
        public int StepIndex { get; private set; }
        public bool IsComplete => StepIndex >= _steps.Count;
        public string CurrentStep => IsComplete ? null : _steps[StepIndex];
        public float Progress => _steps.Count == 0 ? 1f : (float)StepIndex / _steps.Count;

        public event Action<int, string> StepDone;
        public event Action<string> Crafted;

        public CraftRitual(string itemId, IReadOnlyList<string> steps)
        {
            if (steps == null || steps.Count == 0) throw new ArgumentException("A ritual needs at least one step.");
            ItemId = itemId;
            _steps = steps;
        }

        public static CraftRitual ForCharm(CharmEntry charm) => new CraftRitual(charm.id, charm.craftSteps);

        public void CompleteStep()
        {
            if (IsComplete) return;
            StepDone?.Invoke(StepIndex, _steps[StepIndex]);
            StepIndex++;
            if (IsComplete) Crafted?.Invoke(ItemId);
        }

        public void ResumeAt(int stepIndex) => StepIndex = Math.Max(0, Math.Min(stepIndex, _steps.Count));
    }

    /// <summary>
    /// A scent sachet: dried herbs tied in cloth, used as a smell anchor for grounding.
    /// It's never brewed or eaten (see the herb safety rules). Choose 1 to 3 herbs.
    /// </summary>
    public static class SachetBlend
    {
        public const int MaxHerbs = 3;

        public static bool IsValid(IReadOnlyCollection<string> herbIds, PracticeLibrary library, out string reason)
        {
            reason = null;
            if (herbIds == null || herbIds.Count == 0) { reason = "Choose at least one herb."; return false; }
            if (herbIds.Count > MaxHerbs) { reason = $"A sachet holds up to {MaxHerbs} herbs."; return false; }
            if (herbIds.Distinct().Count() != herbIds.Count) { reason = "Each herb only once."; return false; }
            foreach (var id in herbIds)
            {
                if (!library.Herbs.Any(h => h.id == id)) { reason = $"'{id}' isn't a herb."; return false; }
                if (!library.Herbs.First(h => h.id == id).anchorSenses.Contains("Smell")) { reason = $"'{id}' has no scent to anchor to."; return false; }
            }
            return true;
        }

        /// <summary>Combined sensory text for the grounding moment.</summary>
        public static string Describe(IEnumerable<string> herbIds, PracticeLibrary library) =>
            string.Join(" ", herbIds.Select(id => library.Herbs.First(h => h.id == id).sensory));
    }
}
