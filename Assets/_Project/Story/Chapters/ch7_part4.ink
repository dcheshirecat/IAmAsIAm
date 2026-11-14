// IN THE GAME as a DRAFT (Claude, 2026-11-13. The owner's Doc edits come first). Chapter 7 (Daoism), Part 4.
// vo-speakers: Maya=maya, Lin Daozhang=lin, Auntie Mei=mei
// ─────────────────────────────────────────────────────────────────────────────
//  Part 4. Lanterns. Seattle, August 2029. Maya is 30.
//  Zhongyuan, the Ghost Festival (research): the 15th day of the 7th lunar month, in the Daoist Three Yuan. Offerings and rites
//  for wandering, uncared-for spirits (pudu: tables of food set out for them), often spread across the whole month.
//  My guesses, for the owner to correct:
//   - Lin Daozhang leads the pudu rite at the temple. Tables of offerings on the pavement out front.
//   - That night, paper water lanterns set on the lake for the wandering dead (a Taiwanese custom. The place is fictional,
//     a small shore on Lake Washington). Maya sets one for Mira, and one for the little girl who waited in the fellowship hall.
//   - The chapter ends as Maya. It unlocks nobody (owner).
// ─────────────────────────────────────────────────────────────────────────────

=== ch7_part4 ===
~ set_age(30)
~ set_date(2029, 8, 24)  // Claude's guess at the Ghost Festival's day in 2029 (it follows the lunar calendar)
# cw: religious-content
# cw: childhood-reference
-> wake

= wake
# place: seattle_apartment
# shot: close
# objective: Get ready for work
~ text_from("auntie_mei", "Tonight 6pm, the big rite. Then lanterns at the lake, 9pm. You come. You help with the tables.")
- (room)
+ {room < 2} [Look: The altar]
    You put an orange on the altar, next to Mira's stone. It looks right there. # look: orange-altar
    -> room
* [Use: Your keys]
-
-> work

= work
# place: burke_museum_hall
# shot: wide
# objective: Get through the workday
- (hall)
+ {hall < 2} [Look: The clock]
    3:15. You keep checking it, the way you used to check it for Friday sunset. It doesn't hurt the same way. # look: clock-not-the-same
    -> hall
* [Use: Your lunch]
    Leftovers, at your desk. Priya stops by: "Ghost Month? My mom says don't swim, don't whistle at night." # look: priya-ghost-month
-
-> rite

= rite
# place: cid_temple_yard
# shot: wide
# objective: Help set out the tables
- (tables)
+ {tables < 4} [Look: The tables]
    Folding tables down the pavement, covered in red cloth: rice, fruit, biscuits, bottled water, a whole roast pig, instant noodles. # look: pudu-tables
    -> tables
+ {tables < 4} [Look: Lin Daozhang]
    In embroidered robes and a black cap you've never seen him wear, holding a sword of coins and a bell. Not the tax man at all. # look: lin-robes
    -> tables
+ {tables < 4} [Look: The people passing]
    Tourists photographing, a bus driver bowing at a red light, an old man adding a can of beer to the table for someone. # look: pudu-passing
    -> tables
* {tables >= 2} [Use: A box of oranges (help)]
    You set oranges in neat rows. Auntie Mei moves every one of them. She's smiling. # look: oranges-moved
-
Lin Daozhang: Tonight we feed the ones nobody feeds. Who died far from home, who nobody remembers, who waited and nobody came. # vo: ch7-daoism_lin_0025
~ set_experience("Derealization", 0.3)
Heather (inside): Who waited and nobody came. # vo: ch7-daoism_heather-inside_0026
* [Use: Mira's stone]
    Your thumb through the hole. The bell, the chanting, the smell of the roast pig. You're here. You stay. # look: stone-pudu
    ~ nudge(-0.2)
* [Use: Your feet (soft knees)]
    Weight into one foot. Then the other. Water around the rock. # look: feet-pudu
    ~ nudge(-0.2)
-
-> lanterns

= lanterns
# place: lake_lantern_shore
# shot: wide
# objective: Set your lanterns on the water
- (shore)
+ {shore < 3} [Look: The lake]
    Black water, a hundred small lights already going out across it, slow as breath. # look: lake-lanterns
    -> shore
+ {shore < 3} [Look: Auntie Mei]
    Lighting each lantern's candle with her own lighter, shielding it with her hand like it's a baby bird. # look: mei-lighter
    -> shore
* [Use: The first lantern (for Mira)]
    ~ record_choice("ch7.p4.lanterns", "mira", "At the Ghost Festival you set a lantern on the lake for Mira.")
    For Mira. It turns once, then goes. # look: lantern-mira
-
* [Use: The second lantern (for the little one)]
    ~ record_choice("ch7.p4.lanterns", "little", "At the Ghost Festival you set a second lantern for the little girl who waited in the fellowship hall.")
    For the little girl in the fellowship hall, who waited, and was very good. It goes out after the first one, a little behind, the way she always was. # look: lantern-little
    ~ nudge(-0.3)
-
Maya: She waited, and nobody came. I'm here now. That counts, doesn't it? # vo: ch7-daoism_maya_0027
Lin Daozhang: That's the whole festival. Somebody comes. # vo: ch7-daoism_lin_0028
Heather (inside): Somebody came. # vo: ch7-daoism_heather-inside_0029
~ system_log("maya", "Ghost Festival. Two lanterns: Mira, and the little one in the fellowship hall. Somebody came. -M")
-> night

= night
# place: seattle_apartment
# shot: close
# objective: Go home
* [Use: The bed]
    The orange on the altar, glowing a little in the streetlight. Two small lights going out across a lake, together. # look: two-lights-lake
-
~ text_to("tanya", "I put two lanterns on a lake tonight. One for Mira. One for me, when I was five.")
~ text_from("tanya", "Oh, love. Both of them are lucky to have you.")
# shot: inner
# place: black
Seattle. August 2029. # chapter-end
-> END
