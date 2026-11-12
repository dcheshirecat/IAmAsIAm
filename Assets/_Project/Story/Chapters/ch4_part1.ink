// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 4 (Islam), Part 1.
// vo-speakers: Maya=maya, Chaplain Amal=amal
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. The day program. Chicago, September 2026. The body is 28.
//  Owner's beats: after Maya gets back from Nepal. The chapter starts as Izzy; Izzy takes over along with Heather; Maya only
//  appears sometimes. The guide is a Muslim chaplain at a psychiatric day ward. Islam shown: Sufi practice and only what
//  Muslims consider Islam.
//
//  My guesses, for the owner to correct:
//   - Angie refers them to a day program at a hospital after Nepal: five days a week, groups, a quiet room.
//   - The chaplain: "Chaplain Amal" (owner: Chaplain Amal, Maya's age (28), she/her), a Sunni Muslim chaplain who gives spiritual care to
//     anyone who wants it, of any faith or none. She never preaches.
//   - Izzy introduces herself as Izzy. Amal simply calls her that.
//   - The interfaith quiet room has a small arrow on the ceiling pointing to Mecca (the qibla), folded prayer rugs, tissues.
// ─────────────────────────────────────────────────────────────────────────────

=== ch4_part1 ===
~ set_age(28)
~ set_date(2026, 9, 14)  // Claude's guess at the exact day (owner: change it freely)
# cw: medical-setting
# cw: religious-content
# cw: involuntary-switch
~ explore_tradition("islam")
// Owner: Izzy takes over along with Heather.
~ co_conscious("heather", true)
~ text_from("angie", "First day at the program today. You don't have to be anyone in particular there. Just come.")
-> wake

= wake
// Owner: a full day in every part. Claude's scenes, as Izzy (Heather sharing time).
# place: chicago_apartment
# shot: close
# objective: Get to the day program by nine
- (room)
+ {room < 3} [Look: The fridge]
    Oat milk, three cartons. Hot sauce. A note in Heather's square letters: "EAT BREAKFAST. -H" # look: fridge-izzy
    -> room
+ {room < 3} [Look: The mirror]
    Eyeliner, a little wonky. Good enough. It's a psych ward, not a gala. # look: eyeliner-wonky
    -> room
* [Use: The toaster]
    Toast. Heather wins. # look: toast-heather-wins
-
-> commute

= commute
# place: cta_train
# shot: over-shoulder
# objective: Ride to the hospital
- (car)
+ {car < 2} [Look: The window]
    September light on the rooftops. Your stop is the one with the hospital sign. # look: l-september
    -> car
* [Use: Your earbuds (music up)]
    Something loud with a drum machine. Heather, inside, very politely asking for something quieter. # look: earbuds-izzy
-
-> lobby

= lobby
# place: day_ward_group_room
# shot: over-shoulder
# objective: Find a seat before group starts
Izzy: Okay! New place. Name tags. I love a name tag. # vo: ch4-islam_izzy_0001
Heather (inside): Write "Maya". It's her name on the forms. # vo: ch4-islam_heather-inside_0002
- (room)
+ {room < 4} [Look: The name tags]
    A sheet of stickers and a fat marker. You write "IZZY", then, smaller, underneath, "(Maya)". # look: name-tag-izzy
    -> room
+ {room < 4} [Look: The schedule board]
    "9:00 Check-in. 10:00 DBT skills. 11:30 Art. 1:00 Lunch. 2:00 Process group. 3:30 Chaplain's hour (optional)." # look: day-schedule
    -> room
+ {room < 4} [Look: The circle of chairs]
    Twelve chairs, a box of tissues on every third one, a woman knitting very fast, a young man asleep in his hoodie. # look: group-circle
    -> room
* {room >= 2} [Wait (sit down)]
-
~ set_experience("Derealization", 0.3)
Izzy: Check-in: I'm Izzy. I'm... part of Maya. It's a long story. It's a long story with dinosaurs in it. # vo: ch4-islam_izzy_0003
Heather (inside): That's good. That's honest. Breathe. # vo: ch4-islam_heather-inside_0004
-> skills

= skills
# place: day_ward_group_room
# shot: close
# objective: Get through skills group
* [Look: The whiteboard]
    "TIPP: Temperature. Intense exercise. Paced breathing. Paired muscle relaxation." Someone has drawn a little ice cube with a face. # look: tipp-board
-
Feet on the floor. The cold of the water bottle in your hands. # exercise: paced-breathing
~ learn_skill("tipp")
~ nudge(-0.15)
-> chaplain

= chaplain
# place: day_ward_quiet_room
# shot: over-shoulder
# objective: Go to the chaplain's hour
- (quiet)
+ {quiet < 4} [Look: The arrow on the ceiling]
    A small arrow, painted on a ceiling tile, and one word: "Qibla". The direction of Mecca, for anyone who prays. # look: qibla-arrow
    -> quiet
