using System;
using System.ComponentModel;
using StudyBuddy.App.Interfaces;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IApi : INotifyPropertyChanged
    {
        IAuthenticationService Authentication { get; }
        IChallengeService Challenges { get; }
        IFcmTokenService FcmTokens { get; }
        IUserService Users { get; }
        IBadgeService Badges { get; }
        IRequestService Requests { get; }
        ILoggingService Logging { get; }
        IStatisticsService Statistics { get; }
        IImageService ImageService { get; }
        INotificationService Notifications { get; }
        INotificationUserMetadataService NotificationUserMetadataService { get; }
        IDeviceService Device { get; }

        Version AppVersion { get; }
        Version ApiVersion { get; }

        event EventHandler<ChallengeViewModel> ChallengeAccepted;
        void RaiseChallengeAcceptedEvent(object sender, ChallengeViewModel challenge);

        event EventHandler<RequestStateChangedEventArgs> RequestStateChanged;
        void RaiseRequestStateChanged(object sender, RequestStateChangedEventArgs args);

        event EventHandler<FriendshipStateChangedEventArgs> FriendshipStateChanged;
        void RaiseFriendsChanged(object sender, FriendshipStateChangedEventArgs args);

        event AppIsTooOldEventHandler AppIsTooOld;
    }
}