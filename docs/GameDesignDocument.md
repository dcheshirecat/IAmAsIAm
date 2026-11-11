# I Am As I Am — Game Design Document

> Working title, taken from the repository name. Draft 0.1, co-written. Everything marked
> **[DECIDE]** is open for you to change.

---

## 0. Direction from the owner (2026-11-11). This overrides anything below that conflicts.

- **A story game** in the genre of narrative adventures (choices and consequences, close cameras, chapters).
  The story is written in **Ink** (`Assets/_Project/Story`). Choices are remembered (`ChoiceLog`) and recapped.
- **Platforms:** an **Android APK** (built by GitHub Actions, like Seven Witches) and PC, in landscape on both.
- **The character:** raised **Quaker**, left organised religion behind, now believes in **Wicca**, and wants to
  explore as many other traditions as possible.
- **One religion per chapter**, in a **fixed order** (the owner will set the order). **The player chooses which alter
  fronts at the start of each chapter**, and the chapter plays differently depending on who it is. Switching or
  dissociating partway through can happen.
- **Guides:** each chapter, the character finds **a new person within that faith** who guides them. Practitioners'
  voices are based on real testimony (interviews, or public blogs and videos that agree or form a clear stream;
  see `LivedExperienceSources.md`).
- **Frame:** the guides introduce each chapter, and the **character's daily life** carries on between chapters.
- **The exes:** an old **Christian ex**, and a **Jewish ex who became religious and married**. *(Their roles in
  the story: to be confirmed with the owner.)*
- **Triggers:** the character has triggers. The player learns them and tries to avoid them, and when one fires it causes
  unwanted depersonalization, derealization or a switch, **with no control over which** (`TriggerSystem`).
  The draft list is in `TriggerIdeas.md`, waiting for the owner's edits.
- **Practices are played:** the character really uses open practices (tarot spreads, runes, crafting charms,
  sachets, the Wiccan altar) as story actions. Results feed the story; they never give stat buffs.
- **Art:** the Seven Witches pipeline (Nomad Sculpt → Procreate → **Grab3D** → GLB), with this game's own
  list and sizes (`3D_PIPELINE.md`, `ART_LIST.md`, `ART_SIZES.md`).
- **Chapters are the owner's to design.** Claude asks how the chapters should go rather than deciding.
  §4's Wheel-of-the-Year structure and the Samhain premise below were early drafts and are **superseded**.

## 1. Vision

