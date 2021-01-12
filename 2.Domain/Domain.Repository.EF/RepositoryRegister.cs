using Domain.Data;
using Domain.Models;
using Domain.Repository.EF.Basic;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Repository.EF
{
    public static class RepositoryRegister
    {
        public static void RegistRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepository<UserInfo, int>, UserRepository>();
            services.AddScoped<IRepository<Inventory, long>, InventoryRepository>();
            services.AddScoped<IRepository<log_report_build, int>, log_report_buildRepository>();
        }
    }
}