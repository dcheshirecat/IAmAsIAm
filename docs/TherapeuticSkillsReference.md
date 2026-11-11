# Therapeutic Skills Reference

This is the source of truth for every skill in the game. The playable data lives in
`Assets/_Project/Resources/Data/skills.json`, so keep the two in sync. Any change to a
skill's steps needs a source.

## 1. Clinical frame: why the game stays in "stabilization"

The ISSTD *Guidelines for Treating Dissociative Identity Disorder in Adults* (2011), which
are also widely applied to OSDD, describe **phase-oriented treatment**:

1. **Safety, stabilization and symptom reduction.** Grounding, emotion regulation, internal communication and cooperation.
2. **Working through traumatic memories.** Done only with a therapist.
3. **Integration and rehabilitation.** Building a fuller life.

The game teaches **Phase 1 skills only**. That is the material that is safe to practise
alone, and it is the main focus of *Coping with Trauma-Related Dissociation: Skills
Training for Patients and Therapists* (Boon, Steele & van der Hart, 2011).

## 2. Grounding & orienting

| Skill | Steps | Best for | Notes |
|---|---|---|---|
| **5-4-3-2-1** | Name 5 things you see, 4 you can touch, 3 you hear, 2 you smell, 1 you taste | Derealization, fog, early panic | The order can be changed. If a sense isn't available, swap in another. |
| **Orienting to the present** | Say your name, where you are, today's date, your age, and one thing that tells you it is *now* and not *then* | Flashback-like states, time confusion | A core dissociation skill (Boon et al., 2011) |
| **Feet on the floor** | Press your feet down. Notice the floor holding you, then your seat, then your back against the chair. | Depersonalization | |
| **Grounding object** | Hold an object. Describe its temperature, weight, texture and edges. | Any | The game's crystals and stones are used for this |
| **Strong sensory input** | Sour candy, peppermint, a cold pack, a strong scent | Hypoarousal / numbness | This *activates* the body, so use it for shutdown rather than panic |
| **Categories game** | Name, for example, animals A–Z or every blue thing in the room | Racing thoughts, derealization | Engages the thinking brain |

## 3. Breathing

| Pattern | Timing | Default? | Notes |
|---|---|---|---|
| **Paced breathing** (DBT TIPP "P") | Exhale longer than inhale, e.g. in 4 s, out 6 s, about 5–6 breaths/min | ✅ Default | No holds |
| **Box breathing** | In 4, hold 4, out 4, hold 4 | Optional | Breath holds can distress some trauma survivors |
| **Free breath** | Just notice the breath without changing it | Optional | A mindfulness option |

The game's pacer is in `Core/Grounding/BreathingPacer.cs`.

## 4. Mindfulness (DBT core mindfulness, Linehan 2015)

- **Wise Mind:** the overlap of *reasonable mind* and *emotion mind*.
- **"What" skills:** Observe, Describe, Participate.
- **"How" skills:** Non-judgmentally, One-mindfully, Effectively.

## 5. DBT Distress Tolerance

- **STOP:** Stop. Take a step back. Observe. Proceed mindfully.
- **TIPP:** Temperature (cold water on the face), Intense exercise, Paced breathing, Paired muscle relaxation.
  - ⚠️ *Caution:* the cold-water dive response lowers heart rate. Linehan's manual advises medical approval for people with heart conditions, low heart rate, eating disorders, or on beta-blockers.
- **ACCEPTS** (distract): Activities, Contributing, Comparisons, (opposite) Emotions, Pushing away, Thoughts, Sensations.
- **IMPROVE** the moment: Imagery, Meaning, Prayer, Relaxation, One thing in the moment, Vacation, Encouragement.
- **Self-soothe with the five senses.**
- **Radical acceptance.**

## 6. DBT Emotion Regulation

- **Check the Facts.**
- **Opposite Action.**
- **PLEASE:** treat PhysicaL illness, balanced Eating, avoid mood-Altering substances, balanced Sleep, Exercise.

## 7. DBT Interpersonal Effectiveness

- **DEAR MAN:** Describe, Express, Assert, Reinforce. Stay Mindful, Appear confident, Negotiate.
- **GIVE:** (be) Gentle, (act) Interested, Validate, (use an) Easy manner.
- **FAST:** (be) Fair, (no) Apologies (overdone), Stick to values, (be) Truthful.

In the game these are also used **between parts**, as internal negotiation.

## 8. CBT

- **Thought record** (Beck): Situation → Automatic thought → Emotion and intensity (0–100) → Distortion(s) → Evidence for → Evidence against → Balanced thought → Re-rate emotion. Implemented in `Core/Skills/ThoughtRecord.cs`.
- **Cognitive distortions** (Burns, *Feeling Good*, 1980): all-or-nothing thinking, overgeneralization, mental filter, disqualifying the positive, mind reading, fortune telling, magnification/catastrophizing, minimization, emotional reasoning, "should" statements, labeling, personalization, and blame.
- **Behavioral activation:** schedule small, values-based activities.

## 9. Plural-specific communication skills

From *Coping with Trauma-Related Dissociation* and common community practice:
- **Inner meeting place:** an imagined safe space where parts can meet. This is the game's Meeting Table.
- **System journal:** a shared notebook where parts leave each other notes. This is the game's Inner Journal.
- **Agreements:** small promises between parts (e.g. "We'll stop at 10pm to rest"), and keeping them builds trust.
- **Comfort items for little parts:** a soft toy, a favourite smell, a safe blanket.

## Sources

- Linehan, M. M. (2015). *DBT Skills Training Manual* (2nd ed.). Guilford Press.
- Boon, S., Steele, K., & van der Hart, O. (2011). *Coping with Trauma-Related Dissociation*. W. W. Norton.
- International Society for the Study of Trauma and Dissociation (2011). *Guidelines for Treating Dissociative Identity Disorder in Adults, Third Revision*. Journal of Trauma & Dissociation, 12(2), 115–187.
- Beck, J. S. (2020). *Cognitive Behavior Therapy: Basics and Beyond* (3rd ed.). Guilford Press.
- Burns, D. D. (1980). *Feeling Good: The New Mood Therapy*.
- Siegel, D. J. (1999). *The Developing Mind* (window of tolerance).
- American Psychiatric Association (2022). *DSM-5-TR*.
