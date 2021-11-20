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
            var theme = new ThemeViewModel();
            theme.RestoreTheme();
            TinyIoCContainer.Current.Register(theme);

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
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<BadgesViewModel>());
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
            OnResume();
        }

        protected override void OnSleep()
        {
            TinyIoCContainer.Current.Resolve<ThemeViewModel>().RestoreTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override async void OnResume()
        {
            TinyIoCContainer.Current.Resolve<ThemeViewModel>().RestoreTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TinyIoCContainer.Current.Resolve<ThemeViewModel>().RestoreTheme();
            });
        }
    }
}