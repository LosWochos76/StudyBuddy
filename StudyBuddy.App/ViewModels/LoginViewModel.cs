using System.Windows.Input;
using StudyBuddy.ApiFacade;
using StudyBuddy.ApiFacade.Restful;
using StudyBuddy.App.Misc;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public ICommand PasswordForgottenCommand { get; private set; }
        public ICommand ImprintCommand { get; private set; }
        public string EMail { get; set; }
        public string Password { get; set; }

        public LoginViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            LoginCommand = new Command(Login);
            RegisterCommand = new Command(Register);
            PasswordForgottenCommand = new Command(PasswordForgotten);
            ImprintCommand = new Command(Imprint);
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

        private async void Login()
        {
            var uc = new UserCredentials() { EMail = this.EMail, Password = this.Password };
            var result = await api.Authentication.Login(uc);

            if (result)
                await navigation.GoTo("//ChallengesPage");
            else
                await dialog.ShowMessageBox("Achtung!", "Anmdeldung nicht erfolgreich! Zugangsdaten korrekt?");
        }
    }
}