**I Am As I Am** is a story-driven 3D exploration game about living with OSDD
(Other Specified Dissociative Disorder). It follows a plural system through one turning
of the Wheel of the Year. The player moves between an **outer world** (an apartment,
a bus, a workplace, a therapist's office) and an **inner world**, a cottage at the
edge of an old forest where the system's parts live.

The game has three goals, roughly in this order:

1. **Validate.** People who dissociate should see their experience shown accurately and
   kindly: the fog, the lost time, the voices that are *also you*.
2. **Teach real skills.** Every grounding, mindfulness, DBT and CBT skill in the game is
   the real technique, explained accurately and usable away from the game.
3. **Build understanding.** Singlets (non-plural people) should come away understanding
   that dissociation is a *protective* adaptation, not a horror trope.

### Design pillars

| Pillar | What it means in practice |
|---|---|
| **Dissociation protects; it isn't the enemy** | The fog is never a health bar. There are no fail states. Parts are never villains, and protectors are helpers whose methods made sense once. |
| **Cooperation, not cure** | Progress means communication, trust and shared life between parts. The game never presents fusion or final integration as the "good ending", and never presents it as bad either. |
| **Real skills, really explained** | Each skill links to an in-game codex entry with accurate steps, cautions and sources. |
| **Practice as presence** | Herbs, stones, runes and tarot are used the way many practitioners actually use them: as sensory anchors, rituals of attention and prompts for reflection. They are never stat buffs. |
| **The player is always safe** | An always-available Anchor button, comfort settings, content notes, no jump scares, and crisis resources one press away. |

### Audience & rating target
- Primary: plural and dissociative people, and people in DBT/CBT or trauma therapy.
- Secondary: friends, families and clinicians who want to understand.
- Target rating: PEGI 12 / ESRB T. Trauma is *referenced*, never depicted.

### Platforms
PC (Windows/macOS/Linux), then PS5, Xbox Series and Switch-class hardware. The architecture
assumes a controller first (see `TechnicalArchitecture.md`).

---

## 2. The core model: the Window of Tolerance

The central system is based on Dan Siegel's **window of tolerance**, a framework widely
used in trauma-informed therapy.

```
  HYPERAROUSAL  ── panic, racing thoughts, fight/flight
  ─────────────────────────────────────────────  upper edge
  WINDOW        ── present, able to think & feel
  ─────────────────────────────────────────────  lower edge
  HYPOAROUSAL   ── numb, foggy, far away, shutdown
```

- `MindModel.Arousal` runs from -1 (deep shutdown) to +1 (panic). 0 is settled.
- The **window** has a width that **grows permanently** as the player practises skills.
  This is the main progression system, and it mirrors what therapy actually aims for.
- Being outside the window **changes perception, not outcomes**. Nothing is lost and
  nothing fails. The world just gets harder to read until the player grounds.
- **Skills have a direction.** Slow paced breathing helps most with *hyper*arousal.
  Orienting, movement, strong scents and sour tastes help most with *hypo*arousal. A
  mismatched skill still helps a little, which teaches the real lesson gently: if you are
  already numb, deep slow breathing alone may not bring you back.

---

## 3. Dissociative experiences & how they're represented

Following your direction, parts appear in two ways depending on the kind of dissociation.
**Identity-level** experiences (switching, co-consciousness) are **playable and
character-based**. **Perceptual** experiences (depersonalization, derealization, fog)
are **symbolic and environmental**.

| Experience | Representation | Mechanic | Comfort setting cap |
|---|---|---|---|
| **Fog / hypoarousal** | Desaturation, depth-of-field haze, muffled audio, slower walk | Find and engage sensory anchors to clear it | Visual distortion slider |
| **Depersonalization** (feeling outside your body, robotic) | Camera drifts from first person out to an over-the-shoulder view, then further. The player's hands look unfamiliar and footsteps lag. | Body-based skills (feet on floor, temperature, paired muscle relaxation) pull the camera back in | "Disable camera drift" |
| **Derealization** (world feels unreal, flat, dreamlike) | Flattened lighting, wrong scale, paper-thin textures, sound with no source | Orienting skills (name the date, describe five objects, 5-4-3-2-1) | Visual distortion slider |
| **Co-consciousness / passive influence** | Another part's perception layers *over* yours: their colour tint at the screen edge, their glyphs visible, their voice as italic text | Listen, acknowledge and negotiate. Some puzzles need two parts' perceptions at once. | Audio distortion toggle |
| **Switching** | Full change of playable character: different walk, camera height, UI tint, inner voice, abilities to perceive | Voluntary switches need the other part's **consent**. Story switches can be involuntary. | "Ask before involuntary switches" |
| **Time loss** | Hard cut. The player wakes somewhere else with clues around (receipts, a text sent, a half-made tea). | **Gap recovery:** gently reconstruct what happened from clues and parts' testimony. Missing pieces are allowed to stay missing. | Always on, but with a soft fade option |

**Consent is a mechanic.** When the player asks a part to front, the part may say *"not
now"*. That is respected, and it is never a failure. Trust grows through keeping promises
in the inner journal, honouring comfort items, and doing skills *together*.

---

## 4. Structure: The Wheel of the Year (SUPERSEDED: early draft, see §0)

The story is split into eight chapters, one for each sabbat of the modern pagan
**Wheel of the Year**, a widely shared open tradition. Each chapter pairs a seasonal
theme, a part, and a skill family.

| # | Sabbat | Season theme | Featured part (draft) | Skill family | Practice introduced |
|---|---|---|---|---|---|
| 1 | **Samhain** | Thin veils, the year's end | Wren (host) | Grounding: 5-4-3-2-1, orienting | Ancestor altar, hag stone |
| 2 | **Yule** | Darkest night, return of light | Juniper (caretaker) | Paced breathing, self-soothe (5 senses) | Candle work, evergreen, smoky quartz |
| 3 | **Imbolc** | First stirrings, hearth | Pip (little) | Inner safe place, comfort objects | Brigid's cross, lavender, chamomile |
| 4 | **Ostara** | Balance, equal day/night | Bramble (protector) | Wise Mind, STOP | Runes (first ætt) |
| 5 | **Beltane** | Fire, vitality, boundaries | Bramble & Wren | DBT interpersonal skills (DEAR MAN, GIVE, FAST) | Rowan & red thread, carnelian |
| 6 | **Litha** | Longest day, full light | Sorrel (memory-holder) | CBT thought records, cognitive distortions | Tarot (Major Arcana) |
| 7 | **Lughnasadh** | First harvest, craft | Whole system | Emotion regulation: Check the Facts, Opposite Action, PLEASE | Herb drying, witch's ladder |
| 8 | **Mabon** | Gratitude, second harvest | Whole system | Radical acceptance, building a life worth living | Shared altar, the system's own bindrune |

Each chapter follows the same loop:

1. **Outer world day.** Ordinary life with a stressor. Window-of-tolerance pressure builds.
2. **Dissociative episode.** Shown with that chapter's representation.
3. **Inner world.** The cottage, the herb garden, the forest edge. Meet the part, listen, help.
4. **Learn the skill.** A diegetic tutorial, unlocked into the Skill Codex.
5. **Ritual moment.** A cozy crafting or reflection beat: dry herbs, carve a rune, draw a card, journal.
6. **Return.** Use the skill in the outer world. The window widens a little.

### Story premise (SUPERSEDED draft)

> Wren has lost another afternoon. The kettle is cold, there's a sprig of rosemary on the
> counter they don't remember buying, and a note in handwriting that is almost theirs:
> *"We're tired of being a secret from you."*
>
> That night Wren finds the door, and on the other side of it a cottage they've never seen
> but somehow know. The fire is lit. Someone has been keeping it lit for a very long time.

The arc moves from *"something is wrong with me"* to *"there are more of us, and we've
been trying to help"* to *"how do we live well, together?"* It ends at Mabon with the
system choosing **together** what comes next. Possible endings include deeper
cooperation, a shared routine, some parts resting, or a choice to keep working with
their therapist. Every ending is framed as valid.

### Draft cast [DECIDE: the alters still need the owner's confirmation]

These names are placeholders, and none are based on real people. Swap in anything that
feels right.

- **Wren**: the part who handles daily life (work, bills). Tired, kind, doubts themself, and often doesn't remember. Represented: embodied, playable.
- **Juniper**: caretaker who keeps the hearth. Knows the herbs. Patient, warm, a little bossy. Embodied, playable.
- **Pip**: a young part who lives in the herb garden. Loves textures and smells, and is frightened of loud voices. Embodied and playable in safe scenes only.
- **Bramble**: a protector who guards the forest edge. At first appears **only** as thorns, fog and a voice. Prickly and blunt, and has been keeping everyone alive. Becomes embodied as trust grows.
- **Sorrel**: holds hard memories. Appears **symbolically**: tarot imagery, birdsong, a lantern in the trees. Never forced to share. The game respects that some parts carry things that stay private.

---

## 5. The inner world: Hearthwood

A hub you return to, which grows with the system's trust and communication.

- **The Hearth (cottage):** a kitchen and apothecary shelves where herbs are hung and dried. The **Inner Journal** sits on the table. There is a room for each part, and doors open as trust grows.
- **The Herb Garden:** gather and tend real herbs (see `PracticesLoreReference.md`). Every herb has an accurate sensory description and safety notes.
- **The Stone Circle:** where runes are carved and the tarot is drawn, *as reflection*.
- **The Forest Edge:** Bramble's territory. Fog is thickest here early on.
- **The Meeting Table:** based on the "inner meeting place" technique from dissociative-disorder skills training (Boon, Steele & van der Hart, 2011). Parts gather to make decisions together.

The inner world's look follows the window of tolerance. Inside the window it is warm
candlelight, honey-coloured wood and moss. Further out, it becomes liminal, flattened and
fog-bound.

---

## 6. Systems summary

| System | Purpose | Code location |
|---|---|---|
| Window of Tolerance / MindModel | Arousal, window width, dissociative experiences, presence | `Scripts/Core/Mind` |
| System roster & fronting | Members, trust, consent-based switching, co-consciousness | `Scripts/Core/PluralSystem` |
| Inner Journal | Notes between parts, gap recovery, thought records, reflections | `Scripts/Core/PluralSystem` |
| Grounding exercises | Breathing pacer, 5-4-3-2-1, orienting, sensory anchors | `Scripts/Core/Grounding` |
| Skill Codex | DBT/CBT/mindfulness reference with steps, cautions, sources | `Scripts/Core/Skills` + `Resources/Data/skills.json` |
| Thought Records (CBT) | Full Beck-style thought record as a journaling mechanic | `Scripts/Core/Skills` |
| Practices library | Runes, tarot, herbs, crystals, charms. Lore + reflection, **no buffs** | `Scripts/Core/Practices` + JSON |
| Comfort & safety | Anchor button, distortion caps, content notes, crisis resources | `Scripts/Core/Safety` |
| Save | Platform-abstracted storage | `Scripts/Core/Save`, `Scripts/Runtime/Save` |

### Three ways to use the skills (all included)
1. **In-story mechanics.** For example, doing 5-4-3-2-1 lifts the derealization fog, and paced breathing settles a panic spiral.
2. **Real-life Practice Mode.** From the main menu or the Anchor button, the player can run any learned skill *for themselves*, outside the story. Nothing is tracked or judged.
3. **Skill Codex.** Accurate reference cards with steps, when to use them, cautions and sources.
4. **Crisis-safe features.** See `ContentAndSafetyGuidelines.md`.

---

## 7. Art & audio direction

- **Grounded:** hand-painted textures, warm subsurface light, visible craft (stitching, wood grain, chipped enamel). Foley-rich audio: kettle, fire, rain on glass.
- **Shifting:** a URP post-processing stack per experience (see table in §3). Volumes are blended by `DissociationPresentationDriver` and **always capped by Comfort Settings**.
- **No photosensitive triggers.** No strobing and no rapid flashing. Distortion changes are always slow, at least 1.5 s.
- **Music:** a diegetic folk instrument palette (hurdy-gurdy, frame drum, nyckelharpa, voice). Each part has a leitmotif, and co-consciousness layers motifs together.

---

## 8. Out of scope (deliberately)

- Depicting abuse or trauma events on screen.
- Trauma processing mechanics (this is Phase 2 therapeutic work; see `TherapeuticSkillsReference.md` §1).
- Diagnosing the player, or any "test" for dissociative disorders.
- Gameplay stat effects from spiritual practices.
- Closed or culturally specific practices (see `ContentAndSafetyGuidelines.md` §4).

---

## 9. Milestones

| Milestone | Contents |
|---|---|
| **M0 — Scaffold** (this commit) | Docs, data, core systems in C#, tests, Unity project skeleton |
| **M1 — Vertical slice** | Samhain chapter: the apartment, a time-loss gap, the first visit to the Hearth, 5-4-3-2-1 lifting fog, Anchor menu |
| **M2 — Systems complete** | Switching with consent, co-consciousness perception layers, journal UI, codex UI, tarot reflection |
| **M3 — Content** | Chapters 2–8, all parts, full VO/text pass |
| **M4 — Sensitivity & clinical review** | Plural community readers, a clinician familiar with dissociative disorders, a pagan-community reader |
| **M5 — Ports** | Console certification, platform save backends, accessibility audit |

---

## 10. Open questions for you [DECIDE]

1. Should parts' names, looks and pronouns be **customizable**, or should we keep a fixed cast for the story?
2. Do you want an **outer world therapist character**? If so, should they be a supportive presence or appear only occasionally?
3. How much **voice acting** is planned? This affects how co-consciousness "voices" are delivered.
4. Do you want to include the **minor arcana** (56 more cards), or keep the tarot to the 22 major arcana?
5. Should the game have a **"system mode"** where a plural player can log which of *their own* parts is playing? This raises privacy questions, so it would be stored locally only.
