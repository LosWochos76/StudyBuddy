using System.Net.Http;
using StudyBuddy.App.Misc;

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
    }
}