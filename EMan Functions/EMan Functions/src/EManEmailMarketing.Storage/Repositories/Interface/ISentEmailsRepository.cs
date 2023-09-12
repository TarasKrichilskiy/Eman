using EManEmailMarketing.Storage.Models;

namespace EManEmailMarketing.Storage.Repositories.Interface
{
    public interface ISentEmailsRepository : IRepository<SentEmail>
    {
        public Task Add(ClientProgress clientData, string SendGridCallbackID, int EmailID, string toEmail, bool isScraped = false);
    }
}
