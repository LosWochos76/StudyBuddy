using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        public UserViewModel User { get; set; }

        public ProfileViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            this.User = api.Authentication.CurrentUser;
        }
    }
}
