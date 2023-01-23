using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeyondIT.MicroLoan.Infrastructure.Utilities
{
    public class HttpClientUtility : IHttpClientUtility
    {
        public T Get<T>(Uri serviceUri)
        {
            var httpClient = new HttpClient();
            string responseString = httpClient.GetStringAsync(serviceUri.OriginalString).Result;
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        public async Task<T> GetAsync<T>(Uri serviceUri)
        {
            return await Task.Run(() => Get<T>(serviceUri));
        }

        public T Post<T>(Uri serviceUri, string postData)
        {
            var httpClient = new HttpClient();
            HttpContent httpContent = new StringContent(postData);
            HttpResponseMessage responseMessage = httpClient.PostAsync(serviceUri, httpContent).Result;
            string responseString = responseMessage.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        public async Task<T> PostAsyc<T>(Uri serviceUri, string postData)
        {
            return await Task.Run(() => Post<T>(serviceUri, postData));
        }
    }
}