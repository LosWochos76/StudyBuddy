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
        public UserViewModel User { get; set; }

        public EditProfileViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            User = api.Authentication.CurrentUser;
        }
        
    }
}
