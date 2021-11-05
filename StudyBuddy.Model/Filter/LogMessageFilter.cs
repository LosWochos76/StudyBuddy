namespace StudyBuddy.Model
{
    public class LogMessageFilter : BaseFilter
    {
        public LogLevel MinLogLevel { get; set; } = LogLevel.Debug;
        public int? UserId { get; set; }
    }
}