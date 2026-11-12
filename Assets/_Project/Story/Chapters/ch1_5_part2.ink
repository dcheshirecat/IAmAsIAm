// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 1.5 (Axel), Part 2.
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Mark. Champaign-Urbana and Indianapolis, September to December 2018. The body is 20.
//  Owner's beats: Mark is instantly kind and funny and matches Axel's values and humour; dates, studying, cuddling,
//  parties. They go to Pride with Mark's church, which is queer-friendly and warm. Christmas with Mark's family, at their
//  Methodist church. Other alters' signs show up for Axel too, but less than for Maya.
//
//  My guesses, for the owner to correct:
//   - Pride is the town's Pride festival in September; Mark's church walks with a banner. Axel goes as Mark's date.
//   - (Owner) Mark's hometown is Indianapolis; his parents' Methodist church has a candlelight service on Christmas Eve.
//   - The signs: a note on the fridge in round letters (Izzy), the bed made with hospital corners (Nina).
// ─────────────────────────────────────────────────────────────────────────────

=== ch1_5_part2 ===
~ set_age(20)
~ set_date(2018, 10, 6)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
# cw: time-loss
-> study

= study
# place: college_apartment_axel
# shot: close
# objective: Study (allegedly)
Mark: Okay. Page forty. "Durkheim argues"... no. I can't. Your couch is too comfortable. This is your fault. # vo: ch1-5-axel_mark_0018
- (couch)
+ {couch < 3} [Look: Mark]
    Mark, upside down on the couch, the book on his face. # look: mark-upside-down
    -> couch
+ {couch < 3} [Look: The fridge]
    A note in round letters. "Full moon Tues!! -I". You don't know an I. # look: fridge-note-axel
    ~ set_experience("Derealization", 0.2)
    -> couch
* [Use: The couch (cuddle)]
    ~ record_choice("ch1_5.p2.study", "cuddle", "Studying with Mark turned into falling asleep on the couch with him. You didn't mind at all.")
    Mark's arm. The book on the floor. Durkheim can wait. # look: couch-cuddle
* [Use: The book (actually study)]
    ~ record_choice("ch1_5.p2.study", "study", "Studying with Mark, you actually studied. He was impressed. Then you both fell asleep anyway.")
    Page forty. Page forty-one. Mark asleep on your shoulder by page forty-three. # look: page-43
-
-> pride

= pride
# place: town_pride
# shot: wide
# objective: Find Mark's church at Pride
Mark: There! The banner with the dove. That's us. Well, them. Well, us. You're with me, so you're us. # vo: ch1-5-axel_mark_0019
- (march)
+ {march < 3} [Look: The banner]
    "St. Luke's Episcopal Church: All Are Welcome. We Mean It." A rainbow dove. # look: church-banner
    -> march
+ {march < 3} [Look: The crowd]
    Glitter. Strollers. A drag queen with a parasol. A grandmother with a sign: "I love my trans grandson." # look: pride-crowd
    -> march
* [Use: Mark's hand]
    ~ record_choice("ch1_5.p2.pride", "hand", "At Pride, you held Mark's hand in front of everyone.")
    His hand. Warm. Nobody looking twice. # look: pride-hand
-
Axel: A church at Pride. If you'd told the old me this, I'd have laughed you out of the room. # vo: ch1-5-axel_axel_0020
Mark: The old you sounds like a lot. I like this one. # vo: ch1-5-axel_mark_0021
-> christmas

= christmas
# place: methodist_church
# shot: wide
# objective: Survive Christmas with Mark's family
Cassie: You must be Axel! We've heard so much. Come in, come in, you're freezing. # vo: ch1-5-axel_cassie_0022
~ set_experience("Depersonalization", 0.2)
- (pews)
+ {pews < 3} [Look: The candles]
    A small candle in every hand, a paper ring around each one to catch the wax. # look: methodist-candles
    -> pews
+ {pews < 3} [Look: The hymnal]
    A Methodist hymnal. Somebody's grandmother's name inside the cover. # look: methodist-hymnal
    -> pews
* [Use: Your candle]
-
Mark: You okay? You've gone quiet. You never go quiet. # vo: ch1-5-axel_mark_0023
* [Talk: Mark (tell him churches make you angry)]
    ~ record_choice("ch1_5.p2.christmas", "honest", "On Christmas Eve, you told Mark churches make you angry. He held your hand and didn't try to fix it.")
    Axel: Churches make me angry. Not this one. Just... the idea. I grew up in one. Long story. # vo: ch1-5-axel_axel_0024
    Mark: You don't have to tell me. I'm glad you came anyway. # vo: ch1-5-axel_mark_0025
* [Talk: Mark (make a joke)]
    ~ record_choice("ch1_5.p2.christmas", "joke", "On Christmas Eve, you made a joke so Mark wouldn't worry. He laughed, and held your hand anyway.")
    Axel: I'm fine. Just calculating how many open flames are in this building. It's a lot. # vo: ch1-5-axel_axel_0026
    Mark: Fair. Fire safety is a spiritual practice. # vo: ch1-5-axel_mark_0027
-
// Lost time (owner: less than Maya's).
~ set_experience("TimeLoss", 0.3)
# place: mark_family_guest_room
# shot: close
* [Look: The bed]
    The bed, made with tight corners, the pillow squared. You don't remember making it. # look: bed-corners
-
-> ch1_5_part3  // the next part of the same chapter (one run)
