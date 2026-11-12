using System.IO;
using System.Runtime.CompilerServices;
using IAmAsIAm.Core;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Safety;
using IAmAsIAm.Core.Skills;
using IAmAsIAm.Core.Story;
using IAmAsIAm.Core.Traditions;
using IAmAsIAm.Core.Triggers;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    /// <summary>
    /// Loads the shipped JSON data and runs the content validator over it.
    /// Works both in the Unity Test Runner (JsonUtility) and in the standalone .NET
    /// harness in tools/CoreVerify (System.Text.Json), so CI can run it without a Unity licence.
    /// </summary>
    public class RealDataTests
    {
        static string DataDir([CallerFilePath] string thisFile = "") =>
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(thisFile), "..", "..", "Resources", "Data"));

        static T Load<T>(string fileName)
        {
            string json = File.ReadAllText(Path.Combine(DataDir(), fileName));
#if UNITY_5_3_OR_NEWER
            return UnityEngine.JsonUtility.FromJson<T>(json);
#else
            return System.Text.Json.JsonSerializer.Deserialize<T>(json,
                new System.Text.Json.JsonSerializerOptions { IncludeFields = true });
#endif
        }

        static DataValidator.Bundle LoadAll() => new DataValidator.Bundle
        {
            Runes = Load<RuneData>("runes.json"),
            Tarot = Load<TarotData>("tarot_major.json"),
            Herbs = Load<HerbData>("herbs.json"),
            Crystals = Load<CrystalData>("crystals.json"),
            Charms = Load<CharmData>("charms.json"),
            Skills = Load<SkillCatalogData>("skills.json"),
            Members = Load<SystemMembersData>("system_members.json"),
            Crisis = Load<CrisisResourceData>("crisis_resources.json"),
            Spreads = Load<TarotSpreadData>("tarot_spreads.json"),
            Chapters = Load<ChapterData>("chapters.json"),
            Traditions = Load<TraditionData>("traditions.json"),
            Triggers = Load<TriggerData>("triggers.json"),
        };

        [Test]
        public void ShippedData_PassesContentValidation()
        {
            var issues = DataValidator.Validate(LoadAll());
            Assert.That(issues, Is.Empty, string.Join("\n", issues));
        }

        [Test]
        public void ShippedData_BuildsLibraryAndRoster()
        {
            var b = LoadAll();
            var library = new PracticeLibrary(b.Runes, b.Tarot, b.Herbs, b.Crystals, b.Charms);
            Assert.That(library.AnchorsFor("Touch"), Is.Not.Empty);

            var roster = new SystemRoster(b.Members.members, b.Members.initialFrontId);
            Assert.That(roster.FrontingId, Is.EqualTo("maya"));
            Assert.That(roster.GetState("theresa").presentation, Is.EqualTo(MemberPresentation.Symbolic));

            var codex = new SkillCodex(b.Skills.skills);
            Assert.That(codex.Get("paced-breathing").Direction, Is.EqualTo(Core.Mind.SkillDirection.LowersArousal));

            var traditions = new TraditionJournal(b.Traditions.traditions);
            Assert.That(traditions.Get("humanism"), Is.Not.Null, "Choosing no religion must always be a path.");
            Assert.That(traditions.TryPractice("indigenous-closed", "anything"), Is.False);
        }

        [Test]
        public void Places_HaveNamesCitiesAndMapPositions()
        {
            var data = Load<PlaceData>("places.json");
            Assert.That(data.places.Count, Is.GreaterThan(0));
            var ids = new System.Collections.Generic.HashSet<string>();
            foreach (var p in data.places)
            {
                Assert.That(ids.Add(p.id), Is.True, $"place '{p.id}' is listed twice");
                if (p.id == "black") continue;
                Assert.That(string.IsNullOrEmpty(p.name), Is.False, $"place '{p.id}' needs a name");
                if (string.IsNullOrEmpty(p.city)) continue;
                Assert.That(data.City(p.city), Is.Not.Null, $"place '{p.id}': unknown city '{p.city}'");
                Assert.That(p.x >= 0f && p.x <= 1f && p.y >= 0f && p.y <= 1f, Is.True, $"place '{p.id}': map position must be 0..1");
            }
        }
        [Test]
        public void Craft_PracticesHaveStepsAndOnlyRealMembersPractise()
        {
            var craft = Load<CraftData>("craft.json");
            var members = new System.Collections.Generic.HashSet<string>(System.Linq.Enumerable.Select(Load<SystemMembersData>("system_members.json").members, m => m.id));
            var ids = new System.Collections.Generic.HashSet<string>();
            Assert.That(craft.practices.Count, Is.GreaterThan(0));
            foreach (var p in craft.practices)
            {
                Assert.That(ids.Add(p.id), Is.True, $"practice '{p.id}' is listed twice");
                Assert.That(string.IsNullOrEmpty(p.name), Is.False, $"practice '{p.id}' needs a name");
                Assert.That(p.steps.Count, Is.GreaterThan(0), $"practice '{p.id}' needs its steps (they go in the Book of Shadows)");
            }
            // Owner: only Maya and Izzy practise Wicca.
            Assert.That(craft.wiccanMemberIds, Is.EquivalentTo(new[] { "maya", "izzy" }));
            foreach (var r in craft.refusals) Assert.That(members.Contains(r.memberId), Is.True, $"refusal for unknown member '{r.memberId}'");
            foreach (var m in members)
                if (!craft.wiccanMemberIds.Contains(m) && m != "theresa" && m != "holden")
                    Assert.That(craft.refusals.Exists(r => r.memberId == m), Is.True, $"'{m}' doesn't practise Wicca: give them a line for the closed Book");

            var runes = Load<RuneData>("runes.json").runes.ConvertAll(r => r.id);
            var herbs = Load<HerbData>("herbs.json").herbs.ConvertAll(h => h.id);
            var stones = Load<CrystalData>("crystals.json").crystals.ConvertAll(c => c.id);
            foreach (var id in craft.hardRuneIds) Assert.That(runes, Has.Member(id), $"hard rune '{id}'");
            foreach (var id in craft.charmHerbIds) Assert.That(herbs, Has.Member(id), $"charm herb '{id}'");
            foreach (var id in craft.charmStoneIds) Assert.That(stones, Has.Member(id), $"charm stone '{id}'");
            foreach (var id in System.Linq.Enumerable.Concat(craft.charmRemoverIds, craft.charmAdderIds)) Assert.That(members.Contains(id), Is.True, id);
            Assert.That(craft.charmOwnThings.Count, Is.GreaterThan(0));
            // Owner: every herb and stone has a researched meaning; stones also have their history.
            foreach (var h in Load<HerbData>("herbs.json").herbs) Assert.That(string.IsNullOrWhiteSpace(h.charmMeaning), Is.False, $"herb '{h.id}' has no charmMeaning");
            foreach (var c in Load<CrystalData>("crystals.json").crystals)
            {
                Assert.That(string.IsNullOrWhiteSpace(c.charmMeaning), Is.False, $"stone '{c.id}' has no charmMeaning");
                Assert.That(string.IsNullOrWhiteSpace(c.history), Is.False, $"stone '{c.id}' has no history");
            }
            Assert.That(craft.charmAdditions.Count, Is.GreaterThan(0));
            foreach (var id in RitualSession.Supported) Assert.That(craft.practices.Exists(p => p.id == id), Is.True, $"ritual '{id}' has no practice in craft.json");
        }
    }
}
