using System;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public class RequestStateChangedEventArgs : EventArgs
    {
        public RequestViewModel Request { get; set; }
        public RequestStateChangedEventType Type { get; set; }
    }

    public enum RequestStateChangedEventType
    {
        Accepted = 1,
        Denied = 2,
        Deleted = 3,
        Created = 4
    }
}
