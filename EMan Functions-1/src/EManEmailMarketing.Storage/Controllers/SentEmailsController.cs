using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EManEmailMarketing.Storage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SentEmailsController : Controller
    {
        private readonly ISentEmailsRepository sentEmailsRepository;
        public SentEmailsController(ISentEmailsRepository sentEmailsRepository)
        {
            this.sentEmailsRepository = sentEmailsRepository;
        }

        [Route("")]
        public string Index()
        {
            return "this is the sent emails controller";
        }

        [Route("all")]
        public async Task<IEnumerable<SentEmail>> GetAllSentEmails()
        {
            //return await context.ClientsProgress.ToListAsync();

            return await sentEmailsRepository.GetAll();
        }
    }
}
