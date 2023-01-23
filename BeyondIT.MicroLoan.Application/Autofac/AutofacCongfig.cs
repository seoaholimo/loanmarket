using Autofac;
using AutoMapper;
using Autofac.Extras.DynamicProxy;
using BeyondIT.MicroLoan.Application.Autofac.Aop;
using BeyondIT.MicroLoan.Infrastuture.Email;
using System.Collections.Generic;
using System.Text;
using BeyondIT.MicroLoan.Application.Mappings;
using BeyondIT.MicroLoan.Infrastructure.Database;

namespace BeyondIT.MicroLoan.Application.Autofac
{
    public class AutofacCongfig
   {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterInstance(AutoMapperConfig.CreateMappers()).As<IMapper>();

            builder.RegisterType<EmailService>()
                 .As<IEmailService>()
                 .EnableInterfaceInterceptors()
                 .InterceptedBy(typeof(RetryInteceptor));
            builder.RegisterType<MicroLoanContext>().AsSelf();
            builder.RegisterType<SeedDataService>().AsSelf();

            RegisterInterceptors(builder);

        }
        public static void RegisterInterceptors(ContainerBuilder builder)
        {
            builder.Register(c => new RetryInteceptor());
            builder.Register(c => new ExceptionInterceptor());
            builder.RegisterType<CacheInterceptor>().AsSelf();
            builder.RegisterType<LogExceptionAndReturnDefaultResultInterceptor>().AsSelf();
            builder.RegisterType<CircuitBreakerInterceptor>().AsSelf();
           
        }

   }
}
