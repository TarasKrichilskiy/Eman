using EManEmailMarketing.Storage.Models;

namespace EManEmailMarketing.Storage.Repositories.Interface
{
    public interface IEmailRepository : IRepository<Emails>
    {
        Task<Emails?> GetNextEmail(string client);

        Task<Emails?> GetNextEmail(int clientid, string stateFilter);
        Task UpdateEmailSentForPaul(int EmailID, string client);
    }
}
