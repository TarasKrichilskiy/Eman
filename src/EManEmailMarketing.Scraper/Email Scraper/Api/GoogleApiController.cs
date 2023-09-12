using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Email_Scraper.Api
{
    public class GoogleApiController : ApiController
    {
        public HttpClient Client { get; set; }

        public GoogleApiController(string query)
        {
            //DotNetEnv.Env.TraversePath().Load();
            
            //var API_KEY = DotNetEnv.Env.GetString("GOOGLE_API_KEY");
            //var CX = DotNetEnv.Env.GetString("CX");

            StringBuilder builder = new StringBuilder();

            builder.Append("https://www.googleapis.com/customsearch/v1?key=");
            builder.Append("AIzaSyBrWsMAMgP-_2Y452B7ySk2zgGB9IlXSvE" + "&cx=");
            builder.Append("204fa34b0f0924242" + "&q=");
            builder.Append(query);

            this.url = builder.ToString();
        }
        public override async Task<List<string>> fetch()
        {
            string response = await connect();
            if (response != null)
            {
                return extract(response);
            }
            else
            {
                return null;
            }
        }

        private async Task<string> connect()
        {
            
            //this.Client.BaseAddress = new Uri(this.url);
            this.Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await Client.GetAsync(this.url);  
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Console.WriteLine("Ran past the API calls limit for Google Search API");

            //There's most likely a better way to return when you out of API calls.
            return null;
        }

        private List<string> extract(string response)
        {
            JObject responseJson = JObject.Parse(response);

            //if there is only 1 page of search results, this will crash
            //TODO: Fix
            int startIndex = Convert.ToInt32(responseJson["queries"]["nextPage"][0]["startIndex"].ToString());

            StringBuilder builder = new StringBuilder(this.url);
            builder.Append("&start=" + startIndex.ToString());
            this.url = builder.ToString();

            List<string> urls = new List<string>();
            JArray items = (JArray)responseJson["items"];
            for (int i = 0; i < items.Count; i++)
            {
                string url = items[i]["formattedUrl"].ToString();
                urls.Add(url);
            }

            return urls;
        }
    }
}
