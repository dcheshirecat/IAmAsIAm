// IN THE GAME (owner, 2026-11-12: push the story into the game). Still a draft for the owner: their Doc edits come first. Prelude (Wicca), Part 3.
// vo-speakers: Maya=maya
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Tanya. The same town, December 2017 (Yule, sophomore year). Maya is 19.
//  (Owner: Yule 2017, so Maya and Tanya have time to build a solid relationship before Axel fronts in junior year.)
//  Owner's beats: at 19 (the owner moved it to Yule 2017) Maya gets serious, through a shop and a coven: an open, eclectic
//  teaching coven with public sabbat circles (only open practice shown). The guide is the coven elder,
//  Tanya (42, they/them, kind but strict). Lost time keeps growing, and Maya still doesn't understand it.
//  Explore-first: the shop and the circle carry it. Tanya teaches grounding first, always.
//
//  My guesses, for the owner to correct:
//   - Maya finds the shop's flyer in her own bag ("Open Yule circle. All welcome.") and goes.
//   - Tanya runs the shop and the teaching coven; they're the person behind the counter Izzy met in Part 2.
//     Tanya's strictness: phones off, ground first, no skipping steps, "you can't cast a circle if you're
//     not in the room."
//   - Tanya notices Maya drifting and asks where she went, gently; Maya says she's fine. Tanya doesn't
//     push yet. Tanya is the guide who stays (owner): in touch all game, mostly by text and email.
//   - The public Yule circle is in the park: the longest night, every candle out, then lit one by one.
//   - (Owner: yes, Izzy) The chapter ends with a switch to Izzy at the circle: Maya loses the rest of the night, and wakes
//     with a Book of Shadows whose first page is in round handwriting, signed "I".
// ─────────────────────────────────────────────────────────────────────────────

=== prelude_part3 ===
~ set_age(19)
~ set_date(2017, 12, 14)  // Claude's guess at the exact day (owner: change it freely)
# cw: religious-content
# cw: depersonalization
# cw: time-loss
# cw: involuntary-switch
-> flyer

= flyer
# place: college_apartment_maya
# shot: close
# objective:
* [Look: Your bag]
    A flyer, folded in four, at the bottom of your bag. "The Circle. Open Yule circle, December 21st. All welcome." # look: yule-flyer
-
* [Look: The back of the flyer]
    "GO." Round letters. Underlined three times. # look: flyer-go
    ~ set_experience("Derealization", 0.25)
-
~ journal("Note", "The Circle. Yule, the 21st.")
-> finals

= finals
// Owner: more of each part of Maya's day. Claude's scene: finals week, and Mom about Christmas.
# place: college_library
# shot: wide
# objective: Study for your last final
- (study)
+ {study < 3} [Look: The tables]
    Midnight, and every table full. Energy drinks, highlighters, someone asleep on a textbook. # look: finals-tables
    -> study
+ {study < 3} [Look: Your notes]
    Your notes, neat, and in the margin, in round letters: "breathe!!" # look: notes-breathe
    -> study
* [Use: Your phone]
    ~ text_from("mother", "Christmas is all hands here, Christmas Eve AND Christmas Day. Maybe come the 27th? We'll figure it out! Love, Mom")
    Your phone buzzes on the table. # look: phone-buzz-finals
-
* [Use: Your phone (answer Mom)]
    ~ record_choice("prelude.p3.christmas", "stay", "In finals week Mom asked you to come home on the 27th. You told her you'd stay on campus over break.")
    ~ text_to("mother", "It's ok. I'll stay here for break. Merry Christmas.")
    "It's ok. I'll stay here for break." Sent. She hearts it. # look: mom-heart
