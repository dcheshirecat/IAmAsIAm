// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 2 (Judaism), Part 4.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 4. Not here. Chicago, 2025. Maya is 27.
//  Owner's beats: Maya dissociates more and more, and Adeena notices she isn't present in the relationship, and breaks up
//  with her (two and a half years together). After the breakup Maya keeps exploring, alone. Michael and the wedding come
//  much later (Chapter 6), not here. The chapter unlocks nobody.
//
//  My guesses, for the owner to correct:
//   - The breakup is on a Friday, after the candles, the last time. Adeena is kind and very sure.
//   - Owner: show Beta Israel (Ethiopian Jews): a Sigd celebration in November, led by a kes. Tigist (owner: keep the name) is a Beta Israel woman.
//   - Alone, Maya goes to a Renewal Kabbalat Shabbat (singing, drums, candles: the candles trigger her now), a Reform
//     temple's Friday service, and Shabbat lunch at Effie's (Modern Orthodox), who stays kind to her.
//   - The chapter ends with Rabbi Abe, walking by the lake, and the first text to Tanya about how bad the lost time is.
// ─────────────────────────────────────────────────────────────────────────────

=== ch2_part4 ===
~ set_age(27)
~ set_date(2025, 3, 14)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
# cw: time-loss
# cw: breakup
# cw: isolation
-> friday

= friday
# place: adeena_apartment
# shot: close
# objective: Be here
~ set_experience("Depersonalization", 0.7)
~ set_experience("Derealization", 0.5)
* [Use: Your hands (cover your eyes)]
    Warm dark. Her voice, very far away. Further every week. # look: candles-far
-
* [Look: Adeena]
    She's looking at you. She's been looking at you for a while. # look: adeena-looking
-
Adeena: Maya. Come back. Please. Just for dinner. # vo: ch2-judaism_adeena_0043
~ set_experience("TimeLoss", 0.6)
# maybe-switch
-> talk

