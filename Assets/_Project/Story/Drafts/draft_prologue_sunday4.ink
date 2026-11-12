// DRAFT: not canon until the owner approves it. Prologue "This Little Light", Sunday 4.
// vo-speakers: Maya=maya-15
// ─────────────────────────────────────────────────────────────────────────────
//  Sunday 4. Iowa City, 2013. Maya is 15.
//  Owner's beats: an ordinary Sunday. In the car home, among other church news, Sarah mentions in
//  passing that Mira has died; she has no idea how hard it will hit. That night, home alone in her
//  room, Maya hears a voice inside that isn't hers (Nina, inside: only a voice, no control), and then
//  a blackout. This is the first switch (to Nina), and the end of the prologue.
//  The writing is a fifteen-year-old's.
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
# place: church_sanctuary
# shot: wide
-> ordinary

= ordinary
It's an ordinary Sunday.

Maya is fifteen. She sits in the front pew, where she has always sat, though she's too old now for anybody to need to see her. She has Mira's stone in her coat pocket. She always does.

Rebecca sits three rows back with Will. They're going out now. Jamie and Justin sit behind them and kick the pew until Rebecca's mom turns around.

Mr. Carter sits where he always sits, on the left, by the window. Thomas and Reneé are grown up and gone. He still puts his arm along the back of the pew, out of habit, along the empty wood.

They sing. Mom preaches. Mom is a good preacher. Everybody says so. Today it's about the lost sheep, and how the shepherd leaves the ninety-nine to look for the one.

* [Listen.]
    ~ record_choice("prologue.s4.sermon", "listened", "On the last Sunday, you listened to the sermon about the lost sheep.")
    Maya listens. It's a good sermon. She's heard it before, in other years, with other jokes.
* [Read the hymnbook instead.]
    ~ record_choice("prologue.s4.sermon", "hymnbook", "On the last Sunday, you read the hymnbook instead of listening.")
    Maya reads the hymnbook. Somebody has written "Jamie wuz here" inside the back cover in pencil, years ago. She's seen it a hundred times.
* [Hold the stone and wait for the quiet part.]
    ~ record_choice("prologue.s4.sermon", "stone", "On the last Sunday, you held Mira's stone and waited for the quiet part.")
    Maya holds the stone in her pocket. The white line goes all the way around. She follows it with her thumb, around and around, until the quiet part comes.
-
After, there's coffee. Mrs. Hines needs Mom. Mr. Jones needs Mom. The new youth leader needs Mom.

Maya stacks chairs in the fellowship hall, because somebody has to, and waits by the car.
-> car

= car
# place: car
# shot: over-shoulder
In the car, Mom does the news. She does it every week now, like Maya is a secretary.
Sarah: Mr. Jones is having the other knee done in March. The roof fund is short again, I could scream. Rebecca's mother wants to start a book group, which is lovely, but on Wednesdays, which is choir. # vo: prologue-quaker_sarah_0105

The wipers go back and forth. The sky is the color of dishwater. It always is, in February.
Sarah: Oh, and Mira Smith passed. Last week, in Oregon. Her daughter phoned the office. I'll mention it in joys and concerns next Sunday. # vo: prologue-quaker_sarah_0106
Sarah: Anyway, the youth group wants to do a car wash, in February, can you imagine... # vo: prologue-quaker_sarah_0107

Mom keeps talking.

Maya doesn't hear any of it.
~ set_experience("Derealization", 0.6)
~ nudge(0.35)

The car is very bright. The dashboard is very clear, every scratch, every crumb. The wipers go back and forth like a film of wipers. Like a picture of a car, with a picture of a girl in it.

* [Say something.]
    ~ record_choice("prologue.s4.news", "spoke", "When Mom said Mira had died, you said \"Oh.\" It was all you could say.")
    Maya: Oh. # vo: prologue-quaker_maya-15_0108
    Sarah: Hm? Mm. She was a sweetheart. She was very fond of you. Now, the car wash... # vo: prologue-quaker_sarah_0109
* [Say nothing.]
    ~ record_choice("prologue.s4.news", "silent", "When Mom said Mira had died, you didn't say anything. Mom didn't notice.")
    Maya doesn't say anything. Mom doesn't notice that she doesn't say anything.
