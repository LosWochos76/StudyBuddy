using StudyBuddy.ApiFacade;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.ViewModels
{
    public class QrCodeViewModel : ViewModelBase
    {
        public QrCodeViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
        }

        public void AcceptFromQrCode(string text)
        {
            if (text.StartsWith("qr:"))
                dialog.ShowMessage("StudyBuddy QR-Code gefunden! Wollen Sie die Herausforderung annehmen?",
                    "Herausforderung annehmen?",
                    "Ja", "Nein", (answer) =>
                {
                    if (answer)
                    {
                        api.Challenges.AcceptFromQrCode(text);
                        navigation.GoTo("//StatisticsPage");
                    }

                    navigation.GoTo("//ChallengesPage");
                });
        }
    }
}
