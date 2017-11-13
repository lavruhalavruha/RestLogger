using System;

namespace RestLogger.Tests.IntegrationTests.DI
{
    internal interface IDependencyResolver : IDisposable
    {
        T Resolve<T>();
    }
}
