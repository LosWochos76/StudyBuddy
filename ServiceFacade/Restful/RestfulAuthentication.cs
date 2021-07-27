using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.Model;
using StudyBuddy.ServiceFacade;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(RestfulAuthentication))]
namespace StudyBuddy.ServiceFacade
{
    public class RestfulAuthentication : IAuthentication
    {
        private string base_url = "https://localhost:5001/";
        private HttpClient client;
        public string Token { get; private set; } = string.Empty;
        public User CurrentUser { get; private set; } = null;

        public RestfulAuthentication()
        {
            this.client = new HttpClient(GetInsecureHandler());
        }

        private HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        public async Task<bool> Login(UserCredentials credentials)
        {
            try
            {
                var response = await client.PostAsJsonAsync(base_url + "Login", credentials);
                if (response == null || !response.IsSuccessStatusCode)
                    return false;

                var content = await response.Content.ReadAsStringAsync();
                return await LoginFromJson(content);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> LoginFromJson(string content)
        {
            var obj = JObject.Parse(content);
            if (!obj.ContainsKey("token") || !obj.ContainsKey("user"))
                return false;

            Token = obj["token"].ToString();
            CurrentUser = obj["user"].ToObject<User>();

            // Save the Login-Data to the context to be resumed
            Application.Current.Properties["Login"] = content;
            await Application.Current.SavePropertiesAsync();

            return true;
        }

        public async void Logout()
        {
            Token = string.Empty;
            CurrentUser = null;
            Application.Current.Properties.Remove("Login");
            await Application.Current.SavePropertiesAsync();
        }

        public bool IsLoggedIn
        {
            get { return Token != string.Empty; }
        }
    }
}