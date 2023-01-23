using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeyondIT.MicroLoan.Api.Infrastructure.Interfaces
{
    public interface IJsonService
    {
        void ValidateJsonData<T>(T deserializedObject);
    }
}
