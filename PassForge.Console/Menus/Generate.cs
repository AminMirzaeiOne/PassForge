using PassForge.Core.Models;
using PassForge.Core.Services;
using PassForge.Console.Services;
using PassForge.Console.Views;
using System;
using System.Linq;

namespace PassForge.Console.Menus
{
    public static class Generate
    {
        public static void Show(PasswordOptions options)
        {
            ConsoleUI.WriteTitle("تولید پسورد");

            var user = new UserInfo
            {
                FirstName = UserInput.Prompt("نام"),
                LastName = UserInput.Prompt("نام خانوادگی"),
                Nickname = UserInput.Prompt("نام مستعار", true),
                Email = UserInput.Prompt("ایمیل", true, "مثال: a@b.com"),
                Phone = UserInput.Prompt("شماره موبایل", true)
            };

            if (UserInput.PromptYesNo("تاریخ تولد را وارد می‌کنید؟", false))
            {
                var y = UserInput.PromptInt("سال تولد", 1900, DateTime.Now.Year);
                var m = UserInput.PromptInt("ماه تولد", 1, 12);
                var d = UserInput.PromptInt("روز تولد", 1, 31);
                user.DateOfBirth = new DateTime(y, m, d);
            }
            else
            {
                user.AgeYears = UserInput.PromptInt("سن (سال)", 1, 120, 25);
            }

            var extra = UserInput.Prompt("کلمات اضافی (با کاما جدا کنید)", true);
            if (!string.IsNullOrWhiteSpace(extra))
                user.ExtraWords = extra.Split(',').Select(x => x.Trim()).Where(x => x.Length > 0).ToList();

            // تولید
            var gen = new PasswordGenerator(options);
            var list = gen.Generate(user).ToList();

            // انتخاب پیش‌نمایش
            var previewCount = UserInput.PromptInt("تعداد موارد پیش‌نمایش", 0, 200, 20);
            if (previewCount > 0) PasswordPreview.Show(list, previewCount);

            // ذخیره
            if (UserInput.PromptYesNo("ذخیره در فایل؟", true))
            {
                var path = UserInput.Prompt("مسیر فایل خروجی", false, "مثلاً output.txt");
                FileExport.ExportToFile(list, path);
            }
        }
    }
}
