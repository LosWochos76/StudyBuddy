using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public RegisterViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            RegisterCommand = new AsyncCommand(Register, () =>
            {
                return IsFirstnameValid &&
                    IsLastnameValid &&
                    IsNicknameValid &&
                    IsPasswordValid &&
                    IsPasswordRepeatValid;
            });

            DeclineCommand = new AsyncCommand(Decline);
        }

        public IAsyncCommand RegisterCommand { get; }
        public IAsyncCommand DeclineCommand { get; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }

        private bool is_firstname_valid = false;
        public bool IsFirstnameValid
        {
            get { return is_firstname_valid; }
            set
            {
                is_firstname_valid = value;
                NotifyPropertyChanged();
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        private bool is_lastname_valid = false;
        public bool IsLastnameValid
        {
            get { return is_lastname_valid; }
            set
            {
                is_lastname_valid = value;
                NotifyPropertyChanged();
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        private bool is_nickname_valid = false;
        public bool IsNicknameValid
        {
            get { return is_nickname_valid; }
            set
            {
                is_nickname_valid = value;
                NotifyPropertyChanged();
                RegisterCommand.RaiseCanExecuteChanged();

            }
        }

        private bool is_email_valid = false;
        public bool IsEMailValid
        {
            get { return is_email_valid; }
            set
            {
                is_email_valid = value;
                NotifyPropertyChanged();
                RegisterCommand.RaiseCanExecuteChanged();
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
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        private bool is_password_repeat_valid = false;
        public bool IsPasswordRepeatValid
        {
            get { return is_password_repeat_valid; }
            set
            {
                is_password_repeat_valid = value;
                NotifyPropertyChanged();
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        public async Task Register()
        {
            if (Password != PasswordRepeat)
            {
                dialog.ShowError("Passwort und Passwort-Wiederholung stimmen nicht überein!", "Fehler!", "Ok", null);
                return;
            }

            /*if (!EMail.ToLower().EndsWith("@hshl.de") && !EMail.ToLower().EndsWith("@stud.hshl.de"))
            {
                dialog.ShowError("Aktuell können nur E-Mail-Adressen der Hochschule Hamm-Lippstadt genutzt werden!", "Fehler!", "Ok", null);
                return;
            }*/

            var user_id = await api.Users.IdByEmail(EMail);
            if (user_id != null && user_id.ID != 0)
            {
                dialog.ShowError("Es existiert bereits ein Nutzer mit dieser E-Mail! " +
                    "Es kann kein weiterer Nutzer mit der selben E-Mail-Adresse registriert werden!", "Fehler!", "Ok", null);
                return;
            }

            user_id = await api.Users.IdByNickname(Nickname);
            if (user_id != null && user_id.ID != 0)
            {
                dialog.ShowError("Es existiert bereits ein Nutzer mit diesem Spitznamen! " +
                    "Es kann kein weiterer Nutzer mit dem selben Spitznamen egistriert werden!", "Fehler!", "Ok", null);
                return;
            }

            var user = new User();
            user.Firstname = Firstname;
            user.Lastname = Lastname;
            user.Nickname = Nickname;
            user.Email = EMail;
            user.Password = Password;
            var result = await api.Users.Register(user);

            if (result == null)
            {
                dialog.ShowError("Bei der Registrierung ist ein Fehler aufgetreten! Bitte versuchen Sie es später noch einmal!", "Fehler!", "Ok", null);
                return;
            }

            dialog.ShowMessage("Deine Registrierung war erfolgreich! Bitte klicke auf den Link in der Bestätigungsmail, die du erhalten hast." +
                "Danach kannst du dich in der App einloggen.!", "Herzlich willkommen bei Gameucation!");

            await Navigation.Pop();
        }

        public async Task Decline()
        {
            await Navigation.Pop();
        }
    }
}