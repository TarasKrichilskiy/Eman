using EManEmailMarketing.Storage;
using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace EManEmailMarketing.Storage.Repositories
{
    public class ScrapedWebsiteRepository : Repository<ScrapedWebsite>, IScrapedWebsiteRepository
    {
        public ScrapedWebsiteRepository(ApplicationDbContext context) : base(context) { }

        public async Task Add(string website, string keyword)
        {
            
            ScrapedWebsite newScrapedWebsite = new ScrapedWebsite
            {
                Url = website,
                KeyWordUsed = keyword,
                HasBeenScraped = false,
                CreatedDate = DateTime.Now
            };

            await PlutoContext.ScrapedWebsites.AddAsync(newScrapedWebsite);
            await PlutoContext.SaveChangesAsync();
        }

        public async Task<ScrapedWebsite> GetNext()
        {
            ScrapedWebsite? next = await PlutoContext.ScrapedWebsites.FirstOrDefaultAsync(a => a.HasBeenScraped == false);

            if (next != null)
            {
                // TODO: if there is an error upstream and the website is not actually used, we will be wasting websites 
                // by marking them as scraped even if they arent
                next.HasBeenScraped = true;

                await PlutoContext.SaveChangesAsync();

                return next;
            }
            else
            {
                //TODO: come up with a less generic exeption to throw, or just log the issue without throwing and return null
                throw new Exception("No More UnScraped ScrapedWebsites Found");
            }
        }

        public ApplicationDbContext PlutoContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
