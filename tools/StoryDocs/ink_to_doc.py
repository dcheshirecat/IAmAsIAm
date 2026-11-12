#!/usr/bin/env python3
"""Turns an Ink draft into a readable document for the owner (Markdown that Google Docs imports well).

    python3 tools/StoryDocs/ink_to_doc.py Assets/_Project/Story/Drafts/draft_prologue_sunday2.ink > out.md

- The header comment's "My guesses" list becomes a "To check" section.
- Each stitch (= name) becomes a heading.
- "Name: line" becomes a bold speaker; "(inside)" voices are kept.
- A choice becomes "Choice" with one bullet per option; what happens after an option is indented under it.
- Game instructions (~ lines, # tags, -> diverts) are left out; # exercise: tags become a short note.
"""
import re
import sys


def humanize(name):
    return name.replace("_", " ").strip().capitalize()


def convert(text):
    lines = text.split("\n")
    title, beats, guesses = "", [], []
    mode = None
    body_start = 0
    for i, raw in enumerate(lines):
        l = raw.strip()
        if not l.startswith("//"):
            body_start = i
            break
        c = l[2:].strip(" ─")
        if c.startswith("DRAFT:"):
            title = c[len("DRAFT:"):].strip()
        elif c.startswith("My guesses"):
            mode = "guesses"
        elif c.startswith("- ") and mode == "guesses":
            guesses.append(c[2:])
        elif c.startswith("  ") and mode == "guesses" and guesses:
            guesses[-1] += " " + c.strip()
        elif mode == "guesses" and raw.startswith("//     ") and guesses:
            guesses[-1] += " " + c.strip()
        elif c and mode is None and not c.startswith("vo-speakers:"):
            beats.append(c)

    out = []
    heading = next((b for b in beats if re.match(r"^Sunday \d|^Chapter|^Prelude|^Prologue", b)), "")
    out.append(f"# {heading or title}")
    out.append("")
    out.append("_Draft, not canon until you approve it. Edit anything here: Claude carries your changes back into the game._")
    out.append("")
    notes = [b for b in beats if b != heading]
    if notes:
        out.append("**What happens (your beats):** " + re.sub(r"Owner.s beats: ", "", " ".join(notes)))
        out.append("")
    if guesses:
        out.append("**My guesses, for you to check:**")
        out.append("")
        for g in guesses:
            out.append(f"- {g}")
        out.append("")

    in_choice = False
    for raw in lines[body_start:]:
        indent = len(raw) - len(raw.lstrip(" "))
        l = raw.strip()
        if not l or l.startswith("//") or l.startswith("~") or l.startswith("->") or l.startswith("==="):
            continue
        m = re.match(r"^= (\w+)", l)
        if m:
            in_choice = False
            out += ["", f"## {humanize(m.group(1))}", ""]
            continue
        ex = re.search(r"#\s*exercise:\s*([\w-]+)", l)
        ob = re.search(r"#\s*objective:\s*([^#]+)", l)
        if ob:
            out += ["", f"> **Objective:** {ob.group(1).strip()}", ""]
        mem = re.search(r"#\s*memory:\s*([\w-]+)", l)
        if mem:
            out += ["", f"_(A memory scene: \"{mem.group(1)}\")_", ""]
        if re.search(r"#\s*maybe-switch", l):
            out += ["_(An unexpected switch may happen here, if another alter is unlocked.)_", ""]
        sw = re.search(r"#\s*switch:\s*([\w-]+)", l)
        if sw:
            out += [f"_(A switch: {sw.group(1).capitalize()} comes forward. The player has no control over it.)_", ""]
        if l.startswith("#"):
            continue
        if re.search(r"#\s*chapter-end", l):
            l = re.sub(r"\s*#.*$", "", l)
            out += [l, "", "_(The chapter ends here, with the recap of the player's choices.)_", ""]
            continue
        if l == "-" or re.match(r"^-\s*\(\w+\)\s*$", l):
            in_choice = False
            out.append("")
            continue
        l = re.sub(r"\s*#.*$", "", l)
        fr = re.search(r'fronts\("([\w-]+)"\)', l)
        fr_note = f"    - _(Only glows while {fr.group(1).capitalize()} is fronting. Nothing tells the player.)_" if fr else None
        opt = re.match(r"^[*+]\s*(\{[^}]*\}\s*)?(\(\w+\)\s*)?\[(.*)\]\s*(.*)$", l)
        if opt:
            label = opt.group(3)
            if not in_choice:
                explore = re.match(r"^(Look|Use|Talk|Go|Wait)\b", label)
                out += ["", "**Explore** (glowing things; the player picks):" if explore else "**Choice:**", ""]
                in_choice = True
            out.append(f"- **{label}**")
            if fr_note:
                out.append(fr_note)
            rest = re.sub(r"\s*->.*$", "", opt.group(4)).strip()
            if rest:
                out.append(f"    - {rest}")
            continue
        cond = re.match(r'^\{\s*unlocked\("([\w-]+)"\)\s*:\s*->\s*\w+\s*\}$', l)
        if cond:
            out += [f"_(If {cond.group(1).capitalize()} has been unlocked, the memory below plays. Otherwise it's skipped.)_", ""]
            continue

        l = re.sub(r"\{[\w.]+: (.*?) \| (.*?)\}", r"\2 [or, if earlier: \1]", l)
        spk = re.match(r"^([A-Z][\w.' ]*?(?: \(inside\))?): (.*)$", l)
        if spk and len(spk.group(1)) <= 24:
            l = f"**{spk.group(1)}:** {spk.group(2)}"
        if ex:
            l += f" _(the player does the \"{ex.group(1)}\" exercise here)_"
        if in_choice and indent >= 4:
            out.append(f"    - {l}")
        else:
            if in_choice:
                in_choice = False
                out.append("")
            out += [l, ""]
    doc = "\n".join(out)
    return re.sub(r"\n{3,}", "\n\n", doc).strip() + "\n"


if __name__ == "__main__":
    sys.stdout.write(convert(open(sys.argv[1], encoding="utf-8").read()))
