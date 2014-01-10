using System.Linq;
using ScreenScrapping.Console.Logging;
using ScreenScrapping.Console.ScrappingDefinition;
using ScreenScrapping.Engine;

namespace ScreenScrapping.Console
{
    class Program
    {
        static void Main(string[] args)
        {                                    
            var scrappingDefinitions =
                ((ScrappingDefinitionSection)System.Configuration.ConfigurationManager
                    .GetSection("ScrappingDefinition")).Definitions;

            const int entriesLimit = 15;
            const string scrappedSiteDefinitionName = "BelkEN";

            var scrappingDefinition = scrappingDefinitions.FirstOrDefault(d => d.Name == scrappedSiteDefinitionName);

            RunScrapping(scrappingDefinition, entriesLimit);
            
            System.Console.ReadLine();
        }

        private static void RunScrapping(Definition definition, int linkLimit)
        {
            var logger = new ConsoleLogger();
            var scrappignEngine = new EngineManager();

            if (null == definition)
            {
                logger.Log("Unable to find the provided definition name.");
                return;
            }

            var engine = new EngineRunner(scrappignEngine, logger);
            var fieldsdefinition = definition.DetailFields.Select(f => new {Key = f.Name, Value = f.XPath})
                .ToDictionary(i => i.Key, i => i.Value);

            engine.GetScrappedFields(
                definition.BaseUrl, definition.JobDetailLinkUrlXPath,
                fieldsdefinition, definition.NextPageUrlXPath, linkLimit);

            logger.Log("Scrapping finished");
            
        }        
    }
}
