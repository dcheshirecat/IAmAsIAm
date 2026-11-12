# Art list: everything to make, with file names

Everything in the game is a placeholder (coloured blocks, plain panels, shapes drawn in code) until you replace it.
Sizes: [ART_SIZES.md](ART_SIZES.md). 3D how-to: [3D_PIPELINE.md](3D_PIPELINE.md). Hand-over: [ART_SWAP.md](ART_SWAP.md).
**Scene by scene** (every place, who is in it, every close-up, generated from the chapters): [ART_BY_SCENE.md](ART_BY_SCENE.md).

- **In story order (owner, 2026-11-12):** places and people are listed in the order they first appear, so you can make
  them chronologically: the Prologue first, then the Prelude, Chapter 1, Chapter 1.5, Chapter 2. New chapters add rows.
- **First** = what the Prologue needs (the first chapter you play).
- **Wiring:** *Auto* = drop the file in and the game uses it. *Claude* = the game draws a placeholder in code for now; tell
  Claude when the file is in and it gets connected (same name, same shape).
- `<id>` is a permanent id (people.json, places.json, system_members.json). Claude adds ids as chapters are written.

## Part 1: Places (3D rooms and outdoor spaces), in story order
One model per place: `model_room_<place-id>.obj` + `tex_room_<place-id>_<surface>.png` (Procreate, no rig; see
3D_PIPELINE.md). Places in the same building (the church's three rooms, the museum's three) can share one model with
separate rooms; say so in `Guidelines` and Claude splits it. Outdoor places need ground, edges (trees, buildings) and sky.
What's IN each place (the props and close-ups) is in [ART_BY_SCENE.md](ART_BY_SCENE.md) part 3.

