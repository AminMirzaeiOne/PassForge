using PassForge.Core.Models;
using PassForge.Core.Services;
using PassForge.Console.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PassForge.Console.Services
{
    /// <summary>
    /// Fast CLI mode:
    /// Example:
    ///   PassForge.Console --first=Ali --last=Rezaei --email=ali@x.com --phone=0912...
    ///   --dob=2000-03-12 --extra=tehran,milan --min=6 --max=14 --seps="._-"
    ///   --suffixes="!,123,@2025" --lower=on --upper=on --digits=on --special=on
    ///   --specialset="!@#._-" --leet=on --reversed=on --case=on --each=off
    ///   --numbers=on --tokens=2 --maxcount=2000 --save=out.txt --preview=30 --nopreview
    /// </summary>
    public static class Args
    {
        public static bool TryHandleFastMode(string[] args, out int exitCode)
        {
            exitCode = 0;
            try
            {
                var dict = ParseArgs(args);
                if (dict.Count == 0) return false; // no args → fall back to menus

                var user = new UserInfo
                {
                    FirstName = Get(dict, "first"),
                    LastName = Get(dict, "last"),
                    Nickname = Get(dict, "nick"),
                    Email = Get(dict, "email"),
                    Phone = Get(dict, "phone"),
                    ExtraWords = (Get(dict, "extra") ?? "")
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => s.Trim())
                                    .Where(s => s.Length > 0)
                                    .ToList()
                };

                if (TryParseDate(Get(dict, "dob"), out var dob))
                {
                    user.DateOfBirth = dob;
                }
                else if (TryParseInt(Get(dict, "age"), out var ageYears))
                {
                    user.AgeYears = ageYears;
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
                    CustomSuffixes = (Get(dict, "suffixes") ?? "!,123")
                                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(s => s.Trim())
                };

                var generator = new PasswordGenerator(options);
                var list = generator.Generate(user).ToList();

                var noPreview = GetBool(dict, "nopreview", false);
                var previewCount = GetInt(dict, "preview", 20);
                var savePath = Get(dict, "save");

                if (!noPreview)
                {
                    PasswordPreview.Show(list, previewCount);
                }

                if (!string.IsNullOrWhiteSpace(savePath))
                {
                    FileExport.ExportToFile(list, savePath!);
                }

                // If neither preview nor save is requested, print a few to stdout.
                if (noPreview && string.IsNullOrWhiteSpace(savePath))
                {
                    foreach (var p in list.Take(previewCount))
                        System.Console.WriteLine(p);
                }

                return true; // handled via fast mode
            }
            catch (Exception ex)
            {
                ConsoleUI.WriteError(ex.Message);
                exitCode = 1;
                return true; // we consumed args; exit with error code
            }
        }

        // ---------- Helpers ----------
        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var a in args)
            {
                if (string.IsNullOrWhiteSpace(a)) continue;
                var t = a.Trim();
                if (!t.StartsWith("--")) continue;
                t = t[2..];

                var idx = t.IndexOf('=');
                if (idx < 0) d[t] = "true";               // flag → true
                else d[t[..idx]] = t[(idx + 1)..].Trim().Trim('"');
            }
            return d;
        }

        private static string? Get(Dictionary<string, string> d, string key)
            => d.TryGetValue(key, out var v) ? v : null;

        private static bool GetBool(Dictionary<string, string> d, string key, bool def)
        {
            var v = Get(d, key);
            if (v is null) return def;
            return v.Equals("on", StringComparison.OrdinalIgnoreCase) ||
                   v.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                   v.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                   v.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                   v.Equals("y", StringComparison.OrdinalIgnoreCase);
        }

        private static int GetInt(Dictionary<string, string> d, string key, int def)
            => int.TryParse(Get(d, key), out var n) ? n : def;

        private static bool TryParseDate(string? s, out DateTime date)
            => DateTime.TryParseExact(s ?? "", "yyyy-MM-dd",
                                      CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out date);

        private static bool TryParseInt(string? s, out int n)
            => int.TryParse(s, out n);
    }
}
