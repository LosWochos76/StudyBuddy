using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.App.Misc;
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
            var rh = new WebRequestHelper(api.Authentication.Token);
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
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "Challenge/AcceptFromQrCode", HttpMethod.Post, code);
            if (content == null)
                return false;

            var obj = JObject.Parse(content);
            return obj.ContainsKey("status");
        }

        private ChallengeViewModel TryToFindByIdInCache(IEnumerable<ChallengeViewModel> cache, int challenge_id)
        {
            if (cache != null)
                foreach (var obj in cache)
                    if (obj.ID == challenge_id)
                        return obj;

            return null;
        }

        public async Task<ChallengeViewModel> GetById(int challenge_id)
        {
            var challenge = TryToFindByIdInCache(cache, challenge_id);
            if (challenge != null)
                return challenge;

            return await GetByIdFromServer(challenge_id);
        }

        private async Task<ChallengeViewModel> GetByIdFromServer(int challenge_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "Challenge/" + challenge_id, HttpMethod.Get);
            if (content == null)
                return null;

            var jobject = JObject.Parse(content);
            var user = jobject.ToObject<Challenge>();
            var uvm = ChallengeViewModel.FromModel(user);
            return uvm;
        }
    }
}