| | # | Place id | Place | Chapter | Notes |
|---|---|---|---|---|---|
| [ ] First | 1 | `home_kitchen` | Home: the kitchen (Iowa City, 2003-2013) | Prologue | Sarah's house. A kitchen table, a cupboard Maya climbs |
| [ ] First | 2 | `car` | Mom's car | Prologue | Inside a family car, back seat; used at ages 5, 8 and 15 |
| [ ] First | 3 | `church_first_day_school` | Church of Friends: First Day School | Prologue | A Sunday-school room for small children |
| [ ] First | 4 | `church_sanctuary` | Church of Friends: the sanctuary | Prologue | A **programmed** Friends church: pulpit, pews, hymn boards |
| [ ] First | 5 | `church_fellowship_hall` | Church of Friends: the fellowship hall | Prologue | Folding tables, snacks, coffee urn |
| [ ] First | 6 | `home_maya_room` | Home: Maya's room | Prologue | Seen at 11 and 15: it should age with her (two dressings) |
| [ ] | 7 | `college_lecture_hall` | The lecture hall (University of Illinois) | Prelude | Tiered seats |
| [ ] | 8 | `college_dorm_maya` | Maya's dorm room | Prelude, Ch 1 | Freshman dorm; again in 2020 (lockdown) |
| [ ] | 9 | `college_dorm_selene` | Selene's dorm room | Prelude | Samhain altar on the desk |
| [ ] | 10 | `college_apartment_maya` | Maya's apartment, Champaign-Urbana | Prelude, Ch 1, Ch 1.5 | Her things; the tarot deck |
| [ ] | 11 | `college_quad` | The Quad | Prelude, Ch 1 | Outdoor |
| [ ] | 12 | `shop_the_circle` | The Circle (Tanya's shop) | Prelude, Ch 1 | Shelves of decks, candles, herbs, a counter |
| [ ] | 13 | `park_yule_circle` | The park: the Yule circle | Prelude | Outdoor, December, night, candles |
| [ ] | 14 | `park_ostara` | The park: Ostara | Ch 1 | The same park in spring (a second dressing of 13) |
| [ ] | 15 | `college_apartment_axel` | The apartment in Axel's year | Ch 1, Ch 1.5 | The same apartment as 10, Axel's things |
| [ ] | 16 | `campus_ministry` | The campus ministry | Ch 1 | The coat drive |
| [ ] | 17 | `campus_ministry_chapel` | The campus ministry chapel | Ch 1 | Small Catholic chapel |
| [ ] | 18 | `campus_ministry_lawn` | The campus ministry lawn | Ch 1 | Outdoor prayer service, 2020 |
| [ ] | 19 | `campus_barber` | The barber | Ch 1.5 | Axel's haircut |
| [ ] | 20 | `thrift_store` | The thrift store | Ch 1.5 | Racks of clothes, a mirror |
| [ ] | 21 | `campus_cafe` | The campus café | Ch 1.5 | |
| [ ] | 22 | `town_pride` | Pride downtown | Ch 1.5 | Outdoor, crowd, flags |
| [ ] | 23 | `methodist_church` | The Methodist church (Indianapolis) | Ch 1.5 | Christmas Eve, candles |
| [ ] | 24 | `mark_family_guest_room` | Cassie's house: the guest room | Ch 1.5 | |
| [ ] | 25 | `episcopal_church` | St. Luke's Episcopal | Ch 1.5 | Ash Wednesday |
| [ ] | 26 | `protest_march` | The vigil march | Ch 1.5 | Outdoor, night, candles, crowd |
| [ ] | 27 | `queer_affirming_church` | The queer-affirming church | Ch 1.5 | Easter evening |
| [ ] | 28 | `chicago_apartment` | Maya's apartment, Chicago | Ch 2 | The L outside the window; taped boxes |
| [ ] | 29 | `field_museum_hall` | The Field Museum: Stanley Field Hall | Ch 2 | **The real place (owner)**: model it from reference photos of the real hall, with Máximo the titanosaur |
| [ ] | 30 | `field_museum_office` | The Field Museum: the registrar's office | Ch 2 | Adeena's desk |
| [ ] | 31 | `field_museum_gallery` | The Field Museum: the new gallery | Ch 2 | Half-lit cases, a ladder |
| [ ] | 32 | `adeena_apartment` | Adeena's apartment | Ch 2 | The Shabbat table, the candlesticks |
| [ ] | 33 | `synagogue_conservative` | Congregation Etz Chaim (sanctuary) | Ch 2 | Conservative: the ark, the ner tamid, mixed seating |
| [ ] | 34 | `synagogue_social_hall` | Congregation Etz Chaim: the social hall | Ch 2 | A circle of chairs, coffee, rugelach |
| [ ] | 35 | `adeena_parents_house` | Ruth's house (the seder) | Ch 2 | A long table, the seder plate |
| [ ] | 36 | `havurah_living_room` | Hannah's living room | Ch 2 | Cushions, home-made prayer books, a cat |
| [ ] | 37 | `lgbtq_synagogue` | The LGBTQ synagogue | Ch 2 | Pride Shabbat: rainbow and trans flags by the ark |
| [ ] | 38 | `chabad_house` | The Chabad house | Ch 2 | A long table, the Rebbe's portrait by the door |
| [ ] | 39 | `renewal_shabbat` | The Renewal Shabbat | Ch 2 | A circle, drums, guitar |
| [ ] | 40 | `reform_temple` | The Reform temple | Ch 2 | A big sanctuary, bimah |
| [ ] | 41 | `sigd_community_hall` | The Sigd celebration | Ch 2 | Kessim under bright umbrellas, white shawls (check details with Beta Israel sources) |
| [ ] | 42 | `effie_house` | Effie's house | Ch 2 | Modern Orthodox family home, lamps on timers |
| [ ] | 43 | `lakefront_path` | The lakefront path | Ch 2 | Outdoor: Lake Michigan, grey, rocks |
| [ ] | 44 | `therapist_office` | Angie's office (and waiting room) | Ch 3 | Gerald the plant, a basket of grounding things, leaflets, the big chair |
| [ ] | 45 | `ohare_airport` | O'Hare Airport, at night | Ch 3 | A real place: the gate, the departures board, planes in the dark |
| [ ] | 46 | `boudha_guest_house` | Pasang's guest house, Boudha | Ch 3 | Fourth-floor room (tiger blanket, bucket shower), the kitchen with its altar shelf; rooftops and the stupa from the window |
| [ ] | 47 | `boudha_stupa` | Boudhanath Stupa | Ch 3 | A real place, outdoor: the white dome, the Buddha's eyes, prayer wheels, butter-lamp house, pigeons, prayer flags |
| [ ] | 48 | `dharma_shringa_hall` | Dharma Shringa: the meditation hall, Budhanilkantha | Ch 3 | A real place: cushions in rows, forest hills, monkeys on the wall |
| [ ] | 49 | `boudha_monastery` | The monastery behind the guest house | Ch 3 | Shrine hall (golden Buddha, water bowls, young monks) and the narrow lamp room full of butter lamps |
| [ ] | 50 | `lumbini_sacred_garden` | Lumbini: the Sacred Garden | Ch 3 | A real place, outdoor: the Maya Devi Temple, the Ashoka pillar, the pond and bodhi tree with prayer flags; the monasteries along the canal in the distance |

## Part 2: Maps (2D, one per city: `Assets/_Project/Resources/Maps/map_<city-id>.png`) *Auto*
**A map app on Maya's phone (owner).** It opens from Chapter 1 on to get from scene to scene (the Prologue and the Prelude are
door to door), and from the phone's Maps tab. **Portrait 3:4, e.g. 1536 × 2048**, sRGB, drawn like a map app (streets, main
roads, parks, water; no labels needed: the game adds the dot, pins, route and names). Pins are placed from places.json
(x, y from the bottom left), so tell Claude if a pin should move.
| | File | City | Chapters |
|---|---|---|---|
| [ ] | `map_champaign_urbana.png` | Champaign-Urbana | Ch 1, Ch 1.5 |
| [ ] | `map_indianapolis.png` | Indianapolis | Ch 1.5 |
| [ ] | `map_chicago.png` | Chicago (Lake Michigan to the east) | Ch 2 onwards |
| [ ] | `map_kathmandu.png` | Kathmandu (Boudha in the east, Budhanilkantha to the north) | Ch 3 |
| [ ] | `map_lumbini.png` | Lumbini (the Sacred Garden, the monastic zone) | Ch 3 |
| [ ] | `ui_map_dot.png`, `ui_map_pin.png`, `ui_map_frame.png` *Claude* | The blue "you are here" dot, the red destination pin, the phone frame around the map | 128 × 128, 128 × 192, 1024 × 2048 |

