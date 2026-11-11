# I Am As I Am: project context for Claude Code

Read this first. Write important decisions in here as they are made (context gets compressed).
Sister projects (separate repos, same owner and setup): Seven Witches (`dcheshirecat/SevenWitches`), Thimble & Thread.
Don't change them from here. Seven Witches is the reference for the art pipeline and the Android build.

## What we're building
A 3D **story game** (a narrative adventure: chapters, choices and consequences) about living with **OSDD**,
with real grounding / mindfulness / DBT / CBT skills and open pagan practices played in the story.
Design: `docs/GameDesignDocument.md` (§0 has the owner's current direction and overrides the rest).

## Decisions (owner)
- 2026-11-11: Unity **6000.6.0f1**, URP (3D), the new Input System only, the same package versions as Seven Witches.
  glTFast and Ink are vendored as `Packages/*.tgz` (UPM downloads failed on the owner's PC).
- 2026-11-11: **Android APK** via GitHub Actions (`Build Android`, manual run; needs the secrets UNITY_LICENSE,
  UNITY_EMAIL, UNITY_PASSWORD, the same as Seven Witches) plus PC. **Landscape** on both. App id `com.cheshirecat.iamasiam`.
- 2026-11-11: Art pipeline = Seven Witches': Nomad Sculpt → Procreate → **Grab3D Auto-Rig Studio** → one GLB
  with animations. Drive folder "Game Art - I Am As I Am". `docs/3D_PIPELINE.md`, `ART_LIST.md`, `ART_SIZES.md`, `ART_SWAP.md`.
- 2026-11-11: The character was raised **Quaker**, left organised religion, believes in **Wicca**, and explores as many
  other traditions as possible. **One religion per chapter, fixed order** (the owner sets the order).
  **The player chooses the fronting alter per chapter**; switches and dissociation can happen partway through.
- 2026-11-11: Each chapter has **a new guide found within that faith**. The frame is the guides plus the character's daily life.
- 2026-11-11: The exes: an old Christian ex, and a Jewish ex who became religious and married (roles still to confirm).
- 2026-11-11: **Triggers:** the player learns them and tries to avoid them. When one fires, it causes depersonalization,
  derealization or a switch with **no player control**. The list comes from the owner (draft: `docs/TriggerIdeas.md`).
- 2026-11-11: Practices (tarot etc.) are **used by the character in the story**; open practices only, no stat buffs.
- 2026-11-11: Testimony may come from **public blogs/videos when they agree or form identifiable streams**
  (`docs/LivedExperienceSources.md`), or from consented interviews. Never invent any.
- 2026-11-11: **Ask the owner how chapters should go; don't decide story.** Drafts go in `Story/Drafts/` and are not canon.

## Open (ask the owner)
Chapter order and content; which alters (the draft cast in `system_members.json` is unconfirmed) and each alter's
relationship with faith; the exes' roles; the edited trigger list; the Grab3D animation list; how faces show expression.

## Tech
- Assemblies: `IAmAsIAm.Core` (pure C#, no UnityEngine: all rules), `IAmAsIAm.Runtime` (Unity glue),
  `IAmAsIAm.Editor`, `IAmAsIAm.Tests.EditMode`. Namespaces `IAmAsIAm.*`.
- Data is JSON in `Resources/Data`; enums are stored as strings (`EnumText`). `DataValidator` enforces the content
  rules (tests, the editor menu, and the pre-build gate).
- Story: Ink in `Assets/_Project/Story` (`main.ink`, `game_api.ink`). `StoryRunner` binds the functions, and tags become
  interactive moments (`# exercise:`, `# ritual:`, `# explore:`, `# cue:`, `# cw:`). `StoryScreen` builds its UI in code.
- `Editor/BuildTools.cs`: Compile Story, Configure Project (creates the URP asset), Rebuild Boot Scene (the scene is
  GENERATED: change the builder, don't hand-edit), Build Android APK. CI calls `BuildTools.BuildAndroid`.
- Reset statics with `[RuntimeInitializeOnLoadMethod(SubsystemRegistration)]` (Fast Enter Play Mode).
- Legacy `UnityEngine.UI.Text` with the built-in LegacyRuntime font (no TMP Essentials import needed, as in Seven Witches).

## Testing without Unity
- `dotnet test tools/CoreVerify/Tests/Tests.csproj`: Core and all shipped data (CI: Core tests).
- `bash tools/InkCheck/check.sh`: compiles the Ink story with the vendored compiler and auto-plays it (CI: story job).
- The Runtime/Editor layers can't be run here. Real verification is opening the project in Unity / the Android build.

## Conventions (from Seven Witches)
- Work on a feature branch and commit and push often; explain changes in plain sentences; ask before big refactors or deletions.
- Always commit `.meta` files. Never commit Library/, keystores, secrets or builds. Binaries go in LFS (`.gitattributes`).
- Never rename or move art outside Unity; the artist replaces a placeholder by overwriting it (same name, same shape).
- Avoid anything that raises modal pop-ups in the editor (the owner often works remotely from a phone).
