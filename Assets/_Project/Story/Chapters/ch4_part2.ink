// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 4 (Islam), Part 2.
// vo-speakers: Maya=maya, Reneé=renee
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Reneé. Chicago, October 2026. The body is 28.
//  Owner's beats: Reneé finds out that Maya has moved to Chicago and comes to meet her and catch up. They talk about being queer
//  and ex-religious, and Reneé tells them what she overheard between Mira and Sarah (Prologue, Sunday 2). After they find out,
//  they switch to Nina at home.
//  Reneé: Mr. Carter's daughter, much older than Maya (about 36 now); out bisexual; left the church but still Quaker; uses a manual
//  wheelchair.
//
//  My guesses, for the owner to correct:
//   - Reneé found Maya through an old Church of Friends group online, and Christie's photo of "the best dinosaur guide".
//   - They meet at a café in Andersonville. Izzy is fronting, with Heather.
//   - Nina, at home: eleven years old in a 28-year-old's apartment. She builds a blanket fort and writes in the notebook.
// ─────────────────────────────────────────────────────────────────────────────

=== ch4_part2 ===
~ set_age(28)
# cw: childhood-reference
# cw: neglect
# cw: involuntary-switch
~ co_conscious("heather", true)
~ text_from("renee_carter", "This is going to sound strange. It's Reneé Carter, from the Church of Friends in Iowa City. My dad was Mr. Carter? I think you're in Chicago now. I'm coming up next weekend. Coffee?")
~ text_to("renee_carter", "omg. YES. hi. coffee. yes.")
-> cafe

= cafe
# place: andersonville_cafe
# shot: over-shoulder
# objective: Find Reneé
- (tables)
+ {tables < 4} [Look: The door]
    No step. A wide door. A little blue sign. Reneé picked this place on purpose. # look: step-free-door
    -> tables
+ {tables < 4} [Look: The window table]
    Two mugs already there. A woman with short grey-streaked hair, a manual wheelchair, a leather jacket with a small rainbow pin. # look: renee-table
    -> tables
+ {tables < 4} [Look: The pastry case]
    Cardamom buns. Izzy wants all of them. # look: cardamom-buns
    -> tables
