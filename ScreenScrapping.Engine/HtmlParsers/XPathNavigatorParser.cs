using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;
using ScreenScrapping.Engine.Dtos;

namespace ScreenScrapping.Engine.HtmlParsers
{
    internal sealed class XPathNavigatorParser : IHtmlParser
    {
        private XPathNavigator _navigator;

        public XPathNavigatorParser(string htmlContent)
        {
            InitNavigator(htmlContent);
        }

        public IEnumerable<ScrappedHtmlNode> EvaluateXPath(string xpath)
        {                        
            //var nsManager = new XmlNamespaceManager(_navigator.NameTable);            
            //nsManager.AddNamespace("default", "http://www.w3.org/1999/xhtml");
            var nodes = _navigator.Select(xpath);
            while (nodes.MoveNext())
            {
                yield return new ScrappedHtmlNode {NodeText = nodes.Current.Value};
            }
        }

        private void InitNavigator(string htmlContent)
        {
            var document = new XPathDocument(new MemoryStream(Encoding.UTF8.GetBytes(htmlContent)));            
            _navigator = document.CreateNavigator();
        }
    }
}