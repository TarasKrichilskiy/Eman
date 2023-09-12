using EManEmailMarketing.Storage.Models;

namespace EManEmailMarketing.Storage.Repositories.Interface
{
    public interface IScrapedEmailRepository : IRepository<ScrapedEmail>
    {
        /// <summary>
        /// Method to get next available email to send
        /// </summary>
        /// <returns>A ScrapedEmail to send</returns>
        Task<ScrapedEmail?> GetNext();

        /// <summary>
        /// Update an email to indicate it has been sent for Paul
        /// </summary>
        /// <param name="scrapedEmailID"></param>
        /// <returns></returns>
        Task UpdateEmailSentForPaul(int scrapedEmailID);

        /// <summary>
        /// Add new ScrapedEmail entry based on parameters
        /// </summary>
        /// <param name="email"> scraped email</param>
        /// <param name="website"> scraped from website</param>
        /// <param name="keyword"> scraped based on keyword</param>
        /// <returns></returns>
        Task Add(string email, string website, string keyword);
    }
}
