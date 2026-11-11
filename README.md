# I Am As I Am

A story-driven 3D Unity game about living with **OSDD** (Other Specified Dissociative
Disorder). You follow a plural system through one turning of the Wheel of the Year,
moving between everyday life and **Hearthwood**, an inner world of hearth, herb garden
and old forest.

The game teaches **real grounding, mindfulness, DBT and CBT skills**, and weaves in
**open pagan folk practices** (runes, tarot, herbs, stones, charms) as sensory anchors
and reflection prompts. These practices are never stat buffs.

> ⚠️ *I Am As I Am* teaches real coping skills, but it is **not a substitute for
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

## Opening the project

1. Install **Unity 6 LTS (6000.0.x)** with the build modules you need.
2. In Unity Hub: **Add → Add project from disk**, then pick this folder. Let Unity resolve packages and generate `.meta` files.
3. When prompted to enable the new Input System backend, choose **Yes** (the editor restarts).
4. **Create a URP asset**: *Assets → Create → Rendering → URP Asset (with Universal Renderer)*. Assign it in *Project Settings → Graphics* and *Quality*.
5. Run **IAmAsIAm → Validate Data** and **Window → General → Test Runner → EditMode → Run All**.
6. Commit the generated `.meta` files and `ProjectSettings/`.

## Running core tests without Unity

The rules and data validation live in an engine-free assembly, so you can run them with plain .NET 8:

```bash
dotnet test tools/CoreVerify/Tests/Tests.csproj
```

CI runs this on every push (`.github/workflows/core-tests.yml`).

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

**M0 Scaffold:** docs, data, core systems, tests and the Unity project skeleton are done.
Next is **M1 Vertical slice** (the Samhain chapter). See the GDD §9.
