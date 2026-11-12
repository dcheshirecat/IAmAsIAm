// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 2 (Judaism), Part 1.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. The museum. Chicago, spring 2023. Maya is 24.
//  Owner's beats: Maya moved to Chicago for a job (COVID is skipped; the chapter starts in Chicago in 2023). She works at a
//  museum, where she meets Adeena (27, stern and serious with a heart of gold). Tanya is back in Champaign-Urbana: from
//  now on, texting replaces meeting up. Izzy comes forward on stressful days at work, if triggered.
//  This chapter starts as Maya and unlocks nobody.
//
//  My guesses, for the owner to correct:
//   - Maya is a collections assistant (labels, storage, condition reports). Adeena is the registrar: she knows where every
//     object is and what's wrong with every label.
//   - The museum is a city history museum (placeholder name: "the Lakeshore Museum").
//   - The stressful day is an exhibit install with a deadline and a curator who shouts. Izzy may come forward, and the
//     lost time leaves a text to Adeena that Maya didn't write (Izzy asks her for coffee). Adeena says yes.
// ─────────────────────────────────────────────────────────────────────────────

=== ch2_part1 ===
~ set_age(24)
# cw: religious-content
# cw: time-loss
~ explore_tradition("judaism")
~ text_from("tanya", "Thinking of you. Ostara circle was small this year. Your chair's still saved, even from three hours away.")
-> apartment

= apartment
# place: chicago_apartment
# shot: over-shoulder
# objective: Get to work on time
- (room)
+ {room < 4} [Look: The window]
    The L going by, close enough to wave at. Grey lake light. # look: window-l-train
    -> room
+ {room < 4} [Look: The boxes]
    Still taped. Three years in Chicago, and still taped. "MAYA - BOOKS - CHAMPAIGN." # look: taped-boxes
    -> room
+ {room < 4} [Look: Mira's stone]
    On the dresser, by your keys, where you'll see it. # look: stone-dresser
    -> room
+ [Use: Your phone] -> phone
* [Use: Your keys (go to work)]
-
-> museum

= phone
Your phone. # device: phone
-> apartment.room

= museum
# place: museum_collections
# shot: wide
# objective: Finish the labels for the new exhibit
Adeena: You're Maya. The new labels are yours? # vo: ch2-judaism_adeena_0001
Maya: Yes. Hi. # vo: ch2-judaism_maya_0002
Adeena: Three of them have the wrong accession numbers. One of them has the wrong century. # vo: ch2-judaism_adeena_0003
* [Talk: Adeena (say sorry)]
    ~ record_choice("ch2.p1.labels", "sorry", "The first time you met Adeena, she found mistakes in your labels, and you said sorry.")
    Maya: Sorry. I'll fix them. I'm so sorry. # vo: ch2-judaism_maya_0004
    Adeena: Don't be sorry. Be right. I'll show you where the numbers are. # vo: ch2-judaism_adeena_0005
* [Talk: Adeena (ask which century)]
    ~ record_choice("ch2.p1.labels", "century", "The first time you met Adeena, she found mistakes in your labels, and you asked which century you'd got wrong.")
    Maya: Which century did I give it? # vo: ch2-judaism_maya_0006
    Adeena: The nineteenth. It's a toaster. # vo: ch2-judaism_adeena_0007
    For a second, almost, something like a smile. # look: adeena-almost-smile
-
- (desk)
+ {desk < 3} [Look: Adeena's desk]
    Everything square. A mug that says "Registrars Do It By The Number." A small brass candlestick. # look: adeena-desk
    -> desk
+ {desk < 3} [Look: The object on the cart]
    A toaster, 1920s, chrome gone soft with age. Not nineteenth century. # look: toaster
    -> desk
+ {desk < 3} [Look: The clock]
    The install is at four. It's twenty past one. # look: museum-clock
    -> desk
* [Use: The label printer]
-
~ journal("Note", "Adeena, the registrar. Accession numbers are on the back of the condition report.")
-> install

= install
# place: museum_gallery
# shot: over-shoulder
# objective: Get the case ready before four
~ set_experience("Depersonalization", 0.3)
Curator: Where is the case text? Who had the case text? Somebody tell me why nothing in this room is finished! # vo: ch2-judaism_curator_0008
# cue: raised-voices
- (gallery)
+ {gallery < 3} [Look: The empty case]
    Glass, open. Three empty mounts. Your labels, not printed. # look: empty-case
    -> gallery
+ {gallery < 3} [Look: The other assistants]
    Heads down. Nobody looks at you. Everybody looks at you. # look: assistants-heads-down
    -> gallery
* [Use: Mira's stone (in your pocket)]
    The stone. The white line. # look: stone-pocket
    ~ nudge(-0.1)
* [Wait (keep working)]
-
~ set_experience("TimeLoss", 0.6)
# maybe-switch
-> after

= after
// Lost time (owner: Izzy comes forward on stressful days at work). Whoever fronted, the afternoon is gone, and there's a text
// Maya didn't write. Never pointed out.
~ sent_while_away("izzy", "text", "ex_jewish", "thank u for today!! u saved my life. coffee tomorrow? my treat")
~ text_from("ex_jewish", "Yes. 8:30. Not the place with the line.")
# place: museum_gallery
# shot: wide
# objective:
The case is done. The labels are straight. The clock says ten to six. # look: case-done
Adeena: You did good work this afternoon. You were... different. Funnier. # vo: ch2-judaism_adeena_0009
* [Talk: Adeena (say thank you)]
    ~ record_choice("ch2.p1.after", "thanks", "After the install, Adeena told you you'd done good work. You said thank you, and didn't remember doing it.")
    Maya: Thank you. # vo: ch2-judaism_maya_0010
* [Talk: Adeena (ask what you did)]
    ~ record_choice("ch2.p1.after", "asked", "After the install, you asked Adeena what you'd done. She thought you were joking.")
    Maya: What did I do? # vo: ch2-judaism_maya_0011
    Adeena: You told the curator to "breathe into a paper bag." Kindly. He did. # vo: ch2-judaism_adeena_0012
-
Adeena: See you at eight-thirty. # vo: ch2-judaism_adeena_0013
-> home

= home
# place: chicago_apartment
# shot: close
# objective:
* [Use: Your phone]
    Your phone. # device: phone
-
* [Look: Mira's stone]
    By your keys. Where you'll see it. # look: stone-dresser-evening
-
~ text_to("tanya", "Weird day. I think I made a friend?")
~ text_from("tanya", "That's not weird. That's good. Tell me everything Thursday.")
-> ch2_part2  // the next part of the same chapter (one run)
