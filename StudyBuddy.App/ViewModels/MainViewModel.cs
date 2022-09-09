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

        public MainViewModel(IApi api) : base(api)
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
            api.Device.PushPage(new ThemePage());
        }

        public void OpenBrowser(string str)
        {
            api.Device.OpenBrowser(str);
        }

        private void AddFriends()
        {
            api.Device.PushPage(new AddFriendPage());
        }

        private void Friends(object obj)
        {
            api.Device.PushPage(new FriendsPage());
        }

        private void Profile(object obj)
        {
            api.Device.PushPage(new ProfilePage());
        }

        private void Logout(object obj)
        {
            api.Authentication.Logout();
            api.Device.GoToPath("//LoginPage");
        }
    }
}