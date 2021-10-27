using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Nito.AsyncEx;
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
        private List<ChallengeViewModel> cache;
        private AsyncMonitor monitor = new AsyncMonitor();

        public ChallengeService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task ForToday(ObservableCollection<ChallengeViewModel> list, string search_string, bool reload = false)
        {
            if (reload || cache == null)
                await LoadFromServer(list, search_string);
            else
                await LoadFromCache(list, search_string);
        }

        private async Task LoadFromCache(ObservableCollection<ChallengeViewModel> list, string search_string)
        {
            list.Clear();
            foreach (var obj in cache)
                if (obj.ContainsAny(search_string))
                    list.Add(obj);
        }

        private async Task LoadFromServer(ObservableCollection<ChallengeViewModel> list, string search_string)
        {
            using (await monitor.EnterAsync())
            {
                var rh = new WebRequestHelper(api.Authentication.Token);
                var items = await rh.Load<IEnumerable<Challenge>>(base_url + "Challenge/ForToday", HttpMethod.Get, search_string);
                if (items == null)
                    return;

                list.Clear();
                this.cache = new List<ChallengeViewModel>();
                foreach (var obj in items)
                {
                    var vmo = ChallengeViewModel.FromModel(obj);
                    vmo.Tags = (await api.Tags.OfChallenge(obj.ID)).ToString();
                    cache.Add(vmo);

                    if (vmo.ContainsAny(search_string))
                        list.Add(vmo);
                }
            }
        }

        public async Task<bool> AcceptFromQrCode(string code)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var status = await rh.Load<RequestResult>(base_url + "Challenge/AcceptFromQrCode", HttpMethod.Post, code);
            if (status == null)
                return false;

            return status.IsOk;
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
            var user = await rh.Load<Challenge>(base_url + "Challenge/" + challenge_id, HttpMethod.Get);
            if (user == null)
                return null;

            var uvm = ChallengeViewModel.FromModel(user);
            return uvm;
        }
    }
}