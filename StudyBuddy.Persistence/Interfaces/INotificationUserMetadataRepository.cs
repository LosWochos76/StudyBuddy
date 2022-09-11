using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface INotificationUserMetadataRepository
    {
        // Basic CRUD
        IEnumerable<NotificationUserMetadata> GetAll();
        void Insert(NotificationUserMetadata obj);
        void Update(NotificationUserMetadata obj);

        // Misc
        NotificationUserMetadata FindByNotificationAndOwner(int notification_id, int owner_id);
        void DeleteAllForNotification(int notification_id);
    }
}