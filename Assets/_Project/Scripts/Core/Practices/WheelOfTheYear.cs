using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.Practices
{
    public enum MoonPhase { New, WaxingCrescent, FirstQuarter, WaxingGibbous, Full, WaningGibbous, LastQuarter, WaningCrescent }

    /// <summary>
    /// The moon on any date (owner: a moon phase always shown). The mean synodic month counted from a known new moon
    /// (6 January 2000, 18:14 UTC): accurate to within about a day, which is all an esbat needs.
    /// </summary>
    public static class Moon
    {
        public const double SynodicMonth = 29.530588853;
        static readonly DateTime KnownNewMoon = new DateTime(2000, 1, 6, 18, 14, 0, DateTimeKind.Utc);

        /// <summary>The moon's age as a fraction of its cycle: 0 = new, 0.5 = full.</summary>
        public static double Age(DateTime date)
        {
            double days = (date.Date.AddHours(12) - KnownNewMoon).TotalDays;
            double age = (days % SynodicMonth) / SynodicMonth;
            return age < 0 ? age + 1 : age;
        }

        public static MoonPhase Phase(DateTime date)
        {
            int i = (int)Math.Floor(Age(date) * 8 + 0.5) % 8;
            return (MoonPhase)i;
        }

        /// <summary>Within a day and a half of the full moon (an esbat night).</summary>
        public static bool IsFull(DateTime date) => Math.Abs(Age(date) - 0.5) * SynodicMonth <= 1.5;

        /// <summary>Within a day and a half of the new moon.</summary>
        public static bool IsNew(DateTime date) { double a = Age(date); return Math.Min(a, 1 - a) * SynodicMonth <= 1.5; }

        public static string Name(MoonPhase p) => p switch
        {
            MoonPhase.New => "New moon",
            MoonPhase.WaxingCrescent => "Waxing crescent",
            MoonPhase.FirstQuarter => "First quarter",
            MoonPhase.WaxingGibbous => "Waxing gibbous",
            MoonPhase.Full => "Full moon",
            MoonPhase.WaningGibbous => "Waning gibbous",
            MoonPhase.LastQuarter => "Last quarter",
            _ => "Waning crescent",
        };
    }

    /// <summary>One of the eight sabbats (craft.json): its date (Northern Hemisphere), Tanya's text, and its home ritual.</summary>
    [Serializable]
    public sealed class SabbatDef
    {
        public string id;
        public string name;
        public int month;
        public int day;
        /// <summary>What Tanya texts on the day (owner: Tanya's sabbat texts). Only once she's given Maya the Book.</summary>
        public string tanyaText;
        public string arrive;
        public List<string> prepare = new List<string>();
        public string moment;
        public string reflect;
        public string close;
        /// <summary>How often it can be heavy (owner: some sabbats are hard sometimes; Samhain: Mira).</summary>
        public float hardChance;
        public string hardLine;
    }

    /// <summary>The Wheel of the Year: which sabbat is near a date.</summary>
    public static class WheelOfTheYear
    {
        /// <summary>The sabbat within <paramref name="days"/> days of the date (either side), or null.</summary>
        public static SabbatDef Near(DateTime date, IEnumerable<SabbatDef> sabbats, int days = 10)
        {
            SabbatDef best = null;
            double bestGap = double.MaxValue;
            foreach (var s in sabbats ?? Enumerable.Empty<SabbatDef>())
            {
                if (s == null || s.month < 1 || s.month > 12 || s.day < 1) continue;
                foreach (int year in new[] { date.Year - 1, date.Year, date.Year + 1 })
                {
                    var on = new DateTime(year, s.month, Math.Min(s.day, DateTime.DaysInMonth(year, s.month)));
                    double gap = Math.Abs((on - date.Date).TotalDays);
                    if (gap <= days && gap < bestGap) { best = s; bestGap = gap; }
                }
            }
            return best;
        }

        /// <summary>Parses "yyyy-mm-dd" (the saved story date). Null if empty or bad.</summary>
        public static DateTime? Parse(string iso)
        {
            if (string.IsNullOrEmpty(iso)) return null;
            return DateTime.TryParseExact(iso, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var d) ? d : (DateTime?)null;
        }
    }
}
