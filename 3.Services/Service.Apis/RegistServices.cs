
using Service.IApis;
using Microsoft.Extensions.DependencyInjection;

namespace Service.Apis
{
    public static class ServiceRegister
    {
        public static void RegistServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IReportService, ReportService>();

        }
    }
}