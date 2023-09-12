using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EManEmailMarketing.Storage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientProgressController : Controller
    {
        private readonly IClientProgressRepository clientProgressRepository;
        public ClientProgressController(IClientProgressRepository clientProgressRepository) 
        {
            this.clientProgressRepository = clientProgressRepository;
        }

        [Route("")]
        public string Index()
        {
            return "this is the clients progress controller";
        }

        [Route("all")]
        public async Task<IEnumerable<ClientProgress>>  GetAllClientProgress()
        {
            return await clientProgressRepository.GetAll();
        }

        [Route("1")]
        public async Task<ClientProgress> GetByIdof1()
        {
            return await clientProgressRepository.Get(1);
        }
    }
}
