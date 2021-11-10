using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Nito.AsyncEx;
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
        private List<UserViewModel> friends_cache;
        private List<UserViewModel> not_friends_cache;
        private AsyncMonitor monitor = new AsyncMonitor();

        public UserService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task GetFriends(ObservableCollection<UserViewModel> list, string search_text, bool reload = false)
        {
            if (friends_cache == null || reload)
                await GetFriendsFromServer(list, search_text);
            else
                await GetFriendsFromCache(list, search_text);
        }

        private async Task GetFriendsFromCache(ObservableCollection<UserViewModel> list, string search_text)
        {
            list.Clear();
            foreach (var obj in friends_cache)
                if (obj.ContainsAny(search_text))
                    list.Add(obj);
        }

        private async Task GetFriendsFromServer(ObservableCollection<UserViewModel> list, string search_text)
        {
            using (await monitor.EnterAsync())
            {
                var current_user = api.Authentication.CurrentUser;
                var rh = new WebRequestHelper(api.Authentication.Token);
                var content = await rh.Load<IEnumerable<User>>(base_url + "User/" + current_user.ID + "/Friends/", HttpMethod.Get);
                if (content == null)
                    return;

                list.Clear();
                friends_cache = new List<UserViewModel>();
                foreach (var user in content)
                {
                    var obj = UserViewModel.FromModel(user);
                    if (obj.ContainsAny(search_text))
                        list.Add(obj);

                    friends_cache.Add(obj);
                }
            }
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

        public async Task GetNotFriends(ObservableCollection<UserViewModel> list, string search_string, bool reload = false)
        {
            if (not_friends_cache == null || reload)
                await GetNotFriendsFromServer(list, search_string);
            else
                await GetNotFriendsFromCache(list, search_string);
        }

        private async Task GetNotFriendsFromCache(ObservableCollection<UserViewModel> list, string search_text)
        {
            list.Clear();
            foreach (var obj in not_friends_cache)
                if (obj.ContainsAny(search_text))
                    list.Add(obj);
        }

        private async Task GetNotFriendsFromServer(ObservableCollection<UserViewModel> list, string search_text)
        {
            using (await monitor.EnterAsync())
            {
                var current_user = api.Authentication.CurrentUser;
                var rh = new WebRequestHelper(api.Authentication.Token);
                var content = await rh.Load<IEnumerable<User>>(base_url + "User/" + current_user.ID + "/NotFriends/", HttpMethod.Get);
                if (content == null)
                    return;

                list.Clear();
                not_friends_cache = new List<UserViewModel>();
                foreach (var user in content)
                {
                    var obj = UserViewModel.FromModel(user);
                    if (obj.ContainsAny(search_text))
                        list.Add(obj);
                    
                    not_friends_cache.Add(obj);
                }

                foreach (var user in list)
                {
                    user.FriendshipRequest = await api.Requests.GetFriendshipRequest(user.ID);
                }
            }
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
            var user = TryToFindByIdInCache(not_friends_cache, user_id);
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