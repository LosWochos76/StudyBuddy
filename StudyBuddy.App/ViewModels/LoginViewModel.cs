using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model.Enum;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public IAsyncCommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public IAsyncCommand PasswordForgottenCommand { get; }
        public IAsyncCommand SendVerificationMailCommand { get; }
        public ICommand ImprintCommand { get; }
        public ICommand InfoCommand { get; }
        public ICommand RecoverCommand { get; }
        public ICommand VerifyEmailCommand { get; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public string ApiVersionAsString { get => api.ApiVersion.ToString(); }
        public string AppVersionAsString { get => api.AppVersion.ToString(); }

        public LoginViewModel(IApi api) : base(api)
        {
            LoginCommand = new AsyncCommand(Login, () => { return IsEMailValid && IsPasswordValid; });
            RegisterCommand = new Command(Register);
            PasswordForgottenCommand = new AsyncCommand(PasswordForgotten, () => { return IsEMailValid; });
            SendVerificationMailCommand = new AsyncCommand(ResendEmail, () => { return IsEMailValid; });
            ImprintCommand = new Command(Imprint);
            InfoCommand = new Command(Info);
            VerifyEmailCommand = new Command(VerifyEmail);

            api.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("ApiVersion"))
                    NotifyPropertyChanged("ApiVersionAsString");
            };
        }

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
            api.Device.OpenBrowser("https://gameucation.eu/");
        }

        private async void Register()
        {
            await api.Device.PushPage(new RegisterPage());
        }
        public async void VerifyEmail()
        {
            await api.Device.PushPage(new VerifyEmailPage());
        }
        private async Task PasswordForgotten()
        {
            var answer = await api.Device.ShowMessage(
                "Wollen Sie eine E-Mail an '" + EMail + "' schicken, um das Passwort zurückzusetzen?",
                "Passwort zurücksetzen?",
                "Ja", "Nein", null);

            if (!answer)
                return;

            answer = await api.Authentication.SendPasswortResetMail(EMail);
            if (!answer)
                api.Device.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
            else
                api.Device.ShowMessage("Eine E-Mail zum zurücksetzen des Passworts wurde verschickt.", "Passwort zurücksetzen");
        }

        private async Task ResendEmail()
        {
            var answer = await api.Device.ShowMessage(
                "Wollen Sie eine E-Mail an '" + EMail + "' schicken, um das Konto zu bestätigen?",
                "E-Mail bestätigen?",
                "Ja", "Nein", null);

            if (!answer)
                return;

            answer = await api.Authentication.SendVerificationMail(EMail);
            if (!answer)
                api.Device.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
            else
                api.Device.ShowMessage("Eine E-Mail zu Bestätigung des Accounts wurde verschickt.", "E-Mail bestätigen");
        }

        private void Imprint()
        {
            api.Device.OpenBrowser("https://gameucation.eu/impressum/");
        }

        private async Task Login()
        {
            var uc = new UserCredentials { EMail = EMail, Password = Password };
            var result = await api.Authentication.Login(uc);

            switch(result)
            {
                case LoginStatus.Success:
                    await api.Device.GoToPath("//ChallengesPage");
                    break;
                case LoginStatus.EmailNotVerified:
                    var url = "https://backend.gameucation.eu/login/verificationrequired;email=";
                    var link = url + uc.EMail;
                    api.Device.OpenBrowser(link);
                    break;
                case LoginStatus.IncorrectCredentials:
                    api.Device.ShowMessageBox("E-Mail-Aresse oder Passwort ist falsch!", "Achtung!");
                    break;
                case LoginStatus.UserNotFound:
                    api.Device.ShowMessageBox("Es konnte kein Konto mit dieser E-Mail-Adresse gefunden werden.", "Achtung!");
                    break;
                case LoginStatus.InvalidApiResponse:
                    api.Device.ShowMessageBox("Anmeldung nicht erfolgreich! Zugangsdaten korrekt?", "Achtung!");
                    await api.Logging.LogError("Invalid API response for login with " + uc.EMail);
                    break;
                case LoginStatus.NoToken:
                    api.Device.ShowMessageBox("Anmeldung nicht erfolgreich! Zugangsdaten korrekt?", "Achtung!");
                    await api.Logging.LogError("Issue in loginfromjson, no Token/User or Token invalid");
                    break;
                default:
                    api.Device.ShowMessageBox("Anmeldung nicht erfolgreich! Zugangsdaten korrekt?", "Achtung!");
                    await api.Logging.LogError("Undocumented error");
                    break;
            }
        }
    }
}