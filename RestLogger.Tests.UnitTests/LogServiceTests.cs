using System;
using System.Threading.Tasks;

using FakeItEasy;
using NUnit.Framework;

using RestLogger.Domain;
using RestLogger.Infrastructure;
using RestLogger.Infrastructure.Repository;
using RestLogger.Infrastructure.Service;
using RestLogger.Infrastructure.Service.Model.LogDtos;
using RestLogger.Service;

namespace RestLogger.Tests.UnitTests
{
    [TestFixture]
    public class LogServiceTests
    {
        [Test]
        public async Task WhenAddingLogWithValidApplicationThenLogIsAddedToRepository()
        {
            // Arrange
            ILogRepository fakeLogRepository = A.Fake<ILogRepository>();

            ApplicationEntity application = new ApplicationEntity()
            {
                DisplayName = "TestApplicationDisplayName"
            };

            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            A.CallTo(() => fakeApplicationRepository.TryGetByIdAsync(1)).Returns(Task.FromResult(application));

            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();

            LogCreateDto logCreateDto = new LogCreateDto()
            {
                ApplicationId = 1,
                Logger = "TestLogger",
                Level = "TestLevel",
                Message = "TestMessage"
            };

            ILogService logService = new LogService(fakeLogRepository, fakeApplicationRepository, fakeUnitOfWork);

            // Act
            await logService.AddLogAsync(logCreateDto, "TestApplicationDisplayName");

            // Assert
            A.CallTo(() => fakeLogRepository.Add(A<LogEntity>.That.Matches(l => l.Logger == "TestLogger"
                && l.Level == "TestLevel"
                && l.Message == "TestMessage"
                && l.ApplicationId == 1
            ))).MustHaveHappened();
        }

        [Test]
        public async Task WhenAddingLogWithValidApplicationThenUnitOfWorkIsCommitted()
        {
            // Arrange
            ILogRepository fakeLogRepository = A.Fake<ILogRepository>();

            ApplicationEntity application = new ApplicationEntity()
            {
                DisplayName = "TestApplicationDisplayName"
            };

            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            A.CallTo(() => fakeApplicationRepository.TryGetByIdAsync(1)).Returns(Task.FromResult(application));

            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();

            LogCreateDto logCreateDto = new LogCreateDto()
            {
                ApplicationId = 1,
                Logger = "TestLogger",
                Level = "TestLevel",
                Message = "TestMessage"
            };

            ILogService logService = new LogService(fakeLogRepository, fakeApplicationRepository, fakeUnitOfWork);

            // Act
            await logService.AddLogAsync(logCreateDto, "TestApplicationDisplayName");

            // Assert
            A.CallTo(() => fakeUnitOfWork.CommitAsync()).MustHaveHappened();
        }

        [Test]
        public void WhenAddingLogWithAbsentApplicationThenExceptionIsThrown()
        {
            // Arrange
            ILogRepository fakeLogRepository = A.Fake<ILogRepository>();

            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            A.CallTo(() => fakeApplicationRepository.TryGetByIdAsync(A<int>.Ignored)).Returns(Task.FromResult<ApplicationEntity>(null));

            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();

            LogCreateDto logCreateDto = new LogCreateDto()
            {
                ApplicationId = 1,
                Logger = "TestLogger",
                Level = "TestLevel",
                Message = "TestMessage"
            };

            ILogService logService = new LogService(fakeLogRepository, fakeApplicationRepository, fakeUnitOfWork);

            // Act
            ArgumentException ex = Assert.CatchAsync<ArgumentException>(async () => await logService.AddLogAsync(logCreateDto, "TestApplicationDisplayName"));

            // Assert
            StringAssert.Contains("Application with id 1 not found.", ex.Message);
        }

        [Test]
        public void WhenAddingLogWithWrongApplicationDisplayNameThenExceptionIsThrown()
        {
            // Arrange
            ILogRepository fakeLogRepository = A.Fake<ILogRepository>();

            ApplicationEntity application = new ApplicationEntity()
            {
                DisplayName = "TestApplicationDisplayName"
            };

            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            A.CallTo(() => fakeApplicationRepository.TryGetByIdAsync(1)).Returns(Task.FromResult(application));

            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();

            LogCreateDto logCreateDto = new LogCreateDto()
            {
                ApplicationId = 1,
                Logger = "TestLogger",
                Level = "TestLevel",
                Message = "TestMessage"
            };

            ILogService logService = new LogService(fakeLogRepository, fakeApplicationRepository, fakeUnitOfWork);

            // Act
            ArgumentException ex = Assert.CatchAsync<ArgumentException>(async () => await logService.AddLogAsync(logCreateDto, "WrongTestApplicationDisplayName"));

            // Assert
            StringAssert.Contains("Application display name mismatch.", ex.Message);
        }
    }
}
