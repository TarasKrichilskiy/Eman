using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EManEmailMarketing.SmartMail.Services;
using System;

namespace EManEmailMarketing.SmartMail.Controllers
{
    [ApiController]
    [Route("api")]
    public class EmailFrequencyDeterminerController : ControllerBase
    {
        private readonly IEmailFrequencyDeterminer emailFrequencyDeterminer;

        public EmailFrequencyDeterminerController(IEmailFrequencyDeterminer emailFrequencyDeterminer)
        {
            this.emailFrequencyDeterminer = emailFrequencyDeterminer;
        }

        [Route("")]
        public async Task<int> Get()
        {
            return await emailFrequencyDeterminer.GetEmailFrequency(2, DateTime.UtcNow);
        }
    }
}
