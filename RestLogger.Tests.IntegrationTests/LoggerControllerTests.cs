using System;
using System.Threading.Tasks;
using System.Web.Http.Results;

using NUnit.Framework;

using RestLogger.Infrastructure.Service;
using RestLogger.Tests.IntegrationTests.DI;
using RestLogger.WebApi.Controllers;
using RestLogger.WebApi.Models.ApplicationModels;
using RestLogger.WebApi.Models.LogModels;

namespace RestLogger.Tests.IntegrationTests
{
    [TestFixture]
    public class LoggerControllerTests
    {
        [Test]
        public async Task WhenRegisteringApplicationThenApplicationIsRegistered()
        {
            using (IDependencyResolver resolver = DependencyResolverFactory.CreateDependencyResolver())
            {
                LoggerController controller = new LoggerController(resolver.Resolve<IApplicationService>(), resolver.Resolve<ILogService>());

                ApplicationCreateModel applicationCreateModel = new ApplicationCreateModel()
                {
                    DisplayName = GenerateDisplayName(),
                    Password = "TestPassword"
                };

                OkNegotiatedContentResult<int> result = await controller.RegisterAsync(applicationCreateModel) as OkNegotiatedContentResult<int>;
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public async Task WhenRegisteringExistingApplicationThenErrorShouldBeRaised()
        {
            using (IDependencyResolver resolver = DependencyResolverFactory.CreateDependencyResolver())
            {
                LoggerController controller = new LoggerController(resolver.Resolve<IApplicationService>(),
                    resolver.Resolve<ILogService>());

                string displayName = GenerateDisplayName();

                ApplicationCreateModel applicationCreateModel = new ApplicationCreateModel()
                {
                    DisplayName = displayName,
                    Password = "TestPassword"
                };

                OkNegotiatedContentResult<int> result = await controller.RegisterAsync(applicationCreateModel) as OkNegotiatedContentResult<int>;
                Assert.IsNotNull(result);

                BadRequestErrorMessageResult newResult = await controller.RegisterAsync(applicationCreateModel) as BadRequestErrorMessageResult;
                Assert.IsNotNull(newResult);
                Assert.AreEqual(newResult.Message, $"Application with DisplayName {displayName} already exists.");
            }
        }

        [Test]
        public async Task WhenLoggingWithBadApplicationIdThenErrorShouldBeRaised()
        {
            using (IDependencyResolver resolver = DependencyResolverFactory.CreateDependencyResolver())
            {
                LoggerController controller = new LoggerController(resolver.Resolve<IApplicationService>(),
                    resolver.Resolve<ILogService>());

                LogCreateModel logCreateModel = new LogCreateModel()
                {
                    ApplicationId = 0,  // bad application_id
                    Logger = "logger",
                    Level = "level",
                    Message = "message"
                };

                var result = await controller.LogAsync(logCreateModel) as OkNegotiatedContentResult<LogCreationResultModel>;
                Assert.AreEqual(result.Content.success, false);
            }
        }

        private string GenerateDisplayName()
        {
            string testPrefix = "TEST_";
            string guidStr = Guid.NewGuid().ToString().Replace("-", string.Empty);
            string testGuidStr = testPrefix + guidStr.Substring(testPrefix.Length);

            return testGuidStr;
        }
    }
}
