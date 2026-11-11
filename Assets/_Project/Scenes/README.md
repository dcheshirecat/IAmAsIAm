# Scenes (planned, M1 vertical slice)

| Scene | Purpose |
|---|---|
| `Boot` | `GameBootstrap`, `AnchorMenuController`, EventSystem, persistent audio. Loads Title. |
| `Title` | Not-therapy notice (first launch), Comfort settings, Practice Mode, Continue. |
| `Apartment_Samhain` | Outer world: Wren's flat. Time-loss gap with clues (receipt, rosemary, note). |
| `Hearthwood_Hearth` | Inner world hub: kitchen, apothecary, Inner Journal, Meeting Table. |
| `PracticeMode` | Neutral calm space for real-life skill practice outside the story. |

Each gameplay scene needs: a global Volume per dissociative experience, wired to
`DissociationPresentationDriver`; a `MindStateController`; and a player rig with
`FrontingController`, `ShoulderCamera (depersonalization drift)` and `PlayerInteractor`.
