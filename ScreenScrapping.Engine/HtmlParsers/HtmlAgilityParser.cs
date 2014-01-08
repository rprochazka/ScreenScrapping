using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ScreenScrapping.Engine.Helpers;
using ScreenScrapping.Engine.Dtos;

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
        /// returns list of html nodes from the xpath evaluation
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public IEnumerable<ScrappedHtmlNode> EvaluateXPath(string xpath)
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
                    ? matchedNodes.Select(n => new ScrappedHtmlNode { NodeText = n.InnerText }) 
                    : new List<ScrappedHtmlNode>();
        }        

        /// <summary>
        /// evaluate xpath for attribute selection
        /// this is a workaround for the attribute selection in xpath not supported by HtmlAgilityPack
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        private IEnumerable<ScrappedHtmlNode> EvaluateXPath(string xpath, string attributeName)
        {            
            var matchedNodes = _doc.DocumentNode.SelectNodes(xpath);
            if (null != matchedNodes)
            {
                
                    var selectedValues =
                        from matchedNode in matchedNodes
                        let attValue = matchedNode.GetAttributeValue(attributeName, null)
                        where !string.IsNullOrEmpty(attValue)
                        select new ScrappedHtmlNode { NodeText = matchedNode.InnerText, Attributes = new Dictionary<string, string> {{attributeName, attValue}} };

                    return selectedValues;
                
            }

            return new List<ScrappedHtmlNode>();

        }

        private void InitializeParser(string html)
        {            
            _doc = new HtmlDocument();

            //workaround for "different" handling of form element using HtmlAgilityPack
            HtmlNode.ElementsFlags.Remove("form");

            _doc.LoadHtml(html);            
        }

        /// <summary>
        /// determines if the xpath defines attribute selection (xpath ending on \@[AttributeName])
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
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
