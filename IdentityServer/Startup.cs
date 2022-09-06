using IdentityServer.IdentityConfiguration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var dbUsername = string.IsNullOrEmpty(Configuration["DbUsername"]) ? Configuration.GetValue<string>("DbUsername") : Configuration["DbUsername"];
            var dbPassword = string.IsNullOrEmpty(Configuration["DbPassword"]) ? Configuration.GetValue<string>("DbPassword") : Configuration["DbPassword"];
            var dbName = string.IsNullOrEmpty(Configuration["DbName"]) ? Configuration.GetValue<string>("DbName") : Configuration["DbName"];
            var dbHostname = string.IsNullOrEmpty(Configuration["DbHostname"]) ? Configuration.GetValue<string>("DbHostname") : Configuration["DbHostname"];
            var dbPort = string.IsNullOrEmpty(Configuration["DbPort"]) ? Configuration.GetValue<string>("DbPort") : Configuration["DbPort"];

            var con = $"Host=localhost;Port=5432;Database=IdentityServer;Username=postgres;Password=user1234";

            //var con = "Data Source=localhost;Initial Catalog=identityserver;Persist Security Info=False; Integrated Security=True";

            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(options =>
                     options.UseNpgsql(con));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddAspNetIdentity<ApplicationUser>()
                    .AddConfigurationStore(options => // Contains information like clients, scopes, API resources, claims, etc
                    {
                        options.ConfigureDbContext = b => b.UseNpgsql(con, npsql => npsql.MigrationsAssembly(migrationAssembly));
                    })
                    .AddOperationalStore(options => // Contains information like authorization codes, refresh tokens, etc.
                    {
                        options.ConfigureDbContext = b => b.UseNpgsql(con, npsql => npsql.MigrationsAssembly(migrationAssembly));
                    });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            context.Database.Migrate();

            DatabaseInitializer.PopulateIdentityServer(app);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            app.UseIdentityServer();
        }
    }
}
