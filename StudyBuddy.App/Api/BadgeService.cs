using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class BadgeService : IBadgeService
    {
        private readonly IApi api;
        private readonly string base_url;

        public BadgeService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
        }
        private async Task<GameBadgeListViewModel> All(GameBadgeFilter filter)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Get<GameBadgeListViewModel>(base_url + "GameBadge", filter);
        }
        public async Task<GameBadgeViewModel> GetById(int badge_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Load<GameBadgeViewModel>(base_url + "GameBadge/" + badge_id, HttpMethod.Get);
        }
        public async Task<GameBadgeListViewModel> Accepted(string search_string = "", int skip = 0)
        {
            var current_user = api.Authentication.CurrentUser;
            var filter = new GameBadgeFilter()
            {
                OnlyReceived = true,
                SearchText = search_string,
                Count = 10,
                Start = skip,
                CurrentUserId = current_user == null ? 0 : current_user.ID
            };

            return await All(filter);
        }
    }
}