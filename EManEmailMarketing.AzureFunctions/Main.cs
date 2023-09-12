using Email_Scraper;
using EManEmailMarketing.Scraper.Interfaces;
using EManEmailMarketing.Scraper.Services;
using EManEmailMarketing.SmartMail.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.EmailingManager.Services;
using EManEmailMarketing.Storage.Repositories.Interface;
using EManEmailMarketing.Storage.Repositories;
using EManEmailMarketing.Common.DTO;
using static System.Net.Mime.MediaTypeNames;
using EllipticCurve.Utils;
using AngleSharp.Text;

namespace EManEmailMarketing
{
    public class Main : IMain
    {
        private readonly IEmailFrequencyDeterminer emailFrequencyDeterminer;
        private readonly IWebsiteScraper websiteScraper;
        private readonly IEmailService emailService;
        private readonly IClientProgressRepository clientProgressRepository;
        private readonly IClientRepository clientRepository;
        private readonly ISentEmailsRepository sentEmailsRepository;
        private readonly IKeyWordRepository keyWordRepository;
        private readonly IEmailRepository emailRepository;
        private readonly IScrapedEmailRepository scrapedEmailRepository;
        private readonly IScrapedWebsiteRepository scrapedWebsiteRepository;
        private readonly IScrapedEmailLocationRepository scrapedEmailLocationRepository;
        private readonly IEmailCallbackRepository emailCallbackRepository;
        private readonly ILogger logger;

