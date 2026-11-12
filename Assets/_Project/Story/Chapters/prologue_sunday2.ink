// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Prologue "This Little Light", Sunday 2.
// vo-speakers: Maya=maya-08
// ─────────────────────────────────────────────────────────────────────────────
//  Sunday 2. Iowa City, 2006. Maya is 8.
//  Owner's beats: small signs add up over the morning (Maya is hungry and eats all the
//  fellowship-hour snacks; she mentions being home alone in the evenings while Sarah is at
//  meetings; she goes "floaty" and doesn't answer her name). Mira speaks to Sarah. Sarah thanks
//  her warmly, calls Maya "very independent", and is called away by Mrs. Hines.
//  MAYA NEVER KNOWS: the conversation itself is not shown here. Reneé (on crutches, resting in
//  the hallway) overhears it; adult Reneé tells Maya in chapter 4 or 5.
//  Explore-first: the signs are things the player finds (the note, the snack table), not narration.
//
//  My guesses, for the owner to correct:
//   - Autumn (October). Mira has been sitting with Maya most Sundays since Sunday 1.
//   - Miss Susan (school) sent home a note about a parent-teacher conference that Sarah never read.
//   - Mira teaches "five things you can see" as a game (the 5-4-3-2-1 skill).
// ─────────────────────────────────────────────────────────────────────────────

=== prologue_sunday2 ===
~ set_age(8)
~ set_date(2006, 10, 15)  // Claude's guess at the exact day (owner: change it freely)
# cw: childhood-reference
# cw: neglect
# cw: child-alone
# cw: depersonalization
-> wake

= wake
// Owner: more of each part of Maya's day. Claude's scenes. The signs (Sunday 2's beats) start at home.
# place: home_kitchen
# shot: wide
# objective: Make breakfast
- (kitchen)
+ {kitchen < 3} [Look: The fridge]
    Milk that smells wrong. Mustard. Half a lemon. Mommy's sticky notes for the church. # look: fridge-empty
    -> kitchen
+ {kitchen < 3} [Look: The sink]
    Last night's mug. Mommy got in after you were asleep. # look: sink-mug
    -> kitchen
+ {kitchen < 3} [Look: The calendar]
    Every square full in Mommy's writing. Board. Choir. Outreach. Deacons. Nothing on the square for Miss Susan. # look: calendar-full
    -> kitchen
* [Use: The cereal box]
    ~ record_choice("prologue.s2.breakfast", "dry", "That Sunday you ate dry cereal for breakfast, because the milk had gone off.")
    Dry cereal, out of the box. The last of it, mostly crumbs. # look: dry-cereal
-
Sarah: Five minutes, Maya! Teeth! Shoes! # vo: prologue-quaker_sarah_0122
-> drive

= drive
# place: car
# shot: over-shoulder
# objective: Give Mommy Miss Susan's note
- (backseat)
+ [Look: The back seat]
    Coffee cups. Bulletins. Church papers. # look: back-seat
    -> backseat
+ [Look: Your backpack]
    A note from Miss Susan: "Please sign and return. Parent-teacher conference." Nine days old. # look: susan-note
    -> backseat
* [Talk: Mommy (show her the note)]
    ~ record_choice("prologue.s2.note", "showed", "You showed Mommy Miss Susan's note. She said she'd look at it later.")
    Maya: Mommy, Miss Susan says you have to sign this. It's for the conference. # vo: prologue-quaker_maya-08_0042
    Sarah: Put it on the dashboard, honey. I'll look at it later. # vo: prologue-quaker_sarah_0043
    The note, sliding off the dashboard, under the seat. # look: note-falls
* [Use: The note (leave it in your backpack)]
    ~ record_choice("prologue.s2.note", "kept", "You left Miss Susan's note in your backpack. It wasn't a good time.")
    The note. Soft at the corners. # look: note-kept
* [Use: Your pencil (sign it yourself)]
    ~ record_choice("prologue.s2.note", "signed", "You signed Miss Susan's note yourself, in your neatest grown-up writing.")
    "Sarah", in an eight-year-old's neatest writing. Folded small. # look: note-signed
-
-> first_day_school

