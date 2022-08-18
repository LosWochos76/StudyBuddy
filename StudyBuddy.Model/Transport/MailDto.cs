using System;

namespace StudyBuddy.Model
{
    public class MailDto
    {
        public int RecipientID { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}