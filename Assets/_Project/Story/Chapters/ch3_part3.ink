// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 3 (Buddhism), Part 3.
// vo-speakers: Maya=maya, Pasang Lhamo=pasang
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Noble silence. Dharma Shringa, Budhanilkantha, and Boudha, May 2026. Maya is 27.
//  Owner's beats: a 10-day silent meditation course (no talking until day 10). It goes wrong: Maya dissociates badly, and her
//  host doesn't notice until it's too late. Then Heather comes out, at the Buddhist temple (the monastery behind the guest
//  house). Heather isn't unlocked yet (only at the end of the chapter); this is a scripted switch.
//
//  My guesses, for the owner to correct:
//   - Maya never tells anyone how bad it is. Students may speak to the teacher when necessary; Maya says she's fine.
//     The course isn't the villain: the silence, a fresh diagnosis and not saying anything are.
//   - Izzy goes quiet inside too. The days blur.
//   - Day 11: Pasang picks her up, chatting happily, and notices nothing. That night Maya is gone (lost time), and Pasang
//     finds her in the monastery's lamp room, lighting butter lamps: it's Heather (the candle in Chapter 2 was hers).
// ─────────────────────────────────────────────────────────────────────────────

=== ch3_part3 ===
~ set_age(27)
# cw: religious-content
# cw: time-loss
# cw: depersonalization
# cw: involuntary-switch
-> gate

= gate
# place: dharma_shringa_hall
# shot: wide
# objective: Settle in for ten days
- (rules)
+ {rules < 4} [Look: The rules board]
    "Noble silence: no talking, no gestures, no eye contact, from the first evening to the morning of day ten. You may speak to the teacher when necessary." # look: noble-silence-rules
    -> rules
+ {rules < 4} [Look: The timetable]
    "4:00 am: wake-up bell. 4:30 to 6:30: meditation." It goes on. It goes on for a long time. # look: course-timetable
    -> rules
+ {rules < 4} [Look: The hills]
    Forest all around, and Kathmandu down in its bowl of haze. Monkeys on the wall, watching. # look: shivapuri-hills
    -> rules
* {rules >= 1} [Use: Your phone (hand it in)]
    ~ record_choice("ch3.p3.phone", "handed", "At the start of the silent course, you handed in your phone.")
    ~ text_to("tanya", "Handing in my phone now. See you in 10 days. Love you.")
    A paper envelope with your name on it. Your phone goes in. The envelope goes in a box. # look: phone-envelope
-
Teacher: Sit comfortably. Close your eyes. Observe the breath, just as it is. # vo: ch3-buddhism_teacher_0055
~ co_conscious("izzy", false)
-> days

= days
# place: dharma_shringa_hall
# shot: close
# objective: Sit
~ set_experience("Depersonalization", 0.3)
* [Wait (day one: watch the breath)]
    In. Out. Your knees. Your back. In. Out. # look: day-one
-
~ set_experience("Depersonalization", 0.55)
~ set_experience("Derealization", 0.4)
* [Wait (day three: keep your eyes closed)]
    ~ record_choice("ch3.p3.eyes", "closed", "On the silent course, you kept your eyes closed, the way everyone else did.")
    Dark. And further away than dark. Your hands aren't yours. They're very far down there, in somebody's lap. # look: day-three
