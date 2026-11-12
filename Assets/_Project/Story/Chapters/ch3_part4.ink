// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 3 (Buddhism), Part 4.
// vo-speakers: Maya=maya, Pasang Lhamo=pasang
// ─────────────────────────────────────────────────────────────────────────────
//  Part 4. Gently. Boudha and Lumbini, June to July 2026. Maya is 27 (28 by the end? birthday unknown).
//  Owner's beats: it starts to help once the practice is adapted, with the host and with Angie over video: eyes open, short
//  sits, walking meditation, prayer wheels instead of long stillness, Mira's stone in her pocket. Heather appears more and
//  more. Lumbini, the Buddha's birthplace, before going home. Finishing the chapter unlocks Heather (only at the end).
//
//  My guesses, for the owner to correct:
//   - Angie, on video, gives Heather a place in the story without a label: "a part of you who takes care".
//   - Heather's notes start appearing in Maya's notebook, signed "H". (SystemLog, the phone app, comes at the end of Chapter 4.)
//   - Pasang says goodbye with a khata, the white scarf given at partings.
// ─────────────────────────────────────────────────────────────────────────────

=== ch3_part4 ===
~ set_age(27)
# cw: religious-content
# cw: time-loss
~ co_conscious("izzy", true)
-> morning_after

= morning_after
# place: boudha_guest_house
# shot: close
# objective: Talk to Angie
- (room)
+ {room < 4} [Look: The notebook]
    Your notebook, open on the blanket. In round, careful handwriting that isn't quite yours: "Water first. Then food. Then Angie. -H" # look: heather-note
    -> room
+ {room < 4} [Look: The window]
    The stupa, the eyes, the dogs. Pasang's daughter hanging out laundry, looking up at your window every few minutes. # look: window-watched
    -> room
+ {room < 4} [Look: The tea]
    A flask of sweet milky tea outside your door. A note: "DRINK. -PASANG" # look: tea-flask
    -> room
* {room >= 1} [Use: Your laptop (call Angie)]
    The laptop. Angie's face, pixelated, frozen, then moving. # device: laptop
-
Angie (on video): Pasang emailed me. I'm glad she did. How are you, right now, this minute? # vo: ch3-buddhism_angie_0067
* [Talk: Angie (tell her about the course)]
    ~ record_choice("ch3.p4.angie", "course", "On video, you told Angie about the ten days. She wasn't angry. She said you'd done the hard thing too hard.")
    Maya: I went so far away. For days. I think I'm still a bit away. # vo: ch3-buddhism_maya_0068
    Angie (on video): That makes sense. Long silence, eyes closed, a new diagnosis. That's a lot of deep water at once. You did the hard thing too hard. That's all. # vo: ch3-buddhism_angie_0069
* [Talk: Angie (ask about Heather)]
    ~ record_choice("ch3.p4.angie", "heather", "On video, you asked Angie about Heather. She said it sounded like a part of you who takes care.")
    Maya: Pasang says someone called Heather looked after me. I don't know a Heather. # vo: ch3-buddhism_maya_0070
    Angie (on video): It sounds like a part of you who takes care. Who came when you needed it. We can get to know her slowly, if she wants. # vo: ch3-buddhism_angie_0071
-
Angie (on video): New plan. Eyes open. Five minutes, not five hours. Walk instead of sitting, when you need to. And Mira's stone, always in your pocket. # vo: ch3-buddhism_angie_0072
~ learn_skill("grounding-object")
~ journal("Note", "New plan: eyes open. Five minutes. Walk when I need to. Mira's stone, always.")
~ co_conscious("heather", true)
Heather (inside): Water first. I mean it. # vo: ch3-buddhism_heather-inside_0073
-> kora_again

= kora_again
# place: boudha_stupa
# shot: wide
# objective: Walk the kora with Pasang, eyes open
Pasang Lhamo: Today I watch you properly. If you go far away, I pinch you. Gently. Mostly gently. # vo: ch3-buddhism_pasang_0074
- (walk)
+ {walk < 4} [Use: The prayer wheels (eyes open)]
    Brass under your palm, warm from the sun and from everyone before you. One, and the next, and the next. You're here for every one of them. # look: prayer-wheels-open
    ~ nudge(-0.15)
    -> walk
+ {walk < 4} [Look: The pigeons]
    A hundred pigeons lifting all at once, and settling again. Grey wings. Real. # look: pigeons-lift
    -> walk
+ {walk < 4} [Look: Your feet]
    Left, right, on old stone. Clockwise. Your feet, yours. # look: own-feet
    -> walk
