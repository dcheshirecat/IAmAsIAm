// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 5 (Hinduism), Part 2.
// vo-speakers: Maya=maya, Pandit Raghavan=raghavan
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Every day. Seattle, June to September 2027. Maya is 28-29.
//  The temple priest's life, and the deity's day. Maya grounded; Heather visits.
//
//  My guesses, for the owner to correct:
//   - Maya volunteers on Saturdays: making flower garlands for the shrines with Priya's mother and the other aunties.
//   - The deity's day, which the priest keeps every day: waking at dawn with a song (Suprabhatam), bathing (abhishekam),
//     dressing in silk and flowers, offering food, lamps, closing at night. Caring for someone, every single day.
//   - Panditji's story: a priestly family; Vedic school from the age of eight; memorising chants for years; his father and
//     grandfather were temple priests too. His wife and grandchildren are on the phone from India every Sunday.
//   - Shiva on Mondays: milk poured over the lingam, the dark stone.
// ─────────────────────────────────────────────────────────────────────────────

=== ch5_part2 ===
~ set_age(28)
# cw: religious-content
~ co_conscious("heather", false)
-> garlands

= garlands
# place: eastside_mandir_kitchen
# shot: over-shoulder
# objective: Help with the garlands
Priya's mother: Thread, needle, flower, flower, flower, knot. Tight, or Ganesha will lose his necklace in the middle of the aarti. # vo: ch5-hinduism_priya-s-mother_0012
- (table)
+ {table < 4} [Look: The flowers]
    Marigolds, orange and yellow, a mountain of them. Jasmine in a wet cloth. Roses with their stems cut off. # look: garland-flowers
    -> table
+ {table < 4} [Look: The aunties]
    Five women, threading faster than you can see, arguing happily in three languages about a wedding. # look: garland-aunties
    -> table
* {table >= 1} [Use: The needle (make a garland)]
    ~ record_choice("ch5.p2.garland", "made", "On your first Saturday at the temple, you made a marigold garland. It was crooked. It went on Ganesha anyway.")
    Flower, flower, flower, knot. Crooked. Priya's mother looks at it, and laughs, and says Ganesha likes crooked things. # look: crooked-garland
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
-> day

= day
# place: eastside_mandir_hall
# shot: wide
# objective: Watch the evening closing
- (shrine)
+ {shrine < 4} [Look: The main shrine]
    Venkateswara in fresh silk, a new garland: yours, the crooked one, near the bottom. # look: shrine-garland
    -> shrine
+ {shrine < 4} [Look: The Shiva shrine]
    The smooth dark stone, the lingam, still shining from the morning's milk. A brass pot hanging above it, dripping, slowly. # look: shiva-lingam
    -> shrine
+ {shrine < 4} [Look: The schedule board]
    "6:30 am Suprabhatam. 8:00 Abhishekam (Fri). 12:00 Naivedyam. 7:00 pm Aarti. 8:30 Ekanta Seva (closing)." Every day. Every single day. # look: temple-schedule
    -> shrine
* {shrine >= 2} [Talk: Pandit Raghavan]
-
Pandit Raghavan: You're here a lot now. Good. I always say: the temple is where God lives, but it's the people who keep the lights on. # vo: ch5-hinduism_raghavan_0014
* [Talk: Pandit Raghavan (ask how he became a priest)]
    ~ record_choice("ch5.p2.raghavan", "how", "You asked Panditji how he became a priest. His father was one, and his grandfather, and he went to a Vedic school at eight.")
    Maya: How did you become a priest? # vo: ch5-hinduism_maya_0015
    Pandit Raghavan: My father was a priest. His father. His father. At eight years old I went to a Vedic school. Twelve years of chanting before I was allowed to touch the lamp. Twelve years! I was not a patient boy. # vo: ch5-hinduism_raghavan_0016
* [Talk: Pandit Raghavan (ask about the closing)]
    ~ record_choice("ch5.p2.raghavan", "closing", "You asked Panditji about the closing ceremony. He said every day the Lord is woken, bathed, fed, and put to bed, like a beloved guest.")
    Maya: What happens at the closing? # vo: ch5-hinduism_maya_0017
    Pandit Raghavan: Every day we wake Him with a song. We bathe Him, dress Him, feed Him, and at night we sing Him to sleep. Like a beloved guest. Every day. Even when I'm tired. Especially then. # vo: ch5-hinduism_raghavan_0018
-
Maya: Every day. Even when you're tired. Somebody's taking care of someone, every single day. # vo: ch5-hinduism_maya_0019
Heather (inside): That's what I do. That's what I've always done. # vo: ch5-hinduism_heather-inside_0020
~ nudge(-0.2)
~ journal("Note", "The temple's day: wake, bathe, dress, feed, lamps, sleep. Every day. Somebody taking care of someone.")
~ text_to("tanya", "I'm making flower garlands on Saturdays. They're crooked. Heather makes them straight.")
~ text_from("tanya", "You just said Heather's name to me like it was the most normal thing in the world. I'm so glad.")
-> ch5_part3  // the next part of the same chapter (one run)
