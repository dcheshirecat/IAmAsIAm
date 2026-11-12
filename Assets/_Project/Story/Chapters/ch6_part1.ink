// IN THE GAME as a DRAFT (Claude, 2026-11-12: the owner said "continue" before answering the Chapter 6 questions, so this is
// written on Claude's guesses, every one marked below. The owner's answers and Doc edits come first). Chapter 6 (Sikhism), Part 1.
// vo-speakers: Maya=maya, Gurleen=gurleen, Bibi Harjit=harjit
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. Langar. Seattle and Kent, March 2028. Maya is 29.
//  Owner's beats so far: Christie, the old friend from work, tells Maya that Adeena married Michael, and it's triggering (they
//  shared the Shabbat candles every Friday). Maya is in Seattle (since Chapter 5).
//
//  My guesses, for the owner to correct (I asked; these are placeholders until you answer):
//   - Where/when: still Seattle, 2028. The gurdwara is in Kent, south of Seattle (fictional; the area has several real ones).
//   - The guide: BIBI HARJIT KAUR (`bibi_harjit`, placeholder name, ~70, she/her), who has run the Sunday langar kitchen for
//     thirty-five years. "Bibi ji" is how people speak to an older woman. Langar (the free community kitchen) and seva
//     (selfless service) are the most open parts of Sikhi: anyone may eat, anyone may help.
//   - Maya gets there through GURLEEN (`gurleen`, placeholder name, she/her), a colleague at the Burke. Gurleen's family shows
//     how many ways there are to be Sikh: she cuts her hair and comes for the food and her grandmother; her father wears a
//     turban and keeps his hair, but isn't initiated; her brother Arjan (19) took amrit (joined the Khalsa) last year, his own
//     choice, and wears the five Ks.
//   - Starts as Maya and unlocks nobody (owner: Serena and Lily are left; tell me if one of them belongs here).
//  What a visitor does (research: Nesbitt 2016, and gurdwara visitor guides): cover the head, shoes off, wash hands; in the
//  darbar hall, bow to the Guru Granth Sahib (the scripture, the living Guru) if you wish, sit on the floor; karah prasad in
//  cupped hands; in langar sit in rows (pangat) on the floor as equals, take only what you'll eat.
// ─────────────────────────────────────────────────────────────────────────────

=== ch6_part1 ===
~ set_age(29)
~ set_date(2028, 3, 4)  // Claude's guess (the Saturday family day; the gurdwara is the next day)
# cw: religious-content
~ explore_tradition("sikhism")
~ co_conscious("heather", false)
~ text_from("tanya", "How's the rain? How are all of you? I pulled the Star for you this morning. Take that however you like.")
-> burke

= burke
# place: burke_museum_hall
# shot: wide
# objective: Close up after the family day
- (hall)
+ {hall < 3} [Look: The craft tables]
    Glue sticks, paper whales, one small shoe. Two hundred children came through today and you were here for every one of them. # look: craft-tables
    -> hall
+ {hall < 3} [Look: The whale]
    Still hanging there, every bone numbered. You say goodnight to it now. Nobody has caught you yet. # look: burke-whale
    -> hall
* {hall >= 1} [Talk: Gurleen]
-
Gurleen: You were amazing with the dinosaur kid. I'd have cried. I'm going to my nani's gurdwara tomorrow. Do you want to come? There's food. There is always food. # vo: ch6-sikhism_gurleen_0001
* [Talk: Gurleen (say yes)]
    ~ record_choice("ch6.p1.gurleen", "yes", "After the family day at the Burke, you said yes to Gurleen's grandmother's gurdwara.")
    Maya: Yes. I'd love to. What do I need to know? # vo: ch6-sikhism_maya_0002
