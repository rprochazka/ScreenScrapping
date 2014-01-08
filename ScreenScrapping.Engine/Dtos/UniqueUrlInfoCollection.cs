using System;
using System.Web;

namespace ScreenScrapping.Engine.Dtos
{
    public sealed class UniqueUrlInfoCollection : UniqueCollection<UrlLinkInfo>
    {
        protected override UrlLinkInfo ConvertToEligibleFormat(UrlLinkInfo originalItem)
        {
            var decodedUrl = HttpUtility.UrlDecode(originalItem.LinkUrl);
            return new UrlLinkInfo {LinkTitle = originalItem.LinkTitle, LinkUrl = decodedUrl};
        }
    }

    public sealed class UrlLinkInfo : IEquatable<UrlLinkInfo>
    {
        public string LinkUrl { get; set; }
        public string LinkTitle { get; set; }

        public bool Equals(UrlLinkInfo other)
        {
            if (null == other)
            {
                return false;
            }

            return LinkUrl == other.LinkUrl;
        }
    }
}