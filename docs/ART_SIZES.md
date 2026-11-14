# Art sizes

3D models (triangles, textures, names): [3D_PIPELINE.md](3D_PIPELINE.md). What to make: [ART_LIST.md](ART_LIST.md).
How to hand it over: [ART_SWAP.md](ART_SWAP.md). This follows Seven Witches' `Docs/ART_SIZES.md`, with this game's differences.

## Colour profile: sRGB (every file)
- Make every Procreate canvas **sRGB IEC61966-2.1** (new canvas → Colour Profile → RGB). **Not Display P3**:
  Unity reads PNGs as sRGB, so P3 art shifts colour, and Procreate can't convert a canvas later.
- Export **PNG**, never JPEG. Draw at the listed size or bigger, keeping the **same shape**.

## Screen shape: landscape everywhere
Unlike Seven Witches (portrait on phones), **As I Am is landscape on phones and PC**, because it's a
cinematic story game. The UI reference canvas is **2732 × 2048, scaled by height** (the same as Seven Witches' PC layout),
so UI art is shared between phone and PC. Phones are wider than 4:3, so keep important things in the middle 2732 px.

## 3D textures
| What | Texture size | Files |
|---|---|---|
| A playable alter / the shared body | 2048 × 2048 | `tex_<id>_color.png` (+ roughness / metallic) |
| Hair and outfits per alter | 1024 × 1024 | `tex_hair_<alter>_color.png`, `tex_outfit_<alter>_color.png` |
| Guides and named people | 2048 × 2048 | `tex_<id>_color.png` |
| Background people | 1024 × 1024 | `tex_<id>_color.png` |
| Close-up props (tarot deck, journal, stones) | 1024 × 1024 | `tex_prop_<name>_color.png` |
| Furniture and small props | 512 × 512 | `tex_prop_<name>_color.png` |
| Rooms / places (walls, floors) | 2048 × 2048, seamless where it repeats | `tex_room_<name>_<surface>.png` |
| Face expression variants (if chosen) | same as the face's texture | `tex_<id>_face_<expression>.png` |

Android compresses textures as ASTC, so painting at these sizes is enough for tablets.

## 2D art
| Asset | px |
|---|---|
| Full-screen backgrounds / story pictures (`bg_<name>.png`) | 4440 × 2048 (keep what matters in the middle 2732 × 2048) |
| Talking-head portraits for dialogue (`portrait_<id>_<expression>.png`) | 1024 × 1024 |
| Tarot card faces (`tarot_<card-id>.png`): standard tarot shape, 2.75 : 4.75 | 1024 × 1768 |
| Tarot card back (`tarot_back.png`) | 1024 × 1768 |
| Rune staves, each rune carved on wood (`rune_<id>.png`) | 512 × 768 |
| Herb, crystal and charm codex pictures (`codex_<id>.png`) | 1024 × 1024 |
| Tradition codex headers (`codex_tradition_<id>.png`) | 2048 × 1024 |
| Item icons (`item_<id>.png`) | 256 × 256 |
| Dialogue box, panels (9-slice) | 512 × 512 |
| Choice buttons, wide buttons (9-slice) | 640 × 200 |
| The Anchor button (+ `_pressed`), always on screen | 384 × 384 |
| Small UI icons | 128 × 128 |
| City maps on the phone (`Resources/Maps/map_<city-id>.png`), portrait 3:4 | 1536 × 2048 |
| Map dot / pin / phone frame | 128 × 128 / 128 × 192 / 1024 × 2048 |
| Phone and laptop icons (right edge) | 256 × 256 |
| Joystick ring / knob, Use button | 512 × 512 / 256 × 256 / 384 × 384 |
| Phone screen / laptop screen (9-slice) | 1024 × 1536 / 1536 × 1024 |
| Message bubbles (9-slice) | 512 × 256 |
| Dissociation mist (`fx_mist.png`, seamless) | 1024 × 1024 |
| Breathing guide (a lantern or circle that grows and shrinks) | 1024 × 1024 |
| Title logo | 2048 wide |
| App icon (no transparency; Android rounds it) | 1024 × 1024 |
| Android adaptive icon: `app_icon_background.png` + `app_icon_foreground.png` (picture inside the middle 60%) | 1024 × 1024 each |

**Photosensitivity:** never draw flashing, high-contrast stripes or flicker frames for effects. Distortion
in this game is always slow and soft (ContentAndSafetyGuidelines.md).

**Tarot art must be original.** Later recolourings of the Rider-Waite-Smith deck are copyrighted. The
structure and symbolism are free to use; the pictures should be yours.

Naming: lowercase, underscores, `.png`. Masters (`.procreate`, `.nomad`) live in `SourceArt/` / Drive → `Masters`.
