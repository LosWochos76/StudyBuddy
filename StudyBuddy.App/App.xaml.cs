using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.App.Views;
using TinyIoC;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SetupServices();

            MainPage = new MainPage();
        }

        private void SetupServices()
        {
            TinyIoCContainer.Current.Register(new ThemeViewModel());
            TinyIoCContainer.Current.Register<IApi>(new ApiFacade());
            TinyIoCContainer.Current.Register<IDialogService>(new DialogService());
            TinyIoCContainer.Current.Register<INavigationService>(new NagigationService());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<LoginViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<MainViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<ChallengesViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<StatisticsViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<QrCodeViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<FriendsViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<NotificationsViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<FlyoutHeaderViewModel>());
        }

        private void SetupFirebasePushNotificationsHandler()
        {
        }

        protected override async void OnStart()
        {
            if (!Current.Properties.ContainsKey("Login"))
                return;

            var content = Current.Properties["Login"].ToString();
            var api = TinyIoCContainer.Current.Resolve<IApi>();
            var result = await api.Authentication.LoginFromJson(content);

            if (result)
                await Shell.Current.GoToAsync("//ChallengesPage");
            else
                Current.Properties.Remove("Login");

            OnResume();
        }

        protected override void OnSleep()
        {
            TinyIoCContainer.Current.Resolve<ThemeViewModel>().ApplyTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
            Connectivity.ConnectivityChanged -= App_ConnectivityChanged;
        }

        protected override async void OnResume()
        {
            TinyIoCContainer.Current.Resolve<ThemeViewModel>().ApplyTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
            Connectivity.ConnectivityChanged += App_ConnectivityChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TinyIoCContainer.Current.Resolve<ThemeViewModel>().ApplyTheme();
            });
        }

        private void App_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (e.NetworkAccess == NetworkAccess.None || e.NetworkAccess == NetworkAccess.Unknown ||
                    e.NetworkAccess == NetworkAccess.Local || e.NetworkAccess == NetworkAccess.ConstrainedInternet)
                {
                    await Current.MainPage.DisplayAlert("Achtung!", $"Es wurde keine Internetverbindung gefunden!\nVerbindungstyp: {e.NetworkAccess.ToString()}", "Ok");
                }
            });
        }
    }
}