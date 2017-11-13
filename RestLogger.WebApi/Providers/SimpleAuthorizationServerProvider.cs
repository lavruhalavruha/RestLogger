using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.Owin.Security.OAuth;

using RestLogger.Infrastructure.Service;
using RestLogger.Infrastructure.Service.Model.ApplicationDtos;

namespace RestLogger.WebApi.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IApplicationService _applicationService;

        public SimpleAuthorizationServerProvider(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            ApplicationDto applicationDto = await _applicationService.FindApplicationAsync(context.UserName, context.Password);

            if (applicationDto == null)
            {
                context.SetError("invalid_grant", "Application Id or Secret is incorrect.");

                return;
            }

            ClaimsIdentity identity = new ClaimsIdentity(new User()
            {
                AuthenticationType = context.Options.AuthenticationType,
                IsAuthenticated = true,
                Name = context.UserName
            });

            context.Validated(identity);
        }
    }
}
