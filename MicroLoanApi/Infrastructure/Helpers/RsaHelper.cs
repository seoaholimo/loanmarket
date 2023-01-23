using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace BeyondIT.MicroLoan.Api.Infrastructure.Helpers
{
    public static class RsaHelper
    {
        public static RSACryptoServiceProvider PublicKeyFromPemFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "AdviceNote.Infrastructure.Wso2.Certificates.wso2carbon.pem";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            
            using (TextReader publicKeyTextReader = new StreamReader(stream))
            {
                Org.BouncyCastle.X509.X509Certificate publicKeyParam = (Org.BouncyCastle.X509.X509Certificate) new PemReader(publicKeyTextReader).ReadObject();

                RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();

                RsaKeyParameters rsaKeyParameters = (RsaKeyParameters) publicKeyParam.GetPublicKey();

                cryptoServiceProvider.ImportParameters(new RSAParameters
                {
                    Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                    Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
                });

                return cryptoServiceProvider;
            }
        }
    }
}