// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 2 (Judaism), Part 3.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Her people. Chicago, 2024. Maya is 25.
//  Owner's beats: Maya meets the other streams through Adeena's family and friends and the holidays (and, after the
//  breakup, alone: Part 4). Heather's one moment: Maya finds a single tall candle lit on the windowsill on the date of
//  Mira's death, and doesn't remember lighting it. Heather is NOT unlocked and is never named here.
//  Adeena is Conservative and becomes Hasidic (here: the first pull, a class at a Chabad house).
//
//  My guesses, for the owner to correct:
//   - Owner: Mira died on January 18th (2013). The candle is on January 18, 2024.
//   - Adeena's family at the Passover seder (owner's names): her mom Ruth (Reform), Uncle Lee (secular,
//     Humanistic Judaism), cousin Effie (Modern Orthodox), and Grandma, her grandmother, whose family came from Morocco
//     (Sephardi traditions: the Mimouna at the end of Passover).
//   - Friends: Hannah's Reconstructionist havurah (a lay-led group in a living room), and a Pride Shabbat at an
//     LGBTQ congregation.
// ─────────────────────────────────────────────────────────────────────────────

=== ch2_part3 ===
~ set_age(25)
~ set_date(2024, 1, 18)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
# cw: time-loss
-> candle

= candle
// Heather (owner): one moment, never explained, never pointed out.
# place: chicago_apartment
# shot: over-shoulder
# objective: Get ready for work
~ set_experience("Derealization", 0.3)
- (morning)
+ {morning < 4} [Look: The windowsill]
    A candle. One, tall, in a glass. Lit. Burning low, like it's been burning all night. # look: tall-candle-sill
    -> morning
+ {morning < 4} [Look: The matches]
    The box from the kitchen drawer. On the sill. You don't keep matches by the window. # look: matches-sill
    -> morning
+ {morning < 4} [Look: Your phone]
    Thursday, January 18. A date that means something, and you can't think what. # look: phone-january
    -> morning
* {morning >= 2} [Use: The candle (leave it burning)]
    ~ record_choice("ch2.p3.candle", "left", "On the morning of January 18, you found a tall candle burning on your windowsill. You didn't remember lighting it. You left it burning.")
    You leave it. It seems important to leave it. # look: candle-left
* {morning >= 2} [Use: The candle (blow it out)]
    ~ record_choice("ch2.p3.candle", "out", "On the morning of January 18, you found a tall candle burning on your windowsill. You didn't remember lighting it. You blew it out, and felt sorry, and didn't know why.")
    A thin line of smoke. Then you feel sorry, and you don't know why. # look: candle-out
-
~ text_from("tanya", "Thinking of you today. I know today is hard.")
-> winter_day

= winter_day
// Owner: a full day in every part. Claude's scenes: the rest of January 18th (Mira's day), which Maya doesn't know is Mira's day.
# place: cta_train
# shot: over-shoulder
# objective: Get to work
- (car)
+ {car < 2} [Look: The window]
    Snow on the tracks, the lake frozen white at the edges. # look: l-window-snow
    -> car
* [Use: The pole (hold on)]
    You keep thinking about the candle. Your hands smell of matches. # look: l-matches
-
-> winter_work

= winter_work
# place: field_museum_gallery
# shot: wide
# objective: Give the noon tour
- (gallery)
+ {gallery < 3} [Look: The tour group]
    Six retirees and a man who keeps correcting your Latin. You let him. # look: retirees-latin
    -> gallery
+ {gallery < 3} [Look: The date on the schedule]
    "JAN 18." Something about it. You can't think what. # look: jan-18
    ~ set_experience("Derealization", 0.2)
    -> gallery
* [Talk: Christie (lunch)]
    Christie's soup, split two ways, in the break room. She does the voices of every curator. You laugh until you're here again. # look: christie-soup
    ~ nudge(-0.1)
-
-> winter_evening

