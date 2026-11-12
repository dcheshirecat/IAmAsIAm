using System;
using IAmAsIAm.Core.Story;
using NUnit.Framework;

namespace IAmAsIAm.Tests.EditMode
{
    public class PartMarkerTests
    {
        [Test]
        public void Host_UsesTheBodysAge()
        {
            Assert.That(PartMarkerText.Line("maya", "Maya", true, "26", 18), Is.EqualTo("Maya, age 18"));
        }

        [Test]
        public void UnmetAlter_IsUnknown_WithTheirOwnAge()
        {
            Assert.That(PartMarkerText.Line("jacob", "Jacob", false, "14", 21), Is.EqualTo("???, age 14"));
            Assert.That(PartMarkerText.Line("jacob", "Jacob", true, "14", 21), Is.EqualTo("Jacob, age 14"));
        }

        [Test]
        public void NoAge_JustTheName_AndTheMonth()
        {
            Assert.That(PartMarkerText.Line("theresa", "Theresa", true, "", 20), Is.EqualTo("Theresa"));
            Assert.That(PartMarkerText.When(new DateTime(2016, 10, 27)), Is.EqualTo("October 2016"));
            Assert.That(PartMarkerText.When(null), Is.EqualTo(""));
        }
    }
}
