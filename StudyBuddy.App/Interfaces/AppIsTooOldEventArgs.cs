using System;

namespace StudyBuddy.App.Api
{
    public delegate void AppIsTooOldEventHandler(object source, AppIsTooOldEventArgs args);

    public class AppIsTooOldEventArgs : EventArgs
    {
        public Version AppVersion { get; set; }
        public Version ApiVersion { get; set; }
    }
}