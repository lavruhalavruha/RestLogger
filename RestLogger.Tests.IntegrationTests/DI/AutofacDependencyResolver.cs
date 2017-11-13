using System.Linq;

using Autofac;

using RestLogger.Infrastructure.Helpers.PasswordHasher;
using RestLogger.Service;
using RestLogger.Service.Helpers.PasswordHasher;
using RestLogger.Storage;
using RestLogger.Storage.Repository;

namespace RestLogger.Tests.IntegrationTests.DI
{
    internal class AutofacDependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public AutofacDependencyResolver()
        {
            ContainerBuilder builder = new ContainerBuilder();

            RegisterTypes(builder);

            _container = builder.Build();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }

        private void RegisterTypes(ContainerBuilder builder)
        {
            // DatabaseFactory
            builder.RegisterType<DatabaseFactory>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(ApplicationRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // UnitOfWork
            builder.RegisterType<UnitOfWork>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Services
            builder.RegisterAssemblyTypes(typeof(ApplicationService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Password hasher
            builder.RegisterType< PasswordWithSaltHasherSha512>()
                .As<IPasswordWithSaltHasher>()
                .InstancePerLifetimeScope();
        }
    }
}
