using System.Text.RegularExpressions;

namespace ScreenScrapping.Engine.Helpers
{
    public static class Extensions
    {
        public static string ToLower(this string inputValue, string regexPattern)
        {
            return Regex.Replace(inputValue, regexPattern, m => m.Value.ToLower());
        }
    }
}
