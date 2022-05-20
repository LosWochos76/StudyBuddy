using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class BadgeService : IBadgeService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;

        public BadgeService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        private async Task<GameBadgeListViewModel> All(GameBadgeFilter filter)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Get<GameBadgeListViewModel>(base_url + "GameBadge", filter);
        }

        public async Task<GameBadgeListViewModel> ForToday(string search_string = "", int skip = 0)
        {
            //var current_user = api.Authentication.CurrentUser;
            var filter = new GameBadgeFilter()
            {
                SearchText = search_string,
                //OnlyReceived = true,
                //CurrentUserId = current_user.ID,
                Count = 10,
                Start = skip
            };
            return await All(filter);
        }
    }
}