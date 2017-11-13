using System;
using System.Threading.Tasks;

using FakeItEasy;
using NUnit.Framework;

using RestLogger.Domain;
using RestLogger.Infrastructure;
using RestLogger.Infrastructure.Helpers.PasswordHasher;
using RestLogger.Infrastructure.Helpers.PasswordHasher.Model;
using RestLogger.Infrastructure.Repository;
using RestLogger.Infrastructure.Service;
using RestLogger.Infrastructure.Service.Model.ApplicationDtos;
using RestLogger.Service;

namespace RestLogger.Tests.UnitTests
{
    [TestFixture]
    public class ApplicationServiceTests
    {
        [Test]
        public async Task WhenAddingApplicationThenApplicationIsAddedToRepository()
        {
            // Arrange
            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            A.CallTo(() => fakeApplicationRepository.TryGetByDisplayNameAsync(A<string>.Ignored)).Returns(Task.FromResult<ApplicationEntity>(null));

            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();

            IPasswordWithSaltHasher fakePasswordHasher = A.Fake<IPasswordWithSaltHasher>();
            A.CallTo(() => fakePasswordHasher.HashPassword(A<string>.Ignored)).Returns(new HashWithSaltResult()
            {
                Hash = "TestHash",
                Salt = "TestSalt"
            });

            IApplicationService applicationService = new ApplicationService(fakeApplicationRepository, fakeUnitOfWork, fakePasswordHasher);

            ApplicationCreateDto applicationCreateDto = new ApplicationCreateDto()
            {
                DisplayName = "TestApplicationDisplayName",
                Password = "TestPassword"
            };

            // Act
            await applicationService.AddApplicationAsync(applicationCreateDto);

            // Assert
            A.CallTo(() => fakeApplicationRepository.Add(A<ApplicationEntity>.That.Matches(a => a.DisplayName == "TestApplicationDisplayName"
                && a.PasswordHash == "TestHash"
                && a.PasswordSalt == "TestSalt"
            ))).MustHaveHappened();
        }

        [Test]
        public async Task WhenAddingApplicationThenUnitOfWorkIsCommitted()
        {
            // Arrange
            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            A.CallTo(() => fakeApplicationRepository.TryGetByDisplayNameAsync(A<string>.Ignored)).Returns(Task.FromResult<ApplicationEntity>(null));

            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();

            IPasswordWithSaltHasher fakePasswordHasher = A.Fake<IPasswordWithSaltHasher>();
            A.CallTo(() => fakePasswordHasher.HashPassword(A<string>.Ignored)).Returns(new HashWithSaltResult());

            IApplicationService applicationService = new ApplicationService(fakeApplicationRepository, fakeUnitOfWork, fakePasswordHasher);

            ApplicationCreateDto applicationCreateDto = new ApplicationCreateDto()
            {
                DisplayName = "TestApplicationDisplayName",
                Password = "TestPassword"
            };

            // Act
            await applicationService.AddApplicationAsync(applicationCreateDto);

            // Assert
            A.CallTo(() => fakeUnitOfWork.CommitAsync()).MustHaveHappened();
        }

        [Test]
        public void WhenAddingExistingApplicationThenExceptionIsThrown()
        {
            // Arrange
            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            A.CallTo(() => fakeApplicationRepository.TryGetByDisplayNameAsync(A<string>.Ignored)).Returns(Task.FromResult(new ApplicationEntity()));

            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();

            IPasswordWithSaltHasher fakePasswordHasher = A.Fake<IPasswordWithSaltHasher>();

            IApplicationService applicationService = new ApplicationService(fakeApplicationRepository, fakeUnitOfWork, fakePasswordHasher);

            ApplicationCreateDto applicationCreateDto = new ApplicationCreateDto()
            {
                DisplayName = "TestApplicationDisplayName",
                Password = "TestPassword"
            };

            // Act
            ArgumentException ex = Assert.CatchAsync<ArgumentException>(async () => await applicationService.AddApplicationAsync(applicationCreateDto));

            // Assert
            StringAssert.Contains("Application with DisplayName TestApplicationDisplayName already exists.", ex.Message);
        }

        [Test]
        public async Task WhenFindingExistingApplicationWithValidPasswordThenValidApplicationIsReturned()
        {
            // Arrange
            ApplicationEntity application = new ApplicationEntity()
            {
                Id = 0,
                DisplayName = "TestApplicationDisplayName",
                PasswordHash = "TestPasswordHash",
                PasswordSalt = "TestPasswordSalt"
            };

            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            A.CallTo(() => fakeApplicationRepository.TryGetByDisplayNameAsync(A<string>.Ignored)).Returns(Task.FromResult(application));

            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();

            IPasswordWithSaltHasher fakePasswordHasher = A.Fake<IPasswordWithSaltHasher>();
            A.CallTo(() => fakePasswordHasher.CheckPassword("TestPassword", "TestPasswordHash", "TestPasswordSalt")).Returns(true);

            IApplicationService applicationService = new ApplicationService(fakeApplicationRepository, fakeUnitOfWork, fakePasswordHasher);

            // Act
            ApplicationDto applicationDto = await applicationService.FindApplicationAsync("TestApplicationDisplayName", "TestPassword");

            // Assert
            Assert.IsNotNull(applicationDto);
            Assert.AreEqual(0, applicationDto.Id);
            Assert.AreEqual("TestApplicationDisplayName", applicationDto.DisplayName);
        }

