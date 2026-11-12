// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 1.5 (Axel), Part 1.
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. Axel. Champaign-Urbana, August 2018 (the start of junior year). The body is 20.
//  Owner's beats: a flashback, never explained, just played (always as Axel the first time). Axel's first act: a haircut
//  and new clothes. He knows a little about Maya from context clues. He tells Selene and his professors first (Selene just
//  thinks he transitioned), disappears on Tanya, and only then meets Mark. Axel: a dark sense of humour, loves fun,
//  never at anyone's expense.
//  Explore-first: the barber, the thrift store, Maya's room as Axel finds it.
//
//  My guesses, for the owner to correct:
//   - Axel comes forward at the end of summer 2018, just before junior year starts.
//   - The haircut is at a barber's on campus; the clothes come from a thrift store.
//   - The context clues: Maya's planner (another handwriting), Mira's stone, the napkin with a phone number, the dresses.
//   - He tells Selene over coffee, and emails his professors to ask them to use "Axel" and "he".
// ─────────────────────────────────────────────────────────────────────────────

=== ch1_5_part1 ===
~ set_age(20)
~ set_date(2018, 8, 20)  // Claude's guess at the exact day (owner: change it freely)
# cw: time-loss
# cw: religious-content
-> mirror

= mirror
# place: college_apartment_maya
# shot: close
# objective: Figure out whose room this is
- (room)
+ {not planner} [Look: The planner] -> planner
+ {not stone} [Look: The stone on the desk] -> stone
+ {not napkin} [Look: The napkin pinned to the corkboard] -> napkin
+ {not closet} [Look: The closet] -> closet
* {planner and closet} [Use: Your keys (go out)] -> barber

= planner
"Thursday 7pm, T." "Water the seed." Neat little letters. Not yours. # look: maya-planner
Axel: Somebody here has her life together. Respect. # vo: ch1-5-axel_axel_0001
-> mirror.room

= stone
A grey stone with a hole worn right through it. Smooth from being held. # look: stone-axel
-> mirror.room

= napkin
~ record_choice("ch1_5.p1.napkin", "looked", "As Axel, you found a napkin with a phone number and the name Mira. You left it pinned where it was.")
A paper napkin. A phone number in shaky blue pen. "Mira." # look: napkin-axel
-> mirror.room

= closet
Dresses. A blue one at the back, too small, kept anyway. # look: closet-dresses
Axel: Okay. Not today. # vo: ch1-5-axel_axel_0002
-> mirror.room

= barber
# place: campus_barber
# shot: over-shoulder
# objective: Get a haircut
Axel: Short. Shorter. Yeah. Like that. # vo: ch1-5-axel_axel_0003
* [Look: The mirror]
    ~ record_choice("ch1_5.p1.hair", "mirror", "As Axel, you watched your hair fall in the barber's mirror and finally recognised the face.")
    Hair on the floor. A face in the mirror. Yours. # look: barber-mirror
-
# place: thrift_store
# objective: Find something that fits
- (rack)
+ {rack < 3} [Look: The flannel shirts]
    Green and black. Red and black. All of them, honestly. # look: thrift-flannels
    -> rack
+ {rack < 3} [Look: The boots]
    Brown boots, scuffed at the toes. Somebody else's walking, already in them. # look: thrift-boots
    -> rack
* [Use: The fitting room]
-
Axel: There he is. # vo: ch1-5-axel_axel_0004
-> selene

= selene
# place: campus_cafe
# shot: wide
# objective: Tell Selene
~ text_from("tanya", "Thursday, 7. We've missed you.")
* [Look: Your phone]
    "Tanya." A shop with candles in the window. Someone who expects a girl named Maya. # look: tanya-unread
    ~ record_choice("ch1_5.p1.tanya", "ignored", "As Axel, you saw Tanya's messages, and didn't answer. You didn't answer any of them.")
-
Selene: Okay, you texted "big news, coffee, now", so I'm here, I have coffee, and I'm vibrating. # vo: ch1-5-axel_selene_0005
* [Talk: Selene (just say it)]
    Axel: My name's Axel. He, him. That's... the news. # vo: ch1-5-axel_axel_0006
