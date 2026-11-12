// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 5 (Hinduism), Part 1.
// vo-speakers: Maya=maya, Pandit Raghavan=raghavan
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. Rain. Seattle, April 2027. Maya is 28.
//  Owner's beats: Maya moves to Seattle at this point in the game. The guide is a temple priest. The chapter starts as Maya, now
//  more grounded, but still experiencing "visits" from Heather.
//
//  My guesses, for the owner to correct:
//   - Owner: she moves to Seattle for other reasons (to ask), and starts working at the Burke Museum (a real natural history
//     museum, at the University of Washington) almost immediately, as an educator (Claude's guess for the job).
//   - The temple: a South Indian temple on the Eastside (fictional, like the real temples in Bothell and Bellevue), to Sri
//     Venkateswara (Vishnu), with shrines to Shiva, the Goddess and Ganesha.
//   - The priest: "Pandit Raghavan" (placeholder name; about 55, he/him), from a priestly family in South India, trained from
//     boyhood at a Vedic school. People call him Panditji.
//   - Heather's visits: a voice inside, a note in SystemLog, sometimes sharing time.
// ─────────────────────────────────────────────────────────────────────────────

=== ch5_part1 ===
~ set_age(28)
# cw: religious-content
~ explore_tradition("hinduism")
~ gain_app("systemlog")
~ text_from("tanya", "Seattle!! Further away than ever. You'd better text me twice as much.")
~ text_from("chaplain_amal", "Thinking of you on moving day. The rain is supposed to be lovely, once you stop noticing it.")
-> move_in

= move_in
# place: seattle_apartment
# shot: over-shoulder
# objective: Unpack
- (boxes)
+ {boxes < 4} [Look: The boxes]
    "MAYA - BOOKS." "IZZY - DO NOT OPEN (tarot)." "H - kitchen, the good knife." Different handwriting on every box. All of you packed. # look: labelled-boxes
    -> boxes
+ {boxes < 4} [Look: The window]
    Rain on the glass, soft and endless. Somewhere behind it, a mountain you've been promised but haven't seen. # look: seattle-rain
    -> boxes
+ {boxes < 4} [Look: The windowsill]
    You put Mira's stone there first. Then the little brass prayer wheel. Then the khata, folded. # look: sill-unpacked
    -> boxes
* {boxes >= 2} [Use: The boxes (unpack, this time)]
    ~ record_choice("ch5.p1.unpack", "unpacked", "In Seattle, you unpacked every box in the first week. In Chicago they stayed taped for three years.")
    Every box, open. Books on shelves. It takes a week. In Chicago it took never. # look: boxes-unpacked
-
~ system_log("heather", "Kitchen's done. The good knife is in the second drawer. Proud of you. -H")
Heather (inside): Water first. # vo: ch5-hinduism_heather-inside_0001
Maya: I know. I know. I'm getting it. # vo: ch5-hinduism_maya_0002
-> work

= work
# place: burke_museum_hall
# shot: wide
# objective: Find your new desk
- (hall)
+ {hall < 3} [Look: The whale]
    A whale skeleton, hanging over everything, every bone numbered. You laugh. Of course there's a whale. # look: burke-whale
    -> hall
+ {hall < 3} [Look: Your badge]
    "MAYA - EDUCATOR." Just Maya. You can be just Maya here, at work. The others can have the evenings. # look: educator-badge
    -> hall
* {hall >= 1} [Talk: Priya]
-
Priya: You're the new educator? From Chicago? I'm Priya, collections. You look like you need a walk and a coffee and someone to tell you where the good bathroom is. # vo: ch5-hinduism_priya_0003
Priya: Also. My family's temple has a festival on Sunday, Rama Navami. Lots of food. You should come. You don't have to do anything, just come. # vo: ch5-hinduism_priya_0004
* [Talk: Priya (say yes)]
    ~ record_choice("ch5.p1.priya", "yes", "In your first week in Seattle, you said yes to Priya's family's temple festival.")
    Maya: Yes. I'd like that. Really. # vo: ch5-hinduism_maya_0005
* [Talk: Priya (ask what to wear)]
    ~ record_choice("ch5.p1.priya", "wear", "In your first week in Seattle, you asked Priya what to wear to her temple. She said: something you can take your shoes off in.")
    Maya: What do I wear? What do I do? # vo: ch5-hinduism_maya_0006
    Priya: Something you can sit on the floor in. Shoes off at the door. Copy my mom. Everybody copies my mom. # vo: ch5-hinduism_priya_0007
-
-> temple

= temple
// A South Indian temple: the main shrine to Sri Venkateswara (Vishnu), smaller shrines around the hall. Visitors watch darshan
// and aarti, ring the bell, and receive prasad with the right hand.
# place: eastside_mandir_hall
# shot: wide
# objective: Take your shoes off and go in
- (hall)
+ {hall < 5} [Look: The shoe racks]
    A hundred pairs of shoes, and children's sandals, and yours, at the end. Bare feet on cool stone. # look: temple-shoes
    -> hall
+ {hall < 5} [Use: The bell]
    A brass bell over the door. Priya's niece shows you: ring it as you come in, to tell God you're here, and to tell yourself. # look: temple-bell
    -> hall
+ {hall < 5} [Look: The main shrine]
    Venkateswara, black stone and gold, almost hidden under garlands of marigold and jasmine. His eyes are covered: they say his gaze is too strong. # look: venkateswara
    -> hall
+ {hall < 5} [Look: The side shrines]
    Ganesha, round and smiling, by the door. Shiva, a smooth dark stone, wet with milk. The Goddess, in red silk. Everyone has somewhere to go. # look: side-shrines
    -> hall
* {hall >= 2} [Wait (stand with Priya's family)]
-
~ set_experience("Derealization", 0.2)
The aarti: the priest circling a tray of flames before the shrine, everyone singing, a bell, a drum, a conch. # look: aarti-flame
Pandit Raghavan: Come. Come, closer. You can take the light: hands over the flame, then over your eyes. Like this. # vo: ch5-hinduism_raghavan_0008
* [Use: The aarti flame (take the light)]
    ~ record_choice("ch5.p1.aarti", "light", "At your first aarti, you held your hands over the flame and then over your eyes, the way Panditji showed you.")
    Warm, over your palms. Warm, over your eyes. # look: take-the-light
    ~ nudge(-0.2)
* [Wait (just watch)]
    ~ record_choice("ch5.p1.aarti", "watch", "At your first aarti, you watched from the side. Panditji smiled and said watching is also fine.")
    Pandit Raghavan: Watching is also fine. God is not offended by shy people. # vo: ch5-hinduism_raghavan_0009
-
Heather (inside): A flame, held up for everyone. I like him. # vo: ch5-hinduism_heather-inside_0010
Pandit Raghavan: Prasad. Right hand, please. The sweet one is the best one. Don't tell the others. # vo: ch5-hinduism_raghavan_0011
~ journal("Note", "Rama Navami at the temple. Shoes off. Ring the bell. Take the light. Right hand for prasad.")
-> ch5_part2  // the next part of the same chapter (one run)
