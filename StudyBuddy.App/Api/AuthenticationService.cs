using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;
using StudyBuddy.Model.Enum;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.Api
{
    internal class AuthenticationService : IAuthenticationService
    {
        private IApi api;
        private readonly HttpClient client;

        public AuthenticationService(IApi api)
        {
            this.api = api;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public event LoginStateChangedHandler LoginStateChanged;
        public string Token { get; private set; } = string.Empty;
        public UserViewModel CurrentUser { get; private set; }

        public async Task<LoginStatus> Login(UserCredentials credentials)
        {
            try
            {
                var rh = new WebRequestHelper();
                LoginResult response = await rh.Post<LoginResult>(Settings.ApiUrl + "Login", credentials);
                if (response == null)
                    return LoginStatus.InvalidApiResponse;

                if (response.Status != 0)
                    return response.Status;
                
                string jsonstring = JsonSerializer.Serialize(response);
                var result = await LoginFromJson(jsonstring);

                if (!result)
                    return LoginStatus.NoToken;
                else
                    return response.Status;
                 
            }
            catch (Exception exp)
            {
                return LoginStatus.UndocumentedError;
            }
        }

        public async Task<bool> LoginFromJson(string content)
        {
            if (content == String.Empty)
                return false;

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

            Preferences.Set("Login", content);
            OnLoginStateChanged(true);
            return true;
        }

        public async void Logout()
        {
            Token = string.Empty;
            CurrentUser = null;
            Preferences.Remove("Login");
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
            return await rh.Put<bool>(Settings.ApiUrl + "Login", token);
        }

        public async Task<bool> SendPasswortResetMail(string email)
        {
            var rh = new WebRequestHelper();
            var response = await rh.Load<LoginResult>(Settings.ApiUrl + "Login/SendPasswortResetMail", HttpMethod.Post, email);

            if (response.Status != 0)
                return false;

            return true;
        }

        public async Task<bool> SendVerificationMail(string email)
        {
            var rh = new WebRequestHelper();
            var response = await rh.Load<LoginResult>(Settings.ApiUrl + "Login/SendVerificationMail", HttpMethod.Post, email);

            if (response.Status != 0)
                return false;

            return true;
        }
    }
}