// IN THE GAME as a DRAFT (Claude, 2026-11-14; the owner's Doc edits come first). Chapter 10 (Unitarian Universalism), Part 1.
// vo-speakers: Maya=maya, Anabelle=anabelle, Heather (inside)=heather-inside
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. Every and all. Arcata, October 2032. Maya is 34.
//  Owner: Maya stays in Arcata. The guide is ANABELLE (25, they/them), a member of the fellowship's pagan group. The chapter is
//  about BELONGING: Maya has collected a little of every faith, and this is the first place where that's the point, not a
//  problem. A made-up UU fellowship (not the real one near Arcata). Three seasons. Starts as Maya; unlocks SERENA, who first
//  comes forward when Sarah phones (Part 2).
//
//  Research (docs/Chapter10_UUResearch.md): a UU service opens by lighting the flaming chalice; "joys and concerns" is when
//  people light a candle and share news; coffee hour follows. UUs have no creed: one congregation can hold humanists, Christians,
//  Buddhists, Jews and pagans. CUUPS (the Covenant of UU Pagans) chapters are small groups inside a congregation; many mark the
//  eight sabbats.
//
//  My guesses (placeholders), for the owner to correct:
//   - The fellowship: "the Eleventh Street Fellowship" (Unitarian Universalist), in an old wooden house near the Plaza.
//   - Its pagan circle: "Spiral Fern" (a CUUPS chapter). Anabelle leads it; they grew up UU in Oregon, and study forestry at Cal
//     Poly Humboldt.
//   - The day: Sunday October 31, 2032: the morning service, then the circle's small Samhain that evening.
// ─────────────────────────────────────────────────────────────────────────────

=== ch10_part1 ===
~ set_age(34)
~ set_date(2032, 10, 31)  // a Sunday, and Samhain (Claude's guess)
# cw: religious-content
~ explore_tradition("unitarian-universalism")
~ text_from("tanya", "Samhain blessings, love. Who are you lighting for tonight? I've got Mira's candle going here too.")
-> wake

= wake
# place: arcata_apartment
# shot: close
# objective: Decide about Sunday
- (room)
+ {room < 3} [Look: The sill]
    Mira's stone. The prayer wheel. The khata. Bríd's jar, empty now, holding pens. Maisie's drawing of the marsh. # look: sill-crowded
    -> room
+ {room < 3} [Look: The flyer on the fridge]
    "ELEVENTH STREET FELLOWSHIP. Unitarian Universalist. Sundays 10:30. Whoever you are, whatever you believe, you're welcome." Heather put it there. Or you did. # look: uu-flyer
    -> room
+ {room < 3} [Look: The bookshelf]
    A Book of Shadows, a Dhammapada, a siddur from Rabbi Abe, a Qur'an in English from Amal, a Gita, a Tao Te Ching with tea stains. Nobody's shelf looks like this. # look: shelf-every-faith
    -> room
* [Use: Your coat (go)]
-
Heather (inside): Go on. It says whoever you are. That's us. # vo: ch10-unitarian-universalism_heather-inside_0001
-> fellowship

= fellowship
# place: uu_fellowship
# shot: wide
# objective: Find a seat
- (hall)
+ {hall < 4} [Look: The chalice]
    A small stone bowl on a stand, a flame in it. A teenager lights it while everyone reads the words on the screen together. # look: flaming-chalice
    -> hall
+ {hall < 4} [Look: The people]
    An old man in a bow tie saying the Lord's Prayer under his breath at the wrong moment. A woman with a meditation cushion under her arm. Two dads with a baby. Someone in a Darwin T-shirt. # look: uu-people
    -> hall
+ {hall < 4} [Look: The banner]
    Seven words on a felt banner, round a heart: Justice, Equity, Transformation, Pluralism, Interdependence, Generosity, Love. # look: uu-values-banner
    -> hall
* {hall >= 1} [Go: A chair at the back]
-
-> joys

= joys
# place: uu_fellowship
# shot: over-shoulder
# objective: Watch the candles
- (candles)
+ {candles < 3} [Look: The candle table]
    "Joys and concerns." People go up one by one, light a little candle, and say something out loud. A new grandchild. A biopsy, results Tuesday. A cat named Pancake, who died. # look: joys-concerns
    -> candles
+ {candles < 3} [Look: The minister]
    A small woman with grey hair and a hearing aid, who says "thank you" after every single one and seems to mean it every time. # look: uu-minister
    -> candles
* [Wait (just watch)]
    ~ record_choice("ch10.p1.joys", "watched", "On your first Sunday at the fellowship, you watched the joys and concerns from the back row and didn't go up.")
    You don't go up. It's enough to watch people say true things out loud and not be hushed. # look: back-row-watch
