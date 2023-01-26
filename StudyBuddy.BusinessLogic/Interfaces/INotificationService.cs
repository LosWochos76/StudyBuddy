using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface INotificationService
    {
        NotificationList GetAll(NotificationFilter filter);
        Notification ById(int id);
        void Delete(int id);
        IEnumerable<Notification> GetNotificationsForFriends(NotificationFilter filter);

        void UserAcceptedChallenge(User user, Challenge challenge);
        void UserReceivedBadge(User user, GameBadge badge);
        void UserMadeFriend(User userA, User userB);
    }
}