* [Talk: Selene (make a joke first)]
    Axel: So I've decided to become a lumberjack. Also my name's Axel. He, him. Mostly the second part. # vo: ch1-5-axel_axel_0007
-
Selene: Oh my gosh. Okay. Axel. Hi, Axel. That suits you, actually. That really suits you. # vo: ch1-5-axel_selene_0008
Selene: I always kind of thought you were holding something in. I'm so glad. Can I hug you? I'm hugging you. # vo: ch1-5-axel_selene_0009
~ record_choice("ch1_5.p1.selene", "told", "As Axel, you told Selene your name first. She hugged you, and was glad.")
-> professors

= professors
# place: college_apartment_maya
# shot: close
# objective: Email your professors
* [Use: The laptop]
    "Hi Professor, I wanted to let you know I go by Axel now, and he/him. Thanks!" Four times. Send. # look: email-professors
-
* [Look: The replies]
    "Thanks for telling me, Axel. See you Tuesday." "Noted! Welcome back." "Thank you, Axel." # look: professor-replies
-
Axel: Huh. That was easy. Nothing's ever easy. # vo: ch1-5-axel_axel_0010
-> class

= class
# place: college_lecture_hall
# shot: wide
# objective: Find a seat
// My guess: they meet in a sociology seminar, week two of junior year.
Mark: Is the "Sociology of Religion" in here, or did I walk into a cult? Honestly either is fine. # vo: ch1-5-axel_mark_0011
* [Talk: Mark (dark joke)]
    ~ record_choice("ch1_5.p1.mark", "joke", "When Mark asked if he'd walked into a cult, you told him it depended on the snacks.")
    Axel: Depends. Cults usually have better snacks. This is the regular kind of religion. Bad coffee, worse chairs. # vo: ch1-5-axel_axel_0012
    Mark: Oh, I like you. I'm Mark. # vo: ch1-5-axel_mark_0013
* [Talk: Mark (just say yes)]
    ~ record_choice("ch1_5.p1.mark", "plain", "When Mark asked if he was in the right room, you just said yes and moved your bag off the seat.")
    Axel: Right room. Sit. # vo: ch1-5-axel_axel_0014
    Mark: Thank you. I'm Mark. # vo: ch1-5-axel_mark_0015
-
Axel: Axel. # vo: ch1-5-axel_axel_0016
Mark: Axel. Cool name. Okay, Axel, I'm going to need you to explain the reading to me, because I read it twice and I think it was in Latin. # vo: ch1-5-axel_mark_0017
~ text_from("ex_christian", "hey it's mark from soc of religion. want to do the reading together? i'll bring better coffee")
-> party

= party
// Owner: more of each part of the day (Axel loves fun and parties). Claude's scene: the first party as himself.
# place: campus_house_party
# shot: wide
# objective: Have fun
- (crowd)
+ {crowd < 3} [Look: The kitchen]
    Forty people in a kitchen built for four. Someone's playlist, too loud, perfect. # look: party-kitchen
    -> crowd
+ {crowd < 3} [Look: Selene]
    Selene on the counter, holding court, waving you over with a cup. # look: selene-counter
    -> crowd
+ {crowd < 3} [Look: Your phone]
    Mark: "better coffee. i'm serious. i have a french press. i'm that guy." You grin at it like an idiot. # look: mark-french-press # vo: ch1-5-axel_mark_0047
    -> crowd
* [Talk: The guy by the fridge]
-
Axel: Axel. Yeah, like the car part. No, I don't know which one either. # vo: ch1-5-axel_axel_0048
Selene: He's new. Well, he's not new. He's new-new. Be nice to him or I'll hex you. # vo: ch1-5-axel_selene_0049
~ nudge(-0.15)
Nobody blinks. Someone hands you a drink and calls you "dude", and it's the best word you've ever heard. # look: dude
-> ch1_5_part2  // the next part of the same chapter (one run)
