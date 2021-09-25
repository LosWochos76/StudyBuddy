using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.Model;
using System.Text;

namespace StudyBuddy.ApiFacade
{
    class RestfulChallengeRepository : IChallengeRepository
    {
        private IApiFacade api;
        private string base_url;
        private HttpClient client;

        public RestfulChallengeRepository(IApiFacade api, string base_url)
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
            else
                return null;
        }
    }
}