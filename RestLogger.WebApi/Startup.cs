using System;
using System.Configuration;
using System.Web.Http;

using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

using RestLogger.WebApi.DI;
using RestLogger.WebApi.Providers;

[assembly: OwinStartup(typeof(RestLogger.WebApi.Startup))]
namespace RestLogger.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            WebApiConfig.Register(config);

            IDependencyResolver dependencyResolver = new AutofacDependencyResolver();
            dependencyResolver.Configure(config);

            ConfigureOAuth(app, dependencyResolver);

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app, IDependencyResolver dependencyResolver)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = !bool.Parse(ConfigurationManager.AppSettings["ForceSecuredConnection"]),
                TokenEndpointPath = new PathString("/auth"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = dependencyResolver.Resolve<SimpleAuthorizationServerProvider>()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}