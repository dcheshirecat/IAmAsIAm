using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using IAmAsIAm.Core.PluralSystem;
using IAmAsIAm.Core.Practices;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    public sealed class AlterPracticeTests
    {
        static string DataDir([CallerFilePath] string here = "") =>
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(here), "..", "..", "Resources", "Data"));

        static OwnPracticeData Load()
        {
            string json = File.ReadAllText(Path.Combine(DataDir(), "own_practices.json"));
#if UNITY_5_3_OR_NEWER
            return UnityEngine.JsonUtility.FromJson<OwnPracticeData>(json);
#else
            return System.Text.Json.JsonSerializer.Deserialize<OwnPracticeData>(json, new System.Text.Json.JsonSerializerOptions { IncludeFields = true });
#endif
        }

        [Test]
        public void Practices_OpenAsMayaGetsToKnowThem()
        {
            var bonds = new AlterBonds(Load());
            Assert.That(bonds.Unlocked("jacob"), Is.Empty, "not known yet");
            bonds.GetToKnow("jacob", 2);
            Assert.That(bonds.Unlocked("jacob").Select(p => p.id), Is.EqualTo(new[] { "jacob-sign" }));
            bonds.GetToKnow("jacob", 3);
            Assert.That(bonds.Unlocked("jacob").Count, Is.EqualTo(2));
            bonds.GetToKnow("jacob", 4);
            Assert.That(bonds.Unlocked("jacob").Count, Is.EqualTo(3));
            Assert.That(bonds.Unlocked("nina"), Is.Empty, "one alter at a time");

            var copy = new AlterBonds(Load());
            copy.Restore(bonds.Export());
            Assert.That(copy.Known("jacob"), Is.EqualTo(9));
        }

        [Test]
        public void EveryOwnPractice_IsARitual_NeverInTheBook()
        {
            var data = Load();
            var members = new HashSet<string> { "serena", "nina", "lily", "heather", "jacob", "axel" };
            Assert.That(data.practices.Select(p => p.memberId).Distinct(), Is.EquivalentTo(members), "each alter with a way of their own");
            foreach (var m in members)
                Assert.That(data.practices.Where(p => p.memberId == m).Select(p => p.level).OrderBy(l => l), Is.EqualTo(new[] { 1, 2, 3 }), m);
            foreach (var p in data.practices)
            {
                var s = new RitualSession(p, p.memberId, 1);
                var phases = new List<RitualPhase>();
                int guard = 0;
                while (!s.IsDone && guard++ < 40)
                {
                    if (phases.Count == 0 || phases[phases.Count - 1] != s.Current.Phase) phases.Add(s.Current.Phase);
                    s.Answer(s.Current.Kind == RitualStepKind.Choose ? s.Current.Options[0].Id : "words");
                }
                Assert.That(phases, Is.EqualTo(new[] { RitualPhase.Arrive, RitualPhase.Prepare, RitualPhase.Moment, RitualPhase.Reflect, RitualPhase.Close }), p.id);
                Assert.That(s.Result.Private, Is.True, p.id);
            }
            // Owner: Heather's faith stays a mystery all game.
            foreach (var p in data.practices.Where(x => x.memberId == "heather"))
                foreach (var st in p.steps)
                    Assert.That((st.text ?? "").ToLowerInvariant(), Does.Not.Contain("faith").And.Not.Contain("pray").And.Not.Contain("god"), p.id);
        }
    }
}
