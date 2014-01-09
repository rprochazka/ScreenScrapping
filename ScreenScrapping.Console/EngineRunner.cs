using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ScreenScrapping.Engine;
using ScreenScrapping.Engine.Dtos;

namespace ScreenScrapping.Console
{
    internal class EngineRunner
    {
        private readonly IEngineManager _engineManager;
        public EngineRunner(IEngineManager engineManager)
        {
            _engineManager = engineManager;
        }

        public EngineRunner() : this(new EngineManager())
        {}

        public IEnumerable<UrlLinkInfo> GetDetailLinks(string initialUrl, string detailLinkUrlXPath, string nextPageUrlXPath, int limit)
        {
            return _engineManager.GetDetailLinkUrls(initialUrl, detailLinkUrlXPath, nextPageUrlXPath, limit);
        }

        public IEnumerable<KeyValuePair<string, string>> GetScrappedFields(string jobDetailUrl, IDictionary<string, string> jobDetailFieldsXPath)
        {
            return _engineManager.GetScrappedFields(jobDetailUrl, jobDetailFieldsXPath);
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
