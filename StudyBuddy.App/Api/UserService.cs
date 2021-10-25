using System;
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
        private IEnumerable<UserViewModel> friends_cache;
        private IEnumerable<UserViewModel> users_cache;

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

            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load< IEnumerable<User>>(base_url + "User/" + current_user.ID + "/Friends/", HttpMethod.Get);
            if (content == null)
                return null;

            var result = new List<UserViewModel>();
            foreach (var user in content)
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
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Load<Int32>(base_url + "User/" + current_user.ID + "/CountOfCommonFriends/" + other_user, HttpMethod.Get);
        }

        public async Task<bool> RemoveFriend(int friend_id)
        {
            var current_user = api.Authentication.CurrentUser;

            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<RequestResult>(base_url + "User/" + current_user.ID + "/Friend/" + friend_id, HttpMethod.Delete);
            if (content == null)
                return false;

            return content.IsOk;
        }

        public async Task<IEnumerable<UserViewModel>> GetNotFriends(string search_string, bool reload=false)
        {
            if (users_cache == null || reload)
                users_cache = await GetNotFriendsFromServer();

            IEnumerable<UserViewModel> result;
            if (string.IsNullOrEmpty(search_string))
                result = users_cache;
            else
                result = FilterBySearchText(users_cache, search_string);

            foreach (var obj in result)
                obj.FriendshipRequest = await api.Requests.GetFriendshipRequest(obj.ID);

            return result;
        }

        private IEnumerable<UserViewModel> FilterBySearchText(IEnumerable<UserViewModel> input, string search_string)
        {
            var result = new List<UserViewModel>();
            foreach (var obj in input)
                if (obj.ContainsAny(search_string))
                    result.Add(obj);

            return result;
        }

        private bool IsFriend(int user_id)
        {
            foreach (var obj in friends_cache)
                if (obj.ID == user_id)
                    return true;

            return false;
        }

        // ToDo: Nachladen effizienter machen:
        private async Task<IEnumerable<UserViewModel>> GetNotFriendsFromServer()
        {
            var current_user = api.Authentication.CurrentUser;

            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<IEnumerable<User>>(base_url + "User", HttpMethod.Get);
            if (content == null)
                return null;

            var result = new List<UserViewModel>();
            foreach (var user in content)
            {
                var obj = UserViewModel.FromModel(user);
                if (obj.ID != current_user.ID && !IsFriend(obj.ID))
                {
                    obj.CountOfCommonFriends = await GetCountOfCommonFriends(obj.ID);
                    result.Add(obj);
                }
            }

            return result;
        }

        private UserViewModel TryToFindByIdInCache(IEnumerable<UserViewModel> cache, int user_id)
        {
            if (cache != null)
                foreach (var obj in cache)
                    if (obj.ID == user_id)
                        return obj;

            return null;
        }

        public async Task<UserViewModel> GetById(int user_id)
        {
            var user = TryToFindByIdInCache(users_cache, user_id);
            if (user != null)
                return user;

            user = TryToFindByIdInCache(friends_cache, user_id);
            if (user != null)
                return user;

            return await GetByIdFromServer(user_id);
        }

        private async Task<UserViewModel> GetByIdFromServer(int user_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<User>(base_url + "User/" + user_id, HttpMethod.Get);
            if (content == null)
                return null;

            return UserViewModel.FromModel(content);
        }
    }
}