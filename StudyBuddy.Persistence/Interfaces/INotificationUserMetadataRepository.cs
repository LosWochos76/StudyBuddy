using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.Persistence
{
    public interface INotificationUserMetadataRepository
    {
        // Basic CRUD
        IEnumerable<NotificationUserMetadata> GetAll(NotificationUserMetadataFilter notificationUserMetadataFilter);
        void Insert(NotificationUserMetadata obj);
        void Update(NotificationUserMetadata obj);

        // Misc
        NotificationUserMetadata FindByNotificationAndOwner(int notification_id, int owner_id);
        void DeleteAllForNotification(int notification_id);
        public int GetCount(NotificationUserMetadataFilter filter);
        public void Delete(int id);

    }
}