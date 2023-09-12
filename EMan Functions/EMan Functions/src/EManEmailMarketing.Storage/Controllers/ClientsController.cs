using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EManEmailMarketing.Storage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : Controller
    {
        private readonly IClientRepository clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }


        [Route("")]
        public string Index()
        {
            return "This is the clients controller";
        }

        [Route("all")]
        public async Task<IEnumerable<Client>> GetAllClients()
        {
            try
            {
                var clients = await clientRepository.GetSentEmailsForClient();
                return clients;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}

