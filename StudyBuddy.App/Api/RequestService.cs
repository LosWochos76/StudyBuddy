using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
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
            var content = await rh.DropRequest(base_url + "Request", HttpMethod.Post, request);
            if (content == null)
                return false;

            var obj = JObject.Parse(content);
            return obj.ContainsKey("id");
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
            var content = await rh.DropRequest(base_url + "Request", HttpMethod.Post, request);
            if (content == null)
                return false;

            var obj = JObject.Parse(content);
            return obj.ContainsKey("id");
        }

        public async Task<IEnumerable<RequestViewModel>> ForMe()
        {
            var currentUser = api.Authentication.CurrentUser;
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "Request/ForRecipient/" + currentUser.ID, HttpMethod.Get);
            if (content == null)
                return null;

            var jtoken = JToken.Parse(content);
            if (!(jtoken is JArray))
                return null;

            var result = new List<RequestViewModel>();
            foreach (var obj in jtoken.ToObject<IEnumerable<Request>>())
            {
                var rvm = RequestViewModel.FromModel(obj);
                rvm.Sender = await api.Users.GetById(rvm.SenderID);
                if (rvm.Type == RequestType.ChallengeAcceptance)
                    rvm.Challenge = await api.Challenges.GetById(rvm.ChallengeID.Value);

                result.Add(rvm);
            }

            return result;
        }

        public async Task<bool> Accept(int request_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "Request/Accept/" + request_id, HttpMethod.Post);
            if (content == null)
                return false;

            var obj = JObject.Parse(content);
            return obj.ContainsKey("status");
        }

        public async Task<bool> Deny(int request_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "Request/Deny/" + request_id, HttpMethod.Post);
            if (content == null)
                return false;

            var obj = JObject.Parse(content);
            return obj.ContainsKey("status");
        }
    }
}
