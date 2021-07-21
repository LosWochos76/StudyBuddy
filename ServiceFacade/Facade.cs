using StudyBuddy.Model;

namespace StudyBuddy.ServiceFacade
{
    public class Facade
    {
        private string base_url;
        private static Facade instance = null;
        public Authentication Authentication { get; private set; }
        public Challenges Challenges { get; private set; }

        private Facade()
        {
            base_url = Helper.GetFromEnvironmentOrDefault("base_url", "https://localhost:5001/");
            Authentication = new Authentication(base_url);
            Challenges = new Challenges(base_url, Authentication);
        }

        public static Facade Instance
        {
            get
            {
                if (instance == null)
                    instance = new Facade();

                return instance;
            }
        }
    }
}