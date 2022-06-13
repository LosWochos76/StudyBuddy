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

            api.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("ApiVersion"))
                    NotifyPropertyChanged("ApiVersionAsString");
            };
        }

        public IAsyncCommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public IAsyncCommand PasswordForgottenCommand { get; }
        public ICommand ImprintCommand { get; }
        public ICommand InfoCommand { get; }
        public string EMail { get; set; }
        public string Password { get; set; }

        public string ApiVersionAsString { get => api.ApiVersion.ToString(); }
        public string AppVersionAsString { get => api.AppVersion.ToString(); }

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
            switch(result)
            {
                case 0:
                    await Navigation.GoTo("//ChallengesPage");
                    break;
                case 1:
                    var url = "http://10.0.2.2:4200/login/verificationrequired;email=";
                    var link = url + uc.EMail;
                    dialog.OpenBrowser(link);
                    break;
                case 2:
                    dialog.ShowMessageBox("E-Mail-Aresse oder Passwort ist falsch!", "Achtung!");
                    break;
                case 3:
                    dialog.ShowMessageBox("Es konnte kein Konto mit dieser E-Mail-Adresse gefunden werden.", "Achtung!");
                    break;
                case 4:
                    dialog.ShowMessageBox("Anmeldung nicht erfolgreich! Zugangsdaten korrekt?", "Achtung!");
                    await api.Logging.LogError("Invalid API response for login with " + uc.EMail);
                    break;
                case 5:
                    dialog.ShowMessageBox("Anmeldung nicht erfolgreich! Zugangsdaten korrekt?", "Achtung!");
                    await api.Logging.LogError("Issue in loginfromjson, no Token/User or Token invalid");
                    break;
                default:
                    dialog.ShowMessageBox("Anmeldung nicht erfolgreich! Zugangsdaten korrekt?", "Achtung!");
                    await api.Logging.LogError("Undocumented error");
                    break;
            }
        }
    }
}