using System.Collections.Generic;

namespace ScreenScrapping.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            const string jobListUrl = "http://www.jobs.cz/search/?section=positions&srch%5Bq%5D=.net&srch%5Blocality%5D%5Bname%5D=Praha&srch%5Blocality%5D%5Bcoords%5D=&srch%5Blocality%5D%5Bcode%5D=R200000&srch%5BminimalSalary%5D=&srch%5BcompanyField%5D=&srch%5BemploymentType%5D=&srch%5BworkStatus%5D=&srch%5Bcontract%5D=&srch%5Bozp%5D=";
            const string jobDetailUrl = "http://www.jobs.cz/pd/726894616/?rps=233&section=positions";
            const string jobDetailLinkUrlXPath = "//div[@id='joblist']//div[@class='list']//h3/a/@href";
            const string nextPageUrlXPath = "//div[@id='pager']/span[@class='next']/a/@href";

            var jobDetailFieldsXPath = new Dictionary<string, string>
                                           {
                                               {"jobTitle", "//h2[@id='g2d-name']"},
                                               {"jobDesc", "//div[@id='g2-desc']/p"}
                                           };
            
            var detailLinks = GetDetailLinks(jobListUrl, jobDetailLinkUrlXPath, nextPageUrlXPath);
            //DisplayInConsole(detailLinks);

            foreach (var detailLink in detailLinks)
            {
                var scrappedFields = GetScrappedFields(detailLink, jobDetailFieldsXPath);
                DisplayInConsole(scrappedFields);
            }
        }

        static IEnumerable<string> GetDetailLinks(string initialUrl, string detailLinkUrlXPath, string nextPageUrlXPath)
        {
            var scrappingEngine = new Engine.EngineManager();
            return scrappingEngine.GetDetailLinkUrls(initialUrl, detailLinkUrlXPath, nextPageUrlXPath);
        }

        static Dictionary<string, string> GetScrappedFields(string jobDetailUrl, Dictionary<string, string> jobDetailFieldsXPath)
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

        static void DisplayInConsole(IEnumerable<string> result)
        {
            System.Console.WriteLine("Results:");
            foreach (var item in result)
            {
                System.Console.WriteLine(item);
            }

            System.Console.ReadLine();
        }
    }
}
