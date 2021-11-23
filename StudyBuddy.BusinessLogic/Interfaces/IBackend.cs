using StudyBuddy.BusinessLogic.Interfaces;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public interface IBackend
    {
        User CurrentUser { get; set; }
        IRepository Repository { get; }
        IAuthenticationService AuthenticationService { get; }
        IChallengeService ChallengeService { get; }
        IFcmTokenService FcmTokenService { get; }
        IGameBadgeService GameBadgeService { get; }
        IPushNotificationService PushNotificationService { get; }
        IRequestService RequestService { get; set; }
        ITagService TagService { get; set; }
        IUserService UserService { get; set; }
        IStatisticsService StatisticsService { get; set; }
        IBusinessEventService BusinessEvent { get; }
        ILoggingService Logging { get; }
        NotificationService NotificationService { get; }
        public void SetCurrentUserFromToken(string token);
    }
}