using System.Collections.Generic;
using System.Linq;
using ScreenScrapping.Engine.Dtos;

namespace ScreenScrapping.Console.Logging
{
    class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            System.Console.WriteLine(message);
        }

        public void Log(IEnumerable<KeyValuePair<string, string>> result)
        {
            System.Console.WriteLine("Results:");
            foreach (var item in result)
            {
                System.Console.WriteLine(string.Concat(item.Key, ": ", item.Value));
            }            
        }

        public void Log(string url, IEnumerable<KeyValuePair<string, string>> result)
        {
            System.Console.WriteLine("Results:");
            System.Console.WriteLine(url);
            foreach (var item in result)
            {
                System.Console.WriteLine(string.Concat(item.Key, ": ", item.Value));
            }
            System.Console.WriteLine();            
        }

        public void Log(IEnumerable<IEnumerable<KeyValuePair<string, string>>> result)
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

        public void Log(IEnumerable<UrlLinkInfo> result)
        {
            System.Console.WriteLine("Results:");
            foreach (var item in result)
            {
                System.Console.WriteLine();
                System.Console.WriteLine(item.LinkTitle);
                System.Console.WriteLine(item.LinkUrl);
                System.Console.WriteLine();
            }            
        }
    }
}