using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ScreenScrapping.Engine.HtmlParsers
{
    /// <summary>
    /// html parser which makes use of the HtmlAgilityPack
    /// </summary>
    internal sealed class HtmlAgilityParser : IHtmlParser
    {
        private const string AttributeSelectionRegexPattern = "/@(?<att>[a-zA-Z0-9]+$)";

        private readonly HtmlDocument _doc; 
        public HtmlAgilityParser(string html)
        {
            _doc = new HtmlDocument();
            _doc.LoadHtml(html);
        }

        public IEnumerable<string> EvaluateXPath(string xpath)
        {
            string attributeName;
            if (IsAttributeSelection(xpath, out attributeName))
            {
                return EvaluateXPath(xpath, attributeName);
            }
            return _doc.DocumentNode.SelectNodes(xpath).Select(n => n.InnerText);
        }

        private IEnumerable<string> EvaluateXPath(string xpath, string attributeName)
        {
            return
                _doc.DocumentNode
                        .SelectNodes(xpath)
                        .Select(n => n.GetAttributeValue(attributeName, null))
                        .Where(n => !string.IsNullOrEmpty(n));

        }

        private bool IsAttributeSelection(string xpath, out string attributeName)
        {
            attributeName = null;

            var regex = new Regex(AttributeSelectionRegexPattern, RegexOptions.Compiled);
            var match = regex.Match(xpath);
            if (match.Success)
            {
                attributeName = match.Groups["att"].Value;                
            }

            return !string.IsNullOrEmpty(attributeName);
        }
    }
}