## Part 3: People (3D, Nomad → Procreate → **Grab3D**; `3D Models/<id>/model_<id>.glb`), in story order
### 3a. The system: one shared body, each alter's look
The body changes age through the game; each alter has their own hair, clothes and posture **on the same body**.
| | Id | What | Ages needed so far |
|---|---|---|---|
| [ ] First | `body_child_05`, `body_child_08`, `body_child_11`, `body_teen_15` | The body as a child and teen (the Prologue) | 5, 8, 11, 15 |
| [ ] | `body_adult` | The adult body (one model; small changes by age through hair and clothes) | 18-21 (college), 24-27 (Chicago) |
| [ ] First | `hair_maya`, `outfit_maya_<era>` | Maya: eras `child05`, `child08`, `child11`, `teen15`, `college`, `chicago` | all |
| [ ] | `hair_izzy`, `outfit_izzy` | Izzy (22, she/her): lost-time days, the tarot deck | Prelude on |
| [ ] | `hair_nina`, `outfit_nina` | Nina (11, she/her) | Prologue's end on |
| [ ] | `hair_axel`, `outfit_axel` | Axel (19, he/they): the haircut, thrifted men's clothes (Ch 1.5) | 20 |
| [ ] | `hair_jacob`, `outfit_jacob` | Jacob (14, he/him, Catholic) | Ch 1's end on |
| [ ] | `hair_serena`, `outfit_serena`, `hair_lily`, `outfit_lily`, `hair_heather`, `outfit_heather` | Serena (48), Lily (4), Heather (33): not unlocked yet | later chapters |
| [ ] | Theresa, Holden | Don't front: portraits only (inner world), no 3D needed yet | |

