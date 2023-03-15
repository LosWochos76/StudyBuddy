using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Views;
using Xamarin.CommunityToolkit.ObjectModel;
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
        public AsyncCommand AboutCommand { get; }
        public AsyncCommand PrivacyNoticeCommand { get; }
        public AsyncCommand GetRequestCountCommand { get; }

        string _count = string.Empty;
        public string Count
        {
            get => _count;
            set
            {
                if (_count != value)
                {
                    _count = value;
                    NotifyPropertyChanged(nameof(Count));
                }
            }
        }
        public MainViewModel(IApi api) : base(api)
        {
            ProfileCommand = new Command(Profile);
            FriendsCommand = new Command(Friends);
            LogoutCommand = new Command(Logout);
            ThemeCommand = new Command(Theme);
            AboutCommand = new AsyncCommand(About);
            PrivacyNoticeCommand = new AsyncCommand(PrivacyNotice);
            AddFriendsCommand = new Command(AddFriends);
            GetRequestCountCommand = new AsyncCommand(GetRequestsCount);
        }

        private void Theme(object obj)
        {
            api.Device.PushPage(new ThemePage());
        }

        public Task About()
        {
            return api.Device.OpenBrowser(Settings.WebUrl);
        }

        public Task PrivacyNotice()
        {
            return api.Device.OpenBrowser(Settings.WebUrl + "datenschutzerklaerung");
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

        private async Task GetRequestsCount()
        {
            var requests = await api.Requests.ForMe();
            if (requests.Count == 0)
            {
                Count = string.Empty;
            }
            else
            {
                Count = requests.Count.ToString();
            }
        }
    }
}