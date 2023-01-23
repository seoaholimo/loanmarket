using System;
using Castle.DynamicProxy;

namespace BeyondIT.MicroLoan.Application.Autofac.Aop
{
    public class RetryInteceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            const int retryCount = 3;
            int currentRetry = 0;
            for (;;)
            {
                try
                {
                    invocation.Proceed();

                    break;
                }
                catch (Exception)
                {
                    currentRetry++;

                    if (currentRetry > retryCount)
                    {
                        throw;
                    }
                }
            }
        }
    }
}