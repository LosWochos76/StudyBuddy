using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.Api;
using Xamarin.Forms;

namespace StudyBuddy.App.Test.Mocks
{
    public class DeviceServiceMock : IDeviceService
    {
        private string last_path = string.Empty;
        private Page last_page = null;
        private Dictionary<string, string> preferences = new Dictionary<string, string>();

        public async Task GoToPath(string path)
        {
            last_path = path;
        }

        public async Task PopPage(Page page)
        {
            last_page = page;
        }

        public Task PushPage(Page page)
        {
            throw new NotImplementedException();
        }

        public Task PopPage()
        {
            throw new NotImplementedException();
        }

        public string GetPreference(string key, string default_value)
        {
            if (preferences.ContainsKey(key))
                return preferences[key];
            else
                return default_value;
        }

        public void SetPreference(string key, string value)
        {
            preferences[key] = value;
        }

        public bool HasPreference(string key)
        {
            return preferences.ContainsKey(key);
        }

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

        public Task Share(string title, string body)
        {
            return Task.Run(() => { Console.WriteLine("Sharing content..."); });
        }
    }
}