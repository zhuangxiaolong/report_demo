using System;
using Autofac;
using Domain.Core.Cache;
using Domain.Core.Configuration;
using Domain.Core.Dependency;
using Domain.Core.Handlers;
using Domain.Core.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Core
{
    public static class ServiceRegister
    {
        /// <summary>
        /// 注册断路器
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="exceptionCountResolver"></param>
        /// <param name="durationOfBreakResolver"></param>
        /// <param name="timeoutValueResolver"></param>
        public static void RegistCircuitBreadkingHandler(this ContainerBuilder builder,
                                                         Func<IComponentContext, int> exceptionCountResolver,
                                                         Func<IComponentContext, TimeSpan> durationOfBreakResolver,
                                                         Func<IComponentContext, TimeSpan> timeoutValueResolver)
        {
            builder.ServiceRegist<StringCircuitBreakingHandler>();
            builder.ServiceRegist<ICircuitBreakingHandler<string>>(e =>
            {
                var cc = e.Resolve<IComponentContext>();
                var exceptionTimesParam = new NamedParameter("exceptionsAllowedBeforeBreaking", exceptionCountResolver(cc));
                var durationOfBreakParam = new NamedParameter("durationOfBreak", durationOfBreakResolver(cc));
                var timeoutValueParam = new NamedParameter("timeoutValue", timeoutValueResolver(cc));
                var result = cc.Resolve<StringCircuitBreakingHandler>(exceptionTimesParam, durationOfBreakParam, timeoutValueParam);
                return result;
            });
        }

        public static void RegistAspNetConfigurationProvider(this ContainerBuilder builder)
        {
            builder.ServiceRegist<AspNetCoreConfigProvider, IConfigurationProvider>();
        }

        public static void RegistAppConfigConfigurationProvider(this ContainerBuilder builder)
        {
            builder.ServiceRegist<AppConfigProvider, IConfigurationProvider>();
        }
        public static void RegistResponseCacheInterceptor(this ContainerBuilder builder)
        {
            builder.Register(e =>
            {
                var cc = e.Resolve<IComponentContext>();
                var cacheClient = cc.Resolve<ICacheClient>();
                var clientParameter = new TypedParameter(typeof(ICacheClient), cacheClient);
                return cc.Resolve<ResponseCacheInterceptor>(clientParameter);
            });
        }

        public static void RegistResponseCacheInterceptor(this IServiceCollection services)
        {
            services.AddTransient(e =>
            {
                var cacheClient = e.GetRequiredService<Lazy<ICacheClient>>();
                return new ResponseCacheInterceptor(cacheClient);
            });
        }

    }
}