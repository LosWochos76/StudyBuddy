using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Controls
{
    public partial class ByFriendView : ContentView
    {
        private ChallengeViewModel obj;

        public ByFriendView(ChallengeViewModel obj)
        {
            InitializeComponent();

            this.obj = obj;
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var dialog = TinyIoCContainer.Current.Resolve<IDialogService>();
            var api = TinyIoCContainer.Current.Resolve<IApi>();
            var navigation = TinyIoCContainer.Current.Resolve<INavigationService>();

            var friends = await api.Users.GetFriends();
            if (friends == null || !friends.Any())
            {
                await dialog.ShowMessage("Offenbar hast du noch keine Freunde! Bitte vernetze mit deinen Mit-Studierenden!", "Keine Freunde gefunden!");
                return;
            }

            var list = new List<UserViewModel>(friends);
            var random = new Random();
            int index = random.Next(list.Count);
            var user = list[index];

            var answer = false;
            await dialog.ShowMessage(
                "Willst du eine Bestätigungsanfrage an " + user.FullName + " schicken?",
                "Anfrage verschicken?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var result = await api.Requests.AskForChallengeAcceptance(user.ID, obj.ID);
            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
            }
            else
            {
                await dialog.ShowMessageBox("Sobald dein Freund die Anfrage bestätigt, " +
                    "bekommtst du die Punkte gutgeschrieben.", "Anfrage wurde verschickt!");
                await navigation.Pop();
            }
        }
    }
}
