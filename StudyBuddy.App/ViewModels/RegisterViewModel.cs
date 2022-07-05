using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using Xamarin.CommunityToolkit.ObjectModel;

namespace StudyBuddy.App.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public RegisterViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
        }

        public ICommand RegisterCommand { get; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }

        private bool is_email_valid = false;
        public bool IsEMailValid
        {
            get { return is_email_valid; }
            set
            {
                is_email_valid = value;
                NotifyPropertyChanged();
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
            }
        }

        public bool IsFirstnameValid { get; set; } = true;
        public bool IsLastnameValid { get; set; } = true;
        public bool IsNicknameValid { get; set; } = true;
        public bool IsPasswordRepeatValid { get; set; } = true;
    }
}