* {tables >= 1} [Talk: Reneé]
-
Reneé: Maya. Oh my god. Last time I saw you, you were up to here, eating every cookie at fellowship hour. # vo: ch4-islam_renee_0015
* [Talk: Reneé (tell her you're Izzy)]
    ~ record_choice("ch4.p2.renee", "izzy", "When Reneé came to Chicago, Izzy told her she wasn't Maya, exactly. Reneé just said: nice to meet you, Izzy.")
    Izzy: I'm... Izzy, right now. It's complicated. Maya's in here. Hi! # vo: ch4-islam_izzy_0016
    Reneé: Okay. Hi, Izzy. I'm a bi Quaker in a wheelchair who quit church and still says "thee" when she's drunk. I can do complicated. # vo: ch4-islam_renee_0017
* [Talk: Reneé (just say hi)]
    ~ record_choice("ch4.p2.renee", "hi", "When Reneé came to Chicago, Izzy just said hi, and let her think she was Maya. For now.")
    Izzy: Hi! Hi. You found me. How did you find me? # vo: ch4-islam_izzy_0018
    Reneé: The old Church of Friends group online. And a photo of you with a dinosaur. Your friend Christie posts a lot. # vo: ch4-islam_renee_0019
-
-> talk

= talk
# place: andersonville_cafe
# shot: close
# objective:
Reneé: So. You left too? # vo: ch4-islam_renee_0020
* [Talk: Reneé (ask about leaving)]
    ~ record_choice("ch4.p2.left", "leaving", "Reneé told you how she left the church, and how she's still a Quaker anyway.")
    Izzy: How did you leave? Like, actually leave? # vo: ch4-islam_izzy_0021
    Reneé: I came out, and the church was so very polite about it. So polite I could feel it in my teeth. I still sit in silence on Sunday mornings. Just not there. # vo: ch4-islam_renee_0022
* [Talk: Reneé (ask about being queer there)]
    ~ record_choice("ch4.p2.left", "queer", "You and Reneé talked about being queer in the church you both grew up in.")
    Izzy: Was it bad? Being queer, there? # vo: ch4-islam_izzy_0023
    Reneé: Nobody shouted. That was the problem. Everyone smiled, and nobody asked. You know what that's like. # vo: ch4-islam_renee_0024
-
Reneé: There's something I've wanted to tell you for twenty years. I didn't know if I should. # vo: ch4-islam_renee_0025
Heather (inside): Hold the stone. Whatever it is. Hold the stone first. # vo: ch4-islam_heather-inside_0026
* [Use: Mira's stone (in your pocket)]
    The stone. The hole through it. Heather's hand and yours, on it. # look: stone-before-news
-
-> overheard

= overheard
// The Prologue's Sunday 2, as Reneé tells it (owner: shown later, when adult Maya meets a childhood companion who overheard it).
# memory: sunday2-overheard
# place: andersonville_cafe
# shot: inner
Reneé: I was sixteen. I was hiding in the coat room with my crutches, avoiding my brother. Mira came in with your mom. They didn't see me. # vo: ch4-islam_renee_0027
Reneé: Mira said you were hungry. Every week. That you were home alone in the evenings. That you went somewhere far away and didn't answer your name. She said she was worried. She was so careful. # vo: ch4-islam_renee_0028
Reneé: And your mom thanked her. Really warmly. And she said, "Maya's very independent." And then Mrs. Hines needed her, and she left. And that was it. # vo: ch4-islam_renee_0029
~ set_experience("Depersonalization", 0.6)
~ set_experience("Derealization", 0.5)
Izzy: She saw. Mira saw. # vo: ch4-islam_izzy_0030
Reneé: She saw. I'm so sorry I never said. I was sixteen, and then I was gone, and then it felt too late. # vo: ch4-islam_renee_0031
* [Talk: Reneé (thank her)]
    ~ record_choice("ch4.p2.news", "thanks", "When Reneé told you what she overheard, you thanked her for telling you at last.")
    Izzy: Thank you. For telling us. It isn't too late. # vo: ch4-islam_izzy_0032
* [Wait (say nothing)]
    ~ record_choice("ch4.p2.news", "silent", "When Reneé told you what she overheard, nobody could say anything. She waited, and held your hand.")
    Reneé's hand on yours, on the table, until the coffee is cold. # look: renee-hand
-
~ journal("Note", "Reneé heard Mira tell Mom. Mom said I was \"very independent\". Mira saw.")
// Owner: herbs and stones from different people (Claude's list, owner: "yes"). No new voiced lines.
Reneé leaves something pink on the table by your cup: a rose quartz, the size of a thumb. She gives one to every friend who's had a bad year. # look: renee-rose-quartz
~ gain_stone("rose-quartz", "renee_carter")
~ text_from("renee_carter", "I'm at my hotel. Text me if you need anything tonight. Anything. I mean it.")
-> home

= home
// Owner: after they find out, they switch to Nina at home.
# shot: inner
# place: black
That evening.
# switch: nina
~ co_conscious("heather", true)
# place: chicago_apartment
# shot: close
# objective: Make it safe
- (nina)
+ {nina < 5} [Use: The blankets (build a fort)]
    The tiger blanket from Nepal, the couch cushions, two chairs. A fort. Nobody can see in. # look: blanket-fort
    -> nina
+ {nina < 5} [Look: The napkin]
    The napkin with Mira's number, taped together very carefully. The tape is old now. # look: napkin-taped
    -> nina
+ {nina < 5} [Look: The fridge]
    A grown-up fridge. Grown-up food. You eat a whole sleeve of crackers, sitting on the floor. # look: crackers-floor
    -> nina
+ {nina < 5} [Look: The notes inside]
    Heather's round letters. Izzy's loopy ones. You find a pen. # look: notebook-nina
    -> nina
* {nina >= 2} [Use: The notebook (write)]
    ~ record_choice("ch4.p2.nina", "wrote", "That night, Nina wrote in the notebook in big letters: MIRA TRIED.")
    Big letters, pressing hard: "MIRA TRIED. -N" # look: mira-tried
-
Nina: Mira tried. She tried. Somebody tried. # vo: ch4-islam_nina_0033
Heather (inside): She did. And we're trying too. Into the fort. I'll stay right here. # vo: ch4-islam_heather-inside_0034
~ nudge(-0.3)
~ set_experience("Depersonalization", 0.0)
~ set_experience("Derealization", 0.0)
# switch: izzy
-> ch4_part3  // the next part of the same chapter (one run)