= talk
# place: adeena_apartment
# shot: close
# objective:
Adeena: I'm going to say this once, and I'm going to say it right, so please let me finish. # vo: ch2-judaism_adeena_0044
Adeena: Two and a half years. I love you. And most nights, I'm at this table by myself. You're sitting there, and you're not here. I can't keep lighting candles for someone who isn't here. # vo: ch2-judaism_adeena_0045
* [Talk: Adeena (say you'll try harder)]
    ~ record_choice("ch2.p4.breakup", "try", "When Adeena ended it, you said you'd try harder. She said it wasn't about trying.")
    Maya: I'll try harder. I can... I'll be here. # vo: ch2-judaism_maya_0046
    Adeena: It isn't about trying. I don't think you can, right now. That isn't your fault. It's still true. # vo: ch2-judaism_adeena_0047
* [Talk: Adeena (say you know)]
    ~ record_choice("ch2.p4.breakup", "know", "When Adeena ended it, you told her you knew. You'd known for a while.")
    Maya: I know. I know I'm not. # vo: ch2-judaism_maya_0048
    Adeena: I know you know. That's the worst part. # vo: ch2-judaism_adeena_0049
* [Wait (say nothing)]
    ~ record_choice("ch2.p4.breakup", "silent", "When Adeena ended it, you couldn't say anything at all.")
    The candles. Her face, gold, and wet. # look: adeena-crying
-
Adeena: Get help, Maya. Real help. Promise me that one thing. # vo: ch2-judaism_adeena_0050
~ journal("Note", "Adeena ended it. Two and a half years. \"Get help. Real help.\"")
-> alone

= alone
# place: chicago_apartment
# shot: close
# objective: Find somewhere to be on Friday night
~ text_from("tanya", "Maya. I'm so sorry. Call me, or don't, but I'm here. Every Thursday, every day.")
~ text_from("christie", "Heard about you and Adeena. I'm so sorry. Snack drawer is yours all week. No questions. -C")
// Owner: herbs and stones from different people (Claude's list, owner: "yes").
In your bag, from work: chamomile tea bags, a whole row of them, taped together. "For the bad nights. -C" # look: christie-chamomile
~ gain_herb("chamomile", "christie")
// Owner: cleansing after the breakup (it can be hard). Claude's addition: no new voiced lines.
~ text_from("tanya", "When you're ready, and only then: open a window, rosemary or salt or the bell, room by room. Say what you want the place to hold now. It's still yours.")
~ learn_practice("cleansing", "tanya")
- (week)
+ {week < 4} [Look: The calendar]
    Friday. Friday is a hole in the week now. # look: friday-hole
    -> week
+ {week < 4} [Look: Your phone]
    "Kabbalat Shabbat with drums! Everyone welcome. Bring your voice." A flyer someone shared. # look: renewal-flyer
    -> week
// Owner: lost-time traces of the craft, rarely, never explained (Jacob's rosary).
+ {week < 4} [Look: The rosary]
    A rosary on the altar, between your candles. Wooden beads. You've never owned a rosary. # look: rosary-on-altar
    -> week
+ {week < 4} [Look: Mira's stone]
    On the dresser. The hole worn through it, and the dresser showing through. # look: stone-alone
    -> week
* [Use: The window (cleanse the apartment)]
    The windowsill where the Friday candles stood. You open the window anyway. # ritual: cleansing hard
    -> week
// Owner: Nina is a devout Quaker. Only while Nina fronts (never hinted otherwise). No new voiced lines.
+ {fronts("nina") && week < 4} [Use: The rug (hold Mira in the Light)]
    Cross-legged on the rug. You hold Mira in the Light, the way Mira held you. Then Maya. Then everyone in here. # look: nina-holding-light
    ~ get_to_know("nina", 1)
    -> week
+ [Use: Your phone] -> phone
* {week >= 2} [Use: Your keys (go)]
-
-> renewal

= phone
Your phone. # device: phone
-> alone.week

= renewal
// Jewish Renewal: ecstatic, musical, mystical; drums, chant, dancing.
# place: renewal_shabbat
# shot: wide
# objective: Stay for the singing
- (room)
+ {room < 3} [Look: The drums]
    Hand drums, a guitar, a man with a tambourine and his eyes shut. Somebody's grandma, dancing. # look: renewal-drums
    -> room
+ {room < 3} [Look: The circle]
    Sixty people singing a melody with no words, over and over, until it sounds like one voice. # look: niggun-circle
    -> room
* [Look: The candles]
    Two candles on a little table. A woman lighting them, and covering her eyes. # look: renewal-candles
-
# cue: shabbat-candles
~ set_experience("Derealization", 0.6)
* [Use: Mira's stone (hold on)]
    ~ record_choice("ch2.p4.renewal", "stone", "At the Renewal service, the candles took you somewhere far away. You held Mira's stone until you came back.")
    The stone. Your finger through the hole. The drums, coming back. # look: stone-drums
    ~ nudge(-0.15)
* [Use: The door (go outside)]
    ~ record_choice("ch2.p4.renewal", "left", "At the Renewal service, the candles took you somewhere far away, and you went outside until you could breathe.")
    Cold air. The singing, through the wall. # look: renewal-outside
-
-> reform

= reform
// Reform: a big temple, English and Hebrew, a choir and organ in some congregations, a woman rabbi.
# place: reform_temple
# shot: wide
# objective: Find a seat at the back
- (temple)
+ {temple < 3} [Look: The bimah]
    A rabbi in a flowered tallit, a cantor with a guitar. Half the prayers in English. # look: reform-bimah
    -> temple
+ {temple < 3} [Look: The list of names]
    "Those we remember this week." Someone reads the names aloud. People stand for their own. # look: mourners-names
    -> temple
* [Wait (stay to the end)]
-
-> sigd

= sigd
// Beta Israel (Ethiopian Jews), owner: show them. Sigd, 29 Cheshvan (November 2025), fifty days after Yom Kippur: the community
// renews its covenant with the Torah and its longing for Jerusalem. Traditionally a fast, prayers led by the kessim (the
// community's priests) on a hilltop, readings from the Orit, then breaking the fast together. An Israeli national holiday since
// 2008, and celebrated by Ethiopian Jewish communities abroad.
# place: sigd_community_hall
# shot: wide
# objective: Stay for the end of the fast
Tigist: Welcome, welcome. You found the flyer? Good. Today is Sigd. We fast, we pray, we remember Jerusalem, and tonight we eat. Sit anywhere. # vo: ch2-judaism_tigist_0061
- (fast)
+ {fast < 4} [Look: The kes]
    An old man in white, a white turban, a bright umbrella held over him like a little sky. He reads in a language you've never heard. # look: kes-umbrella
    -> fast
+ {fast < 4} [Look: The book]
    Wrapped in cloth. Tigist says it's the Orit, the Torah in Ge'ez. "My grandmother kissed it every year. So now I do." # look: orit
    -> fast
+ {fast < 4} [Look: The families]
    White shawls with coloured borders. Children asleep on laps. Teenagers filming on phones, and singing anyway. # look: sigd-families
    -> fast
* {fast >= 2} [Talk: Tigist]
-
Tigist: For a thousand years, far from everyone, we kept this. Nobody told us we were Jews. We knew. Sigd is the day we say it again. # vo: ch2-judaism_tigist_0062
* [Talk: Tigist (ask why the umbrellas)]
    ~ record_choice("ch2.p4.sigd", "umbrellas", "At Sigd, you asked Tigist about the bright umbrellas over the kessim.")
    Maya: Why the umbrellas? # vo: ch2-judaism_maya_0063
    Tigist: Honour. Shade, too, on the mountain. Here there's no sun, only the ceiling. We bring them anyway. # vo: ch2-judaism_tigist_0064
* [Talk: Tigist (thank her)]
    ~ record_choice("ch2.p4.sigd", "thanks", "At Sigd, you thanked Tigist for letting you in.")
    Maya: Thank you for letting me be here. # vo: ch2-judaism_maya_0065
    Tigist: Everyone breaks the fast together. Even the guests. Especially the guests. # vo: ch2-judaism_tigist_0066
-
~ journal("Note", "Sigd. Fifty days after Yom Kippur. The kessim under bright umbrellas. \"We knew.\"")
-> rivka

= rivka
// Modern Orthodox: Effie invites Maya for Shabbat lunch after the breakup.
# place: effie_house
# shot: wide
# objective: Have lunch at Effie's
Effie: Adeena's my cousin. You're still my friend. Those are both allowed. Sit. # vo: ch2-judaism_effie_0051
- (lunch)
+ {lunch < 3} [Look: The lights]
    The lamps on timers. Nobody touches a switch from Friday night to Saturday night. Rest, Effie says, from making things happen. # look: shabbat-timers
    -> lunch
+ {lunch < 3} [Look: The children]
    Four of them, all talking at once, all in their Shabbat clothes, one with jam on hers. # look: rivka-children
    -> lunch
* [Talk: Effie (ask about Adeena)]
    ~ record_choice("ch2.p4.rivka", "asked", "At Shabbat lunch, you asked Effie how Adeena was.")
    Effie: She's going to a Hasidic shul in West Rogers Park now. She's... finding something. She asked about you. That's all I'll say. # vo: ch2-judaism_effie_0052
* [Talk: Effie (say thank you)]
    ~ record_choice("ch2.p4.rivka", "thanks", "At Shabbat lunch, you thanked Effie for still having you.")
    Maya: Thank you. For still having me. # vo: ch2-judaism_maya_0053
    Effie: Hachnasat orchim. Welcoming guests. It's a mitzvah. Also, I like you. # vo: ch2-judaism_effie_0054
-
-> lake

= lake
# place: lakefront_path
# shot: over-shoulder
# objective: Walk with Rabbi Abe
Rabbi Abe: I walk every morning. Doctor says it's good for the heart. I say I'm seventy-eight, my heart's done fine, it's my knees that want a word. # vo: ch2-judaism_rabbi-abe_0055
- (walk)
+ {walk < 3} [Look: The lake]
    Grey to the edge of the world. Waves folding over and over on the rocks. # look: lake-grey
    -> walk
* [Look: The path's edge]
    Dried yarrow, flat white heads gone brown along the path. You snap off one stalk. # look: lake-yarrow
    ~ gain_herb("yarrow", "")
    -> walk
+ {walk < 3} [Look: Rabbi Abe]
    Sneakers. A windbreaker. A tie with little fish on it, even here. # look: abe-windbreaker
    -> walk
* [Talk: Rabbi Abe (tell him about the lost time)]
    ~ record_choice("ch2.p4.abe", "told", "Walking by the lake, you told Rabbi Abe about the lost time.")
    Maya: I lose time. Whole days. People tell me things I did and I don't remember doing them. I don't know what's wrong with me. # vo: ch2-judaism_maya_0056
    Rabbi Abe: Pikuach nefesh. Saving a life comes before almost everything. Before Shabbat, before the rules, before pride. Your life too. Go see someone who knows about this. Then come walk with me again. # vo: ch2-judaism_rabbi-abe_0057
* [Talk: Rabbi Abe (ask him why he still believes)]
    ~ record_choice("ch2.p4.abe", "believes", "Walking by the lake, you asked Rabbi Abe why he still believes.")
    Maya: Why do you still believe? After everything. # vo: ch2-judaism_maya_0058
    Rabbi Abe: Some days I don't! I argue. Jacob wrestled an angel all night and wouldn't let go until it blessed him. That's us. We don't let go. # vo: ch2-judaism_rabbi-abe_0059
-
Rabbi Abe: Same time next week? My knees and I will be here. # vo: ch2-judaism_rabbi-abe_0060
~ journal("Note", "Rabbi Abe, by the lake. Pikuach nefesh: saving a life comes first. Your life too.")
-> home

= home
# place: chicago_apartment
# shot: close
# objective:
~ text_to("tanya", "I think something is really wrong with me. The lost time. It's getting worse.")
~ text_from("tanya", "Thank you for telling me. You're not alone in it. Let's find you someone to talk to. I'll help.")
* [Use: Your phone]
    Your phone. # device: phone
-
* [Look: The windowsill]
    The windowsill. Empty. A ring of old wax in the corner. # look: sill-wax
-
Chicago. 2025. # chapter-end
-> END
