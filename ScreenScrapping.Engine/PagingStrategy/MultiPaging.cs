using System.Linq;
using ScreenScrapping.Engine.HtmlParsers;

namespace ScreenScrapping.Engine.PagingStrategy
{
    class MultiPaging : IPagingStrategy
    {
        private readonly string _nextPageXPath;        
        private readonly UniqueUrlCollection _visitedUniqueUrls;

        public MultiPaging(string initialUrl, string nextPageXPath)
        {
            _visitedUniqueUrls = new UniqueUrlCollection();

            _nextPageXPath = nextPageXPath;            
            CurrentPageUrl = initialUrl;
            _visitedUniqueUrls.Add(initialUrl);
        }

        public bool MoveNext(IHtmlParser parser)
        {
            var nextPageUrl = parser.EvaluateXPath(_nextPageXPath).FirstOrDefault();
            if (!string.IsNullOrEmpty(nextPageUrl) && !_visitedUniqueUrls.Contains(nextPageUrl))
            {
                _visitedUniqueUrls.Add(nextPageUrl);
                CurrentPageUrl = nextPageUrl;
                return true;
            }
            return false;
        }

        public string CurrentPageUrl { get; private set; }                
    }
}