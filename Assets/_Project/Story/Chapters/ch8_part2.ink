// IN THE GAME as a DRAFT (Claude, 2026-11-13. The owner's Doc edits come first). Chapter 8 (Druidry), Part 2.
// vo-speakers: Maya=maya, Bríd=brid, Lily=lily
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Lúnasa. County Westmeath, August 2030. Maya is 31.
//  Owner: Lily (4, she/her, Quaker. Holds the waiting for a mother who was always with someone else) first comes forward
//  waiting at a holy well. This is a scripted switch. She is unlocked at the end of the chapter (owner).
//  Research: Lúnasa (1 August) is the start of the harvest. Customs include hilltop gatherings and picking bilberries
//  ("fraughans"). Holy wells: people walk "rounds" sunwise around the well, pray or leave a rag on the tree beside it, and wait.
//  My guesses, for the owner to correct:
//   - The well is fictional ("Brigid's well at the bottom of the lane"), visited by Catholic neighbours and Druids alike.
//   - Bríd's line "I'm good at waiting" echoes Mira's on Sunday 1 on purpose. Nobody points it out.
// ─────────────────────────────────────────────────────────────────────────────

=== ch8_part2 ===
~ set_age(31)
~ set_date(2030, 8, 1)  // Lúnasa (Claude's guess)
# cw: religious-content
# cw: childhood-reference
# cw: involuntary-switch
-> wake

= wake
# place: brid_kitchen
# shot: wide
# objective: Help with the bread
- (k)
+ {k < 3} [Look: The table]
    Flour everywhere. The first bread from the new wheat, Bríd says: Lúnasa bread. You knead until your arms ache. # look: lunasa-bread
    -> k
+ {k < 3} [Look: The window]
    August. The fields gone gold, swallows on the wire, rain coming and going like it's thinking about it. # look: august-window
    -> k
* [Use: The dough (knead it)]
    Push, fold, turn. Like the rotis in Kent. Your hands remember. # look: knead-remember
-
-> hill

= hill
# place: uisneach_hill
# shot: wide
# objective: Pick bilberries on the hill
- (slope)
+ {slope < 3} [Look: The bilberries]
    Small, dark, hiding under the leaves. Your fingers go purple. Bríd's grandchildren eat more than they keep. # look: fraughans
    -> slope
+ {slope < 3} [Look: The view]
    Twenty counties, Bríd says, on a clear day. Today you can see about three, and rain over the fourth. # look: three-counties
    -> slope
* [Use: The basket (fill it)]
    A whole basket. Bríd says that's a good harvest coming. You don't ask how she knows. # look: full-basket
-
-> well

= well
# place: holy_well
# shot: close
# objective: Walk to the well
- (spring)
+ {spring < 4} [Look: The tree]
    A hawthorn bent over the water, hung with rags, ribbons, a rosary, a child's sock, a hospital bracelet. Every one of them someone hoping. # look: rag-tree
    -> spring
+ {spring < 4} [Look: The water]
    Clear, cold, a little stone basin. A tin cup on a chain. # look: well-water
    -> spring
+ {spring < 4} [Look: The woman doing her rounds]
    Josie, Bríd's neighbour, walking slowly round and round the well with her beads, lips moving. She nods at you each time she passes. # look: josie-rounds
    -> spring
* {spring >= 2} [Wait (by the well)]
-
Bríd: You wait for the water to settle. Then you ask. There's no hurry on it. # vo: ch8-druidry_brid_0008
~ set_experience("Depersonalization", 0.5)
The water, settling. A woman far off calling a child's name. Small. You feel very small. # look: very-small
# switch: lily
Lily: Is she coming back? # vo: ch8-druidry_lily_0009
Bríd: Who, pet? # vo: ch8-druidry_brid_0010
Lily: Mommy. She said five minutes. I'm being very good. # vo: ch8-druidry_lily_0011
Bríd: You are. You're being very good. I'll wait with you, so. I'm good at waiting. # vo: ch8-druidry_brid_0012
* [Use: The tin cup]
    ~ record_choice("ch8.p2.lily", "cup", "At the holy well, Lily drank from the tin cup while Bríd waited with her.")
    The cold water. Both hands round the cup. Bríd's hand, close, not touching. # look: lily-cup
* [Use: A ribbon (for the tree)]
    ~ record_choice("ch8.p2.lily", "ribbon", "At the holy well, Lily tied a ribbon on the rag tree, with Bríd's help.")
    A ribbon, tied in a bow, very carefully, very good. # look: lily-ribbon
* [Wait (with Bríd)]
    ~ record_choice("ch8.p2.lily", "waited", "At the holy well, Lily waited with Bríd, and for once someone waited too.")
    Two of you, waiting. Nobody leaves. # look: two-waiting
-
Lily: You stayed. # vo: ch8-druidry_lily_0013
Bríd: I did, pet. # vo: ch8-druidry_brid_0014
# switch: maya
~ co_conscious("lily", true)
~ nudge(-0.3)
-> evening

= evening
# place: brid_kitchen
# shot: close
# objective: Have dinner
- (k)
+ {k < 2} [Look: Bríd]
    Making tea as if nothing happened. Then, very quietly: "Who was the little one, at the well?" # look: brid-asks
    -> k
* [Talk: Bríd (tell her)]
    ~ record_choice("ch8.p2.told", "told", "After the well, you told Bríd about the system. She said: then she's welcome too.")
    You tell her. All of it, as much as you can. She listens with her tea going cold. # look: tell-brid
-
Bríd: Then she's welcome too. They all are. The house has room. # vo: ch8-druidry_brid_0015
Maya: She's been waiting since she was four. Since I was four. # vo: ch8-druidry_maya_0016
Lily (inside): The lady stayed. # vo: ch8-druidry_lily-inside_0017
-> night

= night
# place: brid_farmhouse
# shot: close
# objective: Go to bed
* [Use: The bed]
    Blackberry and bilberry crumble, a blanket, the collie at the foot of the bed. Somebody small inside, finally asleep. # look: collie-bed
-
~ system_log("maya", "Lily came out at the well today. She asked if Mom was coming back. Bríd waited with her. Hello, Lily. We're here. -M")
-> ch8_part3  // the next part of the same chapter (one run)
