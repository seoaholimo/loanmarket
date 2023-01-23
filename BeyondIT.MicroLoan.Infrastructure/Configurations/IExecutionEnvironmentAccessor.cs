namespace BeyondIT.MicroLoan.Infrastructure.Configurations
{
    public interface IExecutionEnvironmentAccessor
    {
        string ApplicationPath { get; }
        bool IsDevelopment { get; }
    }
}