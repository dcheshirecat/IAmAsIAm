// IN THE GAME as a DRAFT (Claude, 2026-11-13. The owner's Doc edits come first). Chapter 9 (Humanism), Part 1.
// vo-speakers: Maya=maya, Heather=heather, Miss Ella=ella, Maisie=maisie, Heather (inside)=heather-inside
// ─────────────────────────────────────────────────────────────────────────────
//  Part 1. Wednesdays. Arcata, California, September 2031. Maya is 33.
//  Owner: after Ireland, a fresh start in a foggy coastal town in Northern California (the owner: a real town, researched:
//  Arcata, on Humboldt Bay). The guide is MISS ELLA (30, she/her), a teacher who runs an after-school club. She was raised
//  religious, left, and built the club because nobody did it for her. The heart of the chapter is the kids, and one of them:
//  MAISIE (13, she/her), who goes quiet and "floaty" and doesn't answer her name. The chapter STARTS AS HEATHER, and it was
//  HEATHER who signed up to volunteer: Maya finds out when Miss Ella thanks her for coming. Unlocks nobody (Serena is saved for
//  another chapter). One school year, 2031-32.
//
//  Research (docs/Chapter9_HumanismResearch.md): Arcata is a small college town on Humboldt Bay, under the redwoods, often in
//  fog. The Plaza is its centre. The Arcata Marsh and the Arcata Community Forest are on either side of town. It's Wiyot land:
//  the Wiyot name for this place is Goudi'ni ("over in the woods").
//
//  My guesses (placeholders), for the owner to correct:
//   - Maya works as an educator at the Natural History Museum of Cal Poly Humboldt (a real, small museum on G Street with a
//     fossil collection and school programs). Her first week.
//   - The school is not named (a real town's real school shouldn't get made-up staff): "the middle school". The club meets in
//     Miss Ella's classroom on Wednesdays, 3:15 to 5:30, with snacks.
//   - Heather's reason, in her own words: somebody should be there at 3:15. Never more than that (her faith stays a mystery).
// ─────────────────────────────────────────────────────────────────────────────

=== ch9_part1 ===
~ set_age(33)
~ set_date(2031, 9, 10)  // a Wednesday (Claude's guess; owner: change it freely)
# cw: childhood-reference
# cw: involuntary-switch
~ explore_tradition("humanism")
~ text_from("tanya", "First week in the fog! Send me a picture of a redwood. Send me a picture of you next to a redwood, for scale.")
-> wake

= wake
// Owner: the chapter starts as Heather.
# place: arcata_apartment
# shot: close
# objective: Make the morning gentle
- (room)
+ {room < 3} [Look: The window]
    Fog to the ground. You can hear the bay but you can't see it. A gull somewhere, complaining. # look: arcata-fog-window
    -> room
+ {room < 3} [Look: The boxes]
    Six still taped. "KITCHEN (Maya)". "BOOKS (everyone)". One in square letters: "CANDLES - CAREFUL". # look: arcata-boxes
    -> room
+ {room < 3} [Look: The sill]
    Mira's stone, the prayer wheel, the khata, a jar of bilberry jam for later. Room for one more thing. # look: arcata-sill
    -> room
* [Use: The kettle]
    Tea, the way she likes it, though she'll never remember making it. Toast cut in triangles. You leave it on the table with a note: "EAT. -H" # look: heather-toast-note
-
-> plaza

= plaza
# place: arcata_plaza
# shot: wide
# objective: Walk into town
- (square)
+ {square < 4} [Look: The Plaza]
    A square of grass with a statue in the middle and old wooden shopfronts all round. People walking dogs in the fog. A bookshop. A café with steamed windows. # look: arcata-plaza
    -> square
+ {square < 4} [Look: The redwoods]
    Above the rooftops, where the town stops, the forest starts, dark and tall and very quiet. # look: redwoods-above-town
    -> square
+ {square < 4} [Look: The café noticeboard]
    Lost cat. Guitar lessons. Room for rent, no smokers. A yellow flyer: "AFTER-SCHOOL CLUB, Wednesdays 3:15. Grown-ups needed. No experience necessary. Just show up. - Miss Ella" # look: club-flyer
    -> square
* {square >= 1} [Use: The flyer (take it)]
-
Somebody should be there at 3:15. You know that better than anyone. # look: flyer-folded
-> office

= office
# place: arcata_middle_school
# shot: over-shoulder
# objective: Sign up
- (desk)
+ {desk < 3} [Look: The office]
    A school office like every school office: a bell schedule, a lost-and-found box of hoodies, a plant somebody waters. # look: school-office
    -> desk
+ {desk < 3} [Look: The hallway]
    Lockers. Paper leaves taped up for September. Somewhere a class reading out loud, badly, together. # look: school-hallway
    -> desk
* [Use: The volunteer form]
-
The form. Name. Availability. "Why do you want to volunteer?" # look: volunteer-form
Heather: Because someone should be there. # vo: ch9-humanism_heather_0001
~ record_choice("ch9.p1.form", "signed", "Heather signed you up to volunteer at Miss Ella's after-school club. On the form, under 'why', she wrote: because someone should be there.")
-> museum

= museum
// Heather goes back inside. Maya comes to at work.
# switch: maya
# place: natural_history_museum
# shot: close
# objective: Get through your first week
~ set_experience("TimeLoss", 0.4)
- (gallery)
+ {gallery < 4} [Look: Your coat]
    A visitor sticker on your coat, with the middle school's name, today's date. You haven't been to a middle school in eighteen years. # look: visitor-sticker
    -> gallery
+ {gallery < 4} [Look: The fossils]
    A long case of them, labelled in a careful old hand: shells and teeth and a whale's ear bone. Two thousand of them, the director says. Your job is to make children care. # look: fossil-case
    -> gallery
