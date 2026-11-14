// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 4 (Islam), Part 3.
// vo-speakers: Maya=maya, Chaplain Amal=amal
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Remembrance. Chicago, December 2026. The body is 28.
//  Owner's beats: Sufi practice. Only what Muslims consider Islam. Izzy and Heather. Maya sometimes.
//
//  My guesses, for the owner to correct:
//   - Sawsan (owner: Sawsan, Palestinian American. 30s, she/her), another patient at the day program, Sunni (as most Palestinian
//     Muslims are). She prays in the quiet room, then counts her tasbih (prayer beads), her grandmother's, from Nablus.
//   - Amal invites them to a Thursday-night dhikr (remembrance) gathering led by her friend Fadi (owner: Fadi, 60s, he/him,
//     Shia), a Sufi teacher, Lebanese American: guests are welcome to sit and listen. Sufism is found among both Sunni
//     and Shia Muslims (the Nimatullahi order, for one, is Shia). His circle has both. Heather finds the repetition steadying. Izzy keeps count, eyes open.
//     (Rhythm and repetition can pull a dissociative person away: they keep their eyes open, as Angie taught.)
// ─────────────────────────────────────────────────────────────────────────────

=== ch4_part3 ===
~ set_age(28)
~ set_date(2026, 12, 10)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
# cw: medical-setting
~ co_conscious("heather", true)
-> quiet_room

= quiet_room
# place: day_ward_quiet_room
# shot: over-shoulder
# objective: Find somewhere quiet after group
- (room)
+ {room < 3} [Look: Sawsan]
    Sawsan, from group, on a prayer rug under the qibla arrow. Standing, bowing, kneeling, forehead to the rug. Then sitting, a string of beads moving through her fingers. # look: sawsan-praying
    -> room
+ {room < 3} [Look: The window]
    The tree in the car park is bare now. The bird feeder is busier. # look: car-park-winter
    -> room
