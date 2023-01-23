
using BeyondIT.MicroLoan.Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;


namespace BeyondIT.MicroLoan.Api.Infrastructure.Services
{
    public class ExecutionEnvironmentAccessor : IExecutionEnvironmentAccessor
    {
        public ExecutionEnvironmentAccessor(IHostingEnvironment hostingEnvironment)
        {
            ApplicationPath = hostingEnvironment.ContentRootPath;
            IsDevelopment = hostingEnvironment.IsDevelopment();
        }

        public string ApplicationPath { get; }
        public bool IsDevelopment { get; }
    }
}