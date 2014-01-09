using System;
using System.Linq;
using System.Web;

namespace ScreenScrapping.Engine.Dtos
{
    public sealed class UniqueAndItemsLimitAwareUrlInfoCollection : UniqueCollection<UrlLinkInfo>
    {
        private readonly int _maxItems;
        public UniqueAndItemsLimitAwareUrlInfoCollection(int maxItems)
        {
            _maxItems = maxItems;
        }

        public bool MaxLimitReached { get { return !ValidateMaxEntriesTreshold(); } }

        protected override UrlLinkInfo ConvertToEligibleFormat(UrlLinkInfo originalItem)
        {
            var decodedUrl = HttpUtility.UrlDecode(originalItem.LinkUrl);
            return new UrlLinkInfo {LinkTitle = originalItem.LinkTitle, LinkUrl = decodedUrl};
        }

        public override void Add(UrlLinkInfo item)
        {
            if (!MaxLimitReached)
            {
                base.Add(item);
            }
        }

        private bool ValidateMaxEntriesTreshold()
        {
            if (_maxItems < 0)
                return true;

            return Items.Count() < _maxItems;
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