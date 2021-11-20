using System;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.ViewModels
{
    public class FlyoutHeaderViewModel : ViewModelBase
    {
        public UserViewModel User { get; set; }

        public FlyoutHeaderViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
            {
                User = UserViewModel.FromModel(args.User);
                NotifyPropertyChanged("User");
            }
        }
    }
}