* [Use: Your notes (don't answer yet)]
    ~ record_choice("prelude.p3.christmas", "later", "In finals week Mom asked you to come home on the 27th. You didn't answer, and studied instead.")
    Back to the notes. You'll answer later. You don't. # look: notes-later
-
-> shop

= shop
# place: shop_the_circle
# shot: wide
# objective: Find the circle
- (shelves)
+ {shelves < 4} [Look: The shelves]
    Herbs in jars, labelled by hand. Candles by colour. A wall of tarot decks. # look: shop-shelves
    -> shelves
+ {shelves < 4} [Look: The notice board]
    "Intro to the Craft, Thursdays, 7 pm. Ground first. Always. - T." # look: notice-board
    -> shelves
+ {shelves < 4} [Look: The bell over the door]
    A brass bell. It sounds like your old church's, only smaller. # look: shop-bell
    ~ set_experience("Depersonalization", 0.2)
    -> shelves
* [Talk: The person at the counter]
-
Tanya: You're here for Yule? Good. Phone off, please, not on silent, off. I'm Tanya, they/them. # vo: prelude-wicca_tanya_0030
* [Talk: Tanya (say it's your first time)]
    ~ record_choice("prelude.p3.first", "first", "When you met Tanya, you told them it was your first circle.")
    Maya: It's my first time. At one of these. I'm Maya. # vo: prelude-wicca_maya_0031
    Tanya: Hm. Is it? # vo: prelude-wicca_tanya_0032
    Tanya: Well. Then you'll do it properly. Everyone does it properly, first time or fortieth. # vo: prelude-wicca_tanya_0033
* [Talk: Tanya (ask what to do)]
    ~ record_choice("prelude.p3.first", "asked", "When you met Tanya, you asked them what you were supposed to do.")
    Maya: I'm Maya. What do I do? I don't want to get it wrong. # vo: prelude-wicca_maya_0034
    Tanya: You'll get some of it wrong. Everybody does. What I won't have is anybody skipping steps. # vo: prelude-wicca_tanya_0035
-
-> ground

= ground
# shot: close
# objective: Ground first
Tanya: Before anything else, we ground. You can't cast a circle if you're not in the room. # vo: prelude-wicca_tanya_0036
Tanya: Feet flat. Feel the floor push back. Now imagine roots, going down, as far as they need to. # vo: prelude-wicca_tanya_0037
Roots, down through the floor. # exercise: feet-on-floor
~ learn_skill("feet-on-floor")
~ learn_practice("grounding-centering", "tanya")
~ nudge(-0.15)
Tanya: Good. Where did you go, just then? Before your feet found the floor. # vo: prelude-wicca_tanya_0038
* [Talk: Tanya (say you're fine)]
    ~ record_choice("prelude.p3.where", "fine", "When Tanya asked where you'd gone, you said you were fine.")
    Maya: Nowhere. I'm fine. I'm always a bit spacey. # vo: prelude-wicca_maya_0039
    Tanya: Mm. Spacey. All right. # vo: prelude-wicca_tanya_0040
* [Talk: Tanya (say you don't know)]
    ~ record_choice("prelude.p3.where", "unsure", "When Tanya asked where you'd gone, you told the truth: you didn't know.")
    Maya: I don't know. Sometimes I'm just... not there. # vo: prelude-wicca_maya_0041
    Tanya: That's an honest answer. I'll take an honest answer over a pretty one, any day. # vo: prelude-wicca_tanya_0042
-
Tanya: Thursdays, seven o'clock, if you want to learn. You'd have to come every week. I mean every week. # vo: prelude-wicca_tanya_0043
~ journal("Note", "Tanya. Thursdays, 7. Every week. Ground first.")
-> circle

= circle
~ set_date(2017, 12, 21)
# place: park_yule_circle
# shot: wide
# objective: Join the circle
- (park)
+ {park < 3} [Look: The circle]
    Twenty people in coats, in a ring in the snow. Children too. A dog. # look: yule-ring
    -> park
+ {park < 3} [Look: The candles]
    A candle in every hand. None of them lit. # look: yule-candles
    -> park
* [Use: Your place in the ring]
-
Tanya: The longest night. Let it be dark for a moment. Nobody has to fill it. # vo: prelude-wicca_tanya_0044
~ set_experience("Depersonalization", 0.5)
~ nudge(0.3)
# maybe-switch
* [Look: The dark]
    Dark. Breath, in clouds. Somebody's hand, somewhere near yours. # look: yule-dark
* [Use: Feet on the floor]
    ~ record_choice("prelude.p3.dark", "roots", "In the dark at Yule, you tried Tanya's roots, and your feet found the snow.")
    Snow under your boots. Roots, going down. # look: yule-roots
    ~ nudge(-0.1)
* [Use: Mira's stone]
    ~ record_choice("prelude.p3.dark", "stone", "In the dark at Yule, you held Mira's stone.")
    The stone, cold through your glove. # look: yule-stone
-
Tanya: And now the light comes back. One flame, passed on. # vo: prelude-wicca_tanya_0045
* [Look: The flame coming around the ring]
    Hand to hand. Closer. Somebody's face, lit gold. # look: flame-coming
-
// Owner: Tanya gives Maya her Book of Shadows, at Yule, after the circle (just before Izzy comes forward).
Tanya: Here. Everyone gets one after their first circle. A Book of Shadows. Write in it every week. I'll know if you don't. # vo: prelude-wicca_tanya_0046
* [Use: The book (take it)]
    ~ gain_book_of_shadows("tanya")
    ~ record_choice("prelude.p3.book", "taken", "After the Yule circle, Tanya gave you a Book of Shadows.")
    Black cloth, a moon on the cover. First page, in Tanya's square letters: "Ground first." # look: bos-gift
    Tucked inside the cover: three dry bay leaves and a bead of amber, warm as a hand. For the sun coming back. # look: bos-bay-amber
    ~ gain_herb("bay", "tanya")
    ~ gain_stone("amber", "tanya")
    ~ learn_practice("dream-journal", "tanya")
    ~ learn_practice("sabbat", "tanya")
-
~ set_experience("TimeLoss", 1.0)
-> gone

= gone
# shot: inner
...
# switch: izzy
~ open_gap("prelude-yule", "book,wax,ticket")
// During the lost night (owner: subtle): Tanya's text arrives, and a reply Maya never wrote is already under it.
~ text_from("tanya", "Thank you for staying to help stack the chairs. Thursday, 7. Phone off.")
~ sent_while_away("izzy", "text", "tanya", "I'll be there. Thank you. Really.")
// The first entry in the Book is Izzy's (owner: lost time leaves practice behind; never pointed out).
~ bos_entry("izzy", "Wheel", "Yule", "Longest night. Everyone in a ring in the snow, then the light came back, hand to hand. Stacked the chairs after. Kept a candle stub. Blessed be. -I")
...
# switch: maya
# place: college_apartment_maya
# shot: close
# objective: What happened?
- (after)
+ {not book} [Look: Your bag] -> book
+ {not wax} [Look: Your hands] -> wax
+ {not ticket} [Look: Your coat pocket] -> ticket
+ [Use: Your phone] -> phone
* {book or wax or ticket} [Use: Feet on the floor] -> end

= book
~ find_clue("prelude-yule", "book")
~ record_choice("prelude.p3.after", "book", "After Yule, someone had written in the Book of Shadows Tanya gave you. It wasn't your handwriting.")
Tanya's book, in your bag. Under "Ground first.", in round, loopy letters that aren't yours: "Book of Shadows. Blessed be. -I" # look: book-of-shadows
-> gone.after

= wax
~ find_clue("prelude-yule", "wax")
Candle wax on your fingers, peeled into a little moon. # look: wax-fingers
-> gone.after

= ticket
~ find_clue("prelude-yule", "ticket")
A receipt from The Circle. "Intro to the Craft, 8 weeks. PAID." # look: class-receipt
-> gone.after

= phone
Your phone. # device: phone
-> gone.after

= end
~ set_experience("TimeLoss", 0.0)
~ nudge(-0.2)
Thursday. Seven o'clock. # chapter-end
-> END
