//using Email_Scraper.Crawlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using AngleSharp;
using AngleSharp.Html.Dom;
using EllipticCurve;
using EManEmailMarketing.Scraper.Email_Scraper.Filtering;

namespace Email_Scraper
{
    //class MyWebClient : WebClient
    //{
    //    protected override WebRequest GetWebRequest(Uri address)
    //    {
    //        HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
    //        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
    //        return request;
    //    }
    //}

    public class ItemScraper
    {
        public string emailPattern = @"([a-zA-Z0-9_\-\.]+)@([a-zA-Z_\-\.]+)\.([a-zA-Z]{1,10})";
        public Regex emailRegex;
        HttpClient client;

        public ItemScraper()
        {
            this.client = new HttpClient();
            emailRegex = new Regex(emailPattern);
        }

        public async Task<UrlResultsDTO> findAsync(string url, int ct = 0)
        {
            try
            {
                string base_url = url;

                HttpResponseMessage response = await client.GetAsync(url);
                HttpContent content = response.Content;
                string htmlSource = await content.ReadAsStringAsync();

                MatchCollection matchedEmails = emailRegex.Matches(htmlSource);

                UrlResultsDTO dto = new UrlResultsDTO();
                HashSet<string> emails = new HashSet<string>();
                HashSet<string> nexturls = new HashSet<string>();

                var context = BrowsingContext.New(Configuration.Default);
                var document = await context.OpenAsync(req => req.Content(htmlSource));
                var anchors = document.QuerySelectorAll("a").OfType<IHtmlAnchorElement>();

                HashSet<string> websiteRoutes = new HashSet<string>();

                foreach (var a in anchors)
                {
                    string foundUrl = a.GetAttribute("href");
                    StringBuilder builder = new StringBuilder();

                    if (foundUrl != null && foundUrl.StartsWith("/"))
                    {
                        char lastCharacter = base_url[base_url.Length - 1];
                        if (lastCharacter == '/')
                        {
                            builder.Append(base_url.Remove(base_url.Length - 1));
                        }
                        else
                        {
                            builder.Append(base_url);
                        }

                        builder.Append(foundUrl);
                        nexturls.Add(builder.ToString());
                        websiteRoutes.Add(builder.ToString());
                    }
                    else if (foundUrl != null && foundUrl.StartsWith("https"))
                    {
                        nexturls.Add(foundUrl);
                    }

                    //20urls is most we need
                    if(nexturls.Count >= 20)
                    {
                        break;
                    }
                }

                EmailFilter emailFilter = new EmailFilter();

                for (int i = 0; i < matchedEmails.Count; i++)
                {
                    if (emailFilter.IsStandard(matchedEmails[i].Value))
                    {
                        emails.Add(matchedEmails[i].Value);
                    }
                }

                dto.CurrentUrl = url;
                dto.Emails = emails;
                var nextct = 0;
                if (ct < 3)
                {
                    foreach (var nexturl in nexturls)
                    {
                        nextct++;
                        if ((ct == 0 && nextct < 20) || (ct == 1 && nextct < 10) || (ct == 2 && nextct < 5))
                        { // need to limit to 3 sub routes since it could get into infinite loop of routes. 20 * 10 * 5 = 1k scraped websites on worst case
                            var nextScrape = await findAsync(nexturl, ct + 1);
                            if (nextScrape != null) { dto.OtherRoutes.Add(nextScrape); }
                        }
                    }
                }

                return dto;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
