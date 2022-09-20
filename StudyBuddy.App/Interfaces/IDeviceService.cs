using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StudyBuddy.App.Api
{
    public interface IDeviceService
    {
        Task GoToPath(string path);
        Task PushPage(Page page);
        Task PopPage();
        string GetPreference(string key, string default_value);
        void SetPreference(string key, string value);
        bool HasPreference(string key);
        void ShowError(string message, string title, string buttonText, Action afterHideCallback);
        void ShowError(Exception error, string title, string buttonText, Action afterHideCallback);
        void ShowMessage(string message, string title);
        void ShowMessage(string message, string title, string buttonText, Action afterHideCallback);
        void ShowMessageBox(string message, string title);
        void OpenBrowser(string url);
        Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback);
        Task<string> DisplayPrompt(string message, string title);
        Task Share(string title, string body);
    }
}