= winter_evening
# place: chicago_apartment
# shot: close
# objective: Make dinner
- (k)
+ {k < 2} [Look: The windowsill]
    The candle, burnt down to nothing. A ring of wax on the sill. # look: wax-ring-evening
    -> k
* [Use: The stove (soup)]
    Soup from a can. You eat it at the window, looking at the ring of wax. # look: soup-window
-
* [Use: The bed]
    You leave the hall light on. You don't know why. # look: hall-light-on
-
-> imbolc

= imbolc
// Owner: every sabbat played at least once across the game (Claude's draft scene). No new voiced lines.
~ set_date(2024, 2, 1)
# place: chicago_apartment
# shot: over-shoulder
# objective: Light the windows for Imbolc
- (sills)
+ {sills < 3} [Look: The windowsill]
    The tall candle's stub is still there from last month. You leave it where it is. # look: imbolc-stub
    -> sills
+ {sills < 3} [Look: The snow]
    Grey snow on the L tracks. Under it, somewhere, seeds. # look: imbolc-snow
    -> sills
* [Use: The candles (keep Imbolc)]
    ~ record_choice("ch2.p3.imbolc", "kept", "At Imbolc, alone in Chicago, you put a candle in every window.")
    Brigid's day. A candle in every window, and one shelf, cleared. # ritual: sabbat imbolc
* [Wait (just one candle)]
    ~ record_choice("ch2.p3.imbolc", "one", "At Imbolc, you lit one candle in the kitchen window. It was enough.")
    One candle, in the kitchen window. That's enough, this year. # look: imbolc-one-candle
-
-> seder

= seder
~ set_date(2024, 4, 22)
# place: adeena_parents_house
# shot: wide
# objective: Find your seat at the seder
Ruth: You're Maya! Adeena told us nothing, which means she likes you very much. Sit, sit. You're next to Lee. I'm sorry in advance. # vo: ch2-judaism_ruth_0028
- (family)
+ {family < 5} [Talk: Lee]
    Lee: I don't believe in God, and I'm here every year. Ask me why. It's the people. And the brisket. Mostly the people. # vo: ch2-judaism_lee_0029
    -> family
+ {family < 5} [Talk: Effie]
    Effie: We do the full seder at my house. Every word. We finish at one in the morning. This is the express version. It's lovely. # vo: ch2-judaism_effie_0030
    -> family
+ {family < 5} [Talk: Grandma]
    Grandma: Taste. Dates, not apples. That's how my mother made it, in Casablanca. At the end of Pesach you come to me for Mimouna. Pancakes, honey. # vo: ch2-judaism_grandma_0031
    -> family
+ {family < 5} [Look: The seder plate]
    An egg, a bone, bitter herbs, something brown and sweet, parsley, salt water. Every one of them a story. # look: seder-plate
    -> family
* {family >= 3} [Wait (listen to the Four Questions)]
-
Adeena: Why is this night different from all other nights? You'll hear it four times. That's the point. We keep asking. # vo: ch2-judaism_adeena_0032
~ journal("Note", "Seder at Adeena's parents'. Ruth (Reform), Uncle Lee (doesn't believe, comes every year), Effie (Orthodox, every word), Grandma (dates, not apples).")
-> friends

= friends
# place: havurah_living_room
# shot: wide
# objective: Go to Hannah's with Adeena
// Reconstructionist: a lay-led havurah, Judaism as the evolving civilization of the Jewish people.
Hannah: No rabbi tonight. It's us. We read, we argue, we eat. Somebody leads, it rotates. It's my turn, so it's chaos. # vo: ch2-judaism_hannah_0033
- (havurah)
+ {havurah < 3} [Look: The books]
    A stack of prayer books someone made themselves, photocopied, stapled, full of notes in the margins. # look: havurah-books
    -> havurah
+ {havurah < 3} [Look: The circle]
    Cushions, a sofa, a cat that thinks it's leading. Nobody in charge, and everybody listening. # look: havurah-circle
    -> havurah
