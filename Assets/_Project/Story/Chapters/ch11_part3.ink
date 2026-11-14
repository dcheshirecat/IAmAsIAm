// IN THE GAME as a DRAFT (Claude, 2026-11-14; the owner's Doc edits come first). Chapter 11 (Jainism), Part 3.
// vo-speakers: Maya=maya, Nirmala-ben=nirmala, Serena (inside)=serena-inside, Heather (inside)=heather-inside, Jacob (inside)=jacob-inside
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Lamps. The Bay Area and Arcata, November 2034. Maya is 36.
//  Owner: the Paryushana arc ends at Diwali. Unlocks nobody.
//
//  Research: for Jains, Diwali marks Mahavira's nirvana (his final liberation) at Pavapuri; lamps stand in for the light of his
//  knowledge, and the next day is the Jain New Year. (2034: Diwali around November 10.)
//
//  My guesses, for the owner to correct:
//   - Replies: Tanya and Mark write back; Adeena doesn't (and that's all right). Sarah: if Maya wrote, a two-line reply.
//   - Nirmala-ben gives Maya a small soft broom like the one used to sweep in front of you so as not to step on anything.
// ─────────────────────────────────────────────────────────────────────────────

=== ch11_part3 ===
~ set_age(36)
~ set_date(2034, 11, 10)  // Diwali 2034 (calendar listings)
# cw: religious-content
~ co_conscious("serena", true)
-> wake

= wake
# place: arcata_apartment
# shot: close
# objective: Read your replies
- (room)
+ {room < 3} [Look: The drawing]
    "ME AND MIRA", in a frame from the thrift store, on the wall above the altar now. Mira's handwriting on the back, facing the wall, safe. # look: drawing-framed
    -> room
+ {room < 3} [Look: The window]
    November rain, steady, the gutters singing. # look: november-gutters
    -> room
* [Use: Your laptop (read your email)]
-
Tanya writes back. "Forgiven, years ago, every day of it. Come for Yule if you can. Bring all of them." # look: reply-tanya
Mark writes back. "Maya. I didn't expect this. Thank you. I'm married now, two kids, still at St. Luke's on holidays. I forgave you a long time ago. I hope you'll forgive me for being so confused. Be well." # look: reply-mark
Nothing from Adeena. The silence sits there, and it's allowed to. # look: no-reply-adeena
{ch11_part2.forgive.wrote_sarah:
    Mom writes back. "Thank you for writing. I don't know that word but I looked it up. Take care of yourself. Mom." Two lines. You read them twice. # look: reply-sarah
}
Serena (inside): Two out of three. That's a good year. # vo: ch11-jainism_serena-inside_0012
-> drive

= drive
# place: car_highway_101
# shot: wide
# objective: Drive south for Diwali
- (road)
+ {road < 2} [Look: The rain]
    The wipers going, the redwoods black and dripping. Somebody in here hums the whole way. # look: rain-wipers-hum
    -> road
* [Wait (drive)]
-
-> diwali

= diwali
# place: jain_centre
# shot: wide
# objective: Light a lamp for Mahavira
- (lamps)
+ {lamps < 4} [Look: The lamps]
    Hundreds of little clay lamps on every step and sill, in rows, in spirals. The temple room doors open wide tonight. # look: jain-diwali-lamps
    -> lamps
+ {lamps < 4} [Look: The children]
    Pathshala kids in their best clothes, carefully not running, carefully carrying a lamp each with both hands. # look: kids-lamps
    -> lamps
+ {lamps < 4} [Look: The image of Mahavira]
    Seated, still, eyes half closed, garlanded tonight. The night he was freed from everything that held him, Nirmala-ben says. Not by a god. By his own long, patient work. # look: mahavira-garland
    -> lamps
* {lamps >= 1} [Talk: Nirmala-ben]
-
Nirmala-ben: Maya! Micchami Dukkadam was good? You wrote? # vo: ch11-jainism_nirmala_0028
Maya: I wrote. Some wrote back. Some didn't. # vo: ch11-jainism_maya_0013
Nirmala-ben: That's how it goes. You don't forgive or ask to get something. You do it to put the weight down. The rest is theirs to carry or not. # vo: ch11-jainism_nirmala_0029
Nirmala-ben: Here. For you. I saw you looking at mine. # vo: ch11-jainism_nirmala_0030
A little soft broom, wool on a wooden handle, like the ones for sweeping in front of you so you don't step on anything small. # look: soft-broom-gift
Nirmala-ben: Not for ants. For you. Sweep a little space in front of yourself, when the cruel voice starts. Be gentle with the jiva you live in. # vo: ch11-jainism_nirmala_0031
* [Talk: Nirmala-ben (thank her)]
    ~ record_choice("ch11.p3.broom", "thanked", "At Diwali, Nirmala-ben gave you a little soft broom, to sweep a gentle space in front of yourself when the cruel voice starts.")
    Maya: Thank you. For all of it. For feeding us. # vo: ch11-jainism_maya_0014
    Nirmala-ben: Feeding people is half of religion. Maybe more. # vo: ch11-jainism_nirmala_0032
-
-> light

= light
# place: jain_centre
# shot: close
# objective: Light your lamp
* [Use: A lamp (light it)]
    ~ record_choice("ch11.p3.lamp", "lit", "At Diwali you lit a clay lamp at the Jain centre, for the light of a man who freed himself by patient work.")
-
The wick catches. A small steady flame, the kind you could carry with both hands. # look: clay-lamp-lit
Heather (inside): That's a good flame. # vo: ch11-jainism_heather-inside_0015
Jacob (inside): Saint Lucy would like it. Don't tell them I said that in a Jain temple. # vo: ch11-jainism_jacob-inside_0016
Serena (inside): I'll tell no one. Happy Diwali, all of you. # vo: ch11-jainism_serena-inside_0017
~ nudge(-0.3)
-> home

= home
# place: arcata_apartment
# shot: close
# objective: Sweep a little space
- (room)
* [Use: The soft broom (sweep in front of the altar)]
    ~ record_choice("ch11.p3.sweep", "swept", "Home from Diwali, you swept a little space in front of the altar with Nirmala-ben's soft broom. The cruel voice was quiet.")
    Three soft strokes on the floor in front of the altar. Nothing there to hurt. That's the point. The cruel little voice, for once, doesn't say a thing. # look: sweep-space
-
~ system_log("maya", "Diwali at the Jain centre. Nirmala-ben gave us a soft broom: sweep a gentle space in front of yourself. Tanya and Mark wrote back. Adeena didn't. That's okay. -M")
~ system_log("heather", "I'm putting the broom by the bed. -H")
~ text_to("anabelle", "Happy Diwali from a Jain temple! I have the recipe. It has no onions. You'll survive.")
~ text_from("anabelle", "I will NOT survive but I'll try. Happy Diwali!!!")
~ journal("Note", "Jain Diwali: the night Mahavira was freed, by his own patient work. A clay lamp. A soft broom from Nirmala-ben, to sweep a gentle space in front of myself. Be gentle with the one you live in.")
# shot: inner
# place: black
Arcata, California. November 2034. # chapter-end
-> END
