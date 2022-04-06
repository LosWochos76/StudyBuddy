using System;

namespace StudyBuddy.Model
{
    public class NotificationUserMetadata
    {
        public int Id { get; set; }
        public int NotificationId { get; set; }
        public int OwnerId { get; set; }
        public bool Liked { get; set; } = false;
        public bool Seen { get; set; } = false;
        public bool Shared { get; set; } = false;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}