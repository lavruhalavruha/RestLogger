using System;
using System.Web.Http;

namespace RestLogger.WebApi.DI
{
    internal interface IDependencyResolver : IDisposable
    {
        void Configure(HttpConfiguration httpConfiguration);

        T Resolve<T>();
    }
}
