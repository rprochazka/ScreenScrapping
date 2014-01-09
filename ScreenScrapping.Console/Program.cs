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

            var scrappingDefinition = scrappingDefinitions.First(d => d.Name == "BelkEN");

            RunScrapping(scrappingDefinition, 15);
            
            System.Console.ReadLine();
        }

        private static void RunScrapping(Definition definition, int linkLimit)
        {
            var engine = new EngineRunner();
            var fieldsdefinition = definition.DetailFields.Select(f => new { Key = f.Name, Value = f.XPath })
                            .ToDictionary(i => i.Key, i => i.Value);
            var scrappedFields = 
                    engine.GetScrappedFields(
                        definition.BaseUrl, definition.JobDetailLinkUrlXPath,
                        fieldsdefinition, definition.NextPageUrlXPath, linkLimit);

            DisplayInConsole(scrappedFields);
        }

        static void DisplayInConsole(IEnumerable<KeyValuePair<string, string>> result)
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
            //System.Console.ReadLine();
        }

        static void DisplayInConsole(IEnumerable<IEnumerable<KeyValuePair<string, string>>> result)
        {
            System.Console.WriteLine("Results:");
            System.Console.WriteLine();
            foreach (var detail in result.ToList())
            {
                foreach (var item in detail)
                {
                    System.Console.WriteLine(string.Concat(item.Key, ": ", item.Value));   
                }
                System.Console.WriteLine();
            }            
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

            //System.Console.ReadLine();
        }
    }
}
