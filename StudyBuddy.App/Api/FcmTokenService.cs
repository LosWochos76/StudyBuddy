using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Plugin.FirebasePushNotification;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.Api
{
    public class FcmTokenService : IFcmTokenService
    {
        private readonly IApi api;
        private readonly HttpClient client;

        public FcmTokenService(IApi api)
        {
            this.api = api;
            client = new HttpClient(Helper.GetInsecureHandler());

            api.Authentication.LoginStateChanged += (sender, args) =>
            {
                if (args.IsLoggedIn)
                {
                    this.UpdateToken();

                    CrossFirebasePushNotification.Current.OnTokenRefresh += (source, eventArgs) =>
                    {
                        this.Save(eventArgs.Token);
                    };
                }
            };
        }

        public async Task<HttpResponseMessage> Save(string fcmToken)
        {
            if (string.IsNullOrEmpty(fcmToken))
                return null;

            var token = api.Authentication.Token;
            if (string.IsNullOrEmpty(token))
                throw new Exception("You must login first");

            var payload = new
            {
                Token = fcmToken
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Settings.ApiUrl + "FcmToken");
            requestMessage.Headers.Add("Authorization", api.Authentication.Token);
            requestMessage.Content =
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            return await client.SendAsync(requestMessage);
        }

        async void UpdateToken()
        {
      
                var token = await CrossFirebasePushNotification.Current.GetTokenAsync();
                this.Save(token);
        }
    }
}