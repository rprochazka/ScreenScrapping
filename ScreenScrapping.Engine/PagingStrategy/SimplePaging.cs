using ScreenScrapping.Engine.HtmlParsers;

namespace ScreenScrapping.Engine.PagingStrategy
{
    class SimplePaging : IPagingStrategy
    {
        public SimplePaging(string pageUrl)
        {
            CurrentPageUrl = pageUrl;
        }

        public bool MoveNext(IHtmlParser parser)
        {
            return false;
        }

        public string CurrentPageUrl { get; private set; }
    }
}