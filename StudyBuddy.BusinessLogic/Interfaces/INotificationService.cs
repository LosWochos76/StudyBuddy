using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface INotificationService
    {
        void CreateNotificationForUser(int userId, string title, string body);
        IEnumerable<Notification> GetNotificationFeedForUser(int userId);
        IEnumerable<Notification> GetNotificationFromUser(int userId);
    }
}