using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ScreenScrapping.Engine.Helpers;

namespace ScreenScrapping.Engine.HtmlParsers
{
    /// <summary>
    /// html parser which makes use of the HtmlAgilityPack
    /// </summary>
    internal sealed class HtmlAgilityParser : IHtmlParser
    {
        private const string AttributeSelectionRegexPattern = "/@(?<att>[a-zA-Z0-9]+$)";

        private HtmlDocument _doc; 
        public HtmlAgilityParser(string html)
        {
            InitializeParser(html);
        }

        /// <summary>
        /// returns list of text nodes from the xpath evaluation
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public IEnumerable<string> EvaluateXPath(string xpath)
        {
            var fixedXPath = FixXPathLetterCase(xpath);

            string attributeName;
            if (IsAttributeSelection(fixedXPath, out attributeName))
            {
                return EvaluateXPath(fixedXPath, attributeName);
            }

            var matchedNodes = _doc.DocumentNode.SelectNodes(fixedXPath);
            
            return 
                null != matchedNodes 
                    ? matchedNodes.Select(n => n.InnerText) 
                    : new List<string>();
        }

        /// <summary>
        /// evaluate xpath for attribute selection
        /// this is a workaround for the attribute selection in xpath not supported by HtmlAgilityPack
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
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

        private void InitializeParser(string html)
        {            
            _doc = new HtmlDocument();

            //workaround for "different" handling of form element using HtmlAgilityPack
            HtmlNode.ElementsFlags.Remove("form");

            _doc.LoadHtml(html);            
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

        /// <summary>
        /// make the element and attribute names lowercase (as the html parsing works with lower cases only)
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private string FixXPathLetterCase(string xpath)
        {
            const string elementNamePattern = "(?<=/)\\w+";
            const string attributeNamePattern = "(?<=@)\\w+";

            return xpath
                .ToLower(elementNamePattern)
                .ToLower(attributeNamePattern);
        }
    }
}
