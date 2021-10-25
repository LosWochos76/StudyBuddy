using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.Api
{
    public class WebRequestHelper
    {
        private string token;
        private HttpClient client;
        private CancellationToken cancellationToken;
        private JsonSerializerOptions options = new JsonSerializerOptions();

        public WebRequestHelper(string token)
        {
            options.PropertyNameCaseInsensitive = true;
            this.token = token;
            this.client = new HttpClient(Helper.GetInsecureHandler());
        }

        public WebRequestHelper(string token, CancellationToken cancellationToken) : this(token)
        {
            this.cancellationToken = cancellationToken;
        }

        public async Task<T> Load<T>(string path, HttpMethod method, object payload = null)
        {
            using (var message = new HttpRequestMessage(method, path))
            {
                if (!string.IsNullOrEmpty(token))
                    message.Headers.Add("Authorization", token);

                var json_string = payload == null ? "" : JsonSerializer.Serialize(payload);
                message.Content = new StringContent(json_string, Encoding.UTF8, "application/json"); ;

                using (var response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<T>(stream, options);
                }
            }
        }
    }
}