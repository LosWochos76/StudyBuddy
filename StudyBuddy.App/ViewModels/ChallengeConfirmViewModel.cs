using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using TinyIoC;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengeConfirmViewModel
    {
        private ChallengeViewModel challenge;
        private IDialogService dialog;
        private INavigationService navigation;
        private IApi api;
        public ICommand ConfirmChallenge { get; }

        public ChallengeConfirmViewModel(ChallengeViewModel challenge)
        {
            this.challenge = challenge;

            ConfirmChallenge = new Command(OnConfirm);
            dialog = TinyIoCContainer.Current.Resolve<IDialogService>();
            api = TinyIoCContainer.Current.Resolve<IApi>();
            navigation = TinyIoCContainer.Current.Resolve<INavigationService>();
        }

        private async void OnConfirm()
        {
            if (challenge.Prove == ChallengeProve.ByTrust)
                await AcceptByTrust();

            if (challenge.Prove == ChallengeProve.ByQrCode)
                await AcceptByQrCode();

            if (challenge.Prove == ChallengeProve.ByFriend)
                await AcceptByRandomTeamMember();

            if (challenge.Prove == ChallengeProve.ByLocation)
                await AcceptByLocation();

            if (challenge.Prove == ChallengeProve.ByKeyword)
                await AcceptByKeyword();
        }

        private async Task AcceptByTrust()
        {
            var answer = false;
            await dialog.ShowMessage(
                "Willst du die Herausforderung wirklich abschließen?",
                "Herausforderung abschließen?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var result = await api.Challenges.Accept(challenge);
            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            await navigation.GoTo("//StatisticsPage");
        }

        private async Task AcceptByQrCode()
        {
            await navigation.Push(new QrCodePage());
        }

        private async Task AcceptByRandomTeamMember()
        {
            var friends = new ObservableCollection<UserViewModel>();
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

            var answer = false;
            await dialog.ShowMessage(
                "Willst du eine Bestätigungsanfrage an " + user.FullName + " schicken?",
                "Anfrage verschicken?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var result = await api.Requests.AskForChallengeAcceptance(user.ID, challenge.ID);
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
            }
        }

        private async Task AcceptByLocation()
        {
            var answer = false;
            await dialog.ShowMessage(
                "Willst du die Herausforderung wirklich abschließen, indem deina aktuelle Geo-Position gecheckt wird?",
                "Herausforderung abschließen?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            Location location = null;
            try
            {
                location = await Geolocation.GetLocationAsync();
            }
            catch (Exception e)
            {
                await api.Logging.LogError(e.ToString());
            }

            if (location == null)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten! Deine Geo-Koordinaten konnten nicht bestimmt werden!", "Fehler!", "Ok", null);
                return;
            }

            var prove_addendum = location.Latitude + ";" + location.Longitude;
            bool result = await api.Challenges.AcceptWithAddendum(challenge, prove_addendum);

            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten! Wahrscheinlich hast du nicht die richtige Position!", "Fehler!", "Ok", null);
                return;
            }
            else
            {
                await navigation.Pop();
            }
        }

        private async Task AcceptByKeyword()
        {
            var keyword = await dialog.DisplayPrompt("Bitte Schlüsselwort eingeben!", "Schlüsselwort");

            if (string.IsNullOrEmpty(keyword))
                return;

            bool result = await api.Challenges.AcceptWithAddendum(challenge, keyword);

            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten! Wahrscheinlich hast du nicht das richtige Schlüsselwort eingegeben!", "Fehler!", "Ok", null);
                return;
            }
            else
            {
                await navigation.Pop();
            }
        }
    }
}