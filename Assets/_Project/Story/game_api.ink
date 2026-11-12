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

// True only while this alter is fronting right now. Use it for hidden memories.
=== function fronts(member) ===
~ return member == fronting
