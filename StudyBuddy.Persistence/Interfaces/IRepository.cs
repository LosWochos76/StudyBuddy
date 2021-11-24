namespace StudyBuddy.Persistence
{
    public interface IRepository
    {
        IUserRepository Users { get; }
        IChallengeRepository Challenges { get; }
        IGameBadgeRepository GameBadges { get; }
        IRequestRepository Requests { get; }
        ITagRepository Tags { get; }
        IFcmTokenRepository FcmTokens { get; }
        ILoggingRepository Logging { get; }
        NotificationRepository Notifications { get; }
    }
}