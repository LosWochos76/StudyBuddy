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
        }

        public async Task<bool> AskForFriendship(int other_user_id)
        {
            var currentUser = api.Authentication.CurrentUser;
            var request = new Request()
            {
                SenderID = currentUser.ID,
                RecipientID = other_user_id,
                Created = DateTime.Now.Date,
                Type = RequestType.Friendship
            };

            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<Request>(base_url + "Request", HttpMethod.Post, request);
            if (content == null)
                return false;

            this.from_me_cache = null;
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

            this.from_me_cache = null;
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

        public async Task<bool> Accept(int request_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<RequestResult>(base_url + "Request/Accept/" + request_id, HttpMethod.Post);
            if (content == null)
                return false;

            this.for_me_cache = null;
            return content.IsOk;
        }

        public async Task<bool> Deny(int request_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<RequestResult>(base_url + "Request/Deny/" + request_id, HttpMethod.Post);
            if (content == null)
                return false;

            this.for_me_cache = null;
            return content.IsOk;
        }

        public async Task<bool> Delete(int request_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<RequestResult>(base_url + "Request/" + request_id, HttpMethod.Delete);
            if (content == null)
                return false;

            this.from_me_cache = null;
            return content.IsOk;
        }
    }
}