* [Use: Mira's stone (in your pocket)]
    ~ record_choice("ch3.p3.eyes", "stone", "On the silent course, you held Mira's stone in your pocket. It helped, a little. Not enough.")
    The white line. Around and around. Your thumb, someone's thumb. # look: day-three-stone
-
Izzy is quiet. Nobody inside is talking. The silence goes all the way down. # look: inside-quiet
~ set_experience("Depersonalization", 0.8)
~ set_experience("TimeLoss", 0.5)
* [Talk: Teacher (say something)]
    ~ record_choice("ch3.p3.teacher", "fine", "On day six, you went to the teacher. You meant to tell her. You said you were fine.")
    Maya: I'm... it's fine. I'm fine. Just tired. # vo: ch3-buddhism_maya_0056
    Teacher: Tiredness passes. Keep observing. Come to me again if you need. # vo: ch3-buddhism_teacher_0057
* [Wait (say nothing)]
    ~ record_choice("ch3.p3.teacher", "silent", "On day six, you didn't tell anyone how far away you'd gone. It was a silent course. You kept the silence.")
    Noble silence. You keep it. You keep it so well you almost aren't there. # look: kept-silence
-
~ set_experience("TimeLoss", 1.0)
# shot: inner
# place: black
Day seven.
Day eight.
Day nine.
-> pickup

= pickup
# place: dharma_shringa_hall
# shot: wide
# objective: Go back to Boudha
~ set_experience("Depersonalization", 0.6)
~ set_experience("TimeLoss", 0.0)
Pasang Lhamo: There she is! Ten days! You look very calm. Very, very calm. Come, the taxi is waiting, he's charging me by the minute, the thief. # vo: ch3-buddhism_pasang_0058
* [Talk: Pasang Lhamo (try to tell her)]
    ~ record_choice("ch3.p3.pickup", "tried", "When Pasang picked you up, you tried to tell her something was wrong. She was talking about the taxi.")
    Maya: Pasang, I don't... # vo: ch3-buddhism_maya_0059
    Pasang Lhamo: And my daughter's exams, and the dog ate a whole shoe, and the monks are painting the gate again, blue this time... # vo: ch3-buddhism_pasang_0060
* [Wait (just nod)]
    ~ record_choice("ch3.p3.pickup", "nodded", "When Pasang picked you up, you just nodded. She didn't notice anything was wrong.")
    You nod. You nod at everything. It's very easy to nod. # look: just-nod
-
~ text_from("tanya", "Day 11! You're out! How was it? Tell me everything.")
~ sent_while_away("heather", "text", "tanya", "She's very tired. She'll write tomorrow.")
~ set_experience("TimeLoss", 1.0)
# shot: inner
# place: black
That night.
-> lamps

= lamps
// Owner: Heather comes out at the Buddhist temple. A scripted switch: she isn't unlocked until the end of the chapter.
# switch: heather
# place: boudha_monastery
# shot: close
# objective: Light the lamps
- (lamp_room)
+ {lamp_room < 3} [Look: The lamp room]
    A narrow room behind the shrine hall, warm with a thousand small flames. Nobody here, this late. # look: lamp-room
    -> lamp_room
+ {lamp_room < 3} [Look: Her hands]
    Steady hands. Not shaking at all. Somebody's been looking after these hands. # look: heather-hands
    -> lamp_room
* {lamp_room >= 1} [Use: A butter lamp (light it)]
    One more flame. For Mira. For the little girl in the fellowship hall. For the one who sat for ten days and nearly didn't come back. # look: heather-lamp
-
Pasang Lhamo: Maya? Maya! I've been looking everywhere. It's two in the morning. What are you doing here? # vo: ch3-buddhism_pasang_0061
Heather: She's very tired. I'm looking after her tonight. # vo: ch3-buddhism_heather_0062
Pasang Lhamo: Who is looking after her? # vo: ch3-buddhism_pasang_0063
* [Talk: Pasang Lhamo (tell her)]
    ~ record_choice("ch3.p3.heather", "told", "The first time Heather came out, at the monastery, she told Pasang who she was.")
    Heather: Me. My name is Heather. Please don't be frightened. She needs water, and sleep, and someone to sit with her. # vo: ch3-buddhism_heather_0064
* [Talk: Pasang Lhamo (ask for water)]
    ~ record_choice("ch3.p3.heather", "water", "The first time Heather came out, at the monastery, she just asked Pasang for water and a blanket.")
    Heather: Water, please. And a blanket. Feet on the floor. That's all she needs right now. # vo: ch3-buddhism_heather_0065
-
Pasang Lhamo: I didn't see. Ten days, and I didn't see. Come. Come home. I'm sitting with you tonight. # vo: ch3-buddhism_pasang_0066
Feet on the floor. Five things you can see: a flame, a flame, a flame, Pasang's hands, the door. # exercise: five-senses
~ nudge(-0.3)
# switch: maya
-> ch3_part4  // the next part of the same chapter (one run)
