using BeyondIT.MicroLoan.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace BeyondIT.MicroLoan.Api.Infrastructure.Services
{
    public class AppSettingsAccessor : IAppSettingsAccessor
    {
        public AppSettingsAccessor(IOptions<AppSettings> optionsAccessor)
        {
            AppSettings = optionsAccessor.Value;
        }

        public AppSettings AppSettings { get; }
    }
}