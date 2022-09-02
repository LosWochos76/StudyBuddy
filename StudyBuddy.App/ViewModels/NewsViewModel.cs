using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Annotations;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class NewsViewModel : INotifyPropertyChanged
    {
        private readonly IApi _api;
        private readonly INavigationService Navigation;

        public NewsViewModel(IApi api, Notification notification, INavigationService navigation)
        {
            Navigation = navigation;
            _api = api;
            Notification = notification;
            Comments.AddRange(notification.Comments.Select(item => new CommentViewModel(item)));
            LikedUsers.AddRange(notification.LikedUsers.Select(item => UserViewModel.FromModel(item)));
            Comments.CollectionChanged += (_, __) => { OnPropertyChanged("NumberOfCommentsText"); };
            ShareNotificationCommand = new Command(ShareNotification);
            LikeNotificationCommand = new Command<NewsViewModel>(LikeNotification);
            OpenLikesUsersModalCommand = new Command<NewsViewModel>(OpenLikedUsers);
            OpenCommentsCommands = new Command<NewsViewModel>(OpenComments);
            this.GetBadge();
        }

        public Command<NewsViewModel> OpenCommentsCommands { get; set; }
        public Command<NewsViewModel> OpenLikesUsersModalCommand { get; set; }
        public ICommand ShareNotificationCommand { get; set; }
        public ICommand LikeNotificationCommand { get; set; }
        public RangeObservableCollection<CommentViewModel> Comments { get; set; } = new();
        public RangeObservableCollection<UserViewModel> LikedUsers { get; set; } = new();
        public Notification Notification { get; }
        public GameBadgeViewModel GameBadgeViewModel { get; set; }
        private bool BadgeLoaded { get; set; } = false; 
        public bool ShowImage => this.Notification.BadgeId.HasValue && BadgeLoaded;

        public async Task GetBadge()
        {
            if (!this.Notification.BadgeId.HasValue)
            {
                return;
            }
        
            var response  = await this._api.Badges.GetById(this.Notification.BadgeId.Value);
            this.GameBadgeViewModel = response;
            this.BadgeLoaded = true;
            this.OnPropertyChanged("ShowImage");
            this.OnPropertyChanged("BadgeLoaded");
            this.OnPropertyChanged("GameBadgeViewModel");
        }
        
        public int Id => Notification.Id;
        public string Title => Notification.Title;
        public string Name => $"{Notification.Owner.Firstname} {Notification.Owner.Lastname}";
        public string NotificationCreated => Notification.Created.ToLongDateString();
        public string NumberOfCommentsText => $"{Comments.Count()} Comments";

        public bool Liked
        {
            get => Notification.Liked;
            set
            {
                Notification.Liked = value;
                OnPropertyChanged("ButtonColor");
                OnPropertyChanged("LikeButtonText");
            }
        }

        public string UsersWhoLikedText
        {
            get
            {
                var users = Notification.LikedUsers;
                if (users.Count() == 0) return "";

                if (users.Count() == 1)
                {
                    var user = users.First();
                    return $"{user.Firstname} {user.Lastname} gefällt das.";
                }
                else
                {
                    var user = users.First();
                    return $"{user.Firstname} {user.Lastname} und {users.Count() - 1} anderen gefällt das.";
                }
            }
        }

        public string FormattedBody => Notification.Body + " am " + Notification.Created.ToShortDateString();
        public UserViewModel NotificationOwner => UserViewModel.FromModel(Notification.Owner);

        public Color ButtonColor
        {
            get
            {
                if (Notification?.Liked is null) return Color.Red;

                if (Notification.Liked)
                    return Color.Chartreuse;
                return Color.Red;
            }
        }

        public string Body => Notification.Body;
        public bool ShowBody => !string.IsNullOrEmpty(Notification.Body);

        public string LikeButtonText
        {
            get
            {
                if (Notification.Liked)
                    return "👍 Like";
                return "👍🏻 Like";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void OpenComments(NewsViewModel viewModel)
        {
            Navigation.Push(new CommentModalPage(viewModel));
        }


        public async void LikeNotification(NewsViewModel news)
        {
            news.Liked = !Liked;
            await _api.NotificationUserMetadataService.LikeNotification(news);

            var currentUser = _api.Authentication.CurrentUser;
            var foundUser = Notification.LikedUsers.Find(user => user.ID == currentUser.ID);


            // TODO: Refactor this: change the place of the following logic
            // maye put this in the "Liked" setter or
            // "UsersWhoLikedText" setter

            // if user liked
            if (Liked)
            {
                // and is not in liked list
                if (foundUser is not null) return;

                Notification.LikedUsers.Add(currentUser);
            }
            // if user unliked
            else
            {
                // and is not in liked list 
                if (foundUser is null) return;

                Notification.LikedUsers.Remove(foundUser);
            }

            OnPropertyChanged("UsersWhoLikedText");
        }

        public void ShareNotification()
        {
            Share.RequestAsync(new ShareTextRequest
            {
                Title = Notification.Title,
                Text = Notification.Body
            });
        }

        private void OpenLikedUsers(NewsViewModel viewModel)
        {
            Navigation.Push(new NotificationsLikedUsersModalPage(viewModel));
        }


        public Notification ToNotification()
        {
            return Notification;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}