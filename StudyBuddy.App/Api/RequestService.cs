using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using StudyBuddy.App.ViewModels;

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

        public async Task<bool> AskForFriendship(UserViewModel obj)
        {
            var currentUser = api.Authentication.CurrentUser;
            var request = new Request()
            {
                SenderID = currentUser.ID,
                RecipientID = obj.ID,
                Created = DateTime.Now.Date,
                Type = RequestType.Friendship
            };

            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<Request>(base_url + "Request", HttpMethod.Post, request);
            if (content == null)
                return false;

            var rvm = RequestViewModel.FromModel(content);
            rvm.Sender = currentUser;
            obj.FriendshipRequest = rvm;
            api.RaiseRequestStateChanged(this, new RequestStateChangedEventArgs() { Request = rvm, Type = RequestStateChangedEventType.Created });
            return true;
        }

        public async Task<bool> AskForChallengeAcceptance(int other_user_id, int challenge_id)
        {
            var currentUser = api.Authentication.CurrentUser;
            var request = new Request()
            {
                SenderID = currentUser.ID,
                RecipientID = other_user_id,
                Created = DateTime.Now.Date,
                Type = RequestType.ChallengeAcceptance,
                ChallengeID = challenge_id
            };

            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<Request>(base_url + "Request", HttpMethod.Post, request);
            if (content == null)
                return false;

            var rvm = RequestViewModel.FromModel(request);
            api.RaiseRequestStateChanged(this, new RequestStateChangedEventArgs() { Request = rvm, Type = RequestStateChangedEventType.Created });
            return true;
        }

        public async Task<IEnumerable<RequestViewModel>> ForMe()
        {
            var currentUser = api.Authentication.CurrentUser;
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<IEnumerable<Request>>(base_url + "Request/ForRecipient/" + currentUser.ID, HttpMethod.Get);
            if (content == null)
                return null;

            var result = new List<RequestViewModel>();
            foreach (var obj in content)
                result.Add(RequestViewModel.FromModel(obj));

            return result;
        }

        public async Task<IEnumerable<RequestViewModel>> FromMe()
        {
            var currentUser = api.Authentication.CurrentUser;
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<IEnumerable<Request>>(base_url + "Request/OfSender/" + currentUser.ID, HttpMethod.Get);
            if (content == null)
                return null;

            var result = new List<RequestViewModel>();
            foreach (var obj in content)
                result.Add(RequestViewModel.FromModel(obj));

            return result;
        }

        public async void AddFriendshipRequests(IEnumerable<UserViewModel> users)
        {
            var requests = await FromMe();

            foreach (var user in users)
                foreach (var req in requests)
                    if (user.ID == req.RecipientID)
                        user.FriendshipRequest = req;
        }

        public async Task<bool> Accept(RequestViewModel request)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<RequestResult>(base_url + "Request/Accept/" + request.ID, HttpMethod.Post);
            if (content == null)
                return false;

            api.RaiseRequestStateChanged(this, new RequestStateChangedEventArgs() { Request = request, Type = RequestStateChangedEventType.Accepted });
            if (request.Type == RequestType.Friendship)
            {
                api.RaiseFriendsChanged(this, new FriendshipStateChangedEventArgs() { Type = FriendshipStateChangedType.Added, Friend = request.Sender });
            }

            return content.IsOk;
        }

        public async Task<bool> Deny(RequestViewModel request)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<RequestResult>(base_url + "Request/Deny/" + request.ID, HttpMethod.Post);
            if (content == null)
                return false;

            api.RaiseRequestStateChanged(this, new RequestStateChangedEventArgs() { Request = request, Type = RequestStateChangedEventType.Denied });
            return content.IsOk;
        }

        public async Task<bool> DeleteFriendshipRequest(UserViewModel obj)
        {
            if (obj == null || obj.FriendshipRequest == null)
                return false;

            var request = obj.FriendshipRequest;
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<RequestResult>(base_url + "Request/" + request.ID, HttpMethod.Delete);
            if (content == null)
                return false;

            obj.FriendshipRequest = null;
            api.RaiseRequestStateChanged(this, new RequestStateChangedEventArgs() { Request = request, Type = RequestStateChangedEventType.Deleted });
            return content.IsOk;
        }
    }
}