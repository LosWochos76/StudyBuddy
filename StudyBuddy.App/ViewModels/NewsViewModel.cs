using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using StudyBuddy.App.Annotations;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class NewsViewModel : INotifyPropertyChanged
    {
        public NewsViewModel(Notification notification)
        {
            Notification = notification;
            Comments.AddRange(notification.Comments.Select(item => new CommentViewModel(item)));
            LikedUsers.AddRange(notification.LikedUsers.Select(item => UserViewModel.FromModel(item)));
        }


        public RangeObservableCollection<CommentViewModel> Comments { get; set; } = new();
        public RangeObservableCollection<UserViewModel> LikedUsers { get; set; } = new();
        public Notification Notification { get; }


        public int Id => Notification.Id;
        public string Title => Notification.Title;
        public string Name => $"{Notification.Owner.Firstname} {Notification.Owner.Lastname}";
        public string NotificationCreated => Notification.Created.ToLongDateString();
        public string NumberOfCommentsText => $"{Notification.Comments.Count()} Comments";

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
                    return $"{user.Firstname} {user.Lastname} und {users.Count()} anderen gefällt das.";
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