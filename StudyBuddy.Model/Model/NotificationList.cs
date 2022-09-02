using System;
using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class NotificationList
    {
        public int Count { get; set; }
        public IEnumerable<Notification> Objects { get; set; }
    }
}

