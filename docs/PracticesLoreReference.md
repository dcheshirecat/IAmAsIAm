# Practices & Lore Reference

The playable data lives in `Assets/_Project/Resources/Data/` (`runes.json`,
`tarot_major.json`, `herbs.json`, `crystals.json`, `charms.json`). This doc explains how
the data is meant to be used and where it comes from.

## How practices function in the game

Practices **never** change stats or outcomes. Each one serves up to three purposes:

| Purpose | Example |
|---|---|
| **Sensory anchor** (connects to grounding skills) | Holding hematite: "Cool, heavier than it looks, smooth as river water." This counts as a *touch* item for 5-4-3-2-1. |
| **Ritual of attention** (mindfulness) | Tying nine knots into a witch's ladder, one breath per knot |
| **Reflection prompt** (journaling) | Drawing Ehwaz: "Who do you move through life alongside? What makes that partnership work?" |

## Runes: Elder Futhark (24 runes, 3 ætts)

- The names are **reconstructed Proto-Germanic**. No complete list of the original names survives. Attested meanings come from the Old English, Old Norwegian and Old Icelandic **Rune Poems** (8th–15th c.), which describe the later Anglo-Saxon and Younger Futhark row of letters.
- Runes were primarily a **writing system**. Evidence for historical divination with runes specifically is thin. Tacitus's *Germania* (c. 98 CE) describes casting lots, but does not say runes were carved on them. Most "rune meanings" used today come from the 20th century, especially Ralph Blum's *The Book of Runes* (1982).
- The Codex shows three things for each rune: the **literal meaning**, any **Rune Poem** association, and the **modern association**, each labeled.
- See the hate-symbol note in `ContentAndSafetyGuidelines.md` §4.

## Tarot: Rider-Waite-Smith

- Published in 1909 (artist Pamela Colman Smith, designer A. E. Waite). The original 1909 artwork is public domain in the US, but **later recolourings are copyrighted**. Plan: commission original art inspired by the structure.
- The game uses tarot **for reflection only**. Each card has a gentle reflection prompt instead of a prediction.
- Heavier cards (Death, The Tower, The Devil, Ten of Swords) carry a `gentleNote` reframing them. Death means endings and transformation, never literal death.
- **Reversals are off by default** (Comfort setting).
- Current data covers the 22 Major Arcana. The 56 Minor Arcana are a **[DECIDE]** in the GDD.

## Herbs

The selection is common European and Mediterranean garden and kitchen herbs. White sage
is excluded (overharvested, and central to closed Indigenous practice). Garden sage
(*Salvia officinalis*) is included instead.

Each entry includes:
- Folklore, using only documented folk associations
- A sensory description (how it is used as a grounding anchor)
- **Safety notes**, which are mandatory, with no dosages and no ingestion instructions

## Crystals & stones

- Each entry includes the **mineral identity** and **Mohs hardness**, because these are facts.
- "Modern associations" are labeled as modern belief, with no claimed effects.
- Care and safety notes cover facts such as: selenite is gypsum and dissolves in water, malachite dust is toxic so don't make "elixirs", and some quartz is heat-treated (e.g. most "citrine").
- A sourcing note: mining ethics vary a lot, so second-hand or known-origin stones are better.

## Charms & amulets (open European folk practice)

Hag stones, rowan crosses with red thread, witch bottles (documented in early modern
England), Brigid's crosses (Irish, woven at Imbolc), witch's ladders, personal sigils
(modern), and bindrunes (modern practice, combining runes).

## Sources

- Page, R. I. (1987). *Runes* (Reading the Past). British Museum Press.
- Dickins, B. (1915). *Runic and Heroic Poems of the Old Teutonic Peoples* (the Rune Poems).
- Waite, A. E. (1910). *The Pictorial Key to the Tarot*.
- Hutton, R. (1996). *The Stations of the Sun: A History of the Ritual Year in Britain*.
- Hutton, R. (1999). *The Triumph of the Moon* (history of modern paganism).
- Merrifield, R. (1987). *The Archaeology of Ritual and Magic* (witch bottles).
- NCCIH (US National Center for Complementary and Integrative Health), herb fact sheets, for safety notes.
