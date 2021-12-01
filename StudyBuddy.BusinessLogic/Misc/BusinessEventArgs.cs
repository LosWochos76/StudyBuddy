using System;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public class BusinessEventArgs : EventArgs
    {
        public BusinessEventType Type { get; set; }
        public User CurrentUser { get; set; }
        public object Payload { get; set; }

        public BusinessEventArgs(BusinessEventType type, object payload)
        {
            Type = type;
            Payload = payload;
        }

        public BusinessEventArgs(BusinessEventType type)
        {
            Type = type;
        }
    }
}
