using System.Text.RegularExpressions;

namespace ScreenScrapping.Engine.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// applies ToLower string method based on the passed regular expression
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="regexPattern"></param>
        /// <returns></returns>
        public static string ToLower(this string inputValue, string regexPattern)
        {
            return Regex.Replace(inputValue, regexPattern, m => m.Value.ToLower());
        }
    }
}
