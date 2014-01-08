using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ScreenScrapping.Engine.Dtos;

namespace ScreenScrapping.Engine.Helpers
{
    internal static class Extensions
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

        public static IEnumerable<UrlLinkInfo> ToUrlLinkInfos(this IEnumerable<ScrappedHtmlNode> scrappedHtmlNodes)
        {
            return
                scrappedHtmlNodes.Select(
                    n => new UrlLinkInfo {LinkTitle = n.NodeText, LinkUrl = n.ExtractHrefAttributeValue()});
        }

        public static string ExtractHrefAttributeValue(this ScrappedHtmlNode htmlNode)
        {
            if (null == htmlNode)
            {
                return null;
            }

            return htmlNode.Attributes.Where(a => a.Key == "href").Select(a => a.Value).FirstOrDefault();
        }
    }
}
