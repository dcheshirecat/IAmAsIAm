# Voice-over: recording and handing over

You voice parts of the game yourself. Recordings go in the Google Drive folder
"Game Art - I Am As I Am" → **Voice-Over**, the same way art goes in its folders ([ART_SWAP.md](ART_SWAP.md)).

## The folders in Drive (Voice-Over)
- `00 Prologue (prologue-quaker)` … `14 Chapter 13 Eastern Orthodox Christianity (ch13-eastern-orthodox)`: one folder per chapter.
- `Daily Life and Inner Voices (not tied to a chapter)`: the frame between chapters, inner voices heard anywhere, the Anchor.
- `Scripts (what to record)`: Claude puts the line lists here (one per chapter, with each line's file name).
- `Retakes (replace a line already sent)`: a new take of a line that's already in the game, with the **same file name**.
- `Masters (WAV originals, optional)`: your full-quality originals, if you want them kept. The game doesn't use these.

## What to record
Not every line has to be voiced. A line with a voice file plays it; every other line stays text only.
Claude marks voiced lines in the story and lists them in `Scripts`. If you record a line that isn't on a list, that's fine:
give it a clear name and say which line it is.

## File names
`vo_<chapter-id>_<speaker>_<number>.mp3`, all lowercase, no spaces. For example:
- `vo_prologue-quaker_mira_0012.mp3`
- `vo_prologue-quaker_maya-05_0003.mp3` (Maya at 5; the prologue uses `maya-05`, `maya-08`, `maya-11`, `maya-15`)
- `vo_daily_nina-inside_0001.mp3` (an inner voice: add `-inside`, like the story's "Nina (inside)")
The number is the line's number on the script list. Never reuse a number for a different line.

## Sound settings
- **MP3, mono, 44.1 kHz (or 48 kHz), 192 kbps or better** (constant bitrate is best).
- One line per file. Leave about **a quarter of a second of quiet** before and after.
- **No music or effects**; the game adds those (and the dissociation effects: muffling, distance, echo).
- Loudness: aim for normal speaking level, peaks no higher than about −1 dB (not clipping). Claude evens out the volume.
- A quiet room with soft things around (a wardrobe of clothes works well). The same mic and spot for the whole game if you can.
- Inner voices ("inside") are recorded **dry and close**, the same as spoken lines; the game makes them sound inside.

## Safety and comfort while recording
This game touches hard things. Record in short sessions, take breaks, and skip any line you don't want to voice:
it stays as text and the game still works. Nothing has to be voiced.

## How the numbers work (for Claude)
Each spoken line in the story carries a tag with its file name, e.g. `Mira: Hello, floor. # vo: prologue-quaker_mira_0036`.
`tools/StoryDocs/voice_script.py` adds tags to new lines and writes the script list. A line keeps its number when its words
change, and numbers are never reused, so a recording always belongs to one line. If a line's words change after it was
recorded, its row in the script list says so, and it can be re-recorded into Retakes with the same name.
Narration (lines without a speaker) isn't voiced. Voices are never generated: every voice is the owner's own recording.

## Telling Claude
Say "new voice lines are in the Drive folder". Claude copies each file into `Assets/_Project/Resources/Voice/<chapter-id>/`,
checks its name, format and loudness, links it to its line in the story, and tells you which lines are now voiced.
A line in Retakes replaces the old file of the same name.
