using PassForge.Core.Models;
using System.Text;

namespace PassForge.Core.Services
{
    public class PasswordGenerator
    {
        private readonly PasswordOptions _opt;

        public PasswordGenerator(PasswordOptions options)
        {
            _opt = options ?? new PasswordOptions();
        }

        // Entry method to generate passwords
        public List<string> Generate(UserInfo info)
        {
            var words = CollectBaseWords(info);

            // Add meaningful digits only
            if (_opt.IncludeDigits)
            {
                var numericTokens = GenerateNumericTokens(info);
                words.AddRange(numericTokens);
            }

            // Add reversed words if option enabled
            if (_opt.IncludeReversed)
            {
                var reversed = words
                    .Where(w => w.Length > 1)
                    .Select(w => new string(w.Reverse().ToArray()))
                    .ToList(); // avoid collection modified exception
                words.AddRange(reversed);
            }

            words = words.DistinctTrimmed().ToList();

            // Optionally add special characters
            if (_opt.IncludeSpecial)
            {
                var specialWords = new List<string>();
                foreach (var word in words)
                {
                    foreach (var sep in _opt.Separators)
                    {
                        specialWords.Add($"{word}{sep}");
                        specialWords.Add($"{sep}{word}");
                    }
                }
                words.AddRange(specialWords);
                words = words.DistinctTrimmed().ToList();
            }

            // Optionally apply Leet transformations
            if (_opt.UseLeet)
            {
                var leetWords = words.Select(ToLeet).ToList();
                words.AddRange(leetWords);
                words = words.DistinctTrimmed().ToList();
            }

            // Limit max count
            return words.Take(_opt.MaxCount).ToList();
        }

        // Collect base words from user info
        private List<string> CollectBaseWords(UserInfo info)
        {
            var list = new List<string>();

            if (!string.IsNullOrWhiteSpace(info.FirstName))
                list.Add(info.FirstName);

            if (!string.IsNullOrWhiteSpace(info.LastName))
                list.Add(info.LastName);

            if (!string.IsNullOrWhiteSpace(info.FirstName) && !string.IsNullOrWhiteSpace(info.LastName))
                list.Add($"{info.FirstName}{info.LastName}");

            return list;
        }

        // Generate numeric tokens related to user info
        private IEnumerable<string> GenerateNumericTokens(UserInfo info)
        {
            var digits = new List<string>();

            if (info.DateOfBirth != null)
            {
                digits.Add(info.DateOfBirth.Value.Year.ToString());
                digits.Add(info.DateOfBirth.Value.ToString("MMdd"));
            }

            if (!string.IsNullOrEmpty(info.Phone))
            {
                var phone = new string(info.Phone.Where(char.IsDigit).ToArray());
                if (phone.Length >= 4)
                    digits.Add(phone[^4..]); // last 4 digits
            }

            // common numeric patterns
            digits.Add("123");
            digits.Add("321");

            return digits.Distinct();
        }

        // Apply simple Leet substitutions
        private string ToLeet(string input)
        {
            var sb = new StringBuilder(input);
            sb.Replace('a', '4');
            sb.Replace('A', '4');
            sb.Replace('e', '3');
            sb.Replace('E', '3');
            sb.Replace('i', '1');
            sb.Replace('I', '1');
            sb.Replace('o', '0');
            sb.Replace('O', '0');
            sb.Replace('s', '5');
            sb.Replace('S', '5');
            return sb.ToString();
        }
    }

    // Extension method to trim duplicates safely
    public static class ListExtensions
    {
        public static List<string> DistinctTrimmed(this IEnumerable<string> source)
        {
            return source
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .ToList();
        }
    }
}
