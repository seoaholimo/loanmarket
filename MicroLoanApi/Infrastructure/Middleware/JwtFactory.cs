using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using BeyondIT.MicroLoan.Api.Infrastructure.Helpers;
using BeyondIT.MicroLoan.Api.Models;
using BeyondIT.MicroLoan.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;



namespace BeyondIT.MicroLoan.Api.Infrastructure.Middleware
{
    public class JwtFactory : IJwtFactory
    {
        private readonly ILogger<JwtFactory> _logger;
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions, ILogger<JwtFactory> logger)
        {
            _logger = logger;
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> GenerateEncodedToken(string emailAddress, ClaimsIdentity identity)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, emailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, await JwtIssuerOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(JwtIssuerOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Rol),
                identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Id),
                identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.IsSuperUser)
            };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: JwtIssuerOptions.NotBefore,
                expires: JwtIssuerOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public string GetAuthenticatedUserEmailAddressFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                if (!(handler.ReadToken(token) is JwtSecurityToken jwtSecurityToken))
                {
                    return null;
                }
                return jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Failed to decode token:{token}");
                return null;
            }
        }
        public ClaimsIdentity GenerateClaimsIdentity(ClientsLogin user)
        {
            return new ClaimsIdentity(new GenericIdentity(user.Username, "Token"), new[]
            {
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Id, user.UserId.ToString()),
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, Helpers.Constants.Strings.JwtClaims.ApiAccess)
            });
        }


        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round(
            (date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (JwtIssuerOptions.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException(@"Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (JwtIssuerOptions.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

    
    }
}