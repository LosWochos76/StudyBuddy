namespace StudyBuddy.Model
{
    public class LogMessageFilter
    {
        public int Start { get; set; } = 0;
        public int Count { get; set; } = 100;
        public LogLevel MinLogLevel { get; set; } = LogLevel.Debug;
        public int? UserId { get; set; }
    }
}