+ {quiet < 4} [Look: The shelf]
    Prayer rugs, folded. A Quran on a stand, a Bible, a Tanakh, a little brass Buddha, a box of tissues. Room for everyone. # look: interfaith-shelf
    -> quiet
+ {quiet < 4} [Look: The window]
    The hospital car park, and one tree going orange. Someone has put a bird feeder out. # look: car-park-tree
    -> quiet
* {quiet >= 1} [Talk: Chaplain Amal]
-
Chaplain Amal: Hi. I'm Amal, the chaplain. I'm Muslim, but this hour is for anyone, any faith or none. I mostly listen. Sometimes I bring dates. # vo: ch4-islam_amal_0005
Izzy: Izzy. Hi. Do chaplains have to be serious? You don't look very serious. # vo: ch4-islam_izzy_0006
Chaplain Amal: Only on Tuesdays. It's Thursday. # vo: ch4-islam_amal_0007
* [Talk: Chaplain Amal (ask why she does this)]
    ~ record_choice("ch4.p1.amal", "why", "On the first day, Izzy asked Chaplain Amal why she does this work.")
    Izzy: Why do you do this? Sit with sad people all day? # vo: ch4-islam_izzy_0008
    Chaplain Amal: The Prophet, peace be upon him, visited the sick. It's one of the things we're asked to do. And honestly? People are most themselves in rooms like this. # vo: ch4-islam_amal_0009
* [Talk: Chaplain Amal (tell her about the others)]
    ~ record_choice("ch4.p1.amal", "others", "On the first day, Izzy told Chaplain Amal there were others: Maya, Heather. Amal didn't flinch.")
    Izzy: There's more of us. Maya, mostly. Heather's here too, today. Is that weird? # vo: ch4-islam_izzy_0010
    Chaplain Amal: Not in this room. Welcome, all of you. You don't have to explain anything to me. # vo: ch4-islam_amal_0011
-
-> surface

= surface
// Owner: Maya only appears sometimes. Here, for a moment.
# switch: maya
# place: day_ward_quiet_room
# shot: close
# objective:
~ set_experience("TimeLoss", 0.4)
* [Look: The name tag]
    "IZZY (Maya)". In your writing. Round letters. Not yours. # look: name-tag-maya
-
Maya: Sorry. I'm... I'm Maya. I think I was just here. Was I just here? # vo: ch4-islam_maya_0012
Chaplain Amal: You were. Someone very funny was here a minute ago. And someone very kind. Take your time. There's tea. # vo: ch4-islam_amal_0013
# switch: izzy
~ set_experience("TimeLoss", 0.0)
Izzy: Tea! Yes. Tea. # vo: ch4-islam_izzy_0014
~ journal("Note", "Day program. Chaplain Amal, Thursdays at 3:30. She said: you don't have to explain anything to me.")
-> evening

= evening
# place: chicago_apartment
# shot: close
# objective: Make dinner
- (k)
+ {k < 2} [Look: The name tag]
    "IZZY", in Sharpie, still stuck to your shirt. You don't take it off. # look: name-tag-home
    -> k
* [Use: The stove (pasta)]
    Pasta with far too much garlic. Heather does the dishes. # look: garlic-pasta
-
* [Use: The bed]
    Mira's stone and the hematite on the nightstand. Day one of the program. Survived it. # look: day-one-survived
-
-> mabon

= mabon
// Owner: every sabbat played at least once across the game (Claude's draft scene). No new voiced lines.
// Izzy practises Wicca too (owner): this one is hers.
~ set_date(2026, 9, 22)
# place: chicago_apartment
# shot: over-shoulder
# objective: Mabon! Apples. Thanks. Go.
- (home)
+ {home < 3} [Look: The apples]
    A bag of apples from the corner store. Three different kinds, because why not. # look: mabon-apples
    -> home
+ {home < 3} [Look: The leaves]
    Red and yellow leaves, pressed flat under the tarot deck. # look: mabon-leaves
    -> home
* [Use: The windowsill (keep Mabon)]
    ~ record_choice("ch4.p1.mabon", "kept", "At Mabon, Izzy kept the sabbat at the windowsill: apples, leaves, three things to be grateful for.")
    Apples on the windowsill, crooked. Good enough. # ritual: sabbat mabon
* [Wait (eat an apple)]
    ~ record_choice("ch4.p1.mabon", "apple", "At Mabon, Izzy just ate an apple on the windowsill and called it a sabbat.")
    Crunch. That counts. # look: mabon-crunch
-
~ bos_entry("izzy", "Wheel", "Mabon", "Three things: tea. The quiet room. Not being alone in here.")
-> ch4_part2  // the next part of the same chapter (one run)
