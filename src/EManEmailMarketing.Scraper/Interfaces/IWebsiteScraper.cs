using Email_Scraper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EManEmailMarketing.Scraper.Interfaces
{
    public interface IWebsiteScraper
    {
        Task<UrlResultsDTO> RunWebsiteScrape(string url);

        Task<List<string>> RunGoogleSeach(string nextword);
    }
}
