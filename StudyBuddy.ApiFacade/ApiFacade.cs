namespace StudyBuddy.ApiFacade
{
    public class ApiFacade : IApi
    {
        //private string base_url = "https://localhost:5001/";
        private readonly string base_url = "https://studybuddy.hshl.de/";

        public ApiFacade()
        {
            Authentication = new AuthenticationService(this, base_url);
            Challenges = new ChallengeService(this, base_url);
        }

        public IAuthenticationService Authentication { get; }
        public IChallengeService Challenges { get; }
    }
}