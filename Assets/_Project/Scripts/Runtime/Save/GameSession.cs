using System;
using System.Collections;
using IAmAsIAm.Runtime.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IAmAsIAm.Runtime.Save
{
    /// <summary>
    /// Starting, switching and leaving games. Each save slot is its own game, so switching slots never mixes
    /// state: the current game is saved, torn down, the scene reloads with a fresh state, and the chosen slot is
    /// loaded into it (or a new game begins). The tarot loading screen covers the whole change.
    /// </summary>
    public static class GameSession
    {
        /// <summary>What to do once the scene has reloaded. Null = show the title screen.</summary>
        public sealed class StartRequest
        {
            public int Slot;
            public bool NewGame;
        }

        static StartRequest _pending;

        public static bool IsRestarting { get; private set; }

        /// <summary>Taken once by <see cref="SaveSlotsScreen"/> when the reloaded scene starts.</summary>
        internal static StartRequest TakePending()
        {
            var p = _pending;
            _pending = null;
            return p;
        }

        public static void LoadSlot(int slot) => Restart(new StartRequest { Slot = slot });
        public static void NewGame(int slot) => Restart(new StartRequest { Slot = slot, NewGame = true });
        public static void ToTitle() => Restart(null);

        /// <summary>
        /// From the title screen the state is still fresh, so the slot loads in place (no reload), behind the
        /// loading screen. <paramref name="apply"/> loads or begins the game and returns false if it couldn't.
        /// </summary>
        public static void BeginFromTitle(Func<bool> apply, Action onFailed)
        {
            LoadingScreen.Run(BeginRoutine(apply, onFailed));
        }

        static IEnumerator BeginRoutine(Func<bool> apply, Action onFailed)
        {
            LoadingScreen.Show();
            yield return null; // Let the card appear before the work starts.
            bool ok = false;
            try { ok = apply(); }
            catch (Exception e) { Debug.LogError("[IAmAsIAm] Starting the game failed: " + e); }
            if (!ok) onFailed?.Invoke();
            LoadingScreen.Hide();
        }

        static void Restart(StartRequest request)
        {
            if (IsRestarting) return;
            IsRestarting = true;
            _pending = request;
            LoadingScreen.Run(RestartRoutine());
        }

        static IEnumerator RestartRoutine()
        {
            LoadingScreen.Show();
            yield return null;

            // Keep what was played in the current slot (only at a safe point: never mid-conversation).
            try { SaveService.Autosave(UnityEngine.Object.FindAnyObjectByType<IAmAsIAm.Runtime.Story.StoryRunner>()); }
            catch (Exception e) { Debug.LogWarning("[IAmAsIAm] Couldn't save before switching: " + e.Message); }

            GameBootstrap.Shutdown();
            yield return null; // The old game object is destroyed at the end of that frame.
            GameServices.ResetStatics();
            SaveService.ClearActiveSlot();
            Time.timeScale = 1f;

            var load = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            while (load != null && !load.isDone) yield return null;
            yield return null; // The new scene's Start methods run (SaveSlotsScreen applies the request).

            IsRestarting = false;
            LoadingScreen.Hide();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            _pending = null;
            IsRestarting = false;
        }
    }
}
