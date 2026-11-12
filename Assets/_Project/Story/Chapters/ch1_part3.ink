// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 1 (Christianity), Part 3.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Senior year. Champaign-Urbana, fall 2019. Maya is 21.
//  Owner's beats: the breakup with Mark is explained during senior year (not played). Everyone abandons Maya except Tanya
//  and Selene, who understand something was wrong and welcome her back into the coven. Maya meets Gracie (a young Catholic
//  nun in training who volunteers at the campus ministry) at a donation drive, and comes to like praying quietly on Sundays.
//  Mark: Episcopal, a kind queer man, left very confused and upset.
//  Explore-first: the quad, the coven, the ministry.
//
//  My guesses, for the owner to correct:
//   - How it's explained: Mark's long email, which Maya finds in senior year and reads. Maya ended it in May, in a way
//     Mark didn't understand. He isn't cruel; he's hurt.
//   - The abandonment: Axel's friends unfollow her, leave the group chat, look away on the quad (Jordan, Sam and Riley
//     are placeholder names).
//   - Tanya and Selene welcome her back at Samhain 2019 at The Circle, with no questions she has to answer.
//   - The donation drive is a coat drive in October; Gracie runs it with a clipboard and a lot of enthusiasm.
// ─────────────────────────────────────────────────────────────────────────────

=== ch1_part3 ===
~ set_age(21)
~ set_date(2019, 9, 30)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
# cw: isolation
# cw: breakup
# cw: detransition
~ text_from("ex_christian", "I'm not going to keep texting. I just need you to know I'm not angry. I'm just lost. I sent you an email.")
~ email_from("ex_christian", "I don't understand", "I keep going over May. One day you were Axel, and you were so happy, finally, you said that, finally. And then you looked at me like you'd never seen me before, and you said your name was Maya, and you asked me to leave. I left. I don't know what I did. If it's something I did, I'm sorry. If it's not, I'm still sorry. I won't pretend I'm okay. I hope you are. Mark.")
-> quad

= quad
# place: college_quad
# shot: over-shoulder
# objective: Get to class
~ set_experience("Depersonalization", 0.3)
- (walk)
+ {walk < 3} [Look: The group by the fountain]
    People you don't know, who know you. One of them sees you, and turns away. # look: quad-turn-away
    ~ nudge(0.15)
    -> walk
+ {walk < 3} [Look: Your phone]
    "Jordan left the group." "Sam left the group." "Riley left the group." # look: left-the-group
    -> walk
+ {walk < 3} [Look: The flyer on the noticeboard]
    "Campus Ministry Coat Drive! Bring a coat, take a coat, get a hot chocolate!" Three exclamation marks. # look: coat-drive-flyer
    -> walk