* [Use: A candle (light one)]
    ~ record_choice("ch10.p1.joys", "candle", "On your first Sunday at the fellowship, you went up and lit a candle at the joys and concerns, for Mira, because it was Samhain. You didn't say anything. Nobody needed you to.")
    You light one. You don't say anything. The minister says "thank you" anyway, as if you had. # look: candle-silent
    ~ nudge(-0.2)
-
-> coffee

= coffee
# place: uu_fellowship
# shot: close
# objective: Survive coffee hour
- (cups)
+ {cups < 3} [Look: The coffee]
    Fair trade, too strong, in mugs that don't match. A plate of somebody's lemon bars, already half gone. # look: coffee-hour
    -> cups
+ {cups < 3} [Look: The noticeboard]
    Buddhist sitting group, Tuesdays. Jewish UU Hanukkah potluck. Humanist book club. "SPIRAL FERN: our Samhain circle tonight, 7pm, the garden. All welcome. Bring a name." # look: uu-noticeboard
    -> cups
* {cups >= 1} [Talk: Anabelle]
-
Anabelle: Hi. You looked at our flyer for a really long time. I'm Anabelle, they/them. I run the pagan circle. Mostly I bring the snacks. # vo: ch10-unitarian-universalism_anabelle_0002
Maya: Maya. She/her. I'm... I'm sort of a lot of things. Wiccan, mostly. But I've been to a gurdwara and a Daoist temple and a holy well in Ireland, and I still pray with a rosary sometimes, and I don't... # vo: ch10-unitarian-universalism_maya_0003
Anabelle: Oh, you're going to fit in so well here. Half this room is a lot of things. # vo: ch10-unitarian-universalism_anabelle_0004
* [Talk: Anabelle (ask how that works)]
    ~ record_choice("ch10.p1.anabelle", "asked", "At your first coffee hour, you asked Anabelle how a church works with no creed. They said: we promise how we'll be with each other, not what we'll believe.")
    Maya: But how does that work? A church with no creed? # vo: ch10-unitarian-universalism_maya_0005
    Anabelle: We don't promise to believe the same things. We promise how we'll treat each other. It's called a covenant. It's messy. It's my favourite thing in the world. # vo: ch10-unitarian-universalism_anabelle_0006
* [Talk: Anabelle (say you might come tonight)]
    ~ record_choice("ch10.p1.anabelle", "tonight", "At your first coffee hour, you told Anabelle you might come to the Samhain circle that night.")
    Maya: I might come tonight. To the circle. # vo: ch10-unitarian-universalism_maya_0007
    Anabelle: Bring a name. Someone you want to remember. That's the whole thing. # vo: ch10-unitarian-universalism_anabelle_0008
-
-> circle

= circle
# place: uu_fellowship_garden
# shot: wide
# objective: Join the circle
- (ring)
+ {ring < 3} [Look: The garden]
    Behind the fellowship: raised beds gone to seed, a plum tree, fairy lights strung on the fence, eight people in a ring on the damp grass. # look: uu-garden
    -> ring
+ {ring < 3} [Look: The altar]
    A card table with a cloth. Apples. Photos. A jar of marigolds. A little plastic dinosaur someone's kid insisted on. # look: samhain-card-table
    -> ring
* {ring >= 1} [Talk: Anabelle]
-
Anabelle: We go round. Say a name if you want to. Or say "pass". Pass is a whole sentence here. # vo: ch10-unitarian-universalism_anabelle_0009
* [Talk: The circle (say Mira's name)]
    ~ record_choice("ch10.p1.samhain", "mira", "At the Spiral Fern's Samhain circle, you said Mira's name again, and eight strangers said it back.")
    Maya: Mira Smith. # vo: ch10-unitarian-universalism_maya_0010
    Eight strangers say her name back to you, softly, all together. # look: name-said-back
* [Wait (say "pass")]
    ~ record_choice("ch10.p1.samhain", "pass", "At the Spiral Fern's Samhain circle, you said 'pass', and nobody minded at all.")
    Maya: Pass. # vo: ch10-unitarian-universalism_maya_0011
    Anabelle nods like you've said something important. Maybe you have. # look: pass-nod
-
~ nudge(-0.2)
-> night

= night
# place: arcata_apartment
# shot: close
# objective: Go to bed
* [Use: The bed]
    Your feet still cold from the grass. Lemon bar crumbs in your coat pocket. A pamphlet: "So you're new here." # look: uu-pamphlet-bed
-
~ text_to("tanya", "I went to a Unitarian church. There's a pagan circle there. Someone said Mira's name back to me. Eight people did.")
~ text_from("tanya", "Oh, the UUs! Half my coven came through a CUUPS circle at some point. You'll be fine there. You'll be more than fine.")
~ journal("Note", "First Sunday at the Eleventh Street Fellowship. A church with no creed: they promise how they'll treat each other, not what they'll believe. Anabelle. Pass is a whole sentence.")
-> ch10_part2  // the next part of the same chapter (one run)
