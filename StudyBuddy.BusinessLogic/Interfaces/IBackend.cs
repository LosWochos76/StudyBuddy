﻿using Microsoft.Extensions.Logging;
using StudyBuddy.BusinessLogic.Interfaces;
using StudyBuddy.BusinessLogic.Services;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public interface IBackend
    {
        User CurrentUser { get; set; }
        ILogger Logger { get; }
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
        IBusinessEventService BusinessEventService { get; }
        ILoggingService Logging { get; }
        INotificationService NotificationService { get; }
        IImageService ImageService { get; }
        NotificationUserMetadataService NotificationUserMetadataService { get; }
        CommentService CommentService { get; }

        void SetCurrentUserFromToken(string token);
    }
}