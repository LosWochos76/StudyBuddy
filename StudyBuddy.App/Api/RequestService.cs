using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class RequestService : IRequestService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;

        public RequestService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<bool> AskForFriendship(int other_user_id)
        {
            var current_user = api.Authentication.CurrentUser;
            var request = new Request()
            {
                SenderID = current_user.ID,
                RecipientID = other_user_id,
                Created = DateTime.Now.Date,
                Type = RequestType.Friendship
            };

            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "Request", HttpMethod.Post, request);
            if (content == null)
                return false;

            var obj = JObject.Parse(content);
            return obj.ContainsKey("id");
        }
    }
}
