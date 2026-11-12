// ─────────────────────────────────────────────────────────────────────────────
//  The bridge between the story and the game. See docs/StoryWriting.md.
//
//  FUNCTIONS change game state instantly. They're bound by StoryRunner.cs; the
//  fallbacks below let Inky and tools/InkCheck play the story without the game.
//
//  TAGS hand control to an interactive moment. The story pauses until it's done:
//    # exercise: five-senses | paced-breathing | feet-on-floor | orienting
//    # ritual: tarot <spread-id> | rune | craft <charm-id> | sachet
//    # explore: <tradition-id>      (opens the codex entry for that tradition)
//    # cw: <content-tag>            (content note; the player may skip with a summary)
//    # cue: <cue-id>                (meets a trigger cue: dissociation starts, no switch)
//    # cue-held: <cue-id>           (can't get away from it: dissociation, then a switch)
//    # shot: wide | close | over-shoulder | inner     (camera hint)
//    # place: <location-id>         (scene/location change)
//    # chapter-end                  (shows the choices recap)
//    # vo: <chapter>_<speaker>_<0000>  (this line's voice file, vo_<...>.mp3; added by tools/StoryDocs/voice_script.py)
//    # objective: <text>            (the goal line at the top of the screen, in the fronting alter's words)
//    # look: <object-id>            (a close-up on an object; the line is just its label)
//    # memory: <id>                 (a hidden memory scene starts; only while the alter it belongs to is fronting, see fronts())
//  HIDDEN MEMORIES (owner): gate them with fronts("<member>"), never unlocked(). The thing that opens one only
//  glows (only exists as a choice) while that alter is fronting. Never hint that another alter is needed.
//    # maybe-switch[: <percent>]    (an unexpected switch MAY happen here, to an unlocked alter; default 15%)
//    # device: computer|phone|laptop  (opens that device's messages; the story waits until it's closed)
//
//  DEVICES (owner): the family computer at 12; a phone and a laptop at 18. Call set_age(<Maya's age>) at the start of
//  every chapter part, which gives her exactly the devices she has at that age. Texts need the phone; email needs a
//  computer. Contacts are people ids (people.json).
//  LOST TIME ON DEVICES (owner): after a blackout, an extra sent message may be there that Maya didn't write:
//  sent_while_away(<member>, "text"|"email", <contact>, <text>). It's never pointed out: very subtle at first.
//  APPS (owner): gain_app("systemlog") puts SystemLog on her phone (the end of Chapter 4). system_log(<member>, <text>)
//  leaves a SIGNED note there from that member (the system talking to itself; the player can write notes too).
//  SHARING TIME (owner): co_conscious(<member>, true/false): an alter is present alongside whoever fronts (co-consciousness).
//  THE CRAFT (owner): gain_book_of_shadows("tanya") at Yule 2017 puts the Book of Shadows beside the phone.
//  learn_practice(<craft.json id>, <teacher people id or "">) writes a practice's steps in it; practised(<id>) asks.
//  bos_entry(<member>, <Practices|Readings|Wheel|Dreams|Notes>, <title>, <text>) writes in it, in that member's hand
//  (lost time: never pointed out). Only Maya and Izzy practise Wicca (craft.json); for anyone else the Book stays shut.
//  gain_herb(<herbs.json id>, <giver>) / gain_stone(<crystals.json id>, <giver>): someone gives her a herb or a stone
//  (owner: she collects them from different people; they go in her charm bags; the Book's "Herbs & stones" page lists them).
//  honour_ancestor(<people id>): someone she honours at the ancestor altar from now on (owner: Mira, her grandparents,
//  people she loses later). New practices (step 4): # ritual: pendulum / tea-leaves / oracle / scrying / ancestor-altar /
//  offering / sigil / dream-journal.
//  gift_charm_bag(<giver>, <herb id>, <stone id>, <intent>) puts a charm bag someone made her in her pocket (she can hold it to
//  ground while she's drifting; alters react to it). Practice moments: # ritual: runes / charm-bag / circle-casting /
//  cleansing (or "cleansing hard": the moment stirs something up).
//
//  EXPLORING (owner: little narration). A place is a hub of sticky choices the 3D scene turns into
//  glowing things to look at, use or talk to:  + [Look: The stone]  + [Use: The phone]  + [Talk: Mira]
//  SPEAKERS: start a line with "Name: ". Lines without a name are narration.
//  Inner voices (co-consciousness) use "Name (inside): ".
// ─────────────────────────────────────────────────────────────────────────────

