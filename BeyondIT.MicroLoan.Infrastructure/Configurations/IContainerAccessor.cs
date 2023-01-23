using Autofac;

namespace BeyondIT.MicroLoan.Infrastructure.Configurations
{
    public interface IContainerAccessor
    {
        IContainer Container { get; }
    }
}