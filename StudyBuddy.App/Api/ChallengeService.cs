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

        public async Task GetAcceptedChallenges(ObservableCollection<ChallengeViewModel> list)
        {
            var filter = new ChallengeFilter()
            {
                OnlyUnacceped = false
            };

            using (await monitor.EnterAsync())
            {
                var rh = new WebRequestHelper(api.Authentication.Token);
                var currentUserId = api.Authentication.CurrentUser.ID;
                var items = await rh.Get<IEnumerable<Challenge>>(base_url + "Challenge/Accepted/" + currentUserId , filter);

                if (items == null)
                    return;

                list.Clear();
                foreach (var obj in items)
                    list.Add(ChallengeViewModel.FromModel(obj));
            }
        }

        public async Task<ChallengeViewModel> AcceptFromQrCode(string code)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var obj = await rh.Load<Challenge>(base_url + "Challenge/AcceptFromQrCode", HttpMethod.Post, code);
            if (obj == null)
                return null;

            var cvm = ChallengeViewModel.FromModel(obj);
            api.RaiseChallengeAcceptedEvent(this, cvm);
            return cvm;
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

        public async Task<bool> Accept(ChallengeViewModel cvm)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var status = await rh.Load<RequestResult>(base_url + "Challenge/" + cvm.ID + "/Accept", HttpMethod.Post);
            if (status == null)
                return false;

            api.RaiseChallengeAcceptedEvent(this, cvm);
            return status.IsOk;
        }

        public Task GetAcceptedChallengesForUser(ObservableCollection<ChallengeViewModel> list, int user_id)
        {
            //ToDo: Falls man nach abgeschlossenen Challenges eines anderen Users/Freundes suchen möchte
            throw new NotImplementedException();
        }
    }
}