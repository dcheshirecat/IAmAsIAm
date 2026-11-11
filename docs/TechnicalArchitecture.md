# Technical Architecture

## Targets & stack

- **Unity 6 LTS (6000.0.x)** with **URP**. Any 6000.0 patch should work, and Unity Hub will offer to upgrade the project.
- **Input System** (`com.unity.inputsystem`). Controller first, with full keyboard and mouse parity.
- **TextMeshPro** (bundled in `com.unity.ugui` 2.x in Unity 6).
- **Timeline** for authored dissociative sequences.
- Recommended, not yet added: **Ink** (inkle) or **Yarn Spinner** for dialogue, and **Cinemachine 3** for camera work. The core doesn't depend on either.

## Assemblies

```
IAmAsIAm.Core           Pure C#. noEngineReferences = true. All rules and state live here.
  ├─ Mind               MindModel (window of tolerance), dissociative experiences
  ├─ PluralSystem       SystemRoster (consent-based fronting), InnerJournal, TimeGap
  ├─ Grounding          BreathingPacer, FiveSensesExercise
  ├─ Skills             SkillCodex, ThoughtRecord, CognitiveDistortion
  ├─ Practices          PracticeLibrary, TarotDeck, RunePouch (lore + prompts only)
  ├─ Safety             ComfortSettings, ContentTags, CrisisResource
  ├─ Save               SaveData, PreferencesData, ISaveStorage
  └─ Util               EnumText, DataValidator

IAmAsIAm.Runtime        Unity glue. References Core, InputSystem, RP Core, TextMeshPro.
  ├─ Bootstrap          GameBootstrap (loads JSON → models), GameServices (static holder)
  ├─ Mind               MindStateController, DissociationPresentationDriver (URP volumes, mixer),
  │                     DepersonalizationCameraRig
  ├─ PluralSystem       FrontingController (embodiment per member), PerceptionGated
  ├─ Grounding          BreathingExerciseView, FiveSensesSession, SenseAnchor
  ├─ Practices          ReflectionAltar (tarot/rune → journal)
  ├─ Safety             AnchorMenuController, CrisisResourcesView
  ├─ Interaction        IInteractable, PlayerInteractor
  └─ Save               FileSaveStorage, SaveService

IAmAsIAm.Editor         Data validation menu and pre-build gate.
IAmAsIAm.Tests.EditMode NUnit tests for Core and all shipped data.
```

**Why put everything in an engine-free Core?** The rules that matter most, such as "no fail
states", "consent before fronting", "mismatched skills still help" and "herbs always carry
safety notes", are all unit-tested without Unity. CI runs them through `tools/CoreVerify`
on plain .NET, so it needs no Unity licence.

## Data flow

```
Resources/Data/*.json ──► DataLoader (JsonUtility) ──► Core models ──► GameServices
                                                           ▲   │ events
         Narrative / Timeline / UnityEvents ── commands ───┘   ▼
                                       Presentation (volumes, camera, audio, UI)
                                                    ▲
                                       ComfortSettings caps every effect
```

- Content lives in **JSON**, so writers and sensitivity readers can edit it without touching Unity. Enums are stored as strings and parsed by `EnumText`.
- The **content rules are code**: `DataValidator` runs in tests, from the editor menu (IAmAsIAm → Validate Data), and as a **pre-build gate**, so a herb with no safety notes cannot ship.

## Key rules enforced in code

| Rule | Where |
|---|---|
| No fail states. Leaving the window affects perception only. | `MindModel` (no damage or health concepts at all) |
| Co-consciousness is never "fixed" by grounding | `MindModel.ApplyGrounding`; tested |
| A part can always say "not now" | `SystemRoster.RequestSwitch` and `restingByChoice`; tested |
| Practices give no gameplay effects | `PracticeData.cs` types have no modifier fields; documented in a header comment |
| Distortion is capped by comfort settings and changes no faster than 1.5 s | `ComfortSettings.CapDistortion`, `DissociationPresentationDriver.minTransitionSeconds` |
| Breath holds are opt-in | `BreathingExerciseView` uses `WithoutHolds()` unless allowed |
| Tarot reversals are opt-in | `TarotDeck.AllowReversals`, default false; tested |
| The Anchor menu always works | `Always` action map, which must never be disabled; `AnchorMenuController` |
| Journal data is local-only | `SaveService` → `ISaveStorage`. No network code exists. |

## Input

`Assets/_Project/Input/IAmAsIAmControls.inputactions` has two maps:
- **Player:** Move, Look, Interact, Journal, InnerVoices.
- **Always:** Anchor (Esc / F1 / Start). Never disable this map, including during cutscenes and dialogue.

## Console notes

- Implement `ISaveStorage` per platform SDK. Never write files directly on consoles.
- `CrisisResourcesView` shows URLs as text where `Application.OpenURL` is unavailable.
- Haptics in `BreathingExerciseView` go through `Gamepad.current`. Add platform-specific adaptive trigger and HD rumble support behind the same interface.

## Rendering: dissociation volumes (art guidance)

| Volume | Suggested overrides |
|---|---|
| Grounded | Warm white balance, slight bloom, rich saturation, gentle vignette |
| Fog | Desaturation (Color Adjustments), depth of field (near focus), lifted blacks, exponential fog via a custom pass |
| Derealization | Flattened contrast, shadows midtones highlights shifted toward unnatural hues, slight lens distortion, reduced ambient occlusion so the world looks "paper-flat" |
| Depersonalization | Chromatic aberration (low), slight vignette, and the camera drift from `DepersonalizationCameraRig` |
| Co-consciousness | Edge tint in the co-conscious member's `colorHex` (custom full-screen pass), soft film grain |

Never use flicker, strobe or high-frequency noise. Run a Harding-style photosensitivity check on every volume.