* [Wait (listen)]
-
-> pride

= pride
# place: lgbtq_synagogue
# shot: wide
# objective: Find Adeena in the crowd
// Pride Shabbat at an LGBTQ synagogue (placeholder. Fictional congregation).
- (crowd)
+ {crowd < 3} [Look: The flags]
    A rainbow flag beside the ark. A trans flag beside that. Nobody explaining why. # look: pride-shabbat-flags
    -> crowd
+ {crowd < 3} [Look: The couples]
    Two old men in matching kippot, holding hands like they've done it for fifty years. They have. # look: old-couple-kippot
    -> crowd
* [Talk: Adeena]
-
Adeena: I wanted you to see this. That there's room. For us. # vo: ch2-judaism_adeena_0034
* [Talk: Adeena (take her hand)]
    ~ record_choice("ch2.p3.pride", "hand", "At Pride Shabbat, you took Adeena's hand.")
    Her hand. She squeezes, once, hard. # look: adeena-hand
* [Talk: Adeena (say thank you)]
    ~ record_choice("ch2.p3.pride", "thanks", "At Pride Shabbat, you thanked Adeena for bringing you.")
    Maya: Thank you. For bringing me. # vo: ch2-judaism_maya_0035
-
-> chabad

= chabad
# place: chabad_house
# shot: over-shoulder
# objective: Pick Adeena up after her class
// Hasidic (Chabad): the start of Adeena's pull. Shown with respect, no villain.
- (house)
+ {house < 3} [Look: Through the door]
    Adeena at a long table, leaning in, asking something, the rebbetzin laughing and answering. You haven't seen her lean in like that. # look: chabad-class
    -> house
+ {house < 3} [Look: The portrait]
    An old man with a white beard, in a frame by the door. Kind eyes. Everyone here seems to know him. # look: rebbe-portrait
    -> house
* [Wait (for Adeena)]
-
Adeena: Sorry. Sorry. We got talking. There's so much I didn't know. I feel like I've been reading the book with half the pages missing. # vo: ch2-judaism_adeena_0036
* [Talk: Adeena (ask what she learned)]
    ~ record_choice("ch2.p3.chabad", "asked", "When Adeena came out of her class at the Chabad house, you asked her what she'd learned.")
    Maya: What did you learn? # vo: ch2-judaism_maya_0037
    Adeena: That every little thing can be holy. Even the way you tie your shoes. Is that silly? # vo: ch2-judaism_adeena_0038
* [Wait (just walk with her)]
    ~ record_choice("ch2.p3.chabad", "walked", "When Adeena came out of her class at the Chabad house, you just walked her home.")
    She talks the whole way home. You listen to most of it. # look: walk-home-chabad
-
-> quiet

= quiet
// Nina comes forward in quiet moments alone at home, if triggered (owner).
# place: chicago_apartment
# shot: close
# objective:
~ set_experience("Depersonalization", 0.45)
* [Look: The apartment]
    Sunday. Adeena at a class. The fridge, thinking. # look: apartment-quiet-2
-
~ set_experience("TimeLoss", 0.5)
# maybe-switch
Adeena: Where were you, Friday? You were sitting right there. You weren't there. # vo: ch2-judaism_adeena_0039
* [Talk: Adeena (say you were tired)]
    ~ record_choice("ch2.p3.where", "tired", "When Adeena asked where you'd gone on Friday, you said you were tired.")
    Maya: Just tired. Work. # vo: ch2-judaism_maya_0040
* [Talk: Adeena (say you don't know)]
    ~ record_choice("ch2.p3.where", "dont-know", "When Adeena asked where you'd gone on Friday, you told her the truth: you didn't know.")
    Maya: I don't know. # vo: ch2-judaism_maya_0041
-
Adeena: Okay. # vo: ch2-judaism_adeena_0042
-> ch2_part4  // the next part of the same chapter (one run)
