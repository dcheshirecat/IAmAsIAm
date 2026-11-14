# As I Am

A 3D story game (Android APK and PC) about living with **OSDD** (Other Specified Dissociative
Disorder). The character was raised Quaker, left organised religion, believes in Wicca,
and sets out to explore other faiths, one religion per chapter, each guided by someone from
inside that faith. The player chooses which alter fronts each chapter.

The game teaches **real grounding, mindfulness, DBT and CBT skills**, and weaves in
**open pagan folk practices** (runes, tarot, herbs, stones, charms) as sensory anchors
and reflection prompts. These practices are never stat buffs.

> ⚠️ *As I Am* teaches real coping skills, but it is **not a substitute for
> professional care**. If you are in crisis, please contact local emergency services or a
> crisis line.

## Documents

| Doc | What's in it |
|---|---|
| [Game Design Document](docs/GameDesignDocument.md) | Vision, pillars, core model, dissociation representation, story structure, cast, milestones, **open questions** |
| [Content & Safety Guidelines](docs/ContentAndSafetyGuidelines.md) | Rules for representing OSDD, trauma, closed practices, herb safety, and comfort features |
| [Therapeutic Skills Reference](docs/TherapeuticSkillsReference.md) | Every skill with accurate steps, cautions and sources |
| [Practices & Lore Reference](docs/PracticesLoreReference.md) | How runes, tarot, herbs, crystals and charms are used, with sources |
| [Technical Architecture](docs/TechnicalArchitecture.md) | Assemblies, data flow, rules enforced in code, input, console notes |
| [3D pipeline](docs/3D_PIPELINE.md), [Art list](docs/ART_LIST.md), [Art sizes](docs/ART_SIZES.md), [Art hand-over](docs/ART_SWAP.md) | Nomad Sculpt → Procreate → Grab3D → the game, as in Seven Witches |
| [Story writing](docs/StoryWriting.md) | Writing chapters in Ink: speakers, tags, game functions |
| [Trigger ideas](docs/TriggerIdeas.md) | DRAFT trigger list for the owner to edit |
| [Lived experience sources](docs/LivedExperienceSources.md) | How practitioners' voices are sourced (never invented) |
| [CLAUDE.md](CLAUDE.md) | Decisions log and working notes |

## Opening the project

1. Install **Unity 6000.6.0f1** (the same as Seven Witches) with the **Android Build Support** module.
2. In Unity Hub: **Add → Add project from disk**, then pick this folder. Let Unity resolve packages and generate `.meta` files.
3. When prompted to enable the new Input System backend, choose **Yes** (the editor restarts).
4. Run **IAmAsIAm → Configure Project** (creates and assigns the URP asset), **IAmAsIAm → Compile Story**, then
   **IAmAsIAm → Rebuild Boot Scene**, and press Play in `Scenes/Boot.unity`.
5. Run **IAmAsIAm → Validate Data** and **Window → General → Test Runner → EditMode → Run All**.
6. Commit the generated `.meta` files, `Story/main.json` and `ProjectSettings/`.

## Building the APK
- **On GitHub:** Actions → **Build Android** → Run workflow, then download `IAmAsIAm-apk` from the run's Artifacts.
  It needs the repo secrets `UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD` (the same values as Seven Witches).
- **In Unity:** IAmAsIAm → Build Android APK (writes `build/Android/IAmAsIAm.apk`).

## Running core tests without Unity

The rules and data validation live in an engine-free assembly, so you can run them with plain .NET 8:

```bash
dotnet test tools/CoreVerify/Tests/Tests.csproj
```

The story is checked the same way: `bash tools/InkCheck/check.sh` compiles the Ink story and auto-plays it.
CI runs both on every push (`.github/workflows/core-tests.yml`).

## Project layout

```
Assets/_Project/
  Scripts/Core/        Engine-free rules: Mind, PluralSystem, Grounding, Skills, Practices, Safety, Save
  Scripts/Runtime/     Unity glue: bootstrap, presentation, camera, input, UI hooks, save
  Scripts/Editor/      Data validator menu and pre-build gate
  Tests/EditMode/      NUnit tests (also run outside Unity via tools/CoreVerify)
  Resources/Data/      Authored content (JSON): runes, tarot, herbs, crystals, charms, skills, cast, crisis lines
  Input/               Input System actions
  Scenes/              Scene plan for the M1 vertical slice
docs/                  Design, safety, skills, lore, architecture
tools/CoreVerify/      .NET harness for CI
```

## Status

**A playable base is in:** walk around a placeholder flat with a close, over-the-shoulder camera (Pokémon Legends: Z-A
style), talk to a placeholder guide, draw tarot, breathe at the altar, with touch controls on Android. It's ready for
the owner's art. Systems, data, the story runner and the APK build are in. **The chapters are being designed with the owner**
(GDD §0 and CLAUDE.md, "Open"). Until then the story is a placeholder line.
