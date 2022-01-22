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

        // Only filled in certain contexts
        public User Sender { get; set; }
        public User Recipient { get; set; }
        public Challenge Challenge { get; set; }
    }
}