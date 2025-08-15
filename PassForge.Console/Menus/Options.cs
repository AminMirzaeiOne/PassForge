using PassForge.Core.Models;
using PassForge.Console.Services;

namespace PassForge.Console.Menus
{
    public static class Options
    {
        public static void Show(PasswordOptions options)
        {
            bool exit = false;
            while (!exit)
            {
                ConsoleUI.WriteTitle("تنظیمات تولید پسورد");
                System.Console.WriteLine($"1. حداقل طول: {options.MinLength}");
                System.Console.WriteLine($"2. حداکثر طول: {options.MaxLength}");
                System.Console.WriteLine($"3. شامل حروف کوچک: {options.IncludeLowercase}");
                System.Console.WriteLine($"4. شامل حروف بزرگ: {options.IncludeUppercase}");
                System.Console.WriteLine($"5. شامل اعداد: {options.IncludeDigits}");
                System.Console.WriteLine($"6. شامل کاراکتر خاص: {options.IncludeSpecial}");
                System.Console.WriteLine($"7. مجموعه کاراکترهای خاص: \"{options.SpecialCharacters}\"");
                System.Console.WriteLine($"8. جداکننده‌ها: \"{options.Separators}\"");
                System.Console.WriteLine($"9. l33t فعال: {options.UseLeet}");
                System.Console.WriteLine($"10. تولید معکوس (Reverse): {options.IncludeReversed}");
                System.Console.WriteLine($"11. حالت‌های حروف (Case Variants): {options.IncludeWordCaseVariants}");
                System.Console.WriteLine($"12. الزام حداقل یک کاراکتر از هر دسته: {options.RequireAtLeastOneFromEachSelectedCategory}");
                System.Console.WriteLine($"13. افزودن الگوهای عددی متداول: {options.AppendCommonNumbers}");
                System.Console.WriteLine($"14. حداکثر تعداد توکن در هر پسورد: {options.MaxTokensPerPassword}");
                System.Console.WriteLine($"15. تعداد خروجی (MaxCount): {options.MaxCount}");
                System.Console.WriteLine($"16. پسوندهای سفارشی: {string.Join(",", options.CustomSuffixes)}");
                System.Console.WriteLine("0. بازگشت");
                ConsoleUI.WriteHint("توجه: جداکننده‌ها رشته‌ای از کاراکترها هستند، مثلاً \"._-\".");

                var choice = UserInput.PromptInt("انتخاب گزینه", 0, 16);

                switch (choice)
                {
                    case 0: exit = true; break;
                    case 1: options.MinLength = UserInput.PromptInt("حداقل طول", 1, 100, options.MinLength); break;
                    case 2: options.MaxLength = UserInput.PromptInt("حداکثر طول", options.MinLength, 200, options.MaxLength); break;
                    case 3: options.IncludeLowercase = UserInput.PromptYesNo("شامل حروف کوچک؟", options.IncludeLowercase); break;
                    case 4: options.IncludeUppercase = UserInput.PromptYesNo("شامل حروف بزرگ؟", options.IncludeUppercase); break;
                    case 5: options.IncludeDigits = UserInput.PromptYesNo("شامل اعداد؟", options.IncludeDigits); break;
                    case 6: options.IncludeSpecial = UserInput.PromptYesNo("شامل کاراکترهای خاص؟", options.IncludeSpecial); break;
                    case 7: options.SpecialCharacters = UserInput.PromptSet("مجموعه کاراکترهای خاص", options.SpecialCharacters); break;
                    case 8: options.Separators = UserInput.PromptSet("جداکننده‌ها", options.Separators); break;
                    case 9: options.UseLeet = UserInput.PromptYesNo("فعال‌سازی l33t؟", options.UseLeet); break;
                    case 10: options.IncludeReversed = UserInput.PromptYesNo("افزودن نسخه‌های معکوس؟", options.IncludeReversed); break;
                    case 11: options.IncludeWordCaseVariants = UserInput.PromptYesNo("تولید حالت‌های حروف؟", options.IncludeWordCaseVariants); break;
                    case 12: options.RequireAtLeastOneFromEachSelectedCategory = UserInput.PromptYesNo("الزام حداقل یکی از هر دسته؟", options.RequireAtLeastOneFromEachSelectedCategory); break;
                    case 13: options.AppendCommonNumbers = UserInput.PromptYesNo("افزودن الگوهای عددی متداول؟", options.AppendCommonNumbers); break;
                    case 14: options.MaxTokensPerPassword = UserInput.PromptInt("حداکثر تعداد توکن", 1, 5, options.MaxTokensPerPassword); break;
                    case 15: options.MaxCount = UserInput.PromptInt("حداکثر تعداد خروجی", 1, 100000, options.MaxCount); break;
                    case 16:
                        var arr = UserInput.PromptList("پسوندهای سفارشی", options.CustomSuffixes as string[] ?? new[] { "!", "123" });
                        options.CustomSuffixes = arr;
                        break;
                }
            }
        }
    }
}
