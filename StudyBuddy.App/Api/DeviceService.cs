using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.Api
{
    public class DeviceService : IDeviceService
    {
        public string GetPreference(string key, string default_value)
        {
            return Preferences.Get(key, default_value);
        }

        public void SetPreference(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public bool HasPreference(string key)
        {
            return Preferences.ContainsKey(key);
        }

        public async Task GoToPath(string path)
        {
            Shell.Current.FlyoutIsPresented = false;

            if (Shell.Current.Navigation.NavigationStack.Count > 0)
            {
                var list = Shell.Current.Navigation.NavigationStack.ToList();
                foreach (var page in list)
                    if (page != null)
                        Shell.Current.Navigation.RemovePage(page);
            }

            await Shell.Current.GoToAsync(path);
        }

        public async Task PushPage(Page page)
        {
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.Navigation.PushAsync(page);
        }

        public async Task PopPage()
        {
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.Navigation.PopAsync();
        }

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

        public async Task Share(string title, string body)
        {
            await Xamarin.Essentials.Share.RequestAsync(new ShareTextRequest
            {
                Title = title,
                Text = body
            });
        }
    }
}