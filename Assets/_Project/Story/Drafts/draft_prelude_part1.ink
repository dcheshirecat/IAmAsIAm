// DRAFT: not canon until the owner approves it. Prelude (Wicca), Part 1.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. Selene. A college town in Illinois, October 2016. Maya is 18.
//  Owner's beats: Maya meets Wicca at college, first through a person: a classmate, Selene. She isn't
//  serious yet. College is out of state, away from Sarah's church. Lost time is growing, and Maya doesn't
//  understand it yet (signs only, never explained).
//  Explore-first: the dorm room and Selene's little altar carry it; very little narration.
//
//  My guesses, for the owner to correct:
//   - The town is Champaign-Urbana (the University of Illinois), and Maya's first class with Selene is
//     Intro to World Religions.
//   - Selene is 18 too, she/her, a solitary eclectic witch since high school; warm, a bit chaotic.
//   - Selene invites Maya to a small Samhain with her friends in a dorm room (an altar, photos of the dead,
//     an apple cut across to show the star). Maya goes, and only watches.
//   - Sarah phones during it, busy with the church's harvest supper; she doesn't ask about Maya.
//   - The first sign at college: Maya wakes up and her shoes are muddy, from a walk she doesn't remember.
// ─────────────────────────────────────────────────────────────────────────────

=== prelude_part1 ===
# cw: religious-content
# cw: neglect
# cw: depersonalization
# cw: time-loss
-> class

= class
# place: college_lecture_hall
# shot: wide
# objective: Find a seat
- (hall)
+ {hall < 3} [Look: The screen]
    "Intro to World Religions. Week 8: New Religious Movements." # look: lecture-screen
    -> hall
+ {hall < 3} [Look: Your notebook]
    Neat notes. A doodle of a candle in the margin, not yours. # look: notebook-candle
    ~ set_experience("Depersonalization", 0.15)
    -> hall
* [Use: The empty seat by the window]
-
Selene: Is this seat taken? It's not, I can see it's not. Hi. I'm Selene. # vo: prelude-wicca_selene_0001
Selene: You're the one who knew all the Quaker stuff last week. The "that of God in everyone" thing. # vo: prelude-wicca_selene_0002
* [Talk: Selene (say you grew up with it)]
    ~ record_choice("prelude.p1.quaker", "grew-up", "When Selene asked about the Quaker stuff, you said you grew up with it.")
    Maya: I grew up with it. My mom's a minister. # vo: prelude-wicca_maya_0003
    Selene: Oh, wow. Like, a pastor? # vo: prelude-wicca_selene_0004
    Maya: Like a pastor. She's... very busy. # vo: prelude-wicca_maya_0005
* [Talk: Selene (say you read the textbook)]
    ~ record_choice("prelude.p1.quaker", "textbook", "When Selene asked about the Quaker stuff, you said you'd just read the textbook.")
    Maya: I just read the chapter. Twice. # vo: prelude-wicca_maya_0006
    Selene: Nobody reads the chapter. You're a legend. # vo: prelude-wicca_selene_0007
