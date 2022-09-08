using System;

namespace StudyBuddy.App
{
    public interface IPreferencesService
    {
        string Get(string key, string default_value);
        void Set(string key, string value);
    }
}