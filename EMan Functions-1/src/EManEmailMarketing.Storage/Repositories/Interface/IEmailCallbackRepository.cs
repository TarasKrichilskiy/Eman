using EManEmailMarketing.Common.DTO;
using EManEmailMarketing.Storage.Models;

namespace EManEmailMarketing.Storage.Repositories.Interface
{
    public interface IEmailCallbackRepository : IRepository<EmailCallback>
    {
        /// <summary>
        /// Creates a new EmailCallback entry based on SendGridCallbackDTO paremeters
        /// </summary>
        /// <param name="callback"> SendGridCallBackDTO</param>
        /// <returns></returns>
        public Task<int?> Add(SendGridCallbackDTO callback);
    }
}
