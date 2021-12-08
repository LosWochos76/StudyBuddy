using System;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        public UserViewModel User { get; set; }
        public ICommand SelectNewProfileImage { get; set; }

        public ProfileViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            SelectNewProfileImage = new Command(OnSelectNewProfileImage);
            this.User = UserViewModel.FromModel(api.Authentication.CurrentUser);
        }

        private void OnSelectNewProfileImage()
        {

        }
    }
}
