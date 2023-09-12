using EManEmailMarketing.Storage.Models;

namespace EManEmailMarketing.Storage.Repositories.Interface
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<string> GetClientStateFilter(int ClientID);
        Task<IEnumerable<Client>> GetSentEmailsForClient();
    }
}
