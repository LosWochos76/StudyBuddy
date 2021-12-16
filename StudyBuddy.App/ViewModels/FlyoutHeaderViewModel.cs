using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.ViewModels
{
    public class FlyoutHeaderViewModel : ViewModelBase
    {
        public UserViewModel User { get; set; }
        public int FriendsCount { get; set; }

        public FlyoutHeaderViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            api.FriendshipStateChanged += Api_FriendshipStateChanged;
        }

        private async void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
            {
                User = args.User;
                NotifyPropertyChanged("User");

                FriendsCount = await api.Users.GetFriendsCount();
                NotifyPropertyChanged("FriendsCount");
            }
        }

        private void Api_FriendshipStateChanged(object sender, FriendshipStateChangedEventArgs e)
        {
            if (e.Type == FriendshipStateChangedType.Added)
                FriendsCount++;
            else
                FriendsCount--;

            NotifyPropertyChanged("FriendsCount");
        }
    }
}
