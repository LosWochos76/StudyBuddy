namespace StudyBuddy.ApiFacade
{
    public class ApiFacade : IApiFacade
    {
        private string base_url = "https://localhost:5001/";
        //private string base_url = "https://studybuddy.hshl.de/";

        public IAuthentication Authentication { get; private set; }
        public IChallengeRepository Challenges { get; private set; }

        public ApiFacade()
        {
            this.Authentication = new RestfulAuthentication(this, base_url);
            this.Challenges = new RestfulChallengeRepository(this, base_url);
        }
    }
}
