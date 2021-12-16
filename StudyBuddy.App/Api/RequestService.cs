using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using StudyBuddy.App.ViewModels;
using Nito.AsyncEx;
using System.Collections.ObjectModel;

namespace StudyBuddy.App.Api
{
    public class RequestService : IRequestService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;
        private List<RequestViewModel> for_me_cache = null;
        private List<RequestViewModel> from_me_cache = null;
        private AsyncMonitor monitor = new AsyncMonitor();

        public RequestService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());

            api.RequestStateChanged += Api_RequestStateChanged;
        }

        private void Api_RequestStateChanged(object sender, RequestStateChangedEventArgs e)
        {
            this.for_me_cache = null;
            this.from_me_cache = null;
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

        public async Task ForMe(ObservableCollection<RequestViewModel> list, bool reload = false)
        {
            if (for_me_cache == null || reload)
                await ForMeFromServer(list);
            else
                await ForMeFromCache(list);
        }

        private async Task ForMeFromServer(ObservableCollection<RequestViewModel> list)
        {
            using (await monitor.EnterAsync())
            {
                var currentUser = api.Authentication.CurrentUser;
                var rh = new WebRequestHelper(api.Authentication.Token);
                var content = await rh.Load<IEnumerable<Request>>(base_url + "Request/ForRecipient/" + currentUser.ID, HttpMethod.Get);
                if (content == null)
                    return;

                list.Clear();
                for_me_cache = new List<RequestViewModel>();
                foreach (var obj in content)
                {
                    var rvm = RequestViewModel.FromModel(obj);
                    list.Add(rvm);
                    for_me_cache.Add(rvm);

                    rvm.Sender = await api.Users.GetById(rvm.SenderID);
                    if (rvm.Type == RequestType.ChallengeAcceptance)
                        rvm.Challenge = await api.Challenges.GetById(rvm.ChallengeID.Value);
                }
            }
        }

        private async Task ForMeFromCache(ObservableCollection<RequestViewModel> list)
        {
            list.Clear();
            foreach (var obj in for_me_cache)
                list.Add(obj);
        }

        public async Task FromMe(ObservableCollection<RequestViewModel> list, bool reload = false)
        {
            if (this.from_me_cache == null || reload)
                await FromMeFromServer(list);
            else
                await FromMeFromCache(list);
        }

        private async Task FromMeFromServer(ObservableCollection<RequestViewModel> list)
        {
            using (await monitor.EnterAsync())
            {
                var currentUser = api.Authentication.CurrentUser;
                var rh = new WebRequestHelper(api.Authentication.Token);
                var content = await rh.Load<IEnumerable<Request>>(base_url + "Request/OfSender/" + currentUser.ID, HttpMethod.Get);
                if (content == null)
                    return;

                list.Clear();
                from_me_cache = new List<RequestViewModel>();
                foreach (var obj in content)
                {
                    var rvm = RequestViewModel.FromModel(obj);
                    list.Add(rvm);
                    from_me_cache.Add(rvm);

                    rvm.Sender = await api.Users.GetById(rvm.SenderID);
                    if (rvm.Type == RequestType.ChallengeAcceptance)
                        rvm.Challenge = await api.Challenges.GetById(rvm.ChallengeID.Value);
                }
            }
        }

        private async Task FromMeFromCache(ObservableCollection<RequestViewModel> list)
        {
            list.Clear();
            foreach (var obj in from_me_cache)
                list.Add(obj);
        }

        public async Task<RequestViewModel> GetFriendshipRequest(int other_user_id)
        {
            if (from_me_cache == null)
            {
                var temp = new ObservableCollection<RequestViewModel>();
                await FromMeFromServer(temp);
            }

            foreach (var r in from_me_cache)
                if (r.Type == RequestType.Friendship && r.RecipientID == other_user_id)
                    return r;

            return null;
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
            from_me_cache.Remove(request);
            api.RaiseRequestStateChanged(this, new RequestStateChangedEventArgs() { Request = request, Type = RequestStateChangedEventType.Deleted });
            return content.IsOk;
        }
    }
}