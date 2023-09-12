using EManEmailMarketing.Common.DTO;
using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EManEmailMarketing.Storage.Repositories
{
    public class EmailCallbackRepository : Repository<EmailCallback>, IEmailCallbackRepository
    {
        public EmailCallbackRepository(ApplicationDbContext context) : base(context) { }

        public async Task<int?> Add(SendGridCallbackDTO callback)
        {
            _ = callback ?? throw new ArgumentNullException(nameof(callback));
  
            var EmailID = callback.EmailId.Split('.')[0];

            var sentEmail = await PlutoContext.SentEmails.SingleOrDefaultAsync(a => a.SendGridID == EmailID);

            if (sentEmail == null) { return null; }

            sentEmail.EmailCallback = callback.Event;
            sentEmail.EmailCallbackDate = DateTime.UtcNow;

            EmailCallback newEmailCallback = new EmailCallback
            {
                SentEmailID = sentEmail.SentEmailID,
                Event = callback.Event,
                Date = DateTime.UtcNow,
                Email = callback.Email,
                IP = callback.ip
            };

            await PlutoContext.EmailCallbacks.AddAsync(newEmailCallback);
            await PlutoContext.SaveChangesAsync();
            return sentEmail.ScrapedEmailID;

        }

        public ApplicationDbContext PlutoContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
