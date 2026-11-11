using IAmAsIAm.Runtime.Grounding;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace IAmAsIAm.Runtime.Safety
{
    /// <summary>
    /// The Anchor: an "I need a break" button that is always available, everywhere, including
    /// cutscenes and dialogue. It pauses the game and opens a calm safe space with breathing,
    /// grounding, and support links. There's no penalty and no timer. The player returns when ready.
    ///
    /// This must live on a persistent object (alongside GameBootstrap), and its input action
    /// must live in an action map that is never disabled. See ContentAndSafetyGuidelines.md §6.
    /// </summary>
    public sealed class AnchorMenuController : MonoBehaviour
    {
        [SerializeField] InputActionReference anchorAction;
        [SerializeField] GameObject anchorPanel;
        [SerializeField] BreathingExerciseView breathing;
        [SerializeField] GameObject supportPanel;

        [Header("Events")]
        public UnityEvent onOpened;
        public UnityEvent onClosed;

        float _previousTimeScale = 1f;

        public bool IsOpen { get; private set; }

        void OnEnable()
        {
            if (anchorAction == null) return;
            anchorAction.action.performed += Toggle;
            anchorAction.action.Enable();
        }

        void OnDisable()
        {
            if (anchorAction != null) anchorAction.action.performed -= Toggle;
        }

        void Toggle(InputAction.CallbackContext _)
        {
            if (IsOpen) Close();
            else Open();
        }

        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            if (anchorPanel != null) anchorPanel.SetActive(true);
            if (supportPanel != null) supportPanel.SetActive(false);
            onOpened?.Invoke();
        }

        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            if (breathing != null) breathing.Stop();
            if (anchorPanel != null) anchorPanel.SetActive(false);
            Time.timeScale = _previousTimeScale <= 0f ? 1f : _previousTimeScale;
            onClosed?.Invoke();
        }

        public void StartBreathing()
        {
            if (breathing == null) return;
            breathing.gameObject.SetActive(true);
            breathing.Begin();
        }

        public void ShowSupport()
        {
            if (supportPanel != null) supportPanel.SetActive(true);
        }
    }
}
