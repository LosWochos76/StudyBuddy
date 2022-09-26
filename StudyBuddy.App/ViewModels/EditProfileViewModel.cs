using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StudyBuddy.App.Api;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class EditProfileViewModel : ViewModelBase
    {
        public Command ConfirmCommand { get; }
        public AsyncCommand CancelCommand { get; }

        public EditProfileViewModel(IApi api) : base(api)
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

        private bool _nicknameInvalid;
        public bool NicknameInvalid
        {
            get => _nicknameInvalid;
            set
            {
                _nicknameInvalid = value;
                NotifyPropertyChanged(nameof(NicknameInvalid));
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

        private bool _passwordInvalid = false;
        public bool PasswordInvalid
        {
            get => _passwordInvalid;
            set 
            {
                _passwordInvalid = value;
                NotifyPropertyChanged(nameof(PasswordInvalid));
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

        public bool ConfirmAllowed() => !_firstnameNotValid && !_lastnameNotValid && !_nicknameNotValid && !_passwordConfirmNotValid;

        private async void Update()
        {
            try
            {
                var user = new UserViewModel();
                user.ID = api.Authentication.CurrentUser.ID;
                user.Email = api.Authentication.CurrentUser.Email;
                user.EmailConfirmed = api.Authentication.CurrentUser.EmailConfirmed;
                user.AccountActive = api.Authentication.CurrentUser.AccountActive;

                user.Firstname = _firstname.Trim();
                user.Lastname = _lastname.Trim();
                user.Nickname = _nickname.Trim();

                var user_id = await api.Users.IdByNickname(Nickname);
                if (user_id != null && user_id.ID != 0)
                {
                    NicknameInvalid = true;
                    return;
                }
                NicknameInvalid = false;
                if (PasswordNotValid)
                {
                    PasswordInvalid = true;
                    return;
                }
                PasswordNotValid = false;
                if (_password != _passwordConfirm)
                {
                    PasswordConfirmNotValid = true;
                    return;
                }
                PasswordConfirmNotValid = false;
                if (!string.IsNullOrEmpty(_password) && _password == _passwordConfirm)
                    user.Password = _password.Trim();
                
                
                var result = await api.Users.Update(user);
                if (result)
                {
                    api.Authentication.CurrentUser.Firstname = user.Firstname;
                    api.Authentication.CurrentUser.Lastname = user.Lastname;
                    api.Authentication.CurrentUser.Nickname = user.Nickname;
                    await api.Device.PopPage();
                }
                else
                {
                    api.Device.ShowError(
                        "Die Änderungen konnten nicht gespeichert werden!",
                        "Achtung!",
                        "Ok",
                        null);
                }
                
            }
            catch(ApiException e)
            {
                api.Device.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
        }

        public async Task Decline()
        {
            await api.Device.PopPage();
        }
    }
}