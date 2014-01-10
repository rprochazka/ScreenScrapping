using System.Collections.Generic;
using ScreenScrapping.Engine.Dtos;

namespace ScreenScrapping.Console.Logging
{
    interface ILogger
    {
        void Log(string message);

        void Log(IEnumerable<KeyValuePair<string, string>> result);

        void Log(string url, IEnumerable<KeyValuePair<string, string>> result);

        void Log(IEnumerable<IEnumerable<KeyValuePair<string, string>>> result);

        void Log(IEnumerable<UrlLinkInfo> result);
    }
}
