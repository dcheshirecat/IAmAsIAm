// IN THE GAME as a DRAFT (Claude, 2026-11-14; the owner's Doc edits come first). Chapter 10 (Unitarian Universalism), Part 3.
// vo-speakers: Maya=maya, Anabelle=anabelle, Serena (inside)=serena-inside, Heather (inside)=heather-inside, Lily (inside)=lily-inside
// ─────────────────────────────────────────────────────────────────────────────
//  Part 3. Flower Communion. Arcata, June 2033. Maya is 34.
//  Owner: a spring Flower Communion; the chapter is about belonging; it ends by unlocking Serena.
//
//  Research: the Flower Communion was created by Norbert Čapek, the founder of the Unitarian church in Prague, in 1923. Everyone
//  brings a flower; they all go into one big vase; at the end everyone takes home a different flower than the one they brought.
//  Many UU congregations hold it in late spring.
//
//  My guesses, for the owner to correct:
//   - Maya brings an armful: one different flower for each of them in the system. She doesn't explain. Nobody asks.
//   - Sarah's letter arrives in April. Maya hasn't opened it yet; it waits on the altar, under Mira's stone, until she's ready.
//     (What it says, and whether she ever opens it, is the owner's to decide.)
// ─────────────────────────────────────────────────────────────────────────────

=== ch10_part3 ===
~ set_age(34)
~ set_date(2033, 6, 5)  // a Sunday (Claude's guess)
# cw: religious-content
~ co_conscious("serena", true)
~ text_from("tanya", "Flower Communion today? I did one at a UU church in Champaign once. I took home a carnation and cried at a red light. Enjoy.")
-> wake

= wake
# place: arcata_apartment
# shot: close
# objective: Pick the flowers
- (room)
+ {room < 3} [Look: The altar]
    Sarah's letter, still sealed, under Mira's stone. It's been there since April. Serena says it can wait as long as it likes. # look: letter-under-stone
    -> room
+ {room < 3} [Look: The window]
    June, and the fog burned off by nine. Nasturtiums all over the neighbour's fence, orange as a warning. # look: nasturtium-fence
    -> room
* [Use: The scissors (go to the yard)]
-
-> garden

= garden
# place: arcata_apartment
# shot: wide
# objective: One flower for each of you
- (yard)
+ {yard < 3} [Look: The yard]
    The landlord's overgrown yard, and the neighbour's, and a bit of the alley that nobody owns. # look: overgrown-yard
    -> yard
* [Use: The scissors (cut the flowers)]
-
A rose for Heather, because she'd never ask for one. A daisy for Lily. A sunflower for Izzy, loud. A bit of lavender for Nina, quiet. Rosemary for Jacob, for remembrance, even if he'd say it's a pagan thing. A bright red poppy for Axel. Yarrow for Serena, who's tough and plain and good for wounds. And one for you, Maya. You can't decide. You pick a nasturtium, orange as a warning. # look: armful-of-flowers
Serena (inside): Theresa and Holden too, if you like. Just a bit of green for each. # vo: ch10-unitarian-universalism_serena-inside_0027
~ record_choice("ch10.p3.flowers", "everyone", "For the Flower Communion you picked one flower for each of you in the system, and a bit of green for Theresa and Holden.")
~ nudge(-0.2)
-> communion

= communion
# place: uu_fellowship
# shot: wide
# objective: Put your flowers in the vase
- (vase)
+ {vase < 3} [Look: The big vase]
    A steel washtub, really, on the communion table, already half full: supermarket tulips, roadside lupins, one plastic flower from a kid who was very clear about it. # look: washtub-vase
    -> vase
+ {vase < 3} [Look: The people]
    The old man in the bow tie with one perfect rose. The Buddhist with a lotus she must have driven to Eureka for. Maisie, in the back with her grandmother, holding a fistful of dandelions. # look: communion-people
    -> vase
