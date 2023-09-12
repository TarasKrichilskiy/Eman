using EManEmailMarketing.Storage.Models;

namespace EManEmailMarketing.Storage.Repositories.Interface
{
    public interface IScrapedWebsiteRepository : IRepository<ScrapedWebsite>
    {
        /// <summary>
        /// Adds a ScrapedWebsite entry
        /// </summary>
        /// <param name="websiteUrl"> the website url</param>
        /// <param name="keyword"> keyword used to find website url</param>
        /// <returns></returns>
        Task Add(string websiteUrl, string keyword);

        /// <summary>
        /// Get next valid website to use
        /// </summary>
        /// <returns>A ScrapedWebsite</returns>
        Task<ScrapedWebsite> GetNext();
    }
}
