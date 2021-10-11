using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.Api
{
    public class WebRequestHelper
    {
        private string token;
        private HttpClient client;

        public WebRequestHelper(string token)
        {
            this.token = token;
            this.client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<string> DropRequest(string path, HttpMethod method, object payload = null)
        {
            var message = new HttpRequestMessage(method, path);

            if (!string.IsNullOrEmpty(token))
                message.Headers.Add("Authorization", token);

            var json_string = payload == null ? "" : JsonSerializer.Serialize(payload);
            var content = new StringContent(json_string, Encoding.UTF8, "application/json");
            message.Content = content;

            var response = await client.SendAsync(message);
            if (response == null || !response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }
    }
}