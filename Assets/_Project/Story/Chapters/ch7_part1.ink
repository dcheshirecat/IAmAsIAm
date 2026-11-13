// IN THE GAME as a DRAFT (Claude, 2026-11-13; the owner's Doc edits come first). Chapter 7 (Daoism), Part 1.
// vo-speakers: Maya=maya, Lin Daozhang=lin, Auntie Mei=mei
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. New Year. Seattle, February 2029. Maya is 30.
//  Owner: still Seattle, 2029; the guide is a Zhengyi Daoist priest (married, lives at home), an older man, at a community
//  temple in the Chinatown-International District (Daoist and folk gods side by side); parts built around Lunar New Year,
//  Qingming, dawn tai chi in a park and the Ghost Festival; starts as Maya, unlocks nobody. A full day in every part.
//
//  My guesses (placeholders), for the owner to correct:
//   - The priest: LIN DAOZHANG (`master_lin`; "Daozhang", Dao master, is the respectful title for a Zhengyi priest; 68, he/him). From a family of Zhengyi priests in Tainan, Taiwan; came
//     to Seattle in 1994; does people's taxes on weekdays and the temple's rites on weekends, the way many Zhengyi priests work
//     part-time. Dry, patient, funny without smiling.
//   - His wife AUNTIE MEI (`auntie_mei`, 65, she/her) runs everything else: the incense, the oranges, the volunteers.
//   - The temple: "Fu Shan Temple" (fictional), upstairs over a bakery: the Jade Emperor in the centre, with Guandi, Mazu and
//     Guanyin, and the City God by the door.
//   - Visitors (research): three sticks of incense, bow three times, place them in the burner; the moon blocks (two red crescent
//     blocks thrown to ask a yes-or-no question: one up, one down means yes).
// ─────────────────────────────────────────────────────────────────────────────

=== ch7_part1 ===
~ set_age(30)
~ set_date(2029, 2, 13)  // Lunar New Year's Day 2029 (Claude's guess at which day of the festival)
# cw: religious-content
~ explore_tradition("daoism")
~ text_from("tanya", "Happy Lunar New Year! The Year of the Rooster, I'm told. Crow at something today.")
-> wake

= wake
# place: seattle_apartment
# shot: close
# objective: Get ready for work
- (room)
+ {room < 3} [Look: The window]
    Grey and soft, rain that can't decide. The mountain in hiding again. # look: feb-rain
    -> room
+ {room < 3} [Look: The altar]
    Mira's stone, the moonstone, a dry marigold, the little clay lamp from Kent. Your altar has become a map. # look: altar-map
    -> room
* [Use: Your keys]
-
-> bus

= bus
# place: seattle_bus
# shot: over-shoulder
# objective: Ride to work
- (ride)
+ {ride < 2} [Look: The street]
    Red lanterns strung across the street in the International District, rocking in the rain. # look: red-lanterns
    -> ride
* [Use: The bell (your stop)]
    Your stop. You stay on one more, just to see the lanterns. # look: one-more-stop
-
-> burke

= burke
# place: burke_museum_hall
# shot: wide
# objective: Give the New Year family tour
- (hall)
+ {hall < 3} [Look: The craft table]
    Paper lanterns and paper roosters, glue everywhere. A grandmother teaching three children to cut a paper flower in one go. # look: paper-roosters
    -> hall
+ {hall < 3} [Look: The flyer]
    Pinned to the staff board: "Fu Shan Temple, New Year incense, all welcome, upstairs from Golden Crown Bakery." # look: fu-shan-flyer
    -> hall
* [Use: Your lunch]
    Leftover dal, eaten fast, the flyer folded in your pocket. # look: dal-fast
-
-> temple

= temple
# place: cid_temple_hall
# shot: wide
# objective: Go up to the temple
- (hall)
+ {hall < 5} [Look: The stairs]
    Narrow stairs over the bakery, smelling of pork buns and then, at the top, of incense. # look: bakery-stairs
    -> hall
