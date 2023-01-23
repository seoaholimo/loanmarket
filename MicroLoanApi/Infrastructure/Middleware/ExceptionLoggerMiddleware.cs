using System;
using System.Threading.Tasks;
using BeyondIT.MicroLoan.Domain.BaseTypes;
using BeyondIT.MicroLoan.Domain.Exceptions;
using BeyondIT.MicroLoan.Infrastructure.Configurations;
using BeyondIT.MicroLoan.Infrastuture.Email;
using Microsoft.AspNetCore.Http;


namespace BeyondIT.MicroLoan.Api.Infrastructure.Middleware
{
    public class ExceptionLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;

        public ExceptionLoggerMiddleware(RequestDelegate next, IEmailService emailService,
            IAppSettingsAccessor appSettingsAccessor, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettingsAccessor.AppSettings;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                var beyondITLoanInterceptorException = ex as BeyondITLoanInterceptorException;
                Exception exception = beyondITLoanInterceptorException != null
                    ? beyondITLoanInterceptorException.OccuredException
                    : ex;

                string parameterDumb = GetParameterDumb(beyondITLoanInterceptorException);

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
                    Subject = "BeyondIT Loan  Exception Occurred",
                    Body = $@"
                              {errorMessage}  
                              <br/><br/>
                              <strong>Stack Trace:</strong>
                              <br/><br/>
                              {exception.StackTrace}
                              <br/><br/>
                              <strong>Url:</strong>
                              <br/><br/>
                              {_httpContextAccessor.HttpContext.Request.Method}
                              <br/><br/>
                              {parameterDumb}"
                };

                _emailService.SendEmail(emailObject);

                throw;
            }
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