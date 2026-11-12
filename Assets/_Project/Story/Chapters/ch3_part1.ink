// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 3 (Buddhism), Part 1.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. Someone to talk to. Chicago, January to March 2026. Maya is 27.
//  Owner's beats: this is where Maya first gets real help. Tanya is the one who gets through to her. The therapist is
//  Angie (35, she/her). The diagnosis is only ever called "a dissociative disorder", never a real label. After the
//  diagnosis, Maya travels to a Buddhist country (owner: option 3). Christie, her friend from the museum, is a friendly face.
//
//  My guesses, for the owner to correct:
//   - Tanya drives up from Champaign-Urbana: one of their rare visits now. Tanya doesn't leave until Maya calls Angie.
//   - A tour where Maya loses her place mid-sentence, and Christie takes over without a fuss.
//   - Angie's office: the first session, and a few weeks later, the words "a dissociative disorder".
//   - Before she goes: a Sunday service at a Buddhist temple in Chicago (Jodo Shinshu: chanting together, "come as you
//     are"), where she first hears about temple stays. Where she travels: the owner chooses (Part 2).
//   - Starting alter and unlock for this chapter: still to set with the owner (Maya for now; nobody unlocked).
// ─────────────────────────────────────────────────────────────────────────────

=== ch3_part1 ===
~ set_age(27)
# cw: time-loss
# cw: medical-setting
# cw: religious-content
~ explore_tradition("buddhism")
~ text_from("tanya", "I'm coming up Saturday. Don't argue. I'm bringing soup.")
-> door

= door
# place: chicago_apartment
# shot: over-shoulder
# objective: Let Tanya in
~ set_experience("Depersonalization", 0.35)
- (mess)
+ {mess < 4} [Look: The mail]
    A drift of it by the door. Three envelopes from the same doctor's office, unopened. # look: unopened-mail
    -> mess
+ {mess < 4} [Look: The fridge]
    Sticky notes. "Buy milk." "CALL TANYA BACK." "you're doing ok." Three different handwritings. All of them yours. # look: fridge-notes
    -> mess
+ {mess < 4} [Look: The windowsill]
    A ring of old wax in the corner, from a candle you don't remember. # look: sill-wax-winter
    -> mess
