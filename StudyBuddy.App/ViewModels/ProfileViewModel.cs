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
        public Command DisableAccountCommand { get; set; }
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
            DisableAccountCommand = new Command(DisableAccount);


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

        public async void DisableAccount()
        {
            var answer = await dialog.ShowMessage(
                "Möchtest du dein Konto wirklich deaktivieren? Das Konto kann danach nicht mehr zur Anmeldung genutzt werden.",
                "Konto deaktivieren?",
                "Ja", "Nein", null);

            if (!answer)
                return;
            User.AccountActive = false;
            var result = await api.Users.Update(User);

            if (!result)
                return;

            dialog.ShowMessage(
                "Ihr Konto wurde deaktiviert und Sie wurden ausgeloggt.",
                "Achtung!");
            api.Authentication.Logout();
            await Navigation.GoTo("//LoginPage");
        }
    }
}
