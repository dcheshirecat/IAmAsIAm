using System;
using System.Collections.Generic;

namespace IAmAsIAm.Core.Traditions
{
    /// <summary>
    /// How a tradition relates to outsiders. This decides what the player may try in the game
    /// versus only learn about. See docs/TraditionsAndTeacher.md.
    /// </summary>
    public enum Openness
    {
        /// <summary>Anyone may learn, visit and take part (e.g. Quaker worship, Druidry, Buddhist meditation).</summary>
        OpenToAll,
        /// <summary>Visitors are welcome to learn and attend, but some practices are for members only.</summary>
        WelcomeToLearnAndVisit,
        /// <summary>Initiatory, oathbound or closed. The game teaches respectfully ABOUT it and never stages its practices.</summary>
        ClosedOrInitiatory,
    }

    /// <summary>
    /// A practice the player can do with the spiritual teacher (or a guest). Only allowed where
    /// the tradition welcomes non-members doing it.
    /// </summary>
    [Serializable]
    public sealed class TryablePractice
    {
        public string id;
        public string name;
        public string description;
        public List<string> steps = new List<string>();
        /// <summary>Must be true. The validator rejects anything else, so it's explicit in the data.</summary>
        public bool appropriateForNonMembers;
        /// <summary>Why it's appropriate, or how to do it respectfully.</summary>
        public string respectNote;
        /// <summary>Mental-health cautions (e.g. trauma-sensitive adaptations for meditation).</summary>
        public List<string> cautions = new List<string>();
    }

    /// <summary>
    /// A real practitioner's words. These come ONLY from consented interviews. Slots marked
    /// "awaiting-interview" ship without text and are never invented. See docs/LivedExperienceInterviews.md.
    /// </summary>
    [Serializable]
    public sealed class LivedExperience
    {
        public string id;
        /// <summary>The topic the interview will cover, e.g. "Finding the tradition as a plural person".</summary>
        public string topic;
        /// <summary>"awaiting-interview", "in-review" or "approved".</summary>
        public string status = "awaiting-interview";
        /// <summary>Reference to the signed consent record (kept outside the repo).</summary>
        public string consentRef;
        /// <summary>How the contributor wants to be credited (or "anonymous").</summary>
        public string credit;
        public string text;
    }

    [Serializable]
    public sealed class TraditionEntry
    {
        public string id;
        public string name;
        /// <summary>A broad grouping for the codex, e.g. "Abrahamic", "Dharmic", "Modern Pagan", "Non-religious".</summary>
        public string family;
        public string overview;
        public List<string> coreBeliefs = new List<string>();
        public List<string> keyPractices = new List<string>();
        public List<string> textsAndSources = new List<string>();
        public List<string> holyDays = new List<string>();
        /// <summary>An <see cref="Openness"/> name.</summary>
        public string openness;
        public string opennessNote;
        public List<string> visitorEtiquette = new List<string>();
        public List<TryablePractice> tryInGame = new List<TryablePractice>();
        /// <summary>Accurate notes on how the tradition meets mental health, trauma and dissociation.</summary>
        public List<string> mentalHealthNotes = new List<string>();
        public List<string> commonMisconceptions = new List<string>();
        public List<LivedExperience> livedExperiences = new List<LivedExperience>();
        /// <summary>Reference sources for fact-checking. At least one is required.</summary>
        public List<string> references = new List<string>();

        public Openness Openness => EnumText.Parse(openness, Openness.WelcomeToLearnAndVisit);
    }

    [Serializable]
    public sealed class TraditionData
    {
        public string sourceNote;
        public List<TraditionEntry> traditions = new List<TraditionEntry>();
    }

    /// <summary>Which traditions the player has explored with the teacher. Exploring never commits the character to anything.</summary>
    public sealed class TraditionJournal
    {
        readonly Dictionary<string, TraditionEntry> _all = new Dictionary<string, TraditionEntry>();
        readonly HashSet<string> _explored = new HashSet<string>();
        readonly HashSet<string> _tried = new HashSet<string>();

        public TraditionJournal(IEnumerable<TraditionEntry> traditions)
        {
            foreach (var t in traditions) _all[t.id] = t;
        }

        public IEnumerable<TraditionEntry> All => _all.Values;
        public IReadOnlyCollection<string> ExploredIds => _explored;
        public IReadOnlyCollection<string> TriedPracticeIds => _tried;
        public TraditionEntry Get(string id) => _all.TryGetValue(id, out var t) ? t : null;

        public bool Explore(string id) => _all.ContainsKey(id) && _explored.Add(id);

        /// <summary>
        /// Records that the player tried a practice. It returns false (and does nothing) for a
        /// practice that isn't open to non-members, which is defence in depth on top of the validator.
        /// </summary>
        public bool TryPractice(string traditionId, string practiceId)
        {
            var t = Get(traditionId);
            if (t == null || t.Openness == Openness.ClosedOrInitiatory) return false;
            var p = t.tryInGame.Find(x => x.id == practiceId);
            if (p == null || !p.appropriateForNonMembers) return false;
            _explored.Add(traditionId);
            _tried.Add(practiceId);
            return true;
        }

        public void Restore(IEnumerable<string> explored, IEnumerable<string> tried)
        {
            _explored.Clear();
            _tried.Clear();
            if (explored != null) foreach (var e in explored) _explored.Add(e);
            if (tried != null) foreach (var t in tried) _tried.Add(t);
        }
    }
}
