using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.Model;

namespace StudyBuddy.ApiFacade
{
    internal class ChallengeService : IChallengeService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;

        public ChallengeService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<IEnumerable<Challenge>> ForToday(string tag_string)
        {
            var token = api.Authentication.Token;
            if (string.IsNullOrEmpty(token))
                throw new Exception("You must login first");

            var message = new HttpRequestMessage(HttpMethod.Get, base_url + "Challenge/ForToday");
            message.Headers.Add("Authorization", api.Authentication.Token);
            message.Content = new StringContent("{\"tag_string\":\"\"}", Encoding.UTF8, "application/json");

            var response = await client.SendAsync(message);
            if (response == null || !response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var jtoken = JToken.Parse(content);

            if (jtoken is JArray)
                return jtoken.ToObject<IEnumerable<Challenge>>();
            return null;
        }

        public async Task<bool> AcceptFromQrCode(string code)
        {
            var token = api.Authentication.Token;
            if (string.IsNullOrEmpty(token))
                throw new Exception("You must login first");

            var message = new HttpRequestMessage(HttpMethod.Post, base_url + "Challenge/AcceptFromQrCode");
            message.Headers.Add("Authorization", api.Authentication.Token);
            message.Content = new StringContent("{\"Payload\":\"" + code + "\"}", Encoding.UTF8, "application/json");

            var response = await client.SendAsync(message);
            if (response == null || !response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(content);
            return obj.ContainsKey("status");
        }
    }
}