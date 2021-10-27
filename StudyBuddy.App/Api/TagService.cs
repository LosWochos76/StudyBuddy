using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class TagService : ITagService
    {
        private readonly IApi api;
        private readonly string base_url;

        public TagService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
        }

        public async Task<TagListViewModel> OfChallenge(int challenge_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load< IEnumerable<Tag>>(base_url + "Tag/Challenge/" + challenge_id, HttpMethod.Get);
            if (content == null)
                return null;

            var result = new TagListViewModel();
            result.Tags = content;
            return result;
        }

        public async Task<TagListViewModel> OfBadge(int badge_id)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Load<IEnumerable<Tag>>(base_url + "Tag/Badge/" + badge_id, HttpMethod.Get);
            if (content == null)
                return null;

            var result = new TagListViewModel();
            result.Tags = content;
            return result;
        }
    }
}