using System;
using System.Threading.Tasks;

namespace BeyondIT.MicroLoan.Infrastructure.Utilities
{
    public interface IHttpClientUtility
    {
        T Get<T>(Uri serviceUri);
        Task<T> GetAsync<T>(Uri serviceUri);
        T Post<T>(Uri serviceUri, string postData);
        Task<T> PostAsyc<T>(Uri serviceUri, string postData);
    }
}