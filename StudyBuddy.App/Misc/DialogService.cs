using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.Misc
{
    public class DialogService : IDialogService
    {
        public async void ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, buttonText);

            if (afterHideCallback != null)
                afterHideCallback();
        }

        public async void ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                error.Message,
                buttonText);

            if (afterHideCallback != null) afterHideCallback();
        }

        public async void ShowMessage(string message, string title)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }

        public async void ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                buttonText);

            if (afterHideCallback != null)
                afterHideCallback();
        }

        public async Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            var result = await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                buttonConfirmText,
                buttonCancelText);

            if (afterHideCallback != null)
                afterHideCallback(result);

            return result;
        }

        public async void ShowMessageBox(string message, string title)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                "OK");
        }

        public void OpenBrowser(string url)
        {
            Launcher.OpenAsync(new Uri(url));
        }

        public async Task<string> DisplayPrompt(string message, string title)
        {
            return await Application.Current.MainPage.DisplayPromptAsync(title, message);
        }
    }
}