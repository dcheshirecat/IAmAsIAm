using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.PluralSystem
{
    /// <summary>One step of an alter's own practice (data-driven, same shape as a Wiccan ritual's steps).</summary>
    [Serializable]
    public sealed class OwnPracticeStep
    {
        /// <summary>Arrive, Prepare, Moment, Reflect or Close.</summary>
        public string phase;
        /// <summary>breathe, hold, tap, choose or write.</summary>
        public string kind;
        public string text;
        public string action;
        public float seconds;
        public List<string> options = new List<string>();
    }

    /// <summary>
    /// A small practice of an alter's own (owner, 2026-11-12): their own faith or way of being, never Maya's Wiccan tools.
    /// Jacob's rosary, Nina's and Lily's Quaker quiet, Heather's candle (her faith stays a mystery all game), Serena's walks,
    /// Axel's loud music. Each opens as Maya gets to know that alter better (<see cref="level"/> 1-3).
    /// </summary>
    [Serializable]
    public sealed class OwnPractice
    {
        public string id;
        public string memberId;
        public string name;
        public int level = 1;
        public float grounding = 0.2f;
        public List<OwnPracticeStep> steps = new List<OwnPracticeStep>();
    }

    [Serializable]
    public sealed class OwnPracticeData
    {
        /// <summary>How well Maya must know an alter for each level to open (level 1, 2, 3).</summary>
        public List<int> levelThresholds = new List<int> { 2, 5, 9 };
        public List<OwnPractice> practices = new List<OwnPractice>();
    }

    [Serializable]
    public sealed class KnownState
    {
        public string memberId;
        public int known;
    }

    /// <summary>
    /// How well Maya (the system) knows each alter (owner: their practices "have to be unlocked by getting to know the alter
    /// better and better"). It grows when they front or share time at the start of a chapter part, when they do one of their
    /// own practices, and when the story says so (Ink get_to_know). Never shown as a number. Pure rules.
    /// </summary>
    public sealed class AlterBonds
    {
        readonly OwnPracticeData _data;
        readonly Dictionary<string, int> _known = new Dictionary<string, int>();

        public AlterBonds(OwnPracticeData data) { _data = data ?? new OwnPracticeData(); }

        public event Action Changed;

        public int Known(string memberId) => memberId != null && _known.TryGetValue(memberId, out var k) ? k : 0;

        public void GetToKnow(string memberId, int amount = 1)
        {
            if (string.IsNullOrEmpty(memberId) || amount == 0) return;
            _known[memberId] = Math.Max(0, Known(memberId) + amount);
            Changed?.Invoke();
        }

        /// <summary>The level of practice open for this alter (0 = none yet).</summary>
        public int Level(string memberId)
        {
            int k = Known(memberId), level = 0;
            var t = _data.levelThresholds ?? new List<int>();
            for (int i = 0; i < t.Count; i++) if (k >= t[i]) level = i + 1;
            return level;
        }

        /// <summary>This alter's own practices that are open now, in order.</summary>
        public List<OwnPractice> Unlocked(string memberId)
        {
            int level = Level(memberId);
            return (_data.practices ?? new List<OwnPractice>())
                .Where(p => p.memberId == memberId && p.level <= level).OrderBy(p => p.level).ToList();
        }

        public OwnPractice Practice(string id) => (_data.practices ?? new List<OwnPractice>()).FirstOrDefault(p => p.id == id);

        public List<KnownState> Export() => _known.Select(kv => new KnownState { memberId = kv.Key, known = kv.Value }).ToList();

        public void Restore(IEnumerable<KnownState> states)
        {
            _known.Clear();
            foreach (var s in states ?? Enumerable.Empty<KnownState>())
                if (s != null && !string.IsNullOrEmpty(s.memberId)) _known[s.memberId] = Math.Max(0, s.known);
            Changed?.Invoke();
        }
    }
}
