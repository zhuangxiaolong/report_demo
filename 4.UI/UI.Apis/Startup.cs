using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain.Core;
using Domain.Core.Cache;
using Domain.Core.Dependency;
using Domain.Core.Http;
using Domain.Core.Middlewares;
using Domain.Data;
using Domain.Repository.EF;
using Exceptionless;
using Exceptionless.Dependency;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.Redis;
using Hangfire.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Service.Apis;
using UI.Assemblers;
using UI.Core;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using IContainer = Autofac.IContainer;

namespace UI.Apis
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
#if DEV
                .AddJsonFile("appsettings.dev.json", false, true)
#endif
#if UAT
                .AddJsonFile("appsettings.uat.json", false, true)
#endif
#if PROD
                .AddJsonFile("appsettings.prod.json", false, true) 
#endif
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        public IContainer Container { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));
            services.AddSingleton(manager);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IUnitOfWork>(e =>
            {
                var connStr = Configuration.GetSection("DbConnects:ConnStr");
                var config = e.GetRequiredService<Configuration>();
                return new OpenApiContext(connStr.Value);
            });
            
            services.AddTransient<ICachePkProvider, Md5CachePkProvider>();
            services.AddTransient<ICacheProvider, RedisCacheProvider>();
            services.AddScoped<ICacheClient>(e =>
            {
                var config = e.GetRequiredService<Configuration>();
                var cacheProvider = e.GetRequiredService<ICacheProvider>();
                var pkProvider = e.GetRequiredService<ICachePkProvider>();
                var client = new RedisClient(config.RedisConnStr, cacheProvider, pkProvider, config.RedisDbIndex, config.RedisAsyncObject);
                client.Initialize();
                return client;
            });
            
            #region Hangfire
            
            var connstr = Configuration.GetSection("RedisConfig:Conn");
            var redisClient = new HangfireRedisClient(connstr.Value);

            var option_hangfire = new RedisStorageOptions
            {
                Db =7,
                Prefix = "test_hangfire"
            };
            services.AddHangfire(config => config.UseRedisStorage(HangfireRedisClient.Connection, option_hangfire));
            
            #endregion

            services.AddSingleton(e =>
            {
                var provider = e.GetRequiredService<Domain.Core.IConfigurationProvider>();
                return new Configuration(provider);
            });
            
            services.RegistResponseCacheInterceptor();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("Open API Platform", new Swashbuckle.AspNetCore.Swagger.Info()
                {
                    Version = "V1",
                    Title = "Open API Platform"
                });
            });
            services.AddTransient<IDependencyResolver, ExceptionResolver>();
            services.AddSingleton(e =>
            {
                var resolver = e.GetRequiredService<IDependencyResolver>();
                return new ExceptionlessConfiguration(resolver);
            });
            services.RegistRepository();
            services.RegistServices();

            var builder = new ContainerBuilder();
            registServices(builder);
            builder.Populate(services);
            Container = builder.Build();
            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            HttpContextCurrent.ServiceProvider = app.ApplicationServices;

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
#if DEV||DEBUG||UAT
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/Open API Platform/swagger.json", "Open API Platform");
               // opt.InjectOnCompleteJavaScript("/Swagger_lang.js");
            });
#endif
            
            #region Hangfire

            var jobOptions = new BackgroundJobServerOptions
            {
                Queues = new[] { "report"},
                WorkerCount = Environment.ProcessorCount * 5, //并发任务数
                ServerName = System.Net.Dns.GetHostName(),//服务器名称
            };

            app.UseHangfireServer(jobOptions);

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAthorizationFilter() },
                IsReadOnlyFunc = (DashboardContext context) => true,
                DisplayStorageConnectionString = false,
            });
            app.UseHangfireDashboard();

            using (var connection = JobStorage.Current.GetConnection())
            {
                var storageConnection = connection as JobStorageConnection;
                if (storageConnection != null)
                {
                    //立即启动
                    //var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("hangfire start!"));
                }
            }

            #endregion


            #region Nlog

            loggerFactory.AddNLog();//添加NLog

#if DEV||DEBUG
            env.ConfigureNLog("nlog.dev.config");
#endif
#if UAT
            env.ConfigureNLog("nlog.uat.config");
#endif
#if PROD
            env.ConfigureNLog("nlog.prod.config");
#endif

            #endregion
        }
        private void registServices(ContainerBuilder builder)
        {
            builder.RegisterInstance(Configuration);
            builder.RegisterAssembler();

            builder.RegistAspNetConfigurationProvider();
            builder.RegistCircuitBreadkingHandler(cc => cc.Resolve<Configuration>()?.ExceptionBeforeCircuit ?? 3,
                cc => cc.Resolve<Configuration>()?.DurationOfBreak ?? TimeSpan.FromSeconds(3),
                cc => cc.Resolve<Configuration>()?.TimeoutValue ?? TimeSpan.FromSeconds(5));
            builder.RegistAspNetConfigurationProvider();
        }
        public class HangfireAthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize([NotNull] DashboardContext context)
            {
                return true;
            }
        }
    }
}
