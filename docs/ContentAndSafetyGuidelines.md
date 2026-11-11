# Content & Safety Guidelines

Every writer, artist and programmer on the project should read this. These are rules,
not suggestions. When in doubt, choose the gentler option and ask.

## 1. The game is not therapy

- The game must show a short, non-dismissable notice on the first launch. It must also stay visible in the Codex and the Anchor menu:
  > *I Am As I Am teaches real coping skills, but it is not a substitute for professional
  > care. If you are in danger or crisis, please contact local emergency services or a
  > crisis line (Anchor menu → Support).*
- The game never diagnoses, assesses or scores the player's mental health.
- No analytics or telemetry on skill use, journal contents or "system mode" data. All of it is stored **locally only**.

## 2. Representing OSDD & dissociation

**Accuracy**
- OSDD is a real diagnosis: *Other Specified Dissociative Disorder* (DSM-5-TR). The game's focus matches its first example, "chronic and recurrent syndromes of mixed dissociative symptoms": identity disturbance less marked than DID, or intrusions without full amnesia. ICD-11 lists the related diagnosis *partial dissociative identity disorder* (6B65).
- Dissociation is a **protective response**, usually developed in response to overwhelming experiences in childhood.
- Amnesia in OSDD may be partial or absent. Many systems experience **co-consciousness**, **passive influence** and **"blurry" fronting** more than clean switches. Show the full range.

**Never**
- Make a part a villain, a monster, or "the evil alter". Protectors and persecutor-presenting parts are trying to help.
- Use dissociation for horror, jump scares, or "the killer was the alter" twists.
- Present fusion or integration as the only good ending, or as a bad one.
- Force a part to disclose memories. "I don't want to talk about that" ends the conversation, every time.
- Show switching as a fun power-up with no cost or meaning.
- Imply that someone is "faking", or debate whether plurality is real.

**Always**
- Use the language the community uses: *system, parts, headmates, alters, fronting, co-fronting, switching, host, little, protector, gatekeeper*. Let characters disagree about the terms they prefer.
- Give the player a part of the cast they'll relate to, and a part they'll need to learn to understand.
- Show daily-life texture: lost time, unfamiliar handwriting, a calendar full of notes, the tiredness.

## 3. Trauma

- Trauma is **referenced, never depicted**. The game never shows abuse, assault or violence against children.
- References come through absence and safety. For example: "Pip won't go near the cellar door. That's okay. The cellar stays shut."
- Self-harm and suicidal ideation are **not depicted**. If the story references them at all, they must be tagged `self-harm-reference` and handled with an approved script plus a crisis-resource prompt.
- Content notes are shown before each chapter, with tags from `ContentWarnings.cs`. Players can choose to skip tagged scenes and still get a summary.

## 4. Pagan & spiritual practices

**Accuracy**
- Use real, sourced meanings: the Elder Futhark rune names and the Rune Poems, Rider-Waite-Smith tarot, documented herb folklore.
- Say clearly when something is a **modern** association. For example, most rune "divinatory" meanings are 20th century. Crystal metaphysical properties are modern New Age beliefs.
- Present practices respectfully as **meaning-making and attention**, neither as proven science nor as nonsense.

**No gameplay effects.** Crystals, herbs, amulets and runes **never** give stat bonuses.
They work the way they do for many real practitioners:
- **Sensory anchors:** the cool weight of hematite, the smell of lavender. This connects directly to grounding skills.
- **Ritual as attention:** the act of tying a witch's ladder is a mindfulness practice ("one thing in the moment").
- **Reflection prompts:** tarot and runes prompt journaling, never predictions.

**Open practices only.** We avoid closed and culturally specific practices, including:
- White sage "smudging" and other Indigenous North American ceremonial practices. This also includes dreamcatchers and medicine wheels.
- Hoodoo, Vodou, Santería/Lucumí, Candomblé and other African diasporic religions.
- Practices from closed or initiatory traditions, presented as if they were open.
- Instead, use: garden herbs, open European folk practices (hag stones, rowan crosses, witch bottles, Brigid's crosses), the modern pagan Wheel of the Year, personal sigils and bindrunes, and candle and altar work.

**Runes and hate symbols.** Some runes (notably Othala, Algiz, Tiwaz and Sowilo in certain
stylizations) have been misused by white supremacist groups. The game presents runes in
historical and linguistic context. It must never use hate-group variants (e.g. Othala with
"wings"/serifs, double Sowilo), and the Codex notes that inclusive heathen communities
reject this misuse.

**Herbs: physical safety.** Every herb entry carries safety notes, and the game never
gives dosages or tells the player to ingest anything. This matters especially for St
John's wort, which **interacts with many medications including antidepressants and
hormonal contraception**. Our audience is likely to be on psychiatric medication, so this
warning is non-negotiable.

## 5. Therapeutic skills

- Skill steps must match the source material (see `TherapeuticSkillsReference.md`).
- Each skill entry includes **cautions**. For example, the TIPP *temperature* skill (cold water) is not recommended for people with heart conditions or eating disorders without medical advice.
- **Breath holds are optional.** The default breathing exercise is paced breathing with no holds, because breath holds can be distressing for some trauma survivors. Box breathing is offered as a choice.
- The game stays in **stabilization**. It teaches safety, grounding and communication skills, and never attempts trauma processing.

## 6. Player comfort features (must ship)

| Feature | Default |
|---|---|
| **Anchor button**: pauses everything, opens the safe space with breathing, grounding and support links | Always available, including in cutscenes |
| Visual distortion intensity cap | 100%, adjustable to 0% |
| Disable camera drift (depersonalization) | Off |
| Disable audio distortion | Off |
| Ask before involuntary switches | Off (story intent), opt-in |
| Content notes before chapters | On |
| Skip tagged content (with summary) | Per tag |
| Tarot reversals | Off |
| Breath holds in exercises | Off |
| Reduce motion | Follows OS setting where available |
| Text speed / auto-advance | Player choice |

## 7. Review process

Before any chapter ships:
1. A plural or dissociative community sensitivity reader.
2. A clinician familiar with dissociative disorders and DBT.
3. A reader from an open pagan or heathen community (for practice content).
4. A photosensitivity check (Harding test or equivalent) on all distortion effects.
