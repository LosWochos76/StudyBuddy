using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public interface INotificationUserMetadataService
    {
        Task<NotificationUserMetadata> LikeNotification(NewsViewModel news);
        Task<NotificationUserMetadata> SetNotificationToSeen(NewsViewModel news);
        Task<NotificationUserMetadata> Upsert(NotificationUserMetadataUpsert notificationUserMetadataUpsert);
    }
}