* {vase >= 1} [Use: Your armful (put them in)]
-
Ten flowers, all at once. Somebody in the front row says "oh!" A child counts them out loud. Nobody asks why. # look: ten-flowers-in
* [Talk: Anabelle]
-
Anabelle: That's one each, isn't it? For all of you? # vo: ch10-unitarian-universalism_anabelle_0028
Maya: Is that... weird? # vo: ch10-unitarian-universalism_maya_0029
Anabelle: Maya. You just did the most Unitarian Universalist thing I've ever seen. It's in the newsletter. I'm writing it right now. # vo: ch10-unitarian-universalism_anabelle_0030
-> take

= take
# place: uu_fellowship
# shot: over-shoulder
# objective: Take a flower home
The minister tells the story. A church in Prague, 1923, a minister who wanted a communion that belonged to everybody. Bring something. Take something different home. # look: capek-story
- (choose)
+ {choose < 3} [Look: The washtub]
    Everyone's flowers, all mixed up now. You can't tell whose is whose. That's the point. # look: flowers-mixed
    -> choose
* [Use: A flower (take one)]
    ~ record_choice("ch10.p3.take", "took", "At the Flower Communion you took a flower home that wasn't yours: a single, slightly bent dandelion.")
    A dandelion, slightly bent. Maisie, from the back row, gives you a thumbs up. # look: bent-dandelion
-
Serena (inside): That's belonging, I think. You bring your strange armful, and they hand you back a dandelion. # vo: ch10-unitarian-universalism_serena-inside_0031
Lily (inside): I like the yellow one. # vo: ch10-unitarian-universalism_lily-inside_0032
-> coffee

= coffee
# place: uu_fellowship
# shot: close
# objective: Stay for coffee hour
- (cups)
+ {cups < 2} [Look: The noticeboard]
    "NEW MEMBERS: the book is by the door. No test. No creed. Just your name." # look: membership-book-notice
    -> cups
* [Talk: Anabelle]
-
Anabelle: So. You've been to a gurdwara and a Daoist temple and a well in Ireland, and now us. Are you staying? # vo: ch10-unitarian-universalism_anabelle_0033
* [Talk: Anabelle (say you're staying)]
    ~ record_choice("ch10.p3.stay", "staying", "When Anabelle asked if you were staying at the fellowship, you said yes: all of you.")
    Maya: I think we are. All of us. It's the first place nobody's asked me to pick one. # vo: ch10-unitarian-universalism_maya_0034
    Anabelle: Good. We'd have missed the flowers. # vo: ch10-unitarian-universalism_anabelle_0035
* [Talk: Anabelle (say you don't know yet)]
    ~ record_choice("ch10.p3.stay", "maybe", "When Anabelle asked if you were staying at the fellowship, you said you didn't know yet, and they said that was a fine answer too.")
    Maya: I don't know yet. I've never stayed anywhere. # vo: ch10-unitarian-universalism_maya_0036
    Anabelle: "Don't know yet" is a whole theology here. Take your time. The coffee's always bad. # vo: ch10-unitarian-universalism_anabelle_0037
-
-> night

= night
# place: arcata_apartment
# shot: close
# objective: Put the dandelion somewhere
- (room)
* [Use: The altar (put the dandelion there)]
    In a jam jar, next to Mira's stone, next to Sarah's letter, next to Maisie's drawing. A bent dandelion holding court. # look: dandelion-altar
-
Heather (inside): It looks right there. # vo: ch10-unitarian-universalism_heather-inside_0038
Serena (inside): It does. Leave the letter where it is. There's no hurry. There's never been a hurry, actually. We just didn't know. # vo: ch10-unitarian-universalism_serena-inside_0039
~ system_log("serena", "A dandelion on the altar. A letter we haven't opened. A church that didn't ask anyone to leave. Good day. -S")
~ text_to("tanya", "I brought one flower for each of us. They didn't ask why. I took home a dandelion. I think I belong somewhere, Tanya.")
~ text_from("tanya", "You've always belonged with me. I'm glad you found a few more places. Water the dandelion. It'll pretend not to need it.")
~ journal("Note", "Flower Communion. Ten flowers in, one dandelion out. Nobody asked me to pick one faith. Serena says there's no hurry, and there never was.")
# shot: inner
# place: black
Arcata, California. June 2033. # chapter-end
-> END
