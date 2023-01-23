using System;
using Microsoft.AspNetCore.Http;

namespace BeyondIT.MicroLoan.Api.Infrastructure.Extensions
{
    public static class RequestExtensions
    {
        public static string CreateAbsoluteUrl(this HttpRequest httpRequest, Uri apiUrl)
        {
            return string.Concat(
                $"{httpRequest.Scheme}://",
                apiUrl == null
                    ? httpRequest.Host.ToUriComponent()
                    : apiUrl.Authority,
                httpRequest.PathBase.ToUriComponent(),
                httpRequest.Path.ToUriComponent(),
                httpRequest.QueryString.ToUriComponent());
        }
    }
}