# Handing art over

The same process as Seven Witches. Everything in the game is a placeholder (coloured blocks and plain panels)
until you replace it. For what to make, see [ART_LIST.md](ART_LIST.md); for sizes, [ART_SIZES.md](ART_SIZES.md); for 3D, [3D_PIPELINE.md](3D_PIPELINE.md).

## The usual way: the Google Drive folder "Game Art - I Am As I Am"
1. **Make it** in Procreate (sRGB) or Nomad → Procreate → Grab3D, at the listed size.
2. **Name it exactly** as in ART_LIST.md (lowercase, underscores). The name is how the game knows what it is.
3. **Put it in the matching subfolder**:
   - `UI`: buttons, panels, icons, logo, app icon.
   - `Mockups`: drawings of whole screens (`mockup_<name>.png`) for Claude to build screens from.
   - `Character Art - Finished` → `Talking Heads` (if portraits are used) / `Full Body` (reference turnarounds).
   - `Character Art - Sketches`: work in progress. Claude looks at these but never puts them in the game.
   - `3D Models`: one folder per id (`3D Models/wren/`), holding the Grab3D `model_<id>.glb`, and for props `model_<prop>.obj` + `tex_` PNGs.
   - `World and Places`: room textures, backgrounds, story pictures.
   - `Tarot and Runes`: card faces, the card back, rune staves.
   - `Voice-Over`: your recorded voice lines (MP3), one folder per chapter. Its own read-me and [VOICE.md](VOICE.md) explain names and settings.
   - `Guidelines`: notes and style references for Claude (read before swapping art in).
   - `Masters`: `.procreate` and `.nomad` files you want kept (Claude copies them to `SourceArt/`).
4. **Tell Claude** "new art is in the Drive folder". Claude copies each file over the placeholder of the same
   name (keeping its `.meta`), checks size, shape, colour profile and transparency, wires in anything new,
   commits, and sends a screenshot.

## Big files
GLB/OBJ files are often too big for the Drive connector. Either use **Google Drive for desktop** (Claude reads
the files from the disk), or upload to GitHub in the browser: `IAmAsIAm` → `Assets/_Project/Art/Models` →
Add file → Upload files, typing `<id>/` first to make the folder.

## Other ways
- Send PNGs to Claude directly in a message, named as in ART_LIST.md.
- On the PC: overwrite the placeholder in `Assets/_Project/Art/...` with your file (same name, same shape).
  Don't delete the old file first, or its `.meta` (which holds every link to it) can be lost.
