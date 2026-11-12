// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 2 (Judaism), Part 2.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Friday nights. Chicago, summer 2023. Maya is 25.
//  Owner's beats: Adeena is Conservative. Their special moment, every week: Adeena lights the Shabbat candles, and Maya
//  covers her eyes with her for the blessing. Maya meets Rabbi Abe (78, he/him, "vital and refreshing") at Adeena's
//  synagogue, at an interfaith couples' meetup. Nina comes forward in quiet moments alone at home, if triggered.
//
//  My guesses, for the owner to correct:
//   - Owner: Adeena's synagogue is Congregation Etz Chaim (Conservative).
//   - They start dating in early summer, after the coffee. The first Shabbat dinner is at Adeena's apartment.
//   - Rabbi Abe calls the meetup "keruv", bringing close (a real Conservative outreach word).
//   - The Saturday morning service: Maya follows the English, stands when everyone stands, and is told that's enough.
// ─────────────────────────────────────────────────────────────────────────────

=== ch2_part2 ===
~ set_age(25)
~ set_date(2023, 7, 7)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
-> wake

= wake
// Owner: a full day in every part. Claude's scenes: the Friday itself, before the first Shabbat.
# place: chicago_apartment
# shot: close
# objective: Get ready for work
~ text_from("ex_jewish", "Sunset is 8:16. Come at 7:30. Bring nothing. I mean it. Nothing.")
- (room)
+ {room < 3} [Look: The fan]
    The box fan in the window, pushing hot air around. July in Chicago. # look: box-fan
    -> room
+ {room < 3} [Look: Your closet]
    What do you wear to Shabbat? You try on three things. You'll change again after work. # look: closet-three
    -> room
* [Use: Your keys]
-
-> workday

= workday
# place: field_museum_hall
# shot: wide
# objective: Give the summer camp tour
- (hall)
+ {hall < 3} [Look: The campers]
    Thirty eight-year-olds in matching orange shirts, every one of them asking if Máximo is real. # look: campers-orange
    -> hall
+ {hall < 3} [Look: The clock]
    3:40. 3:41. Sunset is 8:16. # look: clock-sunset
    -> hall
* [Talk: The campers (answer the questions)]
    "Is he real?" Real bones, cast copies, real once. They decide that's the best answer anyone's ever given. # look: campers-answer
-
* [Use: The snack drawer]
    Christie's snack drawer. A granola bar, in place of lunch. "Big night?" she says. You don't say how big. # look: snack-drawer-friday
-
-> friday

= friday
# place: adeena_apartment
# shot: over-shoulder
# objective: Get to Adeena's before sunset
Adeena: You're early. Good. Early is correct. Shoes off, if you want. Help me with the table. # vo: ch2-judaism_adeena_0014
- (table)
+ {table < 4} [Look: The challah]
    Two loaves under an embroidered cloth. She braided them herself. She doesn't say so. You can tell. # look: challah-cloth
    -> table
+ {table < 4} [Look: The candlesticks]
    The small brass one from her desk at work, and its twin. "My grandmother's. Both of them. Don't touch, please." # look: brass-candlesticks
    -> table
+ {table < 4} [Look: The bookshelf]
    Prayer books with soft spines, cookbooks with softer ones. A photo: a big family, squinting into the sun. # look: adeena-bookshelf
    -> table
