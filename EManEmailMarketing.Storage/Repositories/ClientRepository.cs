using EManEmailMarketing.Storage;
using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EManEmailMarketing.Storage.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context) {}

        public async Task<IEnumerable<Client>> GetByPlanClicksPerMonthDesc(int planClicksPerMonth)
        {
            return await PlutoContext.Clients.OrderByDescending(c => c.PlanClicksPerMonth).Take(planClicksPerMonth).ToListAsync();
        }

        public async Task<string> GetClientStateFilter(int ClientID)
        {
            var clientFilter = await PlutoContext.Clients.FirstOrDefaultAsync(a => a.ClientID == ClientID);

            return clientFilter.StateFilter;
        }

        public async Task<IEnumerable<Client>> GetSentEmailsForClient()
        {
            return await PlutoContext.Clients
                .Include(a => a.SentEmailList).ToListAsync();
        }

        public ApplicationDbContext PlutoContext
        {
            get { return Context as ApplicationDbContext; }
        }

    }
}
