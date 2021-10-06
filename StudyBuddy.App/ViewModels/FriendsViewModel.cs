using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class FriendsViewModel : ViewModelBase
    {
        public IEnumerable<UserViewModel> Friends { get; set; } = new List<UserViewModel>();
        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand AddFriendCommand { get; set; }

        public FriendsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            this.RefreshCommand = new Command(Reload);
            this.DetailsCommand = new Command<UserViewModel>(Details);
            this.AddFriendCommand = new Command(AddFriend);
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                Reload();
        }

        public async void Reload()
        {
            Friends = await api.Users.GetFriends(true);
            NotifyPropertyChanged("Friends");

            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
        }

        public void Details(UserViewModel obj)
        {
            navigation.Push(new FriendPage(obj));
        }

        public void AddFriend()
        {
            navigation.Push(new AddFriendPage());
        }
    }
}
