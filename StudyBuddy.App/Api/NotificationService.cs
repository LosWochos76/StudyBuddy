using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;
using TinyIoC;

namespace StudyBuddy.App.Api
{
    public class NotificationService : INotificationService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;

        public NotificationService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<IEnumerable<NotificationViewModel>> GetNotificationsForFriends(int skip)
        {
            var filter = new NotificationFilter()
            {
                Start = skip,
                Count = 10,
                UserID = api.Authentication.CurrentUser.ID,
                WithOwner = true,
                WithComments = true,
                WithLikedUsers = true,
                WithBadge = true
            };

            var rh = new WebRequestHelper(api.Authentication.Token);
            var content = await rh.Get<IEnumerable<NotificationViewModel>>(base_url + "Notification/Feed", filter);
            if (content == null)
                return new List<NotificationViewModel>();

            return content;
        }

        private async Task<NotificationUserMetadata> Upsert(NotificationUserMetadataUpsert notificationUserMetadataUpsert)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var response = await rh.Post<NotificationUserMetadata>(base_url + "NotificationUserMetadata", notificationUserMetadataUpsert);
            return response;
        }

        public async Task<NotificationUserMetadata> Like(NotificationViewModel obj)
        {
            var upsert = new NotificationUserMetadataUpsert
            {
                NotificationId = obj.Id,
                Liked = obj.Liked
            };

            var response = await Upsert(upsert);
            return response;
        }

        public async Task<NotificationUserMetadata> HasSeen(NotificationViewModel obj)
        {
            var upsert = new NotificationUserMetadataUpsert
            {
                NotificationId = obj.Id,
                Seen = true
            };

            var response = await Upsert(upsert);
            return response;
        }

        public async Task<CommentViewModel> AddComment(NotificationViewModel notification, string comment_text)
        {
            var ci = new Comment()
            {
                NotificationId = notification.Id,
                Text = comment_text
            };

            var rh = new WebRequestHelper(api.Authentication.Token);
            var response = await rh.Post<CommentViewModel>(base_url + "Comment", ci);
            return response;
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