using System;
using System.Collections.Generic;
using System.Text;

namespace BeyondIT.MicroLoan.Domain.Exceptions
{
    public class BeyondITLoanInterceptorException: Exception
    {
         public BeyondITLoanInterceptorException(Exception occuredException, string method, string jsonParameters)
         {
            OccuredException = occuredException;
            Method = method;
            JsonParameters = jsonParameters;
         }
        public Exception OccuredException { get; }
        public string Method { get; }
        public string JsonParameters { get; }
    }
}
