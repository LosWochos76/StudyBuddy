using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.BusinessLogic
{
    public interface INotificationUserMetadataService
    {
        NotificationUserMetadataList GetAll(NotificationUserMetadataFilter notificationUserMetadataFilter);
        void Upsert(NotificationUserMetadata obj);
        public void Delete(int id);
    }
}