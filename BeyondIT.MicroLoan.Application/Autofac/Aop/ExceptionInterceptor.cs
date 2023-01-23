using System;
using System.Collections.Generic;
using System.Reflection;
using BeyondIT.MicroLoan.Domain.Exceptions;
using Castle.DynamicProxy;
using Newtonsoft.Json;

namespace BeyondIT.MicroLoan.Application.Autofac.Aop
{
    public class ExceptionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception exception)
            {
                if (exception is BeyondITLoanException || exception is BeyondITLoanInterceptorException)
                {
                    throw;
                }

                ParameterInfo[] paremeters = invocation.Method.GetParameters();

                var arguments = new Dictionary<string, object>();
                for (int index = 0; index < invocation.Arguments.Length; index++)
                {
                    object argument = invocation.Arguments[index];
                    ParameterInfo parameter = paremeters[index];
                    arguments.Add(parameter.Name, argument);
                }

                throw new BeyondITLoanInterceptorException(exception, invocation.Method.Name, JsonConvert.SerializeObject(arguments));
            }
        }
    }
}