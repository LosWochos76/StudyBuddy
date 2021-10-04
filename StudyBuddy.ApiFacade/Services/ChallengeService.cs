using System.Collections.Generic;
using System.Net.Http;
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
            tag_string = "#all";
            var rh = new RequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "Challenge/ForToday", HttpMethod.Get, tag_string);
            var jtoken = JToken.Parse(content);

            if (jtoken is JArray)
                return jtoken.ToObject<IEnumerable<Challenge>>();
            else
                return null;
        }

        public async Task<bool> AcceptFromQrCode(string code)
        {
            var rh = new RequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "Challenge/AcceptFromQrCode", HttpMethod.Post, new { Payload = code });
            var obj = JObject.Parse(content);
            return obj.ContainsKey("status");
        }
    }
}