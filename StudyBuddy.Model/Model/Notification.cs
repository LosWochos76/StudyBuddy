using System;
using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class Notification : Entity
    {
        public int OwnerId { get; set; } = 0;
        public User Owner { get; set; } = null;
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int? BadgeId { get; set; } = null;
        public GameBadge Badge { get; set; } = null;
        public DateTime Updated { get; set; } = DateTime.Now.Date;
        public IEnumerable<User> LikedUsers { get; set; } = new List<User>();
    }
}