using System;
using System.Net;
using System.Text;
using System.Web;

namespace ScreenScrapping.Engine.Helpers
{
    internal class WebDownloader
    {
        public string GetUrlContent(string url)
        {
            Guard(url);

            var webClient = new WebClient {Encoding = Encoding.UTF8};
            var htmlEncoded = webClient.DownloadString(url);

            return HttpUtility.HtmlDecode(htmlEncoded);
        }   
     
        void Guard(string url)
        {
            Uri absoluteUri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                throw new ArgumentException("Provided url is not a valid absolute Uri");
            }
        }
    }
}
