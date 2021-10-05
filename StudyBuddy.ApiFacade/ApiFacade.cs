using StudyBuddy.ApiFacade.Services;

namespace StudyBuddy.ApiFacade
{
    public class ApiFacade : IApi
    {
        //public static string base_url = "https://localhost:5001/";
        private readonly string base_url = "https://studybuddy.hshl.de/";

        public ApiFacade()
        {
            Authentication = new AuthenticationService(this, base_url);
            Challenges = new ChallengeService(this, base_url);
            FcmTokens = new FcmTokenService(this, base_url);
            Users = new UserService(this, base_url);
        }

        public IAuthenticationService Authentication { get; }
        public IChallengeService Challenges { get; }
        public IFcmTokenService FcmTokens { get; }
        public IUserService Users { get; }
    }
}