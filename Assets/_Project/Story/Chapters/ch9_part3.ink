// IN THE GAME as a DRAFT (Claude, 2026-11-13; the owner's Doc edits come first). Chapter 9 (Humanism), Part 3.
// vo-speakers: Maya=maya, Miss Ella=ella, Maisie=maisie, Heather (inside)=heather-inside, Heather=heather
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Here. Arcata, June 2032. Maya is 33.
//  Owner: the end of the school year. Maya kept showing up AND spoke up. The chapter unlocks nobody.
//
//  My guesses, for the owner to correct:
//   - The club's last Wednesday is a clean-up on the Arcata Marsh trails, then a goodbye picnic. Maisie is moving up to high
//     school. She's getting help now (from the counsellor; the game doesn't say more), and this time, at roll call, she answers.
//   - Miss Ella mentions World Humanist Day (June 21), and that she's training as a humanist celebrant (someone who leads
//     non-religious weddings and funerals), as a little look at organised humanism.
//   - Maisie gives Maya a drawing of the marsh. Heather comes forward for a moment at the end, to say goodbye to the kids.
// ─────────────────────────────────────────────────────────────────────────────

=== ch9_part3 ===
~ set_age(33)
~ set_date(2032, 6, 9)  // a Wednesday, the club's last one of the year (Claude's guess)
# cw: childhood-reference
-> wake

= wake
# place: arcata_apartment
# shot: close
# objective: Get ready for the last Wednesday
- (room)
+ {room < 3} [Look: The calendar]
    Thirty-four purple squares. One left. # look: calendar-last-square
    -> room
+ {room < 3} [Look: The window]
    June, and for once no fog. The bay flat and silver, the hills green, the redwoods sharp against the sky. # look: arcata-clear-june
    -> room
* [Use: The kitchen (make the snacks)]
    You and Heather made four dozen cookies last night. Some of them are shaped like trilobites. On purpose. # look: trilobite-cookies
-
-> marsh

= marsh
# place: arcata_marsh
# shot: wide
# objective: Clean up the trail
- (trail)
+ {trail < 4} [Look: The marsh]
    Ponds and reeds and boardwalks out to the bay. Egrets standing very still. Someone's dog trying, very hard, not to chase them. # look: arcata-marsh
    -> trail
+ {trail < 4} [Look: The kids]
    Fourteen kids with litter-pickers and gloves, treating every bottle cap like treasure. Luis has found a shoe and will not be parted from it. # look: litter-pickers
    -> trail
+ {trail < 4} [Look: Miss Ella]
    Sunburned already, a clipboard, a whistle she never blows. # look: ella-clipboard
    -> trail
* {trail >= 2} [Talk: Miss Ella]
-
Miss Ella: Roll call, humans! Last one of the year. Say "here", or make any noise at all. # vo: ch9-humanism_ella_0035
Miss Ella: Jayden. Priya. Luis. Maisie? # vo: ch9-humanism_ella_0036
Maisie: Here. # vo: ch9-humanism_maisie_0037
~ record_choice("ch9.p3.roll", "here", "On the last Wednesday of the year, at roll call on the Arcata Marsh, Maisie answered her name.")
Miss Ella doesn't say anything. She just looks over at you and makes a small tick, very slowly, and grins. # look: ella-slow-tick
~ nudge(-0.3)
-> picnic

= picnic
# place: arcata_marsh
# shot: close
# objective: Sit with Miss Ella
- (bench)
+ {bench < 3} [Look: The bay]
    Pelicans going by in a line, low over the water, like they're late for something. # look: pelicans-line
    -> bench
+ {bench < 3} [Look: Your cookies]
    Gone. All of them. The trilobites first. # look: cookies-gone
    -> bench
* [Talk: Miss Ella]
-
Miss Ella: Can I tell you something embarrassing? I'm training to be a humanist celebrant. Weddings, funerals, baby namings. No God in them. Just people saying what's true about each other. # vo: ch9-humanism_ella_0038
Maya: That's not embarrassing. That's beautiful. # vo: ch9-humanism_maya_0039
Miss Ella: My mom thinks I'm going to hell. I think I'm going to a lot of weddings. June twenty-first is World Humanist Day, there's a picnic on the Plaza. Bring everybody. # vo: ch9-humanism_ella_0040
* [Talk: Miss Ella (say what you believe)]
    ~ record_choice("ch9.p3.belief", "said", "On the marsh, Miss Ella asked what you believe. You said: a lot of things, from a lot of people, and this year, the plain one: show up.")
    Maya: I believe a lot of things. I've been collecting them, from a lot of people. But this year I believed the plain one. You show up. You say their name. You don't let it go. # vo: ch9-humanism_maya_0041
    Miss Ella: That's the whole of humanism, honestly. The rest is footnotes. # vo: ch9-humanism_ella_0042
