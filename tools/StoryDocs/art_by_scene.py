#!/usr/bin/env python3
"""Lists every place, person and close-up the chapters need, in story order (owner: models in chronological order).

    python3 tools/StoryDocs/art_by_scene.py > docs/ART_BY_SCENE.md

Reads Story/Chapters/*.ink in chapter order (chapters.json -> chapters.ink root knot -> its parts), places.json for place
names, people.json and system_members.json for who's who. Re-run whenever a chapter changes.
"""
import json
import os
import re

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
STORY = os.path.join(ROOT, "Assets", "_Project", "Story", "Chapters")
DATA = os.path.join(ROOT, "Assets", "_Project", "Resources", "Data")

SPEAKER = re.compile(r"^\s*(?P<name>[A-Z][\w.' ]*?)(?P<inside> \(inside\)| \(on video\))?: (?P<text>.+?)\s*(#.*)?$")
LOOK = re.compile(r"#\s*look:\s*([a-z0-9-]+)")
MEMORY = re.compile(r"#\s*memory:\s*([a-z0-9-]+)")
PLACE = re.compile(r"#\s*place:\s*([a-z0-9_]+)")
AGE = re.compile(r"set_age\((\d+)\)")
TALK = re.compile(r"\[Talk: ([^\](]+?)\s*(?:\(|\])")


def load(name):
    return json.load(open(os.path.join(DATA, name), encoding="utf-8"))


def chapter_files():
    chapters = load("chapters.json")["chapters"]
    roots = {}
    text = open(os.path.join(STORY, "chapters.ink"), encoding="utf-8").read()
    for m in re.finditer(r"===\s*(\w+)\s*===\s*\n->\s*(\w+)", text):
        roots[m.group(1)] = m.group(2)
    for ch in chapters:
        first = roots.get(ch.get("inkKnot", ""))
        if not first:
            continue
        files, knot = [], first
        while knot and os.path.exists(os.path.join(STORY, knot + ".ink")):
            files.append(knot)
            body = open(os.path.join(STORY, knot + ".ink"), encoding="utf-8").read()
            nxt = re.findall(r"^->\s*(\w+)\s*//\s*the next part", body, re.M)
            knot = nxt[-1] if nxt else None
        yield ch, files


def clean(text):
    text = re.sub(r"\s*#.*$", "", text).strip()
    text = re.sub(r"^[*+\-]\s*(\{[^}]*\}\s*)?(\[[^\]]*\])?\s*", "", text).strip()
    return text


def main():
    places = {p["id"]: p for p in load("places.json")["places"]}
    people = {p["name"]: p for p in load("people.json")["people"]}
    members = {m["displayName"]: m for m in load("system_members.json")["members"]}
    for p in list(people.values()):  # "Mira" speaks; people.json has "Mira Smith".
        people.setdefault(p["name"].split(" ")[0], p)
        people.setdefault(" ".join(p["name"].split(" ")[:2]), p)

    place_order, place_info = [], {}
    person_order, person_info = [], {}
    scenes = []

    for ch, files in chapter_files():
        for f in files:
            age, place = None, None
            for raw in open(os.path.join(STORY, f + ".ink"), encoding="utf-8"):
                line = raw.rstrip("\n")
                if line.strip().startswith("//"):
                    continue
                m = AGE.search(line)
                if m:
                    age = int(m.group(1))
                m = PLACE.search(line)
                if m:
                    place = m.group(1)
                    if place != "black" and place not in place_info:
                        place_order.append(place)
                        place_info[place] = {"first": (ch["title"], f), "people": [], "looks": [], "memories": []}
                if place is None or place == "black":
                    continue
                info = place_info[place]

                who = []
                m = SPEAKER.match(line)
                if m and not line.strip().startswith(("*", "+", "~", "-", "=")):
                    who.append((m.group("name").strip(), bool(m.group("inside"))))
                for t in TALK.findall(line):
                    if not t.startswith("Say "):
                        who.append((t.strip(), False))
                for name, inside in who:
                    if name in ("Mom", "Mommy"):
                        name = "Sarah"
                    # Only real speakers (narration can contain a colon: "Sermon notes: ...").
                    if inside or (name not in people and name not in members):
                        continue
                    key = name
                    if key in members:
                        key = f"{name} (the body, {age})" if age is not None else name
                    if key not in info["people"]:
                        info["people"].append(key)
                    if name not in person_info:
                        person_order.append(name)
                        person_info[name] = {"first": (ch["title"], f), "ages": set(), "places": []}
                    if age is not None and name in members:
                        person_info[name]["ages"].add(age)
                    if place not in person_info[name]["places"]:
                        person_info[name]["places"].append(place)

                m = LOOK.search(line)
                if m:
                    info["looks"].append((m.group(1), clean(line)))
                m = MEMORY.search(line)
                if m:
                    info["memories"].append(m.group(1))

    out = []
    out.append("# Art by scene (generated)\n")
    out.append("_Made by `tools/StoryDocs/art_by_scene.py` from the chapters in the game. Don't edit by hand: re-run it._\n")
    out.append("Everything the chapters show, **in story order** (owner: models in chronological order). Each place lists who is "
               "there and every close-up (`# look:`), which are the props and details to model or paint. Hidden memories "
               "(`# memory:`) are scenes seen only by an alter.\n")
    out.append("## 1. Places, in the order they first appear\n")
    out.append("| # | Place id | Place | City | First in | People there |")
    out.append("|---|---|---|---|---|---|")
    for i, pid in enumerate(place_order, 1):
        p = places.get(pid, {})
        info = place_info[pid]
        out.append(f"| {i} | `{pid}` | {p.get('name', pid)} | {p.get('city', '') or '-'} | {info['first'][0].replace(' (title to come)', '')} ({info['first'][1]}) | {', '.join(info['people']) or '-'} |")
    out.append("")
    out.append("## 2. People, in the order they first appear\n")
    out.append("System members are one shared body: list each ALTER's look (hair, clothes, posture) for the ages shown.\n")
    out.append("| # | Who | Id | Kind | First in | Ages (the body) | Places |")
    out.append("|---|---|---|---|---|---|---|")
    for i, name in enumerate(person_order, 1):
        info = person_info[name]
        if name in members:
            kind, pid = "system member", members[name]["id"]
        elif name in people:
            kind, pid = "person", people[name]["id"]
        else:
            kind, pid = "person (no id yet)", "-"
        ages = ", ".join(str(a) for a in sorted(info["ages"])) or "-"
        out.append(f"| {i} | {name} | `{pid}` | {kind} | {info['first'][0].replace(' (title to come)', '')} | {ages} | {', '.join(info['places'])} |")
    out.append("")
    out.append("## 3. Close-ups and props, scene by scene\n")
    for pid in place_order:
        info = place_info[pid]
        if not info["looks"] and not info["memories"]:
            continue
        out.append(f"### {places.get(pid, {}).get('name', pid)} (`{pid}`)\n")
        seen = set()
        for look, text in info["looks"]:
            if look in seen:
                continue
            seen.add(look)
            out.append(f"- `{look}`: {text}")
        for mem in dict.fromkeys(info["memories"]):
            out.append(f"- hidden memory `{mem}` (a scene played only by an alter)")
        out.append("")
    print("\n".join(out))


if __name__ == "__main__":
    main()
