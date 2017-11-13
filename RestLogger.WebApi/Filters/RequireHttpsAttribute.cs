using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

namespace RestLogger.WebApi.Filters
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        private readonly bool _isActive;

        public RequireHttpsAttribute()
        {
            _isActive = bool.Parse(ConfigurationManager.AppSettings["ForceSecuredConnection"]);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!_isActive)
            {
                base.OnAuthorization(actionContext);

                return;
            }

            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "HTTPS Required"
                };
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}
