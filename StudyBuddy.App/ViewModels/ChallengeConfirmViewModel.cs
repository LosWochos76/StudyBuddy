using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengeConfirmViewModel : ViewModelBase
    {
        private ChallengeViewModel challenge;
        public RangeObservableCollection<UserViewModel> Friends { get; set; }
        public UserViewModel SelectedFriend { get; set; }
        public ICommand ConfirmChallenge { get; }

        public ChallengeConfirmViewModel(IApi api, ChallengeViewModel challenge) : base(api)
        {
            this.challenge = challenge;
            ConfirmChallenge = new Command(OnConfirm);

            LoadFriends();
        }

        private async Task LoadFriends()
        {
            Friends = new RangeObservableCollection<UserViewModel>();
            var recipients = await api.Users.GetFriends();
            Friends.AddRange(recipients.Objects);
        }

        private void OnConfirm()
        {
            if (challenge.Prove == ChallengeProve.ByTrust)
                AcceptByTrust();

            if (challenge.Prove == ChallengeProve.ByQrCode)
                AcceptByQrCode();

            if (challenge.Prove == ChallengeProve.ByFriend)
                AcceptByFriend();

            if (challenge.Prove == ChallengeProve.ByLocation)
                AcceptByLocation();

            if (challenge.Prove == ChallengeProve.ByKeyword)
                AcceptByKeyword();
            
            if (challenge.Prove == ChallengeProve.BySystem)
                AcceptBySystem();
        }

        private async void AcceptBySystem()
        {
            // to be programmed
        }

        private async void AcceptByTrust()
        {
            var answer = await api.Device.ShowMessage(
                "Willst du die Herausforderung wirklich abschließen?",
                "Herausforderung abschließen?",
                "Ja", "Nein", null);

            if (!answer)
                return;

            var result = await api.Challenges.Accept(challenge);
            if (!result)
            {
                api.Device.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            await api.Device.GoToPath("//StatisticsPage");
        }

        private void AcceptByQrCode()
        {
            api.Device.PushPage(new QrCodePage());
        }

        private async void AcceptByFriend()
        {
            if (!Friends.Any())
            {
                api.Device.ShowMessage("Offenbar hast du noch keine Freunde! " +
                    "Bitte vernetze mit deinen Mit-Studierenden!", "Keine Freunde gefunden!");
                return;
            }

            if (SelectedFriend == null)
                return;

            var result = await api.Requests.AskForChallengeAcceptance(SelectedFriend.ID, challenge.ID);
            if (!result)
            {
                api.Device.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            api.Device.ShowMessageBox("Sobald dein Freund die Anfrage bestätigt, " +
                "bekommtst du die Punkte gutgeschrieben.", "Anfrage wurde verschickt!");
            SelectedFriend = null;

            await api.Device.GoToPath("//StatisticsPage");
        }

        private async void AcceptByLocation()
        {
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
                api.Device.ShowError("Ein Fehler ist aufgetreten! Deine Geo-Koordinaten konnten nicht bestimmt werden!", "Fehler!", "Ok", null);
                return;
            }

            var message = string.Format("Willst du die Herausforderung wirklich abschließen, " +
                "indem du deine aktuelle Position ({0:F4}x{1:F4}) meldest?", location.Latitude, location.Longitude);

            var answer = await api.Device.ShowMessage( message, "Herausforderung abschließen?", "Ja", "Nein", null);
            if (!answer)
                return;

            var geo = new GeoCoordinate()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };

            var result = await api.Challenges.AcceptWithLocation(challenge, geo);
            if (!result.Success)
            {
                var distance = result.UserPosition.Distance(result.TargetPosition);
                message = string.Format("Du darfst nicht weiter, als {0:F2}m von deinem Ziel entfernt sein! Aktuell sind es aber noch {1:F2}m!",
                    result.TargetPosition.Radius, distance);

                api.Device.ShowError(message, "Fehler!", "Ok", null);
                return;
            }

            await api.Device.GoToPath("//StatisticsPage");
        }

        private async void AcceptByKeyword()
        {
            var keyword = await api.Device.DisplayPrompt("Bitte Schlüsselwort eingeben!", "Schlüsselwort");

            if (string.IsNullOrEmpty(keyword))
                return;

            bool result = await api.Challenges.AcceptWithAddendum(challenge, keyword);
            if (!result)
            {
                api.Device.ShowError("Ein Fehler ist aufgetreten! Wahrscheinlich hast du nicht das richtige Schlüsselwort eingegeben!", "Fehler!", "Ok", null);
                return;
            }

            await api.Device.GoToPath("//StatisticsPage");
        }
    }
}