* [Talk: Gurleen (ask if it's okay)]
    ~ record_choice("ch6.p1.gurleen", "okay", "After the family day at the Burke, you asked Gurleen if it was okay for you to come to the gurdwara. She laughed.")
    Maya: Is it okay? For someone who isn't Sikh? # vo: ch6-sikhism_maya_0003
    Gurleen: The kitchen feeds anyone who walks in. That's the whole point. Bring a scarf. # vo: ch6-sikhism_gurleen_0004
-
-> sunday

= sunday
// Owner: a full day in every part. Claude's scenes: the Sunday from waking up to bed.
~ set_date(2028, 3, 5)
# place: seattle_apartment
# shot: close
# objective: Be ready when Gurleen comes
~ text_from("gurleen", "outside in 10!! dont eat breakfast. i mean it. nani will be offended")
- (room)
+ {room < 3} [Look: The scarf]
    Gurleen's blue scarf, folded on the chair. You practise tying it in the mirror. It looks like a scarf someone else tied. # look: scarf-practice
    -> room
+ {room < 3} [Look: The windowsill]
    Mira's stone, the prayer wheel, the khata, a dry marigold from the mandir. The sill is getting crowded. # look: sill-crowded
    -> room
* [Use: Your keys]
-
-> drive

= drive
# place: gurleen_car
# shot: over-shoulder
# objective: Ride to Kent
- (car)
+ {car < 3} [Look: The road]
    I-5 in the rain, the warehouses, the mountain hiding. Forty minutes south. # look: i5-rain
    -> car
+ {car < 3} [Look: Gurleen]
    Singing along to Punjabi pop at full volume, badly, happily, drumming on the wheel. # look: gurleen-singing
    -> car
* [Talk: Gurleen (ask what to do)]
    "Shoes off, head covered, wash your hands, sit on the floor. If you don't know, copy nani. Everybody copies nani." # look: gurleen-rules
-
-> darbar

= darbar
// The darbar hall: the Guru Granth Sahib on a raised platform under a canopy, wrapped in cloth (rumalas); the granthi sits behind
// it and waves the chaur (a whisk) over it. Kirtan: hymns from it, sung with harmonium and tabla. Ik Onkar: One God.
# place: gurdwara_darbar_hall
# shot: wide
# objective: Cover your head and go in
- (hall)
+ {hall < 5} [Look: The shoe room]
    Racks of shoes, and a basket of orange scarves for anyone who forgot. You tie the blue one Gurleen lent you. Water for your hands, cold. # look: gurdwara-shoes
    -> hall
+ {hall < 5} [Look: The Guru Granth Sahib]
    A book, under a canopy, on cushions, wrapped in embroidered cloth. A man sits behind it and slowly waves a white whisk over it, the way you'd keep someone cool. It isn't treated like a book. It's treated like someone. # look: guru-granth-sahib
    -> hall
+ {hall < 5} [Look: The canopy]
    One symbol stitched in gold on the canopy: Ik Onkar. Gurleen whispers: One. There is One. Everything. # look: ik-onkar
    -> hall
+ {hall < 5} [Look: The singers]
    Three men on a low stage: a harmonium, a pair of tabla drums, voices that go up and up. You don't know the words. Your shoulders come down anyway. # look: kirtan
    -> hall
+ {hall < 5} [Look: Gurleen's family]
    Her nani, small and fierce, in white. Her dad in a navy turban. Her little brother Arjan, in a turban too, with a steel bracelet and a short sword in a sheath at his side. "He took amrit last year," Gurleen whispers. "His choice. I cut my hair. We're all Sikh." # look: gurleen-family
    -> hall
* {hall >= 2} [Wait (sit on the floor with everyone)]
-
~ set_experience("Derealization", 0.15)
At the end everyone stands for a long prayer, hands together. A sweet, warm and heavy with ghee, pressed into your cupped hands. # look: karah-prasad
Gurleen: Karah prasad. Eat it all. Now: langar. # vo: ch6-sikhism_gurleen_0005
-> langar

= langar
# place: gurdwara_langar_hall
# shot: wide
# objective: Sit down for langar
- (hall)
+ {hall < 3} [Look: The rows]
    Long mats on the floor, rows and rows. A doctor next to a truck driver next to a toddler next to you. Nobody sits higher. # look: pangat
    -> hall
+ {hall < 3} [Look: The servers]
    Teenagers and grandfathers with buckets of dal and baskets of roti, walking the rows. "Waheguru" means more, please. A hand up means enough. # look: langar-servers
    -> hall
* {hall >= 1} [Wait (hold out your hands for a roti)]
-
Warm roti in your two hands. Dal. Something with potatoes. Rice pudding. Nobody asked who you are, or what you believe, or whether you deserve it. # look: langar-plate
~ nudge(-0.15)
* [Use: The plate (eat slowly)]
    ~ record_choice("ch6.p1.langar", "slowly", "At your first langar, you ate slowly, on the floor, between strangers.")
    Every bite. Your knees on the mat, the hum of a hundred people eating. You're here. # look: langar-eat
* [Wait (watch the kitchen door)]
    ~ record_choice("ch6.p1.langar", "kitchen", "At your first langar, you kept looking at the kitchen door, where the noise and the steam came from.")
    The swinging door. Steam, laughing, a woman's voice giving orders in Punjabi over all of it. # look: kitchen-door
-
-> kitchen

= kitchen
# place: gurdwara_kitchen
# shot: over-shoulder
# objective: Take your plate back
- (k)
+ {k < 3} [Look: The pots]
    Pots big enough to bathe in. A man stirring one with a paddle. # look: langar-pots
    -> k
+ {k < 3} [Look: The roti table]
    Six women in a row, rolling, flipping, slapping dough from hand to hand. Not one of them looks down. # look: roti-table
    -> k
* {k >= 1} [Talk: Bibi Harjit]
-
Bibi Harjit: You. New girl with Gurleen. You ate? Good. You want to help? Wash your hands. Sit. # vo: ch6-sikhism_harjit_0006
* [Talk: Bibi Harjit (sit down)]
    ~ record_choice("ch6.p1.seva", "helped", "Bibi Harjit put a rolling pin in your hand at your first langar, and you sat down at the roti table.")
    Maya: Okay. Show me. # vo: ch6-sikhism_maya_0007
* [Talk: Bibi Harjit (say you'll ruin them)]
    ~ record_choice("ch6.p1.seva", "nervous", "At your first langar you told Bibi Harjit you'd ruin the rotis. She told you to ruin some, then.")
    Maya: I'll ruin them. I've never made one. # vo: ch6-sikhism_maya_0008
    Bibi Harjit: So ruin some. The hungry won't mind. # vo: ch6-sikhism_harjit_0009
-
A ball of dough. A small board. A rolling pin. Yours comes out shaped like Ohio. # look: maya-roti
Bibi Harjit: Turn it. Press, turn, press, turn. Don't think. Say Waheguru if you want, the hands like it. # vo: ch6-sikhism_harjit_0010
* [Use: The rolling pin (press and turn)]
    ~ record_choice("ch6.p1.roti", "turned", "At your first seva, you pressed and turned until the roti was nearly round.")
    Press. Turn. Press. Turn. The board, the flour, the warmth from the griddle. Nearly round. # look: roti-round
    ~ nudge(-0.2)
* [Use: Mira's stone (feel it in your pocket first)]
    ~ record_choice("ch6.p1.roti", "stone", "At your first seva, you touched Mira's stone in your pocket before you started rolling.")
    Your finger through the hole. Then the flour. Then press, turn. # look: stone-flour
    ~ nudge(-0.2)
-
Heather (inside): Good hands. Look at that. # vo: ch6-sikhism_heather-inside_0011
Bibi Harjit: Next Sunday, seven o'clock. The onions need someone. # vo: ch6-sikhism_harjit_0012
~ journal("Note", "Langar in Kent. Scarf, shoes off, wash hands. Sit in a row with everyone. Waheguru = more, please. Bibi Harjit: next Sunday, seven, onions.")
~ text_to("tanya", "I rolled forty rotis today for strangers. A woman called Bibi Harjit told me not to think. It worked.")
~ text_from("tanya", "Forty! Kitchen magic is the oldest kind. Go back.")
-> evening

= evening
# place: seattle_apartment
# shot: close
# objective: Get home
- (room)
+ {room < 2} [Look: Your hands]
    Flour still in the creases of your knuckles. You don't wash it off yet. # look: flour-knuckles
    -> room
* [Use: The fridge (Gurleen's nani's leftovers)]
    Three foil packets Gurleen's nani pressed on you at the door. Dal, sabzi, two rotis. Dinner, and lunch tomorrow. # look: nani-foil
-
* [Use: The bed]
    Your feet ache from the kitchen floor in a good way. Press, turn, press, turn, until you're asleep. # look: press-turn-sleep
-
-> ch6_part2  // the next part of the same chapter (one run)
