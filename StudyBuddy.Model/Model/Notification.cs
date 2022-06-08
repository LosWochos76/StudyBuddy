using System;
using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class Notification
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int? BadgeId { get; set; } = null;
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Liked { get; set; } = false;
        public bool Seen { get; set; } = false;
        public bool Shared { get; set; } = false;
        public List<User> LikedUsers { get; set; } = new List<User>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}