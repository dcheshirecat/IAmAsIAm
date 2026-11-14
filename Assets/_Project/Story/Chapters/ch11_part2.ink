// IN THE GAME as a DRAFT (Claude, 2026-11-14. The owner's Doc edits come first). Chapter 11 (Jainism), Part 2.
// vo-speakers: Maya=maya, Nirmala-ben=nirmala, Serena (inside)=serena-inside, Nina (inside)=nina-inside, Heather (inside)=heather-inside
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Micchami Dukkadam. The Bay Area, September 2034. Maya is 36.
//  Owner: forgiveness. At Paryushana MAYA OPENS SARAH'S LETTER (what it says: Claude's draft, the owner decides), AND she asks
//  forgiveness of the people she hurt in lost time (Mark, Adeena, Tanya), by email.
//
//  Research: Samvatsari is the last day of the Svetambara Paryushana (2034: around September 17). Many fast. In the evening comes
//  the long Samvatsari pratikraman (a ritual of reflection and repentance), and afterwards everyone asks forgiveness of everyone:
//  "Micchami Dukkadam". One verse said in it: "Khamemi savve jiva, savve jiva khamantu me, mitti me savva bhuesu, veram majjha na
//  kenai": I forgive all living beings, may all beings forgive me. I am friends with all, and enemies with none.
//
//  My guesses, for the owner to correct:
//   - The letter (Claude's draft): a short note from Sarah, clearing out her office after retiring, and inside it a crayon drawing
//     Maya made at five, "ME AND MIRA", that Mira gave Sarah when she moved away. Never Sarah's side: no reasons, no apology.
//   - Writing to Sarah is the player's choice: Micchami Dukkadam, or not yet. Both are all right.
// ─────────────────────────────────────────────────────────────────────────────

=== ch11_part2 ===
~ set_age(36)
~ set_date(2034, 9, 17)  // Samvatsari, a Sunday in 2034 (calendar listings; dates vary by community)
# cw: religious-content
# cw: childhood-reference
~ co_conscious("serena", true)
~ text_from("tanya", "Thinking of you on your forgiveness day. Remember: forgiving is a door, not a debt. You can stand in it as long as you like.")
-> wake

= wake
# place: arcata_apartment
# shot: close
# objective: Decide about the letter
- (room)
+ {room < 3} [Look: The altar]
    The letter, under Mira's stone, since April of last year. The stamp has an American flag on it. Her handwriting on the front, the same as on every permission slip. # look: letter-handwriting
    -> room
+ {room < 3} [Look: The window]
    September sun for once, the bay bright, the fog waiting offshore like it's being polite. # look: september-bay
    -> room
* [Use: The letter (put it in your bag)]
    ~ record_choice("ch11.p2.letter", "bag", "On Samvatsari morning, you took Sarah's letter out from under Mira's stone and put it in your bag, still sealed.")
-
Serena (inside): We'll drive. You can open it there, or not at all. I'll be right here either way. # vo: ch11-jainism_serena-inside_0007
-> centre

= centre
# place: jain_centre
# shot: wide
# objective: Find Nirmala-ben
- (hall)
+ {hall < 4} [Look: The hall]
    Packed. Hundreds of people in white and cream. Many haven't eaten since yesterday, some not for eight days. Everyone very gentle with each other. # look: samvatsari-hall
    -> hall
+ {hall < 4} [Look: The kitchen]
    A side door, and volunteers packing tomorrow's breakfast for the people breaking their fasts. A tray of food covered with a cloth, with a sticky note: "MAYA. EAT. - N." # look: tray-for-maya
    -> hall
* {hall >= 1} [Talk: Nirmala-ben]
-
Nirmala-ben: You came! Did you eat? Eat first. Then sit at the back for pratikraman. It's three hours. You can leave whenever you want. The door is a door. # vo: ch11-jainism_nirmala_0025
* [Use: The tray (eat)]
    ~ record_choice("ch11.p2.tray", "ate", "On Samvatsari, you ate the tray Nirmala-ben had set aside for you before sitting down for pratikraman.")
    ~ nudge(-0.2)
-
-> pratikraman

= pratikraman
# place: jain_centre
# shot: over-shoulder
# objective: Sit through pratikraman
- (sit)
+ {sit < 3} [Look: The people]
    Rows of them on the carpet, some with a small white cloth held at their mouths so their breath won't harm the tiny lives in the air. A girl Maisie's age mouthing along without the book. # look: pratikraman-rows
    -> sit
+ {sit < 3} [Look: Nirmala-ben]
    Near the front, eyes closed, lips moving, perfectly still for an hour at a time. # look: nirmala-still
    -> sit
* {sit >= 1} [Wait (listen to the verse)]
-
A verse comes round, over and over, and someone has printed the English for visitors: "I forgive all living beings. May all beings forgive me. I am friends with all, and enemies with none." # look: khamemi-verse
Nina (inside): It's like meeting. Waiting in the quiet for what's true. # vo: ch11-jainism_nina-inside_0008
~ nudge(-0.2)
Afterwards, everyone turns to everyone, hands together, and says it: Micchami Dukkadam. Strangers say it to you. A child says it to you, very seriously. # look: micchami-everyone
Nirmala-ben: Micchami Dukkadam, Maya. If I hurt you, knowingly or not, please forgive me. # vo: ch11-jainism_nirmala_0026
Maya: You haven't... Micchami Dukkadam. Did I say it right? # vo: ch11-jainism_maya_0009
Nirmala-ben: Perfectly. Now. You have something in your bag that's been heavy all day. Go and sit in the car with it. Forgiveness doesn't need an audience. # vo: ch11-jainism_nirmala_0027
-> letter

