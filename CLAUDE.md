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
  with animations. **Grab3D only: never suggest Mixamo, Blender or other rigging/animation tools.**
  Drive folder "Game Art - I Am As I Am" (id 1UPA-7eBawSL_SNpq51jZ_sPygYRrZRjb; "Read me first" in Guidelines). `docs/3D_PIPELINE.md`, `ART_LIST.md`, `ART_SIZES.md`, `ART_SWAP.md`.
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

- 2026-11-11: **Chapter order:** Prologue = Quaker (backstory) → Prelude = Wicca (more backstory) → Ch1 Christianity (ex 1)
  → Ch2 Judaism (ex 2) → the rest (order to come). `chapters.json`; the knots are placeholders in `Story/chapters_placeholder.ink`.
- 2026-11-11: **The parent** is a single mother, a Quaker minister, who neglects raising her child in favour of the congregation (`people.json`).
- 2026-11-11: **Both kinds of Quaker meeting exist in this world**: programmed (pastoral Friends churches with ministers, hymns,
  sermons) and unprogrammed (silent worship). **As much diversity within each religion as possible**: show its different
  streams, branches and denominations, not one "standard" version (traditions.json `streams`; each stream sourced).
- 2026-11-11: **The triggers** are the owner's 18 in `triggers.json`. Each STARTS dissociation (random outcome); a **switch only after
  not being able to get away** (switchAfterSeconds, default 20, or `# cue-held:`). The validator forbids Switch as a direct outcome.
- 2026-11-11: **Grab3D facts** (from grab3d.com/tools/auto-rig, its clips manifest): 7 CC0 clips (idle, walk, run, walk-cartoon,
  run-cartoon, wave, jump); 19-bone humanoid only; no fingers; no face rig; exports GLB/FBX with Unity Humanoid bone names.
  Other story poses are made in Unity from the Humanoid rig. Faces: swapped face paintings recommended (owner to confirm).
- 2026-11-11: This session runs on the personal environment "Cheshirecat games (Grab3D docs)" (allows grab3d.com).

- 2026-11-11: **The system (owner):** Maya (host), Serena, Izzy, Nina, Lily, Heather, Jacob, Axel; Theresa and Holden don't front.
  `system_members.json`, `docs/SystemMembers.md`. Maya fronts at the start.
- 2026-11-11: **People:** the mother is **Sarah**, minister of a **programmed Friends church in the USA**; ex 1 **Mark** (Christian);
  ex 2 **Adeena** (Jewish, became religious and married).
- 2026-11-11: **Chapter order after Judaism:** 3 Buddhism, 4 Islam, 5 Hinduism, 6 Sikhism, 7 Daoism, 8 Druidry, 9 Humanism,
  10 Unitarian Universalism, 11 Jainism, 12 Hellenic polytheism, 13 Eastern Orthodox Christianity (the order the owner listed them;
  confirmed by the owner "for now"). Shinto, Bahá'í and Heathenry stay in the codex only.
- 2026-11-11: **Jacob (14, Catholic):** a Catholic church is part of Chapter 1's diversity, AND Jacob's faith comes up elsewhere too.
- 2026-11-11: **Write the chapters in order**, starting with the Prologue, asking the owner how each one goes.
- 2026-11-11: Temporary app icon `Art/UI/app_icon.png` (marked TEST); `BuildTools.SetAppIcon` sets it. Overwrite to replace.
- 2026-11-11: **Alters' pronouns, ages and faith** are the owner's (`docs/SystemMembers.md`). Heather's faith is hidden from the
  character until later (`hiddenUntilLater`). Theresa has no pronouns (use the name). Maya holds the job; Izzy runs the lost-time days.
  Links: Maya, Nina, Lily, Heather and Jacob see Sarah as their mother; Axel dated Mark as a trans man; Maya dated Adeena,
  who married **Michael**.
- 2026-11-11: **Faces:** face paintings on the 3D models in the world, **2D portraits** beside the dialogue box.

- 2026-11-12: **Prologue (owner):** Iowa City, Iowa; the body was born in 1998. You start as **young Maya**. The game eases the
  player into dissociation and the first time losing time; the **first switch** comes at the very end, home alone in Maya's room
  after services, then a blackout. Guide: a kind older Friend in Sarah's church who tries to be there for Maya and fails.
  Sarah is well-meaning but absent, neglectful to the point of harm; **never show her side**. Nothing is off-limits to show.
