﻿using System.Collections.Generic;
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
        private readonly HttpClient client;

        public UserService(IApi api)
        {
            this.api = api;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<int> GetFriendsCount()
        {
            var filter = new FriendFilter() { };
            var currentUserId = api.Authentication.CurrentUser.ID;
            var rh = new WebRequestHelper(api.Authentication.Token);
            var result = await rh.Get<UserListViewModel>(Settings.ApiUrl + "User/" + currentUserId + "/Friends/", filter);
            return result.Count;
        }

        public async Task<UserListViewModel> GetFriends(string search_string = "", int skip = 0)
        {
            var filter = new FriendFilter()
            {
                Count = 10,
                Start = skip,
                SearchText = search_string
            };

            var currentUserId = api.Authentication.CurrentUser.ID;
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Get<UserListViewModel>(Settings.ApiUrl + "User/" + currentUserId + "/Friends/", filter);
        }
        public async Task<int> GetCommonFriends(UserViewModel friend)
        {
            var filter = new FriendFilter()
            {
            };
            var currentUserId = api.Authentication.CurrentUser.ID;
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Get<int>(Settings.ApiUrl + "User/" + currentUserId + "/CountOfCommonFriends/" + friend.ID, filter);
        }
        public async Task<UserListViewModel> GetNotFriends(string search_string = "", int skip = 0)
        {
            var filter = new FriendFilter()
            {
                Count = 10,
                Start = skip,
                SearchText = search_string,
                WithFriendshipRequest = true
            };

            var currentUserId = api.Authentication.CurrentUser.ID;
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Get<UserListViewModel>(Settings.ApiUrl + "User/" + currentUserId + "/NotFriends/", filter);
        }

        public async Task<bool> RemoveFriend(UserViewModel uvm)
        {
            var current_user = api.Authentication.CurrentUser;

            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<RequestResult>(Settings.ApiUrl + "User/" + current_user.ID + "/Friend/" + uvm.ID, HttpMethod.Delete);
            if (content == null)
                return false;

            api.RaiseFriendsChanged(this, new FriendshipStateChangedEventArgs() { Friend = uvm, Type = FriendshipStateChangedType.Removed });
            return content.IsOk;
        }

        public async Task<UserViewModel> GetById(int user_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Load<UserViewModel>(Settings.ApiUrl + "User/" + user_id, HttpMethod.Get);
        }

        public async Task<bool> Update(UserViewModel uvm)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Put<User>(Settings.ApiUrl + "User/" + uvm.ID, UserViewModel.ToModel(uvm));
            return content != null;
        }

        public async Task<UserId> IdByEmail(string email)
        {
            var rh = new WebRequestHelper();
            return await rh.Load<UserId>(Settings.ApiUrl + "User/UserIdByEmail/" + email, HttpMethod.Get);
        }

        // TODo: Die Klasse UserId ist doch wohl ein Scherz, oder?!?
        public async Task<UserId> IdByNickname(string nickname)
        {
            var rh = new WebRequestHelper();
            return await rh.Load<UserId>(Settings.ApiUrl + "User/UserIdByNickname/" + nickname, HttpMethod.Get);
        }

        public async Task<UserViewModel> Register(UserViewModel new_user)
        {
            var rh = new WebRequestHelper();
            return await rh.Post<UserViewModel>(Settings.ApiUrl + "User/", UserViewModel.ToModel(new_user));
        }

        public async Task<IEnumerable<UserViewModel>> Likers(int notification_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Load<IEnumerable<UserViewModel>>(Settings.ApiUrl + "Notification/" + notification_id + "/Likers", HttpMethod.Get);
        }
    }
}