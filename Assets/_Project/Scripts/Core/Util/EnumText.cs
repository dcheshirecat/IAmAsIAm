using System;

namespace IAmAsIAm.Core
{
    /// <summary>
    /// Data files store enums as readable strings, because Unity's JsonUtility writes enums as
    /// ints. This parses them leniently, with a fallback value.
    /// </summary>
    public static class EnumText
    {
        public static T Parse<T>(string value, T fallback) where T : struct, Enum =>
            !string.IsNullOrWhiteSpace(value) && Enum.TryParse(value.Trim(), true, out T result) ? result : fallback;

        public static bool IsValid<T>(string value) where T : struct, Enum =>
            !string.IsNullOrWhiteSpace(value) && Enum.TryParse(value.Trim(), true, out T _);
    }
}
