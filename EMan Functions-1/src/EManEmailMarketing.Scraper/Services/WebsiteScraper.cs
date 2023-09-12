using Email_Scraper;
using EManEmailMarketing.Scraper.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Email_Scraper.Api;
using System.Linq;

namespace EManEmailMarketing.Scraper.Services
{
    public class WebsiteScraper : IWebsiteScraper
    {
        public WebsiteScraper() {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public async Task<List<string>> RunGoogleSeach(string nextword)
        {
            ApiControllerFactory factory = new ApiControllerFactory();
            HttpClient client = new HttpClient();

            ApiController controller = factory.createController("google", nextword, client);

            List<string> urlsFromApi = new List<string>();
            bool apiCallsAvailable = true;
            while (apiCallsAvailable)
            {
                try
                {
                    List<string> listOfUrls = await controller.fetch();
                    if (listOfUrls != null)
                    {
                        urlsFromApi.AddRange(listOfUrls);
                    }
                    else
                    {
                        apiCallsAvailable = false;
                    }
                }
                catch (Exception e)
                {
                    break;
                }
            }

            return urlsFromApi;
        }

        public async Task<UrlResultsDTO> RunWebsiteScrape(string url)
        {
            ItemScraper scraper = new ItemScraper();

            try
            {
                var result = await scraper.findAsync(url);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
