using PassForge.Core.Models;
using PassForge.Core.Services;
using System.Globalization;

namespace PassForge.Console.Services
{
    public static class Args
    {
        public static bool TryHandleFastMode(string[] args, out int exitCode)
        {
            exitCode = 0;
            try
            {
                var dict = ParseArgs(args);

                if (dict.Count == 0) return false; // بدون آرگومان → برو به منو

                var user = new UserInfo
                {
                    FirstName = Get(dict, "first"),
                    LastName = Get(dict, "last"),
                    Nickname = Get(dict, "nick"),
                    Email = Get(dict, "email"),
                    Phone = Get(dict, "phone"),
                    ExtraWords = Get(dict, "extra")?.Split(',').Select(s => s.Trim()).Where(s => s.Length > 0).ToList() ?? new()
                };

                if (TryParseDate(Get(dict, "dob"), out var dob))
                {
                    user.DateOfBirth = dob;
                }
                else if (TryParseInt(Get(dict, "age"), out var age))
                {
                    user.AgeYears = age;
                }

                var options = new PasswordOptions
                {
                    MinLength = GetInt(dict, "min", 8),
                    MaxLength = GetInt(dict, "max", 16),
                    IncludeLowercase = GetBool(dict, "lower", true),
                    IncludeUppercase = GetBool(dict, "upper", true),
                    IncludeDigits = GetBool(dict, "digits", true),
                    IncludeSpecial = GetBool(dict, "special", true),
                    SpecialCharacters = Get(dict, "specialset") ?? "!@#$%^&*._-",
                    UseLeet = GetBool(dict, "leet", true),
                    IncludeReversed = GetBool(dict, "reversed", true),
                    IncludeWordCaseVariants = GetBool(dict, "case", true),
                    RequireAtLeastOneFromEachSelectedCategory = GetBool(dict, "each", false),
                    Separators = Get(dict, "seps") ?? "._-",
                    MaxTokensPerPassword = GetInt(dict, "tokens", 2),
                    AppendCommonNumbers = GetBool(dict, "numbers", true),
                    MaxCount = GetInt(dict, "maxcount", 5000),
                    CustomSuffixes = (Get(dict, "suffixes") ?? "!,123").Split(',').Select(s => s.Trim())
                };

                var gen = new PasswordGenerator(options);
                var list = gen.Generate(user).ToList();

                var noPreview = GetBool(dict, "nopreview", false);
                var savePath = Get(dict, "save");

                if (!noPreview)
                {
                    PassForge.Console.Views.PasswordPreview.Show(list, GetInt(dict, "preview", 20));
                }

                if (!string.IsNullOrWhiteSpace(savePath))
                {
                    PassForge.Console.Services.FileExport.ExportToFile(list, savePath!);
                }

                // اگر نه پیش‌نمایش و نه ذخیره انتخاب شده بود، حداقل چند مورد چاپ کن
                if (noPreview && string.IsNullOrWhiteSpace(savePath))
                {
                    foreach (var p in list.Take(20))
                        System.Console.WriteLine(p);
                }

                return true;
            }
            catch (Exception ex)
            {
                PassForge.Console.Services.ConsoleUI.WriteError(ex.Message);
                exitCode = 1;
                return true; // مصرف args انجام شد
            }
        }

        // --- Helpers ---
        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var a in args)
            {
                var t = a.Trim();
                if (!t.StartsWith("--")) continue;
                t = t[2..];
                var idx = t.IndexOf('=');
                if (idx < 0) d[t] = "true";
                else d[t[..idx]] = t[(idx + 1)..].Trim().Trim('"');
            }
            return d;
        }

        private static string? Get(Dictionary<string, string> d, string key) =>
            d.TryGetValue(key, out var v) ? v : null;

        private static bool GetBool(Dictionary<string, string> d, string key, bool def)
        {
            var v = Get(d, key);
            if (v is null) return def;
            return v.Equals("on", StringComparison.OrdinalIgnoreCase) ||
                   v.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                   v.Equals("1");
        }

        private static int GetInt(Dictionary<string, string> d, string key, int def)
            => int.TryParse(Get(d, key), out var n) ? n : def;

        private static bool TryParseDate(string? s, out DateTime date)
            => DateTime.TryParseExact(s ?? "", "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out date);

        private static bool TryParseInt(string? s, out int n)
            => int.TryParse(s, out n);
    }
}
