// IN THE GAME as a DRAFT (Claude, 2026-11-13; the owner's Doc edits come first). Chapter 7 (Daoism), Part 3.
// vo-speakers: Maya=maya, Lin Daozhang=lin, Auntie Mei=mei
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Water. Seattle, June 2029. Maya is 30.
//  Owner: dawn tai chi in a park. My guesses: Hing Hay Park (the real square in the International District), where older
//  people practise every morning; Lin Daozhang among them. Tai chi as not forcing: the answer to Chapter 3's silent course.
//  Lin Daozhang's story (placeholder): a family of Zhengyi priests in Tainan for six generations; his father taught him the rites
//  at twelve; he came to Seattle in 1994 "for my wife's job and the rain"; he does taxes on weekdays.
//  Tao Te Ching chapter 8 (Legge): "The highest excellence is like (that of) water" (the chapter's intro quote).
// ─────────────────────────────────────────────────────────────────────────────

=== ch7_part3 ===
~ set_age(30)
~ set_date(2029, 6, 14)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
-> dawn

= dawn
# place: seattle_apartment
# shot: close
# objective: Get up before the sun
- (room)
+ {room < 2} [Look: The clock]
    4:58 a.m. Light already, pale, June in Seattle. You set the alarm yourself. Nobody else did. # look: alarm-458
    -> room
* [Use: Your shoes]
-
-> park

= park
# place: hing_hay_park
# shot: wide
# objective: Join the morning practice
- (square)
+ {square < 3} [Look: The pavilion]
    The red pavilion in the corner, and under it, twenty people in loose clothes moving very slowly, all together. # look: hing-hay-pavilion
    -> square
+ {square < 3} [Look: Lin Daozhang]
    In the back row, in an old windbreaker, moving like water finding its way down a hill. # look: lin-taichi
    -> square
* {square >= 1} [Use: The back row (join in)]
-
Lin Daozhang: Don't copy the shapes. Copy the slowness. Feet first. Knees soft. Let the arms come along later. # vo: ch7-daoism_lin_0016
* [Use: Your feet (soft knees)]
    ~ record_choice("ch7.p3.taichi", "joined", "At dawn in Hing Hay Park you did tai chi in the back row with Lin Daozhang, slowly, feet first.")
    Weight into one foot. Then, slowly, the other. Your arms float up without being sent. You are in your body the whole time. # look: taichi-feet
    ~ nudge(-0.35)
* [Wait (watch, then try one move)]
    ~ record_choice("ch7.p3.taichi", "watched", "At dawn in Hing Hay Park you watched the tai chi, then tried one move. Lin Daozhang said one is enough.")
    One move. Push the water away. Bring the water back. Lin Daozhang nods: one is enough today. # look: taichi-one-move
    ~ nudge(-0.2)
-
Maya: In Nepal I sat still for ten days. I lost myself completely. This is the first time being still didn't scare me. # vo: ch7-daoism_maya_0017
Lin Daozhang: Sitting still with force is still force. Water doesn't try. It goes around the rock, and in a thousand years the rock is gone. # vo: ch7-daoism_lin_0018
-> work

= work
# place: burke_museum_hall
# shot: wide
# objective: Give the morning tour
- (hall)
+ {hall < 2} [Look: The whale]
    A whale is mostly water too, you tell the third-graders. They are delighted and disgusted. # look: whale-water
    -> hall
* [Use: Your lunch (a bun from Golden Crown)]
    A pork bun from the bakery under the temple, at your desk. Auntie Mei made you take two. # look: bun-desk
-
-> evening

= evening
# place: cid_temple_hall
# shot: close
# objective: Bring Auntie Mei's container back
- (hall)
+ {hall < 3} [Look: The quiet temple]
    Weekday evening: no crowd, one stick of incense burning, the Jade Emperor and the City God keeping each other company. # look: quiet-temple
    -> hall
+ {hall < 3} [Look: The ledger on the desk]
    Not the City God's: Lin Daozhang's. A calculator, a stack of tax forms, a red pen. # look: tax-forms
    -> hall
* [Talk: Lin Daozhang]
-
Lin Daozhang: Six generations of priests in Tainan, my family. My father taught me the rites when I was twelve. I hated it. I wanted to play baseball. # vo: ch7-daoism_lin_0019
Lin Daozhang: Then I came here, for my wife's job, and the rain. Nobody here needed a priest. Then everybody did. So: taxes on Monday, the gods on Sunday. # vo: ch7-daoism_lin_0020
* [Talk: Lin Daozhang (ask if he still wanted to be a priest)]
    ~ record_choice("ch7.p3.lin", "wanted", "Lin Daozhang told you how he became a priest, and you asked if he'd wanted to. He said: like water, you go where the hill goes.")
    Maya: Did you want to be one? In the end? # vo: ch7-daoism_maya_0021
    Lin Daozhang: Want, don't want. Water doesn't want the sea. It goes where the hill goes. It arrives. # vo: ch7-daoism_lin_0022
* [Talk: Lin Daozhang (ask about baseball)]
    ~ record_choice("ch7.p3.lin", "baseball", "Lin Daozhang told you how he became a priest. You asked about baseball. He still watches every Mariners game.")
    Maya: Do you still like baseball? # vo: ch7-daoism_maya_0023
    Lin Daozhang: Every Mariners game. They lose, I practise acceptance. Very spiritual team. # vo: ch7-daoism_lin_0024
-
-> night

= night
# place: seattle_apartment
# shot: close
# objective: Go to bed
* [Use: The bed]
    Your legs ache in a good way. Weight into one foot. Then the other. Asleep before the other. # look: taichi-sleep
-
~ journal("Note", "Tai chi at dawn, Hing Hay Park. Copy the slowness, not the shapes. Water goes around the rock.")
-> ch7_part4  // the next part of the same chapter (one run)
