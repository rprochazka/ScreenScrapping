using System.Collections.Generic;

namespace ScreenScrapping.Engine.HtmlParsers
{
    internal interface IHtmlParser
    {
        /// <summary>
        /// returns list of text nodes from the xpath evaluation
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        IEnumerable<string> EvaluateXPath(string xpath);
    }
}