* {mess >= 1} [Use: The door]
-
Tanya: There you are. # vo: ch3-buddhism_tanya_0001
Tanya: Sit. Soup first. Then we talk. Then you call someone, and I don't leave until you do. # vo: ch3-buddhism_tanya_0002
* [Talk: Tanya (say you're fine)]
    ~ record_choice("ch3.p1.tanya", "fine", "When Tanya came up to Chicago, you told her you were fine. She didn't believe you, kindly.")
    Maya: I'm fine. Really. Just tired. # vo: ch3-buddhism_maya_0003
    Tanya: You've told me that every week for a year. I keep every text, Maya. Some of them don't sound like you. # vo: ch3-buddhism_tanya_0004
* [Talk: Tanya (tell her the truth)]
    ~ record_choice("ch3.p1.tanya", "truth", "When Tanya came up to Chicago, you told her the truth: you were losing whole days.")
    Maya: I'm losing whole days. People tell me things I did. I don't remember any of it. # vo: ch3-buddhism_maya_0005
    Tanya: I know. I've known for a long time. I just didn't know how to say it so you'd hear it. # vo: ch3-buddhism_tanya_0006
-
Tanya: I found someone. A therapist who knows about this. Not about being "a bit stressed". About this. Her name is Angie. # vo: ch3-buddhism_tanya_0007
~ text_from("tanya", "Angie. Tuesdays and Thursdays, evenings. Here's her number. I'm sitting right here.")
* [Use: Your phone (call Angie)]
    ~ record_choice("ch3.p1.call", "called", "With Tanya sitting beside you, you called Angie.")
    Your phone. Ringing. Tanya's hand on your back, steady. # look: phone-call-angie
-
~ journal("Note", "Angie. Thursday, 6 pm. Tanya stayed until I called.")
~ nudge(-0.2)
-> tour

= tour
# place: field_museum_hall
# shot: wide
# objective: Give the eleven o'clock tour
- (hall)
+ {hall < 3} [Look: Máximo]
    The titanosaur. You've told its story a hundred times. You start, and then... # look: maximo-winter
    -> hall
+ {hall < 3} [Look: The tour group]
    Twenty adults on a members' tour, waiting. Somebody coughs. # look: members-tour
    -> hall
* {hall >= 1} [Wait (start the tour)]
-
~ set_experience("Derealization", 0.5)
~ set_experience("TimeLoss", 0.3)
# cue: stared-at
Christie: ...and SUE, upstairs, is sixty-seven million years old, which Maya taught me, didn't you, Maya? Let's go see her. # vo: ch3-buddhism_christie_0008
* [Talk: Christie]
-
Christie: Hey. You stopped. In the middle of a word. I've got the rest of this one. Go sit in the office. Snack drawer. # vo: ch3-buddhism_christie_0009
* [Talk: Christie (thank her)]
    ~ record_choice("ch3.p1.christie", "thanks", "When you stopped in the middle of a tour, Christie took over, and you thanked her.")
    Maya: Thank you. I'm sorry. # vo: ch3-buddhism_maya_0010
    Christie: Nope. No sorry. That's what the snack drawer's for. # vo: ch3-buddhism_christie_0011
* [Talk: Christie (tell her you're seeing someone)]
    ~ record_choice("ch3.p1.christie", "told", "When you stopped in the middle of a tour, you told Christie you were starting therapy. She was glad.")
    Maya: I'm starting therapy. Thursday. # vo: ch3-buddhism_maya_0012
    Christie: Good. Good! Tell me if you need Thursdays off. I'll cover. I owe you a dinosaur. # vo: ch3-buddhism_christie_0013
-
-> angie

= angie
# place: therapist_office
# shot: over-shoulder
# objective: Wait for Angie
- (waiting)
+ {waiting < 4} [Look: The plant]
    A big, slightly overwatered plant. Someone has named it: a little card says "Gerald." # look: gerald-plant
    -> waiting
+ {waiting < 4} [Look: The basket]
    Smooth stones, a stress ball, a tangle toy, a little bag of lavender. "Take one if you like." # look: fidget-basket
    -> waiting
+ {waiting < 4} [Look: The leaflets]
    "Grounding when you feel far away." "What is dissociation?" You don't take one. You read the title twice. # look: leaflets
    -> waiting
* {waiting >= 1} [Talk: Angie]
-
Angie: Maya? I'm Angie. Come on in. Sit wherever feels okay. The big chair's comfier, but it eats people. # vo: ch3-buddhism_angie_0014
Angie: Tanya said you've been losing time. Can you tell me what that's like? Start anywhere. There's no wrong place to start. # vo: ch3-buddhism_angie_0015
* [Talk: Angie (the lost days)]
    ~ record_choice("ch3.p1.first", "days", "In your first session, you told Angie about the lost days.")
    Maya: I lose days. Sometimes a whole afternoon, sometimes most of a year. I find things I bought. Texts I sent. # vo: ch3-buddhism_maya_0016
* [Talk: Angie (the floaty feeling)]
    ~ record_choice("ch3.p1.first", "floaty", "In your first session, you told Angie about going floaty, ever since you were little.")
    Maya: Since I was little, I go... floaty. Like I'm behind glass. Like I'm watching me from up in the corner. # vo: ch3-buddhism_maya_0017
* [Talk: Angie (the handwriting)]
    ~ record_choice("ch3.p1.first", "handwriting", "In your first session, you told Angie about the notes in handwriting that isn't yours.")
    Maya: There are notes on my fridge. In my writing, but not my writing. Does that make sense? # vo: ch3-buddhism_maya_0018
-
Angie: It makes sense. Thank you for telling me. Before we go further: can you feel your feet on the floor? Let's start there. # vo: ch3-buddhism_angie_0019
Feet on the floor. The chair holding you. # exercise: five-senses
~ learn_skill("five-senses")
-> weeks

= weeks
# place: therapist_office
# shot: close
# objective:
// A few weeks later. Owner: only ever "a dissociative disorder", never a real label.
* [Look: The calendar on the wall]
    March. Six Thursdays. # look: angie-calendar
-
Angie: I want to give you some words for what we've been talking about. Not a box. A map. # vo: ch3-buddhism_angie_0020
Angie: What you're describing, the lost time, the far-away feeling, the parts of you that write the notes: it sounds like a dissociative disorder. # vo: ch3-buddhism_angie_0021
~ set_experience("Derealization", 0.4)
* [Talk: Angie (ask what that means)]
    ~ record_choice("ch3.p1.words", "means", "When Angie said \"a dissociative disorder\", you asked what that meant.")
    Maya: What does that mean? For me? # vo: ch3-buddhism_maya_0022
    Angie: It means your mind found a way to survive something it couldn't get through all in one piece. It worked. It kept you going. And it means it can get easier, with time. # vo: ch3-buddhism_angie_0023
* [Talk: Angie (ask if it's your fault)]
    ~ record_choice("ch3.p1.words", "fault", "When Angie said \"a dissociative disorder\", you asked if it was your fault. She said no.")
    Maya: Did I do this? Is it my fault? # vo: ch3-buddhism_maya_0024
    Angie: No. Not even a little. You were a child who had to find a way through. You found one. # vo: ch3-buddhism_angie_0025
* [Wait (say nothing)]
    ~ record_choice("ch3.p1.words", "silent", "When Angie said \"a dissociative disorder\", you couldn't say anything. She waited with you.")
    Angie waits. She doesn't hurry you. The clock. Gerald, outside, being overwatered. # look: angie-waits
-
Angie: You don't have to do anything with it today. Just let it be true. # vo: ch3-buddhism_angie_0026
~ set_experience("Derealization", 0.0)
~ nudge(-0.25)
~ journal("Note", "\"A dissociative disorder.\" Not a box. A map. It isn't my fault.")
~ text_to("tanya", "She said it has a name. Sort of. I'm okay. I think I'm okay.")
~ text_from("tanya", "I'm so proud of you. You did the hardest part.")
-> temple

= temple
// A Buddhist temple in Chicago (fictional people): Jodo Shinshu (Shin, Pure Land), a Japanese American tradition. Sunday
// service in English; everyone chants the nembutsu together; the teaching that you're embraced as you are.
# place: buddhist_temple_chicago
# shot: wide
# objective: Find a seat at the Sunday service
- (hondo)
+ {hondo < 4} [Look: The altar]
    Gold, and quiet. A standing Buddha, small, one hand raised as if to say "it's all right". Incense, thin and straight. # look: shin-altar
    -> hondo
+ {hondo < 4} [Look: The service book]
    "Namu Amida Butsu." In English underneath: "I take refuge in the Buddha of Immeasurable Light." # look: service-book
    -> hondo
+ {hondo < 4} [Look: The people]
    Grandparents, toddlers, a man in a suit, two students in hoodies. Nobody asks why you're here. # look: shin-sangha
    -> hondo
* {hondo >= 2} [Wait (chant with them)]
-
Namu Amida Butsu. Everyone, together. Your voice lost in theirs, which is all right. # look: nembutsu
~ nudge(-0.2)
Reverend Mori: Shinran taught that we don't have to fix ourselves first. We're embraced just as we are, the broken parts too. Especially the broken parts. # vo: ch3-buddhism_reverend-mori_0027
-> after_service

= after_service
# place: buddhist_temple_chicago
# shot: over-shoulder
# objective: Stay for tea
* [Talk: Reverend Mori]
-
Reverend Mori: First time? Welcome. There's tea, and there's manju, and nobody will make you talk about Buddhism over either. # vo: ch3-buddhism_reverend-mori_0028
* [Talk: Reverend Mori (ask about going further)]
    ~ record_choice("ch3.p1.mori", "further", "After the service, you asked Reverend Mori how people go deeper. He told you about temple stays, and to go gently.")
    Maya: How do people... go further? With this? # vo: ch3-buddhism_maya_0029
    Reverend Mori: Some go to a temple far away for a while. A temple stay. If you go, go gently. Bring your doctor's advice in your pocket, next to your passport. # vo: ch3-buddhism_reverend-mori_0030
* [Talk: Reverend Mori (just say thank you)]
    ~ record_choice("ch3.p1.mori", "thanks", "After the service, you just thanked Reverend Mori. He said the door's open on Sundays.")
    Maya: Thank you. For having me. # vo: ch3-buddhism_maya_0031
    Reverend Mori: The door's open every Sunday. Some people come for years. Some go far away and come back. Both are fine. # vo: ch3-buddhism_reverend-mori_0032
-
-> plan

= plan
# place: chicago_apartment
# shot: close
# objective: Talk to Angie about going away
~ email_from("angie", "Before you go", "Maya, I think a trip could be good for you, with a plan. Short sits, eyes open is fine, walk when you need to. Your grounding stone in your pocket. Text Tanya every day. Call me if you need to: we'll do a session on video. You can come back early. That isn't failing. A.")
- (evening)
+ {evening < 3} [Look: Mira's stone]
    On the dresser, by your passport. # look: stone-passport
    -> evening
+ {evening < 3} [Look: The suitcase]
    Open on the bed. Empty. It looks very brave. # look: empty-suitcase
    -> evening
* {evening >= 1} [Use: Your laptop]
    Your laptop. Angie's email, and a dozen tabs about temple stays. # device: laptop
-
~ journal("Note", "Angie's plan: short sits, eyes open, walk when I need to. Text Tanya every day. Coming back early isn't failing.")
// Where she travels: the owner chooses. Part 2 isn't written yet.
To be continued.
-> END
