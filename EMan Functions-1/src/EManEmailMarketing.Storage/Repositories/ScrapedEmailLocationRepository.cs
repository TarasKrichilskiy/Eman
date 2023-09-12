using EManEmailMarketing.Common.DTO;
using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EManEmailMarketing.Storage.Repositories
{
    public class ScrapedEmailLocationRepository : Repository<ScrapedEmailLocation>, IScrapedEmailLocationRepository
    {
        public ScrapedEmailLocationRepository(ApplicationDbContext context) : base(context) { }

        public async Task Add(int ScrapedEmailID, LocationDetailsDTO locationDetails)
        {
            ScrapedEmailLocation scrapedEmailLocation = new ScrapedEmailLocation
            {
                ScrapedEmailID = ScrapedEmailID,
                Country = locationDetails.country,
                State = locationDetails.regionName,
                City = locationDetails.city,
                Zip = locationDetails.zip,
                ISP = locationDetails.isp,
                IsProxy = locationDetails.proxy,
                IsHosted = locationDetails.hosting,
                IsMobile = locationDetails.mobile,
                CreatedDate = DateTime.UtcNow
            };

            await PlutoContext.ScrapedEmailLocations.AddAsync(scrapedEmailLocation);
            await PlutoContext.SaveChangesAsync();
        }

        public ApplicationDbContext PlutoContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
