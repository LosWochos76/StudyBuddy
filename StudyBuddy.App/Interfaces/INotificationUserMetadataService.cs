using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Interfaces
{
    public interface INotificationUserMetadataService
    {
        public Task<NotificationUserMetadata> Upsert(NotificationUserMetadata notificationUserMetadataUpsert);
        public Task<NotificationUserMetadata> Like(NotificationViewModel obj, bool liked);
        public Task<NotificationUserMetadata> Share(NotificationViewModel obj);
    }
}