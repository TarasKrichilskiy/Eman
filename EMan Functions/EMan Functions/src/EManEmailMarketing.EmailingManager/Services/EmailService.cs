using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Linq.Expressions;
using System.Net.Mail;

namespace EManEmailMarketing.EmailingManager.Services
{
    public class EmailService : IEmailService
    {
        private readonly IClientProgressRepository clientProgressRepository;
        private readonly ISentEmailsRepository sentEmailsRepository;
        public EmailService(IClientProgressRepository clientProgressRepository, ISentEmailsRepository sentEmailsRepository)
        {
            this.clientProgressRepository = clientProgressRepository;
            this.sentEmailsRepository = sentEmailsRepository;
        }
        public async Task SendEmailForClient(int? clientProgressId, string email)
        {
            try {
                await clientProgressRepository.GetAll();
                var ClientProgress = await clientProgressRepository.Get((int)clientProgressId);
                var sendGridID = await SendEmail("Free Estimate for your HVAC Needs", ClientProgress.EmailHTMLTemplate, email, "");
                bool isScraped = false;
                sentEmailsRepository.Add(new SentEmail
                {
                    ClientID = ClientProgress.ClientID,
                    EmailID = !isScraped ? 1 : null,
                    FromRec = "emanemailmarketing@gmail.com",
                    SentDate = DateTime.UtcNow,
                    SendGridID = sendGridID,
                    ToEmail = email,
                    ScrapedEmailID = isScraped ? 1 : null,
                    HTMLBody = ClientProgress.EmailHTMLTemplate
                });
                await sentEmailsRepository.SaveChanges();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
            

        private string SendGridAPIKey = "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";
        private string FromEmail = "emanemailmarketing@gmail.com";

        public async Task SendException(Exception e)
        {
            var message = $"<html><div><font color=\"red\"><pre>Error With VMG IOS App</pre></font></div>";
            var count = 1;
            while (e != null)
            {
                message += $"<div><pre>\nError #{count++}</pre></div><div><pre>Type: {e.GetType()}</pre></div><div><pre>{e.Message}</pre></div><div><pre>{e.StackTrace}</pre></div>";
                e = e.InnerException;
            }
            message += "</html>";

            await SendEmail("VMG Error Email", message, "romankrich16@gmail.com", "Roman Krichilskiy");
        }

        public async Task<string> SendEmail(string Subject, string Body, string ToEmail, string ToName)
        {
            var client = new SendGridClient(SendGridAPIKey);
            var msg = new SendGridMessage();
            msg.From = new EmailAddress(FromEmail, "EMan");
            msg.AddTo(ToEmail, ToName);
            msg.Subject = Subject;
            msg.HtmlContent = Body;
            var result = await client.SendEmailAsync(msg);
            return result.DeserializeResponseHeaders(result.Headers)["X-Message-Id"];
        }
    }
}
