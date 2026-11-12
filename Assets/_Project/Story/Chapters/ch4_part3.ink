// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 4 (Islam), Part 3.
// vo-speakers: Maya=maya, Chaplain Noor=noor
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Remembrance. Chicago, December 2026. The body is 28.
//  Owner's beats: Sufi practice; only what Muslims consider Islam. Izzy and Heather; Maya sometimes.
//
//  My guesses, for the owner to correct:
//   - Zahra (placeholder name; 30s, she/her), another patient at the day program: Shia (Twelver), Iraqi American. She prays in
//     the quiet room on a small clay tablet from Karbala (a turbah), and tells Izzy about Imam Hussain.
//   - Noor invites them to a Thursday-night dhikr (remembrance) gathering led by her friend Idris (placeholder name; 60s, he/him),
//     a Sufi teacher: guests are welcome to sit and listen. Heather finds the repetition steadying; Izzy keeps count, eyes open.
//     (Rhythm and repetition can pull a dissociative person away: they keep their eyes open, as Angie taught.)
// ─────────────────────────────────────────────────────────────────────────────

=== ch4_part3 ===
~ set_age(28)
# cw: religious-content
# cw: medical-setting
~ co_conscious("heather", true)
-> quiet_room

= quiet_room
# place: day_ward_quiet_room
# shot: over-shoulder
# objective: Find somewhere quiet after group
- (room)
+ {room < 3} [Look: Zahra]
    Zahra, from group, on a prayer rug under the qibla arrow. Standing, bowing, kneeling, her forehead resting on a small round tablet of clay. # look: zahra-praying
    -> room
+ {room < 3} [Look: The window]
    The tree in the car park is bare now. The bird feeder is busier. # look: car-park-winter
    -> room
* {room >= 1} [Wait (until she's finished)]
-
Zahra: Sorry, I was just finishing. You can stay. You're not in my way. # vo: ch4-islam_zahra_0035
* [Talk: Zahra (ask about the clay tablet)]
    ~ record_choice("ch4.p3.zahra", "turbah", "Zahra showed Izzy her turbah, the clay tablet from Karbala, and told her about Imam Hussain.")
    Izzy: What's the little stone? You put your head on it. # vo: ch4-islam_izzy_0036
    Zahra: A turbah. Clay from Karbala, where Imam Hussain was killed, the Prophet's grandson. We're Shia. When I put my head on it, I remember someone who stood up when it cost everything. # vo: ch4-islam_zahra_0037
* [Talk: Zahra (ask how she keeps praying here)]
    ~ record_choice("ch4.p3.zahra", "here", "Zahra told Izzy that praying five times a day gives her somewhere to be, even on the worst days.")
    Izzy: How do you keep doing it? Five times a day, in here? # vo: ch4-islam_izzy_0038
    Zahra: On the bad days it's the only time I know exactly where I am. Facing that way. Doing this. Five little anchors. # vo: ch4-islam_zahra_0039
-
Heather (inside): Five little anchors. I like that. # vo: ch4-islam_heather-inside_0040
-> invitation

= invitation
# place: day_ward_quiet_room
# shot: close
# objective:
Chaplain Noor: My friend Idris holds a dhikr on Thursday nights. Remembrance. People sit together and repeat the names of God, very simply, over and over. Guests come and just listen. Would you like to come? # vo: ch4-islam_noor_0041
* [Talk: Chaplain Noor (ask if it's okay to just sit)]
    ~ record_choice("ch4.p3.invite", "sit", "When Noor invited you to a dhikr, you asked if you could just sit and listen. She said that's what guests do.")
    Izzy: Can we just... sit? Not join in? # vo: ch4-islam_izzy_0042
    Chaplain Noor: That's exactly what guests do. Sit at the edge. If you go far away, come out into the hall. I'll be there. # vo: ch4-islam_noor_0043
* [Talk: Chaplain Noor (say yes)]
    ~ record_choice("ch4.p3.invite", "yes", "When Noor invited you to a dhikr, Izzy said yes straight away.")
    Izzy: Yes. Obviously yes. Heather wants to go. Heather never wants to go anywhere. # vo: ch4-islam_izzy_0044
-
-> dhikr

= dhikr
// Sufi dhikr: remembrance of God, repeating a phrase together ("La ilaha illallah"), often seated in a circle, sometimes with a
// gentle sway. Non-Muslim guests sit and listen.
# place: sufi_dhikr_room
# shot: wide
# objective: Sit at the edge of the circle
- (circle)
+ {circle < 4} [Look: The circle]
    Twenty people on cushions, knees almost touching. Old men, young women, a teenager in headphones around her neck. # look: dhikr-circle
    -> circle
+ {circle < 4} [Look: Idris]
    White beard, a knitted cap, a string of prayer beads moving through his fingers. He smiles at you like he's been expecting you. # look: idris
    -> circle
+ {circle < 4} [Look: The shoes]
    Forty shoes in a row by the door. Yours at the end, a little apart. # look: shoes-row
    -> circle
* {circle >= 2} [Wait (sit at the edge)]
-
Idris: Welcome, welcome. Sit anywhere. You don't have to say anything. The heart listens on its own. # vo: ch4-islam_idris_0045
La ilaha illallah. Over and over, low, together, like the sea. # look: dhikr-chant
~ set_experience("Depersonalization", 0.35)
* [Use: Mira's stone (eyes open)]
    ~ record_choice("ch4.p3.dhikr", "open", "At the dhikr, you kept your eyes open and held Mira's stone, and stayed.")
    Eyes open. The stone. Izzy counting the beads going round Idris's fingers: thirty-one, thirty-two. You stay. # look: dhikr-eyes-open
    ~ nudge(-0.2)
* [Wait (close your eyes)]
    ~ record_choice("ch4.p3.dhikr", "closed", "At the dhikr, you closed your eyes and started to drift. Heather opened them again.")
    Dark, and the sound, and further... Heather opens your eyes. Gently. Not tonight. # look: dhikr-eyes-closed
    ~ set_experience("Depersonalization", 0.55)
-
Heather (inside): It's the same as the prayer wheels. Round and round. It holds you, if you keep your eyes open. # vo: ch4-islam_heather-inside_0046
Idris: Remembering God, for us, is remembering where we are. That we are held. Come back any Thursday. # vo: ch4-islam_idris_0047
~ set_experience("Depersonalization", 0.0)
~ journal("Note", "Dhikr on Thursdays. Idris. Eyes open. Round and round: it holds you.")
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
-> ch4_part4  // the next part of the same chapter (one run)
