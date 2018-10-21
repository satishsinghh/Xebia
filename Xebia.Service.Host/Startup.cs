using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xebia.Model;
using Xebia.DatabaseCore;
using Xebia.DatabaseCore.Common;
using Xebia.Service.Interface;
using Xebia.DatabaseCore.Extensions;
using Xebia.CommonUtility;
using Xebia.Service.Host.Middleware;

namespace Xebia.Service.Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            if (!env.IsLocal())
            {
                Configuration["ConnectionStrings:XebiaDatabase"] = GetVaultConnectionString(Configuration["ConnectionStrings:VaultUrl"], Configuration["ConnectionStrings:XebiaSecretPath"]);
                Configuration["ConnectionStrings:XebiaAuditDatabase"] = GetVaultConnectionString(Configuration["ConnectionStrings:VaultUrl"], Configuration["ConnectionStrings:XebiaAuditSecretPath"]);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.Configure<ConnectionStrings>(Configuration.GetSection(ConnectionStrings.ConfigSection));
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IXebiaDatabaseConnection, XebiaDatabaseConnection>();
            services.AddTransient<IXebiaAuditConnection, XebiaAuditConnection>();
            services.AddTransient<IXebiaDatabase, XebiaDatabase>();
            services.AddTransient<IXebiaAuditDatabase, XebiaAuditDatabase>();
            Service.BootStrapper.BootStrapServices(services);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsLocal())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseMvc();
            app.UseCors("CorsPolicy");
            app.UseErrorLogMiddleware();
        }

        private string GetVaultConnectionString(string vaultUrl, string secretPath)
        {
            string connectionString = string.Empty;
            var vaultConfig = new VaultConfigProvider(vaultUrl, secretPath);
            var vaultDataContext = vaultConfig.GetDatabaseConnectionAsync();
            if (vaultDataContext != null)
                connectionString = vaultDataContext.ConnectionString;

            return connectionString;
        }
    }
}
