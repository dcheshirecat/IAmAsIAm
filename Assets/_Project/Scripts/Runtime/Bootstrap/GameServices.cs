using System;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Safety;
using IAmAsIAm.Core.Skills;
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

        public static int InGameDay { get; set; } = 1;
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

        public static void NotifyComfortChanged() => ComfortChanged?.Invoke();

        // Supports "Enter Play Mode Options" with domain reload disabled.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            Mind = null; Roster = null; Journal = null; Skills = null; Practices = null; Crisis = null;
            Comfort = new ComfortSettings();
            InGameDay = 1;
            IsReady = false;
            ComfortChanged = null;
            Ready = null;
        }
    }
}
