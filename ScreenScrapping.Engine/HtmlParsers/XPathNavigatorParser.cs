using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;

namespace ScreenScrapping.Engine.HtmlParsers
{
    internal sealed class XPathNavigatorParser : IHtmlParser
    {
        private XPathNavigator _navigator;

        public XPathNavigatorParser(string htmlContent)
        {
            InitNavigator(htmlContent);
        }

        public IEnumerable<string> EvaluateXPath(string xpath)
        {                        
            //var nsManager = new XmlNamespaceManager(_navigator.NameTable);            
            //nsManager.AddNamespace("default", "http://www.w3.org/1999/xhtml");
            var nodes = _navigator.Select(xpath);
            while (nodes.MoveNext())
            {
                yield return nodes.Current.Value;
            }
        }

        private void InitNavigator(string htmlContent)
        {
            var document = new XPathDocument(new MemoryStream(Encoding.UTF8.GetBytes(htmlContent)));            
            _navigator = document.CreateNavigator();
        }
    }
}