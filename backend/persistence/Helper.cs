using System;

namespace StudyBuddy.Persistence
{
    public class Helper
    {
        public static string GetFromEnvironmentOrDefault(string key, string def)
        {
            string var = Environment.GetEnvironmentVariable(key);
            if (var == null || var == string.Empty)
                return def;
            else
                return var;
        }
    }
}