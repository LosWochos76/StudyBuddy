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
        public Command EditProfileCommand { get; set; }
        public bool IsEditing;
        public ProfileViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {

            User = api.Authentication.CurrentUser;
            EditProfileCommand = new Command(execute: async () =>
            {
                IsEditing = true;
                RefreshCanExecute();
                EditProfile();
            },
            canExecute: () =>
            {
                return !IsEditing;
            });

        }
        public void RefreshCanExecute()
        {
            EditProfileCommand.ChangeCanExecute();
        }
        public async void EditProfile()
        {
            await Shell.Current.Navigation.PushAsync(new EditProfilePage());
            IsEditing = false;
            RefreshCanExecute();
        }
    }
}
