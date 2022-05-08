using System.Collections.Generic;

namespace StudyBuddy.BusinessLogic
{
    public interface IPushNotificationService
    {
        void BroadcastMessage(PushNotificationBroadcastDto obj);

        void SendMessage(IEnumerable<string> tokens, string title, string body,
            PushNotificationData pushNotificationData = null);

        void SendNewNotificationsAvailable();

        void SendUserLikedNotification(int userId);

        void SendUserCommentNotification(int userId);
    }
}