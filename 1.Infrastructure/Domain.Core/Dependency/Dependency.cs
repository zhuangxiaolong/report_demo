using System;
using Exceptionless.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Core.Dependency
{
    public class ExceptionResolver : IDependencyResolver
    {
        public static IServiceProvider Provider;
        public static IServiceCollection Services;
        public ExceptionResolver()
        {

        }

        public void Dispose()
        {

        }

        public void Register(Type serviceType, Type concreteType)
        {
            Services.AddTransient(serviceType, concreteType);
        }

        public void Register(Type serviceType, Func<object> activator)
        {
            Services.AddTransient(serviceType, e => activator.Invoke());
        }

        public object Resolve(Type serviceType)
        {
            return Provider.GetService(serviceType);
        }
    }
}