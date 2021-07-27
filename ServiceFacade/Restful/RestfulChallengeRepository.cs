using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.Model;
using StudyBuddy.ServiceFacade;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(RestfulChallengeRepository))]
namespace StudyBuddy.ServiceFacade
{
    public class RestfulChallengeRepository : IChallengeRepository
    {
        private string base_url = "https://localhost:5001/";
        private string token = string.Empty;
        private HttpClient client = new HttpClient();

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