using System.Net.Http;
using System.Threading.Tasks;
using StudyBuddy.App.Interfaces;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public class NotificationUserMetadataService : INotificationUserMetadataService
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
        public async Task<NotificationUserMetadata> Upsert(NotificationUserMetadata notificationUserMetadataUpsert)
        {
            var rh = new WebRequestHelper(api.Authentication.Token);
            var response = await rh.Post<NotificationUserMetadata>(base_url + "NotificationUserMetadata", notificationUserMetadataUpsert);
            return response;
        }

        public async Task<NotificationUserMetadata> Like(NotificationViewModel obj, bool liked)
        {
            return await Upsert(new NotificationUserMetadata
            {
                OwnerId = obj.OwnerId,
                NotificationId = obj.ID,
                Liked = liked
            });
        }

        public async Task<NotificationUserMetadata> Share(NotificationViewModel obj)
        {
            return await Upsert(new NotificationUserMetadata
            {
                OwnerId = obj.OwnerId,
                NotificationId = obj.ID,
                Shared = true
            });
        }
    }
}