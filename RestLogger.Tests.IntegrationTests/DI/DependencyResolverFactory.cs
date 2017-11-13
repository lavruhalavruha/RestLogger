namespace RestLogger.Tests.IntegrationTests.DI
{
    internal static class DependencyResolverFactory
    {
        public static IDependencyResolver CreateDependencyResolver()
        {
            return new AutofacDependencyResolver();
        }
    }
}
