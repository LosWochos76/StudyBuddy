using System;
using System.Windows.Input;
using StudyBuddy.Model;
using StudyBuddy.ServiceFacade;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App
{
    public partial class LoginPage : ContentPage
    {
        public ICommand TapCommand => new Command<string>(OpenBrowser);

        public LoginPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = this;
        }

        private void OpenBrowser(string url)
        {
            Launcher.OpenAsync(new Uri(url));
        }

        private async void Anmelden_Clicked(System.Object sender, System.EventArgs e)
        {
            var result = await Facade.Instance.Authentication.Login(
                new UserCredentials()
                {
                    EMail = email.Text,
                    Password = password.Text
                });

            if (!result)
            {
                await DisplayAlert("Achtung!", "Anmdeldung nicht erfolgreich! Zugangsdaten korrekt?", "Ok");
            }
        }
    }
}
