using System.Collections.Generic;
using ScreenScrapping.Engine.Dtos;

namespace ScreenScrapping.Engine.HtmlParsers
{
    internal interface IHtmlParser
    {
        /// <summary>
        /// returns list of html nodes from the xpath evaluation
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        IEnumerable<ScrappedHtmlNode> EvaluateXPath(string xpath);
    }
}