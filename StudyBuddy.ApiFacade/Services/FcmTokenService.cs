using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Plugin.FirebasePushNotification;

namespace StudyBuddy.ApiFacade.Services
{
    public class FcmTokenService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;

        private FcmTokenService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }


        private async Task<HttpResponseMessage> Save(string fcmToken)
        {
            var token = api.Authentication.Token;
            if (string.IsNullOrEmpty(token))
                throw new Exception("You must login first");


            var payload = new
            {
                Token = fcmToken
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, base_url + "FcmToken");
            requestMessage.Headers.Add("Authorization", api.Authentication.Token);
            requestMessage.Content =
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(requestMessage);

            return response;
        }



    }
}