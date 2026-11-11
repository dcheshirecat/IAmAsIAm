using System.Collections.Generic;
using System.Linq;
using IAmAsIAm.Core.Grounding;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Safety;
using IAmAsIAm.Core.Skills;

namespace IAmAsIAm.Core
{
    /// <summary>
    /// Enforces the content rules in ContentAndSafetyGuidelines.md on authored data.
    /// It runs from the Unity menu (IAmAsIAm → Validate Data) and in tests.
    /// </summary>
    public static class DataValidator
    {
        public sealed class Bundle
        {
            public RuneData Runes;
            public TarotData Tarot;
            public HerbData Herbs;
            public CrystalData Crystals;
            public CharmData Charms;
            public SkillCatalogData Skills;
            public SystemMembersData Members;
            public CrisisResourceData Crisis;
        }

        public static List<string> Validate(Bundle b)
        {
            var issues = new List<string>();

            // Runes
            var runes = b.Runes?.runes ?? new List<RuneEntry>();
            if (runes.Count != 24) issues.Add($"runes: expected 24 Elder Futhark runes, found {runes.Count}.");
            CheckUnique(issues, "runes", runes.Select(r => r.id));
            foreach (var r in runes)
            {
                if (string.IsNullOrEmpty(r.glyph)) issues.Add($"rune '{r.id}': missing glyph.");
                if (r.aett < 1 || r.aett > 3) issues.Add($"rune '{r.id}': aett must be 1–3.");
                if (string.IsNullOrWhiteSpace(r.reflectionPrompt)) issues.Add($"rune '{r.id}': missing reflection prompt.");
            }
            for (int aett = 1; aett <= 3; aett++)
            {
                int n = runes.Count(r => r.aett == aett);
                if (runes.Count == 24 && n != 8) issues.Add($"runes: ætt {aett} should have 8 runes, has {n}.");
            }

            // Tarot
            var cards = b.Tarot?.cards ?? new List<TarotCardEntry>();
            CheckUnique(issues, "tarot", cards.Select(c => c.id));
            var majors = cards.Where(c => c.arcana == "Major").ToList();
            if (majors.Count != 22) issues.Add($"tarot: expected 22 Major Arcana, found {majors.Count}.");
            if (majors.Select(c => c.number).Distinct().Count() != majors.Count) issues.Add("tarot: duplicate Major Arcana numbers.");
            foreach (var c in cards)
                if (string.IsNullOrWhiteSpace(c.reflectionPrompt)) issues.Add($"tarot '{c.id}': missing reflection prompt.");
            foreach (var heavy in new[] { "Death", "The Tower", "The Devil" })
            {
                var card = majors.FirstOrDefault(c => c.name == heavy);
                if (card != null && string.IsNullOrWhiteSpace(card.gentleNote))
                    issues.Add($"tarot '{card.id}': heavy cards must carry a gentleNote.");
            }

            // Herbs: safety notes are mandatory.
            var herbs = b.Herbs?.herbs ?? new List<HerbEntry>();
            foreach (var h in herbs)
            {
                if (h.safetyNotes == null || h.safetyNotes.Count == 0) issues.Add($"herb '{h.id}': safety notes are mandatory.");
                if (string.IsNullOrWhiteSpace(h.latinName)) issues.Add($"herb '{h.id}': missing Latin name.");
                CheckSenses(issues, $"herb '{h.id}'", h.anchorSenses);
            }
            if (herbs.Any(h => h.latinName != null && h.latinName.Contains("apiana")))
                issues.Add("herbs: white sage (Salvia apiana) is excluded by the content guidelines (closed practice).");
            var sjw = herbs.FirstOrDefault(h => h.id == "st-johns-wort");
            if (sjw != null && !sjw.safetyNotes.Any(n => n.ToLowerInvariant().Contains("antidepressant")))
                issues.Add("herb 'st-johns-wort': must warn about antidepressant interactions.");

            // Crystals & charms
            var crystals = b.Crystals?.crystals ?? new List<CrystalEntry>();
            foreach (var c in crystals)
            {
                if (string.IsNullOrWhiteSpace(c.mineral)) issues.Add($"crystal '{c.id}': mineral identity is required.");
                CheckSenses(issues, $"crystal '{c.id}'", c.anchorSenses);
            }
            var charms = b.Charms?.charms ?? new List<CharmEntry>();
            foreach (var c in charms)
            {
                if (string.IsNullOrWhiteSpace(c.origin)) issues.Add($"charm '{c.id}': origin is required (open practices only).");
                CheckSenses(issues, $"charm '{c.id}'", c.anchorSenses);
            }

            var practiceIds = herbs.Select(h => h.id).Concat(crystals.Select(c => c.id)).Concat(charms.Select(c => c.id)).ToList();
            CheckUnique(issues, "practice items (herbs + crystals + charms)", practiceIds);

            // Skills
            var skills = b.Skills?.skills ?? new List<SkillEntry>();
            CheckUnique(issues, "skills", skills.Select(s => s.id));
            foreach (var s in skills)
            {
                if (!EnumText.IsValid<SkillFamily>(s.family)) issues.Add($"skill '{s.id}': unknown family '{s.family}'.");
                if (!EnumText.IsValid<SkillDirection>(s.direction)) issues.Add($"skill '{s.id}': unknown direction '{s.direction}'.");
                if (s.steps == null || s.steps.Count == 0) issues.Add($"skill '{s.id}': needs steps.");
                if (string.IsNullOrWhiteSpace(s.source)) issues.Add($"skill '{s.id}': needs a source.");
            }

            // System members
            var members = b.Members?.members ?? new List<SystemMemberDefinition>();
            CheckUnique(issues, "system members", members.Select(m => m.id));
            if (b.Members != null && members.All(m => m.id != b.Members.initialFrontId))
                issues.Add($"system members: initialFrontId '{b.Members.initialFrontId}' is not a member.");
            var practiceSet = new HashSet<string>(practiceIds);
            foreach (var m in members)
            {
                if (!EnumText.IsValid<MemberPresentation>(m.initialPresentation))
                    issues.Add($"member '{m.id}': unknown presentation '{m.initialPresentation}'.");
                foreach (var item in m.comfortItemIds ?? new List<string>())
                    if (!practiceSet.Contains(item)) issues.Add($"member '{m.id}': comfort item '{item}' doesn't exist.");
            }

            // Crisis resources
            var crisis = b.Crisis?.resources ?? new List<CrisisResource>();
            if (!crisis.Any(r => r.region == "INTL")) issues.Add("crisis resources: an international fallback (region 'INTL') is required.");
            foreach (var r in crisis)
                if (string.IsNullOrWhiteSpace(r.url) && string.IsNullOrWhiteSpace(r.contact))
                    issues.Add($"crisis resource '{r.name}': needs a contact or URL.");

            return issues;
        }

        static void CheckUnique(List<string> issues, string label, IEnumerable<string> ids)
        {
            foreach (var g in ids.GroupBy(i => i).Where(g => g.Count() > 1))
                issues.Add($"{label}: duplicate id '{g.Key}'.");
        }

        static void CheckSenses(List<string> issues, string label, IEnumerable<string> senses)
        {
            foreach (var s in senses ?? Enumerable.Empty<string>())
                if (!EnumText.IsValid<Sense>(s)) issues.Add($"{label}: unknown anchor sense '{s}'.");
        }
    }
}
