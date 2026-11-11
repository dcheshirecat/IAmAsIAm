using System;
using System.Collections.Generic;

namespace IAmAsIAm.Core.Practices
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  Practice data. IMPORTANT: none of these types carry gameplay modifiers, and
    //  none should ever be added. Practices are lore, sensory anchors and reflection
    //  prompts. See ContentAndSafetyGuidelines.md §4.
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>Anything the player can hold or smell can serve as a grounding anchor.</summary>
    public interface ISensoryAnchor
    {
        string Id { get; }
        string DisplayName { get; }
        /// <summary>Senses this item can be used for in 5-4-3-2-1 (names of <c>Grounding.Sense</c>).</summary>
        IReadOnlyList<string> AnchorSenses { get; }
        /// <summary>The descriptive text shown when the player holds or examines the item.</summary>
        string SensoryDescription { get; }
    }

    [Serializable]
    public sealed class RuneEntry
    {
        public string id;
        /// <summary>Reconstructed Proto-Germanic name, e.g. "Fehu".</summary>
        public string name;
        /// <summary>Unicode Runic block character.</summary>
        public string glyph;
        /// <summary>1, 2 or 3: Freyr's, Hagal's or Tyr's ætt.</summary>
        public int aett;
        /// <summary>Approximate sound value.</summary>
        public string sound;
        /// <summary>What the name literally means.</summary>
        public string literalMeaning;
        /// <summary>Association from the Old English / Norwegian / Icelandic Rune Poems, if any.</summary>
        public string runePoem;
        /// <summary>Modern (mostly 20th century) associations, labelled as such in the UI.</summary>
        public List<string> modernAssociations = new List<string>();
        public string reflectionPrompt;
        /// <summary>Optional: context about misuse by hate groups, and how the game handles it.</summary>
        public string contextNote;
    }

    [Serializable]
    public sealed class TarotCardEntry
    {
        public string id;
        public int number;
        public string name;
        /// <summary>"Major" or "Minor".</summary>
        public string arcana;
        /// <summary>For minors: Wands, Cups, Swords, Pentacles.</summary>
        public string suit;
        public List<string> uprightKeywords = new List<string>();
        public List<string> reversedKeywords = new List<string>();
        public string reflectionPrompt;
        /// <summary>A reframing for cards people often find frightening (Death, The Tower...).</summary>
        public string gentleNote;
    }

    [Serializable]
    public sealed class HerbEntry : ISensoryAnchor
    {
        public string id;
        public string commonName;
        public string latinName;
        public string folklore;
        public string sensory;
        public List<string> anchorSenses = new List<string>();
        /// <summary>Mandatory. Never empty. Validated by the editor data validator.</summary>
        public List<string> safetyNotes = new List<string>();
        public string reflectionPrompt;

        public string Id => id;
        public string DisplayName => commonName;
        public IReadOnlyList<string> AnchorSenses => anchorSenses;
        public string SensoryDescription => sensory;
    }

    [Serializable]
    public sealed class CrystalEntry : ISensoryAnchor
    {
        public string id;
        public string name;
        /// <summary>The mineral identity: a fact.</summary>
        public string mineral;
        public string mohsHardness;
        /// <summary>Modern associations, labelled as belief rather than effect.</summary>
        public List<string> modernAssociations = new List<string>();
        public string tactile;
        public List<string> careNotes = new List<string>();
        public List<string> safetyNotes = new List<string>();
        public List<string> anchorSenses = new List<string>();

        public string Id => id;
        public string DisplayName => name;
        public IReadOnlyList<string> AnchorSenses => anchorSenses;
        public string SensoryDescription => tactile;
    }

    [Serializable]
    public sealed class CharmEntry : ISensoryAnchor
    {
        public string id;
        public string name;
        /// <summary>Where the practice comes from, e.g. "British and Irish folk practice".</summary>
        public string origin;
        public string history;
        /// <summary>How it's made. Crafting this is a mindfulness beat ("one thing in the moment").</summary>
        public List<string> craftSteps = new List<string>();
        public string sensory;
        public List<string> anchorSenses = new List<string>();
        public string reflectionPrompt;

        public string Id => id;
        public string DisplayName => name;
        public IReadOnlyList<string> AnchorSenses => anchorSenses;
        public string SensoryDescription => sensory;
    }

    // JsonUtility needs a root object, so each data file is wrapped in one of these.
    [Serializable] public sealed class RuneData { public string sourceNote; public List<RuneEntry> runes = new List<RuneEntry>(); }
    [Serializable] public sealed class TarotData { public string sourceNote; public List<TarotCardEntry> cards = new List<TarotCardEntry>(); }
    [Serializable] public sealed class HerbData { public string sourceNote; public List<HerbEntry> herbs = new List<HerbEntry>(); }
    [Serializable] public sealed class CrystalData { public string sourceNote; public List<CrystalEntry> crystals = new List<CrystalEntry>(); }
    [Serializable] public sealed class CharmData { public string sourceNote; public List<CharmEntry> charms = new List<CharmEntry>(); }
}
