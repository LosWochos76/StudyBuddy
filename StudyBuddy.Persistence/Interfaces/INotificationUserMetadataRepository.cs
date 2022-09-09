using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface INotificationUserMetadataRepository
    {
        IEnumerable<NotificationUserMetadata> GetAll();
        void Insert(NotificationUserMetadata obj);
        void Update(NotificationUserMetadata obj);
        void Upsert(NotificationUserMetadata obj);
        void DeleteAllForNotification(int notification_id);

        IEnumerable<NotificationUserMetadata> GetAllUnseen();
        NotificationUserMetadata FindNotificationUserMetadata(NotificationUserMetadata obj);
        NotificationUserMetadata FindNotificationUserMetadata(int notificationId, int ownerId);
    }
}