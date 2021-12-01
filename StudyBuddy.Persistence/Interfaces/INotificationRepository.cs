﻿using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Model.Model;

namespace StudyBuddy.Persistence
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> All(NotificationFilter filter);
        IEnumerable<Notification> GetUserNotificationsFeed(NotificationFilter filter);
        void Insert(Notification obj);
    }
}