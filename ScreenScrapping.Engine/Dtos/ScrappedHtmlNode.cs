using System.Collections.Generic;

namespace ScreenScrapping.Engine.Dtos
{
    internal sealed class ScrappedHtmlNode
    {
        public ScrappedHtmlNode()
        {
            Attributes = new Dictionary<string, string>();
        }

        public string NodeText { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Attributes { get; set; }
    }
}
