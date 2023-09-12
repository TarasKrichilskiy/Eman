using EManEmailMarketing.Common.Constants;
using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EManEmailMarketing.Storage.Repositories
{
    public class SentEmailsRepository : Repository<SentEmail>, ISentEmailsRepository
    {
        public SentEmailsRepository(ApplicationDbContext context) : base(context) { }

        public async Task Add(ClientProgress clientData, string SendGridCallbackID, int EmailID, string toEmail, bool isScraped = false)
        {
            SentEmail newSentEmail = new SentEmail
            {
                ClientID = clientData.ClientID,
                SentDate = DateTime.UtcNow,
                SendGridID = SendGridCallbackID,
                ScrapedEmailID = isScraped ? EmailID : null,
                EmailID = isScraped ? null : EmailID,
                ToEmail = toEmail,
                HTMLBody = "",
                FromRec = EmailConstants.CompanyEmail,

            };
            PlutoContext.SentEmails.Add(newSentEmail);
            await PlutoContext.SaveChangesAsync();
        }

        public ApplicationDbContext PlutoContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
