using System.Web;

namespace ScreenScrapping.Engine.Dtos
{
    internal sealed class UniqueUrlCollection : UniqueCollection<string>
    {
        protected override string ConvertToEligibleFormat(string originalItem)
        {
            return HttpUtility.UrlDecode(originalItem);
        }
    }    
}
