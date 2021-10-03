using System.Windows.Input;
using StudyBuddy.ApiFacade;
using StudyBuddy.App.Misc;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog,
            navigation)
        {
            LogoutCommand = new Command(Logout);
        }

        public ICommand LogoutCommand { get; }

        public void Logout()
        {
            api.Authentication.Logout();
            navigation.GoTo("//LoginPage");
        }
    }
}