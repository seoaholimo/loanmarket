namespace BeyondIT.MicroLoan.Api.Infrastructure.Helpers
{

    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol";
                public const string Id = "id";
                public const string IsSuperUser = "isSuperUser";
                public const string AuthorizedUserClientIds = "authorizedUserClientIds";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }
        }
    }
}