* {room >= 1} [Wait (until she's finished)]
-
Sawsan: Sorry, I was just finishing. You can stay. You're not in my way. # vo: ch4-islam_sawsan_0035
* [Talk: Sawsan (ask about the beads)]
    ~ record_choice("ch4.p3.sawsan", "tasbih", "Sawsan showed Izzy her tasbih, her grandmother's prayer beads from Nablus.")
    Izzy: What are the beads? You were counting something. # vo: ch4-islam_izzy_0036
    Sawsan: A tasbih. Thirty-three beads. SubhanAllah, Alhamdulillah, Allahu Akbar, thirty-three times each. They were my teta's, my grandmother's, from Nablus. When my hands don't know what to do, they know this. # vo: ch4-islam_sawsan_0037
* [Talk: Sawsan (ask how she keeps praying here)]
    ~ record_choice("ch4.p3.sawsan", "here", "Sawsan told Izzy that praying five times a day gives her somewhere to be, even on the worst days.")
    Izzy: How do you keep doing it? Five times a day, in here? # vo: ch4-islam_izzy_0038
    Sawsan: On the bad days it's the only time I know exactly where I am. Facing that way. Doing this. Five little anchors. # vo: ch4-islam_sawsan_0039
-
Heather (inside): Five little anchors. I like that. # vo: ch4-islam_heather-inside_0040
-> invitation

= invitation
# place: day_ward_quiet_room
# shot: close
# objective:
Chaplain Amal: My friend Fadi holds a dhikr on Thursday nights. Remembrance. People sit together and repeat the names of God, very simply, over and over. Guests come and just listen. Would you like to come? # vo: ch4-islam_amal_0041
* [Talk: Chaplain Amal (ask if it's okay to just sit)]
    ~ record_choice("ch4.p3.invite", "sit", "When Amal invited you to a dhikr, you asked if you could just sit and listen. She said that's what guests do.")
    Izzy: Can we just... sit? Not join in? # vo: ch4-islam_izzy_0042
    Chaplain Amal: That's exactly what guests do. Sit at the edge. If you go far away, come out into the hall. I'll be there. # vo: ch4-islam_amal_0043
* [Talk: Chaplain Amal (say yes)]
    ~ record_choice("ch4.p3.invite", "yes", "When Amal invited you to a dhikr, Izzy said yes straight away.")
    Izzy: Yes. Obviously yes. Heather wants to go. Heather never wants to go anywhere. # vo: ch4-islam_izzy_0044
-
-> lunch

= lunch
// Owner: a full day in every part. Claude's scenes: lunch at the program, the evening before the Thursday dhikr.
# place: day_ward_group_room
# shot: wide
# objective: Eat lunch
- (table)
+ {table < 3} [Look: The trays]
    Turkey sandwich, apple, a cookie in plastic. Someone trades you their cookie for your apple. # look: ward-trays
    -> table
+ {table < 3} [Look: Sawsan]
    Sawsan, by the window, her grandmother's tasbih in her lap, eating crackers one at a time. # look: sawsan-window
    -> table
* [Talk: Sawsan (sit with her)]
    You sit. Neither of you talks. It's the best lunch all week. # look: sit-with-sawsan
-
-> dhikr

= dhikr
// Sufi dhikr: remembrance of God, repeating a phrase together ("La ilaha illallah"), often seated in a circle, sometimes with a
// gentle sway. Non-Muslim guests sit and listen.
# place: sufi_dhikr_room
# shot: wide
# objective: Sit at the edge of the circle
- (circle)
+ {circle < 5} [Look: The circle]
    Twenty people on cushions, knees almost touching. Old men, young women, a teenager in headphones around her neck. # look: dhikr-circle
    -> circle
+ {circle < 5} [Look: Fadi]
    White beard, a knitted cap, a string of prayer beads moving through his fingers. He smiles at you like he's been expecting you. # look: fadi
    -> circle
+ {circle < 5} [Look: The shelf by the door]
    A small round tablet of pale clay on a folded cloth. Fadi's, for prayer: earth from Karbala. Shia Muslims touch their foreheads to the earth when they pray. # look: fadi-turbah
    -> circle
+ {circle < 5} [Look: The shoes]
    Forty shoes in a row by the door. Yours at the end, a little apart. # look: shoes-row
    -> circle
* {circle >= 2} [Wait (sit at the edge)]
-
Fadi: Welcome, welcome. Sit anywhere. You don't have to say anything. The heart listens on its own. # vo: ch4-islam_fadi_0045
Fadi: Sunni, Shia, the curious, the tired: everyone sits in this circle. I'm Shia, from Beirut, a long time ago. God doesn't check at the door. # vo: ch4-islam_fadi_0062
La ilaha illallah. Over and over, low, together, like the sea. # look: dhikr-chant
~ set_experience("Depersonalization", 0.35)
* [Use: Mira's stone (eyes open)]
    ~ record_choice("ch4.p3.dhikr", "open", "At the dhikr, you kept your eyes open and held Mira's stone, and stayed.")
    Eyes open. The stone. Izzy counting the beads going round Fadi's fingers: thirty-one, thirty-two. You stay. # look: dhikr-eyes-open
    ~ nudge(-0.2)
* [Wait (close your eyes)]
    ~ record_choice("ch4.p3.dhikr", "closed", "At the dhikr, you closed your eyes and started to drift. Heather opened them again.")
    Dark, and the sound, and further... Heather opens your eyes. Gently. Not tonight. # look: dhikr-eyes-closed
    ~ set_experience("Depersonalization", 0.55)
-
Heather (inside): It's the same as the prayer wheels. Round and round. It holds you, if you keep your eyes open. # vo: ch4-islam_heather-inside_0046
Fadi: Remembering God, for us, is remembering where we are. That we are held. Come back any Thursday. # vo: ch4-islam_fadi_0047
~ set_experience("Depersonalization", 0.0)
~ journal("Note", "Dhikr on Thursdays. Fadi. Eyes open. Round and round: it holds you.")
-> after

= after
// Owner: Maya only appears sometimes.
# switch: maya
# place: sufi_dhikr_room
# shot: close
# objective:
* [Look: The shoes]
    Your shoes. The last pair. Your hands, tying them. Yours, for a minute. # look: maya-shoes
-
Maya: I'm here. I think I missed most of it. It feels... quiet. In a good way. # vo: ch4-islam_maya_0048
# switch: izzy
-> night

= night
# place: cta_train
# shot: over-shoulder
# objective: Ride home
- (car)
+ {car < 2} [Look: The window]
    December dark at six. Christmas lights on the porches going by. The words of the dhikr still going round in your chest. # look: l-december
    -> car
* [Use: The seat]
    You fall asleep against the window and wake up exactly at your stop. Heather, probably. # look: l-asleep
-
-> ch4_part4  // the next part of the same chapter (one run)
