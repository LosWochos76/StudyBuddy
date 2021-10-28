using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public class LoggingService : ILoggingService
    {
        private readonly IBackend backend;

        public LoggingService(IBackend backend)
        {
            this.backend = backend;
        }

        public void LogInfo(string message)
        {
            backend.Repository.Logging.Log(new LogMessage()
            {
                Occurence = DateTime.Now,
                Level = LogLevel.Information,
                Source = Component.Api,
                UserId = backend.CurrentUser == null ? 0 : backend.CurrentUser.ID,
                Message = message
            });
        }

        public void LogError(string message)
        {
            backend.Repository.Logging.Log(new LogMessage()
            {
                Occurence = DateTime.Now,
                Level = LogLevel.Error,
                Source = Component.Api,
                UserId = backend.CurrentUser == null ? 0 : backend.CurrentUser.ID,
                Message = message
            });
        }

        public void LogDebug(string message)
        {
            backend.Repository.Logging.Log(new LogMessage()
            {
                Occurence = DateTime.Now,
                Level = LogLevel.Debug,
                Source = Component.Api,
                UserId = backend.CurrentUser == null ? 0 : backend.CurrentUser.ID,
                Message = message
            });
        }

        public void LogCritical(string message)
        {
            backend.Repository.Logging.Log(new LogMessage()
            {
                Occurence = DateTime.Now,
                Level = LogLevel.Critical,
                Source = Component.Api,
                UserId = backend.CurrentUser == null ? 0 : backend.CurrentUser.ID,
                Message = message
            });
        }

        public void LogWarning(string message)
        {
            backend.Repository.Logging.Log(new LogMessage()
            {
                Occurence = DateTime.Now,
                Level = LogLevel.Warning,
                Source = Component.Api,
                UserId = backend.CurrentUser == null ? 0 : backend.CurrentUser.ID,
                Message = message
            });
        }

        public void Log(LogMessage message)
        {
            backend.Repository.Logging.Log(message);
        }

        public IEnumerable<LogMessage> All(LogMessageFilter filter)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            return backend.Repository.Logging.All(filter);
        }

        public void Flush()
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            backend.Repository.Logging.Flush();
        }
    }
}