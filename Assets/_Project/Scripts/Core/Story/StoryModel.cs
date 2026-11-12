using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.Story
{
    /// <summary>
    /// One chapter (one religion per chapter, fixed order). Authored in Resources/Data/chapters.json.
    /// The story itself is written in Ink (Assets/_Project/Story); this is the metadata the
    /// game needs around it: content notes, featured part, which Ink knot to start at.
    /// </summary>
    [Serializable]
    public sealed class ChapterDefinition
    {
        public string id;
        public int number;
        public string sabbat;
        public string title;
        /// <summary>The Ink knot this chapter starts at.</summary>
        public string inkKnot;
        public string featuredMemberId;
        /// <summary>
        /// Who the FIRST playthrough always starts as (owner: Maya up to Chapter 1; Axel for Chapter 1.5). Once the chapter is
        /// finished it can be replayed as any alter unlocked so far (ChapterRuns).
        /// </summary>
        public string startingMemberId;
        /// <summary>The one alter finishing this chapter unlocks (owner: set per chapter, never random). Empty = none.</summary>
        public string unlocksMemberId;
        /// <summary>Where and when, e.g. "Iowa City, Iowa; Maya's childhood (the body was born in 1998)".</summary>
        public string setting;
        public string skillFamily;
        /// <summary>Content tags (see Safety.ContentTags). Shown as content warnings before EVERY chapter (owner's rule); required.</summary>
        public List<string> contentTags = new List<string>();
        /// <summary>Short summary shown in place of skipped content, and on the chapter select.</summary>
        public string summary;
    }

    [Serializable]
    public sealed class ChapterData
    {
        public List<ChapterDefinition> chapters = new List<ChapterDefinition>();
    }

    /// <summary>A meaningful choice the player made. These are shown in the end-of-chapter recap.</summary>
    [Serializable]
    public sealed class ChoiceRecord
    {
        /// <summary>Stable id for the decision point, e.g. "samhain.note".</summary>
        public string decisionId;
        public string chapterId;
        /// <summary>Stable id for the option taken, e.g. "kept-note".</summary>
        public string optionId;
        /// <summary>One line for the recap, e.g. "You kept the note instead of throwing it away."</summary>
        public string summary;
        /// <summary>Who was fronting when the choice was made. In a plural system, that matters.</summary>
        public string madeById;
    }

    /// <summary>
    /// The choices-and-consequences record of a narrative adventure. Ink owns moment-to-moment
    /// story state. This holds the decisions the game remembers across chapters and shows in
    /// recaps. Re-deciding a decision replaces the old answer (e.g. after a chapter replay).
    /// </summary>
    public sealed class ChoiceLog
    {
        readonly List<ChoiceRecord> _records = new List<ChoiceRecord>();

        public IReadOnlyList<ChoiceRecord> Records => _records;
        public event Action<ChoiceRecord> Recorded;

        public void Record(ChoiceRecord record)
        {
            if (record == null || string.IsNullOrEmpty(record.decisionId) || string.IsNullOrEmpty(record.optionId))
                throw new ArgumentException("A choice needs a decisionId and an optionId.");
            _records.RemoveAll(r => r.decisionId == record.decisionId);
            _records.Add(record);
            Recorded?.Invoke(record);
        }

        public string OptionFor(string decisionId) => _records.LastOrDefault(r => r.decisionId == decisionId)?.optionId;
        public bool Chose(string decisionId, string optionId) => OptionFor(decisionId) == optionId;
        public IEnumerable<ChoiceRecord> ForChapter(string chapterId) => _records.Where(r => r.chapterId == chapterId);

        public void Restore(IEnumerable<ChoiceRecord> records)
        {
            _records.Clear();
            if (records != null) _records.AddRange(records);
        }
    }
}
