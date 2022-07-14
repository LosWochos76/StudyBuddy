using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class EditProfileViewModel : ViewModelBase
    {
        public Command ConfirmCommand { get; }
        public AsyncCommand CancelCommand { get; }

        public EditProfileViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            _firstname = api.Authentication.CurrentUser.Firstname;
            _lastname = api.Authentication.CurrentUser.Lastname;
            _nickname = api.Authentication.CurrentUser.Nickname;

            ConfirmCommand = new Command(Update, ConfirmAllowed);
            CancelCommand = new AsyncCommand(Decline);
        }

        private string _firstname = string.Empty;
        public string Firstname
        {
            get => _firstname;
            set
            {
                _firstname = value;
                NotifyPropertyChanged(nameof(Firstname));
            }
        }

        private string _lastname = string.Empty;
        public string Lastname
        {
            get => _lastname;
            set
            {
                _lastname = value;
                NotifyPropertyChanged(nameof(Lastname));
            }
        }

        private string _nickname = string.Empty;
        public string Nickname
        {
            get =>_nickname;
            set
            {
                _nickname = value;
                NotifyPropertyChanged(nameof(Nickname));
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyPropertyChanged(nameof(Password));
            }
        }

        private string _passwordConfirm = string.Empty;
        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                NotifyPropertyChanged(nameof(PasswordConfirm));
            }
        }

        private bool _firstnameNotValid;
        public bool FirstnameNotValid
        {
            get => _firstnameNotValid;
            set
            {
                _firstnameNotValid = value;
                NotifyPropertyChanged(nameof(FirstnameNotValid));
                ConfirmCommand.ChangeCanExecute();
            }
        }

        private bool _lastnameNotValid;
        public bool LastnameNotValid
        {
            get => _lastnameNotValid;
            set
            {
                _lastnameNotValid = value;
                NotifyPropertyChanged(nameof(LastnameNotValid));
                ConfirmCommand.ChangeCanExecute();
            }
        }

        private bool _nicknameNotValid;
        public bool NicknameNotValid
        {
            get => _nicknameNotValid;
            set
            {
                _nicknameNotValid = value;
                NotifyPropertyChanged(nameof(NicknameNotValid));
                ConfirmCommand.ChangeCanExecute();
            }
        }

        private bool _passwordNotValid;
        public bool PasswordNotValid
        {
            get => _passwordNotValid;
            set
            {
                _passwordNotValid = value;
                NotifyPropertyChanged(nameof(PasswordNotValid));
                ConfirmCommand.ChangeCanExecute();
            }
        }

        private bool _passwordConfirmNotValid;
        public bool PasswordConfirmNotValid
        {
            get => _passwordConfirmNotValid;
            set
            {
                _passwordConfirmNotValid = value;
                NotifyPropertyChanged(nameof(PasswordConfirmNotValid));
                ConfirmCommand.ChangeCanExecute();
            }
        }

        public string PasswordCriteria
        {
            get { return "Password muss enhalten:\n-Mindestens 1 Zahl\n-Mindestens 1 Kleinbuchstaben\n-Mindestens 1 Großbuchstaben\n-Mindestens 8 Zeichen"; }
        }

        public Regex PasswordPattern
        {
            get { return new Regex("(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,}"); }
        }

        public bool ConfirmAllowed() => !_firstnameNotValid && !_lastnameNotValid && !_nicknameNotValid && !_passwordNotValid;

        private async void Update()
        {
            try
            {
                var user = new User();
                user.ID = api.Authentication.CurrentUser.ID;
                user.Email = api.Authentication.CurrentUser.Email;
                user.EmailConfirmed = api.Authentication.CurrentUser.EmailConfirmed;
                user.AccountActive = api.Authentication.CurrentUser.AccountActive;

                user.Firstname = _firstname.Trim();
                user.Lastname = _lastname.Trim();
                user.Nickname = _nickname.Trim();

                if (!string.IsNullOrEmpty(_password) && _password == _passwordConfirm)
                    user.Password = _password.Trim();

                var result = await api.Users.Update(user);
                if (result)
                {
                    api.Authentication.CurrentUser.Firstname = user.Firstname;
                    api.Authentication.CurrentUser.Lastname = user.Lastname;
                    api.Authentication.CurrentUser.Nickname = user.Nickname;
                }
                else
                {
                    dialog.ShowError(
                        "Die Änderungen konnten nicht gespeichert werden!",
                        "Achtung!",
                        "Ok",
                        null);
                }

            }
            catch(ApiException e)
            {
                dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
            finally
            {
                await Navigation.Pop();
            }
        }

        public async Task Decline()
        {
            await Navigation.Pop();
        }
    }
}