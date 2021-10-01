using System.Windows.Input;
using StudyBuddy.ApiFacade;
using StudyBuddy.App.Misc;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public ICommand LogoutCommand { get; private set; }

        public SettingsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            this.LogoutCommand = new Command(Logout);
        }

        public void Logout()
        {
            api.Authentication.Logout();
            navigation.GoTo("//LoginPage");
        }
    }
}
