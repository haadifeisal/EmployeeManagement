using IdentityServer.Core.IdentityConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var dbUsername = string.IsNullOrEmpty(builder.Configuration["DbUsername"]) ? builder.Configuration.GetValue<string>("DbUsername") : builder.Configuration["DbUsername"];
var dbPassword = string.IsNullOrEmpty(builder.Configuration["DbPassword"]) ? builder.Configuration.GetValue<string>("DbPassword") : builder.Configuration["DbPassword"];
var dbName = string.IsNullOrEmpty(builder.Configuration["DbName"]) ? builder.Configuration.GetValue<string>("DbName") : builder.Configuration["DbName"];
var dbHostname = string.IsNullOrEmpty(builder.Configuration["DbHostname"]) ? builder.Configuration.GetValue<string>("DbHostname") : builder.Configuration["DbHostname"];
var dbPort = string.IsNullOrEmpty(builder.Configuration["DbPort"]) ? builder.Configuration.GetValue<string>("DbPort") : builder.Configuration["DbPort"];

var con = $"Host={dbHostname};Port={dbPort};Database={dbName};Username={dbUsername};Password={dbPassword}";

var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
         options.UseNpgsql(con));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
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

builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityServer.Core", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServer.Core"));
}

try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        //context.Database.Migrate();
        DatabaseInitializer.PopulateIdentityServer(app);
    }
}
catch (Exception e)
{
    throw;
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseIdentityServer();

app.Run();