= first_day_school
# place: church_first_day_school
# shot: wide
# objective: Find a seat
Rebecca: Hi, Maya! Do you like my new shoes? My mom got them at the mall. # vo: prologue-quaker_rebecca_0044
Maya: They're nice. # vo: prologue-quaker_maya-08_0045
Rebecca: You can sit with us if you want. There's a chair. # vo: prologue-quaker_rebecca_0046
- (room)
+ [Look: Rebecca's table]
    Rebecca, Jamie, Justin and Will. One chair in the corner. # look: rebecca-table
    -> room
+ [Look: The poster]
    The lamp under the basket. Still there. # look: light-poster
    -> room
* [Go: The corner chair]
    ~ record_choice("prologue.s2.rebecca", "corner", "You sat in the corner chair at Rebecca's table.")
    A video game you don't have. Laughing a little bit late. # look: corner-chair
* [Talk: Miss Eloise (help with the crayons)]
    ~ record_choice("prologue.s2.rebecca", "helped", "You helped Miss Eloise set out the crayons instead.")
    Miss Eloise: You're such a help, Maya. A real little minister's daughter. # vo: prologue-quaker_eloise_0047
-
-> fellowship

= fellowship
# place: church_fellowship_hall
# shot: wide
# objective: Get a snack
- (snacks)
+ [Look: The snack table]
    Cookies. Cheese on toothpicks. Grapes. # look: snack-table
    -> snacks
* [Use: The snack table (keep eating)]
    ~ record_choice("prologue.s2.snacks", "ate", "At fellowship hour, you ate as much as you could, while nobody was looking.")
    A napkin full, in your pocket, for later. # look: napkin-pocket
* [Use: The snack table (stop, people might see)]
    ~ record_choice("prologue.s2.snacks", "stopped", "At fellowship hour, you stopped eating in case people noticed.")
    Mrs. Hines, right there. The napkin stays in your pocket. # look: napkin-hidden
-
Mira: Good morning, Maya. I saved you a chair. # vo: prologue-quaker_mira_0048
# objective: Sit with Mira
* [Go: Mira's chair]
* [Talk: Mira (say you're fine where you are)]
    Maya: I'm okay here. # vo: prologue-quaker_maya-08_0049
    Mira: Of course. The chair will wait. # vo: prologue-quaker_mira_0050
-
Mira: What did you have for dinner last night? # vo: prologue-quaker_mira_0051
Maya: Crackers. And peanut butter. Mommy had the board meeting. # vo: prologue-quaker_maya-08_0052
Mira: On your own? # vo: prologue-quaker_mira_0053
Maya: I'm allowed. I lock the door. I know the number for the church. # vo: prologue-quaker_maya-08_0054
Mira: How many evenings is Mommy at meetings, would you say? # vo: prologue-quaker_mira_0055
Maya: Um. Monday. And Wednesday is choir. And Thursday is the board. And sometimes Friday. # vo: prologue-quaker_maya-08_0056
Mira: That's a lot of crackers. # vo: prologue-quaker_mira_0057
Maya: I like crackers. # vo: prologue-quaker_maya-08_0058
-> floaty

= floaty
# shot: inner
# objective:
~ set_experience("Depersonalization", 0.45)
~ nudge(0.2)
Mira: Maya? # vo: prologue-quaker_mira_0059
~ set_experience("Depersonalization", 0.6)
Mira: Maya, dear? # vo: prologue-quaker_mira_0060
* [Talk: Mira (try to answer)]
    ~ record_choice("prologue.s2.floaty", "tried", "When you went floaty and Mira called your name, you tried to answer. It took a long time.")
    Your mouth. Slow, like in a dream. # look: slow-mouth
* [Float (stay up here, it's quieter)]
    ~ record_choice("prologue.s2.floaty", "stayed", "When you went floaty and Mira called your name, you stayed up where it was quiet.")
    ~ set_experience("Depersonalization", 0.7)
    Your knees. The napkin. Mira's hand. Very far down. # look: far-down
-
Mira: Maya. Feet on the floor, love. Hello, floor. # vo: prologue-quaker_mira_0061
~ set_experience("Depersonalization", 0.3)
~ nudge(-0.1)
Maya: ...Hello, floor. # vo: prologue-quaker_maya-08_0062
Mira: There you are. # vo: prologue-quaker_mira_0063
Mira: Let's play a game. Tell me five things you can see. Anything at all. # vo: prologue-quaker_mira_0064
Five things you can see. # exercise: five-senses
~ learn_skill("five-senses")
~ set_experience("Depersonalization", 0.0)
~ nudge(-0.15)
Maya: The coffee pot. Your cane. A grape on the floor. Mrs. Hines's earrings. The window. # vo: prologue-quaker_maya-08_0065
Mira: Five. You're very good at that. # vo: prologue-quaker_mira_0066
Maya: Did I go away? # vo: prologue-quaker_maya-08_0067
Mira: Only a little way. And you came back. That's the important part. # vo: prologue-quaker_mira_0068
-> mira_goes

= mira_goes
# objective: Wait here and finish your grapes
Mira: Maya, will you stay right here and finish your grapes? I need to have a word with your mother. # vo: prologue-quaker_mira_0069
Maya: Am I in trouble? # vo: prologue-quaker_maya-08_0070
Mira: No, dear. Not you. Never you. # vo: prologue-quaker_mira_0071
- (waiting)
+ {waiting < 3} [Look: The hallway]
    Mira with her cane, going to Mommy's office. # look: hallway
    -> waiting
+ {waiting < 3} [Look: The bench by the drinking fountain]
    Reneé Carter, resting, her crutches across her knees. Looking at the floor, very politely. # look: renee-bench
    -> waiting
* [Use: Your grapes]
    Grapes. One at a time, slowly. # look: grapes
    ~ set_experience("TimeLoss", 0.15)
-
-> mira_returns

= mira_returns
# objective:
Mrs. Hines: Pastor Sarah, the hymnal committee is waiting. # vo: prologue-quaker_hines_0072
Sarah: Coming, coming. # vo: prologue-quaker_sarah_0073
Sarah: Thank you, Mira. Really. You're so good to her. She's very independent, our Maya. # vo: prologue-quaker_sarah_0074
* [Look: Mira]
    Mira, walking slower than before. Picking a grape up off the floor. # look: mira-slower
-
* [Talk: Mira (ask what she said to Mommy)]
    ~ record_choice("prologue.s2.ask", "asked", "You asked Mira what she said to Mommy. She said grown-ups were just talking.")
    Maya: What did you say to Mommy? # vo: prologue-quaker_maya-08_0075
    Mira: Oh. Grown-up things. Just talking. # vo: prologue-quaker_mira_0076
* [Use: Your grapes (don't ask)]
    ~ record_choice("prologue.s2.ask", "didnt", "You didn't ask Mira what she said to Mommy.")
    The last grape. # look: last-grape
-
Mira: Maya. If you're ever hungry, or ever on your own and frightened, you can call me. Any time at all. Even late. # vo: prologue-quaker_mira_0077
Mira: Will you remember? # vo: prologue-quaker_mira_0078
Maya: I'll remember. # vo: prologue-quaker_maya-08_0079
* [Look: The napkin]
    A napkin. Mira's number, in shaky blue pen. # look: mira-number
-
~ journal("Note", "Mira's number, on a napkin. In my pocket with the cheese.")
-> home

= home
# place: car
# shot: over-shoulder
# objective: Go home
* [Use: Your pocket]
    The napkin with the number. Kept, not called. Kept for years. # look: napkin-kept
-
-> evening

= evening
# place: home_kitchen
# shot: wide
# objective: Lock the door
Sarah: Outreach committee. Lock the door behind me. I'll be back by nine. You're my big girl. # vo: prologue-quaker_sarah_0123
- (alone)
+ {alone < 3} [Look: The house]
    Quiet. The fridge hums. The clock in the hall. # look: house-quiet
    -> alone
+ {alone < 3} [Use: The lights]
    Every light in the house, on. The kitchen, the hall, the stairs, the bathroom. # look: all-lights
    -> alone
* [Use: The front door (lock it)]
    Click. You check it twice. # look: door-locked
-
* [Use: The cupboard (dinner)]
    ~ record_choice("prologue.s2.dinner", "crackers", "That Sunday night you had crackers and the cheese from your pocket for dinner, on your own.")
    Crackers, and the cheese from fellowship hour, a bit warm from your pocket. # look: crackers-dinner
-
* [Use: The napkin (on the table)]
    Mira's number. You could call. You don't. It's only nine o'clock. Then it's ten. # look: napkin-table
    ~ set_experience("TimeLoss", 0.15)
-
Car lights on the ceiling, much later. You're in bed. You don't remember going up. # look: car-lights-ceiling
-> prologue_sunday3  // the next part of the same chapter (one run)
