# Art list: everything to make, with file names

Everything is a placeholder until replaced. Tick things off as you go. Sizes: [ART_SIZES.md](ART_SIZES.md).
3D how-to: [3D_PIPELINE.md](3D_PIPELINE.md). Hand-over: [ART_SWAP.md](ART_SWAP.md).

- **First** marks what the first playable test (the pilot) needs.
- **⏳ chapters** marks rows that depend on the chapter plan the owner is deciding (which traditions, in
  what order, which guides). Those rows are filled in once the chapters are set.
- `<id>` is a permanent id. Claude assigns ids as characters and places are written.

## Part 1: Screen mock-ups (landscape, 2732 × 2048; subfolder `Mockups`)
| | File | What |
|---|---|---|
| [ ] First | `mockup_dialogue.png` | Talking: speaker name, line, inner-voice style (co-consciousness), choices |
| [ ] First | `mockup_anchor.png` | The Anchor safe space: breathe, ground, support, return when ready |
| [ ] | `mockup_title.png` | Title, Continue / New / Chapters / Practice / Comfort settings |
| [ ] | `mockup_choose_alter.png` | Choosing who fronts at the start of a chapter |
| [ ] | `mockup_trigger.png` | A trigger moment (how depersonalization, derealization or a switch looks over the scene) |
| [ ] | `mockup_journal.png` | The Inner Journal: notes between parts, gaps in time, reflections |
| [ ] | `mockup_triggers_page.png` | What the player has learned about the triggers so far |
| [ ] | `mockup_tarot.png` | A tarot reading laid out (spread positions, card, prompt) |
| [ ] | `mockup_codex_tradition.png` | A tradition's page: overview, openness, what you can try, practitioner voices |
| [ ] | `mockup_codex_skill.png` | A skill card (steps, cautions, source) |
| [ ] | `mockup_recap.png` | End-of-chapter choices recap |
| [ ] | `mockup_comfort.png` | Comfort settings (distortion cap, camera drift, content notes…) |
| [ ] | `mockup_content_note.png` | A content note before a chapter |

## Part 2: UI (`Assets/_Project/Art/UI`)
| | File | px |
|---|---|---|
| [ ] First | `ui_anchor.png` (+ `_pressed`): the Anchor button, always top right | 384 × 384 |
| [ ] First | `ui_dialogue_box.png` (9-slice) | 512 × 512 |
| [ ] First | `ui_choice.png` (+ `_pressed`, 9-slice) | 640 × 200 |
| [ ] First | `ui_breath_guide.png`: the lantern/circle that grows and shrinks | 1024 × 1024 |
| [ ] | `ui_nameplate.png` (9-slice) | 512 × 256 |
| [ ] | `ui_inner_voice_frame.png`: frame for inner-voice lines (9-slice) | 512 × 512 |
| [ ] | `ui_panel.png` (9-slice) | 512 × 512 |
| [ ] | `ui_btn_wide.png` (+ `_pressed`, 9-slice) | 640 × 200 |
| [ ] | `ui_icon_journal.png`, `ui_icon_codex.png`, `ui_icon_triggers.png`, `ui_icon_settings.png`, `ui_icon_practice.png` | 128 × 128 |
| [ ] | `ui_front_badge_<alter>.png`: small badge showing who's fronting | 256 × 256 |
| [ ] | `ui_logo.png`: title logo "I Am As I Am" | 2048 wide |
| [ ] | `app_icon.png` + `app_icon_background.png` + `app_icon_foreground.png` | 1024 × 1024 each |

## Part 3: People (3D, Nomad → Procreate → Grab3D; `3D Models/<id>/model_<id>.glb`)
| | Id | Who | Notes |
|---|---|---|---|
| [ ] First | `body` | The shared body | 15–20k tris; all alters' looks fit this rig |
| [ ] | `hair_<alter>`, `outfit_<alter>` | Each alter's look (one per alter) | Alters: maya, serena, izzy, nina, lily, heather, jacob, axel (theresa and holden are symbolic and don't front) |
| [ ] | `mother` | Sarah, the mother: single mother, minister of a programmed Friends church in the USA (prologue; in flashbacks / memories) | Younger and older versions? ask the owner |
| [ ] | `ex_christian` | Mark, ex 1, the Christian ex | Chapter 1: Christianity |
| [ ] | `ex_jewish`, `ex_jewish_spouse` | Adeena, ex 2, the Jewish ex, who became religious and married, and her spouse | Chapter 2: Judaism |
| [ ] | `guide_<tradition>` | One guide per chapter, a practitioner of that tradition | ⏳ chapters |
| [ ] | `bg_person_1..6` | Background people (meeting attenders, congregants, passers-by) | idle / walk / talk only |

