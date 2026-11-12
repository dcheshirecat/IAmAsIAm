// DRAFT: not canon until the owner approves it. Prologue "This Little Light", Sunday 4.
// vo-speakers: Maya=maya-15
// ─────────────────────────────────────────────────────────────────────────────
//  Sunday 4. Iowa City, 2013. Maya is 15.
//  Owner's beats: an ordinary Sunday. In the car home, among other church news, Sarah mentions in
//  passing that Mira has died; she has no idea how hard it will hit. That night, home alone in her
//  room, Maya hears a voice inside that isn't hers (Nina, inside: only a voice, no control), and then
//  a blackout. This is the first switch (to Nina), and the end of the prologue.
//  Explore-first: the room is the scene. The dress, the letters, the napkin and the stone carry it.
//  On replays (an alter unlocked), the car is a moment where an unexpected switch may happen.
//
//  My guesses, for the owner to correct:
//   - Winter (February). Mira died "last week" in Oregon; her daughter Ellen phoned the church office.
//   - Thomas and Reneé are grown and gone; Mr. Carter still sits in the same pew.
//   - Rebecca is going out with Will.
//   - Maya still has Mira's napkin number (Sunday 2) and the unfinished "Dear Mira" letters (Sunday 3).
//   - Nina's first words, in the voice of "the minister's perfect, quiet child".
// ─────────────────────────────────────────────────────────────────────────────

=== prologue_sunday4 ===
# cw: childhood-reference
# cw: neglect
# cw: child-alone
# cw: derealization
# cw: time-loss
# cw: involuntary-switch
-> ordinary

= ordinary
# place: church_sanctuary
# shot: wide
# objective: Sit through the service
- (pews)
+ {pews < 4} [Look: Rebecca]
    Rebecca and Will, three rows back. Jamie and Justin kicking the pew. # look: rebecca-will
    -> pews
+ {pews < 4} [Look: Mr. Carter]
    Mr. Carter by the window. His arm along the back of the pew. The pew is empty. # look: carter-alone
    -> pews
+ {pews < 4} [Look: The pulpit]
    The lost sheep. The ninety-nine, and the one. # look: pulpit
    -> pews
* [Use: The hymnbook]
    ~ record_choice("prologue.s4.sermon", "hymnbook", "On the last Sunday, you read the hymnbook instead of listening.")
    "Jamie wuz here", in pencil, inside the back cover. # look: hymnbook-jamie
* [Use: Mira's stone (wait for the quiet part)]
    ~ record_choice("prologue.s4.sermon", "stone", "On the last Sunday, you held Mira's stone and waited for the quiet part.")
    Mira's stone. The white line, all the way around. # look: stone-line
* [Wait for the end]
    ~ record_choice("prologue.s4.sermon", "listened", "On the last Sunday, you listened to the sermon about the lost sheep.")
    The last hymn. # look: last-hymn
-
# place: church_fellowship_hall
# objective: Wait by the car
* [Use: The chairs]
    Chairs, stacked. Mrs. Hines, Mr. Jones and the new youth leader, all around Mom. # look: stacked-chairs
-
-> car

= car
# place: car
# shot: over-shoulder
# objective:
Sarah: Mr. Jones is having the other knee done in March. The roof fund is short again, I could scream. Rebecca's mother wants to start a book group, which is lovely, but on Wednesdays, which is choir. # vo: prologue-quaker_sarah_0105
Sarah: Oh, and Mira Smith passed. Last week, in Oregon. Her daughter phoned the office. I'll mention it in joys and concerns next Sunday. # vo: prologue-quaker_sarah_0106
~ set_experience("Derealization", 0.6)
~ nudge(0.35)
Sarah: Anyway, the youth group wants to do a car wash, in February, can you imagine... # vo: prologue-quaker_sarah_0107
# maybe-switch
* [Talk: Mom]
    ~ record_choice("prologue.s4.news", "spoke", "When Mom said Mira had died, you said \"Oh.\" It was all you could say.")
    Maya: Oh. # vo: prologue-quaker_maya-15_0108
    Sarah: Hm? Mm. She was a sweetheart. She was very fond of you. Now, the car wash... # vo: prologue-quaker_sarah_0109
* [Look: The wipers]
    ~ record_choice("prologue.s4.news", "silent", "When Mom said Mira had died, you didn't say anything. Mom didn't notice.")
    Wipers, back and forth. Like a picture of wipers. # look: wipers
* [Use: Mira's stone]
    ~ record_choice("prologue.s4.news", "stone", "When Mom said Mira had died, you gripped Mira's stone in your pocket until it hurt.")
    The stone's edge, biting your palm. The only real thing in the car. # look: stone-grip
-
Sarah: There's soup in the cupboard. I've got the deacons' supper, don't wait up. Love you! # vo: prologue-quaker_sarah_0110
-> room

= room
# place: home_maya_room
# shot: close
# objective:
- (things)
+ {things < 4} [Look: The closet]
    The blue dress, at the back. Too small. The sash still tied in a bow. # look: dress-small
    -> things
+ {things < 4} [Look: The desk drawer]
    ~ record_choice("prologue.s4.room", "letters", "That night, you read your four unfinished letters to Mira.")
    "Dear Mira." "Dear Mira." "Dear Mira." "Dear Mira." # look: four-letters
    -> things
+ {things < 4} [Use: The phone (the number on the napkin)]
    ~ record_choice("prologue.s4.room", "called", "That night, you called Mira's old number from the napkin. It was disconnected.")
    "The number you have dialed is no longer in service." # look: disconnected
    -> things
* [Use: Mira's stone]
    ~ record_choice("prologue.s4.room", "stone", "That night, you held Mira's stone and tried to feel your feet on the floor.")
    The stone, in both hands. Feet on the floor. The floor says nothing back. # look: stone-floor
-
~ set_experience("Depersonalization", 0.6)
~ set_experience("Derealization", 0.5)
~ nudge(0.4)
-> voice

= voice
# shot: inner
~ set_experience("CoConsciousness", 0.5)
Nina (inside): Don't cry. It's all right. # vo: prologue-quaker_nina-inside_0111
Nina (inside): You don't have to. I'm good at this part. I'm always good. # vo: prologue-quaker_nina-inside_0112
Nina (inside): Let me. I'll be quiet, and neat, and nobody will have to worry. I'll take care of it. # vo: prologue-quaker_nina-inside_0113
* [Talk: The voice (who are you?)]
    ~ record_choice("prologue.s4.voice", "who", "When the voice spoke, you asked who it was. It didn't answer.")
    Maya: Who are you? # vo: prologue-quaker_maya-15_0114
    Nina (inside): Shh. It's all right. You can rest. # vo: prologue-quaker_nina-inside_0115
* [Talk: The voice (no, I want to feel this)]
    ~ record_choice("prologue.s4.voice", "no", "When the voice spoke, you said no: you wanted to feel it. It came anyway.")
    Maya: No. I want to... I want to feel it. She was mine. She was the only... # vo: prologue-quaker_maya-15_0116
    Nina (inside): I know. I know. That's why. # vo: prologue-quaker_nina-inside_0117
* [Wait (let it)]
    ~ record_choice("prologue.s4.voice", "let", "When the voice spoke, you didn't say anything. You let it.")
    So tired. # look: so-tired
-
* [Look: Your hand]
    The stone, in somebody's hand. # look: somebody-hand
-
~ set_experience("TimeLoss", 1.0)
-> blackout

= blackout
...
# switch: nina
~ open_gap("prologue-first-switch", "dress,letters,stone")
This Little Light. # chapter-end
-> END
