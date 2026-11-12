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
- 2026-11-12: **Replay rule (REPLACED, see "Chapter rules" below):** was "anyone who fronts can start it"
  (`Core/Story/ChapterRuns.cs`, saved **per save slot**: unlocks do NOT carry across saves, owner 2026-11-12). **Content warnings before every chapter** (validator requires contentTags).

- 2026-11-12: Prologue: guide **Mira (68)**; Sarah's church is the **Church of Friends**; the first switch at the end is **Nina**
  (the first unlock). Replays are per chapter: same events, different abilities/choices per alter; unlocks: see "Chapter rules" below. After the warnings, the player may **skip and read a summary**.
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
  Prologue: `Story/Chapters/prologue_sunday1..4.ink` (+ `_readable.md`), in the game (owner's Doc edits come first).
- 2026-11-12: **The owner edits drafts in Google Drive**: "Game Art - I Am As I Am" → **Story Drafts** (id 17BRsPRw6Ql9tObnHT4Cr0Nnb1i9LT-6q)
  → `00 Prologue - This Little Light` (id 1RPF_IaN8Yvj-yEwbJ15OuAYeh05nzL_a): explore-first Docs "Sunday 1 - Maya, 5 (2003)"
  (1GRTItageeHxbWhCaz083ZYb6Q-XDI3eAE0R81aEsbSE), "Sunday 2 - Maya, 8 (2006)" (1GgcPAhX8pfzqdyFYT-Z6BZeIrEboLxMNijcsgWZcYeo),
  "Sunday 3 - Maya, 11 (2009)" (1N38TzeYBpk8ZAh41NZp8QWgYfffjcldK5ar64LuUns8), "Sunday 4 - Maya, 15 (2013)"
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
- 2026-11-12: **Builds are never cut off, and never wait** (owner): no `concurrency` block, so every push to `main` gets its own
  build and they run side by side (each APK = that push). None is cancelled.
- 2026-11-12: **Touch fixes:** the story screen's dialogue box started visible, so the game was always "busy" (no movement, no
  touch controls, an invisible tap layer ate every touch): it now starts hidden (build 14+). The EventSystem is now kept for the
  whole run (DontDestroyOnLoad) and re-made if missing (`StoryScreen.EnsureEventSystem`), because switching slots reloads the scene.
- 2026-11-12: **Faster builds:** pushes that only touch docs, `*.md`, `Story/Drafts/` or `tools/` don't start an APK build;
  the disk cleanup runs in parallel in the background while the code and the Library cache download. `main` is the
  default branch (owner).
- 2026-11-12: Builds 5, 8 and 9 were killed (exit 137) ~3 s after Unity started, while loading the cached Library, with memory
  free and no kernel OOM. The workflow **retries up to twice** (step ids build/retry/retry2). Before each retry it puts the checkout back exactly as
  committed (chown, `git checkout -- .`, `git clean -fdq`, no Library): build 15's retry refused to start because the first try had
  left untracked ProjectSettings files ("Branch is dirty"). Build 15's kill came ~0.5 s into "MonoManager ReloadAssembly", 2.2 GB used.