* [Use: Your phone (read Mark's email)] -> email
* [Go: To class]
-
-> drive

= email
~ record_choice("ch1.p3.mark", "read", "In senior year, you read Mark's email: how it had ended, and that he wasn't angry, only lost.")
Your phone. # device: phone
-> quad.walk

= drive
# place: campus_ministry
# shot: wide
# objective: Drop off a coat
Gracie: Hi! Hi. Is that for us? Oh, that's a good one. That's a three-winters coat. Thank you! # vo: ch1-christianity_gracie_0014
Gracie: I'm Gracie. Sister Gracie, technically, almost. In training. Do you want a hot chocolate? There's marshmallows. There's too many marshmallows, honestly. # vo: ch1-christianity_gracie_0015
* [Talk: Gracie (say yes)]
    ~ record_choice("ch1.p3.gracie", "stayed", "At the coat drive, you stayed for a hot chocolate with Gracie.")
    Maya: Okay. Yes. Thank you. # vo: ch1-christianity_maya_0016
    Gracie: Sit, sit. You look like you need sitting. Not in a bad way! # vo: ch1-christianity_gracie_0017
* [Talk: Gracie (say you have to go)]
    ~ record_choice("ch1.p3.gracie", "left", "At the coat drive, you told Gracie you had to go. She gave you a hot chocolate for the road anyway.")
    Maya: I have to go, sorry. # vo: ch1-christianity_maya_0018
    Gracie: Take one for the road! Hold on. Two marshmallows. Okay, three. # vo: ch1-christianity_gracie_0019
-
// Owner: herbs and stones from different people (Claude's list, owner: "yes"). No new voiced lines.
Gracie drops a paper twist into your coat pocket: dried calendula, from the ministry garden. Mary's gold, she calls it. # look: gracie-calendula
~ gain_herb("calendula", "gracie")
- (ministry)
+ {ministry < 3} [Look: The chapel door]
    A small chapel, through the door. Empty. Quiet. One candle burning. # look: chapel-door
    -> ministry
+ {ministry < 3} [Look: The sign-up sheet]
    "Quiet prayer, Sundays, 4 pm. All welcome, no need to say anything." # look: quiet-prayer-sheet
    -> ministry
* [Go: Back outside]
-
~ journal("Note", "Campus ministry. Quiet prayer, Sundays at 4. Gracie.")
-> samhain

= samhain
~ set_date(2019, 10, 31)
# place: shop_the_circle
# shot: wide
# objective: Go to Samhain at The Circle
Selene: Maya. You came. Come here. # vo: ch1-christianity_selene_0020
Tanya: Your seat's still saved. It's been saved all year. # vo: ch1-christianity_tanya_0021
* [Talk: Tanya (try to explain)]
    ~ record_choice("ch1.p3.back", "explain", "At Samhain, you tried to explain the lost year to Tanya and Selene. You couldn't, and they didn't need you to.")
    Maya: I don't know what happened. I don't remember last year. Any of it. I know that sounds... # vo: ch1-christianity_maya_0022
    Tanya: It sounds like something went very wrong, and you're here now. That's all I need tonight. # vo: ch1-christianity_tanya_0023
* [Talk: Selene (say you're sorry)]
    ~ record_choice("ch1.p3.back", "sorry", "At Samhain, you told Selene you were sorry. She said there was nothing to be sorry for.")
    Maya: I'm sorry. For whatever I did. # vo: ch1-christianity_maya_0024
    Selene: Hey. No. Whatever happened, that wasn't you being a bad friend. Something was wrong. We could tell. # vo: ch1-christianity_selene_0025
* [Use: Your seat]
    ~ record_choice("ch1.p3.back", "sat", "At Samhain, you just sat down in your old seat. Selene's scarf was on it.")
    Selene's scarf, on your old chair. She moves it. # look: scarf-moved
-
Tanya: Names, if you want to. Anyone you miss. # vo: ch1-christianity_tanya_0026
* [Talk: Say Mira's name]
    Maya: Mira. # vo: ch1-christianity_maya_0027
* [Wait (say nothing)]
    The candles. Everyone's faces, gold. # look: samhain-faces
-
// The charm bag and runes (owner: the craft is an active part of the game). Claude's addition: no new voiced lines.
Selene presses a little cloth bag into your hand. Lavender, a smooth grey stone, three tight knots. She made it in August, just in case. # look: selene-charm-bag
~ gift_charm_bag("selene", "lavender", "smoky-quartz", "Come back")
~ learn_practice("charm-bag", "selene")
~ text_from("tanya", "Thursdays again, if you want them. This month: runes. I have a spare bag. Bring nothing but yourself.")
~ learn_practice("runes", "tanya")
~ text_from("tanya", "And after runes: sigils. Bring a wish, written like it's already true.")
~ learn_practice("sigil", "tanya")
-> sundays

= sundays
# place: campus_ministry_chapel
# shot: close
# objective:
* [Use: A seat at the back]
    The back row. Four people, far apart. Nobody turns around. # look: chapel-back-row
-
~ set_experience("Depersonalization", 0.0)
~ nudge(-0.2)
* [Wait (pray, quietly)]
    ~ record_choice("ch1.p3.sundays", "prayed", "On Sundays that year, you sat in the back of the chapel and prayed quietly.")
    Quiet. Not talking to anyone in particular. Not having to. # look: quiet-prayer
// Owner: Nina is a devout Quaker. Only while Nina fronts (never hinted otherwise). No new voiced lines.
* {fronts("nina")} [Wait (wait in the silence, like meeting)]
    ~ record_choice("ch1.p3.sundays", "meeting", "On a Sunday in the chapel, Nina waited in the silence the way she was taught in meeting, and felt held.")
    Hands in your lap. Eyes down. The silence, like meeting at home. Here it's allowed to just be quiet. # look: nina-chapel-silence
    ~ get_to_know("nina", 1)
* [Wait (just sit)]
    ~ record_choice("ch1.p3.sundays", "sat", "On Sundays that year, you sat in the back of the chapel. You didn't pray. You just sat.")
    Quiet. The candle. Nobody wanting anything. # look: quiet-sitting
-
Gracie: Same time next week? You don't have to. But same time next week. # vo: ch1-christianity_gracie_0028
-> ch1_part4  // the next part of the same chapter (one run)
