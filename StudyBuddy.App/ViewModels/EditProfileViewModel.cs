using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using System.Windows.Input;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class EditProfileViewModel : ViewModelBase
    {
        public User UpdatedUser { get; set; }
        public UserViewModel User { get; set; }
        private string _firstname = string.Empty;
        public string Firstname
        {
            get
            {
                return _firstname;
            }
            set
            {
                _firstname = value;
                NotifyPropertyChanged(_firstname);
            }
        }
        private string _lastname = string.Empty;
        public string Lastname
        {
            get
            {
                return _lastname;
            }
            set
            {
                _lastname = value;
                NotifyPropertyChanged(_lastname);
            }
        }
        private string _nickname = string.Empty;
        public string Nickname
        {
            get
            {
                return _nickname;
            }
            set
            {
                _nickname = value;
                NotifyPropertyChanged(_nickname);
            }
        }
        public ICommand ConfirmCommand { get; set; }

        public EditProfileViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            User = api.Authentication.CurrentUser;
            ConfirmCommand = new Command(Update);
        }

        private async void Update()
        {
            if (_firstname == string.Empty || _lastname == string.Empty)
                return;
            User UpdatedUser = UserViewModel.ToModel(User);
            UpdatedUser.Firstname = _firstname;
            UpdatedUser.Lastname = _lastname;
            UpdatedUser.Nickname = Nickname;
            await api.Users.Update(UpdatedUser);
        }
        
    }
}
