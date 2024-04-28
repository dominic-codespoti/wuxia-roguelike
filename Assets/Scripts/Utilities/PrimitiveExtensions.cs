using System;
using System.Text.RegularExpressions;

public static class PrimitiveExtensions
{
    /// <summary>
    /// Converts an enum to a string with spaces between words and each word uppercased.
    /// </summary>
    public static string ToFriendlyString(this Enum value)
    {
        string enumString = value.ToString();
        return Regex.Replace(enumString, @"(?!^)([A-Z])", " $1").Trim().ToUpperInvariant();
    }
}
