# Lived experience: where practitioners' voices come from

Each chapter's guide is a practitioner of that tradition, and the codex has a "practitioner voices" section.
What they say must come from **real people**, never invented. The owner's rule (2026-11-11): public
testimony from **blogs and videos** may be used **if it comes to a consensus**, or **if distinct streams
within one practice can be identified**. Consented interviews are also welcome.

The data model is `LivedExperience` in `Scripts/Core/Traditions/TraditionData.cs`, and `DataValidator` enforces it.

| `sourceType` | When | Rules (validated) |
|---|---|---|
| `public-consensus` | Several independent practitioners (blogs, videos, talks) say substantially the same thing | ≥ 2 sources (aim for 3+); paraphrased in our own words |
| `public-stream` | Practitioners differ, but along a recognisable line within the practice (e.g. Orthodox vs Reform Judaism; eclectic vs British Traditional Wicca; Theravada vs Zen) | `streamName` set; ≥ 2 sources *within that stream* |
| `interview` | A consented interview | `consentRef` set (the signed form is kept outside the repo); credit as the person wishes |

## How to research an entry
1. Pick a topic (e.g. "What Shabbat rest feels like", "Meditation and dissociation").
2. Find practitioners speaking **in their own voice** about their own practice: personal blogs, YouTube or
   TikTok explainers by members, community talks, podcasts. Prefer people who say which community or stream they're from.
3. Note who agrees, and where they split. If they split along a stream, make one entry per stream.
4. **Paraphrase.** No long quotes, no names unless the person is a public educator quoting themselves publicly.
   The aim is the shared experience, not one person's words.
5. Record each source (title, URL, kind, date accessed) in `sources`.
6. Leave `status: "awaiting"` until reviewed. **A practitioner reader from that tradition** checks it before it
   becomes `approved`. Only approved entries ship with text.

## Never
- Invent a testimony, a quote, or a person.
- Put words from someone outside the tradition into a practitioner's mouth.
- Use testimony from closed traditions to stage their practices (the closed-tradition notes stay "learn about" only).
- Use a single viral voice as "what X believe".
