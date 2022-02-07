using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            LoginCommand = new AsyncCommand(Login, () => { return IsEMailValid && IsPasswordValid; });
            RegisterCommand = new Command(Register);
            PasswordForgottenCommand = new AsyncCommand(PasswordForgotten, () => { return IsEMailValid; });
            ImprintCommand = new Command(Imprint);
            InfoCommand = new Command(Info);
            GetApiVersion();
        }
        private readonly string base_url = "https://api.gameucation.eu/";

        public IAsyncCommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public IAsyncCommand PasswordForgottenCommand { get; }
        public ICommand ImprintCommand { get; }
        public ICommand InfoCommand { get; }
        public string EMail { get; set; }
        public string Password { get; set; }

        private bool is_email_valid = false;
        public bool IsEMailValid
        {
            get { return is_email_valid; }
            set
            {
                is_email_valid = value;
                NotifyPropertyChanged();
                LoginCommand.RaiseCanExecuteChanged();
                PasswordForgottenCommand.RaiseCanExecuteChanged();
            }
        }

        private bool is_password_valid = false;
        public bool IsPasswordValid
        {
            get { return is_password_valid; }
            set
            {
                is_password_valid = value;
                NotifyPropertyChanged();
                LoginCommand.RaiseCanExecuteChanged();
                PasswordForgottenCommand.RaiseCanExecuteChanged();
            }
        }

        private void Info()
        {
            dialog.OpenBrowser("https://gameucation.eu/");
        }

        private void Register()
        {
            dialog.OpenBrowser("https://backend.gameucation.eu/register/");
        }

        private async Task PasswordForgotten()
        {
            var answer = await dialog.ShowMessage(
                "Wollen Sie eine E-Mail an '" + EMail + "' schicken, um das Passwort zurückzusetzen?",
                "Passwort zurücksetzen?",
                "Ja", "Nein", null);

            if (!answer)
                return;

            answer = await api.Authentication.SendPasswortResetMail(EMail);
            if (!answer)
                dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
            else
                dialog.ShowMessage("Eine E-Mail zum zurücksetzen des Passworts wurde verschickt.", "Passwort zurücksetzen");
        }

        private void Imprint()
        {
            dialog.OpenBrowser("https://gameucation.eu/impressum/");
        }

        private async Task Login()
        {
            var uc = new UserCredentials { EMail = EMail, Password = Password };
            var result = await api.Authentication.Login(uc);

            if (result)
                Navigation.GoTo("//ChallengesPage");
            else
                dialog.ShowMessageBox("Anmeldung nicht erfolgreich! Zugangsdaten korrekt?", "Achtung!");
        }

        public string AppVersion { get => api.App_Version.ToString(); }
        private Version _apiVersion = new Version(0,0,0,0);
        public string ApiVersion { get => _apiVersion.ToString(); }
        public async void GetApiVersion()
        {
            var rh = new WebRequestHelper();
            var result = await rh.Get<Version>(base_url + "ApiVersion", HttpMethod.Get);
            if (result == null)
                return;
            _apiVersion = result;
            NotifyPropertyChanged(nameof(_apiVersion));
        }
    }
}