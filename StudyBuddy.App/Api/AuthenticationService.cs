using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.Api
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
        public UserViewModel CurrentUser { get; private set; }

        public async Task<int> Login(UserCredentials credentials)
        {
            try
            {
                var rh = new WebRequestHelper();
                LoginResult response = await rh.Post<LoginResult>(base_url + "Login", credentials);
                if (response == null)
                    return 4;

                if (response.Status != 0)
                    return response.Status;
                
                string jsonstring = JsonSerializer.Serialize(response);
                var result = await LoginFromJson(jsonstring);

                if (!result)
                    return 5;
                else
                    return response.Status;
                 
            }
            catch (Exception exp)
            {
                return 6;
            }
        }

        public async Task<bool> LoginFromJson(string content)
        {
            var obj = JsonDocument.Parse(content);

            JsonElement token_element, user_element;
            if (!obj.RootElement.TryGetProperty("Token", out token_element) ||
                !obj.RootElement.TryGetProperty("User", out user_element))
                return false;

            var token = token_element.ToString();
            var is_valid = await api.Authentication.IsTokenValid(token);
            if (!is_valid)
                return false;

            Token = token;
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            CurrentUser = JsonSerializer.Deserialize<UserViewModel>(user_element.GetRawText(), options);
            await api.ImageService.GetProfileImage(CurrentUser);

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

        public async Task<bool> IsTokenValid(string token)
        {
            var rh = new WebRequestHelper();
            return await rh.Put<bool>(base_url + "Login", token);
        }

        public async Task<bool> SendPasswortResetMail(string email)
        {
            var rh = new WebRequestHelper();
            var status = await rh.Load<RequestResult>(base_url + "Login/SendPasswortResetMail", HttpMethod.Post, email);

            if (status == null)
                return false;

            return status.IsOk;
        }
    }
}