* [Grip the stone.]
    ~ record_choice("prologue.s4.news", "stone", "When Mom said Mira had died, you gripped Mira's stone in your pocket until it hurt.")
    Maya grips the stone in her pocket so hard the edge bites her palm. It's the only real thing in the car.
-
Mom drops her at the house.
Sarah: There's soup in the cupboard. I've got the deacons' supper, don't wait up. Love you! # vo: prologue-quaker_sarah_0110

The car goes. The house is dark.
-> room

= room
# place: home_maya_room
# shot: close
Maya's room.

The blue dress doesn't fit anymore. It's still in the closet, at the back, on its hanger, with the sash tied in a bow. She never takes it out. She never gives it away.

In her desk drawer there are four pieces of paper that say "Dear Mira" and nothing else.

And there's a napkin, soft as cloth now, with a phone number on it in shaky blue pen.

* [Read the letters.]
    ~ record_choice("prologue.s4.room", "letters", "That night, you read your four unfinished letters to Mira.")
    Dear Mira. Dear Mira. Dear Mira. Dear Mira.
    Four years of meaning to.
* [Call the number on the napkin.]
    ~ record_choice("prologue.s4.room", "called", "That night, you called Mira's old number from the napkin. It was disconnected.")
    Maya dials it on the house phone. Her hands are very steady. That's strange, isn't it, how steady they are.
    Three rising notes. A woman's voice that isn't anybody's: "The number you have dialed is no longer in service."
    She listens to it twice.
* [Hold the stone.]
    ~ record_choice("prologue.s4.room", "stone", "That night, you held Mira's stone and tried to feel your feet on the floor.")
    Maya sits on the floor with her back against the bed and holds the stone in both hands. Feet on the floor. Hello, floor.
    The floor doesn't say anything back. It never did. That was the point.
-
She's going to cry. She can feel it coming, from very far down, like weather.

She hasn't cried in front of anybody since she was five. Nobody is here. She could.
~ set_experience("Depersonalization", 0.6)
~ set_experience("Derealization", 0.5)
~ nudge(0.4)
-> voice

= voice
# shot: inner
And then there's a voice.

Not out loud. Inside. Close, like someone sitting right behind her eyes. A girl's voice, younger than her. Very polite. Very calm.
~ set_experience("CoConsciousness", 0.5)
Nina (inside): Don't cry. It's all right. # vo: prologue-quaker_nina-inside_0111

Maya goes very still.

That isn't her. That isn't her thinking. She knows what her own thinking sounds like, and that isn't it.
Nina (inside): You don't have to. I'm good at this part. I'm always good. # vo: prologue-quaker_nina-inside_0112
Nina (inside): Let me. I'll be quiet, and neat, and nobody will have to worry. I'll take care of it. # vo: prologue-quaker_nina-inside_0113

* [Who are you?]
    ~ record_choice("prologue.s4.voice", "who", "When the voice spoke, you asked who it was. It didn't answer.")
    Maya: Who are you? # vo: prologue-quaker_maya-15_0114
    Nina (inside): Shh. It's all right. You can rest. # vo: prologue-quaker_nina-inside_0115
* [No. I want to feel this.]
    ~ record_choice("prologue.s4.voice", "no", "When the voice spoke, you said no: you wanted to feel it. It came anyway.")
    Maya: No. I want to... I want to feel it. She was mine. She was the only... # vo: prologue-quaker_maya-15_0116
    Nina (inside): I know. I know. That's why. # vo: prologue-quaker_nina-inside_0117
* [Say nothing. Let it.]
    ~ record_choice("prologue.s4.voice", "let", "When the voice spoke, you didn't say anything. You let it.")
    Maya doesn't say anything. She's so tired. She has been so tired for so long.
-
The stone is in her hand. Then it's in somebody's hand.

The room goes soft at the edges, like a photograph left in the sun.
~ set_experience("TimeLoss", 1.0)
-> blackout

= blackout
...

...
# switch: nina
~ open_gap("prologue-first-switch", "dress,letters,stone")

This Little Light. # chapter-end
-> END
