using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using EManEmailMarketing.SmartMail.Services;
using EManEmailMarketing.Storage.Models;
using Microsoft.OpenApi.Any;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.Extensions.Logging;

namespace EManEmailMarketing.SmartMail.UnitTests.Services
{
    [TestClass]
    public class EmailFrequencyDeterminerTests
    {
        private readonly EmailFrequencyDeterminer emailFrequencyDeterminer;
        private readonly Mock<IClientProgressRepository> clientProgressRepository;
        private readonly Mock<ILogger<EmailFrequencyDeterminer>> logger;

        public EmailFrequencyDeterminerTests()
        {
            logger = new Mock<ILogger<EmailFrequencyDeterminer>>();
            clientProgressRepository = new Mock<IClientProgressRepository>();
            emailFrequencyDeterminer = new EmailFrequencyDeterminer(clientProgressRepository.Object, logger.Object);
        }

        [TestMethod]
        public void VerifyEmailFrequencyDeterminerDoesNotReturnNegetive()
        {
            // Arrange
            var clientProgress = new ClientProgress
            {
                StartDate = new DateTime(year: 2023, month: 3, day: 2),
                EndDate = new DateTime(year: 2023, month: 4, day: 2),
                Clicks = 0,
                Opens = 0,
                ClicksGoal = 50,
                OpensGoal = 100
            };

            clientProgressRepository.Setup( cpr => cpr.GetCurrentClientProgressByClientId(It.IsAny<int>())).Returns(Task.FromResult(clientProgress));

            // Act
            var result = emailFrequencyDeterminer.GetEmailFrequency(It.IsAny<int>(), DateTime.UtcNow).GetAwaiter().GetResult();

            // Assert
            Assert.IsTrue(result >= 0);
            
        }

        [TestMethod]
        public void VerifyEmailFrequencyReturnsZeroWhenClickGoalReached()
        {
            // Arrange
            var clientProgress = new ClientProgress
            {
                Clicks = 50,
                ClicksGoal = 50,
                Opens = 10,
                OpensGoal = 100,
            };

            clientProgressRepository.Setup(cpr => cpr.GetCurrentClientProgressByClientId(It.IsAny<int>())).Returns(Task.FromResult(clientProgress));

            // Act
            var result = emailFrequencyDeterminer.GetEmailFrequency(It.IsAny<int>(), DateTime.UtcNow).GetAwaiter().GetResult();

            // Assert
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void VerifyEmailFrequencyReturnsZeroWhenOpensGoalReached()
        {
            // Arrange
            var clientProgress = new ClientProgress
            {
                Opens = 100,
                OpensGoal = 100,
                Clicks = 10,
                ClicksGoal = 50
            };

            clientProgressRepository.Setup(cpr => cpr.GetCurrentClientProgressByClientId(It.IsAny<int>())).Returns(Task.FromResult(clientProgress));

            // Act
            var result = emailFrequencyDeterminer.GetEmailFrequency(It.IsAny<int>(), DateTime.UtcNow).GetAwaiter().GetResult();

            // Assert
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void VerifyEmailFrequencyReturnsZeroWhenGoalIsZero()
        {
            // Arrange
            var clientProgress = new ClientProgress
            {
               ClicksGoal = 0,
               OpensGoal = 0,
               Clicks = 0,
               Opens = 0
            };

            clientProgressRepository.Setup(cpr => cpr.GetCurrentClientProgressByClientId(It.IsAny<int>())).Returns(Task.FromResult(clientProgress));

            // Act
            var result = emailFrequencyDeterminer.GetEmailFrequency(It.IsAny<int>(), DateTime.UtcNow).GetAwaiter().GetResult();

            // Assert
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void VerifyEmailFrequencyReturnsZeroWhenClientProgressDoesNotExist()
        {
            // Arrange
            ClientProgress clientProgress = null;

            clientProgressRepository.Setup(cpr => cpr.GetCurrentClientProgressByClientId(It.IsAny<int>())).Returns(Task.FromResult(clientProgress));

            // Act
            var result = emailFrequencyDeterminer.GetEmailFrequency(It.IsAny<int>(), DateTime.UtcNow).GetAwaiter().GetResult();

            // Assert
            Assert.IsTrue(result == 0);
        }
    }
}
