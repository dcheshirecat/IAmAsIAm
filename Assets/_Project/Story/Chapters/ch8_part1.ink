// IN THE GAME as a DRAFT (Claude, 2026-11-13. The owner's Doc edits come first). Chapter 8 (Druidry), Part 1.
// vo-speakers: Maya=maya, Bríd=brid, Tanya=tanya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. Bealtaine. County Westmeath, Ireland, May 2030. Maya is 31.
//  Owner: a trip to Ireland, several weeks across the seasons. The guide leads a grove (an older Irish woman). Tanya knows
//  the grove. Parts built around Bealtaine at Uisneach, Lúnasa and Samhain at Tlachtga. Lily first comes forward waiting at a
//  holy well. The chapter unlocks Lily. A full day in every part.
//
//  My guesses (placeholders), for the owner to correct:
//   - Maya takes unpaid leave from the Burke and comes three times: a few weeks each at Bealtaine, Lúnasa and Samhain, staying in
//     the spare room of the grove's leader.
//   - The leader: BRÍD KENNY (`brid_kenny`, 66, she/her): sheep farmer on the slopes below the Hill of Uisneach, a retired
//     national school teacher, an Irish speaker. She and Tanya met at a gathering in Glastonbury in 2004 and write every Samhain.
//   - Her grove (fictional): Fuinseog Grove (fuinseog: ash tree, in Irish). Members show different streams of modern Druidry: an
//     OBOD member from Dublin who uses the Welsh festival names (Alban Eilir...), a Gaelic polytheist who uses only the Irish names
//     and honours Lugh and Brigid, and Bríd's Catholic neighbour, who comes for the fire and goes to Mass after.
//   - Uisneach (research): the old ceremonial centre of Ireland. A public Bealtaine fire festival is held there each year.
// ─────────────────────────────────────────────────────────────────────────────

=== ch8_part1 ===
~ set_age(31)
~ set_date(2030, 4, 30)  // Bealtaine eve (Claude's guess at the day the part is set)
# cw: religious-content
~ explore_tradition("druidry")
~ text_from("tanya", "Bríd will meet you at Mullingar. She'll be in a green Land Rover with a dog in it. Do what she says. I mean that lovingly.")
-> wake

= wake
# place: brid_farmhouse
# shot: close
# objective: Find the kitchen
- (room)
+ {room < 3} [Look: The window]
    Green. More green than you knew there was. Sheep on the hill, and fog sitting in the dip like milk in a saucer. # look: westmeath-window
    -> room
+ {room < 3} [Look: The clock]
    6:10 a.m. Jet lag, and a sheep under the window explaining something at length. # look: jetlag-sheep
    -> room
* [Go: The kitchen]
-
-> kitchen

= kitchen
# place: brid_kitchen
# shot: wide
# objective: Have breakfast
- (k)
+ {k < 3} [Look: The range]
    An old cream stove, warm all day, a kettle that never quite stops. A collie asleep against it. # look: range-collie
    -> k
+ {k < 3} [Look: The dresser]
    Plates, a Brigid's cross of rushes over the door, a photo of a younger Bríd with Tanya, both laughing in the rain. # look: dresser-photo
    -> k
* [Talk: Bríd]
-
Bríd: You're up. Good. Tea. Brown bread. Then the lambs, then the May bush, then the fire. Bealtaine waits for nobody. # vo: ch8-druidry_brid_0001
Maya: Tanya said to do what you say. # vo: ch8-druidry_maya_0002
Bríd: Tanya was always a clever one. Butter on that. More. # vo: ch8-druidry_brid_0003
-> lambs

= lambs
# place: brid_field
# shot: wide
# objective: Help with the lambs
- (field)
+ {field < 3} [Look: The lambs]
    A dozen of them, all knees, bouncing straight up in the air for no reason at all. # look: lambs-bouncing
    -> field
+ {field < 3} [Look: The hill]
    Uisneach, low and green above the farm. Bríd: "The navel of Ireland. Everything starts there." # look: uisneach-hill
    -> field
