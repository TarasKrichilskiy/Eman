namespace EManEmailMarketing.EmailingManager.Services
{
    public interface IEmailService
    {
        Task SendEmailForClient(int? clientProgressId, string email);
        Task SendException(Exception e);
        Task<string> SendEmail(string Subject, string Body, string ToEmail, string ToName);
    }
}
