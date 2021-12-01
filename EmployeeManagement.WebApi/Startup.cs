using AutoMapper;
using EmployeeManagement.WebApi.DataTransferObjects.Configuration;
using EmployeeManagement.WebApi.Repositories.EmployeeManagement;
using EmployeeManagement.WebApi.Services;
using EmployeeManagement.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;

namespace EmployeeManagement.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapConfiguration());
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            var dataSource = Configuration["DataSource"];
            var catalog = Configuration["Catalog"];
            var user = Configuration["User"];
            var password = Configuration["Password"];

            services.AddDbContext<EmployeeManagementContext>(options =>
            {
                if(string.IsNullOrEmpty(dataSource) || string.IsNullOrEmpty(catalog) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DbConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
                }
                else
                {
                    options.UseSqlServer($"Data Source={dataSource};Initial Catalog={catalog};User ID={user};Password={password}",
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
                }

            });

            services.AddHealthChecks()
                .AddSqlServer(string.IsNullOrEmpty(dataSource) ? Configuration.GetConnectionString("DbConnection") : $"Data Source={dataSource};Initial Catalog={catalog};User ID={user};Password={password}", 
                              name: "sql", timeout: TimeSpan.FromSeconds(3), tags: new[] { "ready" });

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeeManagement.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            SeedDB.Populate(app);

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowAllOrigin");

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployeeManagement.WebApi v1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = async(context, report) =>
                    {
                        var result = JsonSerializer.Serialize(
                            new
                            {
                                status = report.Status.ToString(),
                                checks = report.Entries.Select(entry => new
                                {
                                    name = entry.Key,
                                    status = entry.Value.Status.ToString(),
                                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                    duration = entry.Value.Duration.ToString()
                                })
                            }
                        );

                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    Predicate = (_) => false
                });
            });
        }
    }
}
