// DRAFT: not canon until the owner approves it. Prologue "This Little Light", Sunday 3.
// vo-speakers: Maya=maya-11
// ─────────────────────────────────────────────────────────────────────────────
//  Sunday 3. Iowa City, 2009. Maya is 11.
//  Owner's beats: Maya "performs okay" (neat clothes, a smile, good answers), and Mira believes
//  it. Mira moves away. Small choices only. This Sunday also holds Maya's first real time loss.
//  Explore-first: the performing is something the player DOES (the dress, the bulletins, the right
//  words at the door), not narration.
//  Hidden memory (owner): only while Nina is fronting at that moment does the blue dress glow, and
//  looking at it shows what Nina did in the lost nine hours. Nothing ever tells the player this:
//  for anyone else the dress simply isn't one of the glowing things.
//
//  My guesses, for the owner to correct:
//   - Spring (May). Mira, now about 74, is moving to Oregon to live near her daughter, Ellen.
//   - Mira gives Maya a smooth stone from her garden ("something to hold"): the grounding-object skill.
//   - Mira gives Maya her new address; Maya means to write and doesn't.
//   - The first time loss: after the goodbye, Maya is suddenly at home in her room, with no memory of
//     the drive. Nina (11, "the minister's perfect, quiet child") did the smiling and the hours;
//     she isn't named until Sunday 4.
// ─────────────────────────────────────────────────────────────────────────────

=== prologue_sunday3 ===
~ set_age(11)
# cw: childhood-reference
# cw: neglect
# cw: depersonalization
# cw: time-loss
-> morning

= morning
# place: home_maya_room
# shot: close
# objective: Look perfect for church
- (room)
+ [Look: The mirror]
    A smile. Practiced until it looks right. # look: mirror-smile
    -> room
* [Use: The blue dress]
    The blue dress, ironed last night. The collar straight. # look: blue-dress
-
* [Use: Your hair]
    One hundred brushes. The braid. The clip. # look: braid
-
Sarah: Don't you look nice. Grab the bulletins, would you, sweetheart? They're on the printer. # vo: prologue-quaker_sarah_0080
# objective: Bring the bulletins
* [Use: The bulletins]
    A stack of bulletins, still warm from the printer. # look: bulletins
-
-> church

= church
# place: church_sanctuary
# shot: wide
# objective: Hand out the bulletins
Mrs. Hines: Good morning, Maya. Aren't you a credit to your mother. # vo: prologue-quaker_hines_0081
Maya: Good morning, Mrs. Hines. I like your brooch. # vo: prologue-quaker_maya-11_0082
Mr. Jones: There's our girl. # vo: prologue-quaker_jones_0083
Maya: Good morning, Mr. Jones. How's your knee? # vo: prologue-quaker_maya-11_0084
Rebecca: Hey, Maya. # vo: prologue-quaker_rebecca_0085
Maya: Hey! I like your ponytail. # vo: prologue-quaker_maya-11_0086
Rebecca: Thanks. Will, wait up! # vo: prologue-quaker_rebecca_0087
- (door)
+ {door < 3} [Look: The Carters]
    Mr. Carter holding the door. Reneé on her crutches. A nod, the kind big kids give little kids. # look: carters-door
    -> door
+ {door < 3} [Look: Your face in the glass door]
    Smiling. Somebody very good at it. # look: glass-smile
    ~ set_experience("Depersonalization", 0.25)
    -> door
* [Go: Into worship]
-
-> news

