﻿using System;
using StudyBuddy.App.Api;
using StudyBuddy.App.Interfaces;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.Test.Mocks
{
    public class ApiMock : IApi
    {
        public IAuthenticationService Authentication { get; }
        public IChallengeService Challenges { get; }
        public IFcmTokenService FcmTokens { get; }
        public IUserService Users { get; }
        public IBadgeService Badges { get; }
        public IRequestService Requests { get; }
        public ILoggingService Logging { get; }
        public INotificationService Notifications { get; }
        public IStatisticsService Statistics { get; }
        public IImageService ImageService { get; set; }

        public Version App_Version => throw new NotImplementedException();

        public Version Api_Version => throw new NotImplementedException();

        public ApiMock()
        {
            Authentication = new AuthenticationServiceMock();
            Challenges = new ChallengeServiceMock();
        }

        public event EventHandler<ChallengeViewModel> ChallengeAccepted;
        public void RaiseChallengeAcceptedEvent(object sender, ChallengeViewModel challenge)
        {
            if (ChallengeAccepted != null)
                ChallengeAccepted(sender, challenge);
        }

        public event EventHandler<RequestStateChangedEventArgs> RequestStateChanged;
        public void RaiseRequestStateChanged(object sender, RequestStateChangedEventArgs args)
        {
            if (RequestStateChanged != null)
                RequestStateChanged(sender, args);
        }

        public event EventHandler<FriendshipStateChangedEventArgs> FriendshipStateChanged;
        public void RaiseFriendsChanged(object sender, FriendshipStateChangedEventArgs args)
        {
            if (FriendshipStateChanged != null)
                FriendshipStateChanged(sender, args);
        }
    }
}
