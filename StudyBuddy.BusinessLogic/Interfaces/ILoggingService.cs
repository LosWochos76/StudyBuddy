using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface ILoggingService
    {
        IEnumerable<LogMessage> All(LogMessageFilter filter);
        void LogCritical(string message);
        void LogDebug(string message);
        void LogError(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void Log(LogMessage message);
        void Flush();
    }
}