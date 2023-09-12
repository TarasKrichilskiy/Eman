using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EManEmailMarketing.Storage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailsController : Controller
    {
        private readonly IEmailRepository emailRepository;
        public EmailsController(IEmailRepository emailRepository) 
        { 
            this.emailRepository= emailRepository;
        }

        [Route("")]
        public string Index()
        {
            return "This is the emails controller";
        }

        [Route("all")]
        public async Task<IEnumerable<Emails>> GetAllEmails()
        {
            return await emailRepository.GetAll();
        }
    }
}