* [Use: The bottle (feed the orphan lamb)]
    ~ record_choice("ch8.p1.lamb", "fed", "On Bealtaine morning you bottle-fed an orphan lamb in Bríd's field. It nearly knocked you over.")
    Warm milk, a small hard head shoving at your knees. Your hands are full and you are entirely here. # look: lamb-bottle
    ~ nudge(-0.3)
-
-> maybush

= maybush
# place: brid_farmhouse
# shot: wide
# objective: Dress the May bush
- (bush)
+ {bush < 3} [Look: The hawthorn]
    A small hawthorn by the gate, all white blossom. You don't cut hawthorn, Bríd says. You dress it, and you leave it be. # look: hawthorn-blossom
    -> bush
+ {bush < 3} [Look: The basket]
    Ribbons, eggshells painted yellow, scraps of cloth: everyone in the grove brought something. # look: ribbons-basket
    -> bush
* [Use: A ribbon (tie it on)]
    ~ record_choice("ch8.p1.maybush", "ribbon", "On Bealtaine you tied a ribbon on the May bush at Bríd's gate.")
    A blue ribbon, the colour of Mira's sky in the fellowship hall window. You tie it on a low branch. # look: blue-ribbon
-
Bríd: The grove comes at seven. You'll meet them all. One calls it Alban Eilir and one will correct him, and my neighbour Josie comes for the fire and goes to Mass after. We are all Druids, more or less. Josie less. # vo: ch8-druidry_brid_0004
-> fire

= fire
# place: uisneach_hill
# shot: wide
# objective: Climb to the fire
- (hill)
+ {hill < 4} [Look: The crowd]
    Hundreds of people on the hill in the dusk: families, drummers, someone in antlers, teenagers on their phones. # look: uisneach-crowd
    -> hill
+ {hill < 4} [Look: The grove]
    Bríd's grove in a ring a little apart: twelve people in ordinary rain jackets, one in a white robe, all holding hands. # look: grove-ring
    -> hill
+ {hill < 4} [Look: The Catstone]
    A great split boulder on the slope. Bríd says it's where the provinces meet. Everybody touches it on the way past. # look: catstone
    -> hill
* {hill >= 2} [Wait (for the fire)]
-
The fire goes up all at once, a roar, a tower of orange, and every face on the hill is lit. # look: bealtaine-fire
~ set_experience("Derealization", 0.3)
Bríd: We call the quarters, and then we say what we're bringing into summer. You don't have to. You can just stand here and be warm. # vo: ch8-druidry_brid_0005
* [Talk: The grove (say what you're bringing)]
    ~ record_choice("ch8.p1.fire", "spoke", "At the Bealtaine fire on Uisneach you told the grove what you were bringing into summer: all of you.")
    Maya: I'm bringing all of us. Everyone. Whoever turns up. # vo: ch8-druidry_maya_0006
    Bríd: Then everyone's welcome. # vo: ch8-druidry_brid_0007
* [Wait (be warm)]
    ~ record_choice("ch8.p1.fire", "warm", "At the Bealtaine fire on Uisneach you didn't say anything. You stood and were warm.")
    The heat on your face, the cold on your back. You stand there and are warm. # look: warm-front-cold-back
-
~ nudge(-0.2)
-> night

= night
# place: brid_farmhouse
# shot: close
# objective: Go to bed
* [Use: The bed]
    Two in the morning. Smoke in your hair, mud to your knees, a hot water bottle Bríd left without saying. You're asleep before you can be grateful. # look: hot-water-bottle
-
~ text_to("tanya", "I fed a lamb. I stood by the biggest fire I've ever seen. Bríd says hello and that you owe her a letter.")
~ text_from("tanya", "I do. Tell her it's coming. Tell her it's been coming since 2019.")
~ journal("Note", "Bealtaine at Uisneach. A May bush, a blue ribbon. What I'm bringing into summer: all of us.")
-> ch8_part2  // the next part of the same chapter (one run)
