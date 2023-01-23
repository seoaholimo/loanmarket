
using Autofac;
using BeyondIT.MicroLoan.Infrastructure.Configurations;

namespace BeyondIT.MicroLoan.Api.Infrastructure.Services
{
    public class ContainerAccessor: IContainerAccessor
    {
        public IContainer Container => Startup.Container;
    }
}