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
        INotificationRepository Notifications { get; }
        IStatisticsRepository StatisticsRepository { get; }
        IBusinessEventRepository BusinessEvents { get; }
        IImageRepository Images { get; }
        INotificationUserMetadataRepository NotificationUserMetadataRepository { get; }
        ICommentsRepository CommentsRepository { get; }
    }
}