// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 4 (Islam), Part 4.
// vo-speakers: Maya=maya, Chaplain Amal=amal
// ─────────────────────────────────────────────────────────────────────────────
//  Part 4. Iftar. Chicago, February to March 2027 (Ramadan). The body is 28.
//  Owner's beats: Angie suggests SystemLog, and the chapter's end puts it on Maya's phone (the system leaves each other notes).
//
//  My guesses, for the owner to correct:
//   - Amal invites them to a mosque's open community iftar in Ramadan: everyone waits for sunset, breaks the fast with dates and
//     water at the call to prayer, the community prays (guests wait at the back), and then dinner, long and loud.
//   - Their last day at the day program: Zahra, Amal, a goodbye.
//   - Angie, in her office: "a notebook for all of you, on your phone". The first notes, from Heather and Izzy, and Maya reading them.
// ─────────────────────────────────────────────────────────────────────────────

=== ch4_part4 ===
~ set_age(28)
# cw: religious-content
~ co_conscious("heather", true)
~ text_from("zahra", "Ramadan Mubarak!! Amal says you're coming to the iftar. Sit with me. My mom made too much food. She always makes too much food.")
-> iftar

= iftar
// An open community iftar: guests of any faith welcome. The fast breaks at sunset (maghrib), with dates and water.
# place: mosque_iftar_hall
# shot: wide
# objective: Wait for sunset
- (hall)
+ {hall < 4} [Look: The tables]
    Long tables, paper tablecloths, a date and a cup of water at every place. Nobody eats yet. Everyone is waiting together. # look: iftar-tables
    -> hall
+ {hall < 4} [Look: The children]
    Kids running between the tables with plates of samosas they're not allowed to touch yet. A grandmother catches one, laughing. # look: iftar-children
    -> hall
+ {hall < 4} [Look: The clock]
    Sunset: 5:41. It's 5:38. Three hundred people watching the same minute hand. # look: iftar-clock
    -> hall
* {hall >= 1} [Talk: Zahra]
-
Zahra: When you hear the call, you eat the date. That's it. That's the whole secret. # vo: ch4-islam_zahra_0049
The call to prayer, from the front of the hall, rising and falling. Three hundred people breaking their fast at once. # look: adhan-iftar
* [Use: The date]
    ~ record_choice("ch4.p4.iftar", "date", "At the iftar, you ate the date at the call to prayer, with everyone.")
    Sweet, sticky, very sweet. Someone passes you water before you ask. # look: date-eaten
-
Chaplain Amal: Now they pray, and then they eat everything. Guests can wait here, or watch from the back. Either is good. # vo: ch4-islam_amal_0050
* [Wait (watch from the back)]
    ~ record_choice("ch4.p4.prayer", "watched", "At the iftar, you watched the prayer from the back of the hall, in rows, shoulder to shoulder.")
    Rows, shoulder to shoulder, everyone moving together: standing, bowing, kneeling, foreheads to the floor. Then stillness. # look: rows-praying
* [Wait (stay at the table)]
    ~ record_choice("ch4.p4.prayer", "table", "At the iftar, you stayed at the table and helped Zahra's mom guard the samosas.")
    Zahra's mom hands you a serving spoon and a look that means: guard the samosas. You guard them with your life. # look: guard-samosas
-
Heather (inside): Everyone eating together. Nobody alone. That's the part I needed. # vo: ch4-islam_heather-inside_0051
~ nudge(-0.25)
-> last_day

= last_day
# place: day_ward_group_room
# shot: close
# objective: Say goodbye at the program
- (goodbyes)
+ {goodbyes < 3} [Look: The circle of chairs]
    The same twelve chairs. You know everyone's names now, and which one is the knitting woman's. # look: last-circle
    -> goodbyes
+ {goodbyes < 3} [Look: The card]
    A card signed by the group. Zahra has drawn a tiny turbah. Someone has drawn an ice cube with a face. # look: goodbye-card
    -> goodbyes
* {goodbyes >= 1} [Talk: Chaplain Amal]
-
Chaplain Amal: You did the work here. All of you. I'll pray for you, if that's okay. For all of you. # vo: ch4-islam_amal_0052
* [Talk: Chaplain Amal (say yes)]
    ~ record_choice("ch4.p4.amal", "yes", "On the last day, Chaplain Amal asked if she could pray for all of you. You said yes.")
    Izzy: Yes. All of us. Even the grumpy ones. # vo: ch4-islam_izzy_0053
* [Talk: Chaplain Amal (thank her)]
    ~ record_choice("ch4.p4.amal", "thanks", "On the last day, you thanked Chaplain Amal for never needing you to explain.")
    Izzy: Thank you for never making us explain. # vo: ch4-islam_izzy_0054
-
-> angie

= angie
# place: therapist_office
# shot: over-shoulder
# objective: Talk to Angie
* [Look: Gerald]
    Gerald the plant has a little brass prayer wheel next to him now. He seems pleased about it. # look: gerald-prayer-wheel
-
Angie: Izzy, Heather, Maya: you're all writing in that notebook. Which is wonderful. But it lives in a drawer. What if it lived on your phone? # vo: ch4-islam_angie_0055
Angie: One shared place. Anyone who comes forward can leave a note. Anyone can read them. No secrets from each other, if you can manage it. # vo: ch4-islam_angie_0056
* [Talk: Angie (ask what to call it)]
    ~ record_choice("ch4.p4.log", "name", "Angie suggested a shared log on the phone, and Izzy named it SystemLog.")
    Izzy: What do we call it? Ooh. SystemLog. Like a ship's log. Captain's log, stardate whatever. # vo: ch4-islam_izzy_0057
* [Talk: Angie (ask if Maya will read it)]
    ~ record_choice("ch4.p4.log", "maya", "Angie suggested a shared log on the phone. You asked if Maya would read it. Angie said that was the point.")
    Izzy: Will Maya read it? She's scared of us, a bit. # vo: ch4-islam_izzy_0058
    Angie: That's the point. So she doesn't have to be scared of what she can't see. # vo: ch4-islam_angie_0059
-
// Owner: Angie suggests SystemLog; the end of Chapter 4 puts it on the phone.
~ gain_app("systemlog")
~ system_log("heather", "Water first. Then food. Then Angie on Thursday. We're doing well. -H")
~ system_log("izzy", "hi everyone!!! this is SystemLog. be nice. write stuff. -Izzy")
-> reading

= reading
# switch: maya
# place: therapist_office
# shot: close
# objective:
* [Use: Your phone (open SystemLog)]
    Your phone. A new app. Two notes, in two colours, signed. # device: phone
-
Maya: They wrote to me. They're... writing to me. # vo: ch4-islam_maya_0060
Angie: To all of you. Write back, when you're ready. # vo: ch4-islam_angie_0061
~ journal("Note", "SystemLog: one place on the phone where all of us can leave notes. Write back when I'm ready.")
# shot: inner
# place: black
Chicago. March 2027. # chapter-end
-> END
