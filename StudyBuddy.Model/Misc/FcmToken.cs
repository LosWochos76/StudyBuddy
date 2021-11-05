using System;

namespace StudyBuddy.Model
{
    public class FcmToken
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public int UserID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public DateTime LastSeen { get; set; } = DateTime.Now.Date;
    }

    public class FcmTokenSaveDto
    {
        public string Token { get; set; }

        public FcmToken ToFcmToken()
        {
            return new FcmToken
            {
                Token = Token
            };
        }
    }
}