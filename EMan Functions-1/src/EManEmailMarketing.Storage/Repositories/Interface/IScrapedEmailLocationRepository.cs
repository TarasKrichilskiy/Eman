using EManEmailMarketing.Common.DTO;
using EManEmailMarketing.Storage.Models;

namespace EManEmailMarketing.Storage.Repositories.Interface
{
    public interface IScrapedEmailLocationRepository : IRepository<ScrapedEmailLocation>
    {
        /// <summary>
        /// Add a new entry to ScrapedEmailLocation repo 
        /// </summary>
        /// <param name="ScrapedEmailID"> id of scraped email</param>
        /// <param name="locationDetails"> location details of email</param>
        /// <returns></returns>
        Task Add(int ScrapedEmailID, LocationDetailsDTO locationDetails);
    }
}
