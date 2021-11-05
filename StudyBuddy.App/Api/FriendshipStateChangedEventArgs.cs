using System;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public class FriendshipStateChangedEventArgs : EventArgs
    {
        public UserViewModel Friend { get; set; }
        public FriendshipStateChangedType Type { get; set; }
    }

    public enum FriendshipStateChangedType
    {
        Added = 1,
        Removed = 2
    }
}
