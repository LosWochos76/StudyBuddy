﻿using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IPushNotificationService
    {
        void BroadcastMessage(PushNotificationBroadcastDto obj);
        void SendMessage(IEnumerable<string> tokens, string title, string body, PushNotificationData pushNotificationData = null);
        void SendUserLikedNotification(int userId);
        void SendUserAcceptedChallenge(User user, Challenge challenge);
    }
}