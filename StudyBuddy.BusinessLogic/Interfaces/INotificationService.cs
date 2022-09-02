using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface INotificationService
    {
        NotificationList GetAll(NotificationFilter filter);
        Notification ById(int id);
        void Delete(int id);

        IEnumerable<Notification> GetNotificationFeedForUser(int userId, NotificationFilter filter);
        IEnumerable<Notification> GetNotificationFromUser(int userId);

        void CreateNotificationForUser(int userId, string title, string body, int? badgeId = null);
        void UserAcceptedChallenge(User user, Challenge challenge);
        void UserReceivedBadge(User user, GameBadge badge);
    }
}