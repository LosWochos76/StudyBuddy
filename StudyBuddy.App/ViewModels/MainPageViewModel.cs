using System.Windows.Input;
using StudyBuddy.ApiFacade;
using StudyBuddy.App.Misc;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class MainPageViewModel
    {
        private IApiFacade api;
        private IDialogService dialog;
        private INagigationService navigation;

        public ICommand LogoutCommand { get; private set; }

        public MainPageViewModel()
        {
            api = App.ServiceProvider.GetService(typeof(IApiFacade)) as IApiFacade;
            dialog = App.ServiceProvider.GetService(typeof(IDialogService)) as IDialogService;
            navigation = App.ServiceProvider.GetService(typeof(INagigationService)) as INagigationService;

            LogoutCommand = new Command(Logout);
        }

        public void Logout()
        {
            api.Authentication.Logout();
            navigation.GoTo("//LoginPage");
        }
    }
}
