// DRAFT SAMPLE: not canon. Written before the chapter plan; kept only for the owner to review or discard.
// ─────────────────────────────────────────────────────────────────────────────
//  Chapter 1: Samhain, "The Kettle Is Cold"
//  Wren loses an afternoon, finds clues, meets Wen, and finds the door.
//  Featured skill: 5-4-3-2-1 grounding. Practice: a first tarot card.
// ─────────────────────────────────────────────────────────────────────────────

=== samhain ===
# cw: time-loss
# cw: derealization
# place: apartment
# shot: close
-> wake

= wake
~ open_gap("samhain-afternoon", "receipt,rosemary,deck,note")
~ set_experience("TimeLoss", 1.0)
The kettle is cold.

Wren's hand is resting on it, flat, like it's been there a while. The window is the wrong colour. It was bright a moment ago. It's nearly dark now.
~ set_experience("Derealization", 0.6)
~ nudge(0.35)

The kitchen looks like a photograph of a kitchen. Very clear, and very far away.

* (clock) [Look at the clock.]
    The oven clock says 16:42.
    The last thing Wren remembers is putting the kettle on. That was lunchtime.
* (phone) [Check your phone.]
    Two messages sent to Mum at 14:10. Wren doesn't remember writing them. They're polite and normal and slightly too cheerful.
    "Sounds lovely, see you Sunday x."
* (sit) [Sit down before your legs decide for you.]
    Wren sits on the kitchen floor with their back against the cupboard. The lino is cold through their jeans. That's something.
-
{not clock: Somewhere behind the fog, Wren knows it's later than it should be. Hours later.}

Wren: Not again. Not again, not again.
-> ground

= ground
# shot: inner
Then, from somewhere that isn't the room, a voice. Not out loud. Warm, low, with a kitchen in it.
~ set_experience("CoConsciousness", 0.3)
Juniper (inside): Easy, love. Look around you. Five things you can see.

* [Listen to the voice.]
    ~ record_choice("samhain.voice", "listened", "When the voice spoke, you listened.")
    ~ adjust_trust("juniper", 0.1)
    Wren doesn't know whose voice it is. It doesn't matter right now. It sounds like it knows the way out.
* [Ignore it. Voices are a bad sign.]
    ~ record_choice("samhain.voice", "ignored", "When the voice spoke, you tried to ignore it.")
    Wren squeezes their eyes shut. But the voice just waits, patiently, the way a kettle waits.
    Juniper (inside): Five things, love. Whenever you're ready.
-
Five things you can see. # exercise: five-senses
~ learn_skill("five-senses")
~ set_experience("Derealization", 0.15)
~ nudge(-0.25)

The kitchen comes back slowly, the way feeling comes back into a cold hand. It's just a kitchen again. Wren's kitchen.
~ set_experience("CoConsciousness", 0.0)
-> clues

= clues
# shot: wide
The afternoon has left things behind, like a tide.

- (clue_loop)
* (receipt) [The receipt on the counter.]
    ~ find_clue("samhain-afternoon", "receipt")
    A paper receipt, slightly crumpled. THE CROSSROADS: BOOKS, HERBS & CURIOS. 13:52.
    One tarot deck, one bundle of dried rosemary, one pot of chamomile.
    Wren has never been inside The Crossroads. Wren has walked past it a hundred times.
    -> clue_loop
* (rosemary) [The sprig of rosemary.]
    ~ find_clue("samhain-afternoon", "rosemary")
    A sprig of rosemary, laid carefully on the windowsill, as if someone wanted it to be found.
    Wren picks it up without thinking. Rubs it. The smell is sharp and green and clean.
    Something about it is like being remembered.
    -> clue_loop
* (deck) [The box on the table.]
    ~ find_clue("samhain-afternoon", "deck")
    A tarot deck, the cellophane neatly peeled off and folded, the way Wren would never bother to.
    Wren turns the top card over. It shows a figure kneeling by water under a sky full of stars.
    -> clue_loop
* {receipt or rosemary or deck} [The note, folded under the deck.]
    -> note
* {not (receipt and rosemary and deck)} [Leave the rest. That's enough for now.]
    Wren doesn't have to know everything about the afternoon right now. Some of it can stay missing.
    ~ record_choice("samhain.clues", "left-some", "You left some of the afternoon unexplored.")
    -> note

= note
~ find_clue("samhain-afternoon", "note")
# shot: close
It's on the back of a shopping list, in handwriting that's almost Wren's. Rounder. Patient.

"We're tired of being a secret from you. We're not going anywhere. That's a good thing, we promise. You don't have to do anything. Just maybe come and see. J."

~ nudge(0.2)
Wren reads it three times. Their heart is going fast.

* [Keep it. Put it in your pocket.]
    ~ record_choice("samhain.note", "kept", "You kept the note.")
    ~ adjust_trust("juniper", 0.1)
    Wren folds it small and puts it in their pocket, where they can feel it.
* [Put it in the drawer. Not today.]
    ~ record_choice("samhain.note", "drawer", "You put the note in the drawer for another day.")
    Wren puts it in the drawer with the takeaway menus and closes it. Not today.
    Not today is allowed.
* [Write back underneath it.]
    ~ record_choice("samhain.note", "wrote-back", "You wrote back.")
    ~ adjust_trust("juniper", 0.2)
    ~ learn_skill("system-journal")
    Wren finds a pen. Their hand is shaking a little.
    Underneath, they write: "Who are you?"
    ~ journal("Note", "Who are you?")
    It's the first thing Wren has ever written to someone who lives in the same body.
-
The kettle is still cold. Wren switches it on again, and listens to it start to hum.
Tomorrow, Wren will go to The Crossroads.
-> END
