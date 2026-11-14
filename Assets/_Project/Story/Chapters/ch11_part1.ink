// IN THE GAME as a DRAFT (Claude, 2026-11-14; the owner's Doc edits come first). Chapter 11 (Jainism), Part 1.
// vo-speakers: Serena=serena, Maya=maya, Nirmala-ben=nirmala, Heather (inside)=heather-inside, Serena (inside)=serena-inside
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. Ahimsa. Arcata and the Bay Area, April 2034. Maya is 35.
//  Owner: weekend trips from Arcata to a (made-up) Jain centre in the Bay Area; the guide is a Jain TEACHER; the chapter is about
//  FORGIVENESS (Paryushana, Micchami Dukkadam) AND NOT HARMING HERSELF (ahimsa turned inward). It STARTS AS SERENA; it unlocks
//  nobody (everyone is unlocked). Sarah's letter gets opened in Part 2, and Maya asks forgiveness of people she hurt in lost time.
//
//  Research (docs/Chapter11_JainismResearch.md): ahimsa (non-harm to every living being, jiva), aparigraha (non-attachment),
//  anekantavada (many-sidedness). A Jain temple (derasar) holds images of the Tirthankaras; visitors take off shoes and leave
//  leather outside. Many Jains eat before sunset and avoid root vegetables (pulling them up kills the whole plant and the small
//  lives in the soil). Svetambara, Digambara, Sthanakvasi and Terapanth Jains often share one centre in North America.
//
//  My guesses (placeholders), for the owner to correct:
//   - The teacher: NIRMALA SHAH, "Nirmala-ben" (`nirmala_shah`, 58, she/her; "ben" means sister, a respectful Gujarati form):
//     teaches philosophy at a community college and leads the centre's Sunday "Jainism for the curious" class for adults.
//   - The centre (made up): the Jain Center of the Peninsula, in a converted church in a quiet suburb south of San Francisco.
//   - Serena drives (five hours each way) because Serena handles the grown-up things.
// ─────────────────────────────────────────────────────────────────────────────

=== ch11_part1 ===
~ set_age(35)
~ set_date(2034, 4, 22)  // a Saturday, Earth Day (Claude's guess)
# cw: religious-content
~ explore_tradition("jainism")
~ text_from("anabelle", "Have fun at the Jain centre!! Bring me back a recipe. Something with no onions. I'm SO curious how that works")
-> wake

= wake
// Owner: the chapter starts as Serena.
# place: arcata_apartment
# shot: close
# objective: Get on the road by six
- (room)
+ {room < 3} [Look: The altar]
    Mira's stone. The dandelion, dried now, pressed flat. Sarah's letter still sealed underneath. Not today. # look: letter-not-today
    -> room
+ {room < 3} [Look: The fridge]
    A note in Heather's square letters: "SHE FORGETS TO EAT ON LONG DRIVES. -H" Serena packs two sandwiches. # look: heather-fridge-drive
    -> room
* [Use: The car keys]
-
Serena: Five hours. Coffee, the coast road, the radio low. Nobody needs to talk. # vo: ch11-jainism_serena_0001
-> drive

= drive
# place: car_highway_101
# shot: wide
# objective: Drive south
- (road)
+ {road < 3} [Look: The redwoods]
    The Avenue of the Giants, trees so tall the road goes dark at nine in the morning. # look: avenue-giants
    -> road
+ {road < 3} [Look: The passenger seat]
    The sandwiches. A notebook with "JAINISM?" written on the cover in Maya's round hand. # look: notebook-jainism
    -> road
* [Wait (just drive)]
-
-> centre

= centre
# place: jain_centre
# shot: wide
# objective: Find the class
- (hall)
+ {hall < 4} [Look: The shoe racks]
    Hundreds of shoes, neatly paired, outside the door. A sign: "Please leave shoes and leather items here." # look: jain-shoe-racks
    -> hall
+ {hall < 4} [Look: The temple room]
    Through a glass door, white marble, and seated figures with calm half-closed eyes. A woman sweeps the floor in front of her with a soft little broom, so she won't step on anything alive. # look: derasar-glass
    -> hall
+ {hall < 4} [Look: The noticeboard]
    Pathshala (Sunday school) timetable. A Navkar Mantra chanting group. "Svetambara and Digambara schedules: see website." A lost child's sandal pinned up like a notice. # look: jain-noticeboard
    -> hall
* {hall >= 1} [Talk: Nirmala-ben]
-
Nirmala-ben: You must be the Arcata one. Five hours! Come, sit. Shoes off, yes, good. Everyone calls me Nirmala-ben. It means sister Nirmala. Don't be shy with it. # vo: ch11-jainism_nirmala_0018
Serena: Serena. Today, anyway. Maya will want the notes. # vo: ch11-jainism_serena_0002
Nirmala-ben: Then I'll speak slowly for both of you. # vo: ch11-jainism_nirmala_0019
~ record_choice("ch11.p1.intro", "serena", "On the first day at the Jain centre, Serena introduced herself, and Nirmala-ben said she'd speak slowly for both of you.")
-> class