+ {hall < 5} [Look: The Jade Emperor]
    A gold-faced figure in robes in the middle of the altar, calm as a mountain, oranges heaped in front of him. # look: jade-emperor
    -> hall
+ {hall < 5} [Look: The other shrines]
    A red-faced general with a long beard. A woman in a crown, for sailors. Guanyin in white. An old man by the door with a ledger: the City God, keeping the books. # look: side-gods
    -> hall
+ {hall < 5} [Look: The crowd]
    Grandparents, toddlers in red, a man in a suit on his lunch break, all with incense, all bowing. Nobody is in charge of anybody. # look: new-year-crowd
    -> hall
* {hall >= 2} [Talk: Auntie Mei]
-
Auntie Mei: First time? Here. Three sticks. Light them from the big candle, not your lighter. Hold them like this, bow three times, put them in the burner. Then you can ask. # vo: ch7-daoism_mei_0001
* [Use: The incense (three bows)]
    ~ record_choice("ch7.p1.incense", "bowed", "On Lunar New Year, Auntie Mei showed you how to offer incense: three sticks, three bows.")
    Three sticks, warm in your hands. Bow. Bow. Bow. The smoke goes up without being told. # look: three-bows
    ~ nudge(-0.15)
* [Wait (watch first)]
    ~ record_choice("ch7.p1.incense", "watched", "On Lunar New Year, you watched the others offer incense before you tried it.")
    Auntie Mei: Watch, then. Everybody watches the first time. Even the gods. # vo: ch7-daoism_mei_0002
-
-> blocks

= blocks
# place: cid_temple_hall
# shot: close
# objective: Ask a question
An older man in a grey cardigan steps down from the altar. Auntie Mei: "My husband. Lin Daozhang. Daozhang means the Dao master. He likes it, don't tell him." # look: lin-cardigan
Lin Daozhang: You want to ask something? These are the moon blocks. You ask a question in your heart, yes or no. Then you throw. One up, one down: yes. # vo: ch7-daoism_lin_0003
* [Use: The moon blocks (ask about Seattle)]
    ~ record_choice("ch7.p1.blocks", "home", "On Lunar New Year you asked the moon blocks if Seattle was home now. Yes.")
    Is this home now? Two red crescents on the floor. One up, one down. # look: blocks-yes
    Lin Daozhang: Yes. Hm. Good question. Most people ask about money. # vo: ch7-daoism_lin_0004
* [Use: The moon blocks (ask about all of you)]
    ~ record_choice("ch7.p1.blocks", "us", "On Lunar New Year you asked the moon blocks if all of you would be all right. They landed both face down. Lin Daozhang said that means ask again later.")
    Will we be all right? Both face down. # look: blocks-down
    Lin Daozhang: Both down. That means: not now. Ask again later. The gods are busy today, it's New Year. # vo: ch7-daoism_lin_0005
-
Maya: Thank you. I don't know what I'm doing. # vo: ch7-daoism_maya_0006
Lin Daozhang: Nobody knows what they're doing. The Dao doesn't mind. Come back on the ninth day, for the Jade Emperor's birthday. Bring nothing. Eat something. # vo: ch7-daoism_lin_0007
-> night

= night
# place: seattle_apartment
# shot: close
# objective: Go home
- (k)
+ {k < 2} [Look: Your coat]
    It smells of incense. You don't hang it up yet. # look: coat-incense
    -> k
* [Use: The bakery bag]
    Two pork buns from downstairs, Auntie Mei's orders. Dinner, at the window. # look: pork-buns
-
* [Use: The bed]
    You dream of smoke going up without being told. # look: dream-smoke
-
~ journal("Note", "Fu Shan Temple. Three sticks, three bows. Moon blocks: one up, one down is yes. Lin Daozhang: nobody knows what they're doing.")
-> ch7_part2  // the next part of the same chapter (one run)
