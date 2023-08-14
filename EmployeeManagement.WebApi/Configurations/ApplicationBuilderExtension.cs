using Microsoft.AspNetCore.Builder;

namespace EmployeeManagement.WebApi.Configurations
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
