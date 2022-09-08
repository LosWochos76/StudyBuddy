using System;
using System.Collections.Generic;

namespace StudyBuddy.App.Test.Mocks
{
    public class PreferencesServiceMock : IPreferencesService
    {
        private Dictionary<string, string> preferences = new Dictionary<string, string>();

        public string Get(string key, string default_value)
        {
            if (preferences.ContainsKey(key))
                return preferences[key];
            else
                return default_value;
        }

        public void Set(string key, string value)
        {
            preferences[key] = value;
        }
    }
}