// IN THE GAME as a DRAFT (Claude, 2026-11-14; the owner's Doc edits come first). Chapter 10 (Unitarian Universalism), Part 2.
// vo-speakers: Maya=maya, Anabelle=anabelle, Serena=serena, Sarah=sarah, Heather (inside)=heather-inside
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. The longest night. Arcata, December 2032. Maya is 34.
//  Owner: SERENA first comes forward when SARAH PHONES (out of the blue, the first time in years). Maya can't speak; Serena takes
//  the call, polite and grown-up, and sets a boundary Maya never could. Serena is NOT unlocked until the end of the chapter.
//
//  My guesses, for the owner to correct:
//   - Why Sarah calls: she's retiring as minister of the Church of Friends in the spring, the meeting is planning a celebration,
//     and someone asked if her daughter would say a few words. She got the number from Rebecca's mother. She's warm and
//     cheerful and doesn't hear herself (never her side: no reasons, no explaining, just how it lands).
//   - Serena calls her "Sarah", not "Mom": Serena has never seen Sarah as her mother (the system's links).
//   - The day: the Sunday before the solstice. A winter solstice service in the morning; the Spiral Fern's Yule that night.
// ─────────────────────────────────────────────────────────────────────────────

=== ch10_part2 ===
~ set_age(34)
~ set_date(2032, 12, 19)  // the Sunday before the solstice (Claude's guess)
# cw: religious-content
# cw: childhood-reference
# cw: involuntary-switch
-> wake

= wake
# place: arcata_apartment
# shot: close
# objective: Get ready for the solstice service
- (room)
+ {room < 3} [Look: The window]
    Dark at eight in the morning, rain on the glass, the streetlight still on. # look: december-dark
    -> room
+ {room < 3} [Look: The altar]
    A Yule log from the Community Forest, three candle holes drilled by Izzy, badly. A sprig of redwood. # look: yule-log-home
    -> room
* [Use: The kettle]
    Tea, toast. A text from Anabelle: "Bring a candle tonight! Any candle! Birthday candles count!" # look: anabelle-text
-
-> service

= service
# place: uu_fellowship
# shot: wide
# objective: Sit through the solstice service
- (hall)
+ {hall < 4} [Look: The lights]
    Every light in the hall is off. Just the chalice, and one candle for each person, lit hand to hand down the rows. # look: solstice-candles-rows
    -> hall
+ {hall < 4} [Look: The minister]
    She reads a poem about the dark being a kind of rest, then a bit of Isaiah for the Christians, then says "and the light comes back either way." # look: solstice-sermon
    -> hall
+ {hall < 4} [Look: Anabelle]
    Two rows ahead, in a knitted hat with antlers on it, mouthing "hi" over their shoulder. # look: anabelle-antlers
    -> hall
* {hall >= 2} [Wait (until the end)]
-
-> call

= call
# place: uu_fellowship_garden
# shot: close
# objective: Answer your phone
Your phone buzzes as you step out into the rain. An Iowa number. You know it before you know it. # look: iowa-number
~ set_experience("Depersonalization", 0.4)
* [Use: Your phone (answer)]
-
Sarah (on the phone): Maya? Is that you, honey? It's Mom! I got your number from Rebecca's mother, isn't that funny? How are you? # vo: ch10-unitarian-universalism_sarah_0012
Maya: I... # vo: ch10-unitarian-universalism_maya_0013
Sarah (on the phone): I'm retiring! In May. Thirty-one years. The meeting's planning a whole celebration, and Mrs. Hines said, well, wouldn't it be lovely if your daughter said a few words? And I thought, why not? We could... # vo: ch10-unitarian-universalism_sarah_0014
~ set_experience("Derealization", 0.6)
The garden tilts. A fellowship hall in Iowa. The light in the windows. Your mouth won't open. # look: garden-tilts
-> serena

= serena
// Owner: Serena comes forward here, for the first time. A scripted switch (she's unlocked only at the end of the chapter).
# switch: serena
# place: uu_fellowship_garden
# shot: close
# objective: Finish the call
~ co_conscious("maya", true)
Serena: Sarah. This is... let's say this is someone who takes care of things. Maya can't talk just now. # vo: ch10-unitarian-universalism_serena_0015
Sarah (on the phone): Who? I'm sorry, I don't... # vo: ch10-unitarian-universalism_sarah_0016
Serena: That's all right. I'll be brief. We won't be speaking at your celebration. We hope it's a good one. Thirty-one years is a long time to give a place. # vo: ch10-unitarian-universalism_serena_0017
Serena: If you'd like to know Maya now, you can write to her. A letter. She'll read it when she's ready, and not before. Goodbye, Sarah. # vo: ch10-unitarian-universalism_serena_0018
* [Use: The phone (hang up)]
    ~ record_choice("ch10.p2.call", "boundary", "When Sarah called on the solstice Sunday, someone steady took the phone and told her: write, and Maya will read it when she's ready. Then hung up.")
