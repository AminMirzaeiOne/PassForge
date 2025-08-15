
namespace PassForge.Console.Views
{
    public static class PasswordPreview
    {
        public static void Show(IEnumerable<string> passwords, int maxPreview = 20)
        {
            System.Console.WriteLine("📄 پیش‌نمایش پسوردها:");
            int count = 0;
            var list = passwords as IList<string> ?? passwords.ToList();
            foreach (var p in list.Take(maxPreview))
                System.Console.WriteLine($"  {++count}. {p}");

            if (list.Count > maxPreview)
                System.Console.WriteLine($"... و {list.Count - maxPreview} پسورد دیگر");
        }
    }
}
