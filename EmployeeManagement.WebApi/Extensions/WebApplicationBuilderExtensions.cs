using Microsoft.AspNetCore.Builder;
using System.IO;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagement.WebApi.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureSecrets(this WebApplicationBuilder builder)
        {
            var secretsFilePattern = "*.json";
            var applicationDirectory = Directory.GetParent(AppContext.BaseDirectory).FullName;
            var secretsDirectory = Path.Combine(applicationDirectory, "Secrets");

            string[] secretsFileNames = null;
            try
            {
                Console.WriteLine($"Startup: Secrets directory {secretsDirectory}");

                secretsFileNames = Directory.GetFiles(secretsDirectory, secretsFilePattern);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Startup: Secrets folder not found");
            }
            catch (Exception)
            {
                Console.WriteLine("Startup: Secrets could not be configured");
            }

            secretsFileNames?.ToList().ForEach(secretFileName =>
            {
                try
                {
                    builder.Configuration.AddJsonFile(secretFileName, optional: true, reloadOnChange: true);
                }
                catch
                {
                    Console.WriteLine($"Startup: Secrets file {secretFileName} could not be loaded.");
                }
            });

            return builder;
        }
    }
}