- 2026-11-12: **Replay rule:** anyone who fronts during a chapter can START that chapter once it's finished in full
  (`Core/Story/ChapterRuns.cs`, saved **per save slot**: unlocks do NOT carry across saves, owner 2026-11-12). **Content warnings before every chapter** (validator requires contentTags).

- 2026-11-12: Prologue: guide **Mira (68)**; Sarah's church is the **Church of Friends**; the first switch at the end is **Nina**
  (the first unlock). Replays are per chapter: same events, different abilities/choices per alter; unlocks are **paced: at most one
  new alter per finished run** (`ChapterRuns.MaxNewUnlocksPerRun`). After the warnings, the player may **skip and read a summary**.
  Story-scripted switches use the tag `# switch: <member>`. Still open: Maya's (the body's) age in the prologue.
- 2026-11-12: Owner wants **save slots** and an **animated loading screen with a random tarot card** each time.
  Built: **3 slots** (`Core/Save/SaveSlots.cs`; each slot = full save + small header), a title screen at start-up and a
  "Saves" button beside the Anchor (`Runtime/Save/SaveSlotsScreen.cs`). Autosave to the active slot when a scene ends and
  on app pause/quit, **never mid-conversation**. Switching slots saves first, then reloads the scene with a fresh state
  (`Runtime/Save/GameSession.cs`). Loading screen `Runtime/Loading/LoadingScreen.cs` (+ `Core/Practices/LoadingCard.cs`):
  a random Major Arcana card (never the same twice running) rises, turns over and floats; shows its reflection prompt;
  at least 2.6 s; Reduce motion = no movement; reversals follow the comfort setting. Card art from `Art/Tarot/`
  (`tarot_back.png`, `tarot_<card-id>.png`), wired by Rebuild Boot Scene. **The Anchor is drawn above every screen** (sort 200).

- 2026-11-12: **Prologue = "This Little Light"** (owner). **A few Sundays across several ages** of Maya, ending at the first switch.
  **Mira fails in three steps:** first she defers to Sarah (raises it, backs down), then she doesn't see enough (Maya hides how
  bad it is), then she leaves.
- 2026-11-12: Prologue Sundays at Maya's ages **5, 8, 11 and 15** (2003, 2006, 2009, 2013). Mira **moves away, then gets
  ill, then dies**. At the end Nina is **only a voice** (Nina, inside) before the blackout: no control yet. **Choices:** the first
  playthrough has small choices only (feel, journal, trust; big events fixed); **real branches open up the more times the chapter
  is replayed**.
- 2026-11-12: **Mira's arc:** 5 Maya meets Mira; 8 Mira raises it with Sarah and backs down; 11 Maya hides how bad it is, and Mira
  moves away; between 11 and 15 she falls ill; 15 she has died: **she never comes back**, and Maya hears it **in passing from Sarah**,
  who has no idea how hard it will hit. That night: Nina's voice, the blackout. **Branches belong to who fronts** (replaying as
  another alter opens that alter's choices). Prologue cast also has **other children** (First Day School), **a family Maya envies**
  (a present parent) and **the elders/board** who keep Sarah busy. Places: **home, the church building, the car, school**.
- 2026-11-12: School is **remembered** only (talk, memories) on the Sundays. The envied family is **a single dad** who is present
  (the contrast with Sarah). Maya has **one friend** in First Day School, **kind but who prefers other kids** over her. At 15 Sarah
  mentions Mira's death **in the car on the way home**, among other church news.
- 2026-11-12: The friend is **not** the single dad's child. **Sunday 1 (5):** Maya goes "floaty" in the long service (a gentle first
  depersonalization) and is then forgotten at church after it; Mira finds her. **The writing grows with Maya** from 5 to 15.
  **Prologue names (owner):** the single dad **Mr. Carter**, his much older children **Thomas** and **Reneé** (watched from afar);
  Maya's friend **Rebecca** (her parents unnamed: "my mom/dad"); the kids Rebecca prefers **Jamie, Justin, Will**; First Day School
  teacher **Miss Eloise**; elders **Mrs. Hines** and **Mr. Jones**; school teacher **Miss Susan** (remembered); **Mira Smith**.
  All in `people.json` and `docs/PrologueNames.md`.
