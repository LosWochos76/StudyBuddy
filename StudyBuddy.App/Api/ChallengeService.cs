using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StudyBuddy.App.Api
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

        public async Task<ChallengeListViewModel> ForToday(string search_string = "", int skip = 0)
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
            return await rh.Get<ChallengeListViewModel>(base_url + "Challenge", filter);
        }
        
        public async Task<ChallengeListViewModel> GetAcceptedChallenges()
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var currentUserId = api.Authentication.CurrentUser.ID;
            //var t = await rh.Get<ChallengeListViewModel>(base_url + "Challenge/Accepted/" + currentUserId, filter);
            var t = await rh.Load<ChallengeListViewModel>(base_url + "Challenge/Accepted/" + currentUserId, HttpMethod.Get);
                        
            return t;
        }
        
        public async Task<ChallengeViewModel> AcceptFromQrCode(string code)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Load<ChallengeViewModel>(base_url + "Challenge/AcceptFromQrCode", HttpMethod.Post, code);
        }

        public async Task<ChallengeViewModel> GetById(int challenge_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Load<ChallengeViewModel>(base_url + "Challenge/" + challenge_id, HttpMethod.Get);
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
            var status = await rh.Load<bool>(base_url + "Challenge/" + cvm.ID + "/AcceptWithAddendum", HttpMethod.Post, prove_addendum);

            if (status)
                api.RaiseChallengeAcceptedEvent(this, cvm);

            return status;
        }

    }
}