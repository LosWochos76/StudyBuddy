using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class BadgeService : IBadgeService
    {
        private readonly IApi _api;
        private readonly string _baseUrl;

        public BadgeService(IApi api, string base_url)
        {
            _api = api;
            _baseUrl = base_url;
        }

        public async Task<GameBadgeViewModel> GetById(int badge_id)
        {
            var rh = new WebRequestHelper(_api.Authentication.Token);
            return await rh.Load<GameBadgeViewModel>(_baseUrl + "GameBadge/" + badge_id, HttpMethod.Get);
        }

        private async Task<GameBadgeListViewModel> All(GameBadgeFilter filter)
        {
            var rh = new WebRequestHelper(_api.Authentication.Token);
            return await rh.Get<GameBadgeListViewModel>(_baseUrl + "GameBadge", filter);
        }

        public async Task<GameBadgeListViewModel> BadgesReceived(string search_string = "", int skip = 0)
        {
            var currentUser = _api.Authentication.CurrentUser;
            var filter = new GameBadgeFilter()
            {
                SearchText = search_string,
                Count = 10,
                Start = skip,
            };

            var rh = new WebRequestHelper(_api.Authentication.Token);
            return await rh.Get<GameBadgeListViewModel>(_baseUrl + "User/" + currentUser + "/GameBadge", filter);
        }
    }
}