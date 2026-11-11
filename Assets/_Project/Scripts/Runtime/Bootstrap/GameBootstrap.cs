using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Safety;
using IAmAsIAm.Core.Skills;
using IAmAsIAm.Core.Story;
using IAmAsIAm.Core.Traditions;
using IAmAsIAm.Core.Triggers;
using IAmAsIAm.Runtime.Save;
using UnityEngine;

namespace IAmAsIAm.Runtime
{
    /// <summary>
    /// Loads authored data from Resources/Data and builds the core models.
    /// Place one in the first scene (Boot). It persists across scene loads.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField, Tooltip("Initial half-width of the window of tolerance. Grows with practice.")]
        float initialWindowHalfWidth = 0.3f;

        static GameBootstrap _instance;

        void Awake()
        {
            if (_instance != null) { Destroy(gameObject); return; }
            _instance = this;
            DontDestroyOnLoad(gameObject);

            var members = DataLoader.Load<SystemMembersData>("system_members");
            var library = new PracticeLibrary(
                DataLoader.Load<RuneData>("runes"),
                DataLoader.Load<TarotData>("tarot_major"),
                DataLoader.Load<HerbData>("herbs"),
                DataLoader.Load<CrystalData>("crystals"),
                DataLoader.Load<CharmData>("charms"));

            var prefs = SaveService.LoadPreferences();

            // Story systems first, so anything listening for Ready finds them.
            GameServices.InitializeStory(
                new TriggerSystem(DataLoader.Load<TriggerData>("triggers").triggers, System.Environment.TickCount),
                new ChoiceLog(),
                new TraditionJournal(DataLoader.Load<TraditionData>("traditions").traditions),
                DataLoader.Load<TarotSpreadData>("tarot_spreads"));

            GameServices.Initialize(
                new MindModel(initialWindowHalfWidth),
                new SystemRoster(members.members, members.initialFrontId),
                new InnerJournal(),
                new SkillCodex(DataLoader.Load<SkillCatalogData>("skills").skills),
                library,
                DataLoader.Load<CrisisResourceData>("crisis_resources"),
                prefs.comfort);
        }
    }

    /// <summary>Loads JSON TextAssets from Resources/Data.</summary>
    public static class DataLoader
    {
        public static T Load<T>(string name) where T : new()
        {
            var asset = Resources.Load<TextAsset>("Data/" + name);
            if (asset == null)
            {
                Debug.LogError($"[IAmAsIAm] Missing data file Resources/Data/{name}.json");
                return new T();
            }
            return JsonUtility.FromJson<T>(asset.text);
        }
    }
}
