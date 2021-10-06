using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class FriendsViewModel : ViewModelBase
    {
        public IEnumerable<UserViewModel> Friends { get; set; } = new List<UserViewModel>();
        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; }

        public FriendsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            this.RefreshCommand = new Command(Reload);
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                Reload();
        }

        public async void Reload()
        {
            Friends = await api.Users.GetFriends();
            NotifyPropertyChanged("Friends");

            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
        }
    }
}
