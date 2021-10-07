namespace StudyBuddy.App.Api
{
    public interface IApi
    {
        IAuthenticationService Authentication { get; }
        IChallengeService Challenges { get; }
        IFcmTokenService FcmTokens { get; }
        IUserService Users { get; }
        ITagService Tags { get; }
        IBadgeService Badges { get; }
        IRequestService Requests { get; }
    }
}