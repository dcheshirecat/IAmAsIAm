using System.IO;
using System.Runtime.CompilerServices;
using IAmAsIAm.Core;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Safety;
using IAmAsIAm.Core.Skills;
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
            Assert.That(roster.FrontingId, Is.EqualTo("wren"));
            Assert.That(roster.GetState("bramble").presentation, Is.EqualTo(MemberPresentation.Symbolic));

            var codex = new SkillCodex(b.Skills.skills);
            Assert.That(codex.Get("paced-breathing").Direction, Is.EqualTo(Core.Mind.SkillDirection.LowersArousal));
        }
    }
}
