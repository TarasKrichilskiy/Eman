using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Email_Scraper.Api
{
    public class ApiControllerFactory
    {

        public ApiController createController(string type, string query, HttpClient client)
        {
            if (type.ToLower() == "google")
            {
                GoogleApiController controller = new GoogleApiController(query);
                controller.Client = client;
                return controller;
            }
            return null;
        }
    }
}
