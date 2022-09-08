using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.App.Api
{
    public class CommentService : ICommentService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;

        public CommentService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task Create(
            CommentInsert insert)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var response = await rh.Post<NotificationUserMetadata>(base_url + "Comment",
                insert);
        }

        public async Task<IEnumerable<CommentViewModel>> GetAllCommentsForNotification(CommentFilter filter)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var comments = await rh.Get<IEnumerable<Comment>>(base_url + "Comment", filter);
            if (comments == null)
                return null;

            var result = new List<CommentViewModel>();
            foreach (var comment in comments)
                result.Add(new CommentViewModel(comment));

            return result;
        }
    }
}