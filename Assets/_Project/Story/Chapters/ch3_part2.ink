// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 3 (Buddhism), Part 2.
// vo-speakers: Maya=maya, Pasang Lhamo=pasang
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Boudha. Kathmandu, Nepal, April 2026. Maya is 27.
//  Owner's beats: three months, alone. The guide is a temple-stay host: Pasang Lhamo Sherpa (about 55, she/her), who runs
//  her family's guest house behind a monastery in Boudha; her brother-in-law is a monk there. Practical and motherly, she
//  walks around the stupa with Maya at dawn. Maya has romanticized ideas about finding herself here, and has to cope with
//  the reality. Maya and Izzy share time.
//
//  My guesses, for the owner to correct:
//   - The reality: jet lag, dust, dogs barking all night, a cold shower, monks on their phones, a stupa full of tourists.
//     None of it is bad. It just isn't the poster.
//   - The kora: always clockwise, prayer wheels, butter lamps, pigeons, grandmothers with prayer beads.
//   - Maya signs up for a 10-day silent course to "really" find herself. Pasang lets her go; she doesn't see the danger.
// ─────────────────────────────────────────────────────────────────────────────

=== ch3_part2 ===
~ set_age(27)
~ set_date(2026, 4, 10)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
~ co_conscious("izzy", true)
~ text_to("tanya", "Landed. Alive. Everything smells like incense and diesel.")
~ text_from("tanya", "Day one. Good. Drink water. Eat something. Tell me tomorrow.")
-> arrival

= arrival
# place: boudha_guest_house
# shot: over-shoulder
# objective: Find your room
Pasang Lhamo: You came alone? With one bag? Good. Less to carry up the stairs. Come, come. Fourth floor. Sorry. # vo: ch3-buddhism_pasang_0039
- (room)
+ {room < 4} [Look: The window]
    Over the rooftops, the great white dome of the stupa, and the Buddha's eyes looking out, all four ways. Also: a satellite dish, laundry, a dog asleep on a water tank. # look: stupa-from-window
    -> room
+ {room < 4} [Look: The bed]
    A thin mattress. A thick, heavy blanket with tigers on it. It's colder than you thought it would be. # look: tiger-blanket
    -> room
+ {room < 4} [Look: The shower]
    A bucket, a heater that takes twenty minutes, and a sign: "PLEASE SAVE WATER. THANK YOU. PASANG." # look: bucket-shower
    -> room
* {room >= 2} [Talk: Pasang Lhamo]
-
Pasang Lhamo: Breakfast at seven. Tea any time. My brother-in-law is a monk at the monastery behind us. If you want to visit, you visit. If you want to sleep, you sleep. First, you sleep. # vo: ch3-buddhism_pasang_0040
* [Talk: Pasang Lhamo (tell her why you came)]
    ~ record_choice("ch3.p2.why", "told", "On your first night in Boudha, you told Pasang you'd come to find yourself. She laughed, kindly.")
    Maya: I came to... find myself. I think. I just got a, a diagnosis, and I thought, if anywhere... # vo: ch3-buddhism_maya_0041
    Pasang Lhamo: You lost something? Check your bag first. Then the monastery. Then sleep. Then we see. # vo: ch3-buddhism_pasang_0042
* [Talk: Pasang Lhamo (just say thank you)]
    ~ record_choice("ch3.p2.why", "thanks", "On your first night in Boudha, you just thanked Pasang. She brought you tea anyway.")
    Maya: Thank you. Really. # vo: ch3-buddhism_maya_0043
    Pasang Lhamo: Tea. I'll bring it. You look like a person who needs tea. # vo: ch3-buddhism_pasang_0044
-
~ set_experience("Derealization", 0.4)
Izzy (inside): Dogs. So many dogs. Are they having a party? # vo: ch3-buddhism_izzy-inside_0045
~ text_to("tanya", "Day one. The dogs bark all night. The shower is a bucket. I think I love it? I think I hate it?")
-> kora

