using System.Collections.Generic;
using IAmAsIAm.Core;
using IAmAsIAm.Core.Practices;
using NUnit.Framework;

namespace IAmAsIAm.Tests
{
    /// <summary>
    /// Rule tests for the validator itself. The real data files are covered by RealDataTests.
    /// </summary>
    public class DataValidatorTests
    {
        [Test]
        public void HerbWithoutSafetyNotes_IsRejected()
        {
            var bundle = new DataValidator.Bundle
            {
                Herbs = new HerbData { herbs = new List<HerbEntry> { new HerbEntry { id = "x", latinName = "X x" } } },
            };
            var issues = DataValidator.Validate(bundle);
            Assert.That(issues, Has.Some.Contains("safety notes are mandatory"));
        }

        [Test]
        public void WhiteSage_IsRejected()
        {
            var bundle = new DataValidator.Bundle
            {
                Herbs = new HerbData
                {
                    herbs = new List<HerbEntry>
                    {
                        new HerbEntry { id = "ws", latinName = "Salvia apiana", safetyNotes = new List<string> { "n" } },
                    },
                },
            };
            Assert.That(DataValidator.Validate(bundle), Has.Some.Contains("white sage"));
        }

        [Test]
        public void HeavyTarotCard_RequiresGentleNote()
        {
            var bundle = new DataValidator.Bundle
            {
                Tarot = new TarotData
                {
                    cards = new List<TarotCardEntry>
                    {
                        new TarotCardEntry { id = "d", name = "Death", arcana = "Major", number = 13, reflectionPrompt = "p" },
                    },
                },
            };
            Assert.That(DataValidator.Validate(bundle), Has.Some.Contains("gentleNote"));
        }
    }
}
