using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class TagService : ITagService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;

        public TagService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<IEnumerable<Tag>> OfChallenge(int challenge_id)
        {
            var current_user = api.Authentication.CurrentUser;

            var rh = new RequestHelper(api.Authentication.Token);
            var content = await rh.DropRequest(base_url + "Tag/OfChallenge/" + challenge_id, HttpMethod.Get);
            if (content == null)
                return null;

            var jtoken = JToken.Parse(content);
            if (!(jtoken is JArray))
                return null;

            var result = new List<Tag>();
            foreach (var tag in jtoken.ToObject<IEnumerable<Tag>>())
                result.Add(tag);

            return result;
        }

        private static string ConvertTagsToTagString(IEnumerable<Tag> tags)
        {
            return string.Join(" ", (from tag in tags select "#" + tag.Name).ToArray());
        }

        public async Task<string> OfChallengeAsString(int challenge_id)
        {
            var tags = await OfChallenge(challenge_id);
            return ConvertTagsToTagString(tags);
        }
    }
}