* {table >= 2} [Talk: Adeena (it's almost time)]
-
Adeena: I light, then I cover my eyes, then the blessing. Then I look, and it's Shabbat. You don't have to do anything. # vo: ch2-judaism_adeena_0015
* [Wait (watch her)]
    ~ record_choice("ch2.p2.candles", "watched", "At your first Shabbat with Adeena, you watched her light the candles.")
    Two flames. Her hands, circling them, three times, drawing the light in. # look: candles-first
* [Use: Your hands (cover your eyes with her)]
    ~ record_choice("ch2.p2.candles", "covered", "At your first Shabbat with Adeena, you covered your eyes with her for the blessing.")
    Your hands over your eyes. Warm dark. Her voice, beside you. # look: candles-covered
-
Adeena: Baruch atah Adonai, Eloheinu melech ha'olam, asher kid'shanu b'mitzvotav v'tzivanu l'hadlik ner shel Shabbat. # vo: ch2-judaism_adeena_0016
Adeena: Shabbat shalom. # vo: ch2-judaism_adeena_0017
* [Talk: Adeena (say it back)]
    Maya: Shabbat shalom. # vo: ch2-judaism_maya_0018
* [Wait (just look at her)]
    She isn't stern now. She's just here. # look: adeena-candlelight
-
~ set_experience("Depersonalization", 0.0)
~ nudge(-0.25)
~ journal("Note", "Friday nights at Adeena's. She lights, we cover our eyes, she says the blessing.")
-> walk_home

= walk_home
# place: lakefront_path
# shot: wide
# objective: Walk home
- (path)
+ {path < 3} [Look: The fireflies]
    Fireflies in the grass by the path. You didn't know Chicago had fireflies. # look: fireflies
    -> path
+ {path < 3} [Look: Your hands]
    Still warm from the candles. Still smelling a little of wax and challah. # look: hands-wax
    -> path
* [Use: The bench (sit a minute)]
    The lake, black and quiet. You sit until the warm feeling is something you could keep. # look: bench-night
-
-> morning

= morning
# place: synagogue_conservative
# shot: wide
# objective: Find a seat with Adeena
// Conservative (Masorti): Hebrew liturgy with English beside it, men and women seated together, egalitarian.
- (shul)
+ {shul < 4} [Look: The sign by the door]
    "Congregation Etz Chaim. Tree of Life. All are welcome." Under it, a man at the door who nods at Adeena by name. # look: etz-chaim-sign
    -> shul
+ {shul < 4} [Look: The prayer book]
    Hebrew on the right-hand page, English on the left. You read from the back to the front. # look: siddur
    -> shul
+ {shul < 4} [Look: The ark]
    A curtain, deep blue, with gold letters. A small lamp above it that never goes out. # look: ark-ner-tamid
    -> shul
+ {shul < 4} [Look: The people]
    Families together. A woman in a tallit reading from the scroll. A baby, loudly, and nobody minding. # look: shul-families
    -> shul
* {shul >= 2} [Wait (stand when everyone stands)]
-
Adeena: You don't need to know the words. Standing with us is enough. # vo: ch2-judaism_adeena_0019
-> keruv

= keruv
# place: synagogue_social_hall
# shot: wide
# objective: Go to the meetup with Adeena
Rabbi Abe: Come in, come in! Coffee's terrible, rugelach's excellent, that's the deal. Abe. Rabbi Abe, if you're formal. Nobody's formal. # vo: ch2-judaism_rabbi-abe_0020
Rabbi Abe: We call this keruv. Bringing close. Some of you married in, some of you are dating in, some of you just like rugelach. All of you are welcome. # vo: ch2-judaism_rabbi-abe_0021
- (hall)
+ {hall < 3} [Look: The circle of chairs]
    Couples, mostly. A man in a kippah holding hands with a man who keeps checking his phone. Two women, one very pregnant. # look: keruv-circle
    -> hall
+ {hall < 3} [Look: Rabbi Abe]
    Seventy-eight, and moving like he's late for something wonderful. Sneakers. A tie with little fish on it. # look: rabbi-abe
    -> hall
* [Talk: Rabbi Abe]
-
Rabbi Abe: And you are? # vo: ch2-judaism_rabbi-abe_0022
* [Talk: Rabbi Abe (say you were raised Quaker)]
    ~ record_choice("ch2.p2.abe", "quaker", "When you met Rabbi Abe, you told him you were raised Quaker.")
    Maya: Maya. I was raised Quaker. Now I'm... Wiccan, mostly. It's complicated. # vo: ch2-judaism_maya_0023
    Rabbi Abe: Complicated! Wonderful. I've been a rabbi fifty years. Complicated is the only kind of person there is. # vo: ch2-judaism_rabbi-abe_0024
* [Talk: Rabbi Abe (say you're with Adeena)]
    ~ record_choice("ch2.p2.abe", "adeena", "When you met Rabbi Abe, you told him you were there with Adeena.")
    Maya: Maya. I'm with Adeena. # vo: ch2-judaism_maya_0025
    Rabbi Abe: Adeena! I've known her since she was a terrifying bat mitzvah girl. She corrected my Hebrew. She was right. # vo: ch2-judaism_rabbi-abe_0026
-
Rabbi Abe: Questions are welcome here. Any question. We're a people who argue with God. You can argue with me. # vo: ch2-judaism_rabbi-abe_0027
~ journal("Note", "Rabbi Abe. Keruv: bringing close. \"Any question.\"")
-> weeks

= weeks
# place: adeena_apartment
# shot: close
# objective:
// The candles, week after week (owner: their special moment, every week).
* [Use: Your hands (cover your eyes)]
    Warm dark. Her voice. Every Friday. # look: candles-weekly
-
* [Use: Your hands (again)]
    Warm dark. The summer going. Her voice. # look: candles-weekly-2
-
* [Wait (look)]
    Two flames, and Adeena, smiling, which she almost never does anywhere else. # look: adeena-smile
-
-> quiet

= quiet
// Nina comes forward in quiet moments alone at home, if triggered (owner).
# place: chicago_apartment
# shot: close
# objective:
~ set_experience("Depersonalization", 0.35)
* [Look: The quiet]
    Sunday afternoon. Adeena at her parents'. The apartment so quiet you can hear the fridge thinking. # look: apartment-quiet
-
* [Use: Mira's stone]
    The stone. Your thumb around the hole, around and around. # look: stone-quiet
-
~ set_experience("TimeLoss", 0.4)
# maybe-switch
~ sent_while_away("nina", "text", "ex_jewish", "Thank you for this week. I'm sorry I was quiet. I'm really happy.")
-> ch2_part3  // the next part of the same chapter (one run)
