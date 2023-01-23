using System;

namespace BeyondIT.MicroLoan.Api.Models
{
    public class JwtPayLoad
    {
        public string Sub { get; set; }
        public string Aud { get; set; }
        public int Nbf { get; set; }
        public string Azp { get; set; }
        public string Iss { get; set; }
        public long Exp { get; set; }
        public int Iat { get; set; }
        public string Jti { get; set; }

        public bool TokenExpired()
        {
            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(Exp).UtcDateTime;
            DateTime now = DateTime.UtcNow;
            return now > dateTime;
        }
    }
}