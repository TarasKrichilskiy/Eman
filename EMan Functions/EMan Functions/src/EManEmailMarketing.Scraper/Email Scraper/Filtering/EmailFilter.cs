using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EManEmailMarketing.Scraper.Email_Scraper.Filtering
{
    public class EmailFilter
    {
        public bool IsStandard(string s)
        {
            s = s.ToLower();


            if (s.Contains(".gov") || s.Contains(".org") || s.Contains("sentry.") || s.Contains("sentry-") ||
                s.Contains("email.com") || s.Contains("address.com") || s.Contains("reply.craigslist") ||
                s.Contains("domain.com") || s.Contains("@sale.craigslist.org"))
            {
                return false;
            }
                

            //for those really short weird emails. Ex: j3@W.Z and 7@j.X
            if ((s.Length - s.IndexOf('@')) <= 3 || (s.IndexOf('@') < 5)) { return false; }

            //need to fix the code for after the @ character. need to check the 
            //character length in between the dots. Toss if length is 1

            string local = s.Split("@")[0];
            Dictionary<char, int> dict = new Dictionary<char, int>();
            foreach (char c in local)
            {
                if (!dict.ContainsKey(c))
                {
                    dict.Add(c, 1);
                }
                   
                else
                {
                    dict[c] = dict[c] + 1;
                }
                    
            }

            if (dict.Count == 1) { return false; }

            return true;
        }
    }
}
