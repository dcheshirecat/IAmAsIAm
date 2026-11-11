using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Runtime.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace IAmAsIAm.Runtime.Practices
{
    /// <summary>
    /// The Stone Circle altar: draw a tarot card or pull a rune as a journaling prompt.
    /// It shows the card and prompt, then the player writes (or skips) a reflection, which goes
    /// in the Inner Journal under whoever is fronting.
    ///
    /// Reminder: draws are prompts, not predictions, and have no gameplay effect.
    /// </summary>
    public sealed class ReflectionAltar : MonoBehaviour, IInteractable
    {
        public enum Mode { Tarot, Rune }

        [SerializeField] Mode mode = Mode.Tarot;
        [Tooltip("Seed for reproducible draws in testing. 0 = random each session.")]
        [SerializeField] int seed;
        [Tooltip("Optional: force a specific card for a scripted story beat.")]
        [SerializeField] string scriptedCardId;

        /// <summary>(title, keywords, prompt, gentleNote) for the UI to display.</summary>
        public UnityEvent<string, string, string, string> onDrawn;

        TarotDeck _deck;
        RunePouch _pouch;
        string _lastReferenceId;

        public string Prompt => mode == Mode.Tarot ? "Draw a card" : "Pull a rune";
        public bool CanInteract => GameServices.IsReady;

        public void Interact()
        {
            int s = seed != 0 ? seed : System.Environment.TickCount;
            var lib = GameServices.Practices;

            if (mode == Mode.Tarot)
            {
                _deck ??= new TarotDeck(lib.Tarot, s);
                _deck.AllowReversals = GameServices.Comfort.allowTarotReversals;
                var draw = string.IsNullOrEmpty(scriptedCardId) ? _deck.Draw() : _deck.DrawSpecific(scriptedCardId);
                scriptedCardId = null; // A scripted draw happens only once.
                _lastReferenceId = draw.Card.id;
                string title = draw.Reversed ? draw.Card.name + " (reversed)" : draw.Card.name;
                onDrawn?.Invoke(title, string.Join(" · ", draw.Keywords), draw.Card.reflectionPrompt, draw.Card.gentleNote);
            }
            else
            {
                _pouch ??= new RunePouch(lib.Runes, s);
                var rune = _pouch.Pull();
                _lastReferenceId = rune.id;
                onDrawn?.Invoke($"{rune.glyph}  {rune.name}", rune.literalMeaning, rune.reflectionPrompt, rune.contextNote);
            }
        }

        /// <summary>Called by the UI when the player writes a reflection. Empty text is fine; it's just not saved.</summary>
        public void SubmitReflection(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || _lastReferenceId == null) return;
            GameServices.Journal.Write(
                mode == Mode.Tarot ? JournalEntryKind.TarotReflection : JournalEntryKind.RuneReflection,
                GameServices.Roster.FrontingId,
                GameServices.InGameDay,
                text,
                _lastReferenceId);
        }
    }
}
