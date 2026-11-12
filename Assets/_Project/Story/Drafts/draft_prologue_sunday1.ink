// DRAFT: not canon until the owner approves it. Prologue "This Little Light", Sunday 1.
// vo-speakers: Maya=maya-05
// ─────────────────────────────────────────────────────────────────────────────
//  Sunday 1. Iowa City, 2003. Maya is 5.
//  Owner's beats: Maya goes "floaty" in the long service (a first, gentle depersonalization),
//  then is forgotten at church after it. Mira finds her. Small choices only.
//  Explore-first (owner): little narration. Each place is things to look at, use or talk to (they glow
//  in the game); the objective line points the way; looking at a thing shows only its label.
//
//  My guesses, for the owner to correct:
//   - The season (a grey, cold March morning).
//   - First Day School comes BEFORE worship (common in programmed Friends churches).
//   - The service: "This Little Light of Mine" (a public-domain spiritual, the title echo), Sarah's sermon,
//     then a few minutes of open worship (silence), as many pastoral Friends churches do.
//   - The smell of the building (wood polish, old hymnbooks, coffee) is planted here: it is the
//     adult trigger "meeting-house-smell". No trigger cue fires in the prologue.
//   - Mira teaches "feet on the floor", the first skill.
//   - Mira only says "She's been waiting a long while" to Sarah here; she speaks up properly on Sunday 2.
//   - Sarah is seen only from outside, through Maya. We never hear her reasons.
// ─────────────────────────────────────────────────────────────────────────────

=== prologue_sunday1 ===
# cw: childhood-reference
# cw: neglect
# cw: child-alone
# cw: depersonalization
-> morning

= morning
# place: home_kitchen
# objective: Get ready for church
Sarah: Yes. Yes, Mrs. Hines, I'll bring the budget sheets. No, I haven't forgotten. # vo: prologue-quaker_sarah_0001
- (kitchen)
+ [Look: The kitchen table]
    The kitchen table. A coffee cup, Mommy's Bible, sermon papers. No bowl. # look: kitchen-table
    -> kitchen
