// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Prelude (Wicca), Part 2.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Things moved. The same town, March 2017 (freshman spring). Maya is 18.
//  Owner's beats: lost time grows: more blackouts, and signs of others (notes, things moved). Maya
//  doesn't understand it yet. She isn't serious about Wicca yet.
//  Explore-first: Maya's apartment is the scene. The signs are things to find, never explained.
//  Hidden memory (owner's rule): the tarot deck glows only while Izzy is fronting, and shows Izzy buying
//  it. Nothing ever tells the player; for anyone else it's simply not one of the glowing things.
//
//  My guesses, for the owner to correct:
//   - (Owner: yes) The signs are Izzy's (22 in the present; Wiccan; runs daily life on the lost-time days): a note
//     signed "I", candles on the windowsill, a tarot deck Maya didn't buy, groceries she didn't buy.
//   - Maya missed a midterm she has no memory of missing.
//   - Selene is drifting: she has a girlfriend now and less time. She saw "Maya" at a full moon meet-up
//     at the shop that Maya doesn't remember.
//   - The shop is "The Circle" (owner), a small witchcraft shop in town.
// ─────────────────────────────────────────────────────────────────────────────

=== prelude_part2 ===
~ set_age(18)
// Lost time on the phone (owner: subtle, never pointed out): a text Maya didn't send is just there.
~ sent_while_away("izzy", "text", "selene", "omg yes. tues!! im bringing the oat milk")
# cw: religious-content
# cw: depersonalization
# cw: derealization
# cw: time-loss
-> apartment

= apartment
# place: college_apartment_maya
# shot: close
# objective: Get ready for class
~ set_experience("TimeLoss", 0.3)
- (things)
+ {not note} [Look: The fridge] -> note
+ {not sill} [Look: The windowsill] -> sill
+ {not deck and fronts("izzy")} [Look: The tarot deck] -> deck
+ {not exam} [Look: The calendar] -> exam
+ [Use: Your phone] -> phone
* {note or sill or exam} [Use: Your keys (go to class)] -> late

= note
~ record_choice("prelude.p2.signs", "note", "That spring, you found a note on the fridge in handwriting that wasn't yours.")
"Full moon Tues!! Don't forget. Bought oat milk. -I" Round letters. Not yours. # look: fridge-note
~ set_experience("Derealization", 0.3)
-> apartment.things

= sill
~ record_choice("prelude.p2.signs", "candles", "That spring, you found candles on your windowsill that you didn't buy.")
Three candles on the windowsill. White, silver, black. Burned halfway down. # look: three-candles
-> apartment.things

= deck
~ record_choice("prelude.p2.signs", "deck", "That spring, you looked at the tarot deck, and remembered buying it.")
A tarot deck, the box soft at the corners. # look: tarot-deck
-> izzy_memory

= izzy_memory
// Hidden memory (owner): only reachable while Izzy is fronting. Izzy buying the deck.
# memory: izzy-the-deck
# shot: inner
* [Look: The shelf]
    Decks, a whole wall of them. This one, with the moon on the box. # look: izzy-shelf
-
* [Look: The person at the counter]
    Short grey hair. Reading glasses on a chain. A look over the top of them, kind, and not fooled. # look: izzy-counter
-
* [Use: The deck]
    Cash from the jar. A paper bag. A flyer tucked inside it: "Classes, Thursdays." # look: izzy-bag
-
-> apartment.things

= exam
~ record_choice("prelude.p2.signs", "exam", "That spring, your calendar said you'd missed a midterm. You had no memory of the day.")
"PSYCH 216 MIDTERM, 9 AM." Tuesday. Today is Thursday. # look: missed-exam
~ nudge(0.2)
-> apartment.things

= phone
Your phone. # device: phone
-> apartment.things

= late
# place: college_quad
# shot: over-shoulder
# objective: Find Selene
Selene: Maya! Hey! Last night was so fun. You were so funny. I didn't know you could do voices. # vo: prelude-wicca_selene_0024
~ set_experience("Derealization", 0.5)
~ nudge(0.25)
* [Talk: Selene (pretend you remember)]
    ~ record_choice("prelude.p2.selene", "pretended", "When Selene talked about a night you didn't remember, you pretended you did.")
    Maya: Ha. Yeah. It was fun. # vo: prelude-wicca_maya_0025
    Selene: The full moon thing at the shop. You knew all the words to the chant! Since when? # vo: prelude-wicca_selene_0026
* [Talk: Selene (ask what you did)]
    ~ record_choice("prelude.p2.selene", "asked", "When Selene talked about a night you didn't remember, you asked what you'd done.")
    Maya: What did I do? Last night. I'm... I was really tired. # vo: prelude-wicca_maya_0027
    Selene: You came to the full moon thing at the shop! You did the chant. You did the chant really loud. # vo: prelude-wicca_selene_0028
* [Look: The ground]
    ~ record_choice("prelude.p2.selene", "silent", "When Selene talked about a night you didn't remember, you looked at the ground and didn't say anything.")
    Bricks. Moss in the cracks. You count them. # look: quad-bricks
-
Selene: I have to run, Jess is waiting. We should get coffee! For real this time. # vo: prelude-wicca_selene_0029
* [Look: Selene, going]
    Selene's scarf, getting smaller across the quad. # look: selene-going
-
~ journal("Note", "Full moon. The shop. Last night. I don't remember.")
# objective:
* [Use: Mira's stone]
    The stone's white line, all the way around. Around and around. # look: stone-line-quad
* [Use: Feet on the floor]
    Bricks under your shoes. Still here. # look: feet-bricks
-
~ set_experience("Derealization", 0.0)
~ set_experience("TimeLoss", 0.0)
-> prelude_part3  // the next part of the same chapter (one run)
