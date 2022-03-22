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
    public class ApiFacade : IApi, INotifyPropertyChanged
    {
        private readonly string base_url = "https://api.gameucation.eu/";
        //private readonly string base_url = "https://studybuddy.hshl.de/";
        //private readonly string base_url = "https://localhost:5001/";
        //private readonly string base_url = "https://10.0.2.2:5001/";
        //private readonly string base_url = "http://192.168.0.199:58947/";

        public Version AppVersion { get; private set; } = new Version(0, 0, 22, 0);
        public Version ApiVersion { get; private set; } = new Version(0, 0, 0, 0);

        public async Task LoadApiVersion()
        {
            var rh = new WebRequestHelper();
            ApiVersion = await rh.Get<Version>(base_url + "ApiVersion", HttpMethod.Get);
            NotifyPropertyChanged("ApiVersion");
        }

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

        public ApiFacade()
        {
            CheckVersion();

            Authentication = new AuthenticationService(this, base_url);
            Challenges = new ChallengeService(this, base_url);
            FcmTokens = new FcmTokenService(this, base_url);
            Users = new UserService(this, base_url);
            Badges = new BadgeService(this, base_url);
            Requests = new RequestService(this, base_url);
            Logging = new LoggingService(this, base_url);
            Notifications = new NotificationService(this, base_url);
            Statistics = new StatisticsService(this, base_url);
            ImageService = new ImageService(this, base_url);
        }

        private async void CheckVersion()
        {
            await LoadApiVersion();
            if (ApiVersion > AppVersion)
            {
                var dialog = TinyIoCContainer.Current.Resolve<IDialogService>();
                await Logging.LogError("App too old!");
                dialog.ShowError("App zu alt!", "Diese Version der App ist zu alt! Bitte updaten!", "Ok", null);
                throw new Exception("App is too old! Please update!");
            }
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
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaiseFriendsChanged(object sender, FriendshipStateChangedEventArgs args)
        {
            if (FriendshipStateChanged != null)
                FriendshipStateChanged(sender, args);
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}