using ScreenScrapping.Engine.HtmlParsers;

namespace ScreenScrapping.Engine.PagingStrategy
{
    /// <summary>
    /// determines the next page in a paging scenario
    /// </summary>
    interface IPagingStrategy
    {
        /// <summary>
        /// determines if there is another page in a paging scenario
        /// </summary>
        /// <param name="parser"> </param>
        /// <returns></returns>
        bool MoveNext(IHtmlParser parser);

        /// <summary>
        /// get the current page url
        /// </summary>
        string CurrentPageUrl { get; }
    }
}
