using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using EManEmailMarketing.Storage.Repositories.Interface;

namespace EManEmailMarketing.Storage.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private readonly QueueClient client;

        public QueueRepository(QueueClient client) {
            this.client = client;
        }

        public async Task<SendReceipt> PostMessage(string message) {
            return await client.SendMessageAsync(message);
        }

        public async Task<PeekedMessage[]> GetAllMessages()
        {
            return await client.PeekMessagesAsync(maxMessages: 32);
        }

        public async Task<QueueMessage[]> ReadNextMessages()
        {
            return await client.ReceiveMessagesAsync(maxMessages: 32);
        }
    }
}
