using Nito.AsyncEx;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<ChallengeViewModel>> ForToday(string search_string)
        {
            var filter = new ChallengeFilter()
            {
                OnlyUnacceped = true,
                SearchText = search_string,
                ValidAt = DateTime.Now.Date,
                Count = 10
            };

            var result = new List<ChallengeViewModel>();

            var rh = new WebRequestHelper(api.Authentication.Token);
            var objects = await rh.Get<IEnumerable<Challenge>>(base_url + "Challenge", filter);

            foreach (var obj in objects)
                result.Add(ChallengeViewModel.FromModel(obj));

            return result;
        }

        public async Task<IEnumerable<ChallengeViewModel>> LoadMore(string search_string, int skip)
        {
            var filter = new ChallengeFilter()
            {
                OnlyUnacceped = true,
                SearchText = search_string,
                ValidAt = DateTime.Now.Date,
                Count = 10,
                Start = skip
            };

            var rh = new WebRequestHelper(api.Authentication.Token);

            var result = new List<ChallengeViewModel>();
            var objects = await rh.Get<IEnumerable<Challenge>>(base_url + "Challenge", filter);
            foreach (var obj in objects)
                result.Add(ChallengeViewModel.FromModel(obj));

            return result;
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
    }
}