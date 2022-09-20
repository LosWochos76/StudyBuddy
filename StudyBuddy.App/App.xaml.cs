using System;
using System.Net;
using System.Threading.Tasks;
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

            if (!CheckConnection())
                return;

            SetupServices();
            var api = TinyIoCContainer.Current.Resolve<IApi>();
            api.AppIsTooOld += Api_AppIsTooOld;

            if (api.AppVersion >= api.ApiVersion)
                MainPage = new MainPage();
        }

        private void Api_AppIsTooOld(object source, AppIsTooOldEventArgs args)
        {
            MainPage = new AppTooOldPage();
        }

        private bool CheckConnection()
        {
            var has_connection = App_HasConnection();
            var api_reachable = Api_Reachable();

            if (has_connection && api_reachable)
                return true;

            MainPage = new NoConnectionPage();
            return false;
        }

        private void SetupServices()
        {
            TinyIoCContainer.Current.Register<IApi>(new ApiFacade());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<ThemeViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<LoginViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<MainViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<ChallengesViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<StatisticsViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<QrCodeViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<FriendsViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<FlyoutHeaderViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<RegisterViewModel>());
        }

        protected override async void OnStart()
        {
            if (App_HasConnection() && Api_Reachable())
            {
                var api = TinyIoCContainer.Current.Resolve<IApi>();
                if (!api.Device.HasPreference("Login"))
                    return;

                var content = api.Device.GetPreference("Login", string.Empty);
                var result = await api.Authentication.LoginFromJson(content);

                if (result)
                    await Shell.Current.GoToAsync("//ChallengesPage");
                else
                    Preferences.Remove("Login");

                OnResume();
            }
        }

        protected override void OnSleep()
        {
            Connectivity.ConnectivityChanged -= App_ConnectivityChanged;
            TinyIoCContainer.Current.Resolve<ThemeViewModel>().ApplyTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            Connectivity.ConnectivityChanged += App_ConnectivityChanged;
            TinyIoCContainer.Current.Resolve<ThemeViewModel>().ApplyTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private bool Api_Reachable()
        {
            var url = "https://api.gameucation.eu/";
            var reachable = false;
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                    reachable = true;
                myHttpWebResponse.Close();
            }
            catch(WebException e)
            {
                //Current.MainPage.DisplayAlert("Achtung!",$"\r\nWebException Raised. The following error occurred : {0},{e.Status}", "Ok");
            }
            catch(Exception e)
            {
                //Current.MainPage.DisplayAlert("Achtung!",$"\nThe following Exception was raised : {0} , {e.Message}", "Ok");
            }
            return reachable;
        }

        private bool App_HasConnection()
        {
            var internet = Connectivity.NetworkAccess;
            if (internet == NetworkAccess.None || internet == NetworkAccess.Unknown ||
                internet == NetworkAccess.Local || internet == NetworkAccess.ConstrainedInternet)
                return false;
            return true;
        }

        private void App_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!App_HasConnection())
                    Current.MainPage.DisplayAlert("Achtung!",
                        $"Es wurde keine Internetverbindung gefunden!\nVerbindungstyp: {e.NetworkAccess.ToString()}",
                        "Ok");
            });
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TinyIoCContainer.Current.Resolve<ThemeViewModel>().ApplyTheme();
            });
        }
    }
}