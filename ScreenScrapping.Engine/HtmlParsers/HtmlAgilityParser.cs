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

            var matchedNodes = _doc.DocumentNode.SelectNodes(xpath);
            
            return 
                null != matchedNodes 
                    ? matchedNodes.Select(n => n.InnerText) 
                    : new List<string>();
        }

        private IEnumerable<string> EvaluateXPath(string xpath, string attributeName)
        {
            var matchedNodes = _doc.DocumentNode.SelectNodes(xpath);
            if (null != matchedNodes)
            {
                return matchedNodes
                        .Select(n => n.GetAttributeValue(attributeName, null))
                        .Where(n => !string.IsNullOrEmpty(n));
            }

            return new List<string>();

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
