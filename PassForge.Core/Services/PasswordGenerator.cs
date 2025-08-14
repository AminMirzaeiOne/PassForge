using PassForge.Core.Models;
using PassForge.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassForge.Core.Services
{
    public class PasswordGenerator
    {
        private readonly PasswordOptions _opt;

        public PasswordGenerator(PasswordOptions? options = null)
        {
            _opt = options ?? new PasswordOptions();
            if (_opt.MinLength < 1) _opt.MinLength = 1;
            if (_opt.MaxLength < _opt.MinLength) _opt.MaxLength = _opt.MinLength;
            if (_opt.MaxTokensPerPassword < 1) _opt.MaxTokensPerPassword = 1;
            if (_opt.MaxCount < 1) _opt.MaxCount = 1000;
        }

        public IEnumerable<string> Generate(UserInfo user)
        {
            if (user == null) yield break;

            var words = BuildWordTokens(user).ToList();
            var numbers = BuildNumberTokens(user).ToList();

            // Variety of letter cases and l33t
            var wordVariants = ExpandWordVariants(words).ToList();

            // Token Combination
            var candidates = ComposeCandidates(wordVariants, numbers);

            // Adding custom extensions
            if (_opt.CustomSuffixes != null && _opt.CustomSuffixes.Any())
            {
                candidates = candidates.SelectMany(c => _opt.CustomSuffixes!.Select(suf => c + suf).Prepend(c));
            }

            // Length and character set filter + duplicate removal + number limit
            var filtered = FilterAndCap(candidates);

            foreach (var pass in filtered)
                yield return pass;
        }

        // ---------- Step 1: Extract tokens from input ----------
        private IEnumerable<string> BuildWordTokens(UserInfo user)
        {
            IEnumerable<string> baseWords()
            {
                yield return user.FirstName.Safe();
                yield return user.LastName.Safe();
                if (!user.FirstName.IsNullOrWhiteSpace() && !user.LastName.IsNullOrWhiteSpace())
                {
                    yield return user.FirstName.Safe() + user.LastName.Safe();
                    yield return user.LastName.Safe() + user.FirstName.Safe();
                }

                if (!user.Nickname.IsNullOrWhiteSpace()) yield return user.Nickname!.Safe();

                if (!user.Email.IsNullOrWhiteSpace())
                {
                    var email = user.Email!.Safe();
                    var at = email.IndexOf('@');
                    if (at > 0)
                    {
                        var local = email.Substring(0, at);
                        var domain = email[(at + 1)..];
                        yield return local;
                        // You can also use the part before the dot in the domain.
                        var dot = domain.IndexOf('.');
                        if (dot > 0) yield return domain[..dot];
                    }
                }

                if (user.ExtraWords is { Count: > 0 })
                {
                    foreach (var w in user.ExtraWords)
                        yield return w.Safe();
                }
            }

            var words = baseWords().DistinctTrimmed().ToList();

            if (_opt.IncludeReversed)
            {
                var reversed = words
                    .Where(w => w.Length > 1)
                    .Select(w => new string(w.Reverse().ToArray()));
                words.AddRange(reversed);
                words = words.DistinctTrimmed().ToList();
            }

            return words;
        }

        private IEnumerable<string> BuildNumberTokens(UserInfo user)
        {
            var nums = new HashSet<string>(StringComparer.Ordinal);

            // From the date of birth : yyyy, yy, yyyymmdd, yymmdd, mmdd
            if (user.DateOfBirth.HasValue)
            {
                var d = user.DateOfBirth.Value;
                nums.Add(d.Year.ToString()); // yyyy
                nums.Add((d.Year % 100).ToString("00")); // yy
                nums.Add($"{d:yyyyMM}");
                nums.Add($"{d:yyyyMMdd}");
                nums.Add($"{d:yyMMdd}");
                nums.Add($"{d:MMdd}");
                nums.Add(d.Month.ToString("00"));
                nums.Add(d.Day.ToString("00"));
            }
            else if (user.AgeYears.HasValue)
            {
                // Approximate year of birth based on age
                var y = Math.Max(0, _opt.ReferenceDate.Year - user.AgeYears.Value);
                nums.Add(y.ToString());
                nums.Add((y % 100).ToString("00"));
            }

            // From phone number: last 2 to 4 digits and common patterns
            if (!user.Phone.IsNullOrWhiteSpace())
            {
                var digits = user.Phone!.NormalizeDigits().DigitsOnly();
                if (digits.Length >= 2) nums.Add(digits[^2..]);
                if (digits.Length >= 3) nums.Add(digits[^3..]);
                if (digits.Length >= 4) nums.Add(digits[^4..]);
                // Prefix or operator code (first 2-3 digits after zero)
                if (digits.StartsWith("0") && digits.Length >= 4)
                {
                    nums.Add(digits.Substring(1, 2));
                    nums.Add(digits.Substring(1, 3));
                }
            }

            // from age
            if (user.AgeYears.HasValue) nums.Add(user.AgeYears.Value.ToString("00"));
            if (user.AgeMonths.HasValue) nums.Add(user.AgeMonths.Value.ToString("00"));
            if (user.AgeDays.HasValue) nums.Add(user.AgeDays.Value.ToString("00"));

            // General patterns
            if (_opt.AppendCommonNumbers)
            {
                nums.Add("123");
                nums.Add("321");
                nums.Add("007");
                nums.Add("999");
                nums.Add(_opt.ReferenceDate.Year.ToString());
                nums.Add((_opt.ReferenceDate.Year % 100).ToString("00"));
            }

            return nums.Where(n => n.Length > 0);
        }

        // ---------- Step 2: Expanding Modes----------
        private IEnumerable<string> ExpandWordVariants(IEnumerable<string> words)
        {
            var set = new HashSet<string>(StringComparer.Ordinal);

            foreach (var w in words)
            {
                IEnumerable<string> variants = new[] { w };

                if (_opt.IncludeWordCaseVariants)
                    variants = variants.SelectMany(x => x.CaseVariants());

                if (_opt.UseLeet)
                    variants = variants.SelectMany(x => x.LeetVariants());

                foreach (var v in variants)
                    if (set.Add(v)) { /* keep unique */ }
            }

            return set;
        }

        // ---------- Step 3: Combinations ----------
        private IEnumerable<string> ComposeCandidates(List<string> words, List<string> numbers)
        {
            var outSet = new HashSet<string>(StringComparer.Ordinal);

            // 3.1 Word-only or number-only
            foreach (var w in words)
                if (TryAdd(outSet, w)) yield return w;

            foreach (var n in numbers)
                if (TryAdd(outSet, n)) yield return n;

            // 3.2 Word+number, number+word combinations (with and without separators)
            foreach (var w in words)
            {
                foreach (var n in numbers)
                {
                    var s1 = w + n;
                    var s2 = n + w;
                    if (TryAdd(outSet, s1)) yield return s1;
                    if (TryAdd(outSet, s2)) yield return s2;

                    foreach (var sep in _opt.Separators)
                    {
                        var s3 = $"{w}{sep}{n}";
                        var s4 = $"{n}{sep}{w}";
                        if (TryAdd(outSet, s3)) yield return s3;
                        if (TryAdd(outSet, s4)) yield return s4;
                    }
                }
            }

            // 3.3 Two-word combinations (with and without separators)
            if (_opt.MaxTokensPerPassword >= 2)
            {
                for (int i = 0; i < words.Count; i++)
                {
                    for (int j = 0; j < words.Count; j++)
                    {
                        if (i == j) continue;
                        var a = words[i];
                        var b = words[j];

                        var s1 = a + b;
                        if (TryAdd(outSet, s1)) yield return s1;

                        foreach (var sep in _opt.Separators)
                        {
                            var s2 = $"{a}{sep}{b}";
                            if (TryAdd(outSet, s2)) yield return s2;
                        }
                    }
                }
            }
        }

        // ---------- Step 4: Filtering and Restricting ----------
        private IEnumerable<string> FilterAndCap(IEnumerable<string> candidates)
        {
            int count = 0;
            foreach (var c in candidates)
            {
                if (c.Length < _opt.MinLength || c.Length > _opt.MaxLength)
                    continue;

                if (!IsCharsetAllowed(c)) continue;
                if (_opt.RequireAtLeastOneFromEachSelectedCategory && !SatisfiesCategoryRequirement(c))
                    continue;

                yield return c;
                count++;
                if (count >= _opt.MaxCount) yield break;
            }
        }

        private bool IsCharsetAllowed(string s)
        {
            foreach (var ch in s)
            {
                if (char.IsLetter(ch))
                {
                    if (char.IsLower(ch) && !_opt.IncludeLowercase) return false;
                    if (char.IsUpper(ch) && !_opt.IncludeUppercase) return false;
                }
                else if (char.IsDigit(ch))
                {
                    if (!_opt.IncludeDigits) return false;
                }
                else
                {
                    // special
                    if (!_opt.IncludeSpecial) return false;

                    // If special is allowed, but the character was not in the list (tightening the restriction):
                    if (!_opt.SpecialCharacters.Contains(ch))
                        return false;
                }
            }
            return true;
        }

        private bool SatisfiesCategoryRequirement(string s)
        {
            bool hasLower = false, hasUpper = false, hasDigit = false, hasSpecial = false;

            foreach (var ch in s)
            {
                if (char.IsLower(ch)) hasLower = true;
                else if (char.IsUpper(ch)) hasUpper = true;
                else if (char.IsDigit(ch)) hasDigit = true;
                else hasSpecial = true;
            }

            if (_opt.IncludeLowercase && !hasLower) return false;
            if (_opt.IncludeUppercase && !hasUpper) return false;
            if (_opt.IncludeDigits && !hasDigit) return false;
            if (_opt.IncludeSpecial && !hasSpecial) return false;
            return true;
        }

        private static bool TryAdd(HashSet<string> set, string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return set.Add(s);
        }
    }
}
