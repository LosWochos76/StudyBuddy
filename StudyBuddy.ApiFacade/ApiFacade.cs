using StudyBuddy.ApiFacade.Services;
using Xamarin.Essentials;

namespace StudyBuddy.ApiFacade
{
    public class ApiFacade : IApi
    {

        
#if DEBUG
        private readonly string base_url = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:5001/" : "https://localhost:5001/";
#else 
        private readonly string base_url = "https://studybuddy.hshl.de/";
#endif


        public ApiFacade()
        {
            Authentication = new AuthenticationService(this, base_url);
            Challenges = new ChallengeService(this, base_url);
            FcmTokens = new FcmTokenService(this, base_url);
        }

        public IAuthenticationService Authentication { get; }
        public IChallengeService Challenges { get; }
        public IFcmTokenService FcmTokens { get; }
    }
}