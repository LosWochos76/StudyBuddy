using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class Repository : IRepository
    {
        private readonly string connection_string;

        public Repository()
        {
            connection_string = string.Format("Host={0};Username={1};Password={2};Database={3}",
                Environment.GetOrDefault("POSTGRESQL_HOST", "localhost"),
                Environment.GetOrDefault("POSTGRESQL_USER", "postgres"),
                Environment.GetOrDefault("POSTGRESQL_PASSWORD", "secret"),
                Environment.GetOrDefault("POSTGRESQL_DATABASE", "postgres"));

            CreateRepositories();
        }

        public IUserRepository Users { get; private set; }
        public IChallengeRepository Challenges { get; private set; }
        public IGameBadgeRepository GameBadges { get; private set; }
        public IRequestRepository Requests { get; private set; }
        public ITagRepository Tags { get; private set; }
        public ILoggingRepository Logging { get; private set; }
        public IFcmTokenRepository FcmTokens { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public IBusinessEventRepository BusinessEvents { get; private set; }
        public IStatisticsRepository StatisticsRepository { get; private set; }
        public IImageRepository Images { get; private set; }

        private void CreateRepositories()
        {
            Users = new UserRepository(connection_string);
            Challenges = new ChallengeRepository(connection_string);
            GameBadges = new GameBadgeRepository(connection_string);
            Requests = new RequestRepository(connection_string);
            Tags = new TagRepository(connection_string);
            FcmTokens = new FcmTokenRepository(connection_string);
            Logging = new LoggingRepository(connection_string);
            Notifications = new NotificationRepository(connection_string);
            BusinessEvents = new BusinessEventRepository(connection_string);
            StatisticsRepository = new StatisticsRepository(connection_string);
            Images = new ImageRepository(connection_string);
        }
    }
}