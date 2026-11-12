// DRAFT: not canon until the owner approves it. Chapter 1 (Christianity), Part 4.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 4. Alone. Champaign-Urbana, March to June 2020. Maya is 21, then 22.
//  Owner's beats: COVID: Maya alone in her dorm; a Zoom graduation where no one speaks to her, quiet and lonely. Then a
//  prayer service at Gracie's ministry: they sing a hymn Maya loved as a child; she can't leave without a major
//  disruption, is too scared to, and starts dissociating. The screen fades to black, and Jacob's lines are spoken as a
//  light appears. That's how Chapter 1 ends (Jacob is its unlock). Then Chapter 1.5 is played, as Axel the first time.
//
//  My guesses, for the owner to correct:
//   - The hymn is "This Little Light of Mine" (the prologue's title), which Maya sang in First Day School.
//   - The prayer service is in June 2020, outdoors, masked and distanced, on folding chairs in rows; Maya is in the
//     middle of a row, and leaving means walking in front of everyone while they sing.
//   - Jacob (14, Catholic, watches the exits) speaks to Maya, inside, gently and practically.
// ─────────────────────────────────────────────────────────────────────────────

=== ch1_part4 ===
~ set_age(21)
# cw: pandemic
# cw: isolation
# cw: religious-content
# cw: depersonalization
# cw: involuntary-switch
-> lockdown

= lockdown
# place: college_dorm_maya
# shot: close
# objective:
~ email_from("tanya", "We're moving online", "The shop is closed for now. Thursdays will be on video, 7 pm, link below. Ground first, even on a screen. And Maya: text me every day. I mean it. Even just a dot. T.")
~ text_from("selene", "my mom is making me come home. im so sorry. call me every night ok")
- (dorm)
+ {dorm < 4} [Look: The window]
    The quad, empty. A single person with a dog, very far away. # look: empty-quad
    -> dorm
+ {dorm < 4} [Look: The desk]
    A laptop. A cold mug. A mask on the lamp. # look: dorm-desk
    -> dorm
+ {dorm < 4} [Use: Your phone] -> phone
* [Use: The laptop (Thursday class)]
    ~ record_choice("ch1.p4.lockdown", "class", "In lockdown, you went to Tanya's Thursday class on your laptop.")
    Six small squares. Tanya's ceiling fan. Everyone with their feet on their own floor. # look: zoom-class
* [Wait (stay in bed)]
    ~ record_choice("ch1.p4.lockdown", "bed", "In lockdown, there were days you didn't get out of bed.")
    The ceiling. The light moving across it. Morning, afternoon. # look: dorm-ceiling
    ~ set_experience("Depersonalization", 0.4)
-
-> graduation

= phone
Your phone. # device: phone
-> lockdown.dorm

= graduation
# objective: Graduate
# shot: close
* [Use: The laptop (the ceremony)]
    A grid of faces. Muted. Names scrolling. # look: zoom-grid
-
* [Look: Your name]
    "Maya ___." Two seconds on the screen. Gone. # look: name-on-screen
-
- (after)
+ {after < 3} [Look: The chat]
    "congrats everyone!!" "CLASS OF 2020" Nobody says your name. # look: zoom-chat
    -> after
+ {after < 3} [Look: The cap]
    A cap, still in its plastic, on the bed. # look: cap-plastic
    -> after
* [Use: Your phone (text Tanya)]
    ~ record_choice("ch1.p4.graduation", "texted", "After your Zoom graduation, you texted Tanya.")
    ~ text_to("tanya", "I graduated.")
    ~ text_from("tanya", "I know. I watched. I saw your name. I'm proud of you.")
    The phone buzzes almost at once. # look: tanya-reply
* [Wait (close the laptop)]
    ~ record_choice("ch1.p4.graduation", "closed", "After your Zoom graduation, you closed the laptop and sat in the quiet.")
    The laptop, closed. Quiet. # look: laptop-closed
-
-> service

= service
# place: campus_ministry_lawn
# shot: wide
# objective: Sit with Gracie
Gracie: You came! Six feet, I know, I'm hugging you from here. Imagine a hug. Okay, sit anywhere. Middle's nicest, there's shade. # vo: ch1-christianity_gracie_0029
- (lawn)
+ {lawn < 3} [Look: The rows of chairs]
    Folding chairs in rows, far apart. Masks. Everyone very still. # look: chairs-rows
    -> lawn
+ {lawn < 3} [Look: The way out]
    The path to the road, behind the last row. Everyone would turn. # look: way-out
    ~ set_experience("Depersonalization", 0.2)
    -> lawn
* [Use: A seat in the middle row]
-
Gracie: Let's sing. Something simple. You'll all know it. # vo: ch1-christianity_gracie_0030
# cue: no-exit
~ set_experience("Depersonalization", 0.6)
~ set_experience("Derealization", 0.5)
~ nudge(0.4)
"This little light of mine." # look: hymn-first-line
* [Look: The way out]
    ~ record_choice("ch1.p4.hymn", "exit", "When they sang the hymn from your childhood, you looked for a way out. Leaving would have meant everyone turning to look.")
    The path, behind the last row. Every face would turn. # look: way-out-turn
* [Use: Feet on the floor]
    ~ record_choice("ch1.p4.hymn", "roots", "When they sang the hymn from your childhood, you tried Tanya's roots. The grass was too far away.")
    Grass. Roots. Too far down. Too far away. # look: roots-far
* [Use: Mira's stone]
    ~ record_choice("ch1.p4.hymn", "stone", "When they sang the hymn from your childhood, you held Mira's stone.")
    The stone. Mira's hand, lifting at the door. # look: stone-mira-door
-
"I'm gonna let it shine." # look: hymn-second-line
~ set_experience("Depersonalization", 0.9)
~ set_experience("TimeLoss", 0.8)
-> light

= light
# shot: inner
# place: black
...
// Owner: the screen fades to black; Jacob's lines are spoken as a light appears.
# switch: jacob
Jacob: I've got you. Don't run. Nobody's looking. # vo: ch1-christianity_jacob_0031
Jacob: Breathe with the song. In on "little". Out on "light". # vo: ch1-christianity_jacob_0032
Jacob: There's a gate behind the chapel. When they sit down, we'll go. I know the way. # vo: ch1-christianity_jacob_0033
* [Wait (let him)]
    ~ record_choice("ch1.p4.jacob", "let", "When the light came, you let the voice take you to the gate.")
    A light. Small. Steady. # look: jacob-light
-
~ learn_skill("paced-breathing")
~ set_experience("TimeLoss", 0.0)
This little light. # chapter-end
-> END
