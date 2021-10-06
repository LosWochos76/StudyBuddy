using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;

namespace StudyBuddy.App.Misc
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

        public static IEnumerable<string> SplitIntoKeywords(string search_text)
        {
            return search_text.ToLower().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool ContainsAny(string property, IEnumerable<string> keywords)
        {
            return property == null ? false : keywords.Any(kw => property.ToLower().Contains(kw));
        }
    }
}