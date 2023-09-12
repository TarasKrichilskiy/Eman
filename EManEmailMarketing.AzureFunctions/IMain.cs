using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EManEmailMarketing
{
    public interface IMain
    {
        Task ProcessEmailRequest(int? clientProgressID, string email);
        Task ProcessEmailForClient(int clientid);
        Task ProcessPaulsEmailNext();
        Task RunNextWebsiteScrape(ILogger log);
        Task ProcessCallback(ILogger log, string req);
        Task RunNextGoogleSearch();
    }
}
