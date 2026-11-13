// IN THE GAME as a DRAFT (Claude, 2026-11-13; the owner's Doc edits come first). Chapter 9 (Humanism), Part 2.
// vo-speakers: Maya=maya, Miss Ella=ella, Maisie=maisie, Heather (inside)=heather-inside, Tanya=tanya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Darwin Day. Arcata, February 2032. Maya is 33.
//  Owner: Maya keeps showing up every week AND speaks up to the teacher (what Mira couldn't do: Mira raised it with Sarah and
//  backed down, Prologue Sunday 2).
//
//  My guesses, for the owner to correct:
//   - Darwin Day (February 12, Charles Darwin's birthday) is celebrated by many humanist groups as a day for science and
//     curiosity. Maya runs a Darwin Day afternoon at the museum, and the club comes on a field trip.
//   - Maisie lives with her grandmother, who works late shifts and picks her up at six. Nothing more is shown or explained:
//     Maya doesn't know Maisie's story, and the game never guesses at it out loud.
//   - Speaking up the right way: Maya tells Miss Ella what she's noticed (not a diagnosis, just what she sees), and Miss Ella
//     takes it to the school counsellor and to Maisie's grandmother. Maya doesn't try to fix it herself.
// ─────────────────────────────────────────────────────────────────────────────

=== ch9_part2 ===
~ set_age(33)
~ set_date(2032, 2, 12)  // Darwin Day, a Thursday in 2032 (the club comes on its field trip day)
# cw: childhood-reference
# cw: depersonalization
~ co_conscious("heather", true)
~ text_from("tanya", "Imbolc was a week ago and I KNOW you made a Brigid's cross out of Bríd's rushes. Picture please.")
-> wake

= wake
# place: arcata_apartment
# shot: close
# objective: Get ready for Darwin Day
- (room)
+ {room < 3} [Look: The door]
    Bríd's rush cross over it, a bit lopsided. Lily helped. # look: rush-cross-door
    -> room
+ {room < 3} [Look: The calendar]
    Every Wednesday since September has a small square drawn round it in purple. Twenty-two squares. You haven't missed one. # look: calendar-squares
    -> room
* [Use: Your bag (pack the fossils)]
    A shark's tooth, a trilobite cast, a fern in a slab of shale. Things you can hold. You put them in the bag in a row. # look: fossils-bag
-
-> museum

= museum
# place: natural_history_museum
# shot: wide
# objective: Welcome the club
- (hall)
+ {hall < 4} [Look: The banner]
    "HAPPY BIRTHDAY, CHARLES DARWIN! (1809)". You painted the cake on it yourself. It looks like a hat. # look: darwin-banner
    -> hall
+ {hall < 4} [Look: The tree of life]
    A long paper tree down the wall: every branch a living thing, the kids' names on the leaves where they've added a creature. # look: tree-of-life
    -> hall
+ {hall < 4} [Look: The club]
    The club pouring through the door in a wave, Miss Ella at the back counting heads. Maisie last, hood up. # look: club-arrives
    -> hall
* {hall >= 1} [Talk: Miss Ella]
-
Miss Ella: This is my favourite day of the year. Don't tell Christmas. # vo: ch9-humanism_ella_0013
Miss Ella: My church used to say this stuff was a lie. All of it. Now I bring kids to look at it and they lose their minds. Look at them. # vo: ch9-humanism_ella_0014
* [Talk: Miss Ella (ask what she believes now)]
    ~ record_choice("ch9.p2.ella", "asked", "On Darwin Day you asked Miss Ella what she believes now. She said: that this is the one life we know about, so we'd better take care of it, and each other.")
    Maya: What do you believe now? If you don't mind me asking. # vo: ch9-humanism_maya_0015
    Miss Ella: That this is the one life we know about. So we take care of it. And each other. That's the whole creed. It fits on a sticky note. Humanists aren't great at long services. # vo: ch9-humanism_ella_0016
* [Wait (watch the kids)]
    ~ record_choice("ch9.p2.ella", "watched", "On Darwin Day you watched the club lose their minds over fossils, and didn't need to ask Miss Ella anything.")
    Jayden holding a trilobite like it might wake up. Priya explaining to Luis, wrongly and with total confidence, how whales used to have legs. (They did.) # look: kids-fossils
-
-> fossils

= fossils
# place: natural_history_museum
# shot: over-shoulder
# objective: Give the fossil talk
- (talk)
+ {talk < 3} [Look: The tooth]
    A great white's tooth, bigger than a kid's palm. Fourteen hands go up at once to hold it. # look: shark-tooth-hands
    -> talk
+ {talk < 3} [Look: Maisie]
    At the edge of the group, not holding anything. Her eyes on the case, or through it. She's gone again. # look: maisie-gone
    -> talk
