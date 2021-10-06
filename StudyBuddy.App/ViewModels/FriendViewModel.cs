using System;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class FriendViewModel
    {
        public UserViewModel User { get; set; }
        public ICommand RemoveFriendCommand { get; set; }

        public FriendViewModel(UserViewModel obj)
        {
            this.User = obj;
            RemoveFriendCommand = new Command(RemoveFriend);
        }

        public async void RemoveFriend()
        {
            var dialog = TinyIoCContainer.Current.Resolve<IDialogService>();
            var answer = false;
            await dialog.ShowMessage(
                "Wollen Sie " + User.Firstname + " als Freund entfernen?",
                "Freund entfernen?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var api = TinyIoCContainer.Current.Resolve<IApi>();
            var result = await api.Users.RemoveFriend(User.ID);

            if (!result)
            {
                await dialog.ShowError("Fehler!", "Der Freund konnte nicht entfernt werden!", "Ok", null);
                return;
            }

            var view_model = TinyIoCContainer.Current.Resolve<FriendsViewModel>();
            view_model.Reload();

            var navigation = TinyIoCContainer.Current.Resolve<INavigationService>();
            await navigation.GoTo("//FriendsPage");
        }
    }
}