// IN THE GAME as a DRAFT (Claude's guesses, marked; the owner's answers and Doc edits come first). Chapter 6 (Sikhism), Part 2.
// vo-speakers: Maya=maya, Gurleen=gurleen, Bibi Harjit=harjit, Christie=christie
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Vaisakhi. Kent, April 2028. Maya is 29.
//  Owner: Christie tells Maya that Adeena married Michael; it's triggering (the Shabbat candles they shared every Friday).
//
//  My guesses, for the owner to correct:
//   - HOW: Christie phones, out of the blue, in the middle of the Vaisakhi nagar kirtan (the street procession). She's kind and
//     has no idea how hard it will land, the way Sarah had no idea about Mira (an echo, never pointed out). Somebody from the
//     Field Museum posted the wedding photos; there was a chuppah (the `weddings` trigger, cue "wedding").
//   - What happens next is the trigger system's (dissociation, maybe a switch to someone unlocked; no player control). Bibi
//     Harjit finds her afterwards and does what she does: sits her down, feeds her, gives her hands something to do (seva as
//     DBT's Contributing, from ACCEPTS: `# exercise: accepts`).
//   - Vaisakhi (research): the harvest festival, and the day in 1699 when Guru Gobind Singh founded the Khalsa. The nagar
//     kirtan: the Guru Granth Sahib on a decorated float, the Panj Pyare (the five beloved) in saffron walking ahead barefoot
//     with swords, sevadars sweeping the road in front of the float, free food stalls all along the route, a gatka (Sikh martial
//     art) display by Nihang Sikhs (the traditional order in blue, with tall turbans).
// ─────────────────────────────────────────────────────────────────────────────

=== ch6_part2 ===
~ set_age(29)
~ set_date(2028, 4, 16)  // Claude's guess (the Sunday after Vaisakhi, April 13; owner: change it freely)
# cw: religious-content
-> onions

= onions
# place: gurdwara_kitchen
# shot: over-shoulder
# objective: Peel the onions
- (k)
+ {k < 3} [Look: The onions]
    A sack of them, taller than a child. Four Sundays now. You've stopped crying into them. Mostly. # look: onion-sack
    -> k
+ {k < 3} [Look: Bibi Harjit]
    Stirring, tasting, scolding a nephew, all at once. Her steel bracelet clicks on the pot every time. # look: bibi-stirring
    -> k
* {k >= 1} [Talk: Bibi Harjit]
-
Bibi Harjit: Today is big. Vaisakhi. The Khalsa's birthday. The whole town walks with the Guru. You'll walk too, then we feed everybody. # vo: ch6-sikhism_harjit_0013
Maya: The whole town? # vo: ch6-sikhism_maya_0014
Bibi Harjit: Whoever comes. We don't check. # vo: ch6-sikhism_harjit_0015
-> procession

= procession
# place: nagar_kirtan_street
# shot: wide
# objective: Walk with the procession
- (street)
+ {street < 5} [Look: The float]
    A flatbed truck in flowers and gold. The Guru Granth Sahib rides on it under its canopy, a granthi waving the whisk all the way. # look: nagar-kirtan-float
    -> street
+ {street < 5} [Look: The five in saffron]
    Five Sikhs in saffron robes, barefoot, swords held upright, walking ahead of everything. Gurleen: "The Panj Pyare. The first five who said yes." # look: panj-pyare
    -> street
+ {street < 5} [Look: The sweepers]
    People with brooms walk in front of the float, sweeping the road clean before the Guru reaches it. Arjan is one of them, very serious. # look: road-sweepers
    -> street
+ {street < 5} [Look: The gatka]
    A ring of people. Inside it, men in blue with tall turbans spin sticks and swords so fast they blur. A little girl in blue does it too, better than some of them. # look: gatka
    -> street
+ {street < 5} [Look: The food stalls]
    Every few houses, a table: samosas, oranges, bottled water, chai. Free. Uncles pressing them into your hands whether you want them or not. # look: kirtan-stalls
    -> street
