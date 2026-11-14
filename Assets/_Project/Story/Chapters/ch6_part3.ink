// IN THE GAME as a DRAFT (Claude's guesses, marked. The owner's answers and Doc edits come first). Chapter 6 (Sikhism), Part 3.
// vo-speakers: Maya=maya, Gurleen=gurleen, Bibi Harjit=harjit
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Sarbat da bhala. Kent, November 2028. Maya is 30.
//
//  My guesses, for the owner to correct:
//   - Guru Nanak's Gurpurab (his birthday, by the lunar calendar. Early November in 2028): an Akhand Path (the whole Guru Granth
//     Sahib read aloud without a break, about 48 hours, by readers taking turns) ends that morning. Kirtan all day. Langar for
//     everyone. Some families light lamps that night.
//   - Bibi Harjit's story: she came from Ludhiana in 1986. When her husband died, she came to the kitchen every day, "because
//     my hands knew what to do when my head didn't". Chardi kala: rising spirits, even in the dark (a Sikh ideal).
//   - Guru Nanak on women (Asa di Var, in the Guru Granth Sahib): "Why call her lesser, from whom kings are born?" Equality is
//     the point of langar too. A young woman in a dastar (turban) reads her turn in the Akhand Path.
//   - The Ardas (the prayer everyone stands for) ends "sarbat da bhala": may all be well, everyone. Maya can wish Adeena well,
//     or not yet. Both are all right.
//   - The chapter ends as Maya. It unlocks nobody (my guess. Owner to decide).
// ─────────────────────────────────────────────────────────────────────────────

=== ch6_part3 ===
~ set_age(30)
~ set_date(2028, 11, 5)  // Claude's guess (owner: change it freely)
# cw: religious-content
~ text_from("tanya", "Thinking of you this morning. Samhain was quiet here. I lit one for Mira, and one for you lot.")
-> dawn

= dawn
// Owner: a full day in every part. Claude's scenes: Guru Nanak's birthday from before dawn (a prabhat pheri, the early-morning
// hymn walk some sangats do around a gurpurab) to the lamp on the sill at night.
# place: nagar_kirtan_street
# shot: wide
# objective: Walk with the morning hymns
- (street)
+ {street < 3} [Look: The singers]
    Five in the morning, dark and cold. Twenty people walking slowly, singing, one harmonium carried on a strap. Porch lights coming on as they pass. # look: prabhat-pheri
    -> street
+ {street < 3} [Look: The house they stop at]
    A family on their steps in coats over pyjamas, handing out cups of hot tea and sweets to the singers. # look: pheri-tea
    -> street
* [Use: The cup of tea]
    Sweet, hot, cardamom. Your hands around it. The sky going grey behind the warehouses. # look: pheri-cup
    ~ nudge(-0.1)
-
-> reading

= reading
# place: gurdwara_darbar_hall
# shot: wide
# objective: Listen to the end of the reading
- (hall)
+ {hall < 4} [Look: The reader]
    A young woman in a deep red dastar, reading her turn of the forty-eight hours. Her voice never hurries. Gurleen: "Simran. She's at uni with Arjan. She reads better than the uncles, don't tell them." # look: simran-reading
    -> hall
+ {hall < 4} [Look: The uncles]
    Two old friends by the wall, one in a white turban and a long white beard, one clean-shaven in a cardigan, sharing a hymn book. Both here every Sunday for fifty years. # look: two-uncles
    -> hall
+ {hall < 4} [Look: The flowers]
    The canopy is all marigolds and lights today. Somebody's grandmother has been up since three. # look: gurpurab-flowers
    -> hall
+ {hall < 4} [Look: Your scarf]
    Your own scarf now, a blue one. You bought it in Kent, in a shop that sold scarves and pressure cookers and wedding jewellery. # look: maya-scarf
    -> hall
* {hall >= 2} [Wait (stand for the prayer)]
-
Everybody stands. One man's voice, then everyone's, at the end: "sarbat da bhala." # look: ardas
Gurleen: It means: the good of everyone. All of everyone. Even the ones you're mad at. That's the annoying part. # vo: ch6-sikhism_gurleen_0028
* [Wait (wish her well)]
    ~ record_choice("ch6.p3.ardas", "well", "At the end of the prayer on Guru Nanak's birthday, you wished Adeena well. And Michael. Mostly meant it.")
    Adeena, and her candles, and her Michael. The good of everyone. You mostly mean it. Mostly is all right. # look: ardas-adeena
    ~ nudge(-0.2)
