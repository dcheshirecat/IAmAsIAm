// IN THE GAME as a DRAFT (Claude, 2026-11-13; the owner's Doc edits come first). Chapter 7 (Daoism), Part 2.
// vo-speakers: Maya=maya, Lin Daozhang=lin, Auntie Mei=mei
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Qingming. Seattle, April 2029. Maya is 30.
//  Qingming (research): in spring, families tend their ancestors' graves (sweeping, weeding, offering food and flowers).
//  My guesses, for the owner to correct:
//   - Lin Daozhang's family invites Maya to help at Lake View Cemetery on Capitol Hill (a real cemetery, with many Chinese
//     family graves). She can't go to Mira's grave (Oregon), so Lin Daozhang offers the temple's furnace for a letter to her.
//   - Burning a letter is Maya's own act, not a Daoist rite (joss paper is offered to the family's own dead); Lin Daozhang
//     only offers the fire.
// ─────────────────────────────────────────────────────────────────────────────

=== ch7_part2 ===
~ set_age(30)
~ set_date(2029, 4, 7)  // Claude's guess (the Saturday after Qingming, April 4)
# cw: religious-content
# cw: childhood-reference
-> wake

= wake
# place: seattle_apartment
# shot: close
# objective: Get ready to meet the Lins
~ text_from("auntie_mei", "Lake View, 9am, north gate. Wear old shoes. Bring gloves. We bring the food.")
- (room)
+ {room < 3} [Look: The altar]
    Mira's stone. You pick it up and put it in your pocket. Then you take it out and put it back. Then in your pocket again. # look: stone-pocket-again
    -> room
+ {room < 3} [Look: The window]
    Sun, for once, and cherry blossom along the whole street. # look: cherry-blossom
    -> room
* [Use: Your gloves]
-
-> cemetery

= cemetery
# place: lake_view_cemetery
# shot: wide
# objective: Help sweep the graves
- (hill)
+ {hill < 4} [Look: The view]
    The lake below, the mountain out, the city like a toy. The dead have the best view in Seattle. # look: cemetery-view
    -> hill
+ {hill < 4} [Look: The Lin family stone]
    Chinese characters, then English: Lin. A grandmother, a grandfather, an uncle who died young. Oranges and a teacup on the step. # look: lin-stone
    -> hill
+ {hill < 4} [Look: The grandchildren]
    Three teenagers, sweeping and complaining and, when they think nobody's looking, bowing properly. # look: lin-teenagers
    -> hill
* {hill >= 1} [Use: The broom]
    Leaves, moss, a crisp packet. You sweep someone else's grandmother's step until it's clean. # look: sweep-step
    ~ nudge(-0.15)
-
Lin Daozhang: We sweep, we tell them the news, we eat with them. They like to know who got married and who failed their exams. # vo: ch7-daoism_lin_0008
Auntie Mei: Your family is buried far away? # vo: ch7-daoism_mei_0009
* [Talk: Auntie Mei (tell her about Mira)]
    ~ record_choice("ch7.p2.mira", "told", "At Qingming, you told Auntie Mei about Mira, buried in Oregon, who tried.")
    Maya: Not family, exactly. A woman who looked after me when I was little. In Oregon. I never went to her grave. # vo: ch7-daoism_maya_0010
    Auntie Mei: She looked after you, she's family. That's how it works. # vo: ch7-daoism_mei_0011
* [Talk: Auntie Mei (say there's no one)]
    ~ record_choice("ch7.p2.mira", "noone", "At Qingming, you told Auntie Mei there was no one to visit. She gave you an orange anyway.")
    Maya: There's no one, really. # vo: ch7-daoism_maya_0012
    Auntie Mei: Hm. Somebody always. Here. Eat an orange. # vo: ch7-daoism_mei_0013
-
-> picnic

= picnic
# place: lake_view_cemetery
# shot: close
# objective: Eat with the Lins
- (rug)
+ {rug < 2} [Look: The food]
    Roast pork, buns, oranges, a thermos of tea, all laid out for the grandparents first, then for everyone. # look: qingming-picnic
    -> rug
* [Use: A bun]
    You eat on a rug between graves, in the sun, while a teenager explains a video game to his dead grandfather. # look: bun-graves
-
-> furnace

= furnace
# place: cid_temple_hall
# shot: close
# objective: Write to Mira
Lin Daozhang: You can't go to Oregon today. So. The furnace is hot. Write to her. The smoke knows the way. # vo: ch7-daoism_lin_0014
* [Use: The paper (write "Dear Mira")]
    "Dear Mira." Then, for the first time, the rest of it: I'm okay. I have someone to talk to. We're all still here. # look: dear-mira-finished
    ~ honour_ancestor("guide_quaker")
-
* [Use: The furnace]
    ~ record_choice("ch7.p2.letter", "sent", "At Qingming, you finally finished a letter to Mira, and burned it in the temple furnace.")
    The paper curls, glows, goes up. You've been writing that letter since you were eleven. # look: letter-smoke
    ~ nudge(-0.3)
-
Heather (inside): That's the one. That's the one we never sent. # vo: ch7-daoism_heather-inside_0015
-> night

= night
# place: seattle_apartment
# shot: close
# objective: Go home
- (room)
+ {room < 2} [Look: Your hands]
    Moss under your nails, incense in your hair. # look: moss-nails
    -> room
* [Use: The bed]
    For once, the empty drawer where the four unfinished letters were doesn't feel empty. # look: drawer-not-empty
-
-> ch7_part3  // the next part of the same chapter (one run)
