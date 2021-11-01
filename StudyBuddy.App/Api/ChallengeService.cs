using System;
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
        private AsyncMonitor monitor = new AsyncMonitor();

        public ChallengeService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task ForToday(ObservableCollection<ChallengeViewModel> list, string search_string)
        {
            var filter = new ChallengeFilter()
            {
                OnlyUnacceped = true,
                SearchText = search_string,
                ValidAt = DateTime.Now.Date
            };

            using (await monitor.EnterAsync())
            {
                var rh = new WebRequestHelper(api.Authentication.Token);
                var items = await rh.Get<IEnumerable<Challenge>>(base_url + "Challenge", filter);
                if (items == null)
                    return;

                list.Clear();
                foreach (var obj in items)
                    list.Add(ChallengeViewModel.FromModel(obj));
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
            var rh = new WebRequestHelper(api.Authentication.Token);
            var user = await rh.Load<Challenge>(base_url + "Challenge/" + challenge_id, HttpMethod.Get);
            if (user == null)
                return null;

            var uvm = ChallengeViewModel.FromModel(user);
            return uvm;
        }
    }
}