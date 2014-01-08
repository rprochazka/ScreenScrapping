using System.Linq;
using ScreenScrapping.Engine.Dtos;
using ScreenScrapping.Engine.Helpers;
using ScreenScrapping.Engine.HtmlParsers;

namespace ScreenScrapping.Engine.PagingStrategy
{
    class MultiPaging : IPagingStrategy
    {
        private readonly string _nextPageXPath;        
        private readonly UniqueUrlCollection _visitedUniqueUrls;
        private readonly int _maxEntries;

        public MultiPaging(string initialUrl, string nextPageXPath, int maxEntries = -1)
        {
            _visitedUniqueUrls = new UniqueUrlCollection();

            _nextPageXPath = nextPageXPath;            
            CurrentPageUrl = initialUrl;
            _visitedUniqueUrls.Add(initialUrl);

            _maxEntries = maxEntries;
        }

        public bool MoveNext(IHtmlParser parser)
        {
            var nextPageNode = parser.EvaluateXPath(_nextPageXPath).FirstOrDefault();
            var nextPageUrl = ExtractHrefAttributeValue(nextPageNode);
            if (CanMoveNext(nextPageUrl))            
            {
                _visitedUniqueUrls.Add(nextPageUrl);
                CurrentPageUrl = nextPageUrl;
                return true;
            }
            return false;
        }

        public string CurrentPageUrl { get; private set; }

        private bool CanMoveNext(string nextPageUrl)
        {
            return (
                !string.IsNullOrEmpty(nextPageUrl)
                && !_visitedUniqueUrls.Contains(nextPageUrl)
                && CheckEntriesTreshold());
        }

        private bool CheckEntriesTreshold()
        {
            if (_maxEntries < 0)
            {
                return true;
            }

            return _visitedUniqueUrls.Count() < _maxEntries;
        }

        private string ExtractHrefAttributeValue(ScrappedHtmlNode htmlNode)
        {
            if (null == htmlNode)
            {
                return null;
            }

            return htmlNode.ExtractHrefAttributeValue();
        }
    }
}