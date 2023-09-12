using EManEmailMarketing.Scraper.Services;
using System;
using System.Threading.Tasks;

namespace EManEmailMarketing.Scraper
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var results = await new WebsiteScraper().RunGoogleSeach("hvac in Seattle WA");

            foreach (var result in results)
            {
                var scrapedEmails = new WebsiteScraper().RunWebsiteScrape(result);
                Console.WriteLine("Result: " + scrapedEmails);
            }
        }
    }
}
