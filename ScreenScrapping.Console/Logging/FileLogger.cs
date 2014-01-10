using System.Collections.Generic;
using ScreenScrapping.Engine.Dtos;

namespace ScreenScrapping.Console.Logging
{
    class FileLogger : ILogger
    {
        public void Log(string message)
        {
            throw new System.NotImplementedException();
        }

        public void Log(IEnumerable<KeyValuePair<string, string>> result)
        {
            throw new System.NotImplementedException();
        }

        public void Log(string url, IEnumerable<KeyValuePair<string, string>> result)
        {
            throw new System.NotImplementedException();
        }

        public void Log(IEnumerable<IEnumerable<KeyValuePair<string, string>>> result)
        {
            throw new System.NotImplementedException();
        }

        public void Log(IEnumerable<UrlLinkInfo> result)
        {
            throw new System.NotImplementedException();
        }
    }
}