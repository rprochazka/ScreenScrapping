using System.Collections.Generic;
using System.Linq;
using ScreenScrapping.Console.ScrappingDefinition;
using ScreenScrapping.Engine.Dtos;

namespace ScreenScrapping.Console
{
    class Program
    {
        static void Main(string[] args)
        {                                    
            var scrappingDefinitions =
                ((ScrappingDefinitionSection)System.Configuration.ConfigurationManager
                    .GetSection("ScrappingDefinition")).Definitions;

            var scrappingDefinition = scrappingDefinitions.First(d=>d.Name == "BelkEN");

            var detailLinks = GetDetailLinks(scrappingDefinition.BaseUrl, scrappingDefinition.JobDetailLinkUrlXPath, scrappingDefinition.NextPageUrlXPath).ToList();
            DisplayInConsole(detailLinks);

            foreach (var detailLink in detailLinks)
            {
                var scrappedFields = GetScrappedFields(detailLink.LinkUrl,
                    scrappingDefinition.DetailFields
                        .Select(f => new {Key = f.Name, Value = f.XPath})
                        .ToDictionary(i => i.Key, i => i.Value));
                DisplayInConsole(detailLink.LinkUrl, scrappedFields);
            }            
        }

        static IEnumerable<UrlLinkInfo> GetDetailLinks(string initialUrl, string detailLinkUrlXPath, string nextPageUrlXPath)
        {
            var scrappingEngine = new Engine.EngineManager();
            return scrappingEngine.GetDetailLinkUrls(initialUrl, detailLinkUrlXPath, nextPageUrlXPath);
        }

        static IEnumerable<KeyValuePair<string, string>> GetScrappedFields(string jobDetailUrl, IDictionary<string, string> jobDetailFieldsXPath)
        {
            var scrappingEngine = new Engine.EngineManager();
            return scrappingEngine.GetScrappedFields(jobDetailUrl, jobDetailFieldsXPath);
        }

        static void DisplayInConsole(Dictionary<string, string> result)
        {
            System.Console.WriteLine("Results:");
            foreach (var item in result)
            {
                System.Console.WriteLine(string.Concat(item.Key, ": ", item.Value));
            }

            System.Console.ReadLine();
        }

        static void DisplayInConsole(string url, IEnumerable<KeyValuePair<string, string>> result)
        {
            System.Console.WriteLine("Results:");
            System.Console.WriteLine(url);
            foreach (var item in result)
            {                
                System.Console.WriteLine(string.Concat(item.Key, ": ", item.Value));                
            }
            System.Console.WriteLine();
            System.Console.ReadLine();
        }

        static void DisplayInConsole(IEnumerable<UrlLinkInfo> result)
        {
            System.Console.WriteLine("Results:");
            foreach (var item in result)
            {
                System.Console.WriteLine();
                System.Console.WriteLine(item.LinkTitle);
                System.Console.WriteLine(item.LinkUrl);
                System.Console.WriteLine();
            }

            System.Console.ReadLine();
        }
    }
}
