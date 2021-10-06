using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class UserService : IUserService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;
        private IEnumerable<UserViewModel> friends_cache;

        public UserService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<IEnumerable<UserViewModel>> GetFriends(bool reload = false)
        {
            if (friends_cache == null || reload)
                friends_cache = await GetFriendsFromServer();

            return friends_cache;
        }

        private async Task<IEnumerable<UserViewModel>> GetFriendsFromServer()
        {
            var current_user = api.Authentication.CurrentUser;

            var rh = new RequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "User/" + current_user.ID + "/Friends/", HttpMethod.Get);
            if (content == null)
                return null;

            var jtoken = JToken.Parse(content);
            if (!(jtoken is JArray))
                return null;

            var result = new List<UserViewModel>();
            foreach (var user in jtoken.ToObject<IEnumerable<User>>())
            {
                var obj = UserViewModel.FromModel(user);
                obj.CountOfCommonFriends = await GetCountOfCommonFriends(obj.ID);
                result.Add(obj);
            }

            return result;
        }

        public async Task<int> GetCountOfCommonFriends(int other_user)
        {
            var current_user = api.Authentication.CurrentUser;

            var rh = new RequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "User/" + current_user.ID + "/CountOfCommonFriends/" + other_user, HttpMethod.Get);
            if (content == null)
                return 0;

            return Convert.ToInt32(content);
        }
    }
}