using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ScreenScrapping.Console.Logging;
using ScreenScrapping.Engine;
using ScreenScrapping.Engine.Dtos;

namespace ScreenScrapping.Console
{
    internal class EngineRunner
    {
        private readonly IEngineManager _engineManager;
        private readonly ILogger _logger;
        public EngineRunner(IEngineManager engineManager, ILogger logger)
        {
            _engineManager = engineManager;
            _logger = logger;
        }

        public EngineRunner() : this(new EngineManager(), new ConsoleLogger())
        {}

        public IEnumerable<UrlLinkInfo> GetDetailLinks(string initialUrl, string detailLinkUrlXPath, string nextPageUrlXPath, int limit)
        {
            //_logger.Log("GetDetailLinkUrls started.");
            
            var detailLinks = _engineManager.GetDetailLinkUrls(initialUrl, detailLinkUrlXPath, nextPageUrlXPath, limit).ToList();

            _logger.Log(detailLinks);

            //_logger.Log("GetDetailLinkUrls finished.");

            return detailLinks;
        }

        public IEnumerable<KeyValuePair<string, string>> GetScrappedFields(string jobDetailUrl, IDictionary<string, string> jobDetailFieldsXPath)
        {
            //_logger.Log("GetScrappedFields started.");

            var scrappedFields = _engineManager.GetScrappedFields(jobDetailUrl, jobDetailFieldsXPath);

            _logger.Log(scrappedFields);

            //_logger.Log("GetScrappedFields finished.");

            return scrappedFields;
        }

        public IEnumerable<IEnumerable<KeyValuePair<string, string>>> GetScrappedFields(string initialUrl, string detailLinkUrlXPath, IDictionary<string, string> jobDetailFieldsXPath, string nextPageUrlXPath, int limit)
        {
            var detailLinks = GetDetailLinks(initialUrl, detailLinkUrlXPath, nextPageUrlXPath, limit).ToList();

            var result = new ConcurrentBag<IEnumerable<KeyValuePair<string, string>>>();
            System.Threading.Tasks.Parallel.ForEach(detailLinks,
                info =>
                {
                    var scrappedFields = GetScrappedFields(info.LinkUrl,jobDetailFieldsXPath);
                    result.Add(scrappedFields);                    
                });

            return result;
        }
    }
}
