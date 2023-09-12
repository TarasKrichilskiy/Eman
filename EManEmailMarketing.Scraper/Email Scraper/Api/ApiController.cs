using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Scraper
{
    public abstract class ApiController
    {
        protected string url = string.Empty;

        public abstract Task<List<string>> fetch();
        
    }
}
