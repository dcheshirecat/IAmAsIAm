// InkCheck: compiles the game's Ink story with Ink's own compiler, writes main.json for Unity,
// then auto-plays it many times with random choices. It checks:
//   * the story compiles without errors or warnings,
//   * every playthrough reaches an end without runtime errors,
//   * tags (# exercise / # ritual / # explore / # cw / # cue ...) use known ids,
//   * game functions are called with known ids (members, skills, traditions, experiences).
// Drafts in Story/Drafts are compiled and played too, but never written out.
// Usage: tools/InkCheck/check.sh   (CI runs it on every push)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using IAmAsIAm.Core.Mind;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using IAmAsIAm.Core.Safety;
using IAmAsIAm.Core.Skills;
using IAmAsIAm.Core.Traditions;
using IAmAsIAm.Core.Triggers;

static class InkCheck
{
    static readonly List<string> Problems = new List<string>();
    static readonly JsonSerializerOptions Json = new JsonSerializerOptions { IncludeFields = true };

    static HashSet<string> _places = new HashSet<string>();
    static HashSet<string> _craft = new HashSet<string>();
    static HashSet<string> _people, _members, _skills, _practiceSkills, _traditions, _spreads, _charms, _cues;

    static int Main(string[] args)
    {
        string root = args.Length > 0 ? args[0] : FindRepoRoot();
        string storyDir = Path.Combine(root, "Assets", "_Project", "Story");
        string dataDir = Path.Combine(root, "Assets", "_Project", "Resources", "Data");
        LoadIds(dataDir);

        // The main story: compile, write main.json, play.
        var main = Compile(storyDir, File.ReadAllText(Path.Combine(storyDir, "main.ink")), "main.ink");
        if (main != null)
        {
            File.WriteAllText(Path.Combine(storyDir, "main.json"), main.ToJson());
            Play(main.ToJson(), "main.ink");
            // Every chapter in chapters.json, from its root knot (the game starts them there).
            using (var chapters = JsonDocument.Parse(File.ReadAllText(Path.Combine(dataDir, "chapters.json"))))
                foreach (var c in chapters.RootElement.GetProperty("chapters").EnumerateArray())
                {
                    string knot = c.GetProperty("inkKnot").GetString();
                    if (main.KnotContainerWithName(knot) == null) { Problems.Add($"chapters.json: no knot '{knot}' in the story"); continue; }
                    Play(main.ToJson(), "main.ink:" + knot, 200, knot);
                }
        }

        // Drafts: compiled against the same game API, played, never written out.
        string drafts = Path.Combine(storyDir, "Drafts");
        if (Directory.Exists(drafts))
            foreach (var file in Directory.GetFiles(drafts, "*.ink"))
            {
                string firstKnot = File.ReadLines(file).Select(l => l.Trim())
                    .FirstOrDefault(l => l.StartsWith("==="))?.Trim('=', ' ');
                string wrapper = $"INCLUDE game_api.ink\nINCLUDE Drafts/{Path.GetFileName(file)}\n-> {firstKnot}\n";
                var draft = Compile(storyDir, wrapper, "draft-wrapper-" + Path.GetFileName(file));
                if (draft != null) Play(draft.ToJson(), "Drafts/" + Path.GetFileName(file));
            }

        if (Problems.Count == 0)
        {
            Console.WriteLine("✓ Ink story compiles and every random playthrough ends cleanly.");
            return 0;
        }
        foreach (var p in Problems.Distinct()) Console.WriteLine("✗ " + p);
        return 1;
    }

    static Ink.Runtime.Story Compile(string storyDir, string source, string label)
    {
        var compiler = new Ink.Compiler(source, new Ink.Compiler.Options
        {
            sourceFilename = label,
            fileHandler = new StoryFileHandler(storyDir),
            errorHandler = (msg, type) => Problems.Add($"{label}: compile {type}: {msg}"),
        });
        var story = compiler.Compile();
        if (story == null) Problems.Add($"{label}: failed to compile.");
        return story;
    }

