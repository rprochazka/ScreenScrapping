using System.Collections.Generic;
using System.Linq;
using ScreenScrapping.Engine.Dtos;
using ScreenScrapping.Engine.Helpers;
using ScreenScrapping.Engine.HtmlParsers;
using ScreenScrapping.Engine.PagingStrategy;

namespace ScreenScrapping.Engine
{
    public class EngineManager : IEngineManager
    {
        #region IEngineManager implementation
        /// <summary>
        /// get list of urls on a given (first) page (e.g. links to detail page from the master page) and the following ones
        /// in a paging scenarios
        /// </summary>
        /// <param name="initialUrl">the first page url</param>
        /// <param name="detailLinkUrlXPath">xpath of the detail link url value</param>
        /// <param name="nextPageLinkUrlXPath">xpath of the 'next' link url value</param>        
        /// <param name="maxLinks"></param>
        /// <returns></returns>
        public IEnumerable<UrlLinkInfo> GetDetailLinkUrls(string initialUrl, string detailLinkUrlXPath, string nextPageLinkUrlXPath = null, int maxLinks = -1)
        {
            var pagingStrategy = (string.IsNullOrEmpty(nextPageLinkUrlXPath))
                ? (IPagingStrategy) new SimplePaging(initialUrl)
                : new MultiPaging(initialUrl, nextPageLinkUrlXPath);

            return GetDetailLinkUrls(detailLinkUrlXPath, pagingStrategy, maxLinks);
        }

        /// <summary>
        /// performs screen scrapping of the given page based on the xpath fields definition
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fieldsXPathDefinitions"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetScrappedFields(string url, IDictionary<string, string> fieldsXPathDefinitions)
        {
            var htmlContent = GetHtmlContent(url);
            IHtmlParser parser;
            InitParser(out parser, htmlContent);

            var scrappedFields = ScrapeFields(fieldsXPathDefinitions, parser);

            return
                scrappedFields
                    .Select(f => new {f.Key, Value = string.Join(" ", f.Value.Select(n=>n.NodeText))})
                    .ToDictionary(f => f.Key, f => f.Value);
        }
        #endregion

        private IEnumerable<UrlLinkInfo> GetDetailLinkUrls(string detailLinkUrlXPath, IPagingStrategy paging, int maxEntries)
        {
            var result = new UniqueAndItemsLimitAwareUrlInfoCollection(maxEntries);

            var detailLinkDefinition = new Dictionary<string, string> { { "detailLink", detailLinkUrlXPath } };

            IHtmlParser parser;            
            do
            {
                var urlToProcess = paging.CurrentPageUrl;

                var htmlContent = GetHtmlContent(urlToProcess);
                InitParser(out parser, htmlContent);                

                var scrappedLinks =
                    ScrapeFields(detailLinkDefinition, parser)
                        .SelectMany(f => f.Value)
                        .ToUrlLinkInfos();

                result.AddRange(scrappedLinks);

            } while (paging.MoveNext(parser) && !result.MaxLimitReached);


            return result;
        }        

        private void InitParser(out IHtmlParser parser, string htmlContent)
        {
            parser = new HtmlAgilityParser(htmlContent);
        }

        private string GetHtmlContent(string url)
        {
            var downloader = new WebDownloader();
            return downloader.GetUrlContent(url);
        }

        /// <summary>
        /// performs scrapping of a html content passsed in IHtmlParser based on the xpath definition
        /// pased in xpath definition
        /// </summary>
        /// <param name="xpathDefinitions">key-value pairs of field name and xpath location</param>
        /// <param name="parser">html parser holding the html to parse</param>
        /// <returns>key-value pairs of field name and nodes </returns>
        private IEnumerable<KeyValuePair<string, IEnumerable<ScrappedHtmlNode>>> ScrapeFields(IEnumerable<KeyValuePair<string, string>> xpathDefinitions, IHtmlParser parser)
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