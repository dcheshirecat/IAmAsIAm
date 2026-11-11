# The 3D pipeline: Nomad Sculpt → Procreate → Grab3D → the game

**Owner's choice (2026-11-11):** the same pipeline as Seven Witches (`dcheshirecat/SevenWitches`,
`Docs/3D_PIPELINE.md`), with **Grab3D Auto-Rig Studio** for all rigging and animation. **Grab3D replaces Mixamo,
Blender and any other rigging/animation tool**: no Mixamo, no Blender, no FBX hand-offs. The only tools are
Nomad Sculpt → Procreate → Grab3D. The steps and names match Seven Witches,
so anything learned on one game works on the other. What's different here is the list of models, the
animations and the budgets: this is a close-camera **story game** in landscape on phones and PC.
For the list of what to make, see [ART_LIST.md](ART_LIST.md). For sizes, see [ART_SIZES.md](ART_SIZES.md).

---

## 1. Sculpt and prepare in Nomad Sculpt (iPad)
1. **Sculpt** each person in a **T-pose or A-pose**: arms away from the body, legs a little apart, facing forward, feet flat.
   The auto-rigger needs to see the arms, legs and head clearly.
2. **Keep it game-sized.** This game's camera comes **closer** than Seven Witches' top-down view (dialogue
   close-ups), so faces and hands matter more. Budgets (use **Remesh** / **Decimate** before unwrapping):
   | What | Triangles |
   |---|---|
   | A playable alter (the body everyone shares, plus each alter's look) | 15,000–20,000 |
   | Guides and named people (chapter teachers, the exes, family) | 10,000–15,000 |
   | Background people | 4,000–8,000 |
   | Props you pick up and look at closely (tarot deck, stones, journal, kettle) | 500–3,000 |
   | Furniture and small props | 200–1,500 |
   | A whole room or place of worship (everything in it) | under 60,000 |
   Small details belong in the painting, not the mesh.
3. **One closed mesh** works best for the rigger: join the body, clothes and hair, with no floating pieces inside.
4. **UV unwrap** in Nomad: Misc menu → UV → **Unwrap**. Procreate can't paint a model without UVs.
5. **Export** as **.OBJ** or **.USDZ**, named `model_<id>` (e.g. `model_wren.obj`).
6. Keep the `.nomad` file. It's the master (Drive → `Masters`).

### One body, many people: the alters
The alters share **one body**, but each alter can *feel* different in the body. In the game this shows in
posture, clothing choices, hairstyle, how they move and the camera height they "feel"; the body itself stays the same.
The plan is **one rigged body** (`model_body`), with **hair and clothing as separate models** fitted to the same
skeleton (`model_hair_<alter>`, `model_outfit_<alter>`), which the game swaps on a switch. This is the same
approach Seven Witches planned for its customisable player. **[DECIDE]** with the owner: does each alter
change clothes in-story (they'd have to change physically), or is the look shown only in the inner world?

## 2. Paint in Procreate (iPad)
1. Import the OBJ/USDZ and **paint directly on the model**. Use **sRGB** colours (not Display P3, see ART_SIZES.md).
2. **Paint the light softly, not the time of day.** The game does its own lighting, and the dissociation
   effects (fog, flattened light) depend on that. Gentle hand-painted shading is the look.
3. **Faces:** paint the face neutral. Expressions are a separate decision (see "Faces" below).
4. **Export:** Actions → Share → **Share 3D Model** → OBJ (or USDZ), plus the texture PNGs, renamed:
   - `tex_<id>_color.png` (always)
   - `tex_<id>_roughness.png`, `tex_<id>_metallic.png` (only if Procreate exported them)

## 3. Rig and animate in Grab3D Auto-Rig Studio (browser)
Checked on grab3d.com/tools/auto-rig (2026-11-11). It runs in the browser, and your model never leaves the device.
1. **Load** the painted model (GLB, glTF, FBX, OBJ, or a ZIP with the textures). Use a standing T- or A-pose with the arms
   away from the hips. The tool checks orientation and height in metres.
2. **Markers:** check chin, shoulders, elbows, wrists, knees and groin; drag any that are off. **Create rig** builds a
   **19-bone humanoid skeleton** with volume-based skin weights (robes, hair and loose clothes follow the body).
3. **Weights:** test poses (T pose, arms up, squat, stride, twist), and fix with the weight brush if needed.
4. **Animate:** tick **In place** and **Keep feet on the ground**, and select every clip (list below).
5. **Export:** format **GLB**, bone names **Unity Humanoid**, file name `model_<id>.glb`, with all animations in the file.
   (Unity Humanoid naming lets the game use Unity's humanoid features: head and eye look-at, hand placement, and the poses below.)
6. Props and rooms aren't rigged: export them from Procreate as OBJ + `tex_` PNGs (`model_prop_<name>.obj`, `model_room_<name>.obj`).

**What Grab3D can't do (by its own page):** humanoids only, with two legs, two arms and a head, so there are no animals, wings or tails.
There are **no finger bones** (the hand moves as one piece) and **no face rig**. Animals therefore stay unrigged props that the game moves
simply (a bob, a hop, a turn).

### The animations Grab3D offers (all 7, CC0 public domain)
| Grab3D clip | Use in this game |
|---|---|
| `idle` (2 s, loops) | Standing, breathing: everyone |
| `walk` (1 s, loops) | Walking: everyone |
| `run` (0.6 s, loops) | Walking fast (Shift / LT) |
| `walk-cartoon`, `run-cartoon` | Not needed (stylised/chibi); skip unless the art style goes that way |
| `wave` (1.6 s, once) | Greetings |
| `jump` (1.1 s, once) | A startle at a sudden bang (shortened and blended, so the feet stay down) |

### Story poses the game makes itself (no other tool)
A story game needs more than seven clips. These are made **inside Unity** from the Humanoid rig (posed bones plus look-at),
blended over `idle`. They need nothing beyond your Grab3D GLB:
`sit` / `sit_idle`, `kneel`, `bow`, `hands_together` (prayer), `hands_open`, `hold_object` (grounding stone, journal),
`self_hug`, `shuffle_cards` (both hands forward), `write`, `listen` (a small nod), `talk` (gentle head and hand movement),
`freeze` (dissociation: going still), `dazed` (slow sway, depersonalization), `look_around` (after a switch), `lie_down`.
Hands stay as one piece (no fingers), so hold poses are designed around that.

### Faces **[owner to confirm]**
Grab3D has **no face rig**, so sculpted expressions aren't possible with this pipeline. That leaves two options:
1. **Swapped face paintings (recommended):** paint the face area in Procreate in a few versions, all with the same UVs:
   `tex_<id>_face_neutral.png`, `_happy`, `_sad`, `_worried`, `_blank` (dissociated, far away), `_tense`.
   Each is a full colour texture identical to `tex_<id>_color.png` except the face, and the game swaps between them.
   This suits the close camera and needs only Procreate.
2. **2D talking-head portraits** beside the dialogue box (as in Seven Witches, 1024 × 1024), which can be added on top of option 1.

## 4. Into the game (Claude does this part)
You put the files in Drive (below) and tell Claude. Claude:
1. copies them to `Assets/_Project/Art/Models/<id>/` (`model_<id>.glb`, or `.obj` + `tex_` PNGs),
2. imports the GLB through **glTFast** (vendored in `Packages/`, as in Seven Witches), builds an Animator controller from the clips (`anim_<id>.controller`), and loops idle, walk and the seated clips,
3. makes URP materials with `tex_<id>_color.png` as Base Color (plus roughness and metallic if present),
4. swaps the placeholder blocks for the model with the same id, and sends you a screenshot.

## Google Drive: "Game Art - I Am As I Am"
This works the same way as "Game Art - Seven Witches", with the same subfolders:
`UI`, `Mockups`, `Character Art - Finished` (`Full Body`, `Talking Heads`), `Character Art - Sketches`,
`3D Models` (one folder per id: `3D Models/wren/`), `World and Places`, `Tarot and Runes`, `Guidelines`, `Masters`.

| File | Where it comes from | Needed? |
|---|---|---|
| `model_<id>.glb` | Grab3D: the rigged model with its animations | **Yes** (people) |
| `model_<prop>.obj` + `tex_<prop>_color.png` | Procreate (props and rooms, no rig) | **Yes** (props) |
| `tex_<id>_*.png` | Procreate texture maps, renamed | If not inside the GLB |
| `<id>.nomad` | Nomad Sculpt master | Optional → `Masters` |

All names are lower case with underscores and no spaces. GLB/OBJ files are big, and the Drive connector struggles with them,
so use **Google Drive for desktop**, or upload straight to GitHub (`Assets/_Project/Art/Models/<id>/` → Add file → Upload files),
exactly as in Seven Witches. The repo stores `.glb`, `.obj`, `.usdz` and `.nomad` with Git LFS.

## Pilot **[DECIDE]**
As in Seven Witches, prove the pipeline on **one** character first (suggestion: the shared body with one
alter's look) in the Boot scene's placeholder room, before making the rest.
