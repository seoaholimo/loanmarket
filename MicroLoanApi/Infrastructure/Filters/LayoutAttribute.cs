using System;

namespace BeyondIT.MicroLoan.Api.Infrastructure.Filters
{
    public enum ContainerLayout
    {
        Container,
        ContainerFluid
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class LayoutAttribute : Attribute
    {
        public LayoutAttribute(ContainerLayout containerLayout)
        {
            ContainerLayout = containerLayout;
        }

        public ContainerLayout ContainerLayout { get; }
    }
}