- 2026-11-12: **Sunday 2 (8):** small signs add up (Maya hungry, eating all the fellowship snacks; home alone in the evenings while
  Sarah is at meetings; going "floaty" and not answering her name), so Mira speaks to Sarah. Sarah thanks her warmly, calls Maya
  "very independent", and is called away by Mrs. Hines. **Maya never knows in the prologue:** the scene is shown LATER in the game,
  when adult Maya meets a childhood companion who overheard it. **Sunday 3 (11):** Maya **performs okay** (neat, smiling, good
  answers) and Mira believes it, then moves away. **Sunday 4 (15):** an **ordinary Sunday**; the news in the car lands on it.
- 2026-11-12: **Reneé** overheard Mira and Sarah (Sunday 2). She used **crutches** as a child and uses a **manual wheelchair** as an
  adult. Adult Reneé is an **out bisexual**, has **left the church but still identifies as Quaker**; in **chapter 4 or 5** she and
  Maya meet up to talk about being **queer and ex-religious**, and she tells Maya what she heard.
  **The owner voices everyone who speaks, including the alters' voices** (every spoken line goes on the Drive script lists).
  Prologue drafts: `Story/Drafts/draft_prologue_sunday1..4.ink` (+ `_readable.md`), not canon until approved.
- 2026-11-12: **The owner edits drafts in Google Drive**: "Game Art - I Am As I Am" → **Story Drafts** (id 17BRsPRw6Ql9tObnHT4Cr0Nnb1i9LT-6q)
  → `00 Prologue - This Little Light` (id 1RPF_IaN8Yvj-yEwbJ15OuAYeh05nzL_a): explore-first Docs "Sunday 1 - Maya, 5 (2003)"
  (1GRTItageeHxbWhCaz083ZYb6Q-XDI3eAE0R81aEsbSE), "Sunday 2 - Maya, 8 (2006)" (1GgcPAhX8pfzqdyFYT-Z6BZeIrEboLxMNijcsgWZcYeo),
  "Sunday 3 - Maya, 11 (2009)" (1mdi5zq-RoXj6ZoFSJVB0q5AULZRzPXISi-__OUjKCFA), "Sunday 4 - Maya, 15 (2013)"
  (1K4J-ViOl47-vZCCy2DTf_rMdIgBWJlXCIQruYv8swZg); the first, narrated versions are in its "Old versions (narrated)" folder.
  **The Docs are the source once the owner edits them**: read the Doc (with
  comments) and carry the changes back into the Ink. Docs are made from Ink with `tools/StoryDocs/ink_to_doc.py`.

- 2026-11-12: **The owner voices parts of the game.** Drive "Game Art - I Am As I Am" → **Voice-Over** (id 1pluvFHsiS_BqpKREdIr1B2tOQnxYMnho):
  one folder per chapter (`00 Prologue (prologue-quaker)` … `14 …`), `Daily Life and Inner Voices`, `Scripts (what to record)`
  (Claude puts line lists here), `Retakes`, `Masters (WAV originals, optional)`, and "Read me first - Voice-Over". MP3 mono,
  44.1/48 kHz, ≥192 kbps, one line per file, no music/effects. Names `vo_<chapter-id>_<speaker>_<number>.mp3` (prologue Maya:
  `maya-05/08/11/15`; inner voices `<id>-inside`; outside chapters `daily`). Voicing is optional per line (unvoiced = text only).
  In the game: `Assets/_Project/Audio/Voice/<chapter-id>/`. `docs/VOICE.md`. Not built yet: playback, `# vo:` tags, script export.
  Note: `*.mp3` is in LFS and LFS uploads are blocked from cloud sessions: decide how to commit voice files when the first arrive.
- 2026-11-12: **Voice-over (owner, corrected):** the owner records every voice. **Never generate voices** (no TTS, no AI voices).
  Claude DOES the logistics: script lists, file names, ids, importing the files, playback in the game.
  Every spoken line gets a permanent `# vo: <chapter>_<speaker>_<0000>` tag (= `vo_<...>.mp3`) from
  `tools/StoryDocs/voice_script.py --chapter <id> --assign --csv ... <files>`; numbers never change or get reused. The script
  list goes to Drive Voice-Over → Scripts as a Google Sheet ("00 Prologue - script list (draft)",
  id 1JRpAvAoa5fmtixrocBLkNuJCbr1aL5i06RiXMNBdIps: 117 lines). Prologue Maya voices: `maya-05/08/11/15` (`// vo-speakers:` header).
  Narration isn't voiced (ask the owner if that changes). Rebuild the Sheet when lines change, keeping the owner's Recorded ticks.

