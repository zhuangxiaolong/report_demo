using System;
using Autofac;

namespace Domain.Core.Dependency
{
    public static class ServiceRegister
    {
        public static void ServiceRegist<TService, TProtocol>(this ContainerBuilder builder)
            where TService : IDependency
            where TProtocol : IDependency
        {
            builder.RegisterType<TService>().As<TProtocol>();
        }

        public static void ServiceRegist<TService>(this ContainerBuilder builder)
            where TService : IDependency
        {
            builder.RegisterType<TService>();
        }

        public static void ServiceRegist<TProtocol>(this ContainerBuilder builder, Func<IComponentContext, TProtocol> func)
            where TProtocol : IDependency
        {
            builder.Register(func);
        }
    }
}