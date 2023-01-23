using System;
using System.Collections.Specialized;
using System.Web;

namespace BeyondIT.MicroLoan.Infrastructure.Extensions
{
    public static class UriExtensions
    {
        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            NameValueCollection httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri) {Query = httpValueCollection.ToString()};

            return ub.Uri;
        }
    }
}