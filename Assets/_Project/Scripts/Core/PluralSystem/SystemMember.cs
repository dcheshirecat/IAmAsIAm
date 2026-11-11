using System;
using System.Collections.Generic;

namespace IAmAsIAm.Core.PluralSystem
{
    /// <summary>
    /// How a part is currently represented. Parts can move from Symbolic to Embodied as trust
    /// grows. Theresa and Holden, for example, are mostly symbolic. See GameDesignDocument.md §3–4.
    /// </summary>
    public enum MemberPresentation
    {
        /// <summary>A full character: can be met, spoken with, and (with consent) can front.</summary>
        Embodied,
        /// <summary>Represented through the environment: a voice, an image, weather, birdsong.</summary>
        Symbolic,
    }

    /// <summary>
    /// Authored, static definition of a system member (headmate/part/alter).
    /// Serializable so it can come from JSON or be wrapped in a ScriptableObject.
    /// </summary>
    [Serializable]
    public sealed class SystemMemberDefinition
    {
        public string id;
        public string displayName;
        public string pronouns;

        /// <summary>The age this member feels (owner's words, e.g. "26"). Empty when they have none.</summary>
        public string age;

        /// <summary>This member's relationship with faith (owner's words, e.g. "ex-Quaker, current Wiccan").</summary>
        public string faith;

        /// <summary>Kept hidden from the character (the other parts) until later in the story.</summary>
        public bool hiddenUntilLater;

        /// <summary>A free-text role ("caretaker", "protector", "little"). Descriptive, never a stereotype.</summary>
        public string role;
        public string description;

        /// <summary>Hex colour used for this member's UI tint and co-consciousness edge glow.</summary>
        public string colorHex = "#FFFFFF";

        /// <summary>A <see cref="MemberPresentation"/> name.</summary>
        public string initialPresentation = nameof(MemberPresentation.Embodied);

        public MemberPresentation InitialPresentation => EnumText.Parse(initialPresentation, MemberPresentation.Embodied);

        /// <summary>
        /// Tags for what this member perceives that others may not, e.g. "glyphs", "birdsong",
        /// "hidden-paths". These drive <c>PerceptionGated</c> objects in the Runtime layer.
        /// </summary>
        public List<string> perceptionTraits = new List<string>();

        /// <summary>Practice item ids (herbs, stones, charms) this member finds comforting.</summary>
        public List<string> comfortItemIds = new List<string>();

        /// <summary>Trust needed before this member is willing to front when asked (0..1).</summary>
        public float trustToFront = 0.5f;

        /// <summary>Whether this member may ever be playable. Some parts simply don't front.</summary>
        public bool canFront = true;
    }

    /// <summary>Mutable, saved state for one member.</summary>
    [Serializable]
    public sealed class SystemMembersData
    {
        public List<SystemMemberDefinition> members = new List<SystemMemberDefinition>();
        public string initialFrontId;
    }

    [Serializable]
    public sealed class SystemMemberState
    {
        public string id;
        public float trust;
        public MemberPresentation presentation;

        /// <summary>When true, the member has asked not to front for now. This always wins.</summary>
        public bool restingByChoice;
    }
}
