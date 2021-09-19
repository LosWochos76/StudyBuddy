using StudyBuddy.ApiFacade.Services;

namespace StudyBuddy.ApiFacade
{
    public interface IApi
    {
        IAuthenticationService Authentication { get; }
        IChallengeService Challenges { get; }
        IFcmTokenService FcmTokens { get; }
    }
}