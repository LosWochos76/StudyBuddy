using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.ViewModels
{
    public class QrCodeViewModel : ViewModelBase
    {
        public QrCodeViewModel(IApi api) : base(api)
        {
        }

        public async void AcceptFromQrCode(string text)
        {
            if (!text.StartsWith("qr:"))
                return;

            var answer = await api.Device.ShowMessage(
                "StudyBuddy QR-Code gefunden! Wollen Sie die Herausforderung annehmen?",
                "Herausforderung annehmen?",
                "Ja", "Nein", null);

            if (!answer)
            {
                await api.Device.GoToPath("//ChallengesPage");
                return;
            }

            var cvm = await api.Challenges.AcceptFromQrCode(text);
            if (cvm == null)
            {
                api.Device.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            await api.Device.GoToPath("//StatisticsPage");
        }
    }
}