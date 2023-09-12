using Azure.Storage.Queues.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EManEmailMarketing.Storage.Controllers
{
    [Route("api/emails")]
    [ApiController]
    public class EmailQueueController : ControllerBase
    {
        private readonly ILogger<EmailQueueController> logger;
        private readonly IEmailQueueRepository emailQueueRepository;

        public EmailQueueController(IEmailQueueRepository emailQueueRepository, ILogger<EmailQueueController> logger)
        {
            this.emailQueueRepository = emailQueueRepository;
            this.logger = logger;
        }

        [Route("postemailrequest")]
        public async Task<SendReceipt> PostEmailRequest([FromQuery(Name = "message")] string message)
        {
            logger.LogInformation($"Post email request recieved. Message: {message}");
            return await emailQueueRepository.PostMessage(message);
        }

        [Route("getallwaiting")]
        public async Task<PeekedMessage[]> GetAllWaiting()
        {
            return await emailQueueRepository.GetAllMessages();
        }
    }
}
