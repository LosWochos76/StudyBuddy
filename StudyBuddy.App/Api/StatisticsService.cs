using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Interfaces;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class StatisticsService : IStatisticsService
    {   
        private readonly IApi api;

        public StatisticsService(IApi api)
        {
            this.api = api;
        }

        public async Task<UserStatistics> GetUserStatistics()
        {
            return await GetUserStatisticsForUser(api.Authentication.CurrentUser.ID);
        }

        public async Task<UserStatistics> GetUserStatisticsForUser(int userId)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var obj = await rh.Load<UserStatistics>(Settings.ApiUrl + "Statistics/" + userId, HttpMethod.Get);
            if (obj == null)
                return null;

            return obj;
        }
    }
}
