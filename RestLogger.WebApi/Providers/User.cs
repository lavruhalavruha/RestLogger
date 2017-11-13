using System.Security.Principal;

namespace RestLogger.WebApi.Providers
{
    public class User : IIdentity
    {
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }
    }
}
