// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 2 (Judaism), Part 1.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. The museum. Chicago, spring 2023. Maya is 24.
//  Owner's beats: Maya moved to Chicago for a job (COVID is skipped; the chapter starts in Chicago in 2023). She works at a
//  museum (The Field Museum, as a tour guide), where she meets Adeena (27, stern and serious with a heart of gold). Tanya is back in Champaign-Urbana: from
//  now on, texting replaces meeting up. Izzy comes forward on stressful days at work, if triggered.
//  This chapter starts as Maya and unlocks nobody.
//
//  My guesses, for the owner to correct:
//  Owner: the museum is The Field Museum (the real place); Maya is a tour guide; the shouting curator is Eudora Jones.
//  My guesses, for the owner to correct:
//   - Adeena is the registrar: she knows where every object is and every fact about it.
//   - The stressful day is a members' preview of a new exhibit, with three school groups double-booked and Eudora shouting.
//     Izzy may come forward, and the lost time leaves a text to Adeena that Maya didn't write (coffee). Adeena says yes.
// ─────────────────────────────────────────────────────────────────────────────

=== ch2_part1 ===
~ set_age(24)
~ set_date(2023, 4, 17)  // Claude's guess at the exact day (owner: change it freely)
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
# place: field_museum_hall
# shot: wide
# objective: Give the ten o'clock tour
// The Field Museum (owner: the real place). Stanley Field Hall, with Máximo the titanosaur; SUE the T. rex upstairs.
- (hall)
+ {hall < 3} [Look: Máximo]
    The titanosaur, neck up into the light, longer than the hall feels. The fourth-graders always gasp. You still do, a little. # look: maximo
    -> hall
+ {hall < 3} [Look: The tour group]
    Forty fourth-graders in matching yellow shirts. Two teachers counting heads, again. # look: tour-group
    -> hall
+ {hall < 3} [Look: Your notes]
    Index cards, soft at the corners. "SUE: biggest, most complete T. rex. Tell them about the broken ribs." # look: tour-notes
    -> hall
* {hall >= 1} [Wait (start the tour)]
-
Adeena: You're Maya. The new guide. # vo: ch2-judaism_adeena_0001
Maya: Yes. Hi. # vo: ch2-judaism_maya_0002
Adeena: You just told forty children that SUE is sixty-seven thousand years old. # vo: ch2-judaism_adeena_0003
* [Talk: Adeena (say sorry)]
    ~ record_choice("ch2.p1.labels", "sorry", "The first time you met Adeena, she corrected your tour, and you said sorry.")
    Maya: Sorry. I'll fix it. I'm so sorry. # vo: ch2-judaism_maya_0004
    Adeena: Don't be sorry. Be right. Million. Sixty-seven million. I'll show you where the fact sheets are. # vo: ch2-judaism_adeena_0005
* [Talk: Adeena (ask how old she is)]
    ~ record_choice("ch2.p1.labels", "century", "The first time you met Adeena, she corrected your tour, and you asked how old SUE really is.")
    Maya: How old is she, then? # vo: ch2-judaism_maya_0006
    Adeena: Sixty-seven million. You were off by a factor of a thousand. The children won't remember. I will. # vo: ch2-judaism_adeena_0007
    For a second, almost, something like a smile. # look: adeena-almost-smile
-
-> office

= office
# place: field_museum_office
# shot: over-shoulder
# objective: Find the fact sheets
- (desk)
+ {desk < 3} [Look: Adeena's desk]
    Everything square. A mug that says "Registrars Do It By The Number." A small brass candlestick. # look: adeena-desk
    -> desk
+ {desk < 3} [Look: The fact sheets]
    A binder, colour-coded, labelled in Adeena's tiny capitals. "SUE." "MÁXIMO." "TSAVO LIONS." "ASK ME FIRST." # look: fact-sheets
    -> desk
+ {desk < 3} [Look: The clock]
    The members' preview is at four. It's twenty past one. # look: museum-clock
    -> desk
* [Talk: Christie]
    // Owner: Christie, an old friend from work: the one who, years later (Chapter 6), tells Maya about Adeena's wedding.
    ~ record_choice("ch2.p1.christie", "met", "On your first week at the museum, Christie, another guide, shared her snack drawer with you.")
    Christie: New guide? Christie. I've done the dinosaur tour four hundred times. Snack drawer's here. Don't tell Adeena it exists. # vo: ch2-judaism_christie_0067
    Christie: Kidding. She knows. She pretends not to. That's love, at the Field. # vo: ch2-judaism_christie_0068
    -> desk
* {desk >= 1} [Go: Back to the floor]
-
~ journal("Note", "Adeena, the registrar. SUE is sixty-seven MILLION years old. The fact sheets are in her binder.")
-> install

= install
# place: field_museum_gallery
# shot: over-shoulder
# objective: Get the preview tour ready before four
~ set_experience("Depersonalization", 0.3)
Eudora Jones: Who booked three school groups into this hall at once? Somebody tell me why nothing about today is working! # vo: ch2-judaism_eudora_0008
# cue: raised-voices
- (gallery)
+ {gallery < 3} [Look: The new exhibit]
    Half-lit cases. A ladder. Labels still in their plastic. Four o'clock, and it isn't ready. # look: empty-case
    -> gallery
+ {gallery < 3} [Look: The other guides]
    Heads down. Nobody looks at you. Everybody looks at you. # look: assistants-heads-down
    -> gallery
* [Use: Mira's stone (in your pocket)]
    The stone. Your thumb in the hole. # look: stone-pocket
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
# place: field_museum_gallery
# shot: wide
# objective:
The preview is over. Everyone's gone. The clock says ten to six, and the afternoon is just... gone. # look: case-done
Adeena: You did good work this afternoon. You were... different. Funnier. # vo: ch2-judaism_adeena_0009
* [Talk: Adeena (say thank you)]
    ~ record_choice("ch2.p1.after", "thanks", "After the install, Adeena told you you'd done good work. You said thank you, and didn't remember doing it.")
    Maya: Thank you. # vo: ch2-judaism_maya_0010
* [Talk: Adeena (ask what you did)]
    ~ record_choice("ch2.p1.after", "asked", "After the install, you asked Adeena what you'd done. She thought you were joking.")
    Maya: What did I do? # vo: ch2-judaism_maya_0011
    Adeena: You told Eudora to "breathe into a paper bag." Kindly. She did. # vo: ch2-judaism_adeena_0012
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
