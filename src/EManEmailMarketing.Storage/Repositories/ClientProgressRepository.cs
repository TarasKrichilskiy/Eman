using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EManEmailMarketing.Storage.Repositories
{
    public class ClientProgressRepository : Repository<ClientProgress>, IClientProgressRepository
    {
        public ClientProgressRepository(ApplicationDbContext context) : base(context) { }

        /// <inheritdoc/>
        public async Task<ClientProgress> GetCurrentClientProgressByClientId(int clientId)
        {
            var now = DateTime.UtcNow;
            var currentPlan = await PlutoContext.ClientsProgress.FirstOrDefaultAsync(c => c.ClientID == clientId && now >= c.StartDate && now <= c.EndDate);
            return currentPlan;

        }
        private ApplicationDbContext PlutoContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
