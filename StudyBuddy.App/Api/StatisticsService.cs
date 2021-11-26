using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Interfaces;
using StudyBuddy.App.Models;

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

        public async Task<UserStatistics> GetUserStatistics()
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var obj = await rh.Load<UserStatistics>(base_url + "Statistics/" + api.Authentication.CurrentUser.ID, HttpMethod.Get);
            if (obj == null)
                return null;

            return obj;
        }
    }
}
