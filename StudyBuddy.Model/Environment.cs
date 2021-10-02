namespace StudyBuddy.Model
{
    public class Environment
    {
        public static string GetOrDefault(string key, string def)
        {
            var var = System.Environment.GetEnvironmentVariable(key);
            if (var == null || var == string.Empty)
                return def;
            return var;
        }
    }
}