using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface ILoggingRepository
    {
        IEnumerable<LogMessage> All(LogMessageFilter filter);
        void Flush();
        void Log(LogMessage obj);
    }
}