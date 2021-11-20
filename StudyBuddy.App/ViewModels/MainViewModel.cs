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

        public MainViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            ProfileCommand = new Command(Profile);
            FriendsCommand = new Command(Friends);
            ThemeCommand = new Command(Theme);
            LogoutCommand = new Command(Logout);
        }

        private void Theme(object obj)
        {
            navigation.Push(new ThemePage());
        }

        private void Friends(object obj)
        {
            navigation.Push(new FriendsPage());
        }

        private void Profile(object obj)
        {
            navigation.Push(new ProfilePage());
        }

        private void Logout(object obj)
        {
            api.Authentication.Logout();
            navigation.GoTo("//LoginPage");
        }
    }
}