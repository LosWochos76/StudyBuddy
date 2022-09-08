using System;
using Xamarin.Essentials;

namespace StudyBuddy.App.Api
{
    public class PreferencesService : IPreferencesService
    {
        public string Get(string key, string default_value)
        {
            return Preferences.Get(key, default_value);
        }

        public void Set(string key, string value)
        {
            Preferences.Set(key, value);
        }
    }
}

