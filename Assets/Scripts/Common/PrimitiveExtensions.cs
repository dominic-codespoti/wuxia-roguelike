using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Common
{
    public static class PrimitiveExtensions
    {
        /// <summary>
        /// Converts an enum to a string with spaces between words and each words first letter is upper cased.
        /// </summary>
        public static string ToFriendlyString(this Enum value)
        {
            string enumString = value.ToString();
            var replacedString = Regex.Replace(enumString, @"(?!^)([A-Z])", " $1").Trim();
            return string.Join(' ', replacedString.Split(' ').Select(word => $"{char.ToUpper(word[0])}{word.Substring(1)}"));
        }

        /// <summary>
        /// Returns the instance ID of the GameObject as a string.
        /// </summary>
        public static string Id(this GameObject gameObject)
        {
            return gameObject.GetInstanceID().ToString();
        }

        /// <summary>
        /// Tries to return the component of type T from the GameObject.
        /// </summary>
        public static Maybe<T> MaybeGetComponent<T>(this GameObject gameObject)
        {
            T component = gameObject.GetComponent<T>();
            return component == null ? Maybe<T>.None : Maybe<T>.Some(component);
        }
    }
}
