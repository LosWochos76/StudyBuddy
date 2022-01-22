using System.Threading.Tasks;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Models;
using Xamarin.CommunityToolkit.ObjectModel;

namespace StudyBuddy.App.ViewModels
{
    public class FriendViewModel : ViewModelBase
    {
        public UserViewModel User { get; set; }
        public IAsyncCommand RemoveFriendCommand { get; set; }
        public UserStatistics UserStatistics { get; set; }

        public FriendViewModel(IApi api, IDialogService dialog, INavigationService navigation, UserViewModel obj, UserStatistics userStatistics) : base(api, dialog, navigation)
        {
            User = obj;
            UserStatistics = userStatistics;
            RemoveFriendCommand = new AsyncCommand(RemoveFriend);
        }

        public async Task RemoveFriend()
        {
            var answer = await dialog.ShowMessage(
                "Wollen Sie " + User.Firstname + " als Freund entfernen?",
                "Freund entfernen?",
                "Ja", "Nein", null);

            if (!answer)
                return;

            var result = await api.Users.RemoveFriend(User);

            if (!result)
            {
                dialog.ShowError("Fehler!", "Der Freund konnte nicht entfernt werden!", "Ok", null);
                return;
            }

            await Navigation.Pop();
        }
    }
}