- 2026-11-12: **Storytelling style (owner): very little narration.** The story is mostly **explored**: the player walks the
  places, looks at and uses things, overhears and talks, and the game gently **points them in the right direction** (the owner's
  reference for the feel: *Tell Me Why*; take only the approach, never its story, characters or look). Narration isn't voiced.
  Claude may draft every chapter (owner: "yes"), as drafts in Drive for the owner to edit.
  **Guidance:** an **objective line** (in the fronting alter's words), **inner voices** nudging, and a **soft glow** on things
  worth looking at. **Looking at an object shows only its label**; a **memory scene** plays only if the object is very significant
  to the host or to an alter **already unlocked**. **Playing a chapter as an unlocked alter reveals hidden memories (cutscenes).**
  Once another alter's version of a chapter is unlocked (or a new alter is met), an **unexpected switch** can always happen.
  The prologue drafts are to be **reworked into this explore-first style** (same events and dialogue; keep the owner's Doc edits).

- 2026-11-12: **Hidden memories (owner, correction):** a hidden memory opens only when its alter is **unlocked AND fronting at
  that moment** (Ink `fronts("<member>")`, never `unlocked()`). Otherwise the thing that opens it **simply doesn't glow**: the game
  must **never hint** that another alter, or Nina, is needed. Example: Sunday 3's blue dress glows only while Nina fronts.
- 2026-11-12: **Prelude (owner):** Maya meets Wicca at **college** but only gets **serious in her early twenties**. She finds it first
  through **a person**, later through **a shop or coven**. The guide is **a coven elder**. The system: **lost time grows**
  (more blackouts, signs of others: notes, things moved) and Maya doesn't understand it yet.
  College is **out of state**; the first person is **a classmate**; the coven is an **open, eclectic teaching coven** with public
  sabbat circles (only open practice shown). The elder: the owner will describe them (name, age, pronouns, personality).

- 2026-11-12: **Prelude names (owner):** the coven elder is **Tanya** (42, **they/them**, kind but strict); the college town is in
  **Illinois**; the classmate is **Selene** (both in `people.json`). In the game: `Story/Chapters/prelude_part1..3.ink` (+ `_readable.md`;
  test one alone with `/tmp/claude-0/inkprelude.sh N`): Part 1 Selene (2016, 18, Samhain in her dorm), Part 2 Things moved (2018, 20,
  Izzy's signs; the tarot deck glows only while Izzy fronts), Part 3 Tanya (2019, 21, the shop + public Yule circle, ends in a switch to
  Izzy). Guesses for the owner are in each file's header (Champaign-Urbana, Selene she/her, the shop "Hearth & Moon", Izzy as the
  Prelude's alter, the years). Drive: Story Drafts → `01 Prelude - Wicca (title to come)` (17eY31XFZ0v2E3liYuC-ORgtSgCA5ICTx): Docs
  Part 1 (1k2hkefKG4CDEcST2Xyh14agUvPmeegWSkZvzJs8LxdU), Part 2 (1eiJHSOO4iiKAqFmoDivSvgkBQucZ0mGGXsyxI-1sxy0), Part 3
  (1I0KbwKr1vBhQPLYcnwfasqL5Rnc7u3m7zb_7oPMlGfM). Voice script Sheet "01 Prelude - script list (draft)"
  (1Kc5rTVwGt09SHL7efDKoqK3f0GWwgwznNmKC5oMoLNE: 45 lines; Maya's adult voice is `maya`). Voice-Over Scripts folder id 15B1SB8Y855lNM5Q3sJp2lIDesOkeLD9w.
- 2026-11-12: **Tanya stays (owner):** Maya is in touch with Tanya **through the whole game**, mostly by **SMS and email**, meeting up
  **less and less** as time goes by, with a **final meeting at the end of the game**. The game needs **phone, computer and laptop
  features**, which Maya **gains as she ages**: the **family computer at 12**, a **phone and a laptop at 18** (owner).
- 2026-11-12: **Devices, BUILT:** `Core/Devices/Devices.cs` (tests `DeviceTests`), save v3 `devices`, `GameServices.Devices`,
  `Runtime/Devices/DeviceScreen.cs` (made by StoryScreen; canvas sort 40; a Phone button left of Saves once she has a phone, with
  email there too once she has the laptop; a banner for incoming texts). Ink: `set_age(n)` at the start of EVERY chapter part
  (gives exactly the devices for that age, so replays are right), `gain_device`, `text_from/text_to(contact, text)`,
  `email_from/email_to(contact, subject, text)`, tag `# device: computer|phone|laptop` (opens it; the story waits). Texts need the
  phone; the family computer only opens from the story (it's at home). Contacts = people ids.
  **Lost time on devices (owner):** after a blackout, an extra sent message may be there that Maya didn't write:
  `sent_while_away(member, "text"|"email", contact, text)`. It looks exactly like her own (no marker, no banner) and is **never
  pointed out; very subtle at first**, just one here and there. Who wrote it is saved but never shown.
- 2026-11-12: **Prelude confirmed (owner):** Champaign-Urbana (University of Illinois); Selene she/her; Izzy leaves the signs and comes
  forward at Yule. **The shop is "The Circle"** (was the placeholder "Hearth & Moon"; place id `shop_the_circle`).
- 2026-11-12: **Chapter 1, Christianity (owner):** Maya is **still in college** (Champaign-Urbana). **Mark** (id `ex_christian`) is a
  **Protestant** classmate in her **junior year**. He dates **Axel**, who starts fronting that year and **"comes out" as a trans man as
  the body**. Mark **never learns about the system**; it ends **when Maya comes back**. This is **the most lost time so far**, and Maya
  starts **taking it seriously**. **Chapter 1 ends with graduation, as Maya.** Then **Chapter 1.5** is a **flashback, never explained,
  just played**: the first time, it **must be played as Axel**, whoever was chosen; it can be how Axel is unlocked.
  **Guide: Gracie** (25, she/her), a **young nun still in training**, very bright, peppy and excited about her faith, never harmful.
  **Christian streams shown:** Catholic (Jacob, Gracie), mainline (Episcopal: Mark; Methodist), queer-affirming. Orthodox: Chapter 13 only. **Not** an evangelical
  megachurch (owner: definite no); **no Black church** unless the research is thorough enough (Claude: not without sourced, consented
  testimony). **Tanya** texts and emails all chapter: check-ins, events, holidays, readings, coven meetings, meals. **Axel ignores all of it.**
- 2026-11-12: **Timeline (owner):** the **Yule circle is December 2017** (Maya 19, sophomore year), so Maya and Tanya build a solid
  relationship before Maya "disappears". Prelude: Part 1 Oct 2016 (18), Part 2 Mar 2017 (18), Part 3 Dec 2017 (19).
  **Chapter 1** starts **as Maya** (an unexpected switch to **Izzy or Nina** if triggered). **Junior year (2018-19) is summarized on a
  black screen, not explained** until Chapter 1.5, where it's played as Axel. Maya comes back at the end of junior year: she
  **detransitions**, **breaks up with Mark**, is very confused and upset, and is **abandoned by everyone around her in senior year**.
  **COVID is shown** (graduation May 2020). Chapter 1 **ends with Jacob coming forward at a prayer service after graduation**.
  **Mark is Episcopal**, a **kind queer man**, left very confused and upset when Maya detransitions.
  **Gracie** is a **Catholic** nun in training who **volunteers at the campus ministry**: fundraisers, donation drives, social
  meetings and prayers. **Orthodox Christianity is saved for Chapter 13** (Maya exploring Europe), not shown in Chapter 1.
  Data: `chapters.json` has `ch1-5-axel` (knot `ch1_5_axel`). New content tags: pandemic, isolation, breakup, detransition.
- 2026-11-12: **Chapter rules (owner, replaces the old replay rule):** the **first playthrough of a chapter always starts as its
  `startingMemberId`** (Maya for everything up to Chapter 1; **Axel for Chapter 1.5**). **Each chapter unlocks ONE set alter**
  (`unlocksMemberId`, never random): Prologue **Nina**, Prelude **Izzy**, Chapter 1 **Jacob**, Chapter 1.5 **Axel**. Once finished,
  a chapter can be **replayed as any alter unlocked so far** (e.g. the Prelude as Nina). Dissociation and switches can be triggered in
  any chapter, but **only to the host or an unlocked alter** (`ChapterRuns.CanSwitchTo`, used by `TriggerSystem.CanSwitchTo` and
  `# maybe-switch`; e.g. Heather can't come forward in the Prelude). Story-scripted `# switch:` happens as written.
  `ChapterRuns.Complete(unlocksMemberId)`, `StartersFor(id, startingMemberId)`. Later chapters' starters/unlocks: ask the owner.
  **Unlocks happen only at the END of the whole chapter** (owner): a chapter's parts play in order as ONE run; only the last part has
  `# chapter-end` (the recap, then the unlock). A scripted switch earlier (e.g. Izzy at Yule) doesn't unlock anyone by itself.
  **Starters and unlocks are set chapter by chapter** with the owner; **not every chapter unlocks a new alter** (leave `unlocksMemberId` empty).
- 2026-11-12: **Chapter 1 beats (owner):** sophomore spring after Yule = **Thursday classes with Tanya, first sabbats, a first reading**;
  the sign Axel is coming is **more and more lost time**. The **black screen says only "Junior year." then "May 2019."**
  **Coming back:** Maya finds her body **"dressed up as a man"**; she's **scared and confused**. The breakup with Mark isn't played:
  it's **explained during senior year**. **Senior year: everyone abandons her except Tanya and Selene**, who understand something was
  wrong and **welcome her back into the coven**. **Gracie:** Maya meets her at a **donation drive**, and comes to like **praying quietly on
  Sundays**. The Methodist and queer-affirming churches are seen **as Axel in Chapter 1.5, on a holiday off campus with Mark**; Mark's
  Episcopal church **only in 1.5**. **COVID: alone in her dorm; a Zoom graduation where no one speaks to her**, quiet and lonely.
  **The end:** a prayer service at **Gracie's ministry**; they sing **a hymn Maya loved as a child**, she **can't leave without a major
  disruption**, is too scared to, starts dissociating; **the screen fades to black and Jacob's lines are spoken as a light appears**.
  In the game: `Story/Chapters/ch1_part1..4.ink`. Claude's guess: the hymn is "This Little Light of Mine" (the prologue's title).
  Parts: 1 Thursdays (spring 2018, 19: class, first reading, Ostara, growing lost time, black screen), 2 Coming back (May 2019, 20),
  3 Senior year (fall 2019, 21: Mark's email, abandoned, the coat drive and Gracie, Samhain with Tanya and Selene, quiet Sundays),
  4 Alone (2020: lockdown, Zoom graduation, the prayer service, Jacob). Drive: Story Drafts → `02 Chapter 1 - Christianity (title to
  come)` (1ZNBBCoAvZZQi2bK9kD3NFSC_ciUCQwWg): Part 1 (14YKbWiYuOV6oaFrNsicZuZO_0DDH2QhGpaP4zejpgBs), Part 2
  (1i_4R0_4pg60TRjLSlRJpr7VIog5wWqr_7CvHtAHuQmY), Part 3 (18Bvs_-r1VFrE_X6zwsnOgjvkAv6MmvdYkV_6e45OGnc), Part 4
  (1N1al1-P8tWZ--L6ucxfvpgFgxxSeD4RTTlrOeqxisZI). Script Sheet "02 Chapter 1 - script list (draft)" (1cSvXfcf2TbkcNSMfnW_Fii-ztjv9kVEf_7LjRMhZ-p4, 33 lines).
- 2026-11-12: **Chapter 1.5, Axel (owner):** Axel's first act is **a haircut and new clothes**. **Axel:** a **dark sense of humour**, loves
  fun and **parties**, never at anyone's expense; loves quality time with Mark. **Angry at the Quaker church** but **cautiously open to
  mainstream Christianity through friends**; he goes to church with Mark **only on holidays**, once they've dated a few months.
  **Coming out:** he **never tells Sarah** (barely in touch since Maya left home); tells **Selene and his professors first**, **disappears on
  Tanya**, and starts dating Mark **after** coming out. **Mark:** instantly kind and funny, matches Axel's values and humour; dates,
  studying, cuddling. **Mark's Episcopal church is queer-friendly and warm**; they go to **Pride and protests as a church**. **Izzy is
  triggered at a protest** (Maya went to protests with Sarah as a child). **Christmas and Easter** with Mark's family: the **Methodist**
  church and the **queer-affirming** one. **Axel knows a little about Maya** from context clues; **Selene just thinks he transitioned**.
  Other alters' signs show up for Axel too, **but less than for Maya**. **The end:** Mark **accidentally tears Mira's napkin** (Sunday 2);
  **Axel is gone the next morning**. **Guide: the priest at Mark's church**, kind, invites him for holidays, not in touch consistently.
  **Unlocks are silent until the end of the chapter** (the recap). In the game: `Story/Chapters/ch1_5_part1..4.ink`.
  Parts: 1 Axel (Aug 2018: haircut, thrift store, Selene, professors, meeting Mark), 2 Mark (fall 2018: studying, Pride with the
  church, Christmas Eve at the Methodist church), 3 Holidays (spring 2019: Ash Wednesday and the priest, the vigil where Izzy is
  triggered, Easter at a queer-affirming church), 4 The napkin (May 2019). Placeholders: "The priest" (`mark_priest`), "Mark's mom"
  (`mark_mom`), the church "St. ___'s". Drive: Story Drafts → `03 Chapter 1.5 - Axel (title to come)` (1pOOBlf0DtgjVVELZapx_fKVE_7hTOMgJ):
  Part 1 (1ZGtbXs-7G1g-CyCH8IEG_QQ-fVeWOc43PP8riNHm8so), Part 2 (1Dz9K7UjP6UkeRcxsJscW-uxoKoTRzjAl7uwA-WubyfU), Part 3
  (1IYMBbtmGFUq5peIAWJeYkVfouQhsds5RYOAGdgAQp-A), Part 4 (1H7Ls89KivW6oT-av-x1lgLZhG0fFGCaoXcamNETZmYI). Script Sheet
  "03 Chapter 1.5 - script list (draft)" (1YGR6-Rt3Mt8VFR9vvrCxkhe6VqPXoUiFPmq3SmmNGvk, 46 lines).
- 2026-11-12: **The story is IN THE GAME (owner).** `Story/Chapters/` holds the Prologue, Prelude, Chapter 1 and Chapter 1.5 (one
  `.ink` + `_readable.md` per part, moved from Drafts/). `Chapters/chapters.ink` has each chapter's root knot (chapters.json
  `inkKnot`), which diverts to the first part; each part diverts to the next; only the last part has `# chapter-end`. `main.ink`
  INCLUDEs them all; `chapters_placeholder.ink` keeps Chapters 2-13. InkCheck now plays **every chapter** from its root knot with the
  strict checks (contacts, members, skills...). Owner Doc edits still come first: carry them into `Story/Chapters/`.
- 2026-11-12: **Main menu (owner):** the title screen is a menu: **Continue** (the last save), **New game** and **Load a save**
  (both open the save slots, which have a Back button), **Quit** on computers (`SaveSlotsScreen.ShowMain`). In the game, a
  **Chapters** button (top, left of Phone) opens the chapter list (`StoryScreen.OpenChapterList`): **Begin** the next chapter in order,
  or **Replay** a finished one. The list opens by itself on a new game and after each chapter's recap.
- 2026-11-12: **App icon:** `BuildTools.SetAppIcon` sets every Android icon kind (legacy, round, adaptive: the same picture on both
  adaptive layers) from `Art/UI/app_icon.png` (still the TEST icon until the owner's art).
- 2026-11-12: **Round joystick (owner):** the touch joystick's base and knob are round (sprites made in code, `StoryScreen.Disc`).
- 2026-11-12: **Fog when dissociating, in any way (owner):** `Runtime/Mind/FogOverlay.cs` (made by StoryScreen, under all UI): a light
  wash plus two slowly drifting mist layers (a generated tileable noise texture), and 3D scene fog (`RenderSettings`, exponential
  squared), all following the strongest dissociation (1 - Presence; co-consciousness alone doesn't fog). Capped by
  `ComfortSettings.CapDistortion`, never faster than 2.5 s from none to full, no drifting with Reduce motion.
- 2026-11-12: **Chapter 1.5 names (owner):** the priest is **Pastor Key** (`mark_priest`, **31, he/him**) at **St. Luke's** Episcopal; Mark's mother is **Cassie** (`mark_mom`); Mark is from **Indianapolis**.
- 2026-11-12: **Chapter 2, Judaism (owner):** Maya **moves to Chicago for a job** (COVID is skipped). She meets **Adeena at work**; they're
  together **two and a half years**. Adeena is **Conservative** and becomes **Hasidic**. She meets and marries **Michael only after they
  break up**: a **present-day fact mentioned in Chapter 6**, which Maya hears **from an old friend from work**, and it's **triggering**:
  they shared a **special moment over the Shabbat candles every week** while dating. **Show every stream** (none left out).
  **Guide: Rabbi Abe** (78, he/him), **vital and refreshing**. Alters: **Izzy and Nina**; **Heather only leaves a sign** (now: the tall candle, below), she is **not unlocked** yet (owner correction).
- 2026-11-12: **Chapter 2 answers (owner):** COVID skipped; the chapter **starts in Chicago in 2023**. Maya **works at a museum**.
  **Adeena: 27**, stern and serious with a heart of gold (her look comes with the owner's character design). **The candles:** Adeena
  lights, and Maya covers her eyes with her for the blessing, every Friday. **The breakup:** Maya dissociates more, Adeena notices she
  isn't present in the relationship and ends it. **Rabbi Abe** is at **Adeena's (Conservative) synagogue**, met at an **interfaith
  couples' meetup**. The other streams come through **Adeena's family and friends, the holidays, and Maya alone after the breakup**.
  **Izzy** comes forward on **stressful days at work** (if triggered), **Nina** in **quiet moments alone at home** (if triggered).
  **Heather's one moment:** a **single tall candle lit on the windowsill on the date of Mira's death**; Maya doesn't remember lighting
  it (never explained, Heather never named). **Starts as Maya, unlocks nobody.** Tanya's texting replaces meeting up from here.
  In the game: `Story/Chapters/ch2_part1..4.ink`: 1 The museum (spring 2023, 24), 2 Friday nights (summer 2023, 25: first Shabbat,
  the Conservative shul, keruv with Rabbi Abe), 3 Her people (2024, 25: Heather's candle in February, the seder with Ruth (Reform),
  Lev (Humanistic), Rivka (Modern Orthodox), Savta (Moroccan Sephardi); Dani's Reconstructionist havurah; Pride Shabbat at an LGBTQ
  synagogue; Adeena's first Chabad class), 4 Not here (2025, 27: the breakup; Renewal Kabbalat Shabbat (`# cue: shabbat-candles`),
  a Reform temple, Shabbat lunch at Rivka's, Rabbi Abe by the lake: pikuach nefesh). Placeholders: the museum, the curator, the
  family names, Dani, "Congregation Etz Chaim". Michael is now in Chapter 6's people. Drive: Story Drafts → `04 Chapter 2 - Judaism
  (title to come)` (1xVD3UYELoU_vAhYc_iDteHwX71VSdrDC): Part 1 (1gWjHGNjtKwluj9rx4GKHi0z772SlA69hptJ6PD1SoQk), Part 2
  (1hJKibESyTOx_ADGuAxaCvp4u79xfKhLSOqRpD9eBxV0), Part 3 (1CTd24c5s_ksGSteynJGudt3DSj5g3NOvNDtWYh_Pun4), Part 4
  (1LJ5voECgiOEKpmafhook1brG_BNhIF1lcft8HMxxKoY). Script Sheet "04 Chapter 2 - script list (draft)" (1SWEnM61ellUIGqFxCVxNXPigk5dGYNep76fEHet8cRE, 66 lines).
- 2026-11-12: **Explore the world, not just dialogue (owner).** Every `# place: <id>` is BUILT as a space to walk around
  (`Runtime/World/StoryWorld.cs`, made by StoryScreen): placeholder geometry from the id (outdoors for park/quad/lake/path/march/
  pride/lawn..., a large hall for church/synagogue/temple/museum..., small for car/guest_room; pews, shelves, a bed by keyword),
  away from the test flat (which is hidden while a story place is up, and returned to when the story ends). Explore choices go
  INTO the place (`StoryScreen.TryExplore`): `Look:`/`Use:` = a thing, `Talk:` = a standing figure, `Go:` = a doorway; each glows
  softly (a slow breathing light) with a small label while on offer, and stays as scenery after. Walk up + Use (E / A / the Use
  button). Things she carries (`Your ...`, Mira's stone/napkin), `Talk: Say ...` and `Wait` are small buttons at the bottom.
  Choices that answer the same person two ways stay an on-screen list (dialogue). **Anyone who speaks stands in the place** (a
  figure; the camera turns to them); "Mom"/"Mommy" are Sarah. While exploring the character can walk, but saves, the phone and
  the chapter list still wait for the story. InkCheck fails a chapter whose explore choices come before any `# place:`.
  Writing rule: every hub needs a `# place:`; explore choices are `Verb: Target (detail)` with ONE choice per target.
- 2026-11-12: **Chapter 2 names (owner):** the museum is **The Field Museum** (the real place); Maya is a **tour guide** there
  (Adeena the registrar); the shouting curator is **Eudora Jones** (`eudora`); Adeena's mother **Ruth**, uncle **Lee**, cousin **Effie**
  (Modern Orthodox), **Grandma**; her friend **Hannah** (Reconstructionist havurah); the synagogue **Congregation Etz Chaim**.
  **October 7 / the war: NOT in the game** (owner: no). **Messianic Judaism is left out** (owner agreed); **add Beta Israel
  (Ethiopian Jews)**: a Sigd celebration in Part 4 (Nov 2025; `tigist` is Claude's placeholder name). **Mira died on January 18th**
  (2013): Heather's candle is on January 18, 2024. **Heather: the candle is enough** (no message).
- 2026-11-12: **Later chapters' starters/unlocks: "we'll cross that bridge later"** (owner). **REMIND the owner** when we reach each
  chapter (3-13) to set its `startingMemberId` / `unlocksMemberId` (Serena, Lily and Heather are left to unlock).
- 2026-11-12: **Chapter 3, Buddhism (owner):** this is where Maya **first gets real help: a therapist and a diagnosis**. The guide and
  the traditions: Claude researches open/shared Buddhist practices and proposes (a monk at a local temple, or Maya travelling to a
  Buddhist country after the diagnosis). Research + options: `docs/Chapter3_BuddhismResearch.md`.
- 2026-11-12: **Phone and laptop are always-usable icons on the side of the screen** (owner), not only story choices:
  `DeviceScreen.SideIcon` (right edge above Use; drawn icons made in code; unread counts), shown once she has each device, usable
  during exploring and dialogue (not over the Anchor or an interactive moment). The story's `# device:` beats stay too.
- 2026-11-12: **Walking between scenes + the map (owner).** Within a chapter part, when the story moves to a new `# place:`,
  Maya WALKS there, guided: the story waits (a "travel" moment, `StoryRunner.GoTo`), a glowing doorway "Go: <place>" appears
  in the current place and the objective says "Head to <place>". **From college on (age >= `PlaceData.MapAge` = 18; owner: "a map,
  gained when she leaves for college")** using the way out opens the **city map** (`Runtime/World/MapScreen.cs`): places she's
  been are marked, the next one glows and is the only one to tap (guided); "Not yet" goes back. Cuts (no walk): the first place
  of a run, the first place of each part (`set_age` marks a part = a jump in time), and to/from `black`. Using a story `Go:`
  just before a move skips the second walk. **Places data:** `Resources/Data/places.json` (name, city, map x/y 0..1, the chapter
  it first appears in); InkCheck fails an unknown place; map art goes in `Resources/Maps/map_<city>.png` (placeholder drawn in code).
- 2026-11-12: **The glow is on the objects themselves (owner: subtle but noticeable):** an explore thing's own colour breathes
  slowly toward a warm light (`ChoiceSpot`); no floating labels or lights (the prompt names it up close).
- 2026-11-12: **Art: models in chronological order (owner)**, and **every illustrated/modelled asset listed in the docs**:
  `docs/ART_LIST.md` (rewritten) + `docs/ART_BY_SCENE.md` (generated by `tools/StoryDocs/art_by_scene.py` from the chapters:
  every place, person and close-up, in story order). Re-run it whenever chapters change. Drive copies (Guidelines folder
  1DvSRmpyEemsE3PqNy_sHBlg1E_wBsK2A): "Art list" 1G5jOCPvjsZUYN2LzUlsJy5WBLvAGKvk7PUQZKUTYDwA, "Art by scene" 1iH_kT1To5yXHe-bbVQd0tctZ2ahQ7rHit9amxBLFkw8 (replace when they change).
- 2026-11-12: **Chapter 3 answers (owner):** the guide comes from **option 3: Maya travels to a Buddhist country after the
  diagnosis** (the country: ask). The therapist is **Angie** (`angie`, 35, she/her); **Tanya is the one who gets through to
  Maya**. The diagnosis is only ever **"a dissociative disorder"**, never a real label. **Chicago, 2026, Maya 27.** The old work
  friend who tells Maya about Adeena's wedding in Chapter 6 is **Christie** (`christie`), brought in now: a fellow tour guide
  in Chapter 2 (the snack drawer) and Chapter 3. In the game: `Story/Chapters/ch3_part1.ink`, "Someone to talk to" (Jan-Mar
  2026: Tanya's visit and the call; Christie takes over a tour; Angie's waiting room (Gerald the plant) and first session with
  `# exercise: five-senses`; weeks later "a dissociative disorder"; a Sunday service at a Jodo Shinshu temple in Chicago
  (**Reverend Mori**, Claude's placeholder name) who mentions temple stays; Angie's travel plan by email). It ends "To be
  continued." (no `# chapter-end` yet). Chapter 3 starts as Maya and unlocks nobody FOR NOW: remind the owner to set it.
- 2026-11-12: **Test builds: "Test: any chapter"** in the chapter list (`StoryScreen.TestJumps`) starts any chapter, so the owner
  can test new scenes without replaying everything. Turn it off for a release. **Owner: push when there's a new scene to test.**
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
  places are hubs of sticky choices `+ [Look: …]` / `[Use: …]` / `[Talk: …]`; `fronts("<member>")` gates hidden memories (`unlocked()` is for unexpected switches only).
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
