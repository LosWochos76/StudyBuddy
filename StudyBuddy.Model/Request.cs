using System;

namespace StudyBuddy.Model
{
    public class Request
    {
        public int ID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public int SenderID { get; set; }
        public int RecipientID { get; set; }
        public RequestType Type { get; set; }
        public int? ChallengeID { get; set; }
    }
}
