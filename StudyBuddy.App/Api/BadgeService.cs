using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class BadgeService : IBadgeService
    {
        private readonly IApi _api;
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public BadgeService(IApi api, string base_url)
        {
            this._api = api;
            this._baseUrl = base_url;
            _client = new HttpClient(Helper.GetInsecureHandler());
        }
        private async Task<GameBadgeListViewModel> All(GameBadgeFilter filter)
        {
            var rh = new WebRequestHelper(_api.Authentication.Token);
            return await rh.Get<GameBadgeListViewModel>(_baseUrl + "GameBadge", filter);
        }
        private async Task<GameBadgeListViewModel> AllWithDateReceived(GameBadgeFilter filter)
        {
            var rh = new WebRequestHelper(_api.Authentication.Token);
            return await rh.Get<GameBadgeListViewModel>(_baseUrl + "GameBadge", filter);
        }
        public async Task<GameBadgeViewModel> GetById(int badge_id)
        {
            var rh = new WebRequestHelper(_api.Authentication.Token);
            return await rh.Load<GameBadgeViewModel>(_baseUrl + "GameBadge/" + badge_id, HttpMethod.Get);
        }
        public async Task<GameBadgeListViewModel> Accepted(string search_string = "", int skip = 0)
        {
            var currentUser = _api.Authentication.CurrentUser;
            var filter = new GameBadgeFilter()
            {
                OnlyReceived = true,
                SearchText = search_string,
                Count = 10,
                Start = skip,
                CurrentUserId = currentUser == null ? 0 : currentUser.ID
            };

            return await All(filter);
        }
        public async Task<GameBadgeListViewModel> BadgeReceived(string search_string = "", int skip = 0)
        {
            var currentUser = _api.Authentication.CurrentUser;
            var filter = new GameBadgeFilter()
            {
                OnlyReceived = true,
                SearchText = search_string,
                Count = 10,
                Start = skip,
                CurrentUserId = currentUser == null ? 0 : currentUser.ID
            };

            return await AllWithDateReceived(filter);
        }
    }
}