= class
# place: jain_centre
# shot: over-shoulder
# objective: Listen to the class
- (lesson)
+ {lesson < 4} [Look: The whiteboard]
    Three words in Nirmala-ben's careful hand: AHIMSA (do no harm). APARIGRAHA (hold on lightly). ANEKANTAVADA (no one sees the whole elephant). # look: three-words-board
    -> lesson
+ {lesson < 4} [Look: The others]
    A retired engineer taking notes in pencil. Two university students. A young mother with a baby asleep in a sling. A man who asks too many questions and is answered kindly every time. # look: class-others
    -> lesson
* {lesson >= 1} [Wait (listen)]
-
Nirmala-ben: Ahimsa. Do no harm. Not to people, not to animals, not to the ant, not to the water. We filter the water, we eat before dark so we don't swallow the little flying things. # vo: ch11-jainism_nirmala_0020
Nirmala-ben: People hear this and think: how strict! But the first jiva, the first living soul you are responsible for, is the one you are living in. If you are cruel to yourself, you have broken ahimsa before breakfast. # vo: ch11-jainism_nirmala_0021
* [Talk: Nirmala-ben (ask about that)]
    ~ record_choice("ch11.p1.ahimsa", "asked", "In the first class, you asked Nirmala-ben what it means to be cruel to yourself. She said: skipping meals, no sleep, and the voice that calls you stupid.")
    Serena: What does cruel to yourself look like? To you. # vo: ch11-jainism_serena_0003
    Nirmala-ben: Not eating. Not sleeping. Working until you fall over and calling it virtue. And that little voice in the head that says "stupid, stupid". That voice is violence too. Small, but it adds up. # vo: ch11-jainism_nirmala_0022
* [Wait (write it down)]
    ~ record_choice("ch11.p1.ahimsa", "wrote", "In the first class, you wrote down: the first living soul you're responsible for is the one you're living in.")
    Serena writes it down for Maya. "The first jiva you are responsible for is the one you live in." # look: notebook-first-jiva
-
Heather (inside): I've been saying that for years. Nobody listens to me. # vo: ch11-jainism_heather-inside_0004
-> lunch

= lunch
# place: jain_centre
# shot: close
# objective: Eat lunch before you leave
- (plate)
+ {plate < 3} [Look: The food]
    Dal, rice, a dry cabbage curry, rotis, a sweet made of chickpea flour. No onions, no garlic, no potatoes. It's the best food you've had in months. # look: jain-lunch
    -> plate
+ {plate < 3} [Look: Your sandwiches]
    Still in the bag, from this morning. Heather's note was right. # look: sandwiches-uneaten
    -> plate
* [Use: The plate (eat all of it)]
    ~ record_choice("ch11.p1.lunch", "ate", "At the Jain centre's lunch, you ate everything on the plate, slowly, and went back for the chickpea sweet.")
    ~ nudge(-0.3)
    Slowly. All of it. Nirmala-ben puts a second sweet on the plate without asking. # look: second-sweet
-
Nirmala-ben: Paryushana is in September. Eight days. We fast, we forgive. At the end we say to everyone: "Micchami Dukkadam." If I hurt you, in thought or word or deed, knowingly or not, please forgive me. # vo: ch11-jainism_nirmala_0023
Nirmala-ben: Come for the last day. You don't have to fast. Especially you. Eat. # vo: ch11-jainism_nirmala_0024
-> home

= home
// Serena hands back for the drive home.
# switch: maya
~ co_conscious("serena", true)
# place: arcata_apartment
# shot: close
# objective: Get to bed
~ set_experience("TimeLoss", 0.3)
- (room)
+ {room < 2} [Look: The notebook]
    Two pages of Serena's neat, slanting writing. "Ahimsa starts with the one you live in." Underlined twice, in Heather's pen. # look: notebook-underlined
    -> room
* [Use: The bed]
-
Maya: I don't remember the drive home. # vo: ch11-jainism_maya_0005
Serena (inside): I drove. You slept from Garberville. That was allowed. # vo: ch11-jainism_serena-inside_0006
~ system_log("serena", "Jain centre: good. Teacher: Nirmala-ben. Lesson one: don't be cruel to the body we live in. We ate everything. Paryushana in September. -S")
~ text_to("anabelle", "The Jain food has no onions, no garlic, no potatoes, and it was amazing. I'll ask for the recipe next time.")
~ text_from("anabelle", "No POTATOES?? Okay I need to know everything")
~ journal("Note", "The Jain centre. Nirmala-ben: the first living soul you're responsible for is the one you live in. The cruel little voice counts too. Ate everything.")
-> ch11_part2  // the next part of the same chapter (one run)
