// IN THE GAME as a DRAFT (Claude, 2026-11-13. The owner's Doc edits come first). Chapter 8 (Druidry), Part 3.
// vo-speakers: Maya=maya, Bríd=brid, Lily=lily
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Samhain. County Meath, October 2030. Maya is 32.
//  Research: Tlachtga, the Hill of Ward near Athboy, where tradition says the great Samhain fire was lit. A fire festival is
//  held there again in modern times. Samhain: the dark half of the year begins. The dead are remembered.
//  My guesses, for the owner to correct: the grove remembers its dead by name at the fire. Maya names Mira, her grandparents,
//  and (Lily, sharing time) "the lady at church". The last night at Bríd's. She leaves for Seattle on All Souls'. `# chapter-end`
//  unlocks Lily (owner).
// ─────────────────────────────────────────────────────────────────────────────

=== ch8_part3 ===
~ set_age(32)
~ set_date(2030, 10, 31)  // Samhain (Claude's guess)
# cw: religious-content
# cw: childhood-reference
~ co_conscious("lily", true)
-> wake

= wake
# place: brid_kitchen
# shot: wide
# objective: Help get ready for Samhain
- (k)
+ {k < 3} [Look: The turnip]
    A turnip, not a pumpkin. Bríd carves the old way: a scowling face, a candle inside. Lily (inside) thinks it's the funniest thing she's ever seen. # look: turnip-lantern
    -> k
+ {k < 3} [Look: The barmbrack]
    The Halloween brack, fruit bread with a ring baked inside. Whoever gets the ring gets married, Bríd says. "Or doesn't. It's only bread." # look: barmbrack
    -> k
* [Use: The kettle]
    Tea for the whole grove, in the big brown pot. You've learned how Bríd takes hers: like a brick. # look: tea-like-a-brick
-
-> tlachtga

= tlachtga
# place: tlachtga_hill
# shot: wide
# objective: Climb Tlachtga
- (hill)
+ {hill < 4} [Look: The rings]
    Grassy rings in the hilltop, old banks and ditches, older than anyone's god. # look: tlachtga-rings
    -> hill
+ {hill < 4} [Look: The torches]
    A line of torches coming up from Athboy in the dark, swaying, singing. # look: torch-procession
    -> hill
+ {hill < 4} [Look: The grove]
    Bríd, Josie, the Dublin OBOD man and the Gaelic polytheist arguing happily about what to call the night. Samhain. They agree on that. # look: grove-argue
    -> hill
* {hill >= 2} [Wait (for the fire)]
-
The Samhain fire goes up. The dark half of the year begins. # look: samhain-fire
Bríd: Now we say the names of our dead, out loud, so the fire carries them. Anyone who wants. # vo: ch8-druidry_brid_0018
* [Talk: The fire (say Mira's name)]
    ~ record_choice("ch8.p3.names", "mira", "At the Samhain fire on Tlachtga you said Mira's name out loud.")
    Maya: Mira Smith. Who tried. # vo: ch8-druidry_maya_0019
    ~ honour_ancestor("guide_quaker")
* [Talk: The fire (say your grandparents' names)]
    ~ record_choice("ch8.p3.names", "grandparents", "At the Samhain fire on Tlachtga you said your grandparents' names out loud.")
    Maya: June and Walter. I hardly knew you. Hello. # vo: ch8-druidry_maya_0020
    ~ honour_ancestor("grandma_june")
-
Lily (inside): And the lady. The church lady. With the crackers. # vo: ch8-druidry_lily-inside_0021
Maya: And Mira again, from Lily. With the crackers. # vo: ch8-druidry_maya_0022
~ nudge(-0.3)
-> last_night

= last_night
# place: brid_kitchen
# shot: close
# objective: Say goodbye
- (k)
+ {k < 2} [Look: Your bag]
    Packed. A bag of rushes Bríd says to weave into a cross on the first of February. A jar of bilberry jam for Tanya. # look: packed-rushes
    -> k
* [Talk: Bríd]
-
Bríd: You'll come back. Bealtaine, or whenever. Bring all of them. The little one especially. # vo: ch8-druidry_brid_0023
Maya: Thank you for waiting with her. # vo: ch8-druidry_maya_0024
Bríd: Ah. Someone should have, long ago. Somebody does now. # vo: ch8-druidry_brid_0025
Lily (inside): Bye, lady. # vo: ch8-druidry_lily-inside_0026
~ text_to("tanya", "Coming home tomorrow. Lily is with us now. Bríd waited with her at a well. You knew she would, didn't you.")
~ text_from("tanya", "I hoped. Bríd waits better than anyone I know. Welcome, Lily.")
# shot: inner
# place: black
County Meath. November 2030. # chapter-end
-> END