    static void Play(string json, string label, int runs = 300, string startKnot = null)
    {
        var rng = new Random(1234);
        for (int run = 0; run < runs; run++)
        {
            var story = new Ink.Runtime.Story(json);
            story.onError += (msg, type) => Problems.Add($"{label}: runtime {type}: {msg}");
            Bind(story, label);
            if (startKnot != null) story.ChoosePathString(startKnot);

            int steps = 0;
            string place = null;
            while (steps++ < 5000)
            {
                while (story.canContinue)
                {
                    story.Continue();
                    foreach (var tag in story.currentTags)
                    {
                        CheckTag(tag, label);
                        if (tag.Trim().StartsWith("place:")) place = tag.Substring(tag.IndexOf(':') + 1).Trim();
                    }
                }
                if (story.currentChoices.Count == 0) break;
                // Explore choices are walked to in a place (StoryScreen.TryExplore): a chapter must say where it is first.
                if (startKnot != null && place == null && !label.StartsWith("Drafts/")
                    && story.currentChoices.Any(c => System.Text.RegularExpressions.Regex.IsMatch(c.text ?? "", @"^(Look|Use|Talk|Go):")))
                    Problems.Add($"{label}: explore choices (\"{story.currentChoices[0].text}\") before any # place: (nothing to walk around in)");
                story.ChooseChoiceIndex(rng.Next(story.currentChoices.Count));
            }
            if (steps >= 5000) Problems.Add($"{label}: a playthrough never ended (possible infinite loop).");
        }
    }

    static readonly Random Rng = new Random(99);

