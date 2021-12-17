using System;
using System.Threading.Tasks;

namespace StudyBuddy.App.Misc
{
    public interface IDialogService
    {
        void ShowError(string message, string title, string buttonText, Action afterHideCallback);
        void ShowError(Exception error, string title, string buttonText, Action afterHideCallback);
        void ShowMessage(string message, string title);
        void ShowMessage(string message, string title, string buttonText, Action afterHideCallback);
        void ShowMessageBox(string message, string title);
        void OpenBrowser(string url);
        Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback);
        Task<string> DisplayPrompt(string message, string title);
    }
}