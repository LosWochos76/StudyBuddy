using System;
using System.Collections.Generic;
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

        public WebRequestHelper()
        {
            options.PropertyNameCaseInsensitive = true;
            this.client = new HttpClient(Helper.GetInsecureHandler());
        }

        public WebRequestHelper(string token) : this()
        {
            this.token = token;
        }

        public WebRequestHelper(string token, CancellationToken cancellationToken) : this(token)
        {
            this.cancellationToken = cancellationToken;
        }

        public async Task<T> Post<T>(string path, object payload)
        {
            return await Load<T>(path, HttpMethod.Post, payload);
        }

        public async Task<T> Put<T>(string path, object payload)
        {
            return await Load<T>(path, HttpMethod.Put, payload);
        }

        public async Task<T> Load<T>(string path, HttpMethod method, object payload = null)
        {
            using (var message = new HttpRequestMessage(method, path))
            {
                if (!string.IsNullOrEmpty(token))
                    message.Headers.Add("Authorization", token);

                if (payload != null)
                {
                    var json_string = JsonSerializer.Serialize(payload);
                    message.Content = new StringContent(json_string, Encoding.UTF8, "application/json");
                }

                using (var response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var stream = await response.Content.ReadAsStringAsync();
                        return default(T);
                    }

                    try
                    { 
                        var stream = await response.Content.ReadAsStreamAsync();
                        return await JsonSerializer.DeserializeAsync<T>(stream, options);
                    }
                    catch (Exception ex)
                    {
                        return default(T);
                    }
                }
            }
        }

        public async Task<T> Get<T>(string path, object query)
        {
            var dict = GetPropertyValuesAsDictionary(query);
            var form = new FormUrlEncodedContent(dict);
            var querystring = await form.ReadAsStringAsync();

            using (var message = new HttpRequestMessage(HttpMethod.Get, path + "?" + querystring))
            {
                if (!string.IsNullOrEmpty(token))
                    message.Headers.Add("Authorization", token);    

                using (var response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var stream = await response.Content.ReadAsStringAsync();
                        return default(T);
                    }

                    try
                    { 
                        var stream = await response.Content.ReadAsStreamAsync();
                        return await JsonSerializer.DeserializeAsync<T>(stream, options);
                    }
                    catch
                    {
                        return default(T);
                    }
                }
            }
        }

        private Dictionary<string, string> GetPropertyValuesAsDictionary(object obj)
        {
            var dict = new Dictionary<string, string>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var val = prop.GetValue(obj);
                if (val != null)
                {
                    var type = val.GetType();
                    if (type.Equals(typeof(DateTime)))
                    {
                        var d = (DateTime)val;
                        dict.Add(prop.Name, d.ToString("yyyy-MM-dd"));
                    }
                    else
                        dict.Add(prop.Name, val.ToString());
                }
            }

            return dict;
        }
    }
}