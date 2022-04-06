using System;

namespace StudyBuddy.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public int NotificationId { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}