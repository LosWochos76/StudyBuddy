using System;
using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model.Filter;

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
            var content = await rh.Load<RequestViewModel>(base_url + "Request", HttpMethod.Post, request);
            if (content == null)
                return false;

            content.Sender = currentUser;
            obj.FriendshipRequest = content;
            api.RaiseRequestStateChanged(this, new RequestStateChangedEventArgs() { Request = content, Type = RequestStateChangedEventType.Created });
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
            var content = await rh.Load<RequestViewModel>(base_url + "Request", HttpMethod.Post, request);
            if (content == null)
                return false;

            api.RaiseRequestStateChanged(this, new RequestStateChangedEventArgs() { Request = content, Type = RequestStateChangedEventType.Created });
            return true;
        }

        public async Task<RequestListViewModel> ForMe()
        {
            var filter = new RequestFilter()
            {
                OnlyForRecipient = api.Authentication.CurrentUser.ID,
                WithChallenge = true,
                WithSender = true
            };

            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Get<RequestListViewModel>(base_url + "Request/", filter);
        }

        public async Task<RequestListViewModel> FromMe()
        {
            var filter = new RequestFilter()
            {
                OnlyForSender = api.Authentication.CurrentUser.ID,
                WithChallenge = true,
                WithSender = true
            };

            var rh = new WebRequestHelper(api.Authentication.Token);
            return await rh.Get<RequestListViewModel>(base_url + "Request/", filter);
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