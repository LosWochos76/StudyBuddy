using System;

namespace StudyBuddy.Model
{
    public class NotificationUserMetadata : Entity
    {
        public int NotificationId { get; set; } = 0;
        public int OwnerId { get; set; } = 0;
        public bool? Liked { get; set; }
        public bool? Seen { get; set; }
        public bool? Shared { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
    }
}