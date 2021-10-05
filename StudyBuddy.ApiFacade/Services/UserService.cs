using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.Model;

namespace StudyBuddy.ApiFacade.Services
{
    public class UserService : IUserService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;

        public UserService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<IEnumerable<User>> GetFriends()
        {
            var current_user = api.Authentication.CurrentUser;

            var rh = new RequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "User/Friends/" + current_user.ID, HttpMethod.Get);
            if (content == null)
                return null;

            var jtoken = JToken.Parse(content);
            if (jtoken is JArray)
                return jtoken.ToObject<IEnumerable<User>>();
            else
                return null;
        }
    }
}
