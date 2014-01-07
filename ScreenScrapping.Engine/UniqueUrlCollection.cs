using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenScrapping.Engine
{
    /// <summary>
    /// stores collection of unique url decoded urls
    /// </summary>
    internal sealed class UniqueUrlCollection : IEnumerable<string>
    {
        private readonly IList<string> _visitedUrls;

        public UniqueUrlCollection()
        {
            _visitedUrls = new List<string>();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _visitedUrls.TakeWhile(t => t != null).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string item)
        {
            string decodedUrl;
            if (!Contains(item, out decodedUrl))
            {
                _visitedUrls.Add(decodedUrl);
            }
        }

        public void AddRange(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                Add(url);
            }
        }

        public bool Contains(string item)
        {
            string decodedUrl;
            return Contains(item, out decodedUrl);
        }

        private bool Contains(string item, out string decodeUniqueUrl)
        {
            decodeUniqueUrl = null;

            var decodedUrl = UrlDecode(item);
            if (!_visitedUrls.Contains(decodedUrl))
            {
                decodeUniqueUrl = decodedUrl;
                return false;
            }
            return true;
        }

        private string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }
    }
}