* {street >= 2} [Use: Your phone (it's ringing)]
-
Christie. You haven't talked in a year. # look: phone-christie
Christie (on the phone): Maya! Hey, stranger! Are you at a parade? I can hear drums. # vo: ch6-sikhism_christie_0016
Maya: Sort of. Christie, hi! How are you? # vo: ch6-sikhism_maya_0017
Christie (on the phone): Good, good. Listen, I didn't know if I should tell you. But I'd want to know. Adeena got married. # vo: ch6-sikhism_christie_0018
Christie (on the phone): Last month. A guy called Michael, from her community. Somebody from registration posted the photos. There was a chuppah and everything, she looked so happy. # vo: ch6-sikhism_christie_0019
# cue: wedding
~ set_experience("Derealization", 0.7)
Friday. Two flames. Her hands over her eyes, and yours over hers. Every week. Somebody else's hands now. # look: candles-remembered
Christie (on the phone): Maya? Are you there? # vo: ch6-sikhism_christie_0020
* [Talk: Christie (say you're glad for her)]
    ~ record_choice("ch6.p2.christie", "glad", "When Christie told you Adeena had married Michael, you said you were glad for her, from very far away.")
    Maya: That's... I'm glad. I'm glad she's happy. I have to go, Christie. # vo: ch6-sikhism_maya_0021
* [Talk: Christie (say you have to go)]
    ~ record_choice("ch6.p2.christie", "go", "When Christie told you Adeena had married Michael, you said you had to go, and hung up.")
    Maya: I have to go. Sorry. I'm sorry. # vo: ch6-sikhism_maya_0022
* [Wait (say nothing)]
    ~ record_choice("ch6.p2.christie", "silent", "When Christie told you Adeena had married Michael, you couldn't say anything at all. Christie said your name a few times and then the call ended.")
    The drums. The phone, warm against your ear. Your name, again, small. Then nothing. # look: call-ended
-
# maybe-switch
-> after

= after
# place: gurdwara_langar_hall
# shot: close
# objective: Find somewhere to sit
You're on the floor of the langar hall. You don't remember walking back. There's a cup of chai in front of you that you didn't pour. # look: chai-floor
~ set_experience("Depersonalization", 0.4)
Bibi Harjit: Drink. Then talk, or don't talk. Both are fine. # vo: ch6-sikhism_harjit_0023
* [Talk: Bibi Harjit (tell her)]
    ~ record_choice("ch6.p2.harjit", "told", "After Christie's call, you told Bibi Harjit that the woman you loved had married someone else.")
    Maya: Someone I loved got married. To someone else. I knew it would happen. I didn't know it would feel like this. # vo: ch6-sikhism_maya_0024
    Bibi Harjit: Of course like this. You loved her. Love doesn't read the newspaper. # vo: ch6-sikhism_harjit_0025
* [Wait (just drink the chai)]
    ~ record_choice("ch6.p2.harjit", "chai", "After Christie's call, you didn't say anything. Bibi Harjit sat with you until the chai was gone.")
    Sweet and hot, cardamom. She sits beside you, knees cracking, and doesn't say a word until the cup is empty. # look: bibi-sits
-
Bibi Harjit: Now. Two thousand people are coming hungry. My hands need your hands. Come. # vo: ch6-sikhism_harjit_0026
~ learn_skill("accepts")  // DBT ACCEPTS: Contributing (seva)
* [Use: The dish station (wash)]
    ~ record_choice("ch6.p2.seva", "dishes", "After Christie's call, Bibi Harjit put you on dishes. You washed until your hands were wrinkled and you were back.")
    Hot water. Steel plates, one after another, a thousand of them. Somewhere around the three hundredth, you're back. # look: steel-plates
    ~ nudge(-0.3)
* [Use: The water jug (serve the rows)]
    ~ record_choice("ch6.p2.seva", "water", "After Christie's call, Bibi Harjit sent you down the rows with a water jug. Every cup was someone looking up at you.")
    A steel jug, the rows. Pour, smile, next. Every cup, someone's face looking up at you. # look: water-seva
    ~ nudge(-0.3)
-
Heather (inside): I've got the candle part. Whenever you want it. You don't have to carry that one. # vo: ch6-sikhism_heather-inside_0027
~ system_log("heather", "Christie called. A. is married. You washed a thousand plates. I'm keeping the Friday candles safe for you, for as long as you need. -H")
~ text_to("tanya", "Adeena got married. Christie told me. I'm okay. I'm not okay. I washed a thousand plates.")
~ text_from("tanya", "Oh, love. A thousand plates is a real spell. Call me tonight, any time, I'll be up.")
~ journal("Note", "Vaisakhi. Christie called. Adeena married Michael. I lost some of the procession. Bibi Harjit: drink, then talk or don't. Then dishes, until I was back.")
-> ch6_part3  // the next part of the same chapter (one run)
