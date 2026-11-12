using System;

namespace IAmAsIAm.Core.Story
{
    /// <summary>
    /// What the marker at the start of each chapter part says (owner: "Maya, age 18" or "???, age 14"): whoever is playing
    /// the part, and their age. Maya's age is the body's age in the scene; an alter's is their own age. An alter the player
    /// hasn't met yet (not the host, not unlocked) is "???". Pure rules.
    /// </summary>
    public static class PartMarkerText
    {
        public const string Unknown = "???";

        public static string Name(string memberId, string displayName, bool known) =>
            known ? (string.IsNullOrEmpty(displayName) ? memberId ?? "" : displayName) : Unknown;

        /// <summary>"age 18", or "" if no age is known.</summary>
        public static string Age(string memberId, string memberAge, int bodyAge)
        {
            if (memberId == ChapterRuns.HostId) return bodyAge > 0 ? $"age {bodyAge}" : "";
            return string.IsNullOrWhiteSpace(memberAge) ? "" : $"age {memberAge.Trim()}";
        }

        /// <summary>"Maya, age 18" / "???, age 14" / "Theresa".</summary>
        public static string Line(string memberId, string displayName, bool known, string memberAge, int bodyAge)
        {
            string name = Name(memberId, displayName, known), age = Age(memberId, memberAge, bodyAge);
            return age.Length == 0 ? name : $"{name}, {age}";
        }

        static readonly string[] Months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        /// <summary>"October 2016" for the story's date, or "".</summary>
        public static string When(DateTime? date) => date.HasValue ? $"{Months[date.Value.Month - 1]} {date.Value.Year}" : "";
    }
}