## Part 4: Places (3D rooms: `model_room_<id>.obj` + textures)
| | Id | Place | Notes |
|---|---|---|---|
| [ ] First | `home_kitchen` | The character's flat: kitchen | The Boot scene's placeholder room |
| [ ] | `home_bedroom`, `home_living` | The flat: bedroom, living room with altar shelf | The Wiccan altar lives here |
| [ ] | `hearth` | The inner world: cottage kitchen, hearth, apothecary shelves | Frame between chapters |
| [ ] | `friends_church` | A programmed Friends church (pulpit, pews, hymn boards) | Prologue: Sarah's church, USA. Both kinds of meeting exist in this world |
| [ ] | `meeting_house` | An unprogrammed meeting house (plain room, benches facing each other in a square) | Prologue |
| [ ] | `stone_circle` | Outdoor circle for sabbats | Wicca |
| [ ] | One place per chapter's tradition (e.g. `synagogue`, `church`, `mosque`, `mandir`, `gurdwara`, `buddhist_centre`, `daoist_temple`, `shinto_shrine`, `bahai_centre`, `druid_grove`, `heathen_hof`) | ⏳ chapters | Accurate layout: check each with a practitioner |
| [ ] | `daily_<place>` | Daily-life places (work, bus, café, therapist's office…) | ⏳ chapters |

## Part 5: Props (3D, no rig: `model_prop_<id>.obj` + `tex_prop_<id>_color.png`)
| | Id | What |
|---|---|---|
| [ ] First | `kettle`, `mug`, `journal`, `pen` | Everyday anchors |
| [ ] First | `tarot_deck`, `tarot_card` (one card mesh; faces are 2D art, Part 6) | Tarot is played in the story |
| [ ] | `altar_cloth`, `candle`, `chalice`, `bowl`, `wand` | The Wiccan altar |
| [ ] | `rune_pouch`, `rune_stave` | Runes |
| [ ] | `herb_<id>` × 14, `herb_bundle_dried` | Herbs (ids in herbs.json) |
| [ ] | `crystal_<id>` × 13 | Stones (ids in crystals.json) |
| [ ] | `charm_<id>` × 9 | Charms (ids in charms.json) |
| [ ] | Ritual objects per tradition (e.g. Shabbat candlesticks, prayer beads, a turban/rumal for head covering) | ⏳ chapters; check with practitioners |

## Part 5b: Dialogue portraits (2D, `Character Art - Finished/Talking Heads`)
Face paintings are used on the 3D models in the world; these 2D portraits sit beside the dialogue box.
| | File | px |
|---|---|---|
| [ ] First | `portrait_<alter>_neutral.png` for one alter (the pilot) | 1024 × 1024 |
| [ ] | `portrait_<id>_<expression>.png`: neutral, happy, sad, worried, blank, tense, for each alter (maya, serena, izzy, nina, lily, heather, jacob, axel), sarah, mark, adeena, and each guide | 1024 × 1024 |
| [ ] | Faces on the 3D models: `tex_<id>_face_<expression>.png` (see 3D_PIPELINE.md, Faces) | same as `tex_<id>_color.png` |

## Part 6: 2D card and codex art
| | File | px |
|---|---|---|
| [ ] First | `tarot_back.png` | 1024 × 1768 |
| [ ] | `tarot_<card-id>.png` × 22 Major Arcana (ids in tarot_major.json, e.g. `tarot_major-17-star.png`) | 1024 × 1768 |
| [ ] | `rune_<id>.png` × 24 (ids in runes.json) | 512 × 768 |
| [ ] | `codex_<id>.png` for herbs, crystals and charms | 1024 × 1024 |
| [ ] | `codex_tradition_<id>.png` per tradition | 2048 × 1024 |

## Part 7: Effects (made in-engine; reference art welcome)
Fog, flattened derealization light, depersonalization camera drift, co-consciousness edge tint in the alter's
colour. Reference paintings for each go in `Guidelines` (`ref_fx_<name>.png`). No flashing, ever.