-
Selene: This week's us, by the way. Witches. Page two-forty. Don't believe anything about the Devil, I'll be very offended. # vo: prelude-wicca_selene_0008
* [Look: Selene's bag]
    A pentacle charm on the zip. A paperback with a cracked spine, full of sticky notes. # look: selene-bag
-
Selene: We're doing a little Samhain thing on Monday. In my room. Candles, apple cider, the dead. You should come. # vo: prelude-wicca_selene_0009
~ journal("Note", "Selene's room, Monday. Samhain.")
-> dorm

= dorm
# place: college_dorm_maya
# shot: close
# objective: Get ready for Selene's
- (room)
+ {room < 4} [Look: Your desk]
    A planner, every hour filled in. A mug of cold tea. # look: planner
    -> room
+ {room < 4} [Look: Your shoes]
    Your running shoes, caked in mud. You haven't been running. # look: muddy-shoes
    ~ set_experience("TimeLoss", 0.2)
    ~ record_choice("prelude.p1.shoes", "noticed", "Before Samhain, you noticed your shoes were muddy, from a walk you didn't remember.")
    -> room
+ {room < 4} [Look: The wall above the bed]
    Nothing on it. Everyone else on the floor has posters. # look: bare-wall
    -> room
* [Use: Mira's stone]
    Mira's stone, in your coat pocket. Still. # look: stone-coat
-
-> samhain

= samhain
# place: college_dorm_selene
# shot: wide
# objective:
Selene: You came! Okay. Shoes anywhere. This is Priya, this is Dev. Sit on the bed, it's the only clean surface. # vo: prelude-wicca_selene_0010
- (altar)
+ {altar < 4} [Look: The altar]
    A desk under a scarf. Candles in jam jars. A bowl of water, a bowl of salt. # look: dorm-altar
    -> altar
+ {altar < 4} [Look: The photos]
    Photos of people who've died. A grandmother. A dog. A boy in a graduation gown. # look: ancestor-photos
    -> altar
+ {altar < 4} [Look: The apple]
    An apple, cut across. The seeds make a star. # look: apple-star
    -> altar
* [Talk: Selene (ask what you're supposed to do)]
    Maya: What do I do? I don't want to do it wrong. # vo: prelude-wicca_maya_0011
    Selene: There's no wrong. You just remember someone. Out loud, or in your head. That's it. That's the whole thing. # vo: prelude-wicca_selene_0012
-
Selene: Okay. Lights off. Everyone say a name, if you want to. # vo: prelude-wicca_selene_0013
# shot: close
* [Talk: Say Mira's name]
    ~ record_choice("prelude.p1.samhain", "mira", "At Selene's Samhain, you said Mira's name out loud.")
    Maya: Mira. Mira Smith. # vo: prelude-wicca_maya_0014
    Selene: Hi, Mira. # vo: prelude-wicca_selene_0015
* [Wait (say it in your head)]
    ~ record_choice("prelude.p1.samhain", "silent", "At Selene's Samhain, you said Mira's name in your head, where nobody could hear.")
    Mira. # look: mira-silent
* [Use: Mira's stone]
    ~ record_choice("prelude.p1.samhain", "stone", "At Selene's Samhain, you held Mira's stone instead of saying a name.")
    The stone, warm from your hand. # look: stone-warm
-
-> call

= call
# shot: over-shoulder
* [Look: Your phone]
    "Mom." The screen lights up the whole dark room. # look: phone-mom
-
* [Use: The phone (answer in the hall)]
    ~ record_choice("prelude.p1.call", "answered", "When Mom called during Samhain, you answered it in the hall.")
    Sarah: Hi sweetheart, I can't stay long, the harvest supper's in an hour and the coffee urn's broken. # vo: prelude-wicca_sarah_0016
    Sarah: I just wanted to tell you Mrs. Hines had her hip done. Send her a card, would you? She always asks about you. # vo: prelude-wicca_sarah_0017
    Maya: Okay. I'm at a friend's. We're... # vo: prelude-wicca_maya_0018
    Sarah: Oh, good, good. Love you! Bye! # vo: prelude-wicca_sarah_0019
    ~ set_experience("Depersonalization", 0.35)
    ~ nudge(0.15)
* [Use: The phone (let it ring)]
    ~ record_choice("prelude.p1.call", "ignored", "When Mom called during Samhain, you let it ring. She didn't call back.")
    It rings, and stops. No message. # look: phone-stops
-
Selene: Everything okay? # vo: prelude-wicca_selene_0020
* [Talk: Selene (say it's fine)]
    Maya: It's fine. It was my mom. It's always fine. # vo: prelude-wicca_maya_0021
* [Talk: Selene (ask about the book)]
    ~ record_choice("prelude.p1.book", "asked", "At Samhain, you asked Selene about her book. She lent it to you.")
    Maya: Can I borrow your book? The one with the sticky notes. # vo: prelude-wicca_maya_0022
    Selene: Yes! Oh my gosh. Don't read my notes, they're embarrassing. Read my notes. # vo: prelude-wicca_selene_0023
-
~ explore_tradition("wicca")
-> END
