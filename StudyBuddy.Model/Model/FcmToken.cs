using System;

namespace StudyBuddy.Model
{
    public class FcmToken : Entity
    {
        public string Token { get; set; } = String.Empty;
        public int UserID { get; set; } = 0;
        public DateTime LastSeen { get; set; } = DateTime.Now.Date;
    }
}