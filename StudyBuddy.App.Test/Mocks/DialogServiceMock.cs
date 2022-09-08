using System;
using System.Threading.Tasks;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.Test.Mocks
{
    public class DialogServiceMock : IDialogService
    {
        public Task<string> DisplayPrompt(string message, string title)
        {
            return Task.Run(() => { return string.Empty; });
        }

        public void OpenBrowser(string url)
        {
        }

        public void ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
        }

        public void ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
        }

        public void ShowMessage(string message, string title)
        {
        }

        public void ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
        }

        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            return Task.Run(() => { return true; });
        }

        public void ShowMessageBox(string message, string title)
        {
        }
    }
}
