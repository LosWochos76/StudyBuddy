using System;

namespace StudyBuddy.Model
{
    public class FcmToken
    {
        public int ID { get; set; } = 0;
        public string Token { get; set; } = String.Empty;
        public int UserID { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public DateTime LastSeen { get; set; } = DateTime.Now.Date;
    }
}