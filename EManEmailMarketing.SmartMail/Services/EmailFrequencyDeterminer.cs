using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace EManEmailMarketing.SmartMail.Services
{
    public class EmailFrequencyDeterminer : IEmailFrequencyDeterminer
    {
        private readonly IClientProgressRepository clientProgressRepository;
        private readonly ILogger<EmailFrequencyDeterminer> logger;
        public EmailFrequencyDeterminer(IClientProgressRepository clientProgressRepository, ILogger<EmailFrequencyDeterminer> logger) 
        {
            this.clientProgressRepository = clientProgressRepository ?? throw new ArgumentNullException(nameof(clientProgressRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<int> GetEmailFrequency(int clientId, DateTime currentDay)
        {
            try
            {
                int daysLeftInMonth = DateTime.DaysInMonth(currentDay.Year, currentDay.Month) - currentDay.Day + 1;
                var clientProgress = await clientProgressRepository.GetCurrentClientProgressByClientId(clientId);
                if (clientProgress == null) return 0; // client does not have a plan in place
                if (clientProgress.Clicks >= clientProgress.ClicksGoal || clientProgress.Opens >= clientProgress.OpensGoal) return 0; // goal reached
                int currentWeek = (currentDay.Day - 1) / 7 + 1;
                int emailsToSendTodayBasedOnClicks = ((clientProgress.ClicksGoal - clientProgress.Clicks) / daysLeftInMonth) * (5 - currentWeek);
                int emailsToSendTodayBasedOnOpens = ((clientProgress.OpensGoal - clientProgress.Opens) / daysLeftInMonth) * (5 - currentWeek);
                return emailsToSendTodayBasedOnClicks;
                // TODO: do we need to consider opens or clicks or both?
            }
            catch (Exception ex)
            {
                // TODO: log
                throw;
            }
        }
    }
}
