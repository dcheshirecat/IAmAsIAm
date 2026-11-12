# Maya's Wiccan practice: design (PROPOSAL for the owner)

The owner's answers (2026-11-12) are marked **(owner)**. Everything else is Claude's proposal, for the owner to change.

## What the owner decided
- **Where (owner):** both an **altar at home** (in every place Maya lives) AND a **Book of Shadows** she can open any time,
  like the phone. Practice moments are also written into the chapters (sabbats with Tanya, a reading before a hard day).
- **What practice does (owner):** all four:
  - **grounding:** it can steady her when she's drifting;
  - **reflection:** a prompt, and her answer goes in the Book of Shadows;
  - **it can change the story:** a reading or a spell opens dialogue or choices later;
  - **alters respond.**
  Still **no stat boosts** (owner, 2026-11-11).
- **Practices (owner):**
  - tarot and runes;
  - candle magick;
  - amulets and charm bags;
  - sabbats and the moon;
  - more divination (pendulum, scrying, tea leaves, oracle cards);
  - circle work (grounding and centering, casting a circle, calling the quarters, cleansing, guided meditation);
  - ancestors and devotion (an ancestor altar, offerings, sigils, a dream journal).

  Closed practices are left out: no white sage smudging and no hoodoo. Cleansing uses rosemary, bay, incense, salt or a bell.
- **How hands-on (owner):** small hands-on steps that **feel like rituals, not games**: slow, quiet, never timed, no score, no
  fail state, no "correct" answer.
- **Who practises (owner):** only **Maya** and **Izzy** (Wiccan; loves tarot). **Heather's candles belong to her own,
  hidden faith (owner)**: she leaves the Wiccan tools alone. Everyone else keeps their own religion and **ignores the Wiccan
  tools or actively despises them**. For example, **Jacob** is a devout Catholic who prays to saints. **Each alter with a faith
  gets a small practice of their own at home (owner)**, done the same calm way.
- **The Book of Shadows (owner):** **Tanya gives it to her, at Yule, December 2017.**
- **Can it be hard? (owner):** yes, sometimes. It's mostly comforting, but now and then a practice stirs something up, and lost
  time can leave a practice behind. Content warnings as usual.

## Proposal

