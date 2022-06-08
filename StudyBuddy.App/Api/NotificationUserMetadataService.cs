using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class NotificationUserMetadataService
    {
        private readonly IApi api;
        private readonly string base_url;
        private readonly HttpClient client;


        public NotificationUserMetadataService(IApi api, string base_url)
        {
            this.api = api;
            this.base_url = base_url;
            client = new HttpClient(Helper.GetInsecureHandler());
        }

        public async Task<NotificationUserMetadata> Upsert(
            NotificationUserMetadataUpsert notificationUserMetadataUpsert)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var response = await rh.Post<NotificationUserMetadata>(base_url + "NotificationUserMetadata",
                notificationUserMetadataUpsert);
            return response;
        }

        public async Task<NotificationUserMetadata> LikeNotification(
            NewsViewModel news)
        {
            var notification = news.ToNotification();

            var upsert = new NotificationUserMetadataUpsert
            {
                NotificationId = notification.Id,
                Liked = notification.Liked
            };

            var response = await Upsert(upsert);
            return response;
        }


        public async Task<NotificationUserMetadata> SetNotificationToSeen(
            NewsViewModel news)
        {
            var notification = news.ToNotification();

            var upsert = new NotificationUserMetadataUpsert
            {
                NotificationId = notification.Id,
                Seen = true
            };

            var response = await Upsert(upsert);
            return response;
        }
    }
}