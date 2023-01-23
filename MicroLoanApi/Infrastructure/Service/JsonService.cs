using System;
using System.IO;
using BeyondIT.MicroLoan.Api.Infrastructure.Interfaces;
using BeyondIT.MicroLoan.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BeyondIT.MicroLoan.Api.Infrastructuree.Services
{
    public class JsonService : IJsonService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JsonService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void ValidateJsonData<T>(T deserializedObject)
        {
            if (deserializedObject != null) return;

            Stream stream = _httpContextAccessor.HttpContext.Request.Body;

            stream.Position = 0;
            string jsonString;
            using (var input = new MemoryStream())
            {
                stream.CopyTo(input);

                jsonString = System.Text.Encoding.UTF8.GetString(input.ToArray());
            }

            try
            {
                JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception exception)
            {
                throw new BeyondITLoanInterceptorException(new Exception(exception.Message), "ValidateJsonData", jsonString);
            }
        }
    }
}