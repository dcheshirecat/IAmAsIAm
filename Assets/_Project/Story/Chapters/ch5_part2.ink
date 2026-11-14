// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 5 (Hinduism), Part 2.
// vo-speakers: Maya=maya, Mangala Tai=mangala
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Every day. Seattle, June to September 2027. Maya is 28-29.
//  The temple priest's life, and the deity's day. Maya grounded. Heather visits.
//
//  My guesses, for the owner to correct:
//   - Maya volunteers on Saturdays: making flower garlands for the shrines with Lakshmi (Priya's mother, owner's name) and the other aunties.
//   - The deity's day, which the priest keeps every day: waking at dawn with a song (kakad aarti), bathing (abhishek),
//     dressing in silk and flowers, offering food (bhog), lamps, putting to rest at night (shayan aarti). Caring for someone,
//     every single day.
//   - Mangala Tai's story (owner: a woman priest): her father was a priest and taught her brothers. She learned the chants by
//     listening at the door. After raising her children she trained as a purohita in Pune. Some families still won't have a
//     woman do their rites. Many now ask for her because she explains every word. Her grandchildren call from Pune on Sundays.
//   - Shiva on Mondays: milk poured over the lingam, the dark stone.
// ─────────────────────────────────────────────────────────────────────────────

=== ch5_part2 ===
~ set_age(28)
~ set_date(2027, 6, 19)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
~ co_conscious("heather", false)
-> wake

= wake
// Owner: a full day in every part. Claude's scenes: a Saturday.
# place: seattle_apartment
# shot: close
# objective: Get to the temple by nine
- (room)
+ {room < 3} [Look: The window]
    June, and the sun out at five in the morning. Seattle in summer is a different city. # look: june-sun
    -> room
+ {room < 3} [Look: The altar]
    Your altar, and next to it, a small framed Ganesha Priya's mother gave you. They get along. # look: altar-ganesha
    -> room
* [Use: Your keys]
-
-> garlands

= garlands
# place: eastside_mandir_kitchen
# shot: over-shoulder
# objective: Help with the garlands
Lakshmi: Thread, needle, flower, flower, flower, knot. Tight, or Ganesha will lose his necklace in the middle of the aarti. # vo: ch5-hinduism_lakshmi_0012
- (table)
+ {table < 4} [Look: The flowers]
    Marigolds, orange and yellow, a mountain of them. Jasmine in a wet cloth. Roses with their stems cut off. # look: garland-flowers
    -> table
+ {table < 4} [Look: The aunties]
    Five women, threading faster than you can see, arguing happily in three languages about a wedding. # look: garland-aunties
    -> table
* {table >= 1} [Use: The needle (make a garland)]
    ~ record_choice("ch5.p2.garland", "made", "On your first Saturday at the temple, you made a marigold garland. It was crooked. It went on Ganesha anyway.")
    Flower, flower, flower, knot. Crooked. Lakshmi looks at it, and laughs, and says Ganesha likes crooked things. # look: crooked-garland