+ {gallery < 4} [Look: The sign by the door]
    "The Natural History Museum is on the ancestral land of the Wiyot people. This place is Goudi'ni." # look: goudini-sign
    -> gallery
* {gallery >= 1} [Use: Your phone]
-
A calendar alert you didn't set. "3:15 CLUB. Wednesdays. You'll like them. -H" # look: calendar-heather
~ system_log("heather", "I signed us up. After-school club, Wednesdays. They need grown-ups who show up. I'll come with you. -H")
* [Use: Your phone (go)]
    ~ record_choice("ch9.p1.go", "went", "On your first Wednesday in Arcata, you went to the after-school club Heather signed you up for, without knowing what you'd find.")
* [Use: Your phone (go, but nervous)]
    ~ record_choice("ch9.p1.go", "nervous", "On your first Wednesday in Arcata, you nearly didn't go to the after-school club Heather signed you up for. You went anyway.")
    ~ set_experience("Depersonalization", 0.2)
-
-> club

= club
# place: club_classroom
# shot: wide
# objective: Find Miss Ella
- (room)
+ {room < 4} [Look: The classroom]
    Desks pushed into islands. A whiteboard: "THIS WEEK: WHAT MAKES A GOOD DAY?" A crate of board games held together with tape. # look: club-classroom
    -> room
+ {room < 4} [Look: The kids]
    Fourteen of them, eleven to fourteen years old, very loud and then suddenly not. Two boys arguing about a dragon. # look: club-kids
    -> room
+ {room < 4} [Look: The snack table]
    Goldfish crackers. Apples. A box of granola bars. A kid saying grace over his crackers, eyes shut, and nobody laughing at him. # look: snack-grace
    -> room
* {room >= 1} [Talk: Miss Ella]
-
Miss Ella: Maya! You came! Thank you for coming. You were so sure on the form, I knew you'd turn up. Most people write "maybe". # vo: ch9-humanism_ella_0002
Maya: I... yes. I'm here. What did I write? # vo: ch9-humanism_maya_0003
Miss Ella: "Because someone should be there." I put it on the fridge in the staff room. Don't be mad. # vo: ch9-humanism_ella_0004
* [Talk: Miss Ella (ask about the club)]
    Maya: Why do you run this? You must be so tired by three. # vo: ch9-humanism_maya_0005
    Miss Ella: I was a church kid. Every day of the week. When I left, at nineteen, there was nothing on a Wednesday. No one. So, now there's this. No prayers, no prizes. Just somewhere to be. # vo: ch9-humanism_ella_0006
* [Wait (just say thank you)]
    Maya: Thank you for having me. Us. Me. # vo: ch9-humanism_maya_0007
    Miss Ella: Whoever you brought, they're welcome. # vo: ch9-humanism_ella_0008
-
-> attendance

= attendance
# place: club_classroom
# shot: over-shoulder
# objective: Help with the attendance
Miss Ella: Okay, humans! Roll call! Say "here", or make any noise at all. # vo: ch9-humanism_ella_0009
Miss Ella: Jayden. Priya. Luis. Maisie? Maisie. # vo: ch9-humanism_ella_0010
- (roll)
+ {roll < 3} [Look: The girl by the window]
    Thirteen, maybe. A big grey hoodie, the sleeves over her hands. Looking at the fog like it's a long way off. She doesn't move when her name comes. # look: maisie-window
    -> roll
+ {roll < 3} [Look: Miss Ella]
    She doesn't call it again. She just writes a small tick and moves on, and you see her glance at the window twice. # look: ella-tick
    -> roll
* {roll >= 1} [Talk: Maisie]
-
~ set_experience("Derealization", 0.3)
You know that look. You know it from the inside. A fellowship hall, a long service, the light in the windows. # look: maisie-floaty-mirror
* [Talk: Maisie (say her name, softly)]
    ~ record_choice("ch9.p1.maisie", "name", "On the first Wednesday, Maisie didn't answer her name at roll call. You sat near her and said it once more, softly. She didn't answer. You stayed.")
    Maya: Maisie? I'm Maya. I'm new. I'll just sit here, if that's okay. # vo: ch9-humanism_maya_0011
    Nothing. Then, a long time after, a very small nod. # look: maisie-nod
* [Wait (sit near her)]
    ~ record_choice("ch9.p1.maisie", "sat", "On the first Wednesday, Maisie didn't answer her name at roll call. You didn't say anything. You sat near her by the window until the snack.")
    You sit two desks away and look at the fog too. After a while she looks at you looking at it. # look: maisie-fog-together
-
Heather (inside): Good. Just be there. That's the whole thing. # vo: ch9-humanism_heather-inside_0012
~ nudge(-0.2)
-> night

= night
# place: arcata_apartment
# shot: close
# objective: Go to bed
- (room)
+ {room < 2} [Look: The table]
    The toast from this morning, cold, still in its triangles. You eat it anyway. It's a kind of hello. # look: cold-toast-triangles
    -> room
* [Use: Your phone (write back)]
    ~ system_log("maya", "You signed us up. I'm glad. There's a girl there called Maisie who goes away like we used to. I'm going back next Wednesday. -M")
-
* [Use: The bed]
    The foghorn out on the bay, every thirty seconds, like someone keeping count. # look: foghorn-bed
-
~ text_to("tanya", "Heather signed me up to volunteer at an after-school club. I went. I'm going back. There's a kid there who's a lot like me.")
~ text_from("tanya", "Heather has excellent judgement. So do you. Send me the redwood.")
~ journal("Note", "First Wednesday at the club. Miss Ella: no prayers, no prizes, just somewhere to be. Maisie, by the window, far away.")
-> ch9_part2  // the next part of the same chapter (one run)