= letter
# place: jain_centre_parking
# shot: close
# objective: Open the letter
* [Use: The letter (open it)]
-
~ set_experience("Depersonalization", 0.3)
A single sheet, folded twice. "Maya. Clearing out my office after the celebration. I found this in a folder Mira Smith gave me when she moved away. I don't know why I kept it so long. I thought it should be yours. The celebration was very nice. Mrs. Hines sends her love. Mom." # look: sarah-note
And inside it, a drawing. Crayon, on church paper. A little girl and a tall old lady, side by side, holding something square. "ME AND MIRA." On the back, in Mira's careful hand: "Maya, age 5. She drew this while we waited. Keep it safe. M.S." # look: me-and-mira-drawing
Serena (inside): She kept it. Mira kept it, and gave it to the one person who should have looked at it. And she didn't look. For thirty years. # vo: ch11-jainism_serena-inside_0010
Heather (inside): It's ours now. It came home. # vo: ch11-jainism_heather-inside_0011
~ record_choice("ch11.p2.opened", "opened", "On Samvatsari, in the Jain centre's car park, you opened Sarah's letter. Inside was a drawing you made at five: 'ME AND MIRA'. Mira had kept it.")
~ honour_ancestor("guide_quaker")
~ nudge(-0.2)
-> forgive

= forgive
# place: jain_centre_parking
# shot: close
# objective: Ask forgiveness of the people you hurt
Your laptop on the steering wheel. All the people the lost time took from you, or took you from. Micchami Dukkadam. If I hurt you, knowingly or not. # look: laptop-steering-wheel
- (who)
* [Use: The laptop (write to Tanya)]
    ~ email_to("tanya", "Micchami Dukkadam", "It's a Jain thing: if I hurt you, in thought or word or deed, knowingly or not, please forgive me. I disappeared on you for a whole year once. Someone in here did, anyway. I'm sorry for the year. Thank you for every year since.")
    ~ record_choice("ch11.p2.tanya", "sent", "On Samvatsari you asked Tanya's forgiveness for the year you disappeared.")
    For Tanya, for the year you disappeared. Sent. # look: email-tanya
    -> who
* [Use: The laptop (write to Mark)]
    ~ email_to("ex_christian", "Micchami Dukkadam", "Mark, it's been a long time. There's a Jain practice of asking forgiveness of everyone once a year. If I hurt you, knowingly or not, please forgive me. You were kind. You deserved an explanation I couldn't give you then. I'm doing much better now. I hope you are well.")
    ~ record_choice("ch11.p2.mark", "sent", "On Samvatsari you wrote to Mark, after fifteen years, and asked his forgiveness.")
    For Mark, fifteen years late, and kind. Sent. # look: email-mark
    -> who
* [Use: The laptop (write to Adeena)]
    ~ email_to("ex_jewish", "Micchami Dukkadam", "Adeena, I hope you and Michael are well. There's a Jain practice of asking everyone's forgiveness once a year. If I hurt you by not being there, really there, when we were together, please forgive me. I understand now why you couldn't stay. I'm glad you're happy.")
    ~ record_choice("ch11.p2.adeena", "sent", "On Samvatsari you wrote to Adeena and asked her forgiveness for not being there when you were together.")
    For Adeena, short, careful, no candles in it. Sent. # look: email-adeena
    -> who
* {who >= 1} [Wait (that's enough for today)]
-
And Sarah. The letter in your lap. The drawing on the dashboard, Mira's handwriting facing up.
* (wrote_sarah) [Use: The laptop (write to Sarah: Micchami Dukkadam)]
    ~ email_to("mother", "Thank you for the drawing", "Mom, thank you for sending the drawing. Micchami Dukkadam: it's a Jain way of asking forgiveness for any harm, knowingly or not. I'm not ready to say more than that yet. Maya.")
    ~ record_choice("ch11.p2.sarah", "wrote", "On Samvatsari you wrote to Sarah: thank you for the drawing, and Micchami Dukkadam. Not more than that, not yet.")
    Short. True. Not more than that. Sent. # look: email-sarah
* [Wait (not yet)]
    ~ record_choice("ch11.p2.sarah", "notyet", "On Samvatsari you didn't write to Sarah. Not yet. Nirmala-ben said forgiveness is a door, not a debt.")
    Not yet. Forgiveness is a door, not a debt. You can stand in it as long as you like. # look: not-yet-door
-
-> night

= night
# place: bay_area_motel
# shot: close
# objective: Sleep
- (room)
+ {room < 2} [Look: The drawing]
    Propped against the motel lamp. Two people and a packet of crackers. Thirty-one years in a folder, and now here. # look: drawing-motel-lamp
    -> room
* [Use: The bed]
-
~ system_log("maya", "Opened the letter. It was the drawing from the fellowship hall: me and Mira. Wrote to Tanya, Mark and Adeena. I feel lighter and also like I've been hit by a truck. -M")
~ system_log("serena", "Both of those can be true. Sleep. I'll drive tomorrow. -S")
~ journal("Note", "Samvatsari. Micchami Dukkadam: if I hurt you, knowingly or not, please forgive me. Opened Sarah's letter: 'ME AND MIRA', age five. Mira kept it. It came home.")
-> ch11_part3  // the next part of the same chapter (one run)