// Ritual results the game writes back (card ids from tarot_major.json).
VAR tarot_spread = ""
VAR tarot_today = ""
VAR tarot_was = ""
VAR tarot_is = ""
VAR tarot_help = ""
VAR fronting = "maya"

EXTERNAL set_experience(kind, amount)
EXTERNAL nudge(amount)
EXTERNAL adjust_trust(member, amount)
EXTERNAL learn_skill(skill)
EXTERNAL record_choice(decision, option, summary)
EXTERNAL journal(kind, text)
EXTERNAL open_gap(gap, clues)
EXTERNAL find_clue(gap, clue)
EXTERNAL explore_tradition(tradition)
EXTERNAL card_name(card)
EXTERNAL card_prompt(card)
EXTERNAL unlocked(member)
EXTERNAL fronts(member)
EXTERNAL set_age(age)
EXTERNAL gain_device(device)
EXTERNAL text_from(contact, text)
EXTERNAL text_to(contact, text)
EXTERNAL email_from(contact, subject, text)
EXTERNAL email_to(contact, subject, text)
EXTERNAL sent_while_away(member, channel, contact, text)
EXTERNAL gain_app(app)
EXTERNAL system_log(member, text)
EXTERNAL co_conscious(member, present)
EXTERNAL gain_book_of_shadows(giver)
EXTERNAL learn_practice(practice, teacher)
EXTERNAL bos_entry(member, tab, title, text)
EXTERNAL practised(practice)
EXTERNAL gift_charm_bag(giver, herb, stone, intent)
EXTERNAL gain_herb(herb, giver)
EXTERNAL honour_ancestor(person)
EXTERNAL set_date(year, month, day)
EXTERNAL gain_stone(stone, giver)

=== function set_experience(kind, amount) ===
~ return

=== function nudge(amount) ===
~ return

=== function adjust_trust(member, amount) ===
~ return

=== function learn_skill(skill) ===
~ return

=== function record_choice(decision, option, summary) ===
~ return

=== function journal(kind, text) ===
~ return

=== function open_gap(gap, clues) ===
~ return

=== function find_clue(gap, clue) ===
~ return

=== function explore_tradition(tradition) ===
~ return

=== function card_name(card) ===
~ return "The Star"

=== function card_prompt(card) ===
~ return "What small hope can you hold onto tonight?"

// True once this alter's version of any chapter is unlocked in this save (or it's fronting now).
=== function unlocked(member) ===
~ return member == fronting

=== function set_age(age) ===
~ return

=== function gain_device(device) ===
~ return

=== function text_from(contact, text) ===
~ return

=== function text_to(contact, text) ===
~ return

=== function email_from(contact, subject, text) ===
~ return

=== function email_to(contact, subject, text) ===
~ return

=== function sent_while_away(member, channel, contact, text) ===
~ return

=== function gain_app(app) ===
~ return

=== function system_log(member, text) ===
~ return

=== function co_conscious(member, present) ===
~ return

// True only while this alter is fronting right now. Use it for hidden memories.
=== function fronts(member) ===
~ return member == fronting

// The Book of Shadows (owner: Tanya gives it at Yule, December 2017; only Maya and Izzy practise Wicca).
=== function gain_book_of_shadows(giver) ===
~ return

// She learns a practice (an id from craft.json); its steps go in the Book, with who taught her (a people.json id, or "").
=== function learn_practice(practice, teacher) ===
~ return

// The story writes in the Book, in someone's hand. tab: Practices, Readings, Wheel, Dreams or Notes.
=== function bos_entry(member, tab, title, text) ===
~ return

// Someone gives her a herb (herbs.json id) or a stone (crystals.json id), to keep for her charm bags.
=== function gain_herb(herb, giver) ===
~ return

=== function gain_stone(stone, giver) ===
~ return

// The story's date (owner: a moon phase always shown; Tanya texts on the sabbats). At the start of every chapter part,
// after set_age, and again when a part jumps ahead (e.g. before a sabbat scene).
=== function set_date(year, month, day) ===
~ return

// Someone she honours at the ancestor altar from now on (a people.json id).
=== function honour_ancestor(person) ===
~ return

// Someone gives her a charm bag (herb and stone ids, e.g. "lavender", "smoky-quartz"). It goes in her pocket, and she
// keeps the herb and the stone for her own bags.
=== function gift_charm_bag(giver, herb, stone, intent) ===
~ return

// Has she ever learned this practice?
=== function practised(practice) ===
~ return false
