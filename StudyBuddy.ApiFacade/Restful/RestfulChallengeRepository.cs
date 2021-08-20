using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.Model;
using StudyBuddy.ApiFacade;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(RestfulChallengeRepository))]
namespace StudyBuddy.ApiFacade
{
    public class RestfulChallengeRepository : IChallengeRepository
    {
        private IAuthentication authentication;
        private string base_url = "https://localhost:5001/";
        private HttpClient client;

        public RestfulChallengeRepository()
        {
            authentication = DependencyService.Get<IAuthentication>();
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<IEnumerable<Challenge>> All()
        {
            var token = authentication.Token;
            if (string.IsNullOrEmpty(token))
                throw new Exception("You must login first");

            try
            {
                var response = await client.GetAsync(base_url + "Challenge/");
                if (response == null || !response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(content);
                return obj.ToObject<IEnumerable<Challenge>>();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}