using EmployeeManagement.WebApi.Repositories.EmployeeManagement;
using EmployeeManagement.WebApi.Services.Interfaces;
using EmployeeManagement.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using EmployeeManagement.WebApi;
using EmployeeManagement.WebApi.Extensions;
using AutoMapper;
using EmployeeManagement.WebApi.DataTransferObjects.Configuration;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using FluentValidation;
using EmployeeManagement.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args).ConfigureSecrets();

// Add services to the container.

builder.Services.Configure<AppSettings>(builder.Configuration);

var provider = builder.Services.BuildServiceProvider();
var appSettings = provider.GetRequiredService<IOptions<AppSettings>>();

var allowedOrigins = nameof(appSettings.Value.AllowedOrigins);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapConfiguration());
});
var mapper = mappingConfig.CreateMapper();

var dbUsername = string.IsNullOrEmpty(builder.Configuration["DbUsername"]) ? appSettings.Value.DbUsername : builder.Configuration["DbUsername"];
var dbPassword = string.IsNullOrEmpty(builder.Configuration["DbPassword"]) ? appSettings.Value.DbPassword : builder.Configuration["DbPassword"];
var dbName = string.IsNullOrEmpty(builder.Configuration["DbName"]) ? appSettings.Value.DbName : builder.Configuration["DbName"];
var dbHostname = appSettings.Value.DbHostname;
var dbPort = appSettings.Value.DbPort;

var con = $"Host={dbHostname};Port={dbPort};Database={dbName};Username={dbUsername};Password={dbPassword}";

Console.WriteLine($"\n\nConnectionString: {con}\n\n");

builder.Services.AddDbContext<EmployeeManagementContext>(options => options.UseNpgsql(con));

builder.Services.AddCors(options =>
{
    options.AddPolicy(allowedOrigins, builder =>
    {
        builder.WithOrigins(appSettings.Value.AllowedOrigins) // .WithOrigins(this.Configuration.GetSection("AllowedOrigins").Get<string[]>()).WithHeaders(...)
            .WithHeaders("accept", "content-type", "oigin", "authorization")
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeeManagement.WebApi", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<EmployeeManagementContext>();

context.Database.Migrate();
SeedDB.SeedData(context);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(allowedOrigins);

app.UseAuthorization();

app.MapControllers();

app.AddGlobalErrorHandler();

app.Run();