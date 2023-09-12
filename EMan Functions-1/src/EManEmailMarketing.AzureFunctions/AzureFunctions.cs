using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Net;
using EManEmailMarketing.EmailingManager.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Http;
using EManEmailMarketing.SmartMail.Services;
using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using EManEmailMarketing.Common.DTO;
using EManEmailMarketing.Common.Constants;
using AngleSharp.Dom;
using EManEmailMarketing.Common.Telemetry;
using Azure.Storage.Queues.Models;

namespace EManEmailMarketing
{
    public class AzureFunctions
    {
        private readonly IMain main;
        private readonly IEmailService emailService;
        private readonly IEmailFrequencyDeterminer emailFrequencyDeterminer;
        private readonly IClientRepository clientRepository;
        private readonly IEmailQueueRepository emailQueueRepository;
        private readonly IEmailCallbackRepository emailCallbackRepository;
        private readonly ILogger<AzureFunctions> logger; 

        public AzureFunctions(
            IMain main,
            IEmailService emailService,
            IEmailFrequencyDeterminer emailFrequencyDeterminer,
            IClientRepository clientRepository,
            IEmailQueueRepository emailQueueRepository,
            IEmailCallbackRepository emailCallbackRepository,
            ILogger<AzureFunctions> logger)
        {
            this.main = main ?? throw new ArgumentNullException(nameof(main));
            this.emailService = emailService ?? throw new Exception(nameof(emailService));
            this.emailFrequencyDeterminer = emailFrequencyDeterminer ?? throw new Exception(nameof(emailFrequencyDeterminer));
            this.clientRepository = clientRepository ?? throw new Exception(nameof(clientRepository));
            this.emailQueueRepository = emailQueueRepository ?? throw new Exception(nameof(emailQueueRepository));
            this.emailCallbackRepository = emailCallbackRepository ?? throw new Exception(nameof(emailCallbackRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// A replication of ProcessEmailsDaily that can be triggered manually
        /// </summary>
        /// <param name="myQueueItem"></param>
        /// <returns></returns>
        [FunctionName("ProcessEmailManual")]
        public async Task ProcessEmailManual([QueueTrigger(QueueConstants.EmailQueueManual, Connection = "QueueConnection")] string myQueueItem)
        {
            logger.LogInformation(EventConstants.PROCESS_EMAIL_MANUAL_START, $"processing queue started for: {myQueueItem}");
            try
            {
                var clients = await clientRepository.GetAll();
                foreach (var client in clients)
                {
                    var emailsToSend = await emailFrequencyDeterminer.GetEmailFrequency(client.ClientID, DateTime.UtcNow);
                    logger.LogInformation(EventConstants.PROCESS_EMAIL_MANUAL, $"processing emails request for client {client.ClientID}. Posting {emailsToSend} emails");
                    for (int i = 0; i < emailsToSend; i++)
                    {
                        await emailQueueRepository.PostMessage(client.ClientID.ToString());
                    }
                }
            logger.LogInformation(EventConstants.PROCESS_EMAIL_MANUAL_END, "proccessing queue succeeded");
            }
            catch (Exception ex)
            {
                logger.LogError(EventConstants.PROCESS_EMAIL_MANUAL_ERROR, ex, $"Error while processing process email manual: {myQueueItem}");
            }
        }

        /// <summary>
        /// This is the main function that will run daily and decide how many emails need to be sent for every client
        /// </summary>
        /// <param name="timer">the timer</param>
        /// <returns></returns>
        [FunctionName("ProcessEmailsDaily")]
        public async Task ProcessEmailsDaily([TimerTrigger("0 0 12 * * *")] TimerInfo timer)
        {
            logger.LogInformation(EventConstants.PROCESS_EMAIL_DAILY_START, $"ProcessEmailsDaily Started");
            try
            {
                var clients = await clientRepository.GetAll();
                foreach (var client in clients)
                {
                    if (client.IsActive == true)
                    {
                        var emailsToSend = await emailFrequencyDeterminer.GetEmailFrequency(client.ClientID, DateTime.UtcNow);
                        for (int i = 0; i < emailsToSend; i++)
                        {
                            await emailQueueRepository.PostMessage(client.ClientID.ToString());
                        }
                    }
                }
                logger.LogInformation(EventConstants.PROCESS_EMAIL_DAILY_END, "ProcessEmailsDaily Completed");
            }
            catch (Exception e)
            {
                logger.LogError(EventConstants.PROCESS_EMAIL_DAILY_ERROR, e, $"Error in ProcessEmailsDaily");
            }
        }

        ///// <summary>
        ///// The queue for handeling send email requests
        ///// </summary>
        ///// <param name="message"></param>
        ///// <returns></returns>
        [FunctionName("ProcessEmailSendRequests")]
        public async Task ProcessEmailSendRequests([QueueTrigger(QueueConstants.EmailQueue, Connection = "AzureWebJobsStorage")] string message)
        {
            logger.LogInformation(EventConstants.PROCESS_EMAIL_SEND_REQUEST_START, $"ProcessEmailSendRequests for: {message}");
            try
            {
                await main.ProcessEmailForClient(Int32.Parse(message));
                logger.LogInformation(EventConstants.PROCESS_EMAIL_SEND_REQUEST_END, "ProcessEmailSendRequests for: {message}");
            }
            catch (Exception e)
            {
                logger.LogError(EventConstants.PROCESS_EMAIL_SEND_REQUEST_ERROR, e, $"Error during ProcessEmailSendRequests: {message}");
            }
        }

        [FunctionName("ProcessEmailScheduled")]
        public async Task ProcessEmailScheduled([TimerTrigger("0 */30 9-17 * * *")] TimerInfo myTimer, ExecutionContext context)
        {
            logger.LogInformation($"ProcessEmailScheduled started");
            await main.ProcessPaulsEmailNext();
        }

        [FunctionName("RunNextGoogleScrape")]
        public async Task RunNextGoogleScrape([TimerTrigger("0 */30 2-7 * * *")] TimerInfo myTimer)
        {
            logger.LogInformation(EventConstants.PROCESS_WEBSITE_SCRAPE_START, $"RunNextGoogleScrape started");

            try
            {
                await main.RunNextGoogleSearch();
            }
            catch (Exception ex)
            {
                logger.LogError(EventConstants.PROCESS_WEBSITE_SCRAPE_ERROR, ex, "Error while processing RunNextScrape az function");
            }
            logger.LogInformation(EventConstants.PROCESS_WEBSITE_SCRAPE_END, $"RunNextGoogleScrape ended");
        }

        [FunctionName("RunNextWebsiteScrape")]
        public async Task RunNextScrape([TimerTrigger("0 */10 18-1 * * *")] TimerInfo myTimer)
        {

            logger.LogInformation(EventConstants.PROCESS_WEBSITE_SCRAPE_START, $"RunNextScrape started");
            try
            {
                await main.RunNextWebsiteScrape(logger);
            }
            catch (Exception ex)
            {
                logger.LogError(EventConstants.PROCESS_WEBSITE_SCRAPE_ERROR, ex, "Error while processing RunNextScrape az function");
            }
            logger.LogInformation(EventConstants.PROCESS_WEBSITE_SCRAPE_END, $"RunNextScrape ended");
        }


        [FunctionName("SendGridCallback")]
        public async Task<HttpResponseMessage> SendGridCallback([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req)
        {
            try
            {
                logger.LogInformation("C# SendGridCallback: Started");
                logger.LogInformation("Reqest: " + req.Content.ReadAsStringAsync().Result);
                List<SendGridCallbackDTO> requests = JsonConvert.DeserializeObject<List<SendGridCallbackDTO>>(req.Content.ReadAsStringAsync().Result);

                foreach (var request in requests)
                {
                    await emailCallbackRepository.Add(request);
                }

                return req.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return req.CreateResponse(HttpStatusCode.NotFound, e);
            }

        }
    }
}
