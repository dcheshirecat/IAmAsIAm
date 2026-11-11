# The 3D pipeline: Nomad Sculpt → Procreate → Grab3D → the game

**Owner's choice (2026-11-11):** the same pipeline as Seven Witches (`dcheshirecat/SevenWitches`,
`Docs/3D_PIPELINE.md`), with **Grab3D Auto-Rig Studio** for rigging. The steps and names match Seven Witches,
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
1. **Upload** the painted model to Grab3D Auto-Rig Studio and place the markers (chin, wrists, elbows, knees, groin).
2. **Add the animations** this game needs (list below), picking the closest Grab3D clips.
3. **Export one GLB** with the model, its textures and all animations inside: `model_<id>.glb`.
   Name the clips inside the GLB with the `<anim>` names below where Grab3D allows it. If it doesn't,
   tell Claude which clip is which.
4. Props and rooms aren't rigged: export them from Procreate as OBJ + `tex_` PNGs (`model_prop_<name>.obj`, `model_room_<name>.obj`).

**Non-humanoid creatures** (birds for Sorrel's birdsong, a cat, Pip's "creature friends"): the humanoid
auto-rigger can't rig these. Rig them in **Blender** (as Seven Witches decided), or keep them as simple
animated props. **[DECIDE]** which creatures the game needs.

### Animations this game needs
This is a story game, so it's mostly small, human movements, not combat. **[ASK]** the owner which clips Grab3D offers; the
Mixamo search words are a fallback for finding a similar clip.

| `<anim>` | What | Who | Mixamo search words (fallback) |
|---|---|---|---|
| `idle` | Standing, breathing | everyone | Breathing Idle |
| `walk` | Walking (in place) | everyone | Walking |
| `talk` | Talking with small gestures | everyone with lines | Talking |
| `listen` | Listening, a small nod | everyone with lines | Idle, Head Nod Yes |
| `sit` / `sit_idle` | Sitting down / seated | alters, guides | Sitting, Sitting Idle |
| `kneel` | Kneeling (gardening, Shinto bow, altar work) | alters | Kneeling Idle |
| `pick_up` | Picking up an object | alters | Picking Up |
| `hold_object` | Holding something close (grounding object) | alters | Holding Idle |
| `self_hug` | Arms around self | alters | (closest: Sad Idle) |
| `shuffle_cards` | Shuffling / laying out cards | alters | (closest: Typing / Counting) |
| `write` | Writing in the journal | alters | Writing |
| `bow` | A bow (shrines, greeting) | alters, guides | Bow |
| `pray_hands` | Hands together / hands open | guides, alters | Praying |
| `startle` | A flinch (trigger moments) | alters | Hit Reaction (small), Startled |
| `freeze` | Going still (dissociation) | alters | Idle (with arms down) |
| `dazed` | A slow, unsteady sway (depersonalization) | alters | Drunk Idle (slowed) |
| `look_around` | Looking around, disoriented (after a switch) | alters | Looking Around |
| `sleep` / `wake` | Lying asleep / waking | alters | Sleeping Idle, Getting Up |

Background people only need `idle`, `walk` and `talk`.

### Faces **[DECIDE]**
Close-up story scenes need expression. In order of effort:
1. **Expression textures** (cheapest): paint 4–6 face variants (`tex_<id>_face_<neutral|happy|sad|worried|blank>.png`), and the game swaps them.
2. **2D talking-head portraits** beside the dialogue, like Seven Witches (1024 × 1024).
3. **Blend shapes** (shape keys) sculpted in Blender: the most expressive, and the most work.

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

All names are lower case with underscores and no spaces. GLB/FBX/OBJ files are big, and the Drive connector struggles with them,
so use **Google Drive for desktop**, or upload straight to GitHub (`Assets/_Project/Art/Models/<id>/` → Add file → Upload files),
exactly as in Seven Witches. The repo stores `.glb`, `.fbx`, `.obj`, `.usdz`, `.nomad` and `.blend` with Git LFS.

## Pilot **[DECIDE]**
As in Seven Witches, prove the pipeline on **one** character first (suggestion: the shared body with one
alter's look) in the Boot scene's placeholder room, before making the rest.