* [Wait (not yet)]
    ~ record_choice("ch6.p3.ardas", "notyet", "At the end of the prayer on Guru Nanak's birthday, you couldn't wish Adeena well yet, and you let that be true.")
    Not her. Not yet. Everyone else. That's still a lot of everyone. # look: ardas-not-yet
    ~ nudge(-0.1)
-
-> kitchen

= kitchen
# place: gurdwara_kitchen
# shot: over-shoulder
# objective: Help with the langar
- (k)
+ {k < 3} [Look: The rotis]
    Yours are round now. Round-ish. The aunties have stopped re-rolling them when you're not looking. # look: maya-roti-round
    -> k
+ {k < 3} [Look: The photo by the door]
    A faded photo taped to the wall. The old kitchen, 1990, a young woman with flour to her elbows, laughing. It takes you a second. # look: bibi-photo
    -> k
* {k >= 1} [Talk: Bibi Harjit]
-
Bibi Harjit: You found my photo. Ludhiana girl, two years in America, no English. My husband drove trucks. Sunday, I came here. # vo: ch6-sikhism_harjit_0029
Bibi Harjit: When he died, I came every day. Not for God, even. My hands knew what to do when my head didn't. # vo: ch6-sikhism_harjit_0030
* [Talk: Bibi Harjit (say you know that)]
    ~ record_choice("ch6.p3.bibi", "know", "On Guru Nanak's birthday, Bibi Harjit told you why she came to the kitchen every day, and you told her you knew that feeling.")
    Maya: I know that. The hands knowing. I didn't, before here. # vo: ch6-sikhism_maya_0031
* [Talk: Bibi Harjit (ask if it gets better)]
    ~ record_choice("ch6.p3.bibi", "better", "On Guru Nanak's birthday, you asked Bibi Harjit if it gets better. She said: chardi kala.")
    Maya: Does it get better? # vo: ch6-sikhism_maya_0032
-
Bibi Harjit: We say chardi kala. The spirit goes up, even in the dark. Not because it's easy. Because you keep feeding people. # vo: ch6-sikhism_harjit_0033
Bibi Harjit: Guru Nanak said: why call her less, the one who gives birth to kings? Five hundred years ago, he said this. So. You're not less. Roll. # vo: ch6-sikhism_harjit_0034
* [Use: The rolling pin (press and turn)]
    ~ record_choice("ch6.p3.roti", "rolled", "On Guru Nanak's birthday, you rolled rotis at Bibi Harjit's table until the last of the two thousand had eaten.")
    Press, turn. Waheguru, under your breath, because the hands like it. Two thousand people, and every one gets a roti that's warm. # look: roti-gurpurab
    ~ nudge(-0.2)
-
Heather (inside): Round. Told you. Good hands. # vo: ch6-sikhism_heather-inside_0035
~ system_log("maya", "Guru Nanak's birthday at the gurdwara. 2000 rotis. Sarbat da bhala: the good of everyone. Working on it. -M")
~ journal("Note", "Gurpurab. Sarbat da bhala: the good of everyone. Chardi kala: the spirit goes up, even in the dark, because you keep feeding people.")
~ text_to("tanya", "Chardi kala. It means the spirit rising even in the dark. I think it might be what you've been doing for me all these years.")
~ text_from("tanya", "Ha. Don't tell anyone. They'll all want some.")
-> lamp

= lamp
# place: seattle_apartment
# shot: close
# objective: Light a lamp on the sill
- (sill)
+ {sill < 2} [Look: The street]
    Two houses down, someone has strung lights along the porch rail for the Gurpurab. # look: porch-lights-gurpurab
    -> sill
* [Use: The lamp (light it)]
    One small clay lamp from the gurdwara shop, on the sill by Mira's stone. Your hands, and Heather's, steady. # look: lamp-sill-gurpurab
-
* [Use: The bed]
    The little flame on the ceiling. The good of everyone. You fall asleep in the middle of the list. # look: flame-ceiling
-
# shot: inner
# place: black
Kent, Washington. November 2028. # chapter-end
-> END
