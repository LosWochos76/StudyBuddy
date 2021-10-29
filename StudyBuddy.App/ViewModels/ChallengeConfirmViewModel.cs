using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengeConfirmViewModel : ChallengeViewModel
    {
        private readonly int proveType = 1;
        private readonly int iD;
        public ChallengeConfirmViewModel(int x, int y)
        {
            proveType = x;
            iD = y;
           
            ConfirmChallenge = new Command(OnConfirm);
        }

        public ICommand ConfirmChallenge { get; }
        private async void OnConfirm()
        {
            var dialog = TinyIoCContainer.Current.Resolve<IDialogService>();
            var api = TinyIoCContainer.Current.Resolve<IApi>();
            var navigation = TinyIoCContainer.Current.Resolve<INavigationService>();
            var friends = new ObservableCollection<UserViewModel>();
            var answer = false;

            if (proveType == 1)
            {
                //just ask if user really wants to confirm and update accordingly
                await dialog.ShowMessage(
                    "Willst du die Herausforderung wirklich abschließen?",
                    "Herausforderung abschließen?",
                    "Ja", "Nein", a => { answer = a; });
                if (!answer) 
                    return;
                var result = await api.Requests.Accept(iD);
                if (!result)
                {
                    await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                    return;
                }
            }
            if (proveType == 2)
            {
                await navigation.Push(new QrCodePage());
                return;
            }
            if (proveType == 3)
            {
                
                await api.Users.GetFriends(friends, string.Empty);

                if (!friends.Any())
                {
                    await dialog.ShowMessage("Offenbar hast du noch keine Freunde! Bitte vernetze mit deinen Mit-Studierenden!", "Keine Freunde gefunden!");
                    return;
                }

                var list = new List<UserViewModel>(friends);
                var random = new Random();
                int index = random.Next(list.Count);
                var user = list[index];

                
                await dialog.ShowMessage(
                    "Willst du eine Bestätigungsanfrage an " + user.FullName + " schicken?",
                    "Anfrage verschicken?",
                    "Ja", "Nein", a => { answer = a; });

                if (!answer)
                    return;

                var result = await api.Requests.AskForChallengeAcceptance(user.ID, iD);
                if (!result)
                {
                    await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                    return;
                }
                else
                {
                    await dialog.ShowMessageBox("Sobald dein Freund die Anfrage bestätigt, " +
                        "bekommtst du die Punkte gutgeschrieben.", "Anfrage wurde verschickt!");
                    await navigation.Pop();
                    return;
                } 
            }
            if (proveType == 4)
            {

                //check if user is in appropriate location or disable button if location is not matching
                await dialog.ShowMessage("Beweisverfahren noch nicht implementiert.", "Ungültiges Beweisverfahren!");
                return;
            }
            else
            {
                await dialog.ShowMessage("Diese Herausforderung kann nicht abgeschlossen werden.","Ungültiges Beweisverfahren!");
                return;
            } 
                
        }

    }
}
