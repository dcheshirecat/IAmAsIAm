using IAmAsIAm.Core;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Safety;
using IAmAsIAm.Core.Skills;
using IAmAsIAm.Core.Story;
using IAmAsIAm.Core.Traditions;
using IAmAsIAm.Core.Triggers;
using IAmAsIAm.Runtime;
using UnityEditor;
using UnityEngine;

namespace IAmAsIAm.EditorTools
{
    /// <summary>
    /// IAmAsIAm → Validate Data. Checks every authored data file against the content rules
    /// (mandatory herb safety notes, no closed practices, gentle notes on heavy tarot cards...).
    /// It also runs automatically before every build.
    /// </summary>
    public static class DataValidationMenu
    {
        [MenuItem("IAmAsIAm/Validate Data")]
        public static void ValidateFromMenu()
        {
            var issues = DataValidator.Validate(LoadBundle());
            if (issues.Count == 0)
            {
                Debug.Log("[IAmAsIAm] ✓ All data passes content validation.");
                EditorUtility.DisplayDialog("Data validation", "All data passes content validation.", "OK");
                return;
            }
            foreach (var i in issues) Debug.LogError("[IAmAsIAm] " + i);
            EditorUtility.DisplayDialog("Data validation", $"{issues.Count} issue(s). See the Console.", "OK");
        }

        public static DataValidator.Bundle LoadBundle() => new DataValidator.Bundle
        {
            Runes = DataLoader.Load<RuneData>("runes"),
            Tarot = DataLoader.Load<TarotData>("tarot_major"),
            Herbs = DataLoader.Load<HerbData>("herbs"),
            Crystals = DataLoader.Load<CrystalData>("crystals"),
            Charms = DataLoader.Load<CharmData>("charms"),
            Skills = DataLoader.Load<SkillCatalogData>("skills"),
            Members = DataLoader.Load<SystemMembersData>("system_members"),
            Crisis = DataLoader.Load<CrisisResourceData>("crisis_resources"),
            Spreads = DataLoader.Load<TarotSpreadData>("tarot_spreads"),
            Chapters = DataLoader.Load<ChapterData>("chapters"),
            Traditions = DataLoader.Load<TraditionData>("traditions"),
            Triggers = DataLoader.Load<TriggerData>("triggers"),
        };
    }

    /// <summary>Blocks builds with invalid content.</summary>
    public sealed class DataValidationBuildStep : UnityEditor.Build.IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
        {
            var issues = DataValidator.Validate(DataValidationMenu.LoadBundle());
            if (issues.Count > 0)
                throw new UnityEditor.Build.BuildFailedException(
                    "Content validation failed:\n" + string.Join("\n", issues));
        }
    }
}
