using Microsoft.AspNetCore.Builder;

namespace BeyondIT.MicroLoan.Api.Infrastructure.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionLoggerMiddleware>();
        }
    }
}