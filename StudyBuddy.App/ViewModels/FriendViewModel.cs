using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Models;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class FriendViewModel
    {
        public UserViewModel User { get; set; }
        public ICommand RemoveFriendCommand { get; set; }

        public UserStatistics UserStatistics { get; set; }

        public FriendViewModel(UserViewModel obj, UserStatistics userStatistics)
        {
            this.User = obj;
            this.UserStatistics = userStatistics;
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
            var result = await api.Users.RemoveFriend(User);

            if (!result)
            {
                await dialog.ShowError("Fehler!", "Der Freund konnte nicht entfernt werden!", "Ok", null);
                return;
            }

            var navigation = TinyIoCContainer.Current.Resolve<INavigationService>();
            await navigation.Pop();
        }
    }
}