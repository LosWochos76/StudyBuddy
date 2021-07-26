using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.ServiceFacade
{
    public class AuthenticationEventArgs : EventArgs
    {
        public string Token { get; set; }
        public User CurrentUser { get; set; }
        public bool WasLoggedIn { get; set; }
        public bool WasLoggedOut { get; set; }

        public AuthenticationEventArgs(string token, User current_user, bool was_logged_in, bool was_logged_out)
        {
            this.Token = token;
            this.CurrentUser = current_user;
            this.WasLoggedIn = was_logged_in;
            this.WasLoggedOut = was_logged_out;
        }
    }

    public class Authentication
    {
        private string base_url;
        private HttpClient client;
        public string Token { get; private set; } = string.Empty;
        public User CurrentUser { get; private set; } = null;
        public delegate void AuthenticationStateChangedEventHandler(object sender, AuthenticationEventArgs e);
        public event AuthenticationStateChangedEventHandler AuthenticationStateChanged;

        public Authentication(string base_url)
        {
            this.base_url = base_url;
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

        private async Task<bool> LoginFromJson(string content)
        {
            var obj = JObject.Parse(content);
            if (!obj.ContainsKey("token"))
                return false;

            Token = obj["token"].ToString();
            CurrentUser = obj["user"].ToObject<User>();

            // Save the Login-Data to the context to be resumed
            Application.Current.Properties["Login"] = content;
            await Application.Current.SavePropertiesAsync();

            var args = new AuthenticationEventArgs(Token, CurrentUser, true, false);
            AuthenticationStateChanged?.Invoke(this, args);
            return true;
        }

        public async Task<bool> TryResume()
        {
            if (!Application.Current.Properties.ContainsKey("Login"))
                return false;

            var content = Application.Current.Properties["Login"].ToString();
            return await LoginFromJson(content);
        }

        public async void Logout()
        {
            Token = string.Empty;
            CurrentUser = null;
            Application.Current.Properties.Remove("Login");
            await Application.Current.SavePropertiesAsync();

            var args = new AuthenticationEventArgs(Token, CurrentUser, false, true);
            AuthenticationStateChanged?.Invoke(this, args);
        }

        public bool IsLoggedIn 
        { 
            get { return Token != string.Empty; }
        }
    }
}