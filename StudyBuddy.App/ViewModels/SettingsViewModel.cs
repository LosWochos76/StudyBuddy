using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public UserViewModel User { get; set; }
        public ICommand LogoutCommand { get; }
        public ICommand PickProfileImageCommand { get; }

        public SettingsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            LogoutCommand = new Command(Logout);
            PickProfileImageCommand = new Command(PickProfileImage);

            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
        }

        public ImageSource ProfileImage
        {
            get
            {
                return new FontImageSource()
                { 
                    FontFamily = Device.RuntimePlatform == Device.iOS ? "FontAwesome5Free-Solid" : "fasolid.otf#Regular",
                    Glyph = FontAwesomeIcons.User
                };
            }
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            User = UserViewModel.FromModel(api.Authentication.CurrentUser);
        }

        public void Logout()
        {
            api.Authentication.Logout();
            navigation.GoTo("//LoginPage");
        }

        public void PickProfileImage()
        {
            navigation.Push(new PickProfileImagePage());
        }
    }
}