using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.Model;

namespace StudyBuddy.ServiceFacade
{
    public class Challenges
    {
        private string base_url;
        private string token = string.Empty;
        private HttpClient client = new HttpClient();

        public Challenges(string base_url, Authentication auth)
        {
            this.base_url = base_url;
            auth.AuthenticationStateChanged += AuthenticationStateChanged;
        }

        private void AuthenticationStateChanged(object sender, AuthenticationEventArgs e)
        {
            if (e.WasLoggedIn)
            {
                token = e.Token;
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                client.DefaultRequestHeaders.Authorization = null;
                token = string.Empty;
            }
        }

        public async Task<Challenge> ById(int id)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception("You must login first");
            
            try
            {
                var response = await client.GetAsync(base_url + "Challenge/" + id);
                if (response == null || !response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(content);
                return obj.ToObject<Challenge>();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}