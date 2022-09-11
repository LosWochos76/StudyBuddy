using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface INotificationUserMetadataService
    {
        IEnumerable<NotificationUserMetadata> GetAll();
        void Upsert(NotificationUserMetadata obj);
    }
}