= kora
# place: boudha_stupa
# shot: wide
# objective: Walk around the stupa with Pasang
Pasang Lhamo: Always this way. Clockwise. Keep the stupa on your right. Everyone, every morning. # vo: ch3-buddhism_pasang_0046
- (circle)
+ {circle < 5} [Use: The prayer wheels]
    A row of brass wheels, worn smooth by a million hands. You spin one. It keeps turning after you've gone. # look: prayer-wheels
    -> circle
+ {circle < 5} [Look: The butter lamps]
    Hundreds of little flames in a glass house, and the smell, rich and warm. A woman fills them one by one. # look: butter-lamps
    -> circle
+ {circle < 5} [Look: The grandmothers]
    Old women in long dresses, beads clicking through their fingers, walking faster than you. One of them pats your arm as she passes. # look: kora-grandmothers
    -> circle
+ {circle < 5} [Look: The monks]
    Two young monks in maroon, laughing at something on a phone. A third buying momos. # look: monks-phone
    -> circle
+ {circle < 5} [Look: The tourists]
    Selfie sticks. A tour group in matching hats. A man in white linen, meditating loudly on the steps. # look: stupa-tourists
    -> circle
* {circle >= 3} [Talk: Pasang Lhamo]
-
Maya: I thought it would be... quieter. More... I don't know. Holy? # vo: ch3-buddhism_maya_0047
Pasang Lhamo: It is holy. And it's noisy. Both. You want quiet, you go inside. You want to live, you come out here. # vo: ch3-buddhism_pasang_0048
~ nudge(-0.15)
Izzy (inside): I like it out here. Everyone's walking the same way. Nobody's lost. # vo: ch3-buddhism_izzy-inside_0049
~ journal("Note", "The kora: clockwise, the stupa on your right. Prayer wheels keep turning after you've gone.")
-> kitchen

= kitchen
# place: boudha_guest_house
# shot: close
# objective: Eat something
- (momos)
+ {momos < 3} [Look: The momos]
    A steamer of dumplings, folded by Pasang's daughter faster than you can watch. Chilli sauce that makes your eyes water. # look: momos
    -> momos
+ {momos < 3} [Look: The altar shelf]
    A small shelf above the stove: a photo of a lama, seven little bowls of water in a row, a butter lamp. # look: kitchen-altar
    -> momos
* {momos >= 1} [Use: The flyer on the wall]
    "VIPASSANA. 10 days. Noble silence. Dharma Shringa, Budhanilkantha. Courses run on donation." # look: vipassana-flyer
-
Maya: Ten days of silence. That's it. That's what I need. To really... go in. # vo: ch3-buddhism_maya_0050
Pasang Lhamo: Ten days no talking? I can't do ten minutes! Go, go. Many guests go. They come back very calm, or very hungry. I'll keep your room. # vo: ch3-buddhism_pasang_0051
* [Talk: Pasang Lhamo (ask if it's hard)]
    ~ record_choice("ch3.p2.course", "asked", "Before the silent course, you asked Pasang if it was hard. She said everyone says so, and everyone comes back.")
    Maya: Is it hard? # vo: ch3-buddhism_maya_0052
    Pasang Lhamo: Everyone says so. Everyone comes back. You'll be fine. # vo: ch3-buddhism_pasang_0053
* [Wait (think of Angie's plan)]
    ~ record_choice("ch3.p2.course", "plan", "Before the silent course, you thought of Angie's plan: short sits, eyes open. You went anyway.")
    Short sits. Eyes open. Walk when you need to. Ten days isn't short. You sign up anyway. # look: angie-plan-thought
-
Izzy (inside): Ten days without talking? Who's going to talk to me? # vo: ch3-buddhism_izzy-inside_0054
~ text_to("tanya", "I signed up for a silent retreat! 10 days. No phone. Don't worry if I don't text.")
~ text_from("tanya", "Maya. Did you talk to Angie about this first?")
-> ch3_part3  // the next part of the same chapter (one run)
