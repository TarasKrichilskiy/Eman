using Azure.Storage.Queues;
using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;

namespace EManEmailMarketing.Storage.Repositories
{
    public class EmailQueueRepository : QueueRepository, IEmailQueueRepository
    {
        public EmailQueueRepository(QueueClient client) : base(client) { }
    }
}
