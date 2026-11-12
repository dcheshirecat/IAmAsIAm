using System;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Save;
using IAmAsIAm.Core.Safety;
using IAmAsIAm.Core.Skills;
using IAmAsIAm.Core.Story;
using IAmAsIAm.Core.Traditions;
using IAmAsIAm.Core.Triggers;
using UnityEngine;

namespace IAmAsIAm.Runtime
{
    /// <summary>
    /// The single place that holds the game's core models. It is deliberately minimal: a typed
    /// holder, not a DI framework. Populated by <see cref="GameBootstrap"/>.
    /// </summary>
    public static class GameServices
    {
        public static MindModel Mind { get; private set; }
        public static SystemRoster Roster { get; private set; }
        public static InnerJournal Journal { get; private set; }
        public static SkillCodex Skills { get; private set; }
        public static PracticeLibrary Practices { get; private set; }
        public static CrisisResourceData Crisis { get; private set; }
        public static ComfortSettings Comfort { get; private set; } = new ComfortSettings();
        public static TriggerSystem Triggers { get; private set; }
        public static ChoiceLog Choices { get; private set; }
        public static TraditionJournal Traditions { get; private set; }
        public static TarotSpreadData Spreads { get; private set; }
        public static ChapterData Chapters { get; private set; }
        public static ChapterRuns ChapterRuns { get; private set; }
        public static PreferencesData Preferences { get; private set; }
        /// <summary>Maya's family computer, phone and laptop, with her texts and emails (saved per slot).</summary>
        public static IAmAsIAm.Core.Devices.Devices Devices { get; private set; } = new IAmAsIAm.Core.Devices.Devices();

        public static int InGameDay { get; set; } = 1;
        /// <summary>Time played in the active save slot (seconds, real time). Shown on the save screen.</summary>
        public static float PlaySeconds { get; set; }
        public static bool IsReady { get; private set; }

        /// <summary>Raised whenever comfort settings change, so presentation can re-apply caps.</summary>
        public static event Action ComfortChanged;
        public static event Action Ready;

        internal static void Initialize(MindModel mind, SystemRoster system, InnerJournal journal,
            SkillCodex skills, PracticeLibrary practices, CrisisResourceData crisis, ComfortSettings comfort)
        {
            Mind = mind;
            Roster = system;
            Journal = journal;
            Skills = skills;
            Practices = practices;
            Crisis = crisis;
            Comfort = comfort ?? new ComfortSettings();
            IsReady = true;
            Ready?.Invoke();
        }

        internal static void InitializeStory(TriggerSystem triggers, ChoiceLog choices, TraditionJournal traditions, TarotSpreadData spreads)
        {
            Triggers = triggers;
            Choices = choices;
            Traditions = traditions;
            Spreads = spreads;
        }

        internal static void InitializeChapters(ChapterData chapters, ChapterRuns runs, PreferencesData prefs)
        {
            Chapters = chapters ?? new ChapterData();
            ChapterRuns = runs;
            Preferences = prefs ?? new PreferencesData();
        }

        public static ChapterDefinition Chapter(string id) => Chapters?.chapters?.Find(c => c.id == id);

        /// <summary>Saves preferences (comfort settings). Chapter unlocks are per save, in SaveData.</summary>
        public static void SavePreferences()
        {
            if (Preferences == null) return;
            Preferences.comfort = Comfort;
            IAmAsIAm.Runtime.Save.SaveService.SavePreferences(Preferences);
        }

        public static void NotifyComfortChanged() => ComfortChanged?.Invoke();

        // Supports "Enter Play Mode Options" with domain reload disabled. Also used to restart the game
        // with a fresh state when switching save slots (GameSession.Restart).
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        internal static void ResetStatics()
        {
            Mind = null; Roster = null; Journal = null; Skills = null; Practices = null; Crisis = null;
            Comfort = new ComfortSettings();
            Triggers = null; Choices = null; Traditions = null; Spreads = null;
            Chapters = null; ChapterRuns = null; Preferences = null;
            Devices = new IAmAsIAm.Core.Devices.Devices();
            InGameDay = 1;
            PlaySeconds = 0f;
            IsReady = false;
            ComfortChanged = null;
            Ready = null;
        }
    }
}
