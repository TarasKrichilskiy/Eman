using System;
using System.Threading.Tasks;

namespace EManEmailMarketing.SmartMail.Services
{

    /// <summary>
    /// Given a client, return the number of emails that still need to be sent out today to reach goal
    /// </summary>
    /// <param name="clientId"> A unique id belonging to a client</param>
    /// <param name="currentDay"> the day to work off of</param>
    /// <returns> Then number of emails that should be sent out today </returns>
    public interface IEmailFrequencyDeterminer
    {
        Task<int> GetEmailFrequency(int client, DateTime currentDay);
    }
}