-
// Heather visits: she shares time, quietly, while the hands work.
~ co_conscious("heather", true)
Heather (inside): Let me. Just for a bit. I like this. # vo: ch5-hinduism_heather-inside_0013
~ set_experience("CoConsciousness", 0.6)
* [Wait (let Heather's hands work)]
    ~ record_choice("ch5.p2.heather", "shared", "While you threaded garlands, Heather shared your hands for a while. The garlands came out straight.")
    Your hands, and Heather's, threading. Straight now. Steady. You're both here. Nobody goes far away. # look: shared-hands
-
~ system_log("heather", "Garlands are good for us. Every Saturday, please. -H")
~ co_conscious("heather", false)
-> afternoon

= afternoon
# place: seattle_apartment
# shot: close
# objective: Have a slow evening
- (room)
+ {room < 2} [Look: Your fingers]
    Orange from the marigolds. Heather's work, mostly. # look: orange-fingers
    -> room
* [Use: The stove (dal)]
    Lakshmi's dal recipe, written on the back of a temple flyer. It's nearly right. # look: lakshmi-dal
-
* [Use: The bed]
    Light still in the sky at ten. You fall asleep before it's dark. # look: light-at-ten
-
-> lammas

= lammas
// Owner: every sabbat played at least once across the game (Claude's draft scene). No new voiced lines.
~ set_date(2027, 8, 1)
# place: seattle_apartment
# shot: over-shoulder
# objective: Bake bread for Lammas
- (kitchen)
+ {kitchen < 3} [Look: The dough]
    It rose overnight under a tea towel. Somebody already knocked it back: neat, careful knuckle marks. # look: lammas-dough
    -> kitchen
+ {kitchen < 3} [Look: The window]
    Rain, in August. The plant on the sill drinking it in. # look: lammas-rain
    -> kitchen
* [Use: The oven (bake the loaf)]
    ~ record_choice("ch5.p2.lammas", "baked", "At Lammas, you baked bread and left the first slice for the earth.")
    The kitchen smells like every good thing at once. The first slice, saved for the earth. # ritual: sabbat lammas
* [Wait (let it bake)]
    ~ record_choice("ch5.p2.lammas", "waited", "At Lammas, you let the bread bake and sat on the kitchen floor in the warm.")
    You sit on the kitchen floor in the warm, and wait. # look: lammas-floor
-
~ set_date(2027, 9, 10)
-> day

= day
# place: eastside_mandir_hall
# shot: wide
# objective: Watch the evening closing
- (shrine)
+ {shrine < 4} [Look: The main shrine]
    Rama and Sita in fresh silk, a new garland: yours, the crooked one, near the bottom. # look: shrine-garland
    -> shrine
+ {shrine < 4} [Look: The Shiva shrine]
    The smooth dark stone, the lingam, still shining from the morning's milk. A brass pot hanging above it, dripping, slowly. # look: shiva-lingam
    -> shrine
+ {shrine < 4} [Look: The schedule board]
    "6:30 am Kakad Aarti. 8:00 Abhishek (Mon). 12:00 Bhog. 7:00 pm Sandhya Aarti. 8:30 Shayan Aarti (closing)." Every day. Every single day. # look: temple-schedule
    -> shrine
* {shrine >= 2} [Talk: Mangala Tai]
-
Mangala Tai: You're here a lot now. Good. I always say: the temple is where God lives, but it's the people who keep the lights on. # vo: ch5-hinduism_mangala_0014
* [Talk: Mangala Tai (ask how she became a priest)]
    ~ record_choice("ch5.p2.mangala", "how", "You asked Mangala Tai how she became a priest. She learned the chants listening at the door while her father taught her brothers, and trained in Pune after her children were grown.")
    Maya: How did you become a priest? # vo: ch5-hinduism_maya_0015
    Mangala Tai: My father was a priest. He taught my brothers, and I sat outside the door and learned every word. At thirty-five, children grown, I went to Pune and trained properly. Some families still say no, a woman can't. More say: Tai, you explain it, so we want you. # vo: ch5-hinduism_mangala_0016
* [Talk: Mangala Tai (ask about the closing)]
    ~ record_choice("ch5.p2.mangala", "closing", "You asked Mangala Tai about the closing aarti. She said every day the Lord is woken, bathed, fed, and put to bed, like a beloved guest.")
    Maya: What happens at the closing? # vo: ch5-hinduism_maya_0017
    Mangala Tai: Every day we wake Him with a song. We bathe Him, dress Him, feed Him, and at night we sing Him to sleep. Like a beloved guest. Every day. Even when I'm tired. Especially then. # vo: ch5-hinduism_mangala_0018
-
Maya: Every day. Even when you're tired. Somebody's taking care of someone, every single day. # vo: ch5-hinduism_maya_0019
Heather (inside): That's what I do. That's what I've always done. # vo: ch5-hinduism_heather-inside_0020
~ nudge(-0.2)
~ journal("Note", "The temple's day: wake, bathe, dress, feed, lamps, sleep. Every day. Somebody taking care of someone.")
~ text_to("tanya", "I'm making flower garlands on Saturdays. They're crooked. Heather makes them straight.")
~ text_from("tanya", "You just said Heather's name to me like it was the most normal thing in the world. I'm so glad.")
-> ch5_part3  // the next part of the same chapter (one run)
