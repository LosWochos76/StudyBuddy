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

        public async Task<IEnumerable<ChallengeViewModel>> ForToday(string search_string = "", int skip = 0)
        {
            var filter = new ChallengeFilter()
            {
                OnlyUnacceped = true,
                SearchText = search_string,
                ValidAt = DateTime.Now.Date,
                Count = 10,
                Start = skip
            };

            var result = new List<ChallengeViewModel>();
            var rh = new WebRequestHelper(api.Authentication.Token);
            var objects = await rh.Get<IEnumerable<Challenge>>(base_url + "Challenge", filter);

            if (objects == null)
                return result;

            foreach (var obj in objects)
                result.Add(ChallengeViewModel.FromModel(obj));

            return result;
        }
        
        public async Task<IEnumerable<ChallengeViewModel>> GetAcceptedChallenges()
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
                    return null;

                var list = new List<ChallengeViewModel>();
                foreach (var obj in items)
                    list.Add(ChallengeViewModel.FromModel(obj));

                return list;
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

        public async Task<bool> AcceptWithAddendum(ChallengeViewModel cvm, string prove_addendum)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var status = await rh.Load<bool>(base_url + "Challenge/" + cvm.ID + "/AcceptWithAddendum", HttpMethod.Post);

            if (status)
                api.RaiseChallengeAcceptedEvent(this, cvm);

            return status;
        }
    }
}