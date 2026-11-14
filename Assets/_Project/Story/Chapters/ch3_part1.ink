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
//  Owner: Maya and Izzy share time (co-consciousness) through this chapter. Heather comes out at the Buddhist temple in Nepal,
//  appears more and more, and is unlocked at the end. The trip is impulsive: straight from the diagnosis to O'Hare. No temple in
//  Chicago (owner: cut): Maya has romanticized ideas about finding herself in Nepal, and has to cope with the reality.
// ─────────────────────────────────────────────────────────────────────────────

=== ch3_part1 ===
~ set_age(27)
~ set_date(2026, 1, 12)  // Claude's guess at the exact day (owner: change it freely)
// Owner: lost-time traces of the craft, rarely, never explained (a reading in the Book, in Izzy's hand).
~ bos_entry("izzy", "Readings", "Before", "The Fool. Of course it's the Fool. Pack light.")
# cw: time-loss
# cw: medical-setting
# cw: religious-content
~ explore_tradition("buddhism")
// Owner: Maya and Izzy share time in this chapter.
~ co_conscious("izzy", true)
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
Izzy (inside): Soup. She always brings soup. I love her. # vo: ch3-buddhism_izzy-inside_0034
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
-> train

= train
// Owner: a full day in every part. Claude's scenes.
# place: cta_train
# shot: over-shoulder
# objective: Get to work
- (car)
+ {car < 2} [Look: The window]
    January. The lake steaming in the cold. Tanya's text already: "Proud of you for opening the door." # look: l-january
    -> car
* [Use: The pole (hold on)]
    Your hand on the pole. Somebody else's hand. Your hand again. # look: l-pole-january
    ~ set_experience("Depersonalization", 0.2)
-
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
Izzy (inside): Breathe. I've got the dinosaur facts if you need them. Sixty-seven million. # vo: ch3-buddhism_izzy-inside_0035
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
// Owner: herbs and stones from different people (Claude's list, owner: "yes"). No new voiced lines.
On your way out, Angie holds out the bowl by the door. Worry stones. You take a hematite one: heavy, cool, smooth. For the train home. # look: angie-hematite
~ gain_stone("hematite", "angie")
-> evening

= evening
# place: chicago_apartment
# shot: close
# objective: Make dinner
- (k)
+ {k < 3} [Look: The windowsill]
    The hematite next to Mira's stone. Two stones from two people who noticed. # look: two-stones
    -> k
+ {k < 3} [Look: Angie's card]
    "Angie, LCPC. Next appointment: Thursday, 4 pm." On the fridge, under the SUE magnet. # look: angie-card
    -> k
* [Use: The stove (eggs)]
    Eggs on toast. Izzy wants hot sauce on them. You let her. # look: eggs-hot-sauce
-
* [Use: The bed]
    You write "Thursday 4pm" on your hand, just in case. You sleep with the light on, but you sleep. # look: hand-thursday
-
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
-> impulse

= impulse
// Owner: impulsive. Straight from the diagnosis to O'Hare, and away. Maya has romanticized ideas about finding herself in
// Nepal after the diagnosis, and will have to cope with the reality.
# place: chicago_apartment
# shot: close
# objective: Go home
~ set_experience("CoConsciousness", 0.6)
Izzy (inside): Go. Just go. When have we ever just gone anywhere? # vo: ch3-buddhism_izzy-inside_0036
- (night)
+ {night < 4} [Look: The laptop]
    One tab. A photo: prayer flags over white mountains, a monk smiling at the camera. "Kathmandu. One way. Tonight, 11:40 pm." # look: one-way-ticket
    -> night
+ {night < 4} [Look: The postcard on the fridge]
    The Himalayas, bought years ago at the museum gift shop. You've never been anywhere. # look: himalaya-postcard
    -> night
+ {night < 4} [Look: The suitcase]
    Under the bed. Dusty. It has never been on a plane. # look: dusty-suitcase
    -> night
* {night >= 2} [Use: Your laptop (buy the ticket)]
    ~ record_choice("ch3.p1.ticket", "bought", "The night Angie gave it a name, you bought a one-way ticket to Kathmandu.")
    Click. A confirmation number. Your heart, very loud. # look: ticket-bought
-
Maya: I'm going to find myself. That's what people do. They go to the mountains and they come back... whole. # vo: ch3-buddhism_maya_0033
Izzy (inside): Or we come back with a really good tan. Either way! # vo: ch3-buddhism_izzy-inside_0037
-> ohare

= ohare
# place: ohare_airport
# shot: wide
# objective: Find your gate
~ text_to("angie", "This is going to sound crazy. I'm at O'Hare. I'm going to Nepal. Tonight. Please don't be mad.")
~ text_to("christie", "I need someone to cover my tours. For a while. A long while. I'm so sorry.")
~ text_from("christie", "WHAT. Okay. Okay! Go. I've got the dinosaurs. Bring me a magnet.")
~ text_from("tanya", "NEPAL?? Maya. Okay. Text me every single day. Every day. I mean it.")
- (gate)
+ {gate < 4} [Look: The departures board]
    "DOHA, then KATHMANDU. ON TIME." Real letters. A real place. # look: departures-board
    -> gate
+ {gate < 4} [Look: The window]
    Planes in the dark, lit up like little cities. Your reflection over them, holding a backpack she packed in ten minutes. # look: ohare-window
    -> gate
+ {gate < 4} [Look: The people]
    A family asleep on each other. A man on the phone, crying quietly. Nobody looking at you. # look: ohare-people
    -> gate
* {gate >= 1} [Use: Your phone]
    Your phone. # device: phone
-
~ email_from("angie", "Okay. Here's the plan.", "Maya, I'm not mad. I'd have liked a plan. So here's one, from me, for you: short sits, eyes open is fine, walk when you need to. Your grounding stone in your pocket. Text Tanya every day. We'll do our Thursdays on video. You can come home early. That isn't failing. Go gently. A.")
* [Use: Your laptop (read Angie's email)]
    Your laptop. Angie's email, already. # device: laptop
-
~ journal("Note", "Angie's plan: short sits, eyes open, walk when I need to. Text Tanya every day. Coming home early isn't failing.")
Izzy (inside): Window seat. Obviously. # vo: ch3-buddhism_izzy-inside_0038
-> ch3_part2  // the next part of the same chapter (one run)
