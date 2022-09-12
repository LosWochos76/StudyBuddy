using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using StudyBuddy.App.Interfaces;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using TinyIoC;

namespace StudyBuddy.App.Api
{
    public class ApiFacade : IApi
    {
        private readonly string base_url = "https://api.gameucation.eu/";
        //private readonly string base_url = "https://localhost:5001/";

        public ApiFacade()
        {
            Device = new DeviceService();
            Logging = new LoggingService(this, base_url);
            Authentication = new AuthenticationService(this, base_url);
            Challenges = new ChallengeService(this, base_url);
            FcmTokens = new FcmTokenService(this, base_url);
            Users = new UserService(this, base_url);
            Badges = new BadgeService(this, base_url);
            Requests = new RequestService(this, base_url);
            Notifications = new NotificationService(this, base_url);
            NotificationUserMetadataService = new NotificationUserMetadataService(this, base_url);
            Statistics = new StatisticsService(this, base_url);
            ImageService = new ImageService(this, base_url);

            LoadApiVersion();
        }

        public Version AppVersion { get; } = new(1, 10, 0, 0);
        public Version ApiVersion { get; private set; } = new(0, 0, 0, 0);
        public IStatisticsService Statistics { get; }
        public IAuthenticationService Authentication { get; }
        public IChallengeService Challenges { get; }
        public IFcmTokenService FcmTokens { get; }
        public IUserService Users { get; }
        public IBadgeService Badges { get; }
        public IRequestService Requests { get; }
        public ILoggingService Logging { get; }
        public INotificationService Notifications { get; }
        public INotificationUserMetadataService NotificationUserMetadataService { get; }
        public IImageService ImageService { get; set; }
        public IDeviceService Device { get; set; }

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

        public event EventHandler<ChallengeViewModel> ChallengeAccepted;
        public void RaiseChallengeAcceptedEvent(object sender, ChallengeViewModel challenge)
        {
            if (ChallengeAccepted != null)
                ChallengeAccepted(sender, challenge);
        }

        private async Task LoadApiVersion()
        {
            var rh = new WebRequestHelper();
            ApiVersion = await rh.Get<Version>(base_url + "ApiVersion", HttpMethod.Get);
            NotifyPropertyChanged("ApiVersion");

            if (ApiVersion > AppVersion && AppIsTooOld is not null)
                AppIsTooOld(this, new AppIsTooOldEventArgs() { AppVersion = AppVersion, ApiVersion = ApiVersion });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event AppIsTooOldEventHandler AppIsTooOld;
    }
}