namespace StudyBuddy.Model
{
    public class Environment
    {
        public static string GetOrDefault(string key, string def)
        {
            string var = System.Environment.GetEnvironmentVariable(key);
            if (var == null || var == string.Empty)
                return def;
            else
                return var;
        }
    }
}