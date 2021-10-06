﻿using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using TinyIoC;

namespace StudyBuddy.App.ViewModels
{
    public class QrCodeViewModel : ViewModelBase
    {
        public QrCodeViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
        }

        public async void AcceptFromQrCode(string text)
        {
            var answer = false;
            if (text.StartsWith("qr:"))
                await dialog.ShowMessage(
                    "StudyBuddy QR-Code gefunden! Wollen Sie die Herausforderung annehmen?",
                    "Herausforderung annehmen?",
                    "Ja", "Nein", a => { answer = a; });

            if (!answer)
            {
                await navigation.GoTo("//ChallengesPage");
                return;
            }

            var result = await api.Challenges.AcceptFromQrCode(text);
            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            var challenges_view_model = TinyIoCContainer.Current.Resolve<ChallengesViewModel>();
            challenges_view_model.Reload();
            await navigation.GoTo("//StatisticsPage");
        }
    }
}