= news
# place: church_fellowship_hall
# shot: wide
# objective: Sit with Mira
Mira: Maya, dear. Will you sit with me a minute? I've some news, and I wanted you to hear it from me. # vo: prologue-quaker_mira_0088
Mira: I'm moving to Oregon. To be near my daughter, Ellen. My legs aren't what they were, and she worries. # vo: prologue-quaker_mira_0089
Mira: I leave on Thursday. # vo: prologue-quaker_mira_0090
# objective:
* [Talk: Mira (say it's great news)]
    ~ record_choice("prologue.s3.news", "great", "When Mira said she was moving away, you told her it was great news.")
    Maya: That's great! Oregon is really pretty. We did it in social studies. # vo: prologue-quaker_maya-11_0091
    Mira: It is pretty. I'll send you pictures. # vo: prologue-quaker_mira_0092
* [Talk: Mira (ask her not to go)]
    ~ record_choice("prologue.s3.news", "asked", "When Mira said she was moving away, you almost asked her not to go. You asked about the weather instead.")
    Maya: Does it rain a lot there? # vo: prologue-quaker_maya-11_0093
    Mira: Oh, all the time. I'll grow moss, like my cardigan. # vo: prologue-quaker_mira_0094
* [Look: The window]
    ~ record_choice("prologue.s3.news", "nothing", "When Mira said she was moving away, you didn't say anything for a long time.")
    A fly on the window, walking up and falling down. # look: fly-window
-
-> the_question

= the_question
Mira: Maya. Before I go, I want to ask you something, and I want a true answer. Are you all right? At home? Really? # vo: prologue-quaker_mira_0095
# objective: Tell Mira the truth
~ set_experience("Depersonalization", 0.4)
* [Talk: Mira (say you're fine)]
    ~ record_choice("prologue.s3.question", "fine", "Mira asked if you were really all right. You told her you were fine.")
    Maya: I'm fine! Really. I'm good at looking after myself. Mom says I'm very independent. # vo: prologue-quaker_maya-11_0096
* [Talk: Mira (say it's better now)]
    ~ record_choice("prologue.s3.question", "better", "Mira asked if you were really all right. You told her things were better now.")
    Maya: It's better now. I'm older. I can make pasta and everything. # vo: prologue-quaker_maya-11_0097
* [Talk: Mira (start to tell her)]
    ~ record_choice("prologue.s3.question", "almost", "Mira asked if you were really all right. You almost told her. Then you said you were fine.")
    Maya: Sometimes it's... # vo: prologue-quaker_maya-11_0098
    Maya: Sometimes it's boring, that's all. When Mom's at meetings. But I read a lot. I'm fine. # vo: prologue-quaker_maya-11_0099
-
# objective:
Mira: Well. You've grown into a very capable young lady. I'm proud of you. # vo: prologue-quaker_mira_0100
Mira: I'm glad. I'm so glad, Maya. # vo: prologue-quaker_mira_0101
-> gift

= gift
Mira: From my garden. I used to hold it in meeting, when my mind wandered off. You can have it now. For when you feel far away. # vo: prologue-quaker_mira_0102
* [Look: Mira's stone]
    A stone, grey and flat and smooth as soap. A white line all the way around. # look: mira-stone
-
Something to hold. # exercise: grounding-object
~ learn_skill("grounding-object")
~ nudge(-0.1)
Mira: Write to me. Tell me about school, and books, and everything. # vo: prologue-quaker_mira_0103
Maya: I will. I promise. # vo: prologue-quaker_maya-11_0104
* [Look: The card]
    Mira's new address, in Oregon. Shaky blue pen. # look: mira-address
-
~ journal("Note", "Mira's stone. Mira's address in Oregon. Write to her.")
* [Look: Mira, at the door]
    Mira's hand, lifting, at the door. # look: mira-wave
-
-> lost

= lost
# shot: inner
~ set_experience("Depersonalization", 0.7)
~ nudge(0.3)
~ set_experience("TimeLoss", 0.8)
~ open_gap("prologue-s3-goodbye", "stone,dark")
...
# place: home_maya_room
# shot: close
# objective: What happened?
- (gap)
+ {not time} [Look: The clock] -> time
+ {not stone} [Look: Your pillow] -> stone
+ {not dress and fronts("nina")} [Look: The blue dress] -> dress
* {time or stone} [Use: Feet on the floor] -> steady
* {time or stone} [Use: Mira's stone] -> steady

= time
~ find_clue("prologue-s3-goodbye", "dark")
~ record_choice("prologue.s3.lost", "time", "The first time you lost time, you checked the clock. Nearly nine hours were gone.")
8:47. Church ended at noon. # look: clock-847
-> lost.gap

= stone
~ find_clue("prologue-s3-goodbye", "stone")
~ record_choice("prologue.s3.lost", "stone", "The first time you lost time, you looked for Mira's stone. It was on your pillow.")
Mira's stone, in the middle of your pillow. # look: stone-pillow
-> lost.gap

= dress
~ record_choice("prologue.s3.lost", "dress", "The first time Maya lost time, you looked at the blue dress, and remembered the hours.")
The blue dress on its hanger. The sash tied in a bow. # look: dress-bow
-> nina_memory

= nina_memory
// Hidden memory (owner): only reachable while Nina is fronting. What Nina did in the lost hours.
# memory: nina-lost-hours
# shot: inner
* [Look: The car, at the curb]
    Waiting at the curb with the bulletins. Smiling for Mrs. Hines. # look: nina-curb
-
* [Look: The kitchen]
    Toast, cut into triangles. The plate rinsed. Nobody asked. # look: nina-toast
-
* [Look: The closet]
    The dress, hung up perfectly. The sash, a bow. Good girls are neat. # look: nina-dress
-
-> lost.gap

= steady
~ set_experience("Derealization", 0.0)
~ set_experience("TimeLoss", 0.0)
~ nudge(-0.2)
# objective: Write to Mira
* [Use: A piece of paper]
    "Dear Mira," # look: dear-mira
-
* [Use: A piece of paper]
    "Dear Mira," # look: dear-mira-2
-
* [Use: Mira's stone]
    The stone, in your pocket. Every Sunday. # look: stone-pocket
-
# objective:
-> END
