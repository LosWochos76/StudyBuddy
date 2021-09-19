using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.ApiFacade.Restful;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.ApiFacade
{
    internal class AuthenticationService : IAuthenticationService
    {
        private IApi api;
        private readonly string base_url;
        private readonly HttpClient client;

        public AuthenticationService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public event LoginStateChangedHandler LoginStateChanged;
        public string Token { get; private set; } = string.Empty;
        public User CurrentUser { get; private set; }

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
            catch (Exception exp)
            {
                var test = exp;
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

            OnLoginStateChanged(true);
            return true;
        }

        public async void Logout()
        {
            Token = string.Empty;
            CurrentUser = null;
            Application.Current.Properties.Remove("Login");
            await Application.Current.SavePropertiesAsync();

            OnLoginStateChanged(false);
        }

        public bool IsLoggedIn => Token != string.Empty;

        private void OnLoginStateChanged(bool is_logged_in)
        {
            if (LoginStateChanged != null)
                LoginStateChanged(this, new LoginStateChangedArgs(is_logged_in, CurrentUser, Token));
        }
    }
}