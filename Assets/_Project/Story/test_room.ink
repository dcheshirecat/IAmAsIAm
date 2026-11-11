// ─────────────────────────────────────────────────────────────────────────────
//  TEST ROOM: placeholder interactions for the playable base. NOT story content.
//  Each knot exists only to prove a system works (dialogue, choices, exercises,
//  rituals, the codex). The real chapters replace these once the owner has
//  designed them.
// ─────────────────────────────────────────────────────────────────────────────

=== test_kettle ===
# shot: close
(Test) A kettle. Walking up to things and looking at them works.
-> END

=== test_tarot ===
(Test) A tarot deck on the table.
* [Draw a card for today.]
    You shuffle and draw. # ritual: tarot daily-card
    (Test) You drew {card_name(tarot_today)}.
    {card_prompt(tarot_today)}
* [Lay out three cards.]
    You lay out three cards. # ritual: tarot three-card
    (Test) What could help: {card_name(tarot_help)}.
* [Leave it for now.]
    (Test) Leaving is always a choice.
-
-> END

=== test_altar ===
(Test) The altar shelf: a candle, a bowl, a stone.
* [Sit and breathe for a while.]
    You sit down in front of the altar. # exercise: paced-breathing
    (Test) Breathing exercise finished.
* [Ground: five things you can see.]
    You look around the room. # exercise: five-senses
    (Test) Grounding exercise finished.
* [Make a rowan cross.]
    You take two twigs and the red thread. # ritual: craft rowan-cross
    (Test) Crafting finished.
-
-> END

=== test_guide ===
# shot: over-shoulder
Guide: (Test) I'm a placeholder for a chapter's guide. Talking works, and so do choices.
* [Ask about Quakers.]
    ~ record_choice("test.guide", "quakers", "(Test) You asked about Quakers.")
    Guide: (Test) Here's how a tradition's codex page opens. # explore: quakers
* [Ask about Wicca.]
    ~ record_choice("test.guide", "wicca", "(Test) You asked about Wicca.")
    Guide: (Test) Here's how a tradition's codex page opens. # explore: wicca
* [Say goodbye.]
    ~ record_choice("test.guide", "goodbye", "(Test) You said goodbye.")
    Guide: (Test) Goodbye for now.
-
Guide: (Test) And this is the end-of-chapter recap. # chapter-end
-> END
