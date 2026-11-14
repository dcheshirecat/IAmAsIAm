// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 1.5 (Axel), Part 3.
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Holidays. Champaign-Urbana and Indianapolis, February to April 2019. The body is 20.
//  Owner's beats: Mark's Episcopal church is queer-friendly and warm. Axel goes on holidays, once they've dated a few months.
//  The priest is kind to him and invites him for holidays (they aren't in touch consistently). They go to protests as a
//  church. Izzy is triggered at one, because Maya went to protests with Sarah as a child. Easter with Mark's family, and the
//  queer-affirming church.
//
//  My guesses, for the owner to correct:
//   - The protest is a march in March 2019 that ends in a candlelit vigil, like the peace vigils Sarah took Maya to.
//   - Izzy is triggered by the candles and the singing, and fronts for a little while. Axel comes back holding a candle.
//   - At Easter, Mark's family's Methodist service is early. Mark and Axel also find an Easter evening service at a
//     queer-affirming church in the next town, which Mark has wanted to visit for years.
// ─────────────────────────────────────────────────────────────────────────────

=== ch1_5_part3 ===
~ set_age(20)
~ set_date(2019, 3, 6)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
# cw: childhood-reference
# cw: derealization
# cw: involuntary-switch
# cw: time-loss
-> run

= run
// Owner: more of each part of the day. Claude's scene: Axel's morning run (one of his own practices), music up loud.
# place: college_quad
# shot: over-shoulder
# objective: Go for a run
- (path)
+ {path < 2} [Look: The sky]
    Grey, cold, the sun not up yet. The quad is all yours. # look: quad-dawn
    -> path
* [Use: Your earbuds (music up)]
    Loud. Louder. Your feet on the path, your breath in the cold, the bass in your chest. Nobody's body but yours. # look: run-loud
    ~ nudge(-0.15)
-
-> ash

= ash
# place: episcopal_church
# shot: wide
# objective: Sit with Mark
// Ash Wednesday, a holiday (owner: Axel goes on holidays).
- (nave)
+ {nave < 3} [Look: The windows]
    Stained glass. A woman at a well. A man with a lamb. # look: episcopal-windows
    -> nave
+ {nave < 3} [Look: The notice board]
    "Pride committee meets Tuesdays." "Trans Day of Visibility potluck." "Bring a dish, bring a friend." # look: episcopal-board
    -> nave
* [Use: A seat beside Mark]
-
Father Key: Axel, isn't it? Mark's told us about you. Welcome. Truly. # vo: ch1-5-axel_key_0028
Father Key: Easter's in April. We do a big breakfast after. You'd be very welcome. Both of you. # vo: ch1-5-axel_key_0029
* [Talk: Father Key (say thank you)]
    ~ record_choice("ch1_5.p3.priest", "thanks", "When Father Key welcomed you, you said thank you, and meant it.")
    Axel: Thanks. I'm... not really a church person. But thanks. # vo: ch1-5-axel_axel_0030
    Father Key: Neither are half the people here. That's never stopped anyone. # vo: ch1-5-axel_key_0031
* [Talk: Father Key (dark joke)]
    ~ record_choice("ch1_5.p3.priest", "joke", "When Father Key welcomed you, you asked if they were sure. They laughed.")
    Axel: Are you sure? I'm told I'm a lot. # vo: ch1-5-axel_axel_0032
    Father Key: Good. We're a lot too. # vo: ch1-5-axel_key_0033
-
-> after_ash

= after_ash
// Owner: a full day in every part. Claude's scene.
# place: campus_cafe
# shot: wide
# objective: Get coffee before class
- (cafe)
+ {cafe < 2} [Look: Your reflection]
    A smudge of ash on your forehead in the café window. A guy at the counter asks if you fell. # look: ash-reflection
    -> cafe
* [Talk: The guy at the counter]
    "Ash Wednesday. Remember you're dust. Also, a medium latte." He doesn't know what to say. You love that. # look: ash-latte
-
-> protest

= protest
# place: protest_march
# shot: wide
# objective: March with the church
Mark: They're lighting candles at the end. Here. Hold this. Careful, it drips. # vo: ch1-5-axel_mark_0034
# cue: crowded
~ set_experience("Derealization", 0.4)
- (march)
+ {march < 3} [Look: The candles]
    Hundreds of candles in paper cups. Singing, somewhere up ahead. # look: vigil-candles
    -> march
+ {march < 3} [Look: The signs]
    Hand-painted signs. A child on someone's shoulders, holding one bigger than she is. # look: child-shoulders
    ~ set_experience("Depersonalization", 0.4)
    -> march
* [Use: The candle]
-
// Owner: Izzy is triggered here (Maya went to protests with Sarah as a child).
~ nudge(0.4)
...
# switch: izzy
Izzy: Oh. Oh, candles. We used to do this. Mom made us carry the sign. # vo: ch1-5-axel_izzy_0035
* [Look: The candle]
    The flame, bending in the wind. Round, careful hands around it. # look: izzy-candle
-
...
# switch: axel
* [Look: Your hands]
    ~ record_choice("ch1_5.p3.protest", "gap", "At the vigil, you lost a few minutes. You came back holding a candle you didn't remember lighting.")
    Wax on your fingers. A candle you don't remember lighting. # look: axel-wax
-
Mark: Hey. You were singing. I didn't know you knew the words. # vo: ch1-5-axel_mark_0036
Axel: Yeah. Me neither. # vo: ch1-5-axel_axel_0037
-> easter

= easter
# place: queer_affirming_church
# shot: wide
# objective: Find a seat
Mark: I've wanted to come here since I was fifteen. I used to look it up at night. Okay. Okay, I'm fine. # vo: ch1-5-axel_mark_0038
- (sanctuary)
+ {sanctuary < 3} [Look: The pastor]
    A pastor in a rainbow stole, hugging people at the door like she's known them for years. # look: affirming-pastor
    -> sanctuary
+ {sanctuary < 3} [Look: The congregation]
    Two dads with a toddler. Old women in hats. A boy in a binder and a blazer, beaming. # look: affirming-congregation
    -> sanctuary
* [Use: The seat beside Mark]
-
* [Talk: Mark (tell him you're glad you came)]
    ~ record_choice("ch1_5.p3.easter", "glad", "At Easter, in the queer-affirming church Mark had wanted to visit for years, you told him you were glad you came.")
    Axel: I'm glad I came. Don't make it weird. # vo: ch1-5-axel_axel_0039
    Mark: I'm going to make it so weird. # vo: ch1-5-axel_mark_0040
* [Use: Mark's hand]
    ~ record_choice("ch1_5.p3.easter", "hand", "At Easter, you held Mark's hand while he cried a little in the church he'd wanted to visit for years.")
    His hand, shaking a little. Then not. # look: easter-hand
-
-> ch1_5_part4  // the next part of the same chapter (one run)
