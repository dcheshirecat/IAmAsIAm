# Writing the story (Ink)

The story is written in [Ink](https://www.inklestudios.com/ink/), inkle's open-source narrative language.
Write it in **Inky** (inkle's free editor) or any text editor. Files: `Assets/_Project/Story/`.

- `main.ink` is the root. Each chapter is its own file, `INCLUDE`d from main.
- `game_api.ink` declares the game functions and tags (below). Don't remove the fallbacks: they let Inky
  play the story without the game.
- `Drafts/` holds sample scenes that are **not canon** until the owner approves them.

## Lines
```
Narration is a plain line.
Wen: A spoken line starts with the speaker's name and a colon.
Juniper (inside): An inner voice (co-consciousness) adds "(inside)". It's shown in italics.
```

## Tags: interactive moments (the story waits until they're done)
| Tag | Does |
|---|---|
| `# exercise: five-senses` | Runs a grounding/DBT/CBT skill (any skill in skills.json with `hasPracticeMode`) |
| `# ritual: tarot three-card` | The character does a tarot reading (spread ids in tarot_spreads.json). Results go into `tarot_*` variables. |
| `# ritual: rune` / `# ritual: craft rowan-cross` / `# ritual: sachet` | Pulls a rune / crafts a charm step by step / ties a scent sachet |
| `# explore: quakers` | Opens that tradition's codex page and marks it explored |
| `# chapter-end` | Shows the choices recap |

## Tags: instant
| Tag | Does |
|---|---|
| `# cue: raised-voices` | The character meets a trigger cue. If it's a trigger, it fires (the player can't control the outcome). |
| `# cw: time-loss` | A content note (tags in ComfortSettings.cs → ContentTags) |
| `# shot: close` / `# place: meeting_house` / `# front: <alter>` | Hints for the camera, the location, and who's fronting |

## Functions
`set_experience("Derealization", 0.6)`, `nudge(0.3)`, `adjust_trust("juniper", 0.1)`, `learn_skill("five-senses")`,
`record_choice("decision-id", "option-id", "One line for the recap.")`, `journal("Note", "text")`,
`open_gap("gap-id", "clue1,clue2")`, `find_clue("gap-id", "clue1")`, `explore_tradition("quakers")`,
`card_name(tarot_help)`, `card_prompt(tarot_help)`. The variable `fronting` always holds who's fronting, so branch on it to make a
chapter play differently per alter:
```
{ fronting == "bramble": Bramble crosses their arms. | Wren hesitates at the door. }
```

## Checking
`bash tools/InkCheck/check.sh` compiles the story and plays it 300 times with random choices, checking every tag
and id. In Unity: **IAmAsIAm → Compile Story**.
