using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using BeyondIT.MicroLoan.Domain.BaseTypes;
using BeyondIT.MicroLoan.Domain.Exceptions;
using BeyondIT.MicroLoan.Infrastructure.Configurations;
using BeyondIT.MicroLoan.Infrastructure.Extensions;
using BeyondIT.MicroLoan.Infrastuture.Email;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;


using Polly;
using Polly.CircuitBreaker;


namespace BeyondIT.MicroLoan.Application.Autofac.Aop
{
    public class CircuitBreakerInterceptor : IInterceptor
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<CircuitBreakerInterceptor> _logger;
        private readonly AppSettings _appSettings;
        private static readonly ConcurrentDictionary<string, CircuitBreakerPolicy> CircuitBreakerPolicies = new ConcurrentDictionary<string, CircuitBreakerPolicy>();

        public CircuitBreakerInterceptor(IEmailService emailService, IAppSettingsAccessor appSettingsAccessor, ILogger<CircuitBreakerInterceptor> logger)
        {
            _emailService = emailService;
            _logger = logger;
            _appSettings = appSettingsAccessor.AppSettings;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                string circuitBreakerKey = CreateCircuitBreakerKey(invocation);
                CircuitBreakerPolicy circuitBreakerPolicy = CircuitBreakerPolicies.GetOrAdd(circuitBreakerKey,
                    CreateCircuitBreakerPolicy());

                circuitBreakerPolicy.Execute(invocation.Proceed);
            }
            catch (BrokenCircuitException brokenCircuitException)
            {
                _logger.LogError(brokenCircuitException.InnerException,
                    $"Executing method \"{invocation.TargetType.FullName}.{invocation.Method.Name}\" has failed three times and is temporarily suspended for next 5 minutes to allow underlying service to recover.");

                if (invocation.Method.ReturnType.IsCollectionType())
                {
                    invocation.ReturnValue = Activator.CreateInstance(invocation.Method.ReturnType);
                }
            }
            catch (Exception ex)
            {
                var beyondITInterceptorException = ex as BeyondITLoanInterceptorException;
                Exception exception = beyondITInterceptorException != null
                    ? beyondITInterceptorException.OccuredException
                    : ex;

                string parameterDumb = GetParameterDumb(beyondITInterceptorException);

                string errorMessage = exception.Message;
                Exception innerException = exception.InnerException;
                while (innerException != null)
                {
                    errorMessage = innerException.Message;
                    innerException = innerException.InnerException;
                }

                var emailObject = new EmailObject
                {
                    To = _appSettings.Data.AdminEmail,
                    Subject = "BeyondIT MicroLoan Exception Occurred",
                    Body = $@"
                              {errorMessage}  
                              <br/><br/>
                              <strong>Stack Trace:</strong>
                              <br/><br/>
                              {exception.StackTrace}
                                <br/><br/>
                              {parameterDumb}"
                };

                _emailService.SendEmail(emailObject);
                if (invocation.Method.ReturnType.IsCollectionType())
                {
                    invocation.ReturnValue = Activator.CreateInstance(invocation.Method.ReturnType);
                }
            }
        }

        private static CircuitBreakerPolicy CreateCircuitBreakerPolicy()
        {
            const int exceptionsAllowedBeforeBreaking = 3;
            const int durationOfBreakInMinutes = 5;

            return Policy
                .Handle<Exception>()
                .CircuitBreaker(exceptionsAllowedBeforeBreaking, TimeSpan.FromMinutes(durationOfBreakInMinutes));
        }

        private static string CreateCircuitBreakerKey(IInvocation invocation)
        {
            ParameterInfo[] parameters = invocation.Method.GetParameters();

            return
                $"{invocation.TargetType.FullName}.{invocation.Method.Name},Parameters:{string.Join(",", parameters.Select(s => s.Name))}";
        }

        private static string GetParameterDumb(BeyondITLoanInterceptorException beyondITLoanInterceptorException)
        {
            string dumb = string.Empty;
            if (beyondITLoanInterceptorException != null)
            {
                dumb =
                    $"<strong>Method Called: {beyondITLoanInterceptorException.Method}</strong><br/><br/><strong>Parameters:</strong><br/><br/>{beyondITLoanInterceptorException.JsonParameters}";
            }
            return dumb;
        }
    }
}