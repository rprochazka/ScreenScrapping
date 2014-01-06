using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenScrapping.Engine
{
    public interface IEngineManager
    {
        /// <summary>
        /// get list of urls on a page (e.g. links to detail page from the master page)
        /// </summary>
        /// <param name="initialUrl">the first page url</param>
        /// <param name="detailLinkUrlXPath">xpath of the detail link url value</param>
        /// <returns></returns>
        IEnumerable<string> GetDetailLinkUrls(string initialUrl, string detailLinkUrlXPath);

        /// <summary>
        /// get list of urls on a given (first) page (e.g. links to detail page from the master page) and the following ones
        /// in a paging scenarios
        /// </summary>
        /// <param name="initialUrl">the first page url</param>
        /// <param name="detailLinkUrlXPath">xpath of the detail link url value</param>
        /// <param name="nextPageLinkUrlXPath">xpath of the 'next' link url value</param>
        /// <returns></returns>
        IEnumerable<string> GetDetailLinkUrls(string initialUrl, string detailLinkUrlXPath, string nextPageLinkUrlXPath);

        /// <summary>
        /// performs screen scrapping of the given page based on the xpath fields definition
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fieldsXPathDefinitions"></param>
        /// <returns></returns>
        IDictionary<string, string> GetScrappedFields(string url, IDictionary<string, string> fieldsXPathDefinitions);
    }
}
