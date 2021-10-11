using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public class BadgeService : IBadgeService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;
        private IEnumerable<BadgeViewModel> badge_cache;

        public BadgeService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public Task<IEnumerable<BadgeViewModel>>GetBadges(bool reload = false)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BadgeViewModel>> GetBadgesForUser(bool reload = false)
        {
            throw new NotImplementedException();
        }

    }
}