using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Email_Scraper
{
    public class UrlResultsDTO
    {
        public UrlResultsDTO()
        {
            OtherRoutes = new List<UrlResultsDTO>();
        }

        public string CurrentUrl { get; set; }
        public HashSet<string> Emails { get; set; }
        public List<UrlResultsDTO> OtherRoutes { get; set; }
    }
}