### 1. The Book of Shadows
- **When (owner):** Tanya gives it after the Yule circle (December 2017, the end of the Prelude), just before Izzy comes forward. It's a
  plain cloth-bound book, with one line in Tanya's handwriting on the first page ("Ground first."). Before that, Maya has only the
  tarot deck (Izzy's, bought during lost time in Prelude Part 2) and a windowsill. **BUILT (step 1)**: `Core/Practices/BookOfShadows.cs`
  (data `Resources/Data/craft.json`, save v4 `book`), `Runtime/Practices/BookScreen.cs`, the Ink functions in section 8. In the
  story: Selene's Samhain teaches the ancestor altar; Tanya's grounding at Yule; the gift after the circle; Izzy writes the first
  entry (Yule) during the lost night; Chapter 1's first Thursday (circle casting, tarot, the first reading) and Ostara.
- **Where it lives:** a side icon under the phone, always there once she has it, during exploring and dialogue. It's never shown
  over the Anchor or an interactive moment. It stays hidden in chapters set before she has it (the Prologue and the start of the
  Prelude).
- **What's in it** (tabs, like the phone):
  - **Practices:** the steps of each one she has learned, in her own words, added the first time she does it. "Learned from
    Tanya" or "Learned from Selene" comes with it.
  - **Readings:** every reading and casting, with the date, the cards or runes, and what she wrote about them.
  - **Wheel of the Year:** the eight sabbats and the moons, marked as she keeps them, with what she did.
  - **Dreams:** the dream journal.
  - **Correspondences:** herbs, crystals, colours and runes from `herbs.json`, `crystals.json`, `runes.json` and
    `charms.json`, filled in as she learns them.
- **Lost time:** after a blackout there can be an entry in someone else's handwriting: Izzy's loopy letters (Heather never writes in
  it). Like the lost-time texts, it's **never pointed out**, but the handwriting is different. Later, once SystemLog exists, the
  alters can sign their entries.
- **The others:** Jacob and Axel never open it. If the player opens it while one of them fronts, it stays shut with a line in
  their voice. Jacob: "Not touching that." Axel: "Maya's thing. Pass."

### 2. The altar at home
- **Where:** every home has one: the dorm windowsill (Prelude), the Chicago apartment, the Seattle apartment, and later homes
  too. It grows over the years: the first candle, Selene's deck, Mira's stone, the prayer wheel and the khata from Nepal, a diya
  from Diwali. The things on it come from the story (owner: "every asset listed in the docs" adds to the art list).
- **When:** the player can use it whenever the story is at home and isn't in the middle of something: walk up to it, then Use.
- **What:** using it opens **the practice wheel**, a small calm circle of what she knows. Picking one starts the ritual in place.

### 3. How a ritual plays (feels like a ritual, not a game)
Every ritual follows the same shape, slow and quiet:
1. **Arrive:** the camera comes close, the sound softens, and one breath is shown (the paced-breathing circle).
2. **Prepare:** two or three small hands-on steps: draw a card from the deck, tap to light a wick, drop herbs into a bag one by
   one, turn the pendulum's chain.
3. **The moment:** the card turns, the flame steadies, a rune lands.
4. **Reflect:** a prompt, and the player picks or writes a short answer. It goes into the Book of Shadows.
5. **Close:** "thank and release" (snuff the candle, close the circle). The camera pulls back.

No timer, no score, no wrong answer. Reduce motion and the comfort settings apply. A ritual can be left halfway with "Not
today" (no penalty, no guilt).

### 4. The practices, one by one (small steps)
| Practice | Steps | What it gives |
|---|---|---|
| **Tarot** | shuffle (hold), cut, draw 1, 3 or 10 (Celtic cross) | Reflection prompts per card (`tarot_major.json`, minors to add); a story flag per card (e.g. `drew_tower`). |
| **Runes** | shake the bag, cast, read the ones face up | The same, from `runes.json`. |
| **Candle magick** | choose a colour, dress with oil, carve a word (type it), light, watch | An intention saved; the candle stays lit on the altar (scenery) until the next scene. |
| **Charm bag / amulet** | choose 3 herbs or stones (correspondences shown), add a written intention, tie | Carried: it appears in her pocket bar for the rest of the chapter; some scenes notice it. |
| **Grounding and centering** | the tree meditation: roots down, branches up (hold, then release) | **Grounding:** eases dissociation, like the DBT skills. |
| **Casting a circle** | walk the circle (or tap the four quarters), the elements, a bell | Needed before some sabbat rituals; grounding. |
| **Cleansing** | rosemary or bay, incense, salt, or a bell, room by room | A calmer room (the fog lifts a little at home). |
| **Pendulum** | hold still, ask a yes/no question | A gentle yes/no/maybe answer (reflection, never the "real" future). |
| **Scrying** | a bowl of water or a candle flame, soft focus | **Can be hard:** it can drift into derealization. Eyes-open grounding is offered. |
| **Tea leaves** | drink (tap), swirl, turn the cup, look | Reflection: a shape and a prompt. |
| **Oracle cards** | draw one | A kinder, simpler deck: one word and a prompt. |
| **Ancestor altar** | set out a photo or object, light a candle, speak | Samhain: Mira's stone and napkin; never Sarah (owner: never show her side). |
| **Offerings** | bread, milk, flowers, a song | Devotion to the Goddess and God; sabbat moments. |
| **Sigils** | write an intention, cross out letters, draw the shape | It can be drawn on the candle or put in the charm bag. |
| **Dream journal** | on waking, write a line | Story dreams can be written into it (and alters' dreams). |

### 5. The Wheel of the Year and the moon
- **Sabbats:** the eight sabbats are dates on Maya's calendar through her life. When a chapter's time passes one, the story can
  hold a sabbat moment: a public circle with Tanya, or a small one at the altar. Many are already written (Samhain 2016, Yule
  2017, Ostara 2018, Samhain 2019). Later chapters can mark the ones that fall in them, even quietly, alone.
- **Moons:** a full or new moon can be "tonight" when she's home, and the altar offers an esbat (moon water, a reading).
- **Tanya:** Tanya's texts and emails follow the Wheel ("Happy Imbolc! What are you lighting?").

### 6. Who practises (owner), and how the alters respond
| Alter | Faith | With the Wiccan tools |
|---|---|---|
| **Maya** | Wiccan, ex-Quaker | Practises: everything, growing more confident through the game. |
| **Izzy** | Wiccan | Practises: loves tarot above all, chatty readings, and her own way of shuffling. Leaves readings laid out after lost time. |
| **Heather** | hidden until later (owner) | Her candles belong to her own faith; she leaves the Wiccan tools alone. Her own practice at home: a single candle, lit in silence (its meaning stays hidden until the owner reveals her faith). |
| **Jacob** | Catholic | Despises it. Won't touch the deck; may put it in a drawer, face down. His own practice: the rosary, a saint's holy card (St. Michael), a short prayer. |
| **Nina** | Quaker | Ignores the tools. Her own practice: sitting in silence, as in meeting for worship (waiting in the Light). |
| **Lily** | Quaker | Ignores them. Her own practice: a short, small silence, and a song ("This Little Light of Mine"?), to confirm. |
| **Axel** | ex-Quaker, angry at religion | Ignores them ("Maya's thing. Pass."). |
| **Serena** | no religion | Ignores them. |

The altar's wheel only offers Wiccan practices to Maya and Izzy. **Owner: the others with a faith get their own small practice at
home** (Jacob's rosary, Nina's and Lily's silence, Heather's candle), offered in place of the wheel; Axel and Serena (no religion)
just get a line in character (Jacob also turns the deck face down).

### 7. When a practice is hard (owner: sometimes)
- **A reading too close:** some cards (the Tower, Death, the Five of Cups...) can start a gentle depersonalization the first
  time they come up after a hard scene, with grounding offered.
- **A candle that reminds her:** a candle on January 18 (Mira's death) brings Heather close.
- **Scrying** can drift into derealization.
- **Lost time:** she finds a reading laid out that she didn't do (Izzy), a candle burned down (Heather), or the deck in a drawer,
  face down (Jacob).
- The `practice-distress` content warning is shown before any chapter where a scripted practice is hard.

### 8. In the story (Ink)
These are proposals for the writing tools. Claude adds them when building.
- `# ritual: <practice>[: spread or detail]` already exists for tarot. It's extended to every practice (`# ritual: candle`,
  `# ritual: charm-bag`, `# ritual: runes`, `# ritual: circle`...). The story waits until the ritual is closed.
- `practised("<practice>")`: has she ever done this?
- `last_reading()` / `drew("<card-id>")`: lets later dialogue respond to a reading.
- `bos_entry(member, tab, text)`: the story writes in the Book of Shadows, in a member's hand.
- `gain_book_of_shadows()`: Tanya's gift.
- `learn_practice("<practice>", "<teacher>")`: adds the steps to the Book of Shadows.
- `carrying("<charm id>")`: is she carrying a charm bag she made?

### 9. Building it (in this order, each step pushed so the owner can test)
1. The Book of Shadows (Core data, save, the side icon and tabs), Tanya's gift at Yule, and `learn_practice`.
2. The ritual frame (the five steps above), with tarot, candle and grounding and centering first; the altar in each home.
   **BUILT:** `Core/Practices/RitualSession.cs` (arrive, prepare, moment, reflect, close; steps Breathe / Hold / Tap / Choose /
   Write; "Not today" any time; tests `RitualSessionTests`), `Runtime/Practices/RitualScreen.cs` (a dim veil and a cloth; a hold
   that's let go early only pauses; Reduce motion keeps the breath circle still), `Runtime/World/AltarSpot.cs` (places.json
   `altar: true`: her dorm, the college apartment, Chicago, Seattle; it doesn't glow; usable while exploring). The altar's wheel
   offers grounding plus whatever she's learned; anyone but Maya or Izzy gets their line instead. Ink: `# ritual: tarot` (one card),
   `# ritual: candle`, `# ritual: grounding-centering`. Results: a Book entry (tarot in Readings), a little grounding, story
   variables `ritual_card` / `ritual_colour` / `ritual_word` / `ritual_here`. A hard card (Death, the Tower, the Devil, the Moon:
   craft.json `hardCardIds`) can sometimes start a gentle depersonalization, eased when the ritual closes. Candle colours:
   craft.json `candleColours`. Tanya teaches candles (and says a windowsill altar is plenty) at Chapter 1's first Thursday.
3. Runes, the charm bag, cleansing, casting a circle.
4. The other divination practices, the ancestor altar, offerings, sigils, the dream journal.
5. The Wheel of the Year and the moons in the calendar; Tanya's sabbat texts.
6. Alter responses and lost-time traces; hard moments.
7. Going back through the written chapters to add practice moments (with the owner's OK, chapter by chapter).

Art: every new object (the altar per home, the Book of Shadows, candles, the charm bag, runes, the pendulum, the scrying bowl,
the teacup, an oracle deck) goes in `docs/ART_LIST.md`.