    static void Bind(Ink.Runtime.Story story, string label)
    {
        // Drafts may use retired names (not canon), so only the real story is checked for known ids.
        void Check(bool ok, string what) { if (!ok && !label.StartsWith("Drafts/")) Problems.Add($"{label}: {what}"); }
        string S(object o) => o?.ToString() ?? "";

        story.BindExternalFunctionGeneral("set_experience", a =>
        {
            Check(Enum.TryParse<DissociativeExperience>(S(a[0]), out _), $"set_experience: unknown experience '{a[0]}'");
            return null;
        });
        story.BindExternalFunctionGeneral("nudge", a => null);
        story.BindExternalFunctionGeneral("adjust_trust", a => { Check(_members.Contains(S(a[0])), $"adjust_trust: unknown member '{a[0]}'"); return null; });
        story.BindExternalFunctionGeneral("learn_skill", a => { Check(_skills.Contains(S(a[0])), $"learn_skill: unknown skill '{a[0]}'"); return null; });
        story.BindExternalFunctionGeneral("record_choice", a =>
        {
            Check(S(a[0]).Length > 0 && S(a[1]).Length > 0, "record_choice: needs a decision and an option id");
            return null;
        });
        story.BindExternalFunctionGeneral("journal", a =>
        {
            Check(Enum.TryParse<JournalEntryKind>(S(a[0]), out _), $"journal: unknown entry kind '{a[0]}'");
            return null;
        });
        story.BindExternalFunctionGeneral("open_gap", a => null);
        story.BindExternalFunctionGeneral("find_clue", a => null);
        // Hidden memories: random answers, so the auto-play walks both the locked and the unlocked paths.
        story.BindExternalFunctionGeneral("unlocked", a => { Check(_members.Contains(S(a[0])), $"unlocked: unknown member '{a[0]}'"); return Rng.Next(2) == 0; });
        story.BindExternalFunctionGeneral("fronts", a => { Check(_members.Contains(S(a[0])), $"fronts: unknown member '{a[0]}'"); return Rng.Next(2) == 0; });
        // Devices: contacts are people ids; ages and devices as the owner set them.
        void Contact(object c, string fn) => Check(_people.Contains(S(c)), $"{fn}: unknown contact '{c}' (people.json)");
        story.BindExternalFunctionGeneral("set_age", a => { Check(int.TryParse(S(a[0]), out int age) && age >= 0 && age < 120, $"set_age: not an age '{a[0]}'"); return null; });
        story.BindExternalFunctionGeneral("gain_device", a => { Check(new[] { "computer", "phone", "laptop" }.Contains(S(a[0])), $"gain_device: unknown device '{a[0]}'"); return null; });
        story.BindExternalFunctionGeneral("text_from", a => { Contact(a[0], "text_from"); return null; });
        story.BindExternalFunctionGeneral("text_to", a => { Contact(a[0], "text_to"); return null; });
        story.BindExternalFunctionGeneral("email_from", a => { Contact(a[0], "email_from"); return null; });
        story.BindExternalFunctionGeneral("email_to", a => { Contact(a[0], "email_to"); return null; });
        story.BindExternalFunctionGeneral("sent_while_away", a =>
        {
            Check(_members.Contains(S(a[0])), $"sent_while_away: unknown member '{a[0]}'");
            Check(S(a[1]) == "text" || S(a[1]) == "email", $"sent_while_away: channel must be text or email, not '{a[1]}'");
            Contact(a[2], "sent_while_away");
            return null;
        });
        story.BindExternalFunctionGeneral("gain_app", a => { Check(S(a[0]) == "systemlog", $"gain_app: unknown app '{a[0]}'"); return null; });
        story.BindExternalFunctionGeneral("system_log", a =>
        {
            Check(_members.Contains(S(a[0])), $"system_log: unknown member '{a[0]}'");
            Check(S(a[1]).Trim().Length > 0, "system_log: empty note");
            return null;
        });
        story.BindExternalFunctionGeneral("co_conscious", a => { Check(_members.Contains(S(a[0])), $"co_conscious: unknown member '{a[0]}'"); return null; });
        // The Book of Shadows: known practices (craft.json), members, tabs.
        story.BindExternalFunctionGeneral("gain_book_of_shadows", a => { Contact(a[0], "gain_book_of_shadows"); return null; });
        story.BindExternalFunctionGeneral("learn_practice", a =>
        {
            Check(_craft.Contains(S(a[0])), $"learn_practice: unknown practice '{a[0]}' (craft.json)");
            if (S(a[1]).Length > 0) Contact(a[1], "learn_practice");
            return null;
        });
        story.BindExternalFunctionGeneral("bos_entry", a =>
        {
            Check(_members.Contains(S(a[0])), $"bos_entry: unknown member '{a[0]}'");
            Check(Enum.TryParse<BookTab>(S(a[1]), true, out _), $"bos_entry: unknown tab '{a[1]}' (Practices, Readings, Wheel, Dreams, Notes)");
            Check(S(a[3]).Trim().Length > 0, "bos_entry: empty entry");
            return null;
        });
        story.BindExternalFunctionGeneral("practised", a => { Check(_craft.Contains(S(a[0])), $"practised: unknown practice '{a[0]}'"); return Rng.Next(2) == 0; });
        story.BindExternalFunctionGeneral("explore_tradition", a => { Check(_traditions.Contains(S(a[0])), $"explore_tradition: unknown tradition '{a[0]}'"); return null; });
        story.BindExternalFunctionGeneral("card_name", a => "The Star");
        story.BindExternalFunctionGeneral("card_prompt", a => "A prompt.");
    }

