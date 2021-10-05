using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudyBuddy.ApiFacade
{
    public class RequestHelper
    {
        private string token;
        private HttpClient client;

        public RequestHelper(string token)
        {
            this.token = token;
            this.client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<string> DropRequest(string path, HttpMethod method, object payload = null)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception("You must login first");

            var message = new HttpRequestMessage(method, path);
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