-
Rain on the leaves. The phone, warm in a hand that holds it like it has all the time in the world. # look: phone-steady-hand
-> anabelle

= anabelle
# place: uu_fellowship_garden
# shot: over-shoulder
# objective:
- (rain)
+ {rain < 2} [Look: The plum tree]
    Bare, black, dripping. Someone has hung paper stars on it for tonight. # look: plum-stars
    -> rain
* [Talk: Anabelle]
-
Anabelle: Hey. You okay? You've been standing in the rain for ten minutes. I brought you an umbrella. And a lemon bar. # vo: ch10-unitarian-universalism_anabelle_0019
Serena: Thank you. That's kind. I'm Serena. I'm... part of Maya. The part that handles the grown-up phone calls. # vo: ch10-unitarian-universalism_serena_0020
Anabelle: Hi, Serena. Welcome. We've got a minister, a Buddhist, two witches and a man who thinks he's a Stoic. You're not the strangest thing here today. # vo: ch10-unitarian-universalism_anabelle_0021
Serena: I'll take the lemon bar. # vo: ch10-unitarian-universalism_serena_0022
~ record_choice("ch10.p2.anabelle", "met", "Anabelle met Serena in the rain on the solstice Sunday, and didn't blink. They gave her a lemon bar.")
-> yule

= yule
// Serena goes back; Maya, shaky, comes to the circle.
# switch: maya
~ co_conscious("serena", true)
# place: uu_fellowship_garden
# shot: wide
# objective: Light a candle for the longest night
- (flames)
+ {flames < 3} [Look: The circle]
    Twelve people under a tarp in the dark, each holding an unlit candle. Anabelle in the middle with one match. # look: yule-tarp-circle
    -> flames
+ {flames < 3} [Look: Your hands]
    Still shaking a little. In your pocket, a lemon bar wrapped in a napkin you don't remember being given. # look: lemon-bar-pocket
    -> flames
* {flames >= 1} [Talk: Anabelle]
-
Anabelle: Longest night of the year. We start in the dark, on purpose. Then one light. Then we pass it on. # vo: ch10-unitarian-universalism_anabelle_0023
Maya: Anabelle, earlier, in the garden... was I... # vo: ch10-unitarian-universalism_maya_0024
Anabelle: You were Serena. She was lovely. She ate my lemon bar. Whoever comes to circle, comes to circle. # vo: ch10-unitarian-universalism_anabelle_0025
* [Use: Your candle (take the light)]
    ~ record_choice("ch10.p2.yule", "light", "At the Spiral Fern's Yule, after the call, you took the light from Anabelle's candle and passed it on.")
    The flame comes to your wick from Anabelle's, and you turn and give it to the person on your left, and they give it on. # look: yule-light-passed
* [Wait (hold the dark a moment)]
    ~ record_choice("ch10.p2.yule", "dark", "At the Spiral Fern's Yule, after the call, you held the dark a moment longer before you took the light.")
    You stay in the dark a little longer, on purpose. Then you take the light. # look: yule-dark-then-light
-
Heather (inside): She's here. The steady one. We needed her today. # vo: ch10-unitarian-universalism_heather-inside_0026
~ nudge(-0.3)
-> night

= night
# place: arcata_apartment
# shot: close
# objective: Go home
- (room)
+ {room < 2} [Look: Your phone]
    Call log: "Mom, 4 min 12 s." You don't remember most of it. You remember the word "letter". # look: call-log-mom
    -> room
* [Use: The bed]
-
~ system_log("serena", "Sarah called. I told her to write, and that you'll read it when you're ready. You don't owe her a speech. I'm Serena. I'll be around for the grown-up things. -S")
~ system_log("maya", "Thank you, Serena. Nobody ever said no to her for me before. -M")
~ text_to("tanya", "My mom called. Someone in here answered for me. Her name's Serena. She said no. Nicely. I'm shaking but I think I'm okay.")
~ text_from("tanya", "Then welcome, Serena, and thank you. Light the Yule log, love. The sun comes back either way.")
~ journal("Note", "Solstice Sunday. Sarah called: her retirement, would I speak. Serena answered. She said write a letter, and goodbye. Anabelle gave her a lemon bar. The light came back.")
-> ch10_part3  // the next part of the same chapter (one run)
