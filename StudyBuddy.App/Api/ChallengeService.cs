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

        public ChallengeService(IApi api)
        {
            this.api = api;
        }

        private async Task<ChallengeListViewModel> All(ChallengeFilter filter)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Get<ChallengeListViewModel>(Settings.ApiUrl + "Challenge", filter);
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

            return await All(filter);
        }

        public async Task<ChallengeListViewModel> Accepted(string search_string = "", int skip = 0)
        {
            var filter = new ChallengeFilter()
            {
                OnlyAccepted = true,
                WithDate = true,
                SearchText = search_string,
                Count = 10,
                Start = skip
            };

            return await All(filter);
        }

        public async Task<ChallengeViewModel> AcceptFromQrCode(string code)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Load<ChallengeViewModel>(Settings.ApiUrl + "Challenge/AcceptFromQrCode", HttpMethod.Post, code);
        }

        public async Task<ChallengeViewModel> GetById(int challenge_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Load<ChallengeViewModel>(Settings.ApiUrl + "Challenge/" + challenge_id, HttpMethod.Get);
        }

        public async Task<bool> Accept(ChallengeViewModel cvm)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var status = await rh.Load<RequestResult>(Settings.ApiUrl + "Challenge/" + cvm.ID + "/Accept", HttpMethod.Post);
            if (status == null)
                return false;

            api.RaiseChallengeAcceptedEvent(this, cvm);
            return status.IsOk;
        }

        public async Task<bool> AcceptWithAddendum(ChallengeViewModel cvm, string prove_addendum)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var status = await rh.Load<bool>(Settings.ApiUrl + "Challenge/" + cvm.ID + "/AcceptWithAddendum", HttpMethod.Post, prove_addendum);

            if (status)
                api.RaiseChallengeAcceptedEvent(this, cvm);

            return status;
        }

        public async Task<AcceptChallengeByLocationResultDTO> AcceptWithLocation(ChallengeViewModel cvm, GeoCoordinate location)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var data = new AcceptChallengeByLocationRequestDTO()
            {
                ChallengeID = cvm.ID,
                UserPosition = location
            };

            var result = await rh.Load<AcceptChallengeByLocationResultDTO>(Settings.ApiUrl + "Challenge/AcceptWithLocation", HttpMethod.Post, data);
            if (result.Success)
                api.RaiseChallengeAcceptedEvent(this, cvm);

            return result;
        }
    }
}