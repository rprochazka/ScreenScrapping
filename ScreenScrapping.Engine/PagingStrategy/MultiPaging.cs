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

        public MultiPaging(string initialUrl, string nextPageXPath)
        {
            _visitedUniqueUrls = new UniqueUrlCollection();

            _nextPageXPath = nextPageXPath;            
            CurrentPageUrl = initialUrl;
            _visitedUniqueUrls.Add(initialUrl);
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
            return (!string.IsNullOrEmpty(nextPageUrl)
                && !_visitedUniqueUrls.Contains(nextPageUrl));
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