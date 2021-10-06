using System.Net.Http;
using System.Net.Security;

namespace StudyBuddy.App.Api
{
    public class Helper
    {
        public static HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;

                return errors == SslPolicyErrors.None;
            };

            return handler;
        }
    }
}