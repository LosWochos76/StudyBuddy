using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
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

        public async Task<int> GetFriendsCount()
        {
            var currentUserId = api.Authentication.CurrentUser.ID;
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Load<int>(base_url + "User/" + currentUserId + "/Friends/Count", HttpMethod.Get);
        }

        public async Task<IEnumerable<UserViewModel>> GetFriends(string search_string = "", int skip = 0)
        {
            var filter = new FriendFilter()
            {
                Count = 10,
                Start = skip,
                SearchText = search_string
            };

            var currentUserId = api.Authentication.CurrentUser.ID;
            
            var rh = new WebRequestHelper(api.Authentication.Token);
            var objects = await rh.Get<IEnumerable<User>>(base_url + "User/" + currentUserId + "/Friends/", filter);

            var result = new List<UserViewModel>();
            if (objects == null)
                return result;

            foreach (var user in objects)
                result.Add(UserViewModel.FromModel(user));

            return result;
        }

        public async Task<IEnumerable<UserViewModel>> GetNotFriends(string search_string = "", int skip = 0)
        {
            var filter = new FriendFilter()
            {
                Count = 10,
                Start = skip,
                SearchText = search_string
            };

            var currentUserId = api.Authentication.CurrentUser.ID;
            var rh = new WebRequestHelper(api.Authentication.Token);
            var objects = await rh.Get<IEnumerable<User>>(base_url + "User/" + currentUserId + "/NotFriends/", filter);

            var result = new List<UserViewModel>();
            if (objects == null)
                return result;

            foreach (var user in objects)
                result.Add(UserViewModel.FromModel(user));

            return result;
        }

        public async Task<bool> RemoveFriend(UserViewModel uvm)
        {
            var current_user = api.Authentication.CurrentUser;

            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<RequestResult>(base_url + "User/" + current_user.ID + "/Friend/" + uvm.ID, HttpMethod.Delete);
            if (content == null)
                return false;

            api.RaiseFriendsChanged(this, new FriendshipStateChangedEventArgs() { Friend = uvm, Type = FriendshipStateChangedType.Removed });
            return content.IsOk;
        }

        public async Task<UserViewModel> GetById(int user_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<User>(base_url + "User/" + user_id, HttpMethod.Get);
            if (content == null)
                return null;

            return UserViewModel.FromModel(content);
        }
    }
}