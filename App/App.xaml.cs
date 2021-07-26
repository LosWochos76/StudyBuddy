using System;
using StudyBuddy.ServiceFacade;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Facade.Instance.Authentication.AuthenticationStateChanged += Authentication_AuthenticationStateChanged;
            MainPage = new NavigationPage(new LoginPage());
        }

        private async void Authentication_AuthenticationStateChanged(object sender, AuthenticationEventArgs e)
        {
            if (e.WasLoggedIn)
                await MainPage.Navigation.PushAsync(new MainPage());
            else
                await MainPage.Navigation.PopToRootAsync();
        }

        protected override void OnStart()
        {
            Facade.Instance.Authentication.TryResume();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