- 2026-11-12: **Push everything to `main`** (owner): the Android build runs automatically on every push to `main`
  (and still by hand). Each build's version code is the run number (`v0.1.<run>`, shown bottom right in the game), and it is
  signed with ONE fixed key from the repo secrets `ANDROID_KEYSTORE_BASE64` + `ANDROID_KEYSTORE_PASS` (alias `iamasiam`), so
  updates install over the old app with no uninstall. Without the secrets it falls back to a throwaway key (warning in the
  run's Summary). Never print or commit the key. Test builds show errors on screen (`Runtime/Diagnostics/ErrorOverlay.cs`).
- 2026-11-12: **Faster builds:** pushes that only touch docs, `*.md`, `Story/Drafts/` or `tools/` don't start an APK build;
  the disk cleanup runs in parallel in the background while the code and the Library cache download. `main` is the
  default branch (owner).
- 2026-11-12: Builds 5, 8 and 9 were killed (exit 137) ~3 s after Unity started, while loading the cached Library, with memory
  free and no kernel OOM. The workflow now **retries once from a clean Library** if the first try fails (step ids build/retry).

- 2026-11-12: **Storytelling style (owner): very little narration.** The story is mostly **explored**: the player walks the
  places, looks at and uses things, overhears and talks, and the game gently **points them in the right direction** (the owner's
  reference for the feel: *Tell Me Why*; take only the approach, never its story, characters or look). Narration isn't voiced.
  Claude may draft every chapter (owner: "yes"), as drafts in Drive for the owner to edit.
  **Guidance:** an **objective line** (in the fronting alter's words), **inner voices** nudging, and a **soft glow** on things
  worth looking at. **Looking at an object shows only its label**; a **memory scene** plays only if the object is very significant
  to the host or to an alter **already unlocked**. **Playing a chapter as an unlocked alter reveals hidden memories (cutscenes).**
  Once another alter's version of a chapter is unlocked (or a new alter is met), an **unexpected switch** can always happen.
  The prologue drafts are to be **reworked into this explore-first style** (same events and dialogue; keep the owner's Doc edits).

- 2026-11-12: **Prelude (owner):** Maya meets Wicca at **college** but only gets **serious in her early twenties**. She finds it first
  through **a person**, later through **a shop or coven**. The guide is **a coven elder**. The system: **lost time grows**
  (more blackouts, signs of others: notes, things moved) and Maya doesn't understand it yet.
  College is **out of state**; the first person is **a classmate**; the coven is an **open, eclectic teaching coven** with public
  sabbat circles (only open practice shown). The elder: the owner will describe them (name, age, pronouns, personality).

## Open (ask the owner)
Every chapter's content (ask, chapter by chapter); confirm the chapter order 5–13.
- 2026-11-11: **Camera like Pokémon Legends: Z-A**: third person, close behind and over the shoulder of the character,
  orbiting with the right stick / mouse / a drag on the right of the screen. `Runtime/Camera/ShoulderCamera.cs`.
- 2026-11-11: The owner provides the art **once a playable base exists**: placeholders until then.

## Tech
- Assemblies: `IAmAsIAm.Core` (pure C#, no UnityEngine: all rules), `IAmAsIAm.Runtime` (Unity glue),
  `IAmAsIAm.Editor`, `IAmAsIAm.Tests.EditMode`. Namespaces `IAmAsIAm.*`.
- Data is JSON in `Resources/Data`; enums are stored as strings (`EnumText`). `DataValidator` enforces the content
  rules (tests, the editor menu, and the pre-build gate).
- Story: Ink in `Assets/_Project/Story` (`main.ink`, `game_api.ink`). `StoryRunner` binds the functions, and tags become
  interactive moments (`# exercise:`, `# ritual:`, `# explore:`, `# cue:`, `# cw:`). `StoryScreen` builds its UI in code.
  Explore-first tags: `# objective:` (goal line), `# look: <id>` (close-up + label), `# memory: <id>`, `# maybe-switch[: %]`;
  places are hubs of sticky choices `+ [Look: …]` / `[Use: …]` / `[Talk: …]`; `unlocked("<member>")` gates hidden memories.
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
