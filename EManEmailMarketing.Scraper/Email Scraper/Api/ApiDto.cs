using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Scraper.Api
{
    public class ApiDto
    {
       public string nextApiCall { get; set; }
       public List<string> urls { get; set; }
    }
}
