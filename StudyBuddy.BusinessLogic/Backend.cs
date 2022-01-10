using StudyBuddy.BusinessLogic.Interfaces;
using StudyBuddy.BusinessLogic.Services;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class Backend : IBackend
    {
        private JwtToken jwt = new JwtToken();

        public IRepository Repository { get; private set; }
        public User CurrentUser { get; set; }
        public IAuthenticationService AuthenticationService { get; private set; }
        public IChallengeService ChallengeService { get; private set; }
        public IFcmTokenService FcmTokenService { get; private set; }
        public IGameBadgeService GameBadgeService { get; private set; }
        public IPushNotificationService PushNotificationService { get; private set; }
        public IRequestService RequestService { get; set; }
        public ITagService TagService { get; set; }
        public IUserService UserService { get; set; }
        public IBusinessEventService BusinessEventService { get; private set; }
        public ILoggingService Logging { get; private set; }
        public INotificationService NotificationService { get; }
        public IStatisticsService StatisticsService { get; set; }
        public IImageService ImageService { get; set; }

        public Backend()
        {
            Repository = new Repository();
            AuthenticationService = new AuthenticationService(this);
            ChallengeService = new ChallengeService(this);
            FcmTokenService = new FcmTokenService(this);
            GameBadgeService = new GameBadgeService(this);
            PushNotificationService = new PushNotificationService(this);
            RequestService = new RequestService(this);
            TagService = new TagService(this);
            UserService = new UserService(this);
            BusinessEventService = new BusinessEventService(this);
            Logging = new LoggingService(this);
            NotificationService = new NotificationService(this);
            StatisticsService = new StatisticsService(this);
            ImageService = new ImageService(this);
        }

        public void SetCurrentUserFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return;

            int user_id = jwt.FromToken(token);
            CurrentUser = user_id != 0 ? Repository.Users.ById(user_id) : null;
        }
    }
}