+ [Look: Mommy's papers]
    Sermon notes: "Let your light shine." # look: sermon-notes
    -> kitchen
* [Talk: Mommy]
    ~ record_choice("prologue.s1.breakfast", "asked", "You asked for breakfast. Mommy said there would be snacks at church.")
    Maya: Mommy? I'm hungry. # vo: prologue-quaker_maya-05_0002
    Sarah: There'll be snacks at church, honey. Get your coat. # vo: prologue-quaker_sarah_0003
* [Use: The cracker cupboard]
    ~ record_choice("prologue.s1.breakfast", "cracker", "You found a cracker in the cupboard by yourself.")
    A chair, pushed to the cupboard. One soft cracker. # look: cracker-cupboard
* [Wait by the door]
    ~ record_choice("prologue.s1.breakfast", "waited", "You waited quietly, because Mommy was busy.")
    ~ nudge(0.05)
    Waiting. # look: front-door
-
Sarah: Shoes, Maya. We're late. # vo: prologue-quaker_sarah_0004
# objective: Put your shoes on
* [Use: Your shoes]
    Shoes with sticky straps. On the wrong feet. # look: shoes
-
-> car

= car
# place: car
# shot: over-shoulder
# objective: Go to church
Sarah: "Let your light shine." No. "Each of us carries a light." Each of us... # vo: prologue-quaker_sarah_0005
* [Look: Red cars]
    ~ record_choice("prologue.s1.car", "counted", "In the car, you counted red cars.")
    Red cars: five. # look: red-cars
* [Talk: Mommy]
    ~ record_choice("prologue.s1.car", "asked", "In the car, you asked Mommy where light comes from. She didn't hear.")
    Maya: Mommy, where does light come from? # vo: prologue-quaker_maya-05_0006
    Sarah: Each of us carries... hm? In a minute, sweetheart. # vo: prologue-quaker_sarah_0007
* [Use: The foggy window]
    ~ record_choice("prologue.s1.car", "drew", "In the car, you drew a little house on the foggy window.")
    A house on the window. A sun with a smile on it. # look: foggy-window
-
-> first_day_school

= first_day_school
# place: church_first_day_school
# shot: wide
# objective: Make a lantern
Miss Eloise: Good morning, Maya! Come sit, we're making lanterns today. # vo: prologue-quaker_eloise_0008
Rebecca: Maya! Sit with me! # vo: prologue-quaker_rebecca_0009
- (classroom)
+ [Look: The poster]
    "Let your light shine before others." A lamp under a basket. # look: light-poster
    -> classroom
+ [Look: Rebecca]
    Rebecca. Purple hairband. She can whistle. # look: rebecca
    -> classroom
* [Use: The glue and the yellow paper]
    Two yellow lanterns. # look: lanterns
-
Rebecca: Oh! Hi, Jamie! # vo: prologue-quaker_rebecca_0010
# objective: Finish your lantern
* [Go: Follow Rebecca]
    ~ record_choice("prologue.s1.rebecca", "followed", "When Rebecca went to sit with the boys, you followed her.")
    Rebecca: You can watch, Maya. # vo: prologue-quaker_rebecca_0011
* [Use: Your lantern]
    ~ record_choice("prologue.s1.rebecca", "stayed", "When Rebecca went to sit with the boys, you stayed where you were.")
    Miss Eloise: That's lovely, Maya. Very neat. # vo: prologue-quaker_eloise_0012
* (gave) [Talk: Rebecca (give her your lantern)]
    ~ record_choice("prologue.s1.rebecca", "gave", "When Rebecca went to sit with the boys, you gave her your lantern.")
    Rebecca: Thanks! # vo: prologue-quaker_rebecca_0013
-
Miss Eloise: Time for worship, everybody. Lanterns stay here. You can take them home after. # vo: prologue-quaker_eloise_0014
-> worship

= worship
# place: church_sanctuary
# shot: wide
# objective: Sit in the front pew
- (pews)
+ [Look: The Carters]
    Mr. Carter, Thomas and Reneé. Reneé's crutches lean on the end of the pew. His arm is along the pew behind them. # look: the-carters
    -> pews
+ [Look: The hymnbook]
    "This Little Light of Mine." # look: hymnbook
    -> pews
* [Go: The front pew]
-
Sarah: Each of us carries a light. Even when we can't see it. Even when the room is dark... # vo: prologue-quaker_sarah_0015
-> floaty

= floaty
# shot: inner
# objective: Be very quiet
The quiet part.
~ set_experience("Depersonalization", 0.35)
~ nudge(0.15)
* [Look: Your hands]
    Your hands. Like a doll's hands. Far away. # look: hands
    ~ set_experience("Depersonalization", 0.5)
-
* [Use: The wood of the pew]
    ~ record_choice("prologue.s1.floaty", "wood", "In the quiet part of worship, you went floaty, and held onto the wood of the pew.")
    Wood grain, like a river around a rock. # look: pew-grain
    ~ set_experience("Depersonalization", 0.15)
* [Use: The strap on your shoe]
    ~ record_choice("prologue.s1.floaty", "buckle", "In the quiet part of worship, you went floaty, and held onto the strap on your shoe.")
    Scratchy side. Smooth side. # look: shoe-strap
    ~ set_experience("Depersonalization", 0.15)
* [Look: The light on the floor]
    ~ record_choice("prologue.s1.floaty", "light", "In the quiet part of worship, you went floaty, and watched the light on the floor.")
    A square of light. Dust, falling. # look: light-square
    ~ set_experience("Depersonalization", 0.15)
* [Float]
    ~ record_choice("prologue.s1.floaty", "floated", "In the quiet part of worship, you went floaty, and let yourself float.")
    ~ set_experience("Depersonalization", 0.55)
    A little girl in a blue dress, very still. Very good. # look: from-above
-
~ nudge(-0.1)
~ set_experience("Depersonalization", 0.1)
-> forgotten

= forgotten
# place: church_fellowship_hall
# shot: wide
# objective: Find Mommy
Mrs. Hines: Pastor Sarah, have you got a minute? It's about the roof fund. # vo: prologue-quaker_hines_0016
Mr. Jones: And the board needs to talk about the pledge numbers before next week. # vo: prologue-quaker_jones_0017
Sarah: Of course. Let's go to my office. # vo: prologue-quaker_sarah_0018
Sarah: Wait right here, sweetheart. Five minutes. Be good. # vo: prologue-quaker_sarah_0019
# objective: Wait right here
- (hall)
+ {hall < 4} [Look: The coffee pot]
    The big silver coffee pot. Washed and put away. # look: coffee-pot
    -> hall
+ {hall < 4} [Look: The door]
    The Carters, leaving. Mr. Carter does up Reneé's scarf and holds the door for her crutches. # look: hall-door
    -> hall
+ {hall < 4} [Look: The window]
    Window squares. Eleven, twelve... # look: window-squares
    ~ set_experience("TimeLoss", 0.2)
    The light is higher on the wall now. # look: light-moved
    -> hall
* [Use: Mommy's office door]
    ~ record_choice("prologue.s1.waiting", "knocked", "When you were left waiting, you went to Mommy's office door. You could hear talking. You didn't knock.")
    Mommy's door. Closed. Numbers, and Mrs. Hines laughing. # look: office-door
* [Use: Your lantern]
    ~ record_choice("prologue.s1.waiting", "lantern", "When you were left waiting, you fetched your lantern to keep you company.")
    {first_day_school.gave: Rebecca's lantern. She left it behind. | Your yellow lantern.} # look: lantern
* [Wait right here]
    ~ record_choice("prologue.s1.waiting", "stayed", "When you were left waiting, you stayed exactly where Mommy said. You were very good.")
    ~ nudge(0.1)
    One light left on. # look: one-light
-
-> mira

= mira
# place: church_fellowship_hall
# shot: close
# objective:
Mira: Well, hello. Are you all on your own in here? # vo: prologue-quaker_mira_0020
Maya: I'm waiting. Mommy said five minutes. # vo: prologue-quaker_maya-05_0021
Mira: Did she. Hm. # vo: prologue-quaker_mira_0022
Mira: I'm Mira. I'll wait with you, if that's all right. I'm good at waiting too. # vo: prologue-quaker_mira_0023
Mira: Would you like these? I always take too many. # vo: prologue-quaker_mira_0024
* [Talk: Mira (say you're really hungry)]
    ~ record_choice("prologue.s1.mira", "hungry", "You told Mira you were really hungry. She gave you all her crackers.")
    Maya: I'm really, really hungry. # vo: prologue-quaker_maya-05_0025
    Mira: Have you had any lunch, dear? # vo: prologue-quaker_mira_0026
    Mira: Well. We'll fix that. # vo: prologue-quaker_mira_0027
* [Talk: Mira (take one, say you're fine)]
    ~ record_choice("prologue.s1.mira", "fine", "Mira offered you crackers. You took one, and said you were fine.")
    Maya: Just one. I'm fine. # vo: prologue-quaker_maya-05_0028
    Mira: Of course. Take one. And then, if it turns out you're a two-cracker sort of person, take another. # vo: prologue-quaker_mira_0029
* [Use: Mira's arm (lean on her)]
    ~ record_choice("prologue.s1.mira", "leaned", "You didn't say anything to Mira. You leaned on her arm, and she let you.")
    A moss-green cardigan. It smells like soap. # look: mira-cardigan
-
Maya: Mira? Sometimes I go floaty. # vo: prologue-quaker_maya-05_0030
Mira: Floaty? # vo: prologue-quaker_mira_0031
Maya: Like a balloon. In the quiet part. # vo: prologue-quaker_maya-05_0032
Mira: When I feel far away, do you know what I do? I put my feet flat on the floor, and I push, just a little. And I say hello to the floor. # vo: prologue-quaker_mira_0033
Mira: Shall we try? Shoes and all. # vo: prologue-quaker_mira_0034
Feet on the floor. # exercise: feet-on-floor
~ learn_skill("feet-on-floor")
~ set_experience("Depersonalization", 0.0)
~ nudge(-0.15)
Maya: Hello, floor. # vo: prologue-quaker_maya-05_0035
Mira: Hello, floor. There. Isn't it nice that it holds us up, without being asked. # vo: prologue-quaker_mira_0036
Your shoes. Mira has put them on the right feet. # look: shoes-right
-> sarah_returns

= sarah_returns
Sarah: Oh! Mira. You're a lifesaver, thank you so much. We got so caught up. # vo: prologue-quaker_sarah_0037
Mira: She's been waiting a long while, Sarah. # vo: prologue-quaker_mira_0038
Sarah: I know, I know. It's been one of those days. Maya, coat. Say thank you to Mira. # vo: prologue-quaker_sarah_0039
Maya: Thank you, Mira. # vo: prologue-quaker_maya-05_0040
Mira: Thank you for keeping me company, Maya. # vo: prologue-quaker_mira_0041
* [Look: Mira, at the door]
    Mira, holding the back of the chair, watching you go. # look: mira-watching
-
-> home

= home
# place: car
# shot: over-shoulder
# objective: Go home
* [Use: Feet on the floor]
    Hello, floor. # look: car-floor
-
~ journal("Note", "Sunday. Mira. Hello, floor.")
-> END
