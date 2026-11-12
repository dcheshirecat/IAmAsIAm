// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 5 (Hinduism), Part 3.
// vo-speakers: Maya=maya, Mangala Tai=mangala
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Lamps. Seattle, October to November 2027. Maya is 29.
//  Navaratri (the Goddess) and Diwali (lamps). Maya grounded; Heather visits, and this time Maya knows.
//
//  My guesses, for the owner to correct:
//   - Navaratri: garba dancing in a community hall with Priya's friends (a Gujarati tradition), nine nights for the Goddess.
//   - Diwali: lamps (diyas) along Priya's family's front steps; then the temple. Heather comes forward to light the lamps (her
//     candles again), and Maya, grounded now, stays aware and isn't frightened: the first visit she remembers.
//   - The chapter unlocks nobody yet (to confirm with the owner).
// ─────────────────────────────────────────────────────────────────────────────

=== ch5_part3 ===
~ set_age(29)
~ set_date(2027, 10, 5)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
# cw: involuntary-switch
-> workday

= workday
// Owner: a full day in every part. Claude's scenes: the workday before the garba, and the bus home after.
# place: burke_museum_hall
# shot: wide
# objective: Give the school tour
- (hall)
+ {hall < 3} [Look: The whale]
    Thirty fourth-graders lying on the floor under the whale, looking up. Your idea. The best idea. # look: kids-under-whale
    -> hall
+ {hall < 3} [Look: Priya]
    Priya at the collections door, mouthing "GARBA. TONIGHT. COMFORTABLE SHOES." # look: priya-garba
    -> hall
* [Use: Your lunch]
    Leftover dal, at your desk, while a kid's drawing of the whale dries on the radiator. # look: dal-desk
-
-> garba

= garba
// Navaratri: nine nights for the Goddess. Garba: circles of dancers clapping and turning, around a lamp or an image of the Goddess.
# place: garba_community_hall
# shot: wide
# objective: Join the circle
- (circle)
+ {circle < 4} [Look: The Goddess]
    In the middle of the hall, a small shrine: the Goddess in red, a lamp burning in front of her. Everyone dances around her. # look: navaratri-goddess
    -> circle
+ {circle < 4} [Look: The dancers]
    Hundreds of people in mirrored skirts and bright kurtas, clapping, turning, a wheel of colour going round and round. # look: garba-dancers
    -> circle
+ {circle < 4} [Look: Priya]
    Priya, waving at you with both arms, in a skirt covered in tiny mirrors. "Come ON." # look: priya-garba
    -> circle
* {circle >= 1} [Use: The circle (join in)]
    ~ record_choice("ch5.p3.garba", "joined", "At Navaratri, you joined the garba circle. Clap, turn, step. You were bad at it. Nobody cared.")
    Clap, turn, step, clap. You're bad at it. Nobody cares. An old man shows you the step twice, beaming. # look: garba-joined
-
~ set_experience("Derealization", 0.0)
Maya: I'm here. All of me. I'm actually here. # vo: ch5-hinduism_maya_0021
Priya: You're smiling! You never smile like that at work. # vo: ch5-hinduism_priya_0022
~ nudge(-0.25)
-> late

= late
# place: seattle_bus
# shot: over-shoulder
# objective: Ride home
- (bus)
+ {bus < 2} [Look: Your feet]
    Sore from dancing. The dandiya sticks in your bag, knocking together. # look: sore-feet
    -> bus
* [Use: The window]
    The bus driver humming the garba song. You hum it too. # look: driver-humming
-
-> diwali

= diwali
# place: priya_family_house
# shot: over-shoulder
# objective: Help with the lamps
Lakshmi: Diwali! Lamps on every step. Light chases away darkness, that's the story. Here, you do this side. # vo: ch5-hinduism_lakshmi_0023
- (steps)
+ {steps < 4} [Look: The rangoli]
    A pattern on the doorstep in coloured powder: a lotus, petals of pink and yellow, made by Priya's niece over two hours. # look: rangoli
    -> steps
+ {steps < 4} [Look: The diyas]
    Little clay lamps, a cotton wick in oil, a row of them up every step and along every window. # look: diyas
    -> steps
* {steps >= 1} [Use: The matches]
-
// Heather comes forward to light the lamps. This time, Maya stays aware (owner: more grounded, still experiencing visits).
# switch: heather
~ co_conscious("maya", true)
* [Use: A diya (light it)]
    ~ record_choice("ch5.p3.diwali", "heather", "On Diwali, Heather came forward to light the lamps. Maya stayed, and watched, and wasn't frightened.")
    One flame. Then the next. Heather's hands, steady, the way they were in the lamp room in Boudha. # look: heather-diyas
-
Heather: For Mira. For the little one in the fellowship hall. For all of us, who are still here. # vo: ch5-hinduism_heather_0024
Maya (inside): I can see you. I'm right here. I can see you doing it. # vo: ch5-hinduism_maya-inside_0025
Heather: I know. Hello, Maya. # vo: ch5-hinduism_heather_0026
# switch: maya
~ co_conscious("heather", true)
~ system_log("maya", "Heather lit the lamps on Diwali. I was there the whole time. I saw. Thank you. -M")
-> temple_night

= temple_night
# place: eastside_mandir_hall
# shot: close
# objective: Go to the temple for the Diwali aarti
- (hall)
+ {hall < 3} [Look: The temple]
    Lamps on every ledge, a thousand small flames, the shrines in silk and gold. The whole hall smells of ghee and flowers. # look: temple-diwali
    -> hall
+ {hall < 3} [Look: The children]
    Sparklers outside the door, held at arm's length, a grandfather supervising very seriously. # look: sparklers
    -> hall
* {hall >= 1} [Talk: Mangala Tai]
-
Mangala Tai: Happy Diwali! You look different. Lighter. Something good happened? # vo: ch5-hinduism_mangala_0027
* [Talk: Mangala Tai (tell her)]
    ~ record_choice("ch5.p3.end", "told", "On Diwali, you told Mangala Tai that someone who looks after you lit the lamps, and you were there for it.")
    Maya: Someone who looks after me lit the lamps tonight. And I was there for it. I was there for the whole thing. # vo: ch5-hinduism_maya_0028
    Mangala Tai: Then two lights were lit tonight. Take the aarti. Both of you. # vo: ch5-hinduism_mangala_0029
* [Talk: Mangala Tai (just say Happy Diwali)]
    ~ record_choice("ch5.p3.end", "diwali", "On Diwali, you just wished Mangala Tai a happy Diwali, and took the light.")
    Maya: Happy Diwali, Mangala Tai. # vo: ch5-hinduism_maya_0030
-
Warm, over your palms. Warm, over your eyes. # look: take-the-light-diwali
~ journal("Note", "Diwali. Heather lit the lamps and I was there. Two lights.")
~ text_to("tanya", "Happy Diwali. Heather lit the lamps and I didn't lose any time. I watched. I think we're getting somewhere.")
~ text_from("tanya", "Happy Diwali, all of you. I'm lighting one here too.")
# shot: inner
# place: black
Seattle. November 2027. # chapter-end
-> END
