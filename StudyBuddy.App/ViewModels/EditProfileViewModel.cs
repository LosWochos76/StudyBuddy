using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class EditProfileViewModel : ViewModelBase
    {
        public User UpdatedUser { get; set; }
        public UserViewModel User { get; set; }
        public Command ConfirmCommand { get; }

        

        public EditProfileViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            User = api.Authentication.CurrentUser;
            ConfirmCommand = new Command(Update, ConfirmAllowed);
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
        public bool ConfirmAllowed() =>
            !_firstnameNotValid && !_lastnameNotValid && !_nicknameNotValid;

        private async void Update()
        {
            
            try
            {
                User UpdatedUser = UserViewModel.ToModel(User);
                UpdatedUser.Firstname = _firstname.Trim();
                UpdatedUser.Lastname = _lastname.Trim();
                UpdatedUser.Nickname = Nickname.Trim();
                UpdatedUser.Password = Password.Trim();
                await api.Users.Update(UpdatedUser);
            }
            catch(ApiException e)
            {
                dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
            finally
            {
                await Shell.Current.Navigation.PopAsync();
            }
        }
        
    }
}
