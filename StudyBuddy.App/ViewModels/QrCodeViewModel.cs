using StudyBuddy.ApiFacade;
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
            bool answer = false;
            if (text.StartsWith("qr:"))
                await dialog.ShowMessage(
                    "StudyBuddy QR-Code gefunden! Wollen Sie die Herausforderung annehmen?",
                    "Herausforderung annehmen?",
                    "Ja", "Nein", (a) => { answer = a; });

            if (answer)
            {
                var result = await api.Challenges.AcceptFromQrCode(text);
                if (result)
                {
                    var challenges_view_model = TinyIoC.TinyIoCContainer.Current.Resolve<ChallengesViewModel>();
                    challenges_view_model.Reload();

                    await navigation.GoTo("//StatisticsPage");
                }
                else
                {
                    await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                }
                    
            }
            else
            {
                await navigation.GoTo("//ChallengesPage");
            }  
        }
    }
}