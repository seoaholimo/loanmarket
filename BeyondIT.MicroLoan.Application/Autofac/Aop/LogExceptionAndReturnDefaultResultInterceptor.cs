using System;
using BeyondIT.MicroLoan.Domain.BaseTypes;
using BeyondIT.MicroLoan.Infrastructure.Configurations;
using BeyondIT.MicroLoan.Infrastuture.Email;
using Castle.DynamicProxy;


namespace BeyondIT.MicroLoan.Application.Autofac.Aop
{
    public class LogExceptionAndReturnDefaultResultInterceptor : IInterceptor
    {
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;

        public LogExceptionAndReturnDefaultResultInterceptor(IEmailService emailService, IAppSettingsAccessor appSettingsAccessor)
        {
            _emailService = emailService;
            _appSettings = appSettingsAccessor.AppSettings;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception exception)
            {
                string errorMesage = exception.Message;
                Exception innerException = exception.InnerException;
                while (innerException != null)
                {
                    errorMesage = innerException.Message;
                    innerException = innerException.InnerException;
                }

                var emailObject = new EmailObject
                {
                    To = _appSettings.Data.AdminEmail,
                    Subject = "BeyondIT Microloan  Exception Occurred",
                    Body = $@"
                              {errorMesage}  
                              <br/><br/>
                              <strong>Stack Trace:</strong>
                              <br/><br/>
                              {exception.StackTrace}"
                };

                _emailService.SendEmail(emailObject);

            
                 invocation.ReturnValue = Activator.CreateInstance(invocation.Method.ReturnType);
                
            }
        }
    }
}