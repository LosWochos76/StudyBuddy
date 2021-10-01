namespace StudyBuddy.ApiFacade
{
    public class ApiFacade : IApi
    {
        //private string base_url = "https://localhost:5001/";
        private string base_url = "https://studybuddy.hshl.de/";

        public IAuthenticationService Authentication { get; private set; }
        public IChallengeService Challenges { get; private set; }

        public ApiFacade()
        {
            this.Authentication = new AuthenticationService(this, base_url);
            this.Challenges = new ChallengeService(this, base_url);
        }
    }
}
