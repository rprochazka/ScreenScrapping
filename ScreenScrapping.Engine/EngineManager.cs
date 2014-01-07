using System.Collections.Generic;
using System.Linq;
using ScreenScrapping.Engine.HtmlParsers;
using ScreenScrapping.Engine.PagingStrategy;

namespace ScreenScrapping.Engine
{
    public class EngineManager : IEngineManager
    {        
        public IEnumerable<string> GetDetailLinkUrls(string initialUrl, string detailLinkUrlXPath)
        {
            var pagingStrategy = new SimplePaging(initialUrl);

            return GetDetailLinkUrls(detailLinkUrlXPath, pagingStrategy);
        }

        public IEnumerable<string> GetDetailLinkUrls(string initialUrl, string detailLinkUrlXPath, string nextPageLinkUrlXPath)
        {
            var pagingStrategy = new MultiPaging(initialUrl, nextPageLinkUrlXPath);

            return GetDetailLinkUrls(detailLinkUrlXPath, pagingStrategy);
        }

        public IDictionary<string, string> GetScrappedFields(string url, IDictionary<string, string> fieldsXPathDefinitions)
        {
            var htmlContent = GetUrlContent(url);
            
            var scrappedFields = ScrapeFields(fieldsXPathDefinitions, GetHtmlParser(htmlContent));

            return
                scrappedFields
                    .Select(f => new {f.Key, Value = string.Join(" ", f.Value)})
                    .ToDictionary(f => f.Key, f => f.Value);
        }

        private IEnumerable<string> GetDetailLinkUrls(string detailLinkUrlXPath, IPagingStrategy paging)
        {
            var result = new UniqueUrlCollection();

            var detailLinkDefinition = new Dictionary<string, string> { { "detailLink", detailLinkUrlXPath } };

            IHtmlParser parser;            
            do
            {
                var urlToProcess = paging.CurrentPageUrl;

                var htmlContent = GetUrlContent(urlToProcess);

                parser = GetHtmlParser(htmlContent);

                var scrappedLinks = ScrapeFields(detailLinkDefinition, parser).SelectMany(f => f.Value);

                result.AddRange(scrappedLinks);

            } while (paging.MoveNext(parser));


            return result;
        }

        private IHtmlParser GetHtmlParser(string content)
        {
            return new HtmlAgilityParser(content);
        }

        private string GetUrlContent(string url)
        {
            var downloader = new WebDownloader();
            return downloader.GetUrlContent(url);
        }

        private IEnumerable<KeyValuePair<string, IEnumerable<string>>> ScrapeFields(IEnumerable<KeyValuePair<string, string>> xpathDefinitions, IHtmlParser parser)
        {            
            return
                xpathDefinitions
                    .Select(xpathDefinition => 
                        new {
                                xpathDefinition.Key, 
                                Value = parser.EvaluateXPath(xpathDefinition.Value)
                            })
                    .ToDictionary(i => i.Key, i => i.Value);
        }
    }
}