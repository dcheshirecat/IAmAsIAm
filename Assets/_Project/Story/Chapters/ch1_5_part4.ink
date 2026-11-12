// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 1.5 (Axel), Part 4.
// ─────────────────────────────────────────────────────────────────────────────
//  Part 4. The napkin. Champaign-Urbana, May 2019. The body is 20.
//  Owner's beats: Mark accidentally breaks something from Maya's childhood: he tears Mira's napkin (the phone number from
//  the prologue's Sunday 2). Axel is gone the next morning. (Chapter 1 Part 2 is that morning, as Maya.)
//  Finishing this chapter unlocks Axel.
//
//  My guesses, for the owner to correct:
//   - It happens on an ordinary, happy evening: end-of-year packing, music on, Mark helping take things off the walls.
//   - Axel doesn't know why it hurts so much. He says it's fine. It isn't.
// ─────────────────────────────────────────────────────────────────────────────

=== ch1_5_part4 ===
~ set_age(20)
# cw: time-loss
# cw: involuntary-switch
-> packing

= packing
# place: college_apartment_axel
# shot: close
# objective: Pack up junior year
Mark: Last box! Okay, not last. There are always more boxes. Boxes are the hydra of moving. # vo: ch1-5-axel_mark_0041
- (walls)
+ {walls < 3} [Look: The photos]
    Pride. Christmas Eve, both of you holding candles. Easter, Mark crying a little, laughing at himself. # look: year-photos
    -> walls
+ {walls < 3} [Look: The corkboard]
    Ticket stubs. A party flyer. In the corner, under everything, the napkin with Mira's number. # look: corkboard-napkin
    -> walls
* [Talk: Mark (let him take the corkboard down)]
-
Mark: I'll get the pins out. Hold on... # vo: ch1-5-axel_mark_0042
A sound like paper. # look: napkin-tear
Mark: Oh no. Oh, no, I'm so sorry. It was stuck to the pin. Was that important? It looks old. # vo: ch1-5-axel_mark_0043
~ set_experience("Depersonalization", 0.7)
~ set_experience("Derealization", 0.5)
~ nudge(0.45)
* [Talk: Mark (say it's fine)]
    ~ record_choice("ch1_5.p4.napkin", "fine", "When Mark tore the napkin, you told him it was fine. It wasn't, and you didn't know why.")
    Axel: It's fine. It's just a napkin. It's not even mine. I don't... it's fine. # vo: ch1-5-axel_axel_0044
* [Look: The two halves]
    ~ record_choice("ch1_5.p4.napkin", "halves", "When Mark tore the napkin, you just looked at the two halves for a long time.")
    Two halves. "Mi" on one. "ra" on the other. # look: napkin-halves
* [Use: The tape]
    ~ record_choice("ch1_5.p4.napkin", "tape", "When Mark tore the napkin, you taped it back together very carefully, and didn't say anything.")
    Tape. Very straight. Very careful. Somebody else's careful hands. # look: napkin-tape
-
Mark: Hey. Come here. Whatever it was, I'm sorry. # vo: ch1-5-axel_mark_0045
-> night

= night
# place: college_apartment_axel
# shot: close
# objective:
* [Look: Mark, asleep]
    Mark, asleep, one arm over his face. # look: mark-asleep
-
* [Look: The ceiling]
    The ceiling. A long time. # look: ceiling-axel
-
Axel: Goodnight, Mark. # vo: ch1-5-axel_axel_0046
~ set_experience("TimeLoss", 1.0)
-> morning

= morning
# shot: inner
# place: black
...
// Owner: Axel is gone the next morning.
May 2019. # chapter-end
-> END
