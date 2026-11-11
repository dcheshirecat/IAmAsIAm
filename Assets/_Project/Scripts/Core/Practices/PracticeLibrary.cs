using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.Practices
{
    /// <summary>
    /// All practice lore, loaded once at boot. Pure data access: the Runtime layer handles
    /// loading JSON and passes the parsed objects in.
    /// </summary>
    public sealed class PracticeLibrary
    {
        public IReadOnlyList<RuneEntry> Runes { get; }
        public IReadOnlyList<TarotCardEntry> Tarot { get; }
        public IReadOnlyList<HerbEntry> Herbs { get; }
        public IReadOnlyList<CrystalEntry> Crystals { get; }
        public IReadOnlyList<CharmEntry> Charms { get; }

        readonly Dictionary<string, ISensoryAnchor> _anchors = new Dictionary<string, ISensoryAnchor>();

        public PracticeLibrary(RuneData runes, TarotData tarot, HerbData herbs, CrystalData crystals, CharmData charms)
        {
            Runes = runes?.runes ?? new List<RuneEntry>();
            Tarot = tarot?.cards ?? new List<TarotCardEntry>();
            Herbs = herbs?.herbs ?? new List<HerbEntry>();
            Crystals = crystals?.crystals ?? new List<CrystalEntry>();
            Charms = charms?.charms ?? new List<CharmEntry>();

            foreach (var a in Herbs.Cast<ISensoryAnchor>().Concat(Crystals).Concat(Charms))
            {
                if (_anchors.ContainsKey(a.Id))
                    throw new InvalidOperationException($"Duplicate practice item id '{a.Id}'. Ids must be unique across herbs, crystals and charms.");
                _anchors.Add(a.Id, a);
            }
        }

        public ISensoryAnchor GetAnchor(string id) => _anchors.TryGetValue(id, out var a) ? a : null;

        public IEnumerable<ISensoryAnchor> AnchorsFor(string senseName) =>
            _anchors.Values.Where(a => a.AnchorSenses.Any(s => string.Equals(s, senseName, StringComparison.OrdinalIgnoreCase)));

        public RuneEntry GetRune(string id) => Runes.FirstOrDefault(r => r.id == id);
        public TarotCardEntry GetCard(string id) => Tarot.FirstOrDefault(c => c.id == id);
    }
}
