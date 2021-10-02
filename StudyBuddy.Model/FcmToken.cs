using System;

namespace StudyBuddy.Model
{
    public class FcmToken
    {
        public int ID { get; set; }

        public String Token { get; set; }
        
        public int UserID { get; set; }
        
        public DateTime Created { get; set; } = DateTime.Now.Date;
        
        public DateTime LastSeen { get; set; } 


    }

    public class FcmTokenSaveDto
    {

        public String Token { get; set; }
        

        public FcmToken ToFcmToken()
        {
            return new FcmToken()
            {
                Token = this.Token,
            };
        }

    }
}