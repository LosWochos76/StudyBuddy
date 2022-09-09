using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using TinyIoC;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class NotificationViewModel : Notification, INotifyPropertyChanged
    {
        private readonly IApi api;
        public event PropertyChangedEventHandler PropertyChanged;
        public new UserViewModel Owner { get; set; }
        public new IEnumerable<UserViewModel> LikedUsers { get; set; } = new List<UserViewModel>();
        public new List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public bool HasBadge => BadgeId.HasValue;
        public new GameBadgeViewModel Badge { get; set; }
        public ICommand OpenLikesUsersModalCommand { get; set; }
        public ICommand OpenCommentsCommand { get; set; }
        public ICommand LikeCommand { get; set; }
        public string NumberOfCommentsText => $"{Comments.Count()} Kommentare";

        public NotificationViewModel()
        {
            api = TinyIoCContainer.Current.Resolve<IApi>();
            OpenLikesUsersModalCommand = new Command(OpenLikedUsers);
            OpenCommentsCommand = new Command(OpenComments);
            LikeCommand = new Command(Like);
        }

        public string UsersWhoLikedText
        {
            get
            {
                var users = LikedUsers;
                if (users.Count() == 0)
                    return "";

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

        public string LikeButtonText
        {
            get
            {
                if (Liked)
                    return "👍 Like";

                return "👍🏻 Like";
            }
        }

        private void OpenLikedUsers()
        {
            api.Device.PushPage(new NotificationsLikedUsersModalPage(this));
        }

        private void OpenComments()
        {
            api.Device.PushPage(new CommentModalPage(this));
        }

        public async void Like()
        {
            Liked = !Liked;
            await api.Notifications.Like(this, Liked);
            LikedUsers = await api.Users.Likers(Id);

            NotifyPropertyChanged("LikedUsers");
            NotifyPropertyChanged("UsersWhoLikedText");
            NotifyPropertyChanged("LikeButtonText");
        }

        public void Share()
        {
            api.Device.Share(Title, Body);
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}