
using BeyondIT.MicroLoan.Infrastructure.Configurations;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BeyondIT.MicroLoan.Api.Infrastructure.Middleware
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string emailAddress, ClaimsIdentity identity);
        string GetAuthenticatedUserEmailAddressFromToken(string token);
        ClaimsIdentity GenerateClaimsIdentity(ClientsLogin user);
    }
        
}