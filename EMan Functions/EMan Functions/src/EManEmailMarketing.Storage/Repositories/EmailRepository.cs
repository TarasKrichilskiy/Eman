using EManEmailMarketing.Storage;
using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Linq;

namespace EManEmailMarketing.Storage.Repositories
{
    public class EmailRepository : Repository<Emails>, IEmailRepository
    {
        public EmailRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Emails?> GetNextEmail(string client)
        {
            Emails? next = await PlutoContext.Emails.FirstOrDefaultAsync(a => client == "SendForPaul" ? a.SendForPaul == true : client == "SendForPSGcontractors" ? a.SendForPSGcontractors == true : false);
            
            return next;
        }

        public async Task<Emails?> GetNextEmail(int clientId, string stateFilter)
        {
            return await PlutoContext.Emails
                .Include(e => e.SentEmails.Where(s => s.ClientID != clientId) )
                .FirstOrDefaultAsync(a => a.State == stateFilter);
        }

        public async Task UpdateEmailSentForPaul(int EmailID, string client)
        {
            Emails? next = await PlutoContext.Emails.SingleOrDefaultAsync(a => a.EmailID == EmailID);

            if (next != null)
            {
                if (client == "SendForPaul")
                {
                    next.SendForPaul = false;
                }
                else if (client == "SendForPSGcontractors")
                {
                    next.SendForPSGcontractors = false;
                }

                await PlutoContext.SaveChangesAsync();
            }
        }

        public ApplicationDbContext PlutoContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
