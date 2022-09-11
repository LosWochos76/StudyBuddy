using System;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class RepositoryMock : IRepository
    {
        public RepositoryMock()
        {
            Logging = new LoggingRepositoryMock();
            BusinessEvents = new BusinessEventRepositoryMock();

            var tag_repository = new TagRepositoryMock();
            Tags = tag_repository;

            var nmdr = new NotificationUserMetadataRepositoryMock();
            NotificationUserMetadataRepository = nmdr;

            var challenge_repository = new ChallengeRepositoryMock(tag_repository);
            Challenges = challenge_repository;

            var user_repository = new UserRepositoryMock(challenge_repository, nmdr);
            Users = user_repository;

            Notifications = new NotificationRepositoryMock(user_repository);
            GameBadges = new GameBadgeRepositoryMock(tag_repository, challenge_repository);
        }

        public IUserRepository Users { get; set; }
        public IChallengeRepository Challenges { get; set; }
        public IGameBadgeRepository GameBadges { get; set; }
        public IRequestRepository Requests { get; set; }
        public ITagRepository Tags { get; set; }
        public IFcmTokenRepository FcmTokens { get; set; }
        public ILoggingRepository Logging { get; set; }
        public INotificationRepository Notifications { get; set; }
        public IStatisticsRepository StatisticsRepository { get; set; }
        public IBusinessEventRepository BusinessEvents { get; set; }
        public IImageRepository Images { get; set; }
        public INotificationUserMetadataRepository NotificationUserMetadataRepository { get; set; }
    }
}