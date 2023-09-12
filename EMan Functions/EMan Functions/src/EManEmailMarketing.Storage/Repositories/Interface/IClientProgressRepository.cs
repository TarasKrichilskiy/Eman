using EManEmailMarketing.Storage.Models;

namespace EManEmailMarketing.Storage.Repositories.Interface
{
    public interface IClientProgressRepository : IRepository<ClientProgress>
    {
        /// <summary>
        /// Return client progress for current month
        /// </summary>
        /// <param name="clientId"> the clientId</param>
        /// <returns>ClientProgres</returns>
        public Task<ClientProgress> GetCurrentClientProgressByClientId(int clientId);


    }
}
