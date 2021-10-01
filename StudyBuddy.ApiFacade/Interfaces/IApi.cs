namespace StudyBuddy.ApiFacade
{
    public interface IApi
    {
        IAuthenticationService Authentication { get; }
        IChallengeService Challenges { get; }
    }
}