using System;
using System.Collections.Generic;

namespace PassForge.Core.Models
{
    [Flags]
    public enum CharCategory
    {
        None = 0,
        Lower = 1,
        Upper = 2,
        Digit = 4,
        Special = 8
    }

    public class PasswordOptions
    {
        /// <summary>Restrictions on the allowable character sets (filter). If Special=false, passwords with special characters are removed.</summary>
        public bool IncludeLowercase { get; set; } = true;
        public bool IncludeUppercase { get; set; } = true;
        public bool IncludeDigits { get; set; } = true;
        public bool IncludeSpecial { get; set; } = true;

        /// <summary>If true, it tries to ensure that the password contains at least one character from each selected category.</summary>
        public bool RequireAtLeastOneFromEachSelectedCategory { get; set; } = false;

        public string SpecialCharacters { get; set; } = "!@#$%^&*._-";
        public string Separators { get; set; } = "._-";

        public int MinLength { get; set; } = 8;
        public int MaxLength { get; set; } = 16;

        /// <summary>Maximum number of output passwords (to prevent combination explosion).</summary>
        public int MaxCount { get; set; } = 5000;

        /// <summary>If true, l33t versions (a→@, i→1, e→3, o→0, s→$|5, t→7, etc.) are generated.</summary>
        public bool UseLeet { get; set; } = true;

        /// <summary>If true, a reverse version of each token is also generated..</summary>
        public bool IncludeReversed { get; set; } = true;

        /// <summary>Generate different letter cases (lower/upper/title/asic).</summary>
        public bool IncludeWordCaseVariants { get; set; } = true;

        /// <summary>To calculate year of birth from age, if we don't have DOB.</summary>
        public DateTime ReferenceDate { get; set; } = DateTime.UtcNow;

        /// <summary>Maximum number of tokens that can be combined with separators (2 means 1- and 2-part combinations).</summary>
        public int MaxTokensPerPassword { get; set; } = 2;

        /// <summary>Custom suffixes for trailing appendages (e.g. !, 1, 123, @1234)</summary>
        public IEnumerable<string> CustomSuffixes { get; set; } = new[] { "!", "1", "123" };

        /// <summary>Add common numeric patterns (birth year, month/day, 00-99).</summary>
        public bool AppendCommonNumbers { get; set; } = true;
    }
}
