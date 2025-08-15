using PassForge.Core.Models;
using PassForge.Console.Services;

namespace PassForge.Console.Menus
{
    public static class Main
    {
        public static void Show()
        {
            var options = new PasswordOptions();
            bool exit = false;

            while (!exit)
            {
                System.Console.Clear();
                ConsoleUI.WriteTitle("PassForge - Password List Generator (Console)");
                ConsoleUI.WriteOption(1, "تولید پسورد جدید");
                ConsoleUI.WriteOption(2, "تنظیمات پیشرفته");
                ConsoleUI.WriteOption(3, "راهنمای حالت سریع (CLI)");
                ConsoleUI.WriteOption(0, "خروج");
                ConsoleUI.Separator();

                var choice = UserInput.PromptInt("انتخاب", 0, 3);

                switch (choice)
                {
                    case 0: exit = true; break;
                    case 1: Generate.Show(options); break;
                    case 2: Options.Show(options); break;
                    case 3:
                        ConsoleUI.WriteTitle("راهنمای اجرای سریع");
                        ConsoleUI.WriteHint("""
نمونه:
  PassForge.Console --first=Ali --last=Rezayi --email=ali@x.com --phone=0912... --dob=2000-03-12 --extra=tehran,milan \
--min=6 --max=14 --seps="._-" --suffixes="!,123,@2025" --lower=on --upper=on --digits=on --special=on --specialset="!@#._-" \
--leet=on --reversed=on --case=on --each=off --numbers=on --tokens=2 --maxcount=2000 --save=out.txt --preview=30
""");
                        System.Console.WriteLine("برای بازگشت کلیدی بزنید...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }
    }
}