* {talk >= 1} [Talk: Maisie]
-
~ set_experience("Derealization", 0.3)
Maya: Maisie? # vo: ch9-humanism_maya_0017
Maya: Maisie. It's Maya. I've got something for you to hold. # vo: ch9-humanism_maya_0018
* [Use: The fern fossil (hold it out)]
    ~ record_choice("ch9.p2.fossil", "fern", "On Darwin Day, when Maisie went far away, you put a fossil fern in her hands and told her how old it was, and waited.")
    Maya: This was a fern. Three hundred million years ago. Feel the lines? It's cold, right? It's heavy. # vo: ch9-humanism_maya_0019
* [Use: The shark tooth (hold it out)]
    ~ record_choice("ch9.p2.fossil", "tooth", "On Darwin Day, when Maisie went far away, you put a shark's tooth in her hands and asked her to count the little ridges on its edge, and waited.")
    Maya: Count the little ridges on the edge. I always lose count at about twenty. # vo: ch9-humanism_maya_0020
-
A long time. Then her thumb moves on the stone. # look: maisie-thumb-stone
Maisie: ...It's cold. # vo: ch9-humanism_maisie_0021
Maya: It is. You're here. That's all. You're here with me. # vo: ch9-humanism_maya_0022
Maisie: Sometimes I go somewhere. In my head. I don't mean to. # vo: ch9-humanism_maisie_0023
* [Talk: Maisie (say you know)]
    ~ record_choice("ch9.p2.maisie", "know", "Maisie told you she goes somewhere in her head. You told her you knew what that was like.")
    Maya: I know. I used to go somewhere too. Still do, sometimes. It's not your fault. # vo: ch9-humanism_maya_0024
* [Talk: Maisie (say she can tell you)]
    ~ record_choice("ch9.p2.maisie", "tell", "Maisie told you she goes somewhere in her head. You told her she could always tell you when it happened.")
    Maya: You can tell me when it happens. Any Wednesday. You don't have to explain. # vo: ch9-humanism_maya_0025
-
Maisie: Okay. # vo: ch9-humanism_maisie_0026
Heather (inside): She held on. Now the next part. The part nobody did for us. # vo: ch9-humanism_heather-inside_0027
~ nudge(-0.2)
-> after

= after
# place: club_classroom
# shot: close
# objective: Talk to Miss Ella
- (room)
+ {room < 3} [Look: The window]
    Six o'clock and nearly dark. Maisie's grandmother's car at the curb, engine running, still in her scrubs. Maisie gets in without looking back. # look: grandma-car
    -> room
+ {room < 3} [Look: Miss Ella]
    Stacking chairs, alone, humming. # look: ella-chairs
    -> room
* {room >= 1} [Talk: Miss Ella]
-
This is the part. A fellowship hall, a long time ago. A kind woman who said something, once, and then let it go. # look: mira-backed-down
* [Talk: Miss Ella (tell her what you've seen)]
    ~ record_choice("ch9.p2.spoke", "told", "After Darwin Day you told Miss Ella what you'd seen in Maisie: how she goes away and doesn't hear her name. You didn't let it go.")
    Maya: Can I tell you something about Maisie? I'm not a professional. I just... I see her go away. Every week. She doesn't hear her name. Today she told me she goes somewhere in her head and doesn't mean to. # vo: ch9-humanism_maya_0028
* [Talk: Miss Ella (tell her why it matters to you)]
    ~ record_choice("ch9.p2.spoke", "why", "After Darwin Day you told Miss Ella about Maisie, and why: that you were that kid once, and the grown-up who noticed let it go.")
    Maya: I need to tell you something about Maisie. I was that kid. A woman at my mom's church saw it, and she said something, once, and then she let it go. I don't want to let it go. # vo: ch9-humanism_maya_0029
-
Miss Ella: Thank you. Really. I've seen it too, and I wasn't sure I wasn't imagining it. # vo: ch9-humanism_ella_0030
Miss Ella: I'll talk to the school counsellor tomorrow, and then to her grandma, together. Gently. The right way. # vo: ch9-humanism_ella_0031
Miss Ella: You did the thing grown-ups are supposed to do, Maya. Most of them don't. # vo: ch9-humanism_ella_0032
Maya: Nobody did it for me. # vo: ch9-humanism_maya_0033
Miss Ella: Then you know exactly why it matters. Same as me. # vo: ch9-humanism_ella_0034
~ set_experience("Depersonalization", 0.2)
-> night

= night
# place: arcata_apartment
# shot: close
# objective: Go home
- (room)
+ {room < 2} [Look: The sill]
    Mira's stone. You hold it a while. "You tried," you tell it. "I'm trying the next bit." # look: miras-stone-trying
    -> room
* [Use: The bed]
-
~ system_log("heather", "You didn't let it go. I'm proud of you. -H")
~ text_to("tanya", "I told the teacher about the girl at the club. The one who goes far away. I didn't let it go, Tanya.")
~ text_from("tanya", "I know you didn't. You never would. Oh, love. Mira would be so glad.")
~ journal("Note", "Darwin Day. Maisie held a fern three hundred million years old, and came back. I told Miss Ella. I didn't let it go.")
-> ch9_part3  // the next part of the same chapter (one run)
