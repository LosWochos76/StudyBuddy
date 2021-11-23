using StudyBuddy.App.Interfaces;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.App.Api
{
    public class StatisticsService : IStatisticsService
    {   
        private readonly IApi api;
        private readonly string base_url;

        public StatisticsService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
        }

        public async Task<int> AcceptedChallengesCount()
        {
            var currentUser = api.Authentication.CurrentUser;

            var rh = new WebRequestHelper(api.Authentication.Token);
            var challengesCount = await rh.Load<int>(base_url + "Statistics/AcceptedChallengesCount/" + currentUser.ID, HttpMethod.Get);

            return challengesCount;
        }
    }
}
