// DRAFT: not canon until the owner approves it. Chapter 1 (Christianity), Part 1.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. Thursdays. Champaign-Urbana, spring 2018 (sophomore year). Maya is 19.
//  Owner's beats: after Yule, Maya's life with Tanya and the coven: Thursday classes, her first sabbat, a first reading.
//  The sign that Axel is coming: more and more lost time. Then junior year passes on a black screen that says only
//  "Junior year." and "May 2019." It isn't explained until Chapter 1.5.
//  Played as Maya the first time. An unexpected switch (to an unlocked alter: Nina, Izzy) may happen.
//  Explore-first: The Circle and Maya's room carry it.
//
//  My guesses, for the owner to correct:
//   - Ostara (the spring equinox) is Maya's first sabbat with the coven: seeds planted in paper cups, eggs.
//   - Her first reading is three cards, in Tanya's back room, with Tanya teaching.
//   - The lost time grows across the spring: a missed Thursday, a men's flannel shirt in her laundry, a text to Selene
//     she didn't write. Never explained.
// ─────────────────────────────────────────────────────────────────────────────

=== ch1_part1 ===
~ set_age(19)
# cw: religious-content
# cw: depersonalization
# cw: time-loss
-> thursday

= thursday
# place: shop_the_circle
# shot: wide
# objective: Get to Tanya's class on time
- (backroom)
+ {backroom < 3} [Look: The circle of chairs]
    Six folding chairs. A rug. A candle in the middle, unlit. # look: class-chairs
    -> backroom
+ {backroom < 3} [Look: Selene]
    Selene, cross-legged on a chair, saving the one beside her with her scarf. # look: selene-saving
    -> backroom
* [Use: The chair beside Selene]
-
Tanya: Phones off. Thank you. Feet on the floor. We ground first. # vo: ch1-christianity_tanya_0001
Feet on the floor. Roots. # exercise: feet-on-floor
~ nudge(-0.1)
Tanya: Good. Tonight, three cards each. What was, what is, what could help. And nobody reads their own card out loud until I say so. # vo: ch1-christianity_tanya_0002
Three cards, face down on the rug. # ritual: tarot three-card
Tanya: Maya. What could help? # vo: ch1-christianity_tanya_0003
* [Talk: Tanya (say what you see)]
    ~ record_choice("ch1.p1.reading", "said", "At your first reading, you said out loud what you saw in the card.")
    Maya: {card_name(tarot_help)}. I don't know what it means yet. I think it means... someone stays. # vo: ch1-christianity_maya_0004
    Tanya: Hm. Not a bad first reading. Wrong about two things, right about the important one. # vo: ch1-christianity_tanya_0005
* [Talk: Tanya (say you don't know)]
    ~ record_choice("ch1.p1.reading", "unsure", "At your first reading, you told Tanya you didn't know what the card meant.")
    Maya: I don't know. # vo: ch1-christianity_maya_0006
    Tanya: Good. Then you'll look, instead of guessing. Look at it again. # vo: ch1-christianity_tanya_0007
-
~ journal("TarotReflection", "My first reading, at Tanya's. What could help: {card_name(tarot_help)}.")
-> ostara

= ostara
# place: park_ostara
# shot: wide
# objective: Plant a seed
Selene: Your first sabbat! Okay. Ostara. Spring, balance, eggs, all of it. You plant a seed and say what you want to grow. # vo: ch1-christianity_selene_0008
- (spring)
+ {spring < 3} [Look: The coven]
    A dozen people in the wet grass. Tanya with a basket of paper cups. Somebody's baby in a sling. # look: ostara-coven
    -> spring
+ {spring < 3} [Look: The eggs]
    Eggs dyed with onion skins, orange-brown, set in a ring. # look: ostara-eggs
    -> spring
* [Use: A paper cup of soil]
-
* [Talk: Say what you want to grow (friends)]
    ~ record_choice("ch1.p1.ostara", "friends", "At your first Ostara, you planted a seed for friends.")
    Maya: Friends. People who stay. # vo: ch1-christianity_maya_0009
* [Talk: Say what you want to grow (to be here)]
    ~ record_choice("ch1.p1.ostara", "here", "At your first Ostara, you planted a seed for being here: really here.")
    Maya: Being here. All of the time. # vo: ch1-christianity_maya_0010
* [Wait (say it in your head)]
    ~ record_choice("ch1.p1.ostara", "silent", "At your first Ostara, you planted your seed without saying what it was for.")
    A seed, pressed into the dark with one finger. # look: seed-silent
-
Tanya: Water it. Every day. I'll ask. # vo: ch1-christianity_tanya_0011
~ journal("Note", "Ostara. A seed in a paper cup. Water it every day.")
-> spring_signs

= spring_signs
# place: college_apartment_maya
# shot: close
# objective:
~ set_experience("TimeLoss", 0.4)
~ text_from("tanya", "We missed you tonight. Everything all right?")
~ sent_while_away("axel", "text", "selene", "cant make it thurs. tell T im sorry")
- (room)
+ {not cup} [Look: The windowsill] -> cup
+ {not shirt} [Look: The laundry basket] -> shirt
+ {not date} [Look: Your phone] -> date
* {cup or shirt or date} [Use: Mira's stone] -> fade

= cup
~ record_choice("ch1.p1.signs", "cup", "That spring, your Ostara seedling died. You didn't remember forgetting it.")
The paper cup from Ostara. A seedling, folded over, dry. # look: seedling-dead
-> spring_signs.room

= shirt
~ record_choice("ch1.p1.signs", "shirt", "That spring, you found a man's flannel shirt in your laundry, in your size.")
A flannel shirt, green and black. A man's. Your size. # look: flannel-shirt
~ set_experience("Derealization", 0.3)
-> spring_signs.room

= date
~ record_choice("ch1.p1.signs", "date", "That spring, your phone said it was Friday. The last thing you remembered was Tuesday.")
Friday. The last thing you remember is Tuesday. # look: phone-friday
-> phone

= phone
Your phone. # device: phone
-> spring_signs.room

= fade
The stone. The white line, around and around. # look: stone-line-spring
~ set_experience("TimeLoss", 1.0)
# maybe-switch
-> black_screen

= black_screen
// Owner: almost nothing. Not explained until Chapter 1.5.
# shot: inner
# place: black
Junior year.
May 2019.
-> END
