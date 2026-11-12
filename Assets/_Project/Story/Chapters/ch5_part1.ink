// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 5 (Hinduism), Part 1.
// vo-speakers: Maya=maya, Mangala Tai=mangala
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. Rain. Seattle, April 2027. Maya is 28.
//  Owner's beats: Maya moves to Seattle at this point in the game. The guide is a temple priest. The chapter starts as Maya, now
//  more grounded, but still experiencing "visits" from Heather.
//
//  My guesses, for the owner to correct:
//   - Owner: she can't stand the memories Chicago brings up, and believes Seattle will be a fresh start. She starts working at
//     the Burke Museum (a real natural history museum, at the University of Washington) almost immediately, as an educator
//     (Claude's guess for the job).
//   - The temple (owner: a woman priest, so a North Indian-style community mandir, fictional): Rama and Sita at the centre,
//     with Krishna and Radha, Durga, a Shiva lingam, Ganesha and Hanuman. Community mandirs in the US often have many shrines.
//   - The priest (owner): Mangala Joshi, 62, she/her, called "Mangala Tai" (Tai: older sister, in Marathi). From a
//     Maharashtrian Brahmin family; her father was a priest; trained as a purohita (a woman priest) in Pune in her thirties,
//     the way women have been trained there since the 1970s.
//   - Heather's visits: a voice inside, a note in SystemLog, sometimes sharing time.
// ─────────────────────────────────────────────────────────────────────────────

=== ch5_part1 ===
~ set_age(28)
# cw: religious-content
~ explore_tradition("hinduism")
~ gain_app("systemlog")
~ text_from("tanya", "Seattle!! Further away than ever. You'd better text me twice as much.")
~ text_from("chaplain_amal", "Thinking of you on moving day. The rain is supposed to be lovely, once you stop noticing it.")
// Owner: cleansing when she moves into a new home. Claude's addition: no new voiced lines.
~ text_from("tanya", "Cleanse it before the boxes. Window open, room by room. Say what you want it to hold. A fresh start deserves a clean one.")
~ learn_practice("cleansing", "tanya")
-> move_in

= move_in
# place: seattle_apartment
# shot: over-shoulder
# objective: Unpack
- (boxes)
+ {boxes < 4} [Look: The boxes]
    "MAYA - BOOKS." "IZZY - DO NOT OPEN (tarot)." "H - kitchen, the good knife." Different handwriting on every box. All of you packed. Nothing in them from Chicago that you could leave behind. # look: labelled-boxes
    -> boxes
+ {boxes < 4} [Look: The window]
    Rain on the glass, soft and endless. Somewhere behind it, a mountain you've been promised but haven't seen. # look: seattle-rain
    -> boxes
+ {boxes < 4} [Look: The windowsill]
    You put Mira's stone there first. Then the little brass prayer wheel. Then the khata, folded. # look: sill-unpacked
    -> boxes
* [Look: The parcel]
    A parcel from Champaign on the step. A moonstone in tissue paper, and Tanya's square letters: "For the new moon, and the new home." # look: tanya-moonstone
    ~ gain_stone("moonstone", "tanya")
    -> boxes
* [Look: The tree outside]
    A rowan by the steps, last autumn's berries still hanging on. You take one small sprig. # look: seattle-rowan
    ~ gain_herb("rowan", "")
    -> boxes
* [Use: The doorway (cleanse the new place)]
    Wet air and cedar through the window. Nothing in here has been yours yet. # ritual: cleansing
    -> boxes
* {boxes >= 2} [Use: The boxes (unpack, this time)]
    ~ record_choice("ch5.p1.unpack", "unpacked", "In Seattle, you unpacked every box in the first week. In Chicago they stayed taped for three years.")
    Every box, open. Books on shelves. It takes a week. In Chicago it took never. # look: boxes-unpacked
-
~ system_log("heather", "Kitchen's done. The good knife is in the second drawer. Proud of you. -H")
// Owner: she couldn't stand the memories Chicago brought up; Seattle is meant to be a fresh start.
Heather (inside): Water first. # vo: ch5-hinduism_heather-inside_0001
Maya: I know. I know. I'm getting it. # vo: ch5-hinduism_maya_0002
Maya: No Field Museum. No Friday candles. No lake. Nothing here knows me yet. A fresh start. # vo: ch5-hinduism_maya_0031
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
// A community mandir: Rama and Sita at the centre, shrines to Krishna and Radha, Durga, Shiva, Ganesha and Hanuman around the
// hall. Visitors take darshan and aarti, ring the bell, and receive prasad with the right hand. Rama Navami is Rama's birthday.
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
    Rama and Sita in white marble, Rama with his bow, both almost hidden under marigolds. A little silver cradle in front of them today: it's his birthday. # look: rama-sita
    -> hall
+ {hall < 5} [Look: The side shrines]
    Ganesha, round and smiling, by the door. Krishna with his flute, and Radha. Durga on her tiger, in red silk. Hanuman, kneeling. Shiva, a smooth dark stone, wet with milk. Everyone has somewhere to go. # look: side-shrines
    -> hall
* {hall >= 2} [Wait (stand with Priya's family)]
-
~ set_experience("Derealization", 0.2)
The aarti: a woman in a cream sari, grey braid, reading glasses pushed up, circling a tray of flames before the shrine. Everyone singing, a bell, a drum, a conch. # look: aarti-flame
Mangala Tai: Come. Come, closer. You can take the light: hands over the flame, then over your eyes. Like this. # vo: ch5-hinduism_mangala_0008
* [Use: The aarti flame (take the light)]
    ~ record_choice("ch5.p1.aarti", "light", "At your first aarti, you held your hands over the flame and then over your eyes, the way Mangala Tai showed you.")
    Warm, over your palms. Warm, over your eyes. # look: take-the-light
    ~ nudge(-0.2)
* [Wait (just watch)]
    ~ record_choice("ch5.p1.aarti", "watch", "At your first aarti, you watched from the side. Mangala Tai smiled and said watching is also fine.")
    Mangala Tai: Watching is also fine. God is not offended by shy people. # vo: ch5-hinduism_mangala_0009
-
Heather (inside): A flame, held up for everyone. I like her. # vo: ch5-hinduism_heather-inside_0010
Mangala Tai: Prasad. Right hand, please. The sweet one is the best one. Don't tell the others. # vo: ch5-hinduism_mangala_0011
~ journal("Note", "Rama Navami at the temple. Shoes off. Ring the bell. Take the light. Right hand for prasad.")
-> ch5_part2  // the next part of the same chapter (one run)
