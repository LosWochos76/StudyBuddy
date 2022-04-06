using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.ViewModels
{
    public class QrCodeViewModel : ViewModelBase
    {
        public QrCodeViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
        }

        public async void AcceptFromQrCode(string text)
        {
            if (!text.StartsWith("qr:"))
                return;

            var answer = await dialog.ShowMessage(
                "StudyBuddy QR-Code gefunden! Wollen Sie die Herausforderung annehmen?",
                "Herausforderung annehmen?",
                "Ja", "Nein", null);

            if (!answer)
            {
                await Navigation.GoTo("//ChallengesPage");
                return;
            }

            var cvm = await api.Challenges.AcceptFromQrCode(text);
            if (cvm == null)
            {
                dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            await Navigation.GoTo("//StatisticsPage");
        }
    }
}