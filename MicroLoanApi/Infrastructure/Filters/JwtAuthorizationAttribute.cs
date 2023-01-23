using System;
using System.Security.Cryptography;
using System.Text;
using BeyondIT.MicroLoan.Api.Infrastructure.Helpers;
using BeyondIT.MicroLoan.Api.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;


namespace BeyondIT.MicroLoan.Api.Infrastructure.Filters
{
public class JwtAuthorizationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {

          
        const string jwt = "eyJ4NXQiOiJOVEF4Wm1NeE5ETXlaRGczTVRVMVpHTTBNekV6T0RKaFpXSTRORE5sWkRVMU9HRmtOakZpTVEiLCJraWQiOiJOVEF4Wm1NeE5ETXlaRGczTVRVMVpHTTBNekV6T0RKaFpXSTRORE5sWkRVMU9HRmtOakZpTVEiLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJhZG1pbiIsImF1ZCI6IklOTUUwMktBWlVabUxscU9lUEdqdzR1M1pDMGEiLCJuYmYiOjE1NDg4NDQxODcsImF6cCI6IklOTUUwMktBWlVabUxscU9lUEdqdzR1M1pDMGEiLCJpc3MiOiJodHRwczpcL1wvbG9jYWxob3N0Ojk0NDNcL29hdXRoMlwvdG9rZW4iLCJleHAiOjE1NDg5Mjg3ODcsImlhdCI6MTU0ODg0NDE4NywianRpIjoiMTBjOWI0NjctMTFkMy00MjI2LThlZGUtYjExOTVkNTg0NzcwIn0.g7LeTZ4hqHufkvz88s0qv2eoXNoK8uGCPZO8ZBTHij9t58E9JjbZzQMzDZ4lyxMwfib4pFJqXd8F8t8M6WyaQp_bfUtmekOeveb3-hE_0RxI2MjGwcZ9euVFOtq8BUH7mcXpULXk5sIUErB6cXO60RnXghWQv2yyctQFS-z1jwNyfzO8QZR7qOQiqBaULjGYsJ3Qsnyb-qQNpl4kedTetI6_Die7R7swdfCSvki4gBo7J7vyXG3ykDvrPaDgO95EB2iBtcMjRL3pg9j91QF8lvcodFDNv_znG3Cp0bNNTBQw9MNkIsZPNoTPNOVUV8Ggklf017XWIOTuzJuqr1kRXA";
        string[] jwtParts = jwt.Split('.');
        var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(jwtParts[0] + '.' + jwtParts[1]));

        RSACryptoServiceProvider rsaCryptoServiceProvider = RsaHelper.PublicKeyFromPemFile();
        var signatureDeformatter = new RSAPKCS1SignatureDeformatter(rsaCryptoServiceProvider);
        signatureDeformatter.SetHashAlgorithm("SHA256");

        if (!signatureDeformatter.VerifySignature(hash, FromBase64Url(jwtParts[2])))
        {
            //Unauthorized
        }
        else
        {
            byte[] data = FromBase64Url(jwtParts[1]);
            var payload = Encoding.UTF8.GetString(data);
            var jwtPayload = JsonConvert.DeserializeObject<JwtPayLoad>(payload);
            if (jwtPayload.TokenExpired())
            {
                //Unauthorized
            }
            else
            {
                //Set current user from sub
                base.OnActionExecuting(actionContext);
            }
        }
    }
    private static byte[] FromBase64Url(string base64Url)
    {
        string padded = base64Url.Length % 4 == 0
                ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
        string base64 = padded.Replace("_", "/")
                                  .Replace("-", "+");
        return Convert.FromBase64String(base64);
    }
}
}