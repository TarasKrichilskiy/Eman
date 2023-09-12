using Azure.Storage.Queues.Models;

namespace EManEmailMarketing.Storage.Repositories.Interface
{
    public interface IQueueRepository
    {
        Task<SendReceipt> PostMessage(string message);

        Task<PeekedMessage[]> GetAllMessages();

        Task<QueueMessage[]> ReadNextMessages();

    }
}
