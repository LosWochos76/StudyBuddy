using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        public UserViewModel User { get; set; }
        public ICommand EditProfileCommand { get; set; }
        public ProfileViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {

            User = api.Authentication.CurrentUser;
            EditProfileCommand = new Command(EditProfile);

        }

        public void EditProfile()
        {
            navigation.Push(new EditProfilePage());
        }
    }
}