        public Main(
            IEmailFrequencyDeterminer emailFrequencyDeterminer, 
            IWebsiteScraper websiteScraper,
            IEmailService emailService,
            IClientProgressRepository clientProgressRepository,
            IClientRepository clientRepository,
            ISentEmailsRepository sentEmailsRepository,
            IKeyWordRepository keyWordRepository,
            IEmailRepository emailRepository,
            IScrapedEmailRepository scrapedEmailRepository,
            IScrapedWebsiteRepository scrapedWebsiteRepository,
            IScrapedEmailLocationRepository scrapedEmailLocationRepository,
            IEmailCallbackRepository emailCallbackRepository,
            ILogger<Main> logger)
        {
            this.emailFrequencyDeterminer = emailFrequencyDeterminer ?? throw new ArgumentNullException(nameof(emailFrequencyDeterminer));
            this.websiteScraper = websiteScraper ?? throw new ArgumentNullException(nameof(websiteScraper));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            this.clientProgressRepository = clientProgressRepository ?? throw new ArgumentNullException(nameof(clientProgressRepository));
            this.clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            this.sentEmailsRepository = sentEmailsRepository ?? throw new ArgumentNullException(nameof(sentEmailsRepository)); 
            this.keyWordRepository = keyWordRepository ?? throw new ArgumentNullException(nameof(keyWordRepository));
            this.emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));
            this.scrapedEmailRepository = scrapedEmailRepository ?? throw new ArgumentNullException(nameof(scrapedEmailRepository));
            this.scrapedWebsiteRepository = scrapedWebsiteRepository ?? throw new ArgumentNullException(nameof(scrapedWebsiteRepository));
            this.scrapedEmailLocationRepository = scrapedEmailLocationRepository ?? throw new ArgumentNullException(nameof(scrapedEmailLocationRepository));
            this.emailCallbackRepository = emailCallbackRepository ?? throw new ArgumentNullException(nameof(emailCallbackRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessEmailRequest(int? clientProgressID, string email)
        {
            var clientProgress = await clientProgressRepository.GetCurrentClientProgressByClientId(3);
            var stateFilter = await clientRepository.GetClientStateFilter(3);

            var result = await clientRepository.GetSentEmailsForClient();
            var nextEmail = await emailRepository.GetNextEmail(3, stateFilter);

            //if(clientProgressID == null)
            //{
            //    db.GetNextEmail();
            //}
            //var ClientProgress = await clientProgressRepository.GetCurrentClientProgressByClientId((int)clientProgressID);
            //var sendGridID = await emailService.SendEmail("Free Estimate for your Construction Needs", ClientProgress.EmailHTMLTemplate, email, "");
            //await sentEmailsRepository.Add(ClientProgress, sendGridID, 1, email);
        }

        public async Task ProcessEmailForClient(int clientid)
        {
            var clientProgress = await clientProgressRepository.GetCurrentClientProgressByClientId(clientid);
            var stateFilter = await clientRepository.GetClientStateFilter(clientid);
            var nextEmail = await emailRepository.GetNextEmail(clientid, stateFilter);
            var sendGridID = await emailService.SendEmail(clientProgress.EmailSubject, clientProgress.EmailHTMLTemplate, nextEmail.Email, GetNameToUse(nextEmail));
            await sentEmailsRepository.Add(clientProgress, sendGridID, nextEmail.EmailID, nextEmail.Email, false);
        }

        public async Task ProcessPaulsEmailNext()
        {

            for (var a = 0; a < 2; a++) //do 2 emails in one job
            {
                try
                {
                    var next = await emailRepository.GetNextEmail("SendForPaul");
                    if (next != null)
                    {
                        var ClientProgress = await clientProgressRepository.GetCurrentClientProgressByClientId(1);
                        var sendGridID = await emailService.SendEmail("Free Estimate for your HVAC Needs", ClientProgress.EmailHTMLTemplate, next.Email, GetNameToUse(next));

                        await emailRepository.UpdateEmailSentForPaul(next.EmailID, "SendForPaul");
                        await sentEmailsRepository.Add(ClientProgress, sendGridID, next.EmailID, next.Email, false);
                    }
                }
                catch (Exception e)
                {
                    await emailService.SendException(e);
                }
            }

            for (var a = 0; a < 3; a++) //do 1 for PSG Contractors
            {
                try
                {
                    var next = await emailRepository.GetNextEmail("SendForPSGcontractors");
                    if (next != null)
                    {
                        var ClientProgress = await clientProgressRepository.GetCurrentClientProgressByClientId(2);
                        var sendGridID = await emailService.SendEmail("Free Estimate for your Construction Needs", ClientProgress.EmailHTMLTemplate, next.Email, GetNameToUse(next));

                        await emailRepository.UpdateEmailSentForPaul(next.EmailID, "SendForPSGcontractors");
                        await sentEmailsRepository.Add(ClientProgress, sendGridID, next.EmailID, next.Email, false);
                    }
                }
                catch (Exception e)
                {
                    await emailService.SendException(e);
                }
            }

            for (var a = 0; a < 5; a++) //do five scraped emails in one job
            {
                try
                {

                    var next = await scrapedEmailRepository.GetNext();
                    if (next != null)
                    {
                        var ClientProgress = await clientProgressRepository.GetCurrentClientProgressByClientId(1);
                        var sendGridID = await emailService.SendEmail("Free Estimate for your HVAC Needs", ClientProgress.EmailHTMLTemplate, next.Email, "");

                        await scrapedEmailRepository.UpdateEmailSentForPaul(next.ScrapedEmailID);
                        await sentEmailsRepository.Add(ClientProgress, sendGridID, next.ScrapedEmailID, next.Email, true);
                    }
                }
                catch (Exception e)
                {
                    await emailService.SendException(e);
                }
            }
        }

        private string GetNameToUse(Emails next)
        {
            var nameToUse = "";
            if (!string.IsNullOrWhiteSpace(next.FirstName) && !string.IsNullOrWhiteSpace(next.LastName))
            {
                nameToUse = next.FirstName + " " + next.LastName;
            }
            else if (!string.IsNullOrWhiteSpace(next.Company))
            {
                nameToUse = next.Company;
            }
            return nameToUse;
        }

        public async Task RunNextGoogleSearch()
        {
            var nextWord = await keyWordRepository.GetNextKeyWord();
            logger.LogInformation($"Key Word Used {nextWord}");

            nextWord = nextWord.Trim() + " in Marysville WA";
            var results = await websiteScraper.RunGoogleSeach(nextWord);
            logger.LogInformation($"Google Results Found Total {results.Count}");

            foreach(var url in results)
            {
                try
                {
                    await scrapedWebsiteRepository.Add(url, nextWord);
                    logger.LogInformation($"Url Inserted {url}");
                }
                catch (Exception e)
                {
                    logger.LogError($"Url Failed Inserted {url}");
                }
            }
        }

        public async Task ProcessCallback(ILogger log, string req)
        {
            var ipGeoLocator = new IPGeoLocator();
            List<SendGridCallbackDTO> request = JsonConvert.DeserializeObject<List<SendGridCallbackDTO>>(req);

            foreach (var requst in request)
            {
                try
                {
                    var scrapedEmailID = await emailCallbackRepository.Add(requst);
                    if (scrapedEmailID != null && (requst.Event == "click" || requst.Event == "open")) //IP in open and click only works for location
                    {
                        var result = await ipGeoLocator.GetLocationDetails(requst.ip);
                        if(result != null)
                        {
                            LocationDetailsDTO locationDetails = JsonConvert.DeserializeObject<LocationDetailsDTO>(result);
                            await scrapedEmailLocationRepository.Add((int)scrapedEmailID, locationDetails);
                        }
                    }
                }
                catch (Exception e)
                {
                    log.LogError($"Error: {e.Message}");
                    log.LogError($"Error Stack: {e.StackTrace}");
                }
            }
        }

        public async Task RunNextWebsiteScrape(ILogger log)
        {
            var nextWebsite = await scrapedWebsiteRepository.GetNext();
            log.LogInformation($"Start Scrape For {nextWebsite.Url}");

            try
            {
                var websiteFindings = await websiteScraper.RunWebsiteScrape(nextWebsite.Url);

                if (websiteFindings != null)
                {
                    log.LogInformation($"Website Other Pages Count {websiteFindings.OtherRoutes.Count}");
                    log.LogInformation($"Website Found Emails Count {websiteFindings.Emails.Count}");

                    await InsertScrapedEmailsToDB(websiteFindings, nextWebsite.KeyWordUsed);
                }
            }
            catch (Exception e)
            {
                log.LogError($"Error on scrape of url: {nextWebsite.Url}");
            }
        }

        private async Task InsertScrapedEmailsToDB(UrlResultsDTO finding, string keyword)
        {
            foreach(var email in finding.Emails)
            {
                try
                {
                    await scrapedEmailRepository.Add(email, finding.CurrentUrl, keyword);
                }
                catch (Exception e)
                {
                    // SQL has uneque constraint on email column so this will hit exception often
                }
            }

            foreach (var nextFinding in finding.OtherRoutes)
            {
                await InsertScrapedEmailsToDB(nextFinding, keyword);
            }
        }
    }
}