* [Wait (just watch the kids)]
    ~ record_choice("ch9.p3.belief", "watched", "On the marsh, Miss Ella asked what you believe. You didn't answer. You watched the kids, and that was the answer.")
    Fourteen kids on a boardwalk, shouting at an egret. That's the answer. She seems to know it is. # look: kids-egret
-
-> goodbye

= goodbye
# place: arcata_marsh
# shot: over-shoulder
# objective: Say goodbye to Maisie
- (end)
+ {end < 2} [Look: Maisie's grandmother]
    In the little car park, out of her scrubs for once, waving at you like she knows who you are. Maybe she does. # look: grandma-waves
    -> end
* [Talk: Maisie]
-
Maisie: I made you something. It's not very good. # vo: ch9-humanism_maisie_0043
A drawing of the marsh in coloured pencil. Two people on a bench by the water. One of them is holding a fern. # look: maisie-drawing
Maisie: That's us. On Darwin Day. Except I put us outside, because outside is better. # vo: ch9-humanism_maisie_0044
* [Talk: Maisie (thank her)]
    ~ record_choice("ch9.p3.drawing", "thanked", "On the last Wednesday, Maisie gave you a drawing of the two of you on a bench by the marsh. You told her it was the best thing anyone had given you in years. It was true.")
    Maya: It's the best thing anyone's given me in years. I mean it. # vo: ch9-humanism_maya_0045
* [Talk: Maisie (tell her you'll be at the picnic)]
    ~ record_choice("ch9.p3.drawing", "picnic", "On the last Wednesday, Maisie gave you a drawing of the two of you on a bench by the marsh. You told her you'd see her at the picnic on the Plaza. You meant to.")
    Maya: I'll see you at the picnic on the Plaza. And next year, if you want to come back and help with the little ones. # vo: ch9-humanism_maya_0046
-
Maisie: Okay. Bye, Maya. # vo: ch9-humanism_maisie_0047
-> heather

= heather
// Heather, who signed them up, comes forward for a moment to say goodbye.
# switch: heather
~ co_conscious("maya", true)
# place: arcata_marsh
# shot: close
# objective:
Heather: Bye, Maisie. You did so well this year. We're proud of you. # vo: ch9-humanism_heather_0048
Maisie: You sound different. # vo: ch9-humanism_maisie_0049
Heather: Do I? Maybe I'm just happy. # vo: ch9-humanism_heather_0050
# switch: maya
~ co_conscious("heather", true)
-> home

= home
# place: arcata_apartment
# shot: close
# objective: Put the drawing somewhere
- (room)
* [Use: The sill (put the drawing there)]
    ~ record_choice("ch9.p3.sill", "drawing", "You put Maisie's drawing on the windowsill, next to Mira's stone.")
    Mira's stone. The prayer wheel. The khata. The jar from Bríd. And now a girl and a woman on a bench by the water, holding a fern. # look: sill-drawing
-
Heather (inside): You stayed. # vo: ch9-humanism_heather-inside_0051
Maya: We stayed. You started it. # vo: ch9-humanism_maya_0052
Heather (inside): Someone had to be there at 3:15. # vo: ch9-humanism_heather-inside_0053
~ system_log("maya", "Last Wednesday of the year. Maisie said 'here'. Thank you for signing us up, H. Same time in September? -M")
~ system_log("heather", "Same time in September. -H")
~ text_to("tanya", "Last day of the club. The girl answered her name at roll call. I nearly cried on a boardwalk in front of fourteen children.")
~ text_from("tanya", "Nearly? I'd have wept openly. I'm so proud of you. All of you.")
~ journal("Note", "The last Wednesday. Maisie said 'here'. Miss Ella: the whole of humanism is showing up, the rest is footnotes. A drawing on the sill.")
# shot: inner
# place: black
Arcata, California. June 2032. # chapter-end
-> END
