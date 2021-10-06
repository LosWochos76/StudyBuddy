using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    internal class ChallengeService : IChallengeService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;
        private IEnumerable<ChallengeViewModel> cache;

        public ChallengeService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<IEnumerable<ChallengeViewModel>> ForToday(string search_string, bool reload = false)
        {
            if (cache == null || reload)
                cache = await LoadFromServer(search_string);

            if (string.IsNullOrEmpty(search_string))
                return cache;
            else
                return FilterObjects(cache, search_string);
        }

        private static IEnumerable<ChallengeViewModel> FilterObjects(IEnumerable<ChallengeViewModel> input, string search_string)
        {
            var result = new List<ChallengeViewModel>();
            foreach (var obj in input)
                if (obj.ContainsAny(search_string))
                    result.Add(obj);

            return result;
        }

        private async Task<IEnumerable<ChallengeViewModel>> LoadFromServer(string search_string)
        {
            var rh = new RequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "Challenge/ForToday", HttpMethod.Get, search_string);
            if (content == null)
                return null;

            var jtoken = JToken.Parse(content);
            if (!(jtoken is JArray))
                return null;

            var result = new List<ChallengeViewModel>();
            foreach (var obj in jtoken.ToObject<IEnumerable<Challenge>>())
            {
                var cvm = ChallengeViewModel.FromModel(obj);
                cvm.Tags = await api.Tags.OfChallengeAsString(cvm.ID);
                result.Add(cvm);
            }

            return result;
        }

        public async Task<bool> AcceptFromQrCode(string code)
        {
            var rh = new RequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "Challenge/AcceptFromQrCode", HttpMethod.Post, code);
            if (content == null)
                return false;

            var obj = JObject.Parse(content);
            return obj.ContainsKey("status");
        }
    }
}