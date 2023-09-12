using EManEmailMarketing.Storage;
using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace EManEmailMarketing.Storage.Repositories
{
    public class ScrapedEmailRepository : Repository<ScrapedEmail>, IScrapedEmailRepository
    {
        public ScrapedEmailRepository(ApplicationDbContext context) : base(context) { }

        /// <inheritdoc />
        public async Task<ScrapedEmail?> GetNext()
        {
            // TODO: this should be able to handle any client, not jsut Paul
            ScrapedEmail? next = await PlutoContext.ScrapedEmails.FirstOrDefaultAsync(a => a.SendForPaul == true && a.IsActive == true);
            
            return next;
        }

        
        /// <inheritdoc />
        public async Task UpdateEmailSentForPaul(int scrapedEmailID)
        {
            //TODO: this method should be able to handle any client, not just Paul
            ScrapedEmail? next = await PlutoContext.ScrapedEmails.SingleOrDefaultAsync(a => a.ScrapedEmailID == scrapedEmailID);

            if (next != null)
            {
                next.SendForPaul = false;

                await PlutoContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task Add(string email, string website, string keyword)
        {
            ScrapedEmail newScrapedEmail = new ScrapedEmail
            {
                WebsiteLink = website,
                Email = email,
                KeyWordUsed = keyword,
                DateScraped = DateTime.Now,
                IsActive = true,
            };

            await PlutoContext.ScrapedEmails.AddAsync(newScrapedEmail);
            await PlutoContext.SaveChangesAsync();
        }

        public ApplicationDbContext PlutoContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
