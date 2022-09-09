using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetAll(NotificationFilter filter);
        int GetCount(NotificationFilter filter);
        Notification ById(int id);
        void Insert(Notification obj);
        void Delete(int id);

        IEnumerable<Notification> GetNotificationsForFriends(NotificationFilter filter);
    }
}