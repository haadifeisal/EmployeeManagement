using AutoMapper;
using EmployeeManagement.WebApi.DataTransferObjects.Configuration;
using EmployeeManagement.WebApi.Repositories.EmployeeManagement;
using EmployeeManagement.WebApi.Services;
using EmployeeManagement.WebApi.Services.Interfaces;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

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

            var dbUsername = string.IsNullOrEmpty(Configuration["DbUsername"]) ? Configuration.GetValue<string>("DbUsername") : Configuration["DbUsername"];
            var dbPassword = string.IsNullOrEmpty(Configuration["DbPassword"]) ? Configuration.GetValue<string>("DbPassword") : Configuration["DbPassword"];
            var dbName = string.IsNullOrEmpty(Configuration["DbName"]) ? Configuration.GetValue<string>("DbName") : Configuration["DbName"];
            var dbHostname = string.IsNullOrEmpty(Configuration["DbHostname"]) ? Configuration.GetValue<string>("DbHostname") : Configuration["DbHostname"];
            var dbPort = string.IsNullOrEmpty(Configuration["DbPort"]) ? Configuration.GetValue<string>("DbPort") : Configuration["DbPort"];

            var con = $"Host={dbHostname};Port={dbPort};Database={dbName};Username={dbUsername};Password={dbPassword}";

            //Console.WriteLine($"\n\nConnectionString: {con}\n\n");

            services.AddDbContext<EmployeeManagementContext>(options => options.UseNpgsql(con));

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication("Bearer", options =>
            {
                options.ApiName = "employeeManagementApi";
                options.Authority = "https://localhost:7272";
            });

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeeManagement.WebApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployeeManagement.WebApi v1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
