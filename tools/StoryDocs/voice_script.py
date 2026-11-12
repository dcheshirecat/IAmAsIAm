#!/usr/bin/env python3
"""Voice-over logistics: gives every spoken line a permanent voice id, and writes the script list.

    python3 tools/StoryDocs/voice_script.py --chapter prologue-quaker --assign --csv out.csv FILE.ink [FILE.ink ...]

- A spoken line is "Name: text" or "Name (inside): text". Narration and choice labels aren't voiced.
- --assign adds "# vo: <chapter>_<speaker>_<0000>" to every spoken line that has none (in the .ink files).
  Numbers are per chapter and permanent: a line keeps its number when its text changes, and a number is never reused,
  so a recording always matches its line. (The file to record is vo_<that>.mp3.)
- Speakers become lowercase ids ("Mrs. Hines" -> hines, "Mira" -> mira, "Nina (inside)" -> nina-inside).
  A file can override one with a header comment, e.g.  // vo-speakers: Maya=maya-05
- --csv writes the script list: one row per line, in story order, for a Google Sheet.
- Fails if two lines share an id.
"""
import argparse
import csv
import re
import sys

SPEAKER = re.compile(r"^(?P<indent>\s*)(?P<name>[A-Z][\w.' ]*?)(?P<inside> \(inside\))?: (?P<text>.+?)\s*(?P<tags>#.*)?$")
VO = re.compile(r"#\s*vo:\s*([a-z0-9-]+)_([a-z0-9-]+)_(\d{4})")
TITLES = re.compile(r"^(mr|mrs|ms|miss|dr|pastor)\.?\s+", re.I)


def slug(name):
    return re.sub(r"[^a-z0-9]+", "-", TITLES.sub("", name.strip()).lower()).strip("-")


def overrides(lines):
    out = {}
    for l in lines:
        m = re.match(r"^\s*//\s*vo-speakers:\s*(.*)$", l)
        if m:
            for pair in m.group(1).split(","):
                if "=" in pair:
                    k, v = pair.split("=", 1)
                    out[k.strip()] = v.strip()
    return out


def known_speakers():
    """Names that can speak: the system members and the people in the game's data (full and first names)."""
    import json, os
    data = os.path.join(os.path.dirname(__file__), "..", "..", "Assets", "_Project", "Resources", "Data")
    names = set()
    for m in json.load(open(os.path.join(data, "system_members.json"), encoding="utf-8")).get("members", []):
        names.add(m.get("displayName", ""))
    for p in json.load(open(os.path.join(data, "people.json"), encoding="utf-8")).get("people", []):
        n = p.get("name", "")
        names.add(n)
        if not TITLES.match(n):
            names.add(n.split(" ")[0])
    return {n for n in names if n}


KNOWN = known_speakers()


def scan(path):
    """Yields (line_index, speaker_id, display_name, text, existing_id_or_None, stitch, in_choice_label)."""
    lines = open(path, encoding="utf-8").read().split("\n")
    over = overrides(lines)
    stitch, option = "", None
    for i, raw in enumerate(lines):
        s = raw.strip()
        if s.startswith("//") or s.startswith("~") or s.startswith("->") or not s:
            continue
        m = re.match(r"^= (\w+)", s)
        if m:
            stitch, option = m.group(1), None
            continue
        if s.startswith("==="):
            continue
        o = re.match(r"^\*\s*(\(\w+\)\s*)?\[(.*)\]", s)
        if o:
            option = o.group(2)
            continue
        if s == "-":
            option = None
            continue
        if not raw.startswith("    "):
            option = None
        m = SPEAKER.match(raw)
        if not m or m.group("name") not in KNOWN:
            continue
        name = m.group("name")
        inside = bool(m.group("inside"))
        sid = over.get(name, slug(name)) + ("-inside" if inside else "")
        vo = VO.search(m.group("tags") or "")
        existing = f"{vo.group(1)}_{vo.group(2)}_{vo.group(3)}" if vo else None
        yield i, sid, name + (" (inside)" if inside else ""), m.group("text"), existing, stitch, option, lines


def where(path, stitch):
    """'draft_prologue_sunday2.ink' + 'fellowship' -> 'Sunday 2 / fellowship'."""
    stem = path.split("/")[-1].replace(".ink", "").replace("draft_", "")
    m = re.search(r"(sunday|chapter|part|scene)(\d+)$", stem)
    place = f"{m.group(1).capitalize()} {m.group(2)}" if m else stem
    return f"{place} / {stitch.replace('_', ' ')}"


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--chapter", required=True)
    ap.add_argument("--assign", action="store_true")
    ap.add_argument("--csv")
    ap.add_argument("--status", default="")
    ap.add_argument("files", nargs="+")
    a = ap.parse_args()

    # The highest number already used in this chapter, across all its files.
    used, highest = {}, 0
    for f in a.files:
        for i, sid, name, text, existing, *_ in scan(f):
            if existing:
                if existing in used:
                    sys.exit(f"Duplicate voice id {existing}: {used[existing]} and {f}:{i + 1}")
                used[existing] = f"{f}:{i + 1}"
                if existing.startswith(a.chapter + "_"):
                    highest = max(highest, int(existing[-4:]))

    rows = []
    for f in a.files:
        lines = open(f, encoding="utf-8").read().split("\n")
        changed = False
        for i, sid, name, text, existing, stitch, option, _ in list(scan(f)):
            vid = existing
            if not vid and a.assign:
                highest += 1
                vid = f"{a.chapter}_{sid}_{highest:04d}"
                lines[i] = lines[i].rstrip() + f" # vo: {vid}"
                changed = True
            if not vid:
                continue
            notes = []
            if name.endswith("(inside)"):
                notes.append("Inner voice: record dry and close, like a spoken line.")
            if option:
                notes.append(f"Only if the player picks \"{option.rstrip('.')}\".")
            rows.append({
                "Recorded": "",
                "File name": f"vo_{vid}.mp3",
                "Who": name,
                "Line": re.sub(r"\s*#.*$", "", text).strip(),
                "Where": where(f, stitch),
                "Voice": sid,
                "Notes": " ".join(notes),
                "Status": a.status,
            })
        if changed:
            open(f, "w", encoding="utf-8").write("\n".join(lines))
    if a.csv:
        with open(a.csv, "w", newline="", encoding="utf-8") as out:
            w = csv.DictWriter(out, fieldnames=["Recorded", "File name", "Who", "Voice", "Line", "Where", "Notes", "Status"])
            w.writeheader()
            w.writerows(rows)
    print(f"{len(rows)} voiced lines; highest number in {a.chapter}: {highest:04d}")


if __name__ == "__main__":
    main()
