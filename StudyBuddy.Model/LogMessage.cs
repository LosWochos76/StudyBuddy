using System;

namespace StudyBuddy.Model
{
    public class LogMessage
    {
        public DateTime Occurence { get; set; } = DateTime.Now;
        public string Message { get; set; }
        public LogLevel Level { get; set; }
        public int UserId { get; set; }
        public Component Source { get; set; }
    }
}