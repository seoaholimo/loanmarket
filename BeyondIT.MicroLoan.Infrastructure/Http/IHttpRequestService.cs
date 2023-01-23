namespace BeyondIT.MicroLoan.Infrastructure.Http
{
  
    public interface IHttpRequestService
    {
        string CreateClientUrl(string relativePath);
        string CreateApiUrl(string relativePath);
    }
}