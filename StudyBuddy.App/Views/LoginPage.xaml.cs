using System;
using System.Windows.Input;
using StudyBuddy.Model;
using StudyBuddy.ApiFacade;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App.Views
{
    public partial class LoginPage : ContentPage
    {
        private IAuthentication authentication;
        public ICommand TapCommand => new Command<string>(OpenBrowser);

        public LoginPage()
        {
            InitializeComponent();

            authentication = DependencyService.Get<IAuthentication>();
            BindingContext = this;
        }

        private void OpenBrowser(string url)
        {
            Launcher.OpenAsync(new Uri(url));
        }

        private async void Anmelden_Clicked(System.Object sender, System.EventArgs e)
        {
            var result = await authentication.Login(new UserCredentials()
            {
                EMail = email.Text,
                Password = password.Text
            });

            if (result)
                await Shell.Current.GoToAsync("//ChallengesPage");
            else
                await DisplayAlert("Achtung!", "Anmdeldung nicht erfolgreich! Zugangsdaten korrekt?", "Ok");
        }
    }
}
