using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PassForge.Core.Utils
{
    internal static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string? s) =>
            string.IsNullOrWhiteSpace(s);

        public static string Safe(this string? s) =>
            s?.Trim() ?? string.Empty;

        public static string ToTitleCaseFast(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (input.Length == 1) return input.ToUpperInvariant();
            return char.ToUpperInvariant(input[0]) + input.Substring(1).ToLowerInvariant();
        }

        /// <summary>Convert Persian/Arabic numerals to Latin.</summary>
        public static string NormalizeDigits(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var sb = new StringBuilder(input.Length);
            foreach (var ch in input)
            {
                sb.Append(ch switch
                {
                    '\u06F0' or '\u0660' => '0',
                    '\u06F1' or '\u0661' => '1',
                    '\u06F2' or '\u0662' => '2',
                    '\u06F3' or '\u0663' => '3',
                    '\u06F4' or '\u0664' => '4',
                    '\u06F5' or '\u0665' => '5',
                    '\u06F6' or '\u0666' => '6',
                    '\u06F7' or '\u0667' => '7',
                    '\u06F8' or '\u0668' => '8',
                    '\u06F9' or '\u0669' => '9',
                    _ => ch
                });
            }
            return sb.ToString();
        }

        public static string DigitsOnly(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var sb = new StringBuilder();
            foreach (var ch in input)
                if (char.IsDigit(ch)) sb.Append(ch);
            return sb.ToString();
        }

        public static IEnumerable<string> DistinctTrimmed(this IEnumerable<string> source)
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var s in source)
            {
                var t = s.Safe();
                if (t.Length == 0) continue;
                if (seen.Add(t)) yield return t;
            }
        }

        public static IEnumerable<string> CaseVariants(this string word)
        {
            if (string.IsNullOrEmpty(word)) yield break;
            yield return word;                       // as-is
            yield return word.ToLowerInvariant();
            yield return word.ToUpperInvariant();
            yield return word.ToTitleCaseFast();
        }

        public static IEnumerable<string> LeetVariants(this string word)
        {
            if (string.IsNullOrEmpty(word)) yield break;

            // Simple and common mapping
            var map = new Dictionary<char, string[]>
            {
                ['a'] = new[] { "@", "4" },
                ['e'] = new[] { "3" },
                ['i'] = new[] { "1", "!" },
                ['o'] = new[] { "0" },
                ['s'] = new[] { "$", "5" },
                ['t'] = new[] { "7" },
                ['g'] = new[] { "9" },
                ['b'] = new[] { "8" }
            };

            // Incremental production to prevent compound explosion
            IEnumerable<string> acc = new[] { word };
            foreach (var kv in map)
            {
                var lower = kv.Key;
                var upper = char.ToUpperInvariant(lower);
                var repls = kv.Value;

                var next = new HashSet<string>(StringComparer.Ordinal);
                foreach (var variant in acc)
                {
                    next.Add(variant);
                    for (int i = 0; i < variant.Length; i++)
                    {
                        if (variant[i] == lower || variant[i] == upper)
                        {
                            foreach (var r in repls)
                            {
                                var sb = new StringBuilder(variant);
                                sb[i] = r[0];
                                next.Add(sb.ToString());
                            }
                        }
                    }
                }
                acc = next;
            }
            foreach (var v in acc) yield return v;
        }
    }
}
