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
            LoginCommand = new AsyncCommand(Login);
            RegisterCommand = new Command(Register);
            PasswordForgottenCommand = new Command(PasswordForgotten);
            ImprintCommand = new Command(Imprint);
            TapCommand = new Command(Tap);
        }

        public IAsyncCommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand PasswordForgottenCommand { get; }
        public ICommand ImprintCommand { get; }
        public ICommand TapCommand { get; }
        public string EMail { get; set; }
        public string Password { get; set; }

        private void Tap()
        {
            dialog.OpenBrowser("https://studybuddybackend.web.app/info");
        }

        private void Register()
        {
            dialog.OpenBrowser("https://studybuddybackend.web.app/register");
        }

        private void PasswordForgotten()
        {
            dialog.OpenBrowser("https://studybuddybackend.web.app/passwordforgotten");
        }

        private void Imprint()
        {
            dialog.OpenBrowser("https://studybuddybackend.web.app/imprint");
        }

        private async Task Login()
        {
            var uc = new UserCredentials {EMail = EMail, Password = Password};
            var result = await api.Authentication.Login(uc);

            if (result)
                navigation.GoTo("//ChallengesPage");
            else
                dialog.ShowMessageBox("Achtung!", "Anmdeldung nicht erfolgreich! Zugangsdaten korrekt?");
        }
    }
}