    static void CheckTag(string tag, string label)
    {
        int colon = tag.IndexOf(':');
        string key = (colon < 0 ? tag : tag.Substring(0, colon)).Trim();
        string value = colon < 0 ? "" : tag.Substring(colon + 1).Trim();
        void Fail(string why) => Problems.Add($"{label}: tag '#{tag}': {why}");

        switch (key)
        {
            case "exercise": if (!_practiceSkills.Contains(value)) Fail("not a skill with a practice exercise"); break;
            case "explore": if (!_traditions.Contains(value)) Fail("unknown tradition"); break;
            case "cw": if (!ContentTags.All.Contains(value)) Fail("unknown content tag"); break;
            case "cue": case "cue-held": if (_cues.Count > 0 && !_cues.Contains(value)) Fail("no trigger listens for this cue (triggers.json)"); break;
            case "ritual":
                var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) { Fail("empty ritual"); break; }
                if (parts[0] == "tarot" && parts.Length >= 2 && !_spreads.Contains(parts[1])) Fail("unknown tarot spread");
                else if (parts[0] == "tarot" || parts[0] == "candle" || parts[0] == "grounding-centering") { }
                else if (parts[0] == "craft" && (parts.Length < 2 || !_charms.Contains(parts[1]))) Fail("unknown charm");
                else if (parts[0] != "tarot" && parts[0] != "craft" && parts[0] != "rune" && parts[0] != "sachet") Fail("unknown ritual");
                break;
            case "switch": if (!_members.Contains(value)) Fail("unknown member to switch to"); break;
            case "vo":
                // A voice line's file name without "vo_" and ".mp3": <chapter-id>_<speaker>_<4-digit number> (docs/VOICE.md).
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-z0-9-]+_[a-z0-9-]+_\d{4}$")) Fail("voice tag must look like prologue-quaker_mira_0012");
                break;
            case "objective": break; // Empty clears the goal line.
            case "look": case "memory": if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-z0-9-]+$")) Fail("needs a lowercase id"); break;
            case "maybe-switch": if (value.Length > 0 && (!int.TryParse(value, out int pct) || pct < 1 || pct > 100)) Fail("percent must be 1-100"); break;
            case "place": if (_places.Count > 0 && !label.StartsWith("Drafts/") && !_places.Contains(value)) Fail("unknown place (add it to places.json: name, city, map position)"); break;
            case "shot": case "chapter-end": case "front": break;
            case "device": if (value != "computer" && value != "phone" && value != "laptop") Fail("device must be computer, phone or laptop"); break;
            default: Fail("unknown tag (see game_api.ink)"); break;
        }
    }

    static void LoadIds(string dataDir)
    {
        T Load<T>(string f) => JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine(dataDir, f)), Json);
        var skills = Load<SkillCatalogData>("skills.json").skills;
        _members = new HashSet<string>(Load<SystemMembersData>("system_members.json").members.Select(m => m.id));
        _skills = new HashSet<string>(skills.Select(s => s.id));
        _practiceSkills = new HashSet<string>(skills.Where(s => s.hasPracticeMode).Select(s => s.id));
        _traditions = new HashSet<string>(Load<TraditionData>("traditions.json").traditions.Select(t => t.id));
        _spreads = new HashSet<string>(Load<TarotSpreadData>("tarot_spreads.json").spreads.Select(s => s.id));
        _charms = new HashSet<string>(Load<CharmData>("charms.json").charms.Select(c => c.id));
        _cues = new HashSet<string>(Load<TriggerData>("triggers.json").triggers.SelectMany(t => t.cues));
        _craft = new HashSet<string>(Load<CraftData>("craft.json").practices.Select(p => p.id));
        _places = new HashSet<string>(Load<IAmAsIAm.Core.Story.PlaceData>("places.json").places.Select(p => p.id));
        using (var people = JsonDocument.Parse(File.ReadAllText(Path.Combine(dataDir, "people.json"))))
            _people = new HashSet<string>(people.RootElement.GetProperty("people").EnumerateArray().Select(p => p.GetProperty("id").GetString()));
    }

    static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Assets", "_Project"))) dir = dir.Parent;
        return dir?.FullName ?? throw new InvalidOperationException("Run from inside the repository.");
    }

    sealed class StoryFileHandler : Ink.IFileHandler
    {
        readonly string _root;
        public StoryFileHandler(string root) => _root = root;
        public string ResolveInkFilename(string includeName) => Path.Combine(_root, includeName);
        public string LoadInkFileContents(string fullFilename) => File.ReadAllText(fullFilename);
    }
}
