using System;
using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class LoggingRepositoryMock : ILoggingRepository
    {
        private List<LogMessage> objects = new List<LogMessage>();

        public LoggingRepositoryMock()
        {
        }

        public IEnumerable<LogMessage> All(LogMessageFilter filter)
        {
            return objects;
        }

        public void Flush()
        {
            objects.Clear();
        }

        public void Log(LogMessage obj)
        {
            Console.WriteLine(obj.Occurence.ToString("dd:MM:yyyy HH:mm:ss") + " " + obj.Message);
            objects.Add(obj);
        }
    }
}

