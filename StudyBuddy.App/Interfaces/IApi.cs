﻿using System;
using StudyBuddy.App.Interfaces;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public interface IApi
    {
        IAuthenticationService Authentication { get; }
        IChallengeService Challenges { get; }
        IFcmTokenService FcmTokens { get; }
        IUserService Users { get; }
        ITagService Tags { get; }
        IBadgeService Badges { get; }
        IRequestService Requests { get; }
        ILoggingService Logging { get; }
        IStatisticsService Statistics { get; }
        IImageService ImageService { get; }

        NotificationService Notifications { get; }

        event EventHandler<ChallengeViewModel> ChallengeAccepted;
        void RaiseChallengeAcceptedEvent(object sender, ChallengeViewModel challenge);

        event EventHandler<RequestStateChangedEventArgs> RequestStateChanged;
        void RaiseRequestStateChanged(object sender, RequestStateChangedEventArgs args);

        event EventHandler<FriendshipStateChangedEventArgs> FriendshipStateChanged;
        void RaiseFriendsChanged(object sender, FriendshipStateChangedEventArgs args);
    }
}