* {walk >= 2} [Talk: Pasang Lhamo]
-
Pasang Lhamo: I'm sorry. I should have seen. Ten days, and I was talking about shoes. # vo: ch3-buddhism_pasang_0075
* [Talk: Pasang Lhamo (tell her it's okay)]
    ~ record_choice("ch3.p4.pasang", "okay", "When Pasang said sorry for not seeing, you told her it was okay. You hadn't shown anyone.")
    Maya: It's okay. I didn't show anyone. I'm good at not showing anyone. # vo: ch3-buddhism_maya_0076
    Pasang Lhamo: Not here. Here you show me. That's the rule of my house now. # vo: ch3-buddhism_pasang_0077
* [Talk: Pasang Lhamo (thank her for staying up)]
    ~ record_choice("ch3.p4.pasang", "thanks", "When Pasang said sorry for not seeing, you thanked her for sitting up with you all night.")
    Maya: You sat with me all night. Nobody's ever done that. # vo: ch3-buddhism_maya_0078
    Pasang Lhamo: Then it's late. Very late. But not too late. # vo: ch3-buddhism_pasang_0079
-
-> shrine

= shrine
# place: boudha_monastery
# shot: close
# objective: Sit for five minutes
- (hall)
+ {hall < 3} [Look: The shrine]
    A great golden Buddha, and smaller ones all around, and offerings: water bowls, oranges, a packet of biscuits someone left. # look: shrine-hall
    -> hall
+ {hall < 3} [Look: The young monks]
    Little monks at the back, chanting, one of them yawning, one of them poking another. A teacher's look, and they're quiet. # look: young-monks
    -> hall
* {hall >= 1} [Use: A cushion (sit, eyes open)]
    ~ record_choice("ch3.p4.sit", "open", "In the shrine hall, you sat for five minutes with your eyes open. You stayed.")
    Five minutes. Eyes open. A flame. The stone in your pocket. The chanting, like rain on a roof. You stay. You stay the whole five minutes. # look: five-minutes
-
~ set_experience("Depersonalization", 0.0)
~ set_experience("Derealization", 0.0)
~ nudge(-0.3)
Heather (inside): There. That's it. That's all it has to be. # vo: ch3-buddhism_heather-inside_0080
Izzy (inside): Five minutes! New record! # vo: ch3-buddhism_izzy-inside_0081
~ text_to("tanya", "Day 47. Five minutes, eyes open. I stayed for all of it.")
~ text_from("tanya", "That's the bravest five minutes I've ever heard of.")
-> lumbini

= lumbini
// Lumbini: the Buddha's birthplace (a UNESCO World Heritage site). The Sacred Garden: the Maya Devi Temple over the marker
// stone, the Ashoka pillar, the pond, prayer flags in the trees; the monastic zone, with monasteries built by many countries.
# place: lumbini_sacred_garden
# shot: wide
# objective: See where it began
- (garden)
+ {garden < 5} [Look: The Maya Devi Temple]
    A plain white building, and inside, under glass, a stone. The place where, they say, he was born. People kneel. You don't know what to do, so you stand very still. # look: maya-devi-temple
    -> garden
+ {garden < 5} [Look: The Ashoka pillar]
    A stone pillar, older than almost anything, put here by an emperor to say: here. It happened here. # look: ashoka-pillar
    -> garden
+ {garden < 5} [Look: The pond]
    Green water. A bodhi tree hung with so many prayer flags you can hardly see the leaves. # look: lumbini-pond
    -> garden
+ {garden < 5} [Look: The monasteries]
    Down the long canal: a Thai temple, white and gold; a Burmese one with a golden spire; a Chinese gate; a German one; a Japanese peace pagoda. Everyone came. # look: monastic-zone
    -> garden
* {garden >= 2} [Wait (stand under the tree)]
-
Maya: She was called Maya. His mother. I didn't know that. # vo: ch3-buddhism_maya_0082
Izzy (inside): Okay, that's a little spooky. # vo: ch3-buddhism_izzy-inside_0083
Heather (inside): It's just a name. It's a good one. # vo: ch3-buddhism_heather-inside_0084
~ journal("Note", "Lumbini. His mother was called Maya. Everyone came here, from everywhere, to say: it happened here.")
-> goodbye

= goodbye
# place: boudha_guest_house
# shot: close
# objective: Say goodbye to Pasang
- (pack)
+ {pack < 3} [Look: The suitcase]
    Full now. Prayer flags for Tanya, a magnet for Christie, a little brass prayer wheel for Angie's office. Something for Gerald. # look: full-suitcase
    -> pack
+ {pack < 3} [Look: The notebook]
    The last page, in that round, careful hand: "Go home gently. -H" # look: heather-note-last
    -> pack
* {pack >= 1} [Talk: Pasang Lhamo]
-
Pasang Lhamo: Come here. Bend your head. # vo: ch3-buddhism_pasang_0085
A white silk scarf, a khata, placed around your neck. # look: khata
Pasang Lhamo: You came to find yourself. You didn't. Good. Nobody finds themselves. You found some of them, I think. Bring them all back sometime. # vo: ch3-buddhism_pasang_0086
* [Talk: Pasang Lhamo (promise to come back)]
    ~ record_choice("ch3.p4.goodbye", "promise", "When you left Boudha, you promised Pasang you'd come back, all of you.")
    Maya: I'll come back. All of us. # vo: ch3-buddhism_maya_0087
* [Wait (hug her)]
    ~ record_choice("ch3.p4.goodbye", "hug", "When you left Boudha, you didn't say anything. You hugged Pasang for a long time.")
    She smells of butter lamps and laundry soap. You hold on for a long time. # look: pasang-hug
-
~ text_to("tanya", "Coming home. I didn't find myself. Pasang says that's good. I think she's right.")
~ text_from("tanya", "Come home. I'll bring soup.")
~ co_conscious("heather", true)
# shot: inner
# place: black
Kathmandu. July 2026. # chapter-end
-> END
