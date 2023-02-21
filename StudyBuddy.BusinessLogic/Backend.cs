﻿using Org.BouncyCastle.Asn1.Cmp;
using SkiaSharp;
using StudyBuddy.BusinessLogic.Interfaces;
using StudyBuddy.BusinessLogic.Services;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class Backend : IBackend
    {
        private readonly JwtToken jwt = new JwtToken();

        public IRepository Repository { get; }
        public User CurrentUser { get; set; }
        public IAuthenticationService AuthenticationService { get; }
        public IChallengeService ChallengeService { get; }
        public IFcmTokenService FcmTokenService { get; }
        public IGameBadgeService GameBadgeService { get; }
        public IPushNotificationService PushNotificationService { get; }
        public IRequestService RequestService { get; set; }
        public ITagService TagService { get; set; }
        public IUserService UserService { get; set; }
        public IBusinessEventService BusinessEventService { get; }
        public ILoggingService Logging { get; }
        public INotificationService NotificationService { get; }
        public IStatisticsService StatisticsService { get; set; }
        public IImageService ImageService { get; set; }
        public INotificationUserMetadataService NotificationUserMetadataService { get; }

        public Backend() : this(new Repository())
        {
        }

        public Backend(IRepository repository)
        {
            Repository = repository;
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
            NotificationUserMetadataService = new NotificationUserMetadataService(this);

            ChallengeService.ChallengeCompleted += ChallengeService_ChallengeCompleted;
        }

        public void SetCurrentUserFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return;

            var user_id = jwt.FromToken(token);
            CurrentUser = user_id != 0 ? Repository.Users.ById(user_id) : null;
        }

        private void ChallengeService_ChallengeCompleted(object sender, ChallengeCompletedEventArgs e)
        {
            NotificationService.UserAcceptedChallenge(e.User, e.Challenge);
            GameBadgeService.CheckIfUserEarnedGameBadgeAfterChallengeCompleted(e.User, e.Challenge);
            BusinessEventService.TriggerEvent(this, new BusinessEventArgs(BusinessEventType.ChallengeAccepted, e.Challenge));
        }
    }
}