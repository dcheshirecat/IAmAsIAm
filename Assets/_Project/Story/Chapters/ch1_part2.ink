// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Chapter 1 (Christianity), Part 2.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 2. Coming back. Champaign-Urbana, May 2019 (the end of junior year). Maya is 20.
//  Owner's beats: Maya comes back after the most lost time so far, to her body "dressed up as a man". She's scared and
//  confused. She starts taking the lost time seriously. (The breakup with Mark isn't played: it's explained in senior year.)
//  Care: Axel is real and his transition is his own; Maya is a woman, and undoing it is about who is fronting, never a
//  judgement on trans people. Nothing here says Axel was wrong.
//  Explore-first: the room and the phone carry it. Very little narration.
//
//  My guesses, for the owner to correct:
//   - She comes back in her own room (an apartment Axel moved into), in front of the mirror on the closet door.
//   - What she finds: short hair; a binder; men's clothes; letters addressed to "Axel"; a phone full of texts from Mark
//     ("love you, A"), and dozens of unread invitations from Tanya that Axel ignored.
//   - She takes the binder off, and puts on the one dress left at the back of the closet.
// ─────────────────────────────────────────────────────────────────────────────

=== ch1_part2 ===
~ set_age(20)
~ set_date(2019, 5, 6)  // Claude's guess at the exact day (owner: change it freely)
# cw: time-loss
# cw: depersonalization
# cw: derealization
# cw: detransition
# cw: panic
// Junior year, on Maya's phone: Axel's life with Mark, and Tanya's invitations, all ignored (owner).
~ text_from("tanya", "Samhain at the shop on the 31st. Potluck. Bring nothing, or a photo of someone you miss.")
~ text_from("tanya", "Yule is the 21st. Your seat's saved.")
~ email_from("tanya", "Imbolc, and a reading if you want one", "Maya, it's been a while. Imbolc is on the 1st. If you'd like a reading, or soup, or both, my door is open. No need to explain anything. T.")
~ text_from("tanya", "Thinking of you. Thursday, 7, if you want it.")
~ text_from("ex_christian", "Morning, A. Coffee before class?")
~ text_from("ex_christian", "Last night was really nice. Love you.")
~ text_from("ex_christian", "Mom says you're welcome at Easter again, she loved you at Christmas")
-> mirror

= mirror
# place: college_apartment_axel
# shot: close
# objective: Where am I?
~ set_experience("Depersonalization", 0.8)
~ set_experience("Derealization", 0.6)
~ nudge(0.45)
# cue: own-reflection
- (room)
+ {not glass} [Look: The mirror] -> glass
+ {not mail} [Look: The desk] -> mail
+ {not closet} [Look: The closet] -> closet
+ [Use: Your phone] -> phone
* {glass and (mail or closet)} [Use: Feet on the floor] -> steady

= glass
~ record_choice("ch1.p2.back", "mirror", "When you came back, the first thing you saw was the mirror: short hair, a man's shirt, a face you knew and didn't.")
Short hair. A man's shirt, buttoned to the top. Something tight around your chest. # look: mirror-axel
Maya: What... what is this. Who did this. # vo: ch1-christianity_maya_0012
-> mirror.room

= mail
~ record_choice("ch1.p2.back", "mail", "When you came back, there was mail on the desk addressed to a name you didn't know: Axel.")
Envelopes. "Axel." "Axel." Your address. Your last name. # look: mail-axel
~ set_experience("Derealization", 0.8)
-> mirror.room

= closet
~ record_choice("ch1.p2.back", "closet", "When you came back, your closet was full of men's clothes. One dress was left, at the very back.")
Men's jeans. Flannel shirts. At the very back, one dress, still in dry-cleaner plastic. # look: closet-dress
-> mirror.room

= phone
Your phone. # device: phone
-> mirror.room

= steady
~ nudge(-0.2)
~ set_experience("Derealization", 0.3)
* [Use: The binder (take it off)]
    ~ record_choice("ch1.p2.undo", "binder", "When you came back, you took off the binder first. You could breathe.")
    The binder, off. Breath, all the way down. # look: binder-off
* [Use: The dress (put it on)]
    ~ record_choice("ch1.p2.undo", "dress", "When you came back, you put on the one dress left in the closet.")
    The dress, cold from the plastic. It still fits. # look: dress-fits
* [Wait (sit on the floor)]
    ~ record_choice("ch1.p2.undo", "floor", "When you came back, you sat on the floor for a long time and didn't change anything yet.")
    The floor. Your back against the bed. A long time. # look: floor-long
-
# objective: Find out what happened
* [Look: The calendar on your phone]
    May 2019. The last thing you remember is the spring of 2018. # look: may-2019
-
~ journal("Note", "May 2019. A whole year. Mail for someone called Axel. Texts from someone called Mark. I don't remember any of it.")
~ open_gap("ch1-junior-year", "mirror,mail,closet")
~ find_clue("ch1-junior-year", "mirror")
Maya: A whole year. # vo: ch1-christianity_maya_0013
-> ch1_part3  // the next part of the same chapter (one run)
