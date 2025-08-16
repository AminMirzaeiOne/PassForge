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

    /// <summary>
    /// Configuration options for password generation.
    /// </summary>
    public class PasswordOptions
    {
        /// <summary>
        /// Minimum length of generated passwords.
        /// </summary>
        public int MinLength { get; set; } = 6;

        /// <summary>
        /// Maximum length of generated passwords.
        /// </summary>
        public int MaxLength { get; set; } = 16;

        /// <summary>
        /// Include lowercase letters (a-z) in passwords.
        /// </summary>
        public bool IncludeLowercase { get; set; } = true;

        /// <summary>
        /// Include uppercase letters (A-Z) in passwords.
        /// </summary>
        public bool IncludeUppercase { get; set; } = true;

        /// <summary>
        /// Include digits (0-9) in passwords.
        /// </summary>
        public bool IncludeDigits { get; set; } = true;

        /// <summary>
        /// Include special characters (e.g., !, @, #) in passwords.
        /// Default is false for safer, simpler passwords.
        /// </summary>
        public bool IncludeSpecial { get; set; } = false;

        /// <summary>
        /// Apply "Leet" substitutions (e.g., a -> 4, e -> 3) in passwords.
        /// </summary>
        public bool UseLeet { get; set; } = false;

        /// <summary>
        /// Include reversed versions of base words (e.g., "John" -> "nhoJ").
        /// </summary>
        public bool IncludeReversed { get; set; } = true;

        /// <summary>
        /// Maximum number of passwords to generate.
        /// </summary>
        public int MaxCount { get; set; } = 5000;

        /// <summary>
        /// List of special separator characters to use when IncludeSpecial is true.
        /// </summary>
        public List<char> Separators { get; set; } = new() { '.', '_', '-' };
    }
}
