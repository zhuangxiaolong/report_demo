using Domain.Mapping;
using System.Threading;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data
{
    public partial class OpenApiContext : DbContextBase
    {
        private string _connStr;
        public OpenApiContext(string connectionString)
        : base(connectionString)
        {
            _connStr = connectionString;
           // Database.SetInitializer<OpenApiContext>(null);

            this.SetCommandTimeOut(180);
        }

        public OpenApiContext()
        : this("OpenApiContextConnectstring")
        {
            this.SetCommandTimeOut(180);
        }
        public void SetCommandTimeOut(int Timeout)
        {
            //var objectContext = (this as IObjectContextAdapter).ObjectContext;
           // objectContext.CommandTimeout = Timeout;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(_connStr);
            //return;
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connStr);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new InventoryMapping());
            modelBuilder.ApplyConfiguration(new log_report_buildMapping());
        }
    }


}