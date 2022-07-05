using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand ProfileCommand { get; set; }
        public ICommand FriendsCommand { get; set; }
        public ICommand ThemeCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public Command AddFriendsCommand { get; }
        public Command OpenBrowserCommand { get; }

        public MainViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            ProfileCommand = new Command(Profile);
            FriendsCommand = new Command(Friends);
            LogoutCommand = new Command(Logout);
            ThemeCommand = new Command(Theme);
            OpenBrowserCommand = new Command<string>((x) => OpenBrowser(x));
            AddFriendsCommand = new Command(AddFriends);
        }

        private void Theme(object obj)
        {
            Navigation.Push(new ThemePage());
        }

        public void OpenBrowser(string str)
        {
            dialog.OpenBrowser(str);
        }

        private void AddFriends()
        {
            Navigation.Push(new AddFriendPage());
        }

        private void Friends(object obj)
        {
            Navigation.Push(new FriendsPage());
        }

        private void Profile(object obj)
        {
            Navigation.Push(new ProfilePage());
        }

        private void Logout(object obj)
        {
            api.Authentication.Logout();
            Navigation.GoTo("//LoginPage");
        }
    }
}