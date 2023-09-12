using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EManEmailMarketing
{
    public class IPGeoLocator
    {
        public async Task<string> GetLocationDetails(string ip)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                httpClient.BaseAddress = new Uri("http://ip-api.com/json/" + ip + "?fields=status,message,country,regionName,city,zip,isp,mobile,proxy,hosting");
                HttpResponseMessage httpResponse = await httpClient.GetAsync(ip);
                // If API is success and receive the response, then get the location details
                if (httpResponse.IsSuccessStatusCode)
                {
                    var result = await httpResponse.Content.ReadAsStringAsync();

                    return result;
                }
            }

            return null;
        }
    }

    //public class LocationDetails
    //{
    //    public string status { get; set; }
    //    public string isp { get; set; }
    //    public bool proxy { get; set; }
    //    public bool hosting { get; set; }
    //    public bool mobile { get; set; }
    //    public string country { get; set; }
    //    public string city { get; set; }
    //    public string regionName { get; set; }
    //    public string zip { get; set; }
    //}
}
