using System;
using System.Reflection;
using System.Web.Http;

using Autofac;
using Autofac.Integration.WebApi;

using RestLogger.Infrastructure.Helpers.PasswordHasher;
using RestLogger.Service;
using RestLogger.Service.Helpers.PasswordHasher;
using RestLogger.Storage;
using RestLogger.Storage.Repository;
using RestLogger.WebApi.Providers;

namespace RestLogger.WebApi.DI
{
    internal class AutofacDependencyResolver : IDependencyResolver
    {
        private IContainer _container = null;

        public void Configure(HttpConfiguration httpConfiguration)
        {
            ContainerBuilder builder = new ContainerBuilder();

            RegisterTypes(builder);

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            if (_container != null)
            {
                _container.Dispose();
                _container = null;
            }

            _container = builder.Build();

            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(_container);
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

            // SimpleAuthorizationServerProvider
            builder.RegisterType<SimpleAuthorizationServerProvider>()
                .AsSelf()
                .SingleInstance();
        }

        public T Resolve<T>()
        {
            if (_container == null)
            {
                throw new Exception("Dependency resolver is not configured.");
            }

            return _container.Resolve<T>();
        }

        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
                _container = null;
            }
        }
    }
}
