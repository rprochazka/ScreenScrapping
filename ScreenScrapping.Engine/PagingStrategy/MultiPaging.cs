using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ScreenScrapping.Engine.HtmlParsers;

namespace ScreenScrapping.Engine.PagingStrategy
{
    class MultiPaging : IPagingStrategy
    {
        private readonly string _nextPageXPath;
        private readonly IList<string> _visitedUrls;

        public MultiPaging(string initialUrl, string nextPageXPath)
        {
            _visitedUrls = new List<string>();

            _nextPageXPath = nextPageXPath;            
            CurrentPageUrl = initialUrl;
            AddToVisitedUrls(initialUrl);
        }

        public bool MoveNext(IHtmlParser parser)
        {
            var nextPageUrl = parser.EvaluateXPath(_nextPageXPath).FirstOrDefault();
            if (!string.IsNullOrEmpty(nextPageUrl) && !_visitedUrls.Contains(nextPageUrl))
            {
                AddToVisitedUrls(nextPageUrl);
                CurrentPageUrl = nextPageUrl;
                return true;
            }
            return false;
        }

        public string CurrentPageUrl { get; private set; }

        private void AddToVisitedUrls(string url)
        {
            _visitedUrls.Add(HttpUtility.UrlDecode(url));
        }
    }
}