using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface INotificationUserMetadataRepository
    {
        IEnumerable<NotificationUserMetadata> GetAll();
        void Insert(NotificationUserMetadataInsert insert);
        void Update(NotificationUserMetadataUpdate update);
        void Upsert(NotificationUserMetadataUpsert upsert);
        void DeleteAllForNotification(int notification_id);

        IEnumerable<NotificationUserMetadata> GetAllUnseen();
        NotificationUserMetadata FindNotificationUserMetadata(NotificationUserMetadataUpsert upsert);
        NotificationUserMetadata FindNotificationUserMetadata(int notificationId, int ownerId);
    }
}