        [Test]
        public async Task WhenFindingExistingApplicationWithWrongPasswordThenNullIsReturned()
        {
            // Arrange
            ApplicationEntity application = new ApplicationEntity()
            {
                Id = 0,
                DisplayName = "TestApplicationDisplayName",
                PasswordHash = "TestPasswordHash",
                PasswordSalt = "TestPasswordSalt"
            };

            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            A.CallTo(() => fakeApplicationRepository.TryGetByDisplayNameAsync("TestApplicationDisplayName")).Returns(Task.FromResult(application));

            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();

            IPasswordWithSaltHasher fakePasswordHasher = A.Fake<IPasswordWithSaltHasher>();
            A.CallTo(() => fakePasswordHasher.CheckPassword(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns(false);

            IApplicationService applicationService = new ApplicationService(fakeApplicationRepository, fakeUnitOfWork, fakePasswordHasher);

            // Act
            ApplicationDto applicationDto = await applicationService.FindApplicationAsync("TestApplicationDisplayName", "TestPassword");

            // Assert
            Assert.IsNull(applicationDto);
        }

        [Test]
        public async Task WhenFindingAbsentApplicationThenNullIsReturned()
        {
            // Arrange
            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            A.CallTo(() => fakeApplicationRepository.TryGetByDisplayNameAsync("TestApplicationDisplayName")).Returns(Task.FromResult<ApplicationEntity>(null));

            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();

            IPasswordWithSaltHasher fakePasswordHasher = A.Fake<IPasswordWithSaltHasher>();

            IApplicationService applicationService = new ApplicationService(fakeApplicationRepository, fakeUnitOfWork, fakePasswordHasher);

            // Act
            ApplicationDto applicationDto = await applicationService.FindApplicationAsync("TestApplicationId", "TestApplicationSecret");

            // Assert
            Assert.IsNull(applicationDto);
        }

        [Test]
        public void WhenAddingNullApplicationThenExceptionIsThrown()
        {
            // Arrange
            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();
            IPasswordWithSaltHasher fakePasswordHasher = A.Fake<IPasswordWithSaltHasher>();
            IApplicationService applicationService = new ApplicationService(fakeApplicationRepository, fakeUnitOfWork, fakePasswordHasher);

            // Act
            ArgumentException ex = Assert.CatchAsync<ArgumentException>(async () => await applicationService.AddApplicationAsync(null));

            // Assert
            StringAssert.Contains("Application name and password must be provided.", ex.Message);
        }

        [Test]
        public void WhenAddingApplicationWithEmptyDisplayNameThenExceptionIsThrown()
        {
            // Arrange
            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();
            IPasswordWithSaltHasher fakePasswordHasher = A.Fake<IPasswordWithSaltHasher>();
            IApplicationService applicationService = new ApplicationService(fakeApplicationRepository, fakeUnitOfWork, fakePasswordHasher);

            ApplicationCreateDto applicationCreateDto = new ApplicationCreateDto()
            {
                DisplayName = "",
                Password = "TestPassword"
            };

            // Act
            ArgumentException ex = Assert.CatchAsync<ArgumentException>(async () => await applicationService.AddApplicationAsync(applicationCreateDto));

            // Assert
            StringAssert.Contains("Application name and password must be provided.", ex.Message);
        }

        [Test]
        public void WhenAddingApplicationWithEmptyPasswordThenExceptionIsThrown()
        {
            // Arrange
            IApplicationRepository fakeApplicationRepository = A.Fake<IApplicationRepository>();
            IUnitOfWork fakeUnitOfWork = A.Fake<IUnitOfWork>();
            IPasswordWithSaltHasher fakePasswordHasher = A.Fake<IPasswordWithSaltHasher>();
            IApplicationService applicationService = new ApplicationService(fakeApplicationRepository, fakeUnitOfWork, fakePasswordHasher);

            ApplicationCreateDto applicationCreateDto = new ApplicationCreateDto()
            {
                DisplayName = "TestApplicationDisplayName",
                Password = ""
            };

            // Act
            ArgumentException ex = Assert.CatchAsync<ArgumentException>(async () => await applicationService.AddApplicationAsync(applicationCreateDto));

            // Assert
            StringAssert.Contains("Application name and password must be provided.", ex.Message);
        }
    }
}
