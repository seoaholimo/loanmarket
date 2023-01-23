using System;
using BeyondIT.MicroLoan.Infrastructure.Configurations;
using BeyondIT.MicroLoan.Infrastructure.Http;
using Microsoft.AspNetCore.Http;


namespace BeyondIT.MicroLoan.Api.Infrastructure.Services
{
    public class HttpRequestService : IHttpRequestService
    {
        private readonly HttpContext _httpContext;
        private readonly IAppSettingsAccessor _appSettingsAccessor;
        public HttpRequestService(IHttpContextAccessor httpContextAccessor, IAppSettingsAccessor appSettingsAccessor)
        {
            _appSettingsAccessor = appSettingsAccessor;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public string CreateClientUrl(string relativePath)
        {
            if (!relativePath.StartsWith("/"))
                relativePath = relativePath.Insert(0, "/");

            var baseUri = new Uri(_appSettingsAccessor.AppSettings.Data.SiteUrl);

            return $"{baseUri.Scheme}://{baseUri.Authority}{relativePath}".Replace("/api", string.Empty);
        }

        public string CreateApiUrl(string relativePath)
        {
            if (!relativePath.StartsWith("/"))
                relativePath = relativePath.Insert(0, "/");

            return $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}{relativePath}".Replace("/api", string.Empty);
        }
    }
}