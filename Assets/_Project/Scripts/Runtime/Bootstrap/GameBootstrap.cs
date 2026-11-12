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

        /// <summary>Destroys the running game (this object and everything on it) so a reloaded scene builds a fresh one.</summary>
        internal static void Shutdown()
        {
            if (_instance == null) return;
            var go = _instance.gameObject;
            _instance = null;
            Destroy(go);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => _instance = null;

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

            // Chapters, and who can start each one (owner's replay rule).
            var runs = new ChapterRuns(); // Unlocks come from the loaded save (SaveService), never from preferences.
            GameServices.InitializeChapters(DataLoader.Load<ChapterData>("chapters"), runs, prefs);
            GameServices.Roster.FrontChanged += change => runs.RecordFront(change.NewId);
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
