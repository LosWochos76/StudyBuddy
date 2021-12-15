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

        public async Task<bool> Login(UserCredentials credentials)
        {
            try
            {
                var response = await client.PostAsJsonAsync(base_url + "Login", credentials);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    api.Logging.LogError("No valid response on Login");
                    return false;
                }

                var content = await response.Content.ReadAsStringAsync();
                return await LoginFromJson(content);
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public async Task<bool> LoginFromJson(string content)
        {
            var obj = JsonDocument.Parse(content);

            JsonElement token_element, user_element;
            if (!obj.RootElement.TryGetProperty("token", out token_element) ||
                !obj.RootElement.TryGetProperty("user", out user_element))
                return false;

            Token = token_element.ToString();
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            var user = JsonSerializer.Deserialize<User>(user_element.GetRawText(), options);
            CurrentUser = UserViewModel.FromModel(user);

            // ToDo: Hier muss noch das Profilbild nachgeladen werden
            await api.ImageService.GetProfileImage(CurrentUser);

            // Zudem sollte hier auch noch gecheckt werden, ob die wiederhergestellten Credentials auch noch valide sind?!?

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