### 3b. Everyone else, in the order they first appear
| | Id | Who | Chapter | Notes |
|---|---|---|---|---|
| [ ] First | `mother` | Sarah, Maya's mother, minister of the Church of Friends | Prologue (and later) | Younger (2003) and older (2013) looks |
| [ ] First | `guide_quaker` | Mira Smith (68) | Prologue | The Prologue's guide |
| [ ] First | `eloise` | Miss Eloise, First Day School teacher | Prologue | |
| [ ] First | `rebecca` | Rebecca, Maya's friend | Prologue | A child, at 5 and 8 |
| [ ] First | `jamie`, `justin`, `will` | The children Rebecca prefers | Prologue | Children |
| [ ] First | `hines`, `jones` | Mrs. Hines and Mr. Jones, elders | Prologue | |
| [ ] First | `carter`, `thomas_carter`, `renee_carter` | Mr. Carter and his much older children; Reneé uses crutches | Prologue | Adult Reneé (manual wheelchair) comes in Ch 4 or 5 |
| [ ] | `susan` | Miss Susan, Maya's school teacher | Prologue | Remembered only: a portrait is enough |
| [ ] | `selene` | Selene (she/her), classmate | Prelude, Ch 1, Ch 1.5 | |
| [ ] | `tanya` | Tanya (42, they/them), coven elder | Prelude to the end | In touch all game; final meeting at the end |
| [ ] | `gracie` | Gracie (25), Catholic nun in training | Ch 1 | |
| [ ] | `ex_christian` | Mark, Episcopal, kind, queer | Ch 1, Ch 1.5 | |
| [ ] | `mark_mom` | Cassie, Mark's mother | Ch 1.5 | Indianapolis |
| [ ] | `mark_priest` | Pastor Key (31, he/him) | Ch 1.5 | St. Luke's |
| [ ] | `ex_jewish` | Adeena (27), the registrar | Ch 2 | Look: your character design |
| [ ] | `eudora` | Eudora Jones, curator | Ch 2 | |
| [ ] | `rabbi_abe` | Rabbi Abe (78, he/him) | Ch 2 | Sneakers, a tie with little fish on it |
| [ ] | `ruth`, `lee`, `effie`, `grandma` | Adeena's mother, uncle, cousin, grandmother | Ch 2 | Effie also has four children (background) |
| [ ] | `hannah` | Hannah, Adeena's friend | Ch 2 | |
| [ ] | `tigist` | Tigist (placeholder name), Beta Israel community | Ch 2 | |
| [ ] | `christie` | Christie, fellow tour guide at The Field Museum | Ch 2, Ch 3 (and Ch 6) | |
| [ ] | `angie` | Angie (35, she/her), Maya's therapist | Ch 3 on | Also seen on a laptop screen (video calls) |
| [ ] | `pasang` | Pasang Lhamo Sherpa (about 55, she/her), the temple-stay host | Ch 3 | Practical and motherly; gives Maya a white khata at goodbye |
| [ ] | `course_teacher` | The teacher at the silent course | Ch 3 | |
| [ ] | `ex_jewish_spouse` | Michael, Adeena's husband | Ch 6 | Mentioned only so far |

### 3c. Background people (idle / walk / talk only), by setting
| | Id | Who | Chapter |
|---|---|---|---|
| [ ] First | `bg_friends_1..4` | Church of Friends congregants (2000s Iowa) | Prologue |
| [ ] | `bg_student_1..6` | College students | Prelude, Ch 1, Ch 1.5 |
| [ ] | `bg_coven_1..4` | Circle members at The Circle and in the park | Prelude, Ch 1 |
| [ ] | `bg_church_1..4` | Congregants (Catholic, Episcopal, Methodist, queer-affirming) | Ch 1, Ch 1.5 |
| [ ] | `bg_crowd_1..6` | Pride and the vigil march | Ch 1.5 |
| [ ] | `bg_museum_1..4` | Museum visitors, fourth-graders in yellow shirts, other guides | Ch 2 |
| [ ] | `bg_shul_1..6` | Congregants across the streams (incl. a woman in a tallit) | Ch 2 |
| [ ] | `bg_kes`, `bg_sigd_1..4` | A kes in white with an umbrella; Sigd families in white shawls | Ch 2 |
| [ ] | `bg_airport_1..4` | Travellers at O'Hare at night | Ch 3 |
| [ ] | `bg_monk_1..4`, `bg_young_monk_1..3` | Monks in maroon (Tibetan Buddhist), young monks | Ch 3 |
| [ ] | `bg_kora_1..6` | People walking the kora: grandmothers with prayer beads, tourists, Pasang's daughter | Ch 3 |
| [ ] | `bg_meditator_1..6` | Students on the silent course | Ch 3 |
| [ ] | `bg_pilgrim_1..4` | Pilgrims at Lumbini, from many countries | Ch 3 |

## Part 4: Props (3D, no rig: `model_prop_<id>.obj` + `tex_prop_<id>_color.png`)
Every close-up in the chapters is listed scene by scene in [ART_BY_SCENE.md](ART_BY_SCENE.md) part 3 (e.g. `kitchen-table`,
`challah-cloth`). These are the props that come back across chapters, so make them first:
| | Id | What | First in |
|---|---|---|---|
| [ ] First | `mira_stone` | Mira's stone: a hag stone, grey and flat, with a hole worn right through by water (Maya carries it all game) | Prologue |
| [ ] First | `mira_napkin` | The napkin with Mira's phone number (torn in Ch 1.5) | Prologue |
| [ ] First | `bulletin`, `hymnal`, `coffee_cup`, `bible` | Church of Friends things | Prologue |
| [ ] First | `dear_mira_letters` | The unfinished "Dear Mira" letters | Prologue |
| [ ] | `family_computer` | The family computer (age 12 on) | Prologue (age 12) |
| [ ] | `phone`, `laptop` | Maya's phone and laptop (age 18 on) | Prelude |
| [ ] | `tarot_deck`, `tarot_card` | The tarot deck (box soft at the corners) and one card mesh (faces are 2D: Part 7) | Prelude |
| [ ] | `altar_cloth`, `candle`, `chalice`, `bowl`, `wand` | A Wiccan altar | Prelude |
| [ ] | `book_of_shadows` | Maya's Book of Shadows: black cloth, a crescent moon on the cover, a ribbon (Tanya's gift at Yule) | Prelude |
| [ ] | `seedling_cup` | The Ostara seedling in a paper cup | Ch 1 |
| [ ] | `flannel_shirt` | The green and black flannel shirt | Ch 1 |
| [ ] | `rosary`, `votive_candles` | Catholic chapel things | Ch 1 |
| [ ] | `shabbat_candlesticks`, `challah_cover` | Adeena's grandmother's brass candlesticks; the embroidered cloth | Ch 2 |
| [ ] | `siddur`, `kippah`, `tallit` | Prayer book, head covering, prayer shawl | Ch 2 |
| [ ] | `seder_plate` | Egg, bone, bitter herbs, charoset, parsley, salt water | Ch 2 |
| [ ] | `tall_candle` | Heather's single tall candle in a glass | Ch 2 |
| [ ] | `notebook` | Maya's notebook (Heather's notes in it, signed "H") | Ch 3 |
| [ ] | `prayer_wheel`, `butter_lamp`, `khata` | A brass prayer wheel; a butter lamp; the white silk scarf | Ch 3 |
| [ ] | `meditation_cushion`, `phone_envelope` | A cushion; the envelope her phone is handed in in | Ch 3 |
| [ ] | `suitcase`, `backpack` | Her dusty suitcase and the backpack packed in ten minutes | Ch 3 |
| [ ] | `orit`, `kes_umbrella` | The Orit wrapped in cloth; a bright ceremonial umbrella | Ch 2 |
| [ ] | `rune_pouch`, `rune_stave` | Runes | (practice) |
| [ ] | `herb_<id>` × 15, `herb_bundle_dried` | Herbs (ids in herbs.json; bay added) | (practice) |
| [ ] | `crystal_<id>` × 16 | Stones (ids in crystals.json; jet, amber and Mira's stone added: Mira's stone is the same model as `mira_stone` if there is one) | (practice) |
| [ ] | `charm_<id>` × 9 | Charms (ids in charms.json) | (practice) |
| [ ] | `charm_bag` | Maya's charm bag: a small plum cloth pouch tied with three knots (Selene's gift at Samhain 2019; she makes others) | Ch 1 |
| [ ] | `rune_cloth` | The cloth she casts runes on (dark, plain) | Ch 1 |
| [ ] | `cleansing_bundle`, `cleansing_bell`, `salt_bowl` | Cleansing: a bundle of rosemary and bay, a small brass bell, a bowl of salt water (never white sage) | Ch 2 |
| [ ] | `pendulum` | A clear quartz pendulum on a silver chain (Tanya's suggestion) | Ch 1 |
| [ ] | `teacup_leaves`, `dot` | A cup and saucer with tea leaves; Dot, about seventy, half-moon glasses, a teapot (The Circle) | Ch 1 |
| [ ] | `oracle_deck_selene` | Selene's handmade oracle deck: 12 hand-painted cards, one word each (Rest, Root, Begin, Enough, Ask, Return, Water, Kin, Voice, Shelter, Thaw, Here) | Ch 1 |
| [ ] | `scrying_bowl` | A dark bowl of water (or a single candle) | Ch 3+ |
| [ ] | `ancestor_altar`, `photo_grandpa_walter` | Photos and small things on the altar; Grandpa Walter squinting into the sun by a tractor | Prelude |
| [ ] | `offering_dish` | A small dish: bread, milk or flowers | Ch 1 |
| [ ] | `maypole`, `beltane_fire_bowl` | A birch maypole with ribbons in every colour; a small fire in a bowl | Ch 1 |
| [ ] | `lammas_loaf` | A round loaf, the first slice set aside | Ch 5 |

## Part 5: Dialogue portraits and faces (2D)
Face paintings go on the 3D models in the world; portraits sit beside the dialogue box (`Character Art - Finished/Talking Heads`). *Claude*
| | File | px |
|---|---|---|
| [ ] First | `portrait_<id>_neutral.png` for everyone who speaks in the Prologue: maya-05, maya-08, maya-11, maya-15, sarah, mira, eloise, rebecca, hines, jones | 1024 × 1024 |
| [ ] | `portrait_<id>_<expression>.png` (neutral, happy, sad, worried, blank, tense) for each alter and everyone in Part 3b, in the same order | 1024 × 1024 |
| [ ] | Inner voices: `portrait_<alter>_inside.png` (how an alter looks when speaking inside: softer, faded) | 1024 × 1024 |
| [ ] | Faces on the 3D models: `tex_<id>_face_<expression>.png` (3D_PIPELINE.md, Faces) | same as `tex_<id>_color.png` |

## Part 6: UI (`Assets/_Project/Art/UI`)
| | File | What | px | Wiring |
|---|---|---|---|---|
| [ ] First | `app_icon.png` (+ `app_icon_background.png`, `app_icon_foreground.png`) | The app icon (now a TEST icon) | 1024 × 1024 each | Auto |
| [ ] First | `ui_anchor.png` (+ `_pressed`) | The Anchor button, always top right | 384 × 384 | Claude |
| [ ] First | `ui_dialogue_box.png` (9-slice) | The dialogue box | 512 × 512 | Claude |
| [ ] First | `ui_choice.png` (+ `_pressed`, 9-slice) | Choice buttons (also the bottom "pocket" buttons: her phone, Mira's stone, Wait) | 640 × 200 | Claude |
| [ ] First | `ui_stick_ring.png`, `ui_stick_knob.png` | The round joystick (base ring and knob) | 512 × 512, 256 × 256 | Claude |
| [ ] First | `ui_use.png` (+ `_pressed`) | The Use button, bottom right | 384 × 384 | Claude |
| [ ] | `ui_icon_phone.png`, `ui_icon_laptop.png` | The phone and laptop icons on the right edge | 256 × 256 | Claude |
| [ ] | `ui_device_phone.png`, `ui_device_laptop.png` (9-slice) | The phone and laptop screens (messages and email) | 1024 × 1536 / 1536 × 1024 | Claude |
| [ ] | `ui_bubble_mine.png`, `ui_bubble_theirs.png` (9-slice) | Text-message bubbles | 512 × 256 | Claude |
| [ ] | `ui_icon_book.png`, `ui_book_page.png` (9-slice), `ui_book_cover.png` | The Book of Shadows: its icon on the right edge (under the laptop), a page, and the open cover around it | 256 × 256 / 1024 × 1536 / 1536 × 2048 | Claude |
| [ ] | `ui_icon_charm_bag.png` | The charm bag's icon on the left edge (only while she's drifting): press and hold | 256 × 256 | Claude |
| [ ] | `ui_moon_<phase>.png` × 8 | The moon badge, top left (new, waxing-crescent, first-quarter, waxing-gibbous, full, waning-gibbous, last-quarter, waning-crescent); placeholder drawn in code | 256 × 256 | Claude |
| [ ] | `ui_icon_systemlog.png`, `ui_systemlog_note.png` (9-slice) | SystemLog (from the end of Ch 4): its icon and a note card | 256 × 256 / 1024 × 256 | Claude |
| [ ] | `ui_tree_trunk.png`, `ui_tree_node.png`, `ui_tree_node_next.png`, `ui_tree_leaf.png`, `bg_chapter_tree.png` | The chapter tree (the hub): trunk, chapter node, the next chapter, an alter's leaf (tinted in their colour), background | 64 × 512 / 256 / 256 / 192 / 4440 × 2048 | Claude |
| [ ] | `ui_banner.png` (9-slice) | The "new message" banner | 1024 × 160 | Claude |
| [ ] | `ui_breath_guide.png` | The lantern/circle that grows and shrinks | 1024 × 1024 | Claude |
| [ ] | `ui_panel.png`, `ui_nameplate.png`, `ui_inner_voice_frame.png` (9-slice) | Panels, speaker name, inner-voice frame | 512 × 512 / 512 × 256 | Claude |
| [ ] | `ui_btn_wide.png` (+ `_pressed`, 9-slice) | Menu buttons (title menu, Chapters, Saves) | 640 × 200 | Claude |
| [ ] | `ui_icon_journal.png`, `ui_icon_codex.png`, `ui_icon_triggers.png`, `ui_icon_settings.png`, `ui_icon_practice.png` | Small icons | 128 × 128 | Claude |
| [ ] | `ui_front_badge_<alter>.png` | Who's fronting (top left) | 256 × 256 | Claude |
| [ ] | `ui_logo.png` | Title logo "I Am As I Am" | 2048 wide | Claude |
| [ ] | `bg_title.png` | The title-menu background | 4440 × 2048 | Claude |

## Part 7: 2D cards and codex art
Tarot cards go in `Assets/_Project/Art/Tarot/` (*Auto*: also the loading screen, a random card each time; run Rebuild Boot Scene).
| | File | px |
|---|---|---|
| [ ] First | `tarot_back.png` | 1024 × 1768 |
| [ ] | `tarot_<card-id>.png` × 22 Major Arcana (ids in tarot_major.json, e.g. `tarot_major-17-star.png`) | 1024 × 1768 |
| [ ] | `rune_<id>.png` × 24 (ids in runes.json) | 512 × 768 |
| [ ] | `codex_<id>.png` for herbs, crystals and charms | 1024 × 1024 |
| [ ] | `codex_tradition_<id>.png` per tradition (Quaker, Wicca, Christianity, Judaism, then the rest) | 2048 × 1024 |

## Part 8: Effects (made in the game; reference art welcome in `Guidelines`)
| | File | What |
|---|---|---|
| [ ] | `fx_mist.png` (seamless, white on transparent) | The dissociation fog's drifting mist (now generated noise) *Claude*, 1024 × 1024 |
| [ ] | `ref_fx_<name>.png` | References: fog, flattened derealization light, depersonalization drift, co-consciousness edge tint, the glow on things to explore (a slow warm breathing) |
No flashing, ever.

## Part 9: Screen mock-ups (landscape, 2732 × 2048; subfolder `Mockups`), optional
Drawings of whole screens for Claude to build from: `mockup_title.png` (the main menu), `mockup_explore.png` (walking in a place:
objective line, prompt, pocket buttons, phone and laptop icons, joystick, Use), `mockup_dialogue.png`, `mockup_phone.png`,
`mockup_laptop.png`, `mockup_map.png`, `mockup_chapters.png`, `mockup_anchor.png`, `mockup_loading.png`, `mockup_recap.png`,
`mockup_trigger.png`, `mockup_journal.png`, `mockup_tarot.png`, `mockup_codex_tradition.png`, `mockup_comfort.png`,
`mockup_content_note.png`.
