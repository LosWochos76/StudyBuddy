namespace StudyBuddy.ApiFacade
{
    public interface IApiFacade
    {
        IAuthentication Authentication { get; }
